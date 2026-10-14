#!/usr/bin/env python3
"""Build an exact GitHub Actions subscriber candidate; never invoke Steam/YADA.

This is a nonpublishing preflight only. First-time public Workshop
creation and a protected SteamCMD/tag publisher remain separate steps.
"""
import argparse
import hashlib
import json
from pathlib import Path
import shutil
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
REQUIRED = (
    "About/About.xml",
    "Compatibility/MedievalOverhaul/Patches/MOCropGrowthBalance.xml",
    "LICENSE",
    "Patches/CropGrowthBalance.xml",
    "loadFolders.xml",
)
OPTIONAL = (
    "About/Changelog.txt",
    "About/Manifest.xml",
    "About/PublishedFileId.txt",
    "About/Preview.png",
    "About/preview.png",
    "About/WorkshopTags.xml",
)


def build(output: Path, require_release_metadata: bool = False) -> dict:
    output = output.resolve()
    expected_root = (ROOT / "TestResults").resolve()
    if output == expected_root or expected_root not in output.parents:
        raise ValueError("Candidate output must reside below the untracked TestResults directory")
    if output.exists():
        if output.is_symlink():
            raise ValueError("Refuse to follow candidate-directory symlink")
        shutil.rmtree(output)
    output.mkdir(parents=True)
    missing = [p for p in REQUIRED if not (ROOT / p).is_file()]
    if missing:
        raise ValueError("Missing required runtime/legal files: " + repr(missing))
    files = sorted(set(REQUIRED) | {p for p in OPTIONAL if (ROOT / p).is_file()})
    if "About/Preview.png" in files and "About/preview.png" in files:
        raise ValueError("Case-colliding Workshop preview filenames")
    if require_release_metadata:
        for name in ("About/Manifest.xml", "About/Changelog.txt", "About/Preview.png"):
            if name not in files and not (name == "About/Preview.png" and "About/preview.png" in files):
                raise ValueError("Release candidate missing " + name)
        about = ET.parse(ROOT / "About/About.xml").getroot()
        version = (about.findtext("modVersion") or "").strip()
        if not version or "-dev" in version:
            raise ValueError("Development version cannot be released")
        manifest = ET.parse(ROOT / "About/Manifest.xml").getroot()
        if manifest.findtext("version") != version:
            raise ValueError("Manifest/ About version mismatch")
    digests = {}
    for name in files:
        source = ROOT / name
        if source.is_symlink():
            raise ValueError("Symlink forbidden in runtime stage: " + name)
        if source.suffix.lower() == ".xml":
            ET.parse(source)
        dest = output / name
        dest.parent.mkdir(parents=True, exist_ok=True)
        shutil.copyfile(source, dest)
        if source.read_bytes() != dest.read_bytes():
            raise ValueError("Stage/source content mismatch: " + name)
        digests[name] = hashlib.sha256(dest.read_bytes()).hexdigest()
    real_files = sorted(p.relative_to(output).as_posix() for p in output.rglob("*") if p.is_file())
    if real_files != files:
        raise ValueError("Unexpected candidate files: " + repr(real_files))
    return {"mode": "preflight-only", "release_ready": require_release_metadata,
            "runtime_file_count": len(files), "sha256": digests}


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--output", default="TestResults/WorkshopCandidate")
    parser.add_argument("--require-release-metadata", action="store_true")
    args = parser.parse_args()
    details = build(Path(args.output), args.require_release_metadata)
    print(json.dumps(details, indent=2, sort_keys=True))


if __name__ == "__main__":
    main()
