using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Commands.Base;
using TomeOfDarkness.Utilities;
using HlEX = TomeOfDarkness.Utilities.HelpersExtension;
using System;



namespace TomeOfDarkness.NewContent.NinjaTricks
{
    internal class VanishingTrick
    {
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
                bp.SetName(ToDContext, "Vanishing Trick");
                bp.SetDescription(ToDContext, "As a swift action, the character can disappear for 1 round per level. This ability functions as Invisibility. Using this ability uses up 1 ki point.");
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

            vanishing_trick_feature.IsClassFeature = true;
            vanishing_trick_feature.Ranks = 1;

            vanishing_trick_feature.AddComponent(HlEX.CreatePrerequisiteNoFeature(vanishing_trick_feature));

            n_trick_invisible_blade.Get().AddComponent(HlEX.CreatePrerequisiteNoFeature(n_trick_invisible_blade.Get()));

            ToDContext.Logger.LogPatch("Created Vanishing Trick ninja trick.", vanishing_trick_feature);


        }
    }
}
