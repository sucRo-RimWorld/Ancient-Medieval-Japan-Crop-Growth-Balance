#!/usr/bin/env python3
"""Source-owned, deliberately limited 1.6 crop growth/harvest balance index.

Not a RimWorld simulation or a recommendation to change production crop Defs.
The Vanilla benchmark is the RimWorld Wiki (plants/crop, 2026-10-14).
MO input is original provided Steam archive 3219596926.zip, version 1.6 only.
Grains references are owner Docs/Balance/Crops/SevenGrainFoodChainAudit.md.
"""
from __future__ import annotations

import argparse
import json
import re
import xml.etree.ElementTree as ET
from pathlib import Path
from zipfile import ZipFile

ROOT = Path(__file__).resolve().parents[1]
BASE = {
    "Plant_Potato": (5.8, 11, .4), "Plant_Corn": (11.3, 22, 1.0),
    "Plant_Strawberry": (4.6, 8, 1.0), "Plant_Cotton": (8, 10, 1.0),
    "Plant_Hops": (5, 8, .7), "Plant_Smokeleaf": (7.5, 9, 1.0),
}
MO = {
    "DankPyon_Plant_Onions": (4, 13, .2, 0),
    "DankPyon_Plant_Lentils": (3, 10, .7, 0),
    "DankPyon_Plant_Garlic": (4, 6, .2, 0),
    "DankPyon_Plant_Flax": (7, 18, .6, 0),
    "DankPyon_Plant_Sugarcane": (5.8, 11, .7, 0),
    "DankPyon_Plant_Carrots": (3.25, 6, .6, 0),
    "DankPyon_Plant_Tomatoes": (4.25, 8, 1., .3),
    "DankPyon_Plant_Cabbages": (8, 10, .9, 0),
    "DankPyon_Plant_Pumpkins": (8, 20, .9, 0),
}
# 1:1 processed edible grain proxies; processing and job labor are NOT included.
GRAINS = {"Awa": (6, 13, .5), "Hie": (6, 12, .5),
          "Kibi": (5, 11, .5), "Soba": (4, 8, .4),
          "Barley": (10, 22, .5), "Wheat": (12, 28, .7),
          "Rice": (5, 11, .7)}
MO_XML = "3219596926/1.6/Defs/ThingDefs_Plants/Plants_Cultivated_Farm.xml"
PATH_PATTERN = re.compile(r'^/Defs/ThingDef\[defName="([^"]+)"\]/plant/growDays$')


def growth_patches(path: Path) -> dict[str, float]:
    root = ET.parse(path).getroot()
    if root.tag != "Patch":
        raise ValueError("Not a Patch: " + str(path))
    result = {}
    for node in root.findall("Operation"):
        match = PATH_PATTERN.fullmatch(node.findtext("xpath", ""))
        if node.get("Class") != "PatchOperationReplace" or match is None:
            raise ValueError("Non-growDays patch in " + str(path))
        name = match.group(1)
        if name in result:
            raise ValueError("Duplicate crop: " + name)
        result[name] = float(node.findtext("value/growDays"))
    return result


def verify_mo_source(archive: Path) -> None:
    with ZipFile(archive) as z:
        root = ET.fromstring(z.read(MO_XML))
    nodes = {node.findtext("defName"): node for node in root.findall("ThingDef")}
    for name, (days, yield_count, sensitivity, remaining) in MO.items():
        node = nodes.get(name)
        if node is None:
            raise ValueError("MO 1.6 crop missing: " + name)
        observed = (float(node.findtext("plant/growDays")),
                    float(node.findtext("plant/harvestYield")),
                    float(node.findtext("plant/fertilitySensitivity")),
                    float(node.findtext("plant/harvestAfterGrowth", "0")))
        if any(abs(a - b) > 0.0001 for a, b in zip(observed,
                                                   (days, yield_count, sensitivity, remaining))):
            raise ValueError("MO 1.6 crop source drift: " + name + " " + str(observed))


def harvest_events(grow_days: float, growth_hours: float,
                   window_days: float) -> int:
    """Hypothetical full-maturity cycles, with instantaneous re-sowing.

    harvestAfterGrowth in XML is not proof of after-harvest retained growth.
    No tomato-specific repeat interval is inferred without a native harvest.
    No season, sow/harvest job time, research or trellis constraints are included.
    """
    if grow_days <= 0 or not (0 < growth_hours <= 24) or window_days < 0:
        raise ValueError("Invalid growth model inputs")
    first = grow_days * 24 / growth_hours
    return int((window_days + 1e-9) // first)


def audit(hours: float = 13., mo_zip: Path | None = None) -> dict:
    if not (0 < hours <= 24):
        raise ValueError("hours must be in (0, 24]")
    v = growth_patches(ROOT / "Patches/CropGrowthBalance.xml")
    m = growth_patches(ROOT / "Compatibility/MedievalOverhaul/Patches/MOCropGrowthBalance.xml")
    if set(v) != set(BASE) or set(m) != set(MO) - {"DankPyon_Plant_Cabbages", "DankPyon_Plant_Pumpkins"}:
        raise ValueError("Patch scope drift: target crop identities changed")
    if mo_zip is not None:
        verify_mo_source(mo_zip)
    rows = []
    for provider, names in (("Vanilla", BASE), ("MO", MO)):
        for name, entry in names.items():
            original, yield_count, sensitivity = entry[:3]
            after = entry[3] if len(entry) == 4 else 0
            effective = (v if provider == "Vanilla" else m).get(name, original)
            first_days = effective * 24 / hours
            rows.append({"provider": provider, "defName": name,
                         "originalGrowDays": original, "effectiveGrowDays": effective,
                         "baseHarvestYield": yield_count,
                         "fertilitySensitivity": sensitivity,
                         "harvestAfterGrowth": after,
                         "postHarvestPersistenceNativeVerified": False,
                         "rawItemsPerGrowthDay": round(yield_count / effective, 4),
                         "firstHarvestIdealCalendarDays": round(first_days, 4),
                         "idealFullHarvestsIn10Days": harvest_events(effective, hours, 10),
                         "idealFullHarvestsIn20Days": harvest_events(effective, hours, 20),
                         "fertilityFactors": {str(f): round(1 + sensitivity * (f - 1), 4)
                                             for f in (.5, .7, 1., 1.4)},
                         "loadedNutritionVerified": False})
    return {"scope": "1.6 XML + offline idealized growth model, not native runtime or edible yield",
            "modeledGrowthHoursPerCalendarDay": hours,
            "moOriginalArchiveVerified": mo_zip is not None,
            "fertility50Sowing": "Fertility growth factor does not prove native sow eligibility; check PlantDef and WorkGiver.",
            "repeatHarvestModel": "Instant re-sowing full-maturity model; retained tomato growth UNVERIFIED.",
            "cropRows": rows,
            "grainsReference": {name: {"growDays": a, "baseYield": b, "minimumFertility": c}
                                for name, (a, b, c) in GRAINS.items()}}


def main() -> None:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--mo-zip", type=Path, help="Original MO 1.6 archive; verify canonical Def source")
    parser.add_argument("--growth-hours", type=float, default=13.,
                        help="Hypothetical effective growth hours/day, default ideal 13")
    args = parser.parse_args()
    print(json.dumps(audit(args.growth_hours, args.mo_zip), indent=2, ensure_ascii=False))


if __name__ == "__main__":
    main()
