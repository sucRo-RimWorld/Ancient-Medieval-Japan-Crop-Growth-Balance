# Prior-art and split decision — 2026-10-14

This is a **redistribution without new gameplay** of a previously implemented and approved Grains `growDays` compatibility patch (PR #99). The author approved independent Mod ownership in preference to embedding broad Vanilla/MO crop edits in Grains. Scope unchanged at 6 Vanilla and 7 MO plants, growth day only.

- Vanilla/MO: native fast and uneven crops; MO 1.6 XML inspected from supplied `3219596926.zip`; Lentils `growDays=3`.
- AMJ: Grains seven-grain balance (4-12 growth days); only overlap is comparative growth-time rules. Keep actual grains/Plant_Rice/wheat integration owned by Grains.
- VE: Vanilla Plants Expanded adds crops/plowing; its existing scope is not this exact Grains-aligned base Vanilla/MO retune. Actual VE source and loaded-value interactions **not inspected**.
- Non-VE: Progression Agriculture addresses seeds/unlocks and deduplication; it does not itself establish Grains-compatible crop timing. Actual source and current interactions **not inspected**.

Prior research in original Grains `Docs/Research/CropGrowthNormalization-20261014.md`, historical JSON decision and GitHub PR #99. This split chooses a narrow optional standalone XML patch because changing roots/bulbs/vegetables requires no Grains food-chain runtime dependency. Future substantive additions require their own VE/non-VE source audit before changes. Existing season/nutrition/compatibility claims remain unverified.
