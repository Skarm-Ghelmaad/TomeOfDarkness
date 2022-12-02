using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TabletopTweaks.Core.NewUnitParts.CustomStatTypes;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;
using static TomeOfDarkness.NewUnitParts.CustomActivatableAbilityGroups;
using Kingmaker.UnitLogic.Parts;
using TabletopTweaks.Core.NewUnitParts;
using Kingmaker.EntitySystem.Stats;

namespace TomeOfDarkness.NewUnitParts
{

    public class UnitPartCustomActivatableAbilityGroup: OldStyleUnitPart
    {

        public const int CustomActivatableGroupStart = 30_200;
        public const int CustomActivatableGroupEnd = 42_200;

        [JsonProperty]
        [UsedImplicitly]
        private int[] PersistentGroupsSizeIncreases
        {
            get
            {
                return this.m_GroupsSizeIncreases;
            }
            set
            {
                this.m_GroupsSizeIncreases = (this.m_GroupsSizeIncreases ?? new int[(EnumUtils.GetMaxValue<CustomActivatableAbilityGroup>() - CustomActivatableGroupStart)]);
                for (int i = 0; i < Math.Min(this.m_GroupsSizeIncreases.Length, value.Length); i++)
                {
                    this.m_GroupsSizeIncreases[i] = value[i];
                }
            }
        }

        public void IncreaseGroupSize(ActivatableAbilityGroup group)
        {
            this.m_GroupsSizeIncreases[(int)(group - CustomActivatableGroupStart)]++;
        }

        public void DecreaseGroupSize(ActivatableAbilityGroup group)
        {
            this.m_GroupsSizeIncreases[(int)(group - CustomActivatableGroupStart)]--;
        }

        public int GetGroupSize(ActivatableAbilityGroup group)
        {
            return this.m_GroupsSizeIncreases[(int)(group - CustomActivatableGroupStart)] + 1;
        }


        [HarmonyPatch(typeof(UnitPartActivatableAbility))]
        static class UnitPartActivatableAbilityCustomGroupPatch
        {
            [HarmonyPatch("IncreaseGroupSize", MethodType.Normal)]
            [HarmonyPrefix]
            static bool IncreaseGroupSize_Prefix(UnitPartActivatableAbility __instance, ActivatableAbilityGroup group)
            {
                if (group.IsCustom())
                {
                    __instance.Owner.Ensure<UnitPartCustomActivatableAbilityGroup>().IncreaseGroupSize(group);
                    return false;
                }
                
                return true;
            }

        


            [HarmonyPatch("DecreaseGroupSize", MethodType.Normal)]
            [HarmonyPrefix]
            static bool DecreaseGroupSize_Prefix(UnitPartActivatableAbility __instance, ActivatableAbilityGroup group)
            {
                if (group.IsCustom())
                {
                    __instance.Owner.Ensure<UnitPartCustomActivatableAbilityGroup>().DecreaseGroupSize(group);
                    return false;
                }

                return true;
            }


            [HarmonyPatch("GetGroupSize", MethodType.Normal)]
            [HarmonyPrefix]
            static bool GetGroupSize_Prefix(UnitPartActivatableAbility __instance, ActivatableAbilityGroup group, ref int __result)
            {
                if (group.IsCustom())
                {
                    __result = __instance.Owner.Ensure<UnitPartCustomActivatableAbilityGroup>().GetGroupSize(group);
                    return false;
                }

                return true;
            }

        }

        private int[] m_GroupsSizeIncreases = new int[(EnumUtils.GetMaxValue<CustomActivatableAbilityGroup>() - CustomActivatableGroupStart)];

        public readonly Dictionary<CustomActivatableAbilityGroup, ActivatableAbilityGroup> ActivatableAbilityGroups = new Dictionary<CustomActivatableAbilityGroup, ActivatableAbilityGroup>();

    }

    public static class CustomActivatableAbilityGroups
    {
        //The last ActivatableAbilityGroup in Kingmaker.Blueprints.Classes is "ShifterAspect" [ 0x000000030 = 48 ]
        public enum CustomActivatableAbilityGroup : int
        {
            PressurePoints = 30_200
        }

        public static ActivatableAbilityGroup Group(this CustomActivatableAbilityGroup group)
        {
            return (ActivatableAbilityGroup)group;
        }
        public static CustomActivatableAbilityGroup CustomGroup(this ActivatableAbilityGroup group)
        {
            return (CustomActivatableAbilityGroup)group;
        }
        public static bool IsCustom(this ActivatableAbilityGroup group)
        {
            return (int)group >= UnitPartCustomActivatableAbilityGroup.CustomActivatableGroupStart && (int)group <= UnitPartCustomActivatableAbilityGroup.CustomActivatableGroupEnd;
        }


    }


}
