# Ancient-Medieval-Japan-Crop-Growth-Balance
バニラ、MOの作物の生育期間・収穫量をAMJ作物のバランスに合わせる

Standalone **RimWorld 1.6 development** Mod (XML-only). Rebalances the growth times of 6 original RimWorld crops plus 7 **optional** Medieval Overhaul crops, including lentils (`growDays 3 -> 8`). Grains is not required. CCTO is not required; cold death is outside this Mod's scope.

**Current implementation:** only 13 existing crop `growDays` values are changed (Vanilla 6 / optional MO 7). Yield and nutrition are not changed yet; harvest-yield tuning requires separate native production/processing analysis and an approved specification.

See `Docs/Design.md` for the complete adopted table, exceptions and testing gates. Not released on Steam/2game. Native Linux RimWorld 1.6 loaded-Def checks and isolated real-pawn sow/grow/harvest Pickle scenarios each completed 4/4 profiles successfully on pinned source; the strict full-run gate remains **2/4 PASS**, because both MO profiles emitted 2 independently reproduced FMOD Unity errors. Field sampling used controlled `Plant.TickLong` calendar advances, **not** a full-season simulation. Grains coexistence and 1.7 support are **not yet verified**. See `Docs/Testing/Native16-CropGrowth-20261014.md`.

## Installation

Extract the **playable** ZIP to the RimWorld Mods folder so the directory `Ancient-Medieval-Japan-Crop-Growth-Balance` directly contains `About`, `Patches`, `Compatibility`, and `loadFolders.xml`. Do not place the source ZIP directly in the Mods folder. Select it in the Mods list. MO is optional; with MO installed, put Crop Growth Balance after MO (also encoded as a loadAfter hint). It may be installed independently of Grains. To avoid duplicate patches, update Grains to a revision after the split PR before testing combined profiles.
