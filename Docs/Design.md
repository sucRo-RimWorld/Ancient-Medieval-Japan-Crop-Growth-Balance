# Ancient & Medieval Japan - Crop Growth Balance — 設計書

**Status:** 2026-10-14, independent **RimWorld 1.6 development build only**. Four real-provider 1.6 profiles now confirm the source-owned *loaded growth-day* assertions (one Pickle scenario each, all PASS), **but strict game acceptance is only Vanilla and Vanilla+CCTO (ERROR0)**; MO and MO+CCTO both retain **2 Unity FMOD ERROR** per profile, so the overall native test workflow is FAIL. See [Native16-CropGrowth-20261014.md](Testing/Native16-CropGrowth-20261014.md) for exact source/game/run/evidence. Native real-pawn sow/harvest and controlled 10/20-day `Plant.TickLong` growth sampling have now passed the four named Pickle scenarios at pinned source `c63ceb21a7e6673e2e04284509704acd8f308de9`, but these are **not** whole-season economics or 50/140% fertility field trials; Grains coexistence and 1.7 remain unverified. Steam/2game publication is not approved. Original values derive from Grains PR #99 and are now owned by this dedicated repository. Grains extraction is PR #101 (main `b62d8ec9ee97b50eaa1043b9d51a44e4d1d412c3`).

## Scope and responsibilities

- Own existing **Vanilla/MO** crop maturation and, after a separate quantitative review, crop harvest-yield balance as a stand-alone optional XML patch Mod. **Current V1 development implementation changes growth days only**, with yield unchanged. No dependency on Grains, CCTO, Environment or MO. MO definitions are patched only if MO is active.
- Own the named 13 `growDays` overrides below; this does not add new plants, foods, research or workstations.
- **Grains** owns its seven-grain PlantDefs and their internal crop balance, Vanilla `Plant_Rice` shared upland crop, and conditional MO `DankPyon_Plant_Wheat` integration. These two PlantDefs are excluded from this Mod.
- **CCTO** owns Vanilla/MO low-temperature survival/seasonal death and framework data; this Mod does not modify it.
- **Environment** owns terrain, geography and natural vegetation; this Mod has no environment requirements.
- Future bean/root/fiber crop Mods may use the same balance principles without importing a mandatory runtime dependency. The numerical reference is conceptual only, not shared game data.

## PlantDefs and exact `growDays` values

| Provider | PlantDef | Original 1.6 | AMJ Crop Growth Balance |
|---|---|---:|---:|
| Vanilla | `Plant_Potato` | 5.8 | **6** |
| Vanilla | `Plant_Corn` | 11.3 | **13** |
| Vanilla | `Plant_Strawberry` | 4.6 | **6** |
| Vanilla | `Plant_Cotton` | 8 | **10** |
| Vanilla | `Plant_Hops` | 5 | **7** |
| Vanilla | `Plant_Smokeleaf` | 7.5 | **9** |
| MO | `DankPyon_Plant_Onions` | 4 | **8** |
| MO | `DankPyon_Plant_Lentils` | 3 | **8** |
| MO | `DankPyon_Plant_Garlic` | 4 | **10** |
| MO | `DankPyon_Plant_Flax` | 7 | **9** |
| MO | `DankPyon_Plant_Sugarcane` | 5.8 | **12** |
| MO | `DankPyon_Plant_Carrots` | 3.25 | **6** |
| MO | `DankPyon_Plant_Tomatoes` | 4.25 | **7** |

## What these values mean

Game `growDays` is **not calendar days**: temperature, soil fertility and outdoor light/plant growth periods affect elapsed harvest days. Prefer agronomic relative maturity instead of direct Gregorian conversion. Numbers above are initial balance values; real harvest/food productivity and game experience remain unverified. Specifically MO lentils change `3 -> 8` with `harvestYield=10` unchanged.

**This initial source does not modify** yields, harvested items, fertility, temperature, death thresholds, graphics, research, recipes, nutrition, existing PlantDefs or stored save DefName identities. Future yield rebalance, part of the repository-wide objective, requires a new accepted numeric design plus annual calorie/area/labor accounting before production edits. Vanilla rice remains assigned to Grains when installed; without Grains, Vanilla rice behaves normally. MO wheat remains MO-owned with Grains conditional specialization only when Grains is installed. Trees, medical/special plants and already acceptable growth periods are outside this initial scope.

## Loading and compatibility

`loadFolders.xml` loads `/` for 1.6 and `Compatibility/MedievalOverhaul` only when `DankPyon.Medieval.Overhaul` is active. Root `/Patches/` only references Vanilla Defs. **The conditional MO Patch file must have a distinct relative filename** (`Patches/MOCropGrowthBalance.xml` rather than `Patches/CropGrowthBalance.xml`), because `loadFolders.xml` merges its later overlay and replaces files with identical relative paths. Actual 1.6 MO test initially showed `Plant_Potato=5.8` instead of 6 due to this collision, so `Tests/test_crop_growth.py` rejects any root/conditional relative-path overlap. This test applies only to the demonstrated file shadowing, not unrelated third-party load-order conflicts. Optional loadAfter entries are ordering hints, not hard dependencies. Normal RimWorld patching can conflict with another Mod writing the same `plant/growDays`; final loaded Def validation is mandatory. Both this Mod and old Grains builds may temporarily contain the same 13 crop patches; use an updated Grains checkout before testing clean ownership.

**収穫数・生育期間の数値再評価（確定仕様の変更なし）：** [PreliminaryHarvestProductivity-20261014.md](Balance/PreliminaryHarvestProductivity-20261014.md) の第二次監査では、Vanilla 6・MO 7・MO未調整対照2の基準収穫数と生育日数、Grains七穀の加工差を比較した。13時間/日の理想日照モデルでは20暦日中のトウモロコシ13日・MOサトウキビ12日は満熟不可。MOレンズ豆10個/8日（1.250個/成長日）に対してMOカボチャ20個/8日（2.500個/成長日）だが、これのみで作物性能は確定しない。MOトマトの`harvestAfterGrowth=0.30`はMO/MO+CCTO両構成で**収穫後に株が生存しGrowth=0.3000**となることを実証した。Project [run #38565073363](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38565073363)（owner `5bb15ade9e6aae0c8e412047374ad9a1d0ad2f1a`）では同じ株を制御暦で成長させ、2回目の実Pawn収穫（MO10個／MO+CCTO11個）後もGrowth=0.3000を確認。**支柱への実播種・最短再収穫周期・全マップの季節/仕事量は未検証**なので、再成長率1.633を年間経済バランスの確定値としては採用しない。専用支柱`DankPyon_Post`＋中級農業研究が播種前提で、カボチャにも上級農業研究が必要。肥沃度50%の成長係数は播種可否の保証ではなく、Vanilla主要作物の最低肥沃度70%との区別が必要。**現行13件の`growDays`と全`harvestYield`は維持**し、採用前に実ゲーム収穫・加工後の栄養と労働、Grains共存、VE/非VEを検証する。

**Grains同時ロードでの所有境界テスト（RimWorld 1.6実施済み、厳格2/4）：** [Project Linux実ゲーム #38577898742](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38577898742)（artifact **11900508581**、CGB `f4f5d1a61162b199ecea087fe8f5dc765b386511`、Grains `6cfd4417020caf55facb86233ae8ea02a00560a3`、Project `215a4f457874475fd23cd4b981c13245b5c7fc0b`）にて、実Grains＋CGBをVanilla/CCTO/MO/MO+CCTOの四構成に同時ロード。所有作物CGB Vanilla6＋任意MO7、Grains独立5穀＋`Plant_Rice=5`＋MO/非MO小麦12を実ロード後に確認し、Pickle**各1/1 PASS・終了0**。Unity ERROR**0/0/2/2**（MO系は元のFMOD unsupported-formatエラー）につき厳格判定**2/4 PASS、総合ワークフローFAIL**。対象`growDays`の同時ロード競合は検出されないが、Grains併用時の実Pawn播種・加工・季節生産量・労働、1.7、MO系ERROR0は未検証または未達成。必須依存・Grains所有作物への追加パッチは行わない。詳細は [Native16-CropGrowth](Testing/Native16-CropGrowth-20261014.md)。

## Acceptance

- Static: exact 6 Vanilla and 7 MO `growDays` replacements, one per matched 1.6 crop Def, no other fields touched; MO-only files guarded; unique packageId; 1.6-only support claim; XML parse succeeds.
- Source-owned `Tests/E2E/CropGrowthBalanceSteps.cs` + `Tests/E2E/crop-growth-16.feature` define one strict loaded-Def native scenario per profile. The four native runs must load this Mod's source-pinned real XML and check exact 6 Vanilla values, the additional 7 MO values when MO is present, actual requested MO/CCTO provider identities and no Grains shortcut. A Pickle scenario PASS alone is insufficient: require complete game exit 0, native ERROR0, and independent version/source/log/summary evidence.
- Four 1.6 profiles: Vanilla, Vanilla+CCTO, MO, MO+CCTO. For each, check loaded target values and native ERROR0 logs. Existing confirmed loaded-Def suite remains separately source-pinned.
- **First field-productivity native gate (new):** Source-owned `Tests/E2E/CropGrowthFieldQuickstart.cs`, `crop-growth-field-16.feature`, `CropGrowthFieldSteps.cs` test actual grower `JobDefOf.Sow` and `HarvestDesignated` pawn jobs on a disposable 50×50 map using *original* `Plant_Potato` (no MO) and `DankPyon_Plant_Lentils` (with MO). At real Soil fertility 100%, 25 °C and scripted outdoor daylight, after native sow `Plant.TickLong` is invoked at every 2,000-tick interval with controlled calendar/sky state for samples at 10 and 20 days; no invented crop yield/plant Defs. Only a plant genuinely reaching maturity may be designated and harvested by a real pawn JobDriver. Report harvest items, source `harvestYield` and native fertility growth factors at 50/100/140%.
- **Critical simulation boundary:** Advancing a controlled calendar and calling `Plant.TickLong` **does not run all other map/world/Pawn ticks for 10/20 calendar days**. This tests the real plant growth method and real sow/harvest Jobs but **not** whole-season farming economics, labor-time throughput, season events, long-term temperature/weather, natural-field crop cycles or nutrition from cooking. The original 70% Gravel, 100% Soil and 140% SoilRich **do have actual pawn sow and harvest Jobs** at native 1.6 ([run #38568822665](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38568822665)); however the initial plant-only growth samples used different starting game-calendar dates. Source now shares one starting tick/sky across all three fields, and **matched-calendar actual 1.6 four-profile Pickle PASS** is documented in [run #38570534129](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38570534129). At controlled day10, Potato Gravel/Soil/SoilRich growth=.5956/.6768/.7851 and MO Lentils=.4011/.5076/.6498; real sow and harvest succeed on all three native soils. MO FMOD ERROR2 still means strict overall workflow FAIL; no annual/whole-world simulation, average yield or nutrition economy is established. The new same-tomato controlled **second** Pawn harvest at original 100% Soil is a separate bounded native proof, not a full-season trial. The separate full seasonal/field-labor matrix remains OPEN. Grains-alone and Crop Balance-alone must be distinguishable, and Grains+Crop Balance must preserve Grains' seven crops and return corrected lentils only when Crop Balance is loaded.
- **Loaded harvested-resource numerical probe (source prepared, native rerun required):** The four-profile 1.6 loaded-Def Pickle scenario additionally records original crop base harvestYield, harvested ThingDef, loaded item Nutrition, minimum soil fertility and sowing skill under marker `AMJ_CROP_LOADED_RESOURCE`. The scenario checks presence of the harvested item, positive harvestYield and nonnegative finite Nutrition; also includes original unchanged MO cabbage and pumpkin as comparators, distinct from the seven changed MO crops. Those inputs are not actual Pawn-modified counts, processing yields, worker time, edible output or seasonal/annual production. No changes to the 13 approved crop values, no claims of real 1.6 PASS until the new source runs.
- Confirm 1.7 actual upstream Defs and loadFolders semantics independently before claiming 1.7 support. Do not inherit old Grains game-test successes as new-Mod acceptance.
- Author-only distribution and private play until normal release gates are met.

References: Grains prior-art/source history in GitHub `Ancient-Medieval-Japan-Grains` PR #99 and `Docs/Research/CropGrowthNormalization-20261014.md` at commit `99c4771589a671ba337af7febcc5f1849957926b`.
