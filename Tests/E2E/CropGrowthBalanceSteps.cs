using System;
using System.Linq;
using RimWorks.Pickle;
using RimWorld;
using System.Globalization;
using Verse;

namespace AMJ.CropGrowthBalance.NativeTests
{
    [PickleSteps]
    public sealed class CropGrowthBalanceSteps
    {
        private const string CropPid = "sucro.ancientmedievaljapan.cropgrowthbalance";
        private const string GrainsPid = "sucro.ancientmedievaljapan.core";

        [Then("Crop Growth Balance loaded growth days match the four-profile specification")]
        public void VerifyCropDays(PickleContext ctx)
        {
            string[] active = LoadedModManager.RunningModsListForReading
                .Select(x => x.PackageIdPlayerFacing.ToLowerInvariant()).ToArray();
            string profile = Environment.GetEnvironmentVariable("AMJ_CROP_GROWTH_PROFILE") ?? "";
            bool medieval = profile == "mo" || profile == "mo-ccto";
            bool ccto = profile == "vanilla-ccto" || profile == "mo-ccto";
            ctx.Assert(profile == "vanilla" || profile == "vanilla-ccto" ||
                       profile == "mo" || profile == "mo-ccto",
                       "Unrecognized native profile: " + profile);
            ctx.Assert(active.Count(x => x == CropPid) == 1,
                       "Expected exactly one standalone Crop Growth Balance package");
            ctx.Assert(!active.Contains(GrainsPid),
                       "Standalone native profile must not be supplemented by Grains");
            ctx.Assert(active.Contains("dankpyon.medieval.overhaul") == medieval,
                       "Actual MO provider state disagrees with profile");
            ctx.Assert(active.Contains("sucro.cropcoldtoleranceoverhaul") == ccto,
                       "Actual CCTO provider state disagrees with profile");

            Check(ctx, "Plant_Potato", 6f);
            Check(ctx, "Plant_Corn", 13f);
            Check(ctx, "Plant_Strawberry", 6f);
            Check(ctx, "Plant_Cotton", 10f);
            Check(ctx, "Plant_Hops", 7f);
            Check(ctx, "Plant_Smokeleaf", 9f);
            if (medieval)
            {
                Check(ctx, "DankPyon_Plant_Onions", 8f);
                Check(ctx, "DankPyon_Plant_Lentils", 8f);
                Check(ctx, "DankPyon_Plant_Garlic", 10f);
                Check(ctx, "DankPyon_Plant_Flax", 9f);
                Check(ctx, "DankPyon_Plant_Sugarcane", 12f);
                Check(ctx, "DankPyon_Plant_Carrots", 6f);
                Check(ctx, "DankPyon_Plant_Tomatoes", 7f);
                // Original MO controls, not modified by Crop Growth Balance.
                Check(ctx, "DankPyon_Plant_Cabbages", 8f);
                Check(ctx, "DankPyon_Plant_Pumpkins", 8f);
            }
        }

        private static void Check(PickleContext ctx, string name, float expected)
        {
            ThingDef thing = DefDatabase<ThingDef>.GetNamedSilentFail(name);
            ctx.Assert(thing != null && thing.plant != null,
                       "Expected cultivated PlantDef is absent: " + name);
            if (thing != null && thing.plant != null)
            {
                ctx.Assert(Math.Abs(thing.plant.growDays - expected) < 0.0001f,
                    name + " loaded growDays=" + thing.plant.growDays +
                    "; expected " + expected);

                // Capture actual loaded output inputs, not processed or annual yields.
                ThingDef harvested = thing.plant.harvestedThingDef;
                ctx.Assert(harvested != null, name + " harvestedThingDef missing");
                ctx.Assert(thing.plant.harvestYield > 0f,
                    name + " base harvestYield must be positive");
                if (harvested != null)
                {
                    float rawNutrition = harvested.GetStatValueAbstract(StatDefOf.Nutrition);
                    ctx.Assert(!float.IsNaN(rawNutrition) && rawNutrition >= 0f,
                        name + " harvested item has invalid Nutrition");
                    Log.Message("AMJ_CROP_LOADED_RESOURCE crop=" + name +
                        " growDays=" + thing.plant.growDays.ToString("F4", CultureInfo.InvariantCulture) +
                        " harvestYield=" + thing.plant.harvestYield.ToString("F4", CultureInfo.InvariantCulture) +
                        " harvestedThing=" + harvested.defName +
                        " rawNutrition=" + rawNutrition.ToString("F4", CultureInfo.InvariantCulture) +
                        " fertilityMin=" + thing.plant.fertilityMin.ToString("F4", CultureInfo.InvariantCulture) +
                        " sowMinSkill=" + thing.plant.sowMinSkill);
                }
            }
        }
    }
}
