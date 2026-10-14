using System;
using System.Linq;
using RimWorks.Pickle;
using Verse;

namespace AMJ.CropGrowthBalance.NativeTests
{
    // Source-owned loaded-Def coexistence test only, NOT season/meal E2E.
    [PickleSteps]
    public sealed class CropGrowthGrainsSteps
    {
        private const string CropPid = "sucro.ancientmedievaljapan.cropgrowthbalance";
        private const string GrainsPid = "sucro.ancientmedievaljapan.core";

        [Then("Grains and Crop Growth Balance retain separate crop growth ownership")]
        public void VerifySharedCropOwnership(PickleContext ctx)
        {
            string[] active = LoadedModManager.RunningModsListForReading
                .Select(x => x.PackageIdPlayerFacing.ToLowerInvariant()).ToArray();
            string profile = Environment.GetEnvironmentVariable("AMJ_CROP_GRAINS_PROFILE") ?? "";
            bool mo = profile == "mo-grains" || profile == "mo-ccto-grains";
            bool ccto = profile == "vanilla-ccto-grains" || profile == "mo-ccto-grains";
            ctx.Require(profile == "vanilla-grains" || profile == "vanilla-ccto-grains" ||
                        profile == "mo-grains" || profile == "mo-ccto-grains",
                        "Unsupported native combined source profile: " + profile);
            ctx.Require(active.Count(x => x == CropPid) == 1 &&
                        active.Count(x => x == GrainsPid) == 1,
                        "Genuine independently owned Crop Growth Balance and Grains both required");
            ctx.Require(active.Contains("dankpyon.medieval.overhaul") == mo,
                        "Original MO profile/provider is not as requested");
            ctx.Require(active.Contains("sucro.cropcoldtoleranceoverhaul") == ccto,
                        "Actual CCTO provider is not as requested");

            // CGB's independently owned six Vanilla crop timings.
            Check(ctx, "Plant_Potato", 6f);
            Check(ctx, "Plant_Corn", 13f);
            Check(ctx, "Plant_Strawberry", 6f);
            Check(ctx, "Plant_Cotton", 10f);
            Check(ctx, "Plant_Hops", 7f);
            Check(ctx, "Plant_Smokeleaf", 9f);
            if (mo)
            {
                Check(ctx, "DankPyon_Plant_Onions", 8f);
                Check(ctx, "DankPyon_Plant_Lentils", 8f);
                Check(ctx, "DankPyon_Plant_Garlic", 10f);
                Check(ctx, "DankPyon_Plant_Flax", 9f);
                Check(ctx, "DankPyon_Plant_Sugarcane", 12f);
                Check(ctx, "DankPyon_Plant_Carrots", 6f);
                Check(ctx, "DankPyon_Plant_Tomatoes", 7f);
            }
            // Grains' five bespoke millet/buckwheat/barley crops and two
            // shared crop integration points. Never patch these in CGB XML.
            Check(ctx, "AMJC_Plant_FoxtailMillet_Awa", 6f);
            Check(ctx, "AMJC_Plant_BarnyardMillet_Hie", 6f);
            Check(ctx, "AMJC_Plant_ProsoMillet_Kibi", 5f);
            Check(ctx, "AMJC_Plant_Buckwheat_Soba", 4f);
            Check(ctx, "AMJC_Plant_Barley", 10f);
            Check(ctx, "Plant_Rice", 5f);
            Check(ctx, mo ? "DankPyon_Plant_Wheat" : "AMJC_Plant_Wheat", 12f);
            Log.Message("AMJ_CROP_GRAINS_OWNERSHIP profile=" + profile +
                " CGBVanilla=6 CGBMO=" + (mo ? 7 : 0) +
                " GrainsSeven=7 Rice5=true Wheat12=true loadedDef=true" +
                " sourceNumbersOnly=true noMealOrFieldSimulation=true");
        }

        private static void Check(PickleContext ctx, string defName, float days)
        {
            ThingDef def = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
            ctx.Require(def != null && def.plant != null,
                "Missing original owner cultivated Def: " + defName);
            ctx.Assert(Math.Abs(def.plant.growDays - days) < .0001f,
                "Plant " + defName + " actual loaded growDays=" + def.plant.growDays +
                " expected=" + days);
        }
    }
}
