# Crop Growth Balance — Linux 1.6 native loaded-Def evidence (2026-10-14)

**Owner:** AMJ Crop Growth Balance. **Canonical growth-day numerical contract:** [Design.md](../Design.md), `Patches/CropGrowthBalance.xml` and `Compatibility/MedievalOverhaul/Patches/MOCropGrowthBalance.xml`. This report records actual runtime results; it is not approval for full native or season/yield release.

## Test premises and scope

The Project persistent, isolated Linux host ran actual **RimWorld 1.6.4871 rev600** with source-pinned Crop Growth Balance, official/native Pickle, source-owned [one-scenario loaded-Def assertion](../../Tests/E2E/crop-growth-16.feature) and original conditional MO 1.6 providers. Profiles were separated into fresh game/save/config, with process exit/Unity log checks and exact source SHA.

- **Vanilla:** `Plant_Potato=6`, `Plant_Corn=13`, `Plant_Strawberry=6`, `Plant_Cotton=10`, `Plant_Hops=7`, `Plant_Smokeleaf=9`.
- **MO additionally:** `DankPyon_Plant_Onions=8`, `DankPyon_Plant_Lentils=8`, `DankPyon_Plant_Garlic=10`, `DankPyon_Plant_Flax=9`, `DankPyon_Plant_Sugarcane=12`, `DankPyon_Plant_Carrots=6`, `DankPyon_Plant_Tomatoes=7`.
- **Conditional identities:** verified requested original MO/SYR/VEF packages and CCTO when specified. **Grains is intentionally absent**: this test validates standalone source ownership, not Grains coexistence.
- **Full profile PASS conditions:** one exact Pickle scenario passing, no failed/skipped/flaky, Unity ERROR **0**, game process exit **0**, real version/log, and exact authored source. An XML/static PASS cannot satisfy this gate.

## Initial native run — real regression, before MO path fix

[Project GitHub Actions #38548286262](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38548286262), artifact **11885243453**, source owner `1f084a1dc4a26513ab99fd9de69f1295c737188e`.

| Profile | Pickle | Unity ERROR | Process exit | Strict outcome | Diagnostic |
|---|---|---:|---:|---|---|
| Vanilla | 1/1 | 0 | 0 | PASS | All six Vanilla growth values match |
| Vanilla + CCTO | 1/1 | 0 | 0 | PASS | All six values match |
| MO | 0/1 | 2 | 1 | FAIL | `Plant_Potato` loaded 5.8, expected 6; FMOD errors |
| MO + CCTO | 0/1 | 2 | 1 | FAIL | Same Vanilla patch loss and FMOD errors |

**Root cause found from the real game:** root and conditional MO folders both contained the relative filename `Patches/CropGrowthBalance.xml`; RimWorld 1.6 `loadFolders.xml` overlay caused the conditional file to replace the Vanilla root patch under MO. This was a real crop-loading defect, **not** just an upstream audio incident. The owner fixed it in [PR #4](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Crop-Growth-Balance/pull/4), merged main `14bce853a0828f83d15e47067efc72c8a63cbe34`: MO file renamed to `Patches/MOCropGrowthBalance.xml` and static test added to prohibit relative-path collisions.

## Corrected source native rerun — loaded values established, full MO gate still fails

[Project GitHub Actions #38549522035](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38549522035), artifact **11886495505**, Project test-host [PR #190](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/pull/190), pinned owner source **`14bce853a0828f83d15e47067efc72c8a63cbe34`**.

| Profile | Exact named Pickle / all expected growth-day assertions | Unity ERROR | Process exit | Full strict outcome |
|---|---|---:|---:|---|
| Vanilla | 1/1 PASS | 0 | 0 | **PASS** |
| Vanilla + CCTO | 1/1 PASS | 0 | 0 | **PASS** |
| MO | **1/1 PASS** | **2** | 0 | **FAIL** (FMOD) |
| MO + CCTO | **1/1 PASS** | **2** | 0 | **FAIL** (FMOD) |

Actual corrected run proves **all 13 target crop growth values are loaded at their approved numbers with real MO and CCTO present** (4/4 profile-specific crop/identity Pickle assertions PASS). It does **not** prove four-profile ERROR-zero acceptance: 2/4 profiles only have strict success; full GitHub Actions result is **FAILURE**.

The remaining MO error events are `Cannot create FMOD::Sound instance for clip "" (FMOD error: Unsupported file or audio format.)` (two each). Separately reproduced MO-provider FMOD failures without this crop Mod have been documented by the Project native host owner. **Do not suppress, downgrade or attribute this failure to the crop patch without evidence.** No new native MO ERROR0 claim.

## Focused native soil/tomato field run #38561878157 — first execution, strict FAIL (2026-10-14)

- **Source / host:** owner `1ee767c48e4072b8b5ce0592b4ab032da62943c5`, Project `e24c2bbf121b3c606141d28926d6fe6dbc2d743a`, actual 1.6.4871 rev600. [Actions #38561878157](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38561878157), archived source-owned log artifact **11892407640**.
- **Verified scenario outcomes:** Vanilla 1/1 PASS, CCTO 1/1 PASS, MO **0/1 FAIL**, MO+CCTO 1/1 PASS; all four were launched. Unity ERROR: 0, 0, 2, 2 respectively. MO exit **1**; others exit **0**. Therefore full workflow **FAIL**, 2/4 strict profile PASS, not a four-profile successful acceptance.
- **New MO-only failure cause:** Test constructor generated a random Pawn with Plants skill set to 20 whose actual `PlantHarvestYield` stat was nevertheless **below 1.0**; the fixture explicitly failed `Real harvest requires skilled worker without yield failure` **before** real sow or soil probes in that MO profile. This is a non-deterministic **test worker-selection defect**, not evidence of failed crop balance or an additional application Unity ERROR. The next source change filters real candidate Pawns by both skills and measured yield stat, retaining the original >=1 fixture requirement.
- **Real native WorkGiver job offers (not executed on each terrain):** Vanilla Potato and MO+CCTO Lentils both offered the original `JobDefOf.Sow` on unmodified `Gravel=0.70`, `Soil=1.00`, `SoilRich=1.40`. Vanilla+CCTO Potato likewise passed. This checks **sow-job eligibility only**, not real harvest on 70/140% terrains. Loaded `fertilityMin=0.70` in the three successful profiles; **no actual 50% terrain was instantiated or tested**.
- **100%-fertility controlled growth and real pawn harvest:** Potato Vanilla day10 **0.6768**, day20 **1.0000**, harvest **15 RawPotatoes** (base 11); Potato+CCTO same growth, harvest **14**. MO+CCTO Lentils day10 **0.5076**, day20 **1.0000**, harvest **13 RawLentils** (base 10). All three profiles' original `Plant.TickLong`/Pawn-job scenario passed. The 10/20-day span advances plants on a controlled calendar, **not whole world/map systems**.
- **Direct native tomato result (MO+CCTO only):** A **test-spawned mature original MO tomato** at 100% Soil was harvested by a **real** `HarvestDesignated` pawn job. Logged **10 `DankPyon_RawTomatoes`** (base `harvestYield=8`), `destroyed=False`, `spawned=True`, **`growthAfter=0.3000`**. This **establishes retention after the first real pawn harvest in MO+CCTO**; it does not establish that the normal `DankPyon_Post` sow job, a second ripening cycle or second harvest is functional. The `harvestAfterGrowth=0.30` XML-only 30% retention hypothesis now has *one native-profile/one-job observation*, **not** a measured 8/4.9 repeat-yield rate. The MO-only comparison remains untested because its fixture failed early.
- Scope: actual original game/POV, isolated 50x50 map and source-owned Defs; no production game/Mod/Steam/save changes. This run does not compare years, seasons, raw/eaten Nutrition, Grains processing labor or repeated tillage.
- Rerun requirement: fix random-pawn qualification, pin the resulting exact owner SHA in Project host, repeat the full four-profile matrix, inspect the final logs/summary and preserve the MO FMOD ERROR-zero gate (no waiver).

## Native real-pawn field run — controlled crop sampling, not whole-season farming

[Project Linux 1.6 Actions #38552728714](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38552728714), artifact **11887632208** (game/launcher logs, build logs, individual JSON summaries), owner source **`c63ceb21a7e6673e2e04284509704acd8f308de9`**. Actual RimWorld **1.6.4871 rev600**, isolated source-owned `CropGrowthFieldQuickstart`, `CropGrowthFieldSteps` and one `crop-growth-field-16.feature` scenario per profile. All four scenarios **1/1 PASS**, all four process exits 0, no timeouts. Vanilla and Vanilla+CCTO additionally satisfy Unity ERROR0; MO and MO+CCTO have the same **2 previously independently reproduced FMOD ERRORs each**, so the four-profile strict result is **FAIL**, 2/4 full profiles passing.

The disposable map uses original native crop Defs (no fake growing/yield defs), a generated pawn with growing skill 20, real `JobDefOf.Sow` and `HarvestDesignated` Jobs, Soil fertility 100%, 25 °C and synthetic outdoor daylight. Every 2,000 controlled calendar ticks, `Plant.TickLong` was called; 300 and 600 calls represent 10 and 20 in-game calendar days, **without advancing other map/world/pawn systems for those days**.

| Profile | Original crop | Growth at day 10 | Day 20 | Real pawn harvest (items) | Base harvestYield | Native fertility growth factor 50 / 100 / 140% | Pickle | ERROR | Full gate |
|---|---|---:|---:|---:|---:|---|---|---:|---|
| Vanilla | `Plant_Potato` | 0.6768 | 1.0000 | 15 `RawPotatoes` | 11 | 0.80 / 1.00 / 1.16 | 1/1 PASS | 0 | PASS |
| Vanilla+CCTO | `Plant_Potato` | 0.6768 | 1.0000 | 14 `RawPotatoes` | 11 | 0.80 / 1.00 / 1.16 | 1/1 PASS | 0 | PASS |
| MO | `DankPyon_Plant_Lentils` | 0.5076 | 1.0000 | 14 `DankPyon_RawLentils` | 10 | 0.65 / 1.00 / 1.28 | 1/1 PASS | 2 | FAIL (FMOD) |
| MO+CCTO | `DankPyon_Plant_Lentils` | 0.5077 | 1.0000 | 14 `DankPyon_RawLentils` | 10 | 0.65 / 1.00 / 1.28 | 1/1 PASS | 2 | FAIL (FMOD) |

The 50% and 140% figures are **native fertility growth-factor calculations**, not actual harvests on those terrains. One harvest per profile is not a statistically stable yield or annual calorie comparison; pawn harvest yield bonuses and random rounding mean the 14/15 potato difference is not a CCTO effect. This test **does not establish** full real calendar seasonal transitions, repeated harvests, sow/harvest/processing throughput, cooking nutrition, or normal-play annual yields. No additional crop yield/growth numbers are authorized by these observations.

## Remaining tasks

- Full-season/repeated-crop whole-game map simulation with actual 50/100/140% terrain harvests, 10/20-calendar-day outdoor growing seasons, consumption/processing, calorie/area and work-time accounting; first controlled real pawn sow/harvest and TickLong growth sampling is already evidenced above. Static source ratio audit lives at [PreliminaryHarvestProductivity-20261014.md](../Balance/PreliminaryHarvestProductivity-20261014.md). No new yield number is approved yet.
- Native Grains-alone vs Crop Growth Balance-alone vs optional combined profiles; verify Grains seven crops, `Plant_Rice=5`, MO wheat 12 and no duplicate global patches.
- MO strict ERROR0 remains open on the native Linux host pending independently resolved existing FMOD source issue. Do not weaken overall acceptance thresholds.
- RimWorld **1.7** official source/actual game/Vanilla and MO loaded profile verification; not supported or tested by these 1.6 runs.
- Steam/2game and user's normal Mods folder **not updated or published** by these GitHub test runs.
