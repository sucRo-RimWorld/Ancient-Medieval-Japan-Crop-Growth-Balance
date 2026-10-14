# Prior-art and split decision — 2026-10-14

This is a **redistribution without new gameplay** of a previously implemented and approved Grains `growDays` compatibility patch (PR #99). The author approved independent Mod ownership in preference to embedding broad Vanilla/MO crop edits in Grains. Scope unchanged at 6 Vanilla and 7 MO plants, growth day only.

- Vanilla/MO: native fast and uneven crops; MO 1.6 XML inspected from supplied `3219596926.zip`; Lentils `growDays=3`.
- AMJ: Grains seven-grain balance (4-12 growth days); only overlap is comparative growth-time rules. Keep actual grains/Plant_Rice/wheat integration owned by Grains.
- VE: Vanilla Plants Expanded adds crops/plowing; its existing scope is not this exact Grains-aligned base Vanilla/MO retune. Actual VE source and loaded-value interactions **not inspected**.
- Non-VE: Progression Agriculture addresses seeds/unlocks and deduplication; it does not itself establish Grains-compatible crop timing. Actual source and current interactions **not inspected**.

Prior research in original Grains `Docs/Research/CropGrowthNormalization-20261014.md`, historical JSON decision and GitHub PR #99. This split chooses a narrow optional standalone XML patch because changing roots/bulbs/vegetables requires no Grains food-chain runtime dependency. Future substantive additions require their own VE/non-VE source audit before changes. Existing season/nutrition/compatibility claims remain unverified.

## Native 1.6 regression discovered after split

Project isolated native [Actions #38548286262](https://github.com/sucRo-RimWorld/Ancient-Medieval-Japan-Project/actions/runs/38548286262) loaded the pinned independent owner commit `1f084a1dc4a26513ab99fd9de69f1295c737188e`. Vanilla and Vanilla+CCTO exact 6-crop loaded growth-day tests passed with Unity ERROR0. MO and MO+CCTO exact-crop checks failed at Vanilla potato (loaded 5.8, expected 6), and logged the already independently reproduced 2 MO FMOD errors. Root and conditional MO patches both had the same *relative* `Patches/CropGrowthBalance.xml` filename, so the latter overlaid the former in 1.6 RimWorld's `loadFolders` merging. This is a true production-loading defect separate from FMOD. Renamed the conditional MO file to `Patches/MOCropGrowthBalance.xml` and added a static collision test; a new native four-profile run is required before declaring MO crop values PASS. Do not characterize the failed MO run as an audio-only incident.
