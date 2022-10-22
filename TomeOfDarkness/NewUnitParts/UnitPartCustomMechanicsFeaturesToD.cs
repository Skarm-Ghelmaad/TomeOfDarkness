using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using TabletopTweaks.Core.NewUnitParts;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TomeOfDarkness.NewUnitParts
{
    public class UnitPartCustomMechanicsFeaturesToD
    {
        //I have decided to start with a number above 12.000 (while Vek17 suggests > 1.000).

        public const CustomMechanicsFeature UseWeaponAsLightWeapon = (CustomMechanicsFeature)12_000;
    }
}
