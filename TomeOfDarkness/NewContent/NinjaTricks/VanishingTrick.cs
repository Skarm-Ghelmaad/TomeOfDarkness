using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System.Diagnostics.Tracing;
using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;
using Kingmaker.UnitLogic.Mechanics.Components;
using TomeOfDarkness.NewComponents;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Abilities;
using TomeOfDarkness.Utilities;
using Kingmaker.UnitLogic.Abilities.Components;
using HlEX = TomeOfDarkness.Utilities.HelpersExtension;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.RuleSystem;
using static RootMotion.FinalIK.RagdollUtility;
using System;
using static RootMotion.FinalIK.InteractionTrigger;


namespace TomeOfDarkness.NewContent.NinjaTricks
{
    internal class VanishingTrick
    {
        private static readonly string VanishingTrickFeatureName = "NinjaTrickVanishingTrickFeature.Name";
        private static readonly string VanishingTrickFeatureDescription = "NinjaTrickVanishingTrickFeature.Description";

        public static void ConfigureVanishingTrick()
        {

            var kiResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82");

            var Invisibility_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("525f980cb29bc2240b93e953974cb325");
            var Improved_Invisibility_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("e6b35473a237a6045969253beb09777c");

            var n_trick_invisible_blade = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(ToDContext, "NinjaTrickInvisibleBladeFeature");

            var VanishingTrickIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_VanishingTrick.png");

            var Apply_Invisibility_Buff = HlEX.CreateContextActionApplyBuff(Invisibility_Buff.ToReference<BlueprintBuffReference>(),
                                                                            HlEX.CreateContextDuration(HlEX.CreateContextValue(AbilityRankType.Default)),
                                                                            false, false, false, false, false);



            var Apply_Improved_Invisibility_Buff = HlEX.CreateContextActionApplyBuff(Improved_Invisibility_Buff.ToReference<BlueprintBuffReference>(),
                                                                            HlEX.CreateContextDuration(HlEX.CreateContextValue(AbilityRankType.Default)),
                                                                            false, false, false, false, false);


            var upgrade_action = HlEX.CreateConditional(HlEX.CreateContextConditionHasFact(n_trick_invisible_blade), Apply_Improved_Invisibility_Buff, Apply_Invisibility_Buff);



            var VanishingTrickAbility = Helpers.CreateBlueprint<BlueprintAbility>(ToDContext, "NinjaTrickVanishingTrickAbility", bp => {
                bp.SetName(ToDContext, VanishingTrickFeatureName);
                bp.SetDescription(ToDContext, VanishingTrickFeatureDescription);
                bp.m_Icon = VanishingTrickIcon;
                bp.ResourceAssetIds = Array.Empty<string>();
                bp.Type = AbilityType.Supernatural;
                bp.ActionType = UnitCommand.CommandType.Swift;
                bp.Range = AbilityRange.Personal;
                bp.LocalizedDuration = Helpers.CreateString(ToDContext, "NinjaTrickVanishingTrickAbility.Duration", "1 round/level");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AddComponent(HlEX.CreateRunActions(upgrade_action));
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.Default;
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Class = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };
                    c.m_Progression = ContextRankProgression.AsIs;
                });
                bp.AddComponent(kiResource.CreateResourceLogic());
            });

            VanishingTrickAbility.SetMiscAbilityParametersSelfOnly();

            var vanishing_trick_feature = HlEX.ConvertAbilityToFeature(VanishingTrickAbility, "", "", "Feature", "Ability", false);

            n_trick_invisible_blade.Get().AddPrerequisiteFeature(vanishing_trick_feature);

            ToDContext.Logger.LogPatch("Created Vanishing Trick ninja trick.", vanishing_trick_feature);


        }
    }
}
