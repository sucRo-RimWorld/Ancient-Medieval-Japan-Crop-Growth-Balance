# Converted-audio Linux 1.6 Crop field regression — 4/4 functional PASS (2026-10-14)

**Actual execution:** [Project Actions #38573922208](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38573922208), native log artifact **11898622328**, actual RimWorld **1.6.4871 rev600**, owner source **`7a3c7a745deecd0ba0e58d0a207b9f10e2bf1ca6`**, Project host **`f07c645480779853289d82d232eb49f0bbe95cec`**. GitHub workflow and native `mo-wav-owner-native` job completed **success** (not merely decoder download or a skipped native job).

| Native profile | Source-owned original field Pickle | Unity ERROR | Natural game exit | Converted-audio functional result |
|---|---|---:|---:|---|
| Vanilla | 1/1 | 0 | 0 | PASS |
| Vanilla+CCTO | 1/1 | 0 | 0 | PASS |
| MO | 1/1 | 0 | 0 | PASS |
| MO+CCTO | 1/1 | 0 | 0 | PASS |

The original matched-calendar real Pawn sow/harvest and 70/100/140% terrain tests, original Potato/MO Lentils, 10/20/30-day plant-only growth and same-tomato assertions were run by the **unchanged original owner test code**, using the source-pinned four-profile suite on an isolated licensed Linux game copy. The Project host first verified the original SYR `Sounds/UI/DebugGoose/Honk.mp3` SHA-256 `26dfcb1140ebd2e90af39ef36a29319188d59d7a25605c69fd54c4959f62f639` and MO `Sounds/DankPyon_FlaskBreak.mp3` SHA-256 `a7d5b05bc6c40bbb90182065821b87b31929c9d597b1fee3fe9392c0f5989ad2`, then converted **only their disposable copies** to 44.1 kHz stereo 16-bit PCM WAV. Verified generated WAV digests `ed424458599dd60088e264f01ca51de53f21c155aacb424058a27e6a95f0b591` and `6f9136407694bcbbb9b4036692cc4e02f65a7fb66bea3f138df16c1f6d2e6d43`. Original source provider packages, SoundDefs, source MP3s and user/Steam installations were unchanged. No Unity errors were ignored, filtered or waived.

**Acceptance boundary:** This is **4/4 real-game Pickle + ERROR0 with corrected private audio**, not a claim of **4/4 strict original-provider MP3 acceptance** or audible sound playback. The immediately preceding [unmodified-audio original-provider run #38570534129](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38570534129), artifact 11898411270, had the same four source-owned Pickle scenarios 1/1 each, but MO/MO+CCTO Unity FMOD2 each and failed strict ERROR0. Full-season/economics, 50% native sow, Grains coexistence, 1.7 and public release remain independently OPEN. The controlled terrain/yield numbers and single-plant interpretation are in the subsequent matched-calendar section; WAV conversion **does not change** their values or authorize rebalancing.

---

## Matched-calendar real fertility harvest #38570534129 — four native Pickle PASS (2026-10-14)

**Provenance:** [Project 1.6 Linux run #38570534129](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38570534129), artifact **11898411270**; owner source `7a3c7a745deecd0ba0e58d0a207b9f10e2bf1ca6`, Project host `236002bcf7ba4be28d1b4d3ad5e6573202d73cc3`, native RimWorld `1.6.4871 rev600`. Original Gravel=70%, Soil=100%, SoilRich=140%, native Pawn Sow/Harvest jobs and original Potato or MO Lentils, 25°C and original plant growth `Plant.TickLong` with 2,000-tick steps. Within each profile, baseline 100% Soil, later 70% Gravel and 140% SoilRich growth samples started at **exactly the same `matchedGrowthCalendar`** (reported per profile as 15342/15414/15342/15318 game ticks). These are controlled *plant-only* 10/20/30-day growth measurements, NOT full world/map/colony seasons.

| Profile | 10-day growth on 70% / 100% / 140% | 20-day growth on 70% / 100% / 140% | Original 70% / 100% / 140% actual harvested items |
|---|---|---|---|
| Vanilla Potato | 0.5956 / 0.6768 / 0.7851 | 1.0000 / 1.0000 / 1.0000 | 15 / 15 / 15; base yield 11, Pawn yield stat 1.1232 |
| Vanilla+CCTO Potato | 0.5955 / 0.6767 / 0.7850 | 1.0000 / 1.0000 / 1.0000 | 13 / 14 / 14; base 11, Pawn stat 1.0215 |
| MO Lentils | 0.4011 / 0.5076 / 0.6498 | 0.8540 / 1.0000 / 1.0000 | 14 / 13 / 14; base 10, Pawn stat 1.1300 |
| MO+CCTO Lentils | 0.4011 / 0.5077 / 0.6498 | 0.8542 / 1.0000 / 1.0000 | 14 / 13 / 14; base 10, Pawn stat 1.1300 |

- Growth consistently increases with real fertility under **matched start sky/time**, independently of CCTO at controlled 25°C. Lentils on Gravel are below full growth at simulated day20 (about 0.854); in the native `>=0.999` measurement their first near-full maturity was step **691** on Gravel (about 23.0 calendar days) vs step **449** on SoilRich (about 15.0 calendar days). Potato recorded the same first-threshold steps **482** on Gravel (16.1 days) and **384** on SoilRich (12.8 days). Those are **controlled threshold observations**, not earliest valid harvest times or real days-to-harvest in ordinary weather.
- All four original-provider Pickle scenarios **1/1 PASS**, game exit0. Unity ERROR0 for Vanilla/CCTO, but both MO profiles retain independent 2 upstream FMOD errors, so **overall GitHub workflow FAIL and only two strict profile PASS**; no waivers.
- The numbers are each **one plant, one qualified Pawn** per terrain/profile, no sample mean, no natural calendar/weather/migration, food Nutrition, transport or processing labor. Native tomato two-harvest source-owned assertions remain PASS in both MO profiles; trellis planting, 50% native sow, Grains loaded coexistence and 1.7 remain untested. The initial *different-start-date* soil growth comparison in #38568822665 is superseded for controlled rate comparisons **only** by the matched-calendar run here; its real original sow/harvest Job evidence remains valid. All 13 `growDays` and `harvestYield` production values unchanged.

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

## Focused four-profile rerun — all source-owned Pickle scenarios PASS (2026-10-14)

- [Project Actions #38563498513](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38563498513), artifact **11893059095**; exact owner source `63d85aa1a943ff79f9a7af8816f95e4700da8d81`, Project source `49082be015b87f19bbfc18d3162aff4585976ffa`, actual game 1.6.4871 rev600.
- **Pickle:** Vanilla 1/1, Vanilla+CCTO 1/1, MO 1/1, MO+CCTO 1/1 all PASS with process exit0. **Unity ERROR:** 0/0/2/2. Thus **strict workflow FAIL**, only Vanilla and Vanilla+CCTO meet complete profile gate; independent MO FMOD2 persists with no masking. Random worker eligibility pre-filter successfully let MO-only reach its native jobs.
- **Native original terrain WorkGiver sow offer** (not real sow/harvest on each soil): original `Gravel` 0.70, `Soil` 1.00, `SoilRich` 1.40 each offered `JobDefOf.Sow` for potato in both Vanilla profiles and lentils in both MO profiles; loaded `fertilityMin=0.70` for these crops. Native 50% sow job **NOT RUN**.
- On real `Soil` 1.00, first real pawn sow and harvest with controlled `Plant.TickLong` 10/20-day samples: potato day10 0.6768/day20 1.0000, harvested **16** (Vanilla, worker yieldStat 1.1300), **15** (CCTO, yieldStat 1.1300), base 11. Lentils day10 0.5076/0.5077, day20 1.0000, harvested **14** (MO, yieldStat 1.1232), **13** (MO+CCTO, yieldStat 1.1063), base 10. One harvest per crop/profile does not identify expected mean, annual food/nutrition or CCTO effects.
- **MO tomato — first native same-plant behavior validated twice:** each MO and MO+CCTO test placed the genuine, pre-matured original MO tomato on Soil and used an actual Pawn `HarvestDesignated` Job. Both harvested **11** tomatoes and left the **same living, spawned plant with Growth=0.3000**. This confirms first-harvest 30% retention in both profiles. **Not tested:** sow on real `DankPyon_Post`, real second ripening interval/second harvest, colony season, field labor. Preserving 30% alone does **not** justify adopting the modeled rate 8/4.9 as a long-term harvest economy fact.

## Native original-soil harvest #38568822665 — real Jobs PASS; growth comparison calendar correction OPEN

**Actual evidence:** [Project run #38568822665](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38568822665), artifact **11897140747**; owner source `718c0bff6edda79390f6328d2796bf1c991c4c88`, Project source `ef39b7dbf9425f4305c355ad8c4b5d48a7201e47`, native Linux RimWorld 1.6.4871 rev600. All four original-provider profiles **1/1 Pickle PASS and game exit0**, actual Pawn Sow and Harvest designated on original Gravel 70%, Soil 100% and SoilRich 140%, plus prior MO same-tomato repeat harvesting. Unity ERROR: Vanilla0, CCTO0, MO2, MO+CCTO2 (independent FMOD), so **strict workflow FAIL** and only non-MO profiles meet full runtime gate.

Actual observed items per single plant on 70/100/140% respectively: Vanilla Potato **16/16/15** (base yield 11, worker yield stat 1.1300); Vanilla+CCTO Potato **14/14/14** (base 11, stat 1.0961); MO Lentils **14/13/13** (base 10, stat 1.1300); MO+CCTO Lentils **13/14/14** (base 10, stat 1.1300). Within each profile all soil cycles used one real original Pawn and real sow/harvest Jobs. One plant per soil cannot estimate mean harvest yield.

**Critical scope correction:** First native 70%, 100%, 140% crop trials advanced the *plant-only simulated calendar sequentially*, with **different start game ticks** between soils. The original outdoor sky thereby changed with time and confounds any direct *fertility-vs-growth-speed* comparison. Example: original Potato Soil day10 Growth=.6768, later Gravel day10 Growth=.7041, despite poorer soil. Actual native sow/harvest events and counts remain valid; relative rate comparisons are **NOT accepted**.

**Correction staged, NOT RUN:** preserve the original 100%-Soil field cycle's exact `TicksGame` as `matchedGrowthCalendar`; after a real sow on Gravel or SoilRich, use that identical tick and sky phase for the source-owned `Plant.TickLong` controlled growth samples. Record the reference calendar in each real-harvest log and rerun the complete four profiles. Does not advance world/map systems over a natural year, include 50% native sow or prove average yield, trellis sow or processed Nutrition.


## Native physical harvest comparison by original 70/100/140% soil — prepared, NOT yet executed

The owner field scenario is extended with original terrain `Gravel` (fertility 0.70), `Soil` (1.00; previously executed), and `SoilRich` (1.40) on the same disposable 50×50 map and **actual original crop** (Vanilla potato, MO lentils). For 70 and 140%, tests execute the **real** WorkGiver `JobDefOf.Sow` and Pawn JobDriver; let the **real** grown Plant `TickLong` under controlled 25°C and outdoor sky/calendar to day 10/20/30 (at 2000-tick steps); assert full maturity and real `HarvestDesignated` Pawn JobDriver output. Each profile logs actual crop output, base yield, pawn harvest stat, native fertility, controlled-day growth fractions and first observed maturity step. It resets the original cell terrain and reuses the same qualified Pawn. No yield multiplier is faked, and there is no offsoil spawning shortcut.

**Boundary:** scripted plant-only growth calendar, NOT all game systems/world season. No actual 50% terrain introduced (sow limit checked separately), no MO vine-post sow, no Grains load, no year/food-processing economy. No 70/140% native PASS may be claimed until the exact-source Linux four-profile run and strict ERROR gate are inspected.

## Same-plant tomato second harvest — actual native test #38565073363 (2026-10-14)

- **Exact provenance:** [Project Linux 1.6 Actions #38565073363](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38565073363); artifact **11894281737**, exact owner source `5bb15ade9e6aae0c8e412047374ad9a1d0ad2f1a`; Project `2524bff44adf0e6faeadc92c16fed42e7399efce`; actual game `1.6.4871 rev600` under isolated Xvfb/Pickle host. Production plants are original loaded Vanilla/MO Defs.
- **Result by profile:** Vanilla 1/1 PASS, Vanilla+CCTO 1/1 PASS, MO 1/1 PASS, MO+CCTO 1/1 PASS; all four game exit0, 0 failed/skipped/flaky. Unity ERROR count **0/0/2/2** in that order; the two MO profiles retain independently demonstrated upstream **FMOD** errors, so **overall workflow FAIL; strict per-profile PASS only Vanilla and Vanilla+CCTO**. Do not waive or mask these runtime errors.
- **MO identical original plant harvested twice by real Pawn JobDriver:** controlled original mature `DankPyon_Plant_Tomatoes` was spawned on 100% Soil, 25°C. This explicitly **bypasses actual MO trellis-post sowing and research setup**. First actual `HarvestDesignated` yielded **11** tomatoes (MO) / **10** (MO+CCTO), retaining the original plant spawned/alive at **Growth 0.3000**.
- After the first harvest, the **same living plant** was advanced by actual `Plant.TickLong` calls at **2,000-tick increments** with controlled outdoor sky/calendar. At 10 additional calendar days the original plant measured **Growth 0.9854** for both profiles; at 20 days **0.9997**. This is a plant-only growth sample, **not a map/world-wide 20-day simulation**; 10 days was **not** established as full maturity or as the earliest second harvest date.
- On that same plant a **second real** `WorkGiver_PlantsCut` `JobDefOf.HarvestDesignated` was offered/executed successfully: **10** additional tomatoes (MO) / **11** additional tomatoes (MO+CCTO). The original plant **remained spawned/alive and Growth returned to 0.3000** after the second harvest in both profiles. Two actual jobs therefore establish *same-plant repeat harvesting under this controlled environment*, while the exact minimum ripening interval and normal trellis/work/whole-season productivity remain unmeasured.
- The harvested item counts (11+10 or 10+11) are the outputs of **one controlled test Pawn/profile**, not raw base `harvestYield` 8, or expected average yield across Pawn skills or 70/140% soil. Do **not** adopt an accelerated `8/4.9` gross productivity coefficient as a field-validated rate: it remains a source-ratio calculation under ideal no-labor assumptions. Do not infer CCTO yield impacts from the mirrored totals.
- All four profiles additionally confirmed actual original `Gravel` **70%**, `Soil` **100%**, `SoilRich` **140%** native `JobDefOf.Sow` *offers* for Potato (Vanilla profiles) or Lentils (MO profiles). The single executed sow and harvest remain on **100%** Soil; no actual 50% terrain sow test, 70/140% harvest, normal `DankPyon_Post` sow, Grains coexistence, 1.7 compatibility, cooked Nutrition or calendar seasons were tested.

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
