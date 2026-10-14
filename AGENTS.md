# AMJ Crop Growth Balance Agent Instructions

1. At task start, read this repository's `AGENTS.md`, then `main:Docs/Coordination.md` **once a GitHub main branch has been initialized**; follow Project `AGENTS.md` and `Docs/SharedRules.md`.
2. `Docs/Design.md` is the canonical crop balance scope/values; `Patches/CropGrowthBalance.xml` and `Compatibility/MedievalOverhaul/Patches/CropGrowthBalance.xml` own the XML implementation.
3. Modify only approved `growDays` unless wider changes have a properly reviewed design and VE/non-VE audit. Never modify Grains seven crop values, Vanilla Plant_Rice or MO wheat here; never add CCTO frost rules.
4. Keep MO optional in `loadFolders.xml`; Vanilla and Vanilla+CCTO have no unconditional MO Def references.
5. Test with `python -m unittest discover -s Tests -p 'test_*.py'` before PR, native four-profile Pickle after CI; do not claim loaded-game PASS from static checks.
6. Use topic branches, PR, exact CI and main verification. No direct main writes. GitHub publication and Steam Workshop are distinct.
