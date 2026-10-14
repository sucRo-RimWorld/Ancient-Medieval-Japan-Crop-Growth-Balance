# AMJ Crop Growth Balance coordination

This file is the authoritative owner-workstream handoff only on `main`. The numerical specification is `Docs/Design.md` and executable XML, not this handoff.

## CROP-GROWTH-SPLIT-20261014 — source import

**Owner:** `sucRo-RimWorld/Ancient-Medieval-Japan-Crop-Growth-Balance`. **Status:** independent 1.6 source import, GitHub PR/CI status must be verified by its actual merge SHA. Native RimWorld tests and Steam release NOT RUN.

**Approved scope:** 6 Vanilla `growDays` changes and 7 optional Medieval Overhaul changes (`DankPyon_Plant_Lentils` 3→8); no crop IDs, yields, temperatures, recipes or food adjustments. Unique packageId, separate load folder, no hard dependency on Grains/MO/CCTO. This module owns broad Vanilla/MO crop maturation; Grains retains AMJ's seven grains, shared rice, and conditional MO wheat. Original values in Grains PR #99; Grains extraction PR #101 merged as `b62d8ec9ee97b50eaa1043b9d51a44e4d1d412c3`.

**Verification:** local Python static tests (with provided actual MO 1.6 ZIP) 5/5 PASS before import. GitHub PR/main CI separately required. Actual 1.6 Vanilla, Vanilla+CCTO, MO, MO+CCTO loaded values/ERROR0, real sow and harvest, Grains-alone vs combined-game, 1.7 support, Steam/2game publication are OPEN. Do not extrapolate old Grains test results.
