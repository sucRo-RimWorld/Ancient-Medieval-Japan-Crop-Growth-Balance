"""Regression for growth-day source model; no full-game season/food claims."""
import unittest
from pathlib import Path
import sys

sys.path.insert(0, str(Path(__file__).resolve().parents[1] / "Tools"))
from crop_balance_audit import audit, harvest_events


class CropBalanceAuditTests(unittest.TestCase):
    def test_mo_and_vanilla_scope_and_examples(self):
        rows = {r['defName']: r for r in audit()['cropRows']}
        self.assertEqual(len(rows), 15)  # 13 patched + 2 untouched MO controls
        self.assertEqual(rows['DankPyon_Plant_Lentils']['effectiveGrowDays'], 8)
        self.assertEqual(rows['DankPyon_Plant_Lentils']['baseHarvestYield'], 10)
        self.assertEqual(rows['DankPyon_Plant_Pumpkins']['effectiveGrowDays'], 8)
        self.assertEqual(rows['DankPyon_Plant_Pumpkins']['baseHarvestYield'], 20)
        self.assertEqual(rows['Plant_Corn']['idealFullHarvestsIn20Days'], 0)
        self.assertEqual(rows['DankPyon_Plant_Sugarcane']['idealFullHarvestsIn20Days'], 0)
        self.assertEqual(rows['Plant_Potato']['idealFullHarvestsIn20Days'], 1)

    def test_no_unverified_regrowth_acceleration(self):
        self.assertEqual(harvest_events(7, 13, 20), 1)
        self.assertEqual(harvest_events(7, 13, 23), 1)
        self.assertEqual(harvest_events(7, 13, 30), 2)
        with self.assertRaises(TypeError):
            harvest_events(7, 13, 23, .3)
        tomato = next(r for r in audit()['cropRows']
                      if r['defName'] == 'DankPyon_Plant_Tomatoes')
        self.assertEqual(tomato['harvestAfterGrowth'], .3)
        self.assertFalse(tomato['postHarvestPersistenceNativeVerified'])
        self.assertEqual(tomato['idealFullHarvestsIn20Days'], 1)

    def test_mo_explicit_nutrition_is_not_inherited_nutrition(self):
        rows = {r["defName"]: r for r in audit()["cropRows"]}
        tomato = rows["DankPyon_Plant_Tomatoes"]
        self.assertIsNone(tomato["rawNutritionExplicitInMO16"])
        self.assertIsNone(tomato["grossRawNutritionPerGrowthDayIfExplicit"])
        self.assertFalse(tomato["loadedNutritionVerified"])
        self.assertEqual(rows["DankPyon_Plant_Lentils"]["rawNutritionExplicitInMO16"], .05)
        self.assertEqual(rows["DankPyon_Plant_Lentils"]["grossRawNutritionPerGrowthDayIfExplicit"], .0625)
        self.assertEqual(rows["DankPyon_Plant_Pumpkins"]["grossRawNutritionPerGrowthDayIfExplicit"], .125)

    def test_fertility_factors_are_not_sowability(self):
        rows = {r['defName']: r for r in audit()['cropRows']}
        self.assertEqual(rows['Plant_Potato']['fertilityFactors']['0.5'], .8)
        self.assertEqual(rows['DankPyon_Plant_Lentils']['fertilityFactors']['0.5'], .65)
        self.assertIn('does not prove native sow eligibility', audit()['fertility50Sowing'])

    def test_strict_input_bounds(self):
        for args in ((0, 13, 20), (6, 0, 20), (6, 13, -1)):
            with self.assertRaises(ValueError):
                harvest_events(*args)


if __name__ == "__main__":
    unittest.main()
