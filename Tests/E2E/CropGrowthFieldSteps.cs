using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using RimWorks.Pickle;
using RimWorld;
using Verse;
using Verse.AI;

namespace AMJ.CropGrowthBalance.NativeTests
{
    // TEST-ONLY. The map is a disposable Quickstart: production crop XML
    // and the player's saves/settings are not modified.
    [PickleSteps]
    public sealed class CropGrowthFieldSteps
    {
        [Then("actual pawn sowing ten and twenty day plant growth and harvesting are recorded")]
        public async Task FieldWork(PickleContext ctx)
        {
            FieldFixture fixture = null;
            ExceptionDispatchInfo failure = null;
            try
            {
                await RuntimeThread.Run(() => fixture = new FieldFixture(ctx));
                await fixture.Execute();
            }
            catch (Exception e) { failure = ExceptionDispatchInfo.Capture(e); }
            try { if (fixture != null) await RuntimeThread.Run(fixture.Dispose); }
            catch (Exception cleanup)
            {
                if (failure != null) throw new AggregateException(failure.SourceException, cleanup);
                throw;
            }
            if (failure != null) failure.Throw();
        }

        private sealed class FieldFixture
        {
            private readonly PickleContext ctx;
            private readonly Map map;
            private readonly Stopwatch deadline = Stopwatch.StartNew();
            private readonly TimeSpeed previousSpeed;
            private readonly int oldTicks;
            private readonly float oldTemperature;
            private readonly float oldSky;
            private readonly IntVec3 cell;
            private readonly TerrainDef oldTerrain;
            private readonly float oldSnow;
            private readonly string profile;
            private readonly ThingDef crop;
            private readonly string outputName;
            private readonly bool withMO;
            private readonly Pawn worker;
            private Zone_Growing zone;
            private Plant planted;
            private readonly List<Thing> items = new List<Thing>();

            public FieldFixture(PickleContext context)
            {
                ctx = context;
                map = Find.CurrentMap;
                ctx.Require(map != null && map.Size.x == 50 && map.Size.z == 50,
                    "Crop field test requires disposable 50-cell Quickstart map");
                profile = Environment.GetEnvironmentVariable("AMJ_CROP_GROWTH_PROFILE") ?? "";
                ctx.Require(profile == "vanilla" || profile == "vanilla-ccto" ||
                    profile == "mo" || profile == "mo-ccto", "Invalid real field profile");
                withMO = profile == "mo" || profile == "mo-ccto";
                string defName = withMO ? "DankPyon_Plant_Lentils" : "Plant_Potato";
                crop = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
                ctx.Require(crop != null && crop.plant != null && crop.plant.harvestedThingDef != null,
                    "Required original cultivated crop or harvested ThingDef missing");
                ctx.Require(Math.Abs(crop.plant.growDays - (withMO ? 8f : 6f)) < 0.0001f,
                    "Loaded target crop growth differs from accepted balance");
                outputName = crop.plant.harvestedThingDef.defName;
                previousSpeed = Find.TickManager.CurTimeSpeed;
                oldTicks = Find.TickManager.TicksGame;
                oldTemperature = map.Biome.constantOutdoorTemperature;
                oldSky = map.skyManager.CurSkyGlow;
                cell = CellRect.CenteredOn(map.Center, 7).Cells.FirstOrDefault(c =>
                    c.InBounds(map) && !c.Roofed(map) && c.Standable(map) &&
                    !c.GetThingList(map).Any(t => t is Pawn || t is Building));
                ctx.Require(cell.InBounds(map) && !cell.Roofed(map) && cell.Standable(map),
                    "Open and pawn-safe field fixture unavailable");
                oldTerrain = map.terrainGrid.TerrainAt(cell);
                oldSnow = map.snowGrid.GetDepth(cell);
                Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
                foreach (Thing existing in cell.GetThingList(map).ToList())
                    existing.Destroy(DestroyMode.Vanish);
                map.terrainGrid.SetTerrain(cell, TerrainDefOf.Soil);
                map.snowGrid.SetDepth(cell, 0f);
                map.Biome.constantOutdoorTemperature = 25f;
                foreach (Room room in map.regionGrid.AllRooms)
                    if (room.UsesOutdoorTemperature) room.TempTracker.EqualizeTemperature();
                ctx.Require(Math.Abs(cell.GetTemperature(map) - 25f) < .1f,
                    "Field must hold controlled actual outdoor 25C");
                ctx.Require(Math.Abs(map.fertilityGrid.FertilityAt(cell) - 1f) < .001f,
                    "Field must hold 100% real soil fertility");
                int noon = (12 - GenLocalDate.HourOfDay(map) + 24) % 24;
                Find.TickManager.DebugSetTicksGame(oldTicks + noon * GenDate.TicksPerHour);
                map.skyManager.ForceSetCurSkyGlow(GenCelestial.CurCelestialSunGlow(map));
                zone = new Zone_Growing(map.zoneManager);
                map.zoneManager.RegisterZone(zone);
                zone.AddCell(cell);
                zone.SetPlantDefToGrow(crop);
                ctx.Require(PlantUtility.GrowthSeasonNow(cell, map, crop) &&
                    PlantUtility.CanSowOnGrower(crop, zone), "Native crop cannot sow at 25C on Soil");
                Pawn actor = null;
                for (int attempt = 0; attempt < 25; attempt++)
                {
                    Pawn candidate = PawnGenerator.GeneratePawn(PawnKindDefOf.Colonist, Faction.OfPlayer);
                    if (!candidate.Downed && candidate.skills != null &&
                        !candidate.WorkTypeIsDisabled(WorkTypeDefOf.Growing))
                    { actor = candidate; break; }
                    candidate.Destroy(DestroyMode.Vanish);
                }
                ctx.Require(actor != null, "No grower pawn could be generated");
                worker = actor;
                items.Add(worker);
                GenSpawn.Spawn(worker, map.Center, map);
                worker.workSettings.EnableAndInitializeIfNotAlreadyInitialized();
                worker.skills.GetSkill(SkillDefOf.Plants).Level = 20;
                ctx.Require(worker.GetStatValue(StatDefOf.PlantHarvestYield) >= 1f,
                    "Real harvest requires skilled worker without yield failure");
            }

            private int CountHarvest()
            {
                int ground = map.listerThings.AllThings.Where(t => t.def.defName == outputName)
                    .Sum(t => t.stackCount);
                Thing carried = worker.carryTracker.CarriedThing;
                return ground + (carried != null && carried.def.defName == outputName ?
                    carried.stackCount : 0);
            }

            private void OfferAndStart(Job job, string label)
            {
                ctx.Require(job != null, "Native WorkGiver supplied no job: " + label);
                job.playerForced = true;
                worker.jobs.StartJob(job, JobCondition.InterruptForced);
                ctx.Require(worker.CurJob == job, "Native job not started: " + label);
            }

            private async Task TickJob(Job job, Func<bool> done, string label)
            {
                bool finished = false;
                for (int i = 0; i < 16000 && !finished; i += 64)
                {
                    await RuntimeThread.Run(() =>
                    {
                        ctx.Require(deadline.Elapsed.TotalSeconds < 230,
                            "Native field test timed out at " + label);
                        for (int n = 0; n < 64; n++)
                        {
                            if (done()) { finished = true; break; }
                            ctx.Require(worker.CurJob == job,
                                "Pawn real JobDriver ended without expected result: " + label);
                            Find.TickManager.DoSingleTick();
                        }
                    });
                    await Task.Delay(1);
                }
                await RuntimeThread.Run(() =>
                {
                    ctx.Require(finished || done(),
                        "Native sow/harvest job exceeded 16000 ticks: " + label);
                    worker.jobs.EndCurrentJob(JobCondition.InterruptForced, false);
                });
            }

            public async Task Execute()
            {
                Job sow = null;
                await RuntimeThread.Run(() =>
                {
                    WorkGiver_GrowerSow giver = new WorkGiver_GrowerSow();
                    foreach (IntVec3 unused in giver.PotentialWorkCellsGlobal(worker)) { }
                    sow = giver.JobOnCell(worker, cell, true);
                    ctx.Require(sow != null && sow.def == JobDefOf.Sow &&
                        sow.plantDefToSow == crop, "Actual grower must offer correct sow Job");
                    OfferAndStart(sow, "sow");
                });
                await TickJob(sow, () => cell.GetPlant(map) != null &&
                    cell.GetPlant(map).def == crop &&
                    cell.GetPlant(map).LifeStage != PlantLifeStage.Sowing, "sow");

                await RuntimeThread.Run(() =>
                {
                    planted = cell.GetPlant(map);
                    ctx.Require(planted != null && planted.Spawned && planted.sown,
                        "Only a plant sown by a real pawn job is eligible for growth measurement");
                    ctx.Require(planted.Growth < 1f, "Sown crop unexpectedly fully mature");
                });
                // Controlled calendar samples. Exactly 2,000 game-time ticks are
                // advanced between Plant.TickLong calls. OTHER world systems are
                // not ticked: this is native plant-rate sampling, NOT a whole-game
                // 10/20-day colony simulation.
                float at10 = 0f, at20 = 0f;
                int initialCalendar = 0;
                await RuntimeThread.Run(() => initialCalendar = Find.TickManager.TicksGame);
                for (int k = 1; k <= 600; k += 30)
                {
                    int start = k;
                    await RuntimeThread.Run(() =>
                    {
                        ctx.Require(deadline.Elapsed.TotalSeconds < 230,
                            "Native TickLong crop sample exceeded time limit");
                        for (int j = start; j < Math.Min(start + 30, 601); j++)
                        {
                            Find.TickManager.DebugSetTicksGame(initialCalendar + j * 2000);
                            map.skyManager.ForceSetCurSkyGlow(GenCelestial.CurCelestialSunGlow(map));
                            if (!planted.Destroyed) planted.TickLong();
                            if (j == 300) at10 = planted.Growth;
                            if (j == 600) at20 = planted.Growth;
                        }
                    });
                    await Task.Delay(1);
                }
                await RuntimeThread.Run(() =>
                {
                    float low = PlantUtility.GrowthRateFactorFor_Fertility(crop, .5f);
                    float standard = PlantUtility.GrowthRateFactorFor_Fertility(crop, 1f);
                    float rich = PlantUtility.GrowthRateFactorFor_Fertility(crop, 1.4f);
                    ctx.Assert(low <= standard && standard <= rich,
                        "Native fertility growth factors must be nondecreasing");
                    ctx.Require(at10 > 0f && at10 <= at20 && at20 >= .999f,
                        "Native 10/20-day crop growth incorrect: day10=" + at10 +
                        ", day20=" + at20 + ", crop=" + crop.defName);
                    Log.Message("AMJ_CROP_FIELD_SAMPLE crop=" + crop.defName +
                        " profile=" + profile + " soil=1.0 temperature=25C" +
                        " growth10=" + at10.ToString("F4") +
                        " growth20=" + at20.ToString("F4") +
                        " fertilityFactor_0.5=" + low.ToString("F4") +
                        " fertilityFactor_1.0=" + standard.ToString("F4") +
                        " fertilityFactor_1.4=" + rich.ToString("F4") +
                        " method=Plant.TickLong/control-calendar-2000t");
                });
                int before = 0;
                Job harvest = null;
                await RuntimeThread.Run(() =>
                {
                    before = CountHarvest();
                    map.designationManager.AddDesignation(
                        new Designation(planted, DesignationDefOf.HarvestPlant));
                    harvest = new WorkGiver_PlantsCut().JobOnThing(worker, planted, true);
                    ctx.Require(harvest != null && harvest.def == JobDefOf.HarvestDesignated,
                        "Actual native harvest WorkGiver must offer a pawn job");
                    OfferAndStart(harvest, "harvest");
                });
                await TickJob(harvest, () => planted.Destroyed && CountHarvest() > before, "harvest");
                await RuntimeThread.Run(() =>
                {
                    int output = CountHarvest() - before;
                    ctx.Require(output > 0, "Real pawn harvested no usable crop items");
                    float expected = crop.plant.harvestYield;
                    ctx.Assert(output >= Math.Floor(expected) &&
                               output <= Math.Ceiling(expected * 2),
                        "Native harvest output far from Def yield; observed=" + output +
                        " expected=" + expected);
                    Log.Message("AMJ_CROP_FIELD_HARVEST crop=" + crop.defName +
                        " profile=" + profile + " item=" + outputName +
                        " harvested=" + output + " baseYield=" + expected +
                        " actualPawnJob=true simulatedGrowthOnly=true");
                });
            }

            public void Dispose()
            {
                if (worker != null && worker.Spawned)
                    worker.jobs.EndCurrentJob(JobCondition.InterruptForced, false);
                if (zone != null) zone.Delete(false);
                if (planted != null && !planted.Destroyed)
                    planted.Destroy(DestroyMode.Vanish);
                foreach (Thing thing in items)
                    if (thing != null && !thing.Destroyed) thing.Destroy(DestroyMode.Vanish);
                map.Biome.constantOutdoorTemperature = oldTemperature;
                foreach (Room room in map.regionGrid.AllRooms)
                    if (room.UsesOutdoorTemperature) room.TempTracker.EqualizeTemperature();
                map.terrainGrid.SetTerrain(cell, oldTerrain);
                map.snowGrid.SetDepth(cell, oldSnow);
                Find.TickManager.DebugSetTicksGame(oldTicks);
                map.skyManager.ForceSetCurSkyGlow(oldSky);
                Find.TickManager.CurTimeSpeed = previousSpeed;
            }
        }
    }
}
