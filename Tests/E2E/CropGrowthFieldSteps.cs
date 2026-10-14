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
    // Standalone copy of the proven Grains StageASteps main-thread dispatcher.
    // Never execute Verse map/job mutations on Pickle's background test thread.
    internal static class RuntimeThread
    {
        public static Task Run(Action work)
        {
            var done = new TaskCompletionSource<bool>();
            Action invoke = delegate
            {
                try
                {
                    if (!UnityData.IsInMainThread)
                        throw new InvalidOperationException("Crop test work must run on Unity main thread");
                    work();
                    done.SetResult(true);
                }
                catch (Exception error) { done.SetException(error); }
            };
            RimWorks.Pickle.Runtime.PickleDriver.Post(invoke);
            return done.Task;
        }
    }

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
            private readonly float? oldTemperature;
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
                    {
                        // Filter randomly generated pawn-specific harvest-stat penalties
                        // BEFORE selecting an otherwise valid real worker. Do not
                        // silently change harvest stat defs or expected crop yields.
                        candidate.skills.GetSkill(SkillDefOf.Plants).Level = 20;
                        if (candidate.GetStatValue(StatDefOf.PlantHarvestYield) >= 1f)
                        { actor = candidate; break; }
                    }
                    candidate.Destroy(DestroyMode.Vanish);
                }
                ctx.Require(actor != null,
                    "No eligible actual worker with Plants20 and harvest yield >=1");
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


            // Actual job-offer check on unmodified Vanilla terrain Defs.
            // No 50%-fertility terrain is injected into Vanilla; 50% actual
            // sow/harvest remains untested, and loaded threshold is logged.
            private void ProbeNativeTerrainSowing()
            {
                string[] names = { "Gravel", "Soil", "SoilRich" };
                float[] expected = { .7f, 1f, 1.4f };
                ctx.Require(crop.plant.fertilityMin > .5f,
                    "Loaded crop 50% fertility eligibility changed; check actual sow");
                for (int i = 0; i < names.Length; i++)
                {
                    TerrainDef terrain = DefDatabase<TerrainDef>.GetNamedSilentFail(names[i]);
                    ctx.Require(terrain != null, "Required Vanilla terrain missing " + names[i]);
                    map.terrainGrid.SetTerrain(cell, terrain);
                    float actual = map.fertilityGrid.FertilityAt(cell);
                    ctx.Require(Math.Abs(actual - expected[i]) < .01f,
                        "Real terrain fertility is not expected " + names[i] + ": " + actual);
                    WorkGiver_GrowerSow giver = new WorkGiver_GrowerSow();
                    foreach (IntVec3 unused in giver.PotentialWorkCellsGlobal(worker)) { }
                    Job offered = giver.JobOnCell(worker, cell, true);
                    ctx.Require(offered != null && offered.def == JobDefOf.Sow &&
                        offered.plantDefToSow == crop,
                        "Native sow Job not offered on " + names[i]);
                    Log.Message("AMJ_CROP_SOIL_SOW probe=real_WorkGiver crop=" + crop.defName +
                        " profile=" + profile + " terrain=" + names[i] +
                        " fertility=" + actual.ToString("F2") +
                        " offered=true executed=false");
                }
                map.terrainGrid.SetTerrain(cell, TerrainDefOf.Soil);
                ctx.Require(Math.Abs(map.fertilityGrid.FertilityAt(cell) - 1f) < .01f,
                    "Original 100%-fertility test soil did not restore");
                Log.Message("AMJ_CROP_SOIL50 crop=" + crop.defName +
                    " loadedFertilityMin=" + crop.plant.fertilityMin.ToString("F2") +
                    " native50Job=NOT_RUN no_fabricated_terrain=true");
            }

            private int CountItem(string name)
            {
                int ground = map.listerThings.AllThings.Where(t => t.def.defName == name)
                    .Sum(t => t.stackCount);
                Thing carried = worker.carryTracker.CarriedThing;
                return ground + (carried != null && carried.def.defName == name ?
                    carried.stackCount : 0);
            }

            // Isolated mature MO tomato placed on real map then harvested with
            // actual pawn JobDriver. Placement bypasses MO trellis/research sow
            // conditions intentionally; it cannot establish routine farming.
            private async Task ProbeControlledTomatoHarvest()
            {
                Plant testPlant = null;
                Job job = null;
                string harvestedDef = "";
                int before = 0;
                await RuntimeThread.Run(() =>
                {
                    ThingDef t = DefDatabase<ThingDef>.GetNamedSilentFail("DankPyon_Plant_Tomatoes");
                    ThingDef pumpkin = DefDatabase<ThingDef>.GetNamedSilentFail("DankPyon_Plant_Pumpkins");
                    ctx.Require(t != null && t.plant != null &&
                        t.plant.harvestedThingDef != null &&
                        Math.Abs(t.plant.harvestAfterGrowth - .3f) < .0001f &&
                        Math.Abs(t.plant.growDays - 7f) < .0001f,
                        "Native MO tomato growth/harvest input differs from source");
                    ctx.Require(t.plant.sowTags != null &&
                        t.plant.sowTags.Contains("DankPyon_VinePost") &&
                        t.plant.sowResearchPrerequisites != null &&
                        t.plant.sowResearchPrerequisites.Any(
                            r => r.defName == "DankPyon_IntermediateAgriculture") &&
                        DefDatabase<ThingDef>.GetNamedSilentFail("DankPyon_Post") != null,
                        "Native MO tomato trellis/research constraints missing");
                    ctx.Require(pumpkin != null && pumpkin.plant != null &&
                        Math.Abs(pumpkin.plant.harvestYield - 20f) < .001f &&
                        pumpkin.plant.sowResearchPrerequisites != null &&
                        pumpkin.plant.sowResearchPrerequisites.Any(
                            r => r.defName == "DankPyon_AdvancedAgriculture"),
                        "Native MO pumpkin yield/research constraints differ");
                    harvestedDef = t.plant.harvestedThingDef.defName;
                    testPlant = ThingMaker.MakeThing(t) as Plant;
                    ctx.Require(testPlant != null, "Original MO tomato not spawnable");
                    testPlant.Growth = 1f;
                    GenSpawn.Spawn(testPlant, cell, map);
                    planted = testPlant; // disposal tracking, never production source
                    ctx.Require(testPlant.Spawned && testPlant.Growth >= .999f,
                        "Controlled mature MO tomato placement failed");
                    before = CountItem(harvestedDef);
                    map.designationManager.AddDesignation(
                        new Designation(testPlant, DesignationDefOf.HarvestPlant));
                    job = new WorkGiver_PlantsCut().JobOnThing(worker, testPlant, true);
                    ctx.Require(job != null && job.def == JobDefOf.HarvestDesignated,
                        "Native pawn harvest unavailable on controlled mature tomato");
                    OfferAndStart(job, "tomato-controlled-harvest");
                });
                await TickJob(job, () => CountItem(harvestedDef) > before,
                    "tomato-controlled-harvest");
                await RuntimeThread.Run(() =>
                {
                    int count = CountItem(harvestedDef) - before;
                    ctx.Require(count > 0, "Native tomato harvest produced no food");
                    Log.Message("AMJ_CROP_TOMATO_HARVEST profile=" + profile +
                        " output=" + harvestedDef + " count=" + count +
                        " destroyed=" + testPlant.Destroyed +
                        " spawned=" + testPlant.Spawned +
                        " growthAfter=" + testPlant.Growth.ToString("F4") +
                        " sourceHarvestAfterGrowth=0.30" +
                        " realHarvestJob=true realSowJob=false realTrellis=false" +
                        " repeatedHarvest=CONTROLLED_TEST_NEXT");
                });
                // Probe the same original mature tomato's second cycle.
                // Only Plant.TickLong is calendar-advanced; no natural season,
                // trellis sowing, hauling, meals or colony ticks are simulated.
                int reGrowthStart = 0;
                await RuntimeThread.Run(() =>
                {
                    ctx.Require(testPlant.Spawned && !testPlant.Destroyed &&
                        Math.Abs(testPlant.Growth - .3f) < .001f,
                        "First native tomato harvest did not retain original plant at 30%");
                    reGrowthStart = Find.TickManager.TicksGame;
                });
                float secondAt10 = 0f, secondAt20 = 0f;
                for (int k = 1; k <= 600; k += 30)
                {
                    int start = k;
                    await RuntimeThread.Run(() =>
                    {
                        ctx.Require(deadline.Elapsed.TotalSeconds < 230,
                            "Repeated native tomato TickLong sample timed out");
                        for (int j = start; j < Math.Min(start + 30, 601); j++)
                        {
                            Find.TickManager.DebugSetTicksGame(reGrowthStart + j * 2000);
                            map.skyManager.ForceSetCurSkyGlow(GenCelestial.CurCelestialSunGlow(map));
                            ctx.Require(testPlant.Spawned && !testPlant.Destroyed,
                                "Original tomato unexpectedly vanished before second maturity");
                            testPlant.TickLong();
                            if (j == 300) secondAt10 = testPlant.Growth;
                            if (j == 600) secondAt20 = testPlant.Growth;
                        }
                    });
                    await Task.Delay(1);
                }
                int beforeSecond = 0;
                Job secondJob = null;
                await RuntimeThread.Run(() =>
                {
                    ctx.Require(secondAt10 > .3f && secondAt10 <= secondAt20 &&
                        secondAt20 >= .999f,
                        "Retained tomato failed to regrow: growth10=" + secondAt10 +
                        " growth20=" + secondAt20);
                    beforeSecond = CountItem(harvestedDef);
                    map.designationManager.AddDesignation(
                        new Designation(testPlant, DesignationDefOf.HarvestPlant));
                    secondJob = new WorkGiver_PlantsCut().JobOnThing(worker, testPlant, true);
                    ctx.Require(secondJob != null && secondJob.def == JobDefOf.HarvestDesignated,
                        "Native WorkGiver did not offer same-plant second tomato harvest");
                    OfferAndStart(secondJob, "tomato-second-harvest");
                });
                await TickJob(secondJob, () => CountItem(harvestedDef) > beforeSecond,
                    "tomato-second-harvest");
                await RuntimeThread.Run(() =>
                {
                    int secondCount = CountItem(harvestedDef) - beforeSecond;
                    ctx.Require(secondCount > 0, "Same native tomato yielded no second crop");
                    Log.Message("AMJ_CROP_TOMATO_SECOND_HARVEST profile=" + profile +
                        " item=" + harvestedDef + " count=" + secondCount +
                        " growth10=" + secondAt10.ToString("F4") +
                        " growth20=" + secondAt20.ToString("F4") +
                        " postSecondGrowth=" + testPlant.Growth.ToString("F4") +
                        " originalPlantSurvived=" + (testPlant.Spawned && !testPlant.Destroyed) +
                        " realHarvestJob=true repeatedSamePlant=true" +
                        " sowOnNativeTrellis=NOT_RUN fullWorldSeason=NOT_RUN");
                });
            }

            public async Task Execute()
            {
                await RuntimeThread.Run(ProbeNativeTerrainSowing);
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
                        " workerHarvestStat=" +
                        worker.GetStatValue(StatDefOf.PlantHarvestYield).ToString("F4") +
                        " actualPawnJob=true simulatedGrowthOnly=true");
                });
                if (withMO) await ProbeControlledTomatoHarvest();
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
