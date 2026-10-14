# Vanilla/MO crop gross-yield baseline — 2026-10-14

**Evidence:** real user-provided Medieval Overhaul archive `3219596926.zip`, **only** `3219596926/1.6/Defs/ThingDefs_Plants/Plants_Cultivated_Farm.xml` for crop values and `3219596926/1.6/Defs/ThingDefs_Items/Items_Resource_RawPlant.xml` for item Nutrition. Do not accidentally use the 1.4/1.5 variants of the same relative XML path. No executable game or current 1.7 MO source was inspected here. The production owner `Patches/CropGrowthBalance.xml` and `Compatibility/MedievalOverhaul/Patches/CropGrowthBalance.xml` remain unchanged.

## Gross source-based index

The following is **not calendar days, actual annual yield or tested native gameplay**. It is `harvestYield / effective growDays` at exactly fertility 100%, optimal growth temperature, common outdoor lighting opportunity, no sowing/harvest/movement/time loss and one planting/harvest cycle. `growDays` still needs game light, heat and fertility multipliers. "Nutrition index" is only shown when the **1.6 MO raw ThingDef explicitly defines `statBases/Nutrition=0.05`**; it is `harvestYield × 0.05 / growDays`, without digestion, meals, real Jobs or soil effects.

| Crop | Original growDays | New growDays | Original harvestYield | Units / growDay after patch | Explicit raw Nutrition | Gross Nutrition index |
|---|---:|---:|---:|---:|---:|---:|
| MO onions | 4 | 8 | 13 | 1.625 | 0.05 | 0.0813 |
| MO lentils | 3 | 8 | 10 | 1.250 | 0.05 | 0.0625 |
| MO garlic | 4 | 10 | 6 | 0.600 | 0.05 | 0.0300 |
| MO carrots | 3.25 | 6 | 6 | 1.000 | 0.05 | 0.0500 |
| MO tomatoes | 4.25 | 7 | 8 | 1.143 | 0.05 | 0.0571 |
| MO flax | 7 | 9 | 18 | 2.000 | not explicitly defined | — |
| MO sugarcane | 5.8 | 12 | 11 | 0.917 | not explicitly defined | — |
| MO cabbage — unchanged comparison | 8 | 8 | 10 | 1.250 | 0.05 | 0.0625 |
| MO pumpkin — unchanged comparison | 8 | 8 | 20 | 2.500 | 0.05 | 0.1250 |

**Comparison, not same economic cost:** Grains' approved seven-crop raw final grain-yield proxy at fertility 100% is Kibi 11/5=2.20, Awa 13/6≈2.17, Hie 12/6=2.00, buckwheat 8/4=2.00, barley 22/10=2.20, wheat 28/12≈2.33 and rice 11/5=2.20. For most *processed edible* Grains grains, `0.05 × output` is nominally 0.10–0.117 Nutrition/growDay; but grains require the explicit thresh/hull work and potentially milling and cooking. Existing grains' full source and 1:1 processing rule: [Grains SevenGrainFoodChainAudit](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Grains/blob/main/Docs/Balance/Crops/SevenGrainFoodChainAudit.md). **Do not compare the raw values as interchangeable usable food** or conclude equal labor, storage or seasonal productivity.

## Findings and decisions not yet supported

- Lentils were originally fast at 3 growth-days and produce 10 items per harvest. The adopted 8 days reduces the gross 100%-fertility item-rate from 3.333 to 1.250 units/growth-day (−62.5%). This is a *static ratio*, not a measured harvest labor/annual calorie outcome.
- Pumpkin's unchanged 20 items / 8 days gives 2.5 units and 0.125 source Nutrition proxy / growDay, approximately twice lentils' 0.0625. That difference requires proper soil, sow/harvest work, storage, temperature and eaten/processed nutrition comparison before deciding any additional pumpkin patch.
- Garlic produces fewer nutrition units and may have different non-staple roles; do not tune every crop to the same edible output.
- Flax and sugarcane require their own processing or intended use and **must not** be represented as zero nutrition merely because the raw XML does not directly define 0.05; item inheritance/recipe use must be verified.
- Absolute crops per 10 or 20 calendar days **cannot be derived** from `growDays` alone: outdoor 13h plant growth opportunity, local temperature, terrain fertility, job time and harvest replanting determine elapsed and economic outcomes. Native map simulation remains mandatory.
- Vanilla raw harvestYield and Nutrition and 1.7 upstream versions have **not** been source-verified by this MO-only audit. No yield/meal/tolerance/research patch is authorized from this document.

## Follow-up acceptance in the owner repository

1. Verify in actual loaded game, for each of the four 1.6 profiles, all 13 effective growDays and original effective harvestYield/food Nutrition; fail explicitly on load-order conflicts. Native loaded-Def Pickle covers only the growth-day portion initially.
2. Add real 10/20-day sow/harvest cycles at at least 50%, 100%, 140% fertility, at known temperature/light with real Pawn jobs; record per-tile harvested nutrition and sow/harvest/processing work, not just source rates.
3. Compare MO lentils, cabbage/pumpkin, Vanilla potato/corn and the seven Grains crops under common profiles before proposing any further growth/yield changes. Separate nonfood resource crops.
4. Confirm any 1.7 values using a legitimate actual 1.7 MO/Vanilla provider and game before declaring supported versions. Keep the V1 target 1.6 only until this passes.
