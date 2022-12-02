using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static TabletopTweaks.Core.NewUnitParts.CustomStatTypes;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;

namespace TomeOfDarkness.NewEnums
{
    internal class CustomStatTypesToD
    {
        // This class is used to add new custom stats that I need for my mod.

        public static class CustomStatTypes
        {
            //I have agreed with Vek17 to use only numbers over 11.000 for Stats and 1.001.000 for Attributes.

            public const CustomStatType Chi = (CustomStatType)1_001_201; // This attribute is used with Universal Ki mechanics.



        }

    }
}
