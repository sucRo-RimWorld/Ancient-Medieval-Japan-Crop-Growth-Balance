# Ancient & Medieval Japan - Crop Growth Balance — 設計書

**Status:** 2026-10-14, independent **RimWorld 1.6 development build only**. Four real-provider 1.6 profiles now confirm the source-owned *loaded growth-day* assertions (one Pickle scenario each, all PASS), **but strict game acceptance is only Vanilla and Vanilla+CCTO (ERROR0)**; MO and MO+CCTO both retain **2 Unity FMOD ERROR** per profile, so the overall native test workflow is FAIL. See [Native16-CropGrowth-20261014.md](Testing/Native16-CropGrowth-20261014.md) for exact source/game/run/evidence. Native sow/harvest, full-season yields, Grains coexistence and 1.7 remain unverified. Steam/2game publication is not approved. Original values derive from Grains PR #99 and are now owned by this dedicated repository. Grains extraction is PR #101 (main `b62d8ec9ee97b50eaa1043b9d51a44e4d1d412c3`).

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

**収量の初期監査（非確定値）：** 提供されたMO 1.6原本から収穫数・一部食品の明示的な栄養値を抽出し、`harvestYield/growDays` の相対指数を [PreliminaryHarvestProductivity-20261014.md](Balance/PreliminaryHarvestProductivity-20261014.md) に記録。レンズ豆の採用後の静的指数は1.25個/成長日、カボチャは既存2.5個/成長日。ただし実際の年間収穫数・労働・料理後の栄養量ではなく、追加の数値変更はまだ決定しない。

## Acceptance

- Static: exact 6 Vanilla and 7 MO `growDays` replacements, one per matched 1.6 crop Def, no other fields touched; MO-only files guarded; unique packageId; 1.6-only support claim; XML parse succeeds.
- Source-owned `Tests/E2E/CropGrowthBalanceSteps.cs` + `Tests/E2E/crop-growth-16.feature` define one strict loaded-Def native scenario per profile. The four native runs must load this Mod's source-pinned real XML and check exact 6 Vanilla values, the additional 7 MO values when MO is present, actual requested MO/CCTO provider identities and no Grains shortcut. A Pickle scenario PASS alone is insufficient: require complete game exit 0, native ERROR0, and independent version/source/log/summary evidence.
- Four 1.6 profiles: Vanilla, Vanilla+CCTO, MO, MO+CCTO. For each, check loaded target values and native ERROR0 logs; check sow, harvest and a 10/20-day temperature/fertility growth matrix. Grains-alone and Crop Balance-alone must be distinguishable, and Grains+Crop Balance must preserve Grains' seven crops and return corrected lentils only when Crop Balance is loaded.
- Confirm 1.7 actual upstream Defs and loadFolders semantics independently before claiming 1.7 support. Do not inherit old Grains game-test successes as new-Mod acceptance.
- Author-only distribution and private play until normal release gates are met.

References: Grains prior-art/source history in GitHub `Ancient-Medieval-Japan-Grains` PR #99 and `Docs/Research/CropGrowthNormalization-20261014.md` at commit `99c4771589a671ba337af7febcc5f1849957926b`.
