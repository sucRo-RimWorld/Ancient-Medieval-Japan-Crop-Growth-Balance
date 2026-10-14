"""Strict XML-only acceptance; native RimWorld compatibility is a separate gate."""
import unittest
import xml.etree.ElementTree as ET
from pathlib import Path
from zipfile import ZipFile

ROOT = Path(__file__).resolve().parents[1]
BASE = {"Plant_Potato": 6, "Plant_Corn": 13, "Plant_Strawberry": 6,
        "Plant_Cotton": 10, "Plant_Hops": 7, "Plant_Smokeleaf": 9}
MO = {"DankPyon_Plant_Onions": 8, "DankPyon_Plant_Lentils": 8,
      "DankPyon_Plant_Garlic": 10, "DankPyon_Plant_Flax": 9,
      "DankPyon_Plant_Sugarcane": 12, "DankPyon_Plant_Carrots": 6,
      "DankPyon_Plant_Tomatoes": 7}


def validate_patch(path, expected):
    xml = ET.parse(ROOT / path).getroot()
    assert xml.tag == 'Patch'
    ops = list(xml)
    assert len(ops) == len(expected), (path, len(ops))
    actual = {}
    for op in ops:
        assert op.tag == 'Operation' and op.attrib == {'Class': 'PatchOperationReplace'}
        assert [child.tag for child in op] == ['xpath','value']
        path = op.findtext('xpath')
        assert path.startswith('/Defs/ThingDef[defName="') and path.endswith('"]/plant/growDays')
        content = op.find('value')
        assert len(content) == 1 and content[0].tag == 'growDays'
        name = path[len('/Defs/ThingDef[defName="'):-len('"]/plant/growDays')]
        assert name not in actual, name
        actual[name] = int(content[0].text)
    assert actual == expected, (actual, expected)


class CropGrowthBalanceTests(unittest.TestCase):
    def test_vanilla_values(self):
        validate_patch('Patches/CropGrowthBalance.xml', BASE)

    def test_optional_mo_values(self):
        validate_patch('Compatibility/MedievalOverhaul/Patches/MOCropGrowthBalance.xml', MO)

    def test_no_conditional_loadfolder_shadow(self):
        # RimWorld's loadFolders overlay replaces root XML with the same
        # relative path; the 2026-10-14 real MO run lost all six Base patches.
        base_root = ROOT / "Patches"
        mo_root = ROOT / "Compatibility/MedievalOverhaul/Patches"
        base_paths = {f.relative_to(base_root) for f in base_root.rglob("*.xml")}
        mo_paths = {f.relative_to(mo_root) for f in mo_root.rglob("*.xml")}
        self.assertFalse(base_paths & mo_paths,
                         "Conditional MO folder shadows shared patch XML")
        self.assertIn(Path("CropGrowthBalance.xml"), base_paths)
        self.assertIn(Path("MOCropGrowthBalance.xml"), mo_paths)

    def test_native_field_sources_are_isolated_development_only(self):
        feature = (ROOT / "Tests/E2E/crop-growth-field-16.feature").read_text()
        step = (ROOT / "Tests/E2E/CropGrowthFieldSteps.cs").read_text()
        quick = (ROOT / "Tests/E2E/CropGrowthFieldQuickstart.cs").read_text()
        self.assertIn("@quickstart:CropGrowthFieldQuickstart", feature)
        self.assertEqual(feature.count("Scenario:"), 1)
        self.assertIn("WorkGiver_GrowerSow", step)
        self.assertIn("JobDefOf.Sow", step)
        self.assertIn("WorkGiver_PlantsCut", step)
        self.assertIn("JobDefOf.HarvestDesignated", step)
        self.assertIn("planted.TickLong()", step)
        self.assertIn("j == 300", step)
        self.assertIn("j == 600", step)
        self.assertIn("simulatedGrowthOnly=true", step)
        self.assertIn("ProbeNativeTerrainSowing", step)
        self.assertIn("AMJ_CROP_SOIL_SOW", step)
        self.assertIn("ProbeControlledTomatoHarvest", step)
        self.assertIn("AMJ_CROP_TOMATO_HARVEST", step)
        self.assertIn("candidate.GetStatValue(StatDefOf.PlantHarvestYield) >= 1f", step)
        self.assertIn("workerHarvestStat=", step)
        self.assertIn("repeatedHarvest=NOT_RUN", step)
        self.assertIn("realTrellis=false", step)
        self.assertIn("class CropGrowthFieldQuickstart : AbstractQuickstart", quick)
        self.assertNotIn("CropGrowthFieldSteps", (ROOT / "loadFolders.xml").read_text())
        self.assertNotIn("CropGrowthFieldQuickstart", (ROOT / "loadFolders.xml").read_text())

    def test_conditional_loader_and_unique_packageid(self):
        loader = ET.parse(ROOT / 'loadFolders.xml').getroot()
        self.assertEqual(loader.tag,'loadFolders')
        self.assertEqual([(x.text,tuple(x.attrib.items())) for x in loader.findall('v1.6/li')],
                         [('/',()),('Compatibility/MedievalOverhaul',(('IfModActive','DankPyon.Medieval.Overhaul'),))])
        self.assertEqual(len(loader),1,'Only 1.6 packaging is accepted')
        meta = ET.parse(ROOT / 'About/About.xml').getroot()
        self.assertEqual(meta.findtext('packageId'),'sucro.ancientmedievaljapan.cropgrowthbalance')
        self.assertEqual([x.text for x in meta.findall('supportedVersions/li')],['1.6'])
        self.assertFalse(meta.findall('modDependencies/li'))

    def test_no_accidental_growth_scope(self):
        for f in ROOT.rglob('*.xml'):
            ET.parse(f)
        for scope in (ROOT / 'Patches',ROOT / 'Compatibility/MedievalOverhaul/Patches'):
            for f in scope.rglob('*.xml'):
                s=f.read_text()
                for word in ('harvestYield','fertilityMin','fertilitySensitivity','Temperature','Plant_Rice','DankPyon_Plant_Wheat','AMJC_Plant_'):
                    self.assertNotIn(word,s,(f,word))

    def test_1_6_mo_original_def_ids(self):
        # Optional real-archive regression if the source fixture is separately provided.
        import os
        mo=os.environ.get('AMJ_MO_16_ZIP')
        if not mo: self.skipTest('Optional real MO zip not set')
        with ZipFile(mo) as z:
            xml=ET.fromstring(z.read('3219596926/1.6/Defs/ThingDefs_Plants/Plants_Cultivated_Farm.xml'))
        old={x.findtext('defName'):float(x.findtext('plant/growDays')) for x in xml.findall('ThingDef') if x.findtext('defName') and x.findtext('plant/growDays')}
        reference={'DankPyon_Plant_Onions':4,'DankPyon_Plant_Lentils':3,'DankPyon_Plant_Garlic':4,'DankPyon_Plant_Flax':7,'DankPyon_Plant_Sugarcane':5.8,'DankPyon_Plant_Carrots':3.25,'DankPyon_Plant_Tomatoes':4.25}
        self.assertEqual({k:old.get(k) for k in MO},reference)

if __name__ == '__main__': unittest.main()
