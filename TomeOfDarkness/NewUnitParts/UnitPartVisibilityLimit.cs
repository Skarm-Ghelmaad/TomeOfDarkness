using JetBrains.Annotations;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomeOfDarkness.NewComponents;
using HarmonyLib;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs.Components;
using Newtonsoft.Json;

namespace TomeOfDarkness.NewUnitParts
{
    // Holic75_PT
    public class UnitPartVisibilityLimit : AdditiveUnitPart
    {
        public override CountableFlag UnitPartFlag
        {
            get
            {
                return VisibilityLimit;
            }
        }

        public bool Active()
        {
            bool UnitPartIsActive = false;            
            if (this.UnitPartFlag.Value)
            {
                if (this.m_VisibilityLimits.Any())
                {
                    UnitPartIsActive = true;
                }
            }

            return UnitPartIsActive;    
        }


        public void AddVisibilityLimit(UnitPartVisibilityLimit.VisibilityLimitEntry entry)
        {
            if (this.m_VisibilityLimits == null)
            {
                this.m_VisibilityLimits = new List<UnitPartVisibilityLimit.VisibilityLimitEntry>();
            }
            this.m_VisibilityLimits.Add(entry);
        }

        public void RemoveVisibilityLimit(UnitPartVisibilityLimit.VisibilityLimitEntry entry)
        {
            if (this.m_VisibilityLimits == null)
            {
                return;
            }
            foreach (UnitPartVisibilityLimit.VisibilityLimitEntry vl_entry in this.m_VisibilityLimits)
            {
                if (vl_entry.VisibilityCap == entry.VisibilityCap)
                {
                    this.m_VisibilityLimits.Remove(vl_entry);
                    if (this.m_VisibilityLimits.Count <= 0)
                    {
                        this.m_VisibilityLimits = null;
                    }
                    break;
                }
            }
        }

        public float GetMaxDistance()
        {
            float MaxDistance = 1000;

            foreach (UnitPartVisibilityLimit.VisibilityLimitEntry vl_e in this.m_VisibilityLimits)
            {
                MaxDistance = Math.Min(MaxDistance, vl_e.VisibilityCap.Meters);
            }

            return MaxDistance;
        }


        public CountableFlag VisibilityLimit = new CountableFlag();

        [CanBeNull]
        private List<UnitPartVisibilityLimit.VisibilityLimitEntry> m_VisibilityLimits;

        public class VisibilityLimitEntry
        {
            public Feet VisibilityCap = 0.Feet();
        }

        [HarmonyPatch(typeof(AbilityData), "GetVisualDistance")]
        static class AbilityData_GetVisualDistance_Patch
        {
            static void Postfix(AbilityData __instance, ref float __result)
            {
                AbilityData_GetApproachDistance_Patch.Postfix(__instance, null, ref __result);
            }
        }

        [HarmonyPatch(typeof(AbilityData), "GetApproachDistance")]
        static class AbilityData_GetApproachDistance_Patch
        {
            internal static void Postfix(AbilityData __instance, UnitEntityData target, ref float __result)
            {
                try
                {
                    var caster = __instance.Caster;
                    var part = caster.Get<UnitPartVisibilityLimit>();
                    if (part != null && part.Active())
                    {
                        __result = Math.Min(part.GetMaxDistance(), __result);
                    }
                }
                catch (Exception e)
                {
                    Main.ToDContext.Logger.LogError(e, e.Message);
                }
            }

        }

    }
}
