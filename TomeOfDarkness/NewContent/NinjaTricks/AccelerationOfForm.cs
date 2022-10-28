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
using System.Drawing;
using Kingmaker.UnitLogic.Buffs;
using System;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System.Linq;

namespace TomeOfDarkness.NewContent.NinjaTricks
{
    internal class AccelerationOfForm
    {
        private static readonly string AccelerationOfFormFeatureName = "NinjaTrickAccelerationOfFormFeature.Name";
        private static readonly string AccelerationOfFormFeatureDescription = "NinjaTrickAccelerationOfFormFeature.Description";

        public static void ConfigureAccelerationOfForm()
        {
            var RogueArray = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };

            var kiResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82");

            var Displacement_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("00402bae4442a854081264e498e7a833");

            var Haste_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("03464790f40c3c24aa684b57155f3280");

            var Apply_Displacement_Buff = HlEX.CreateContextActionApplyBuff(Displacement_Buff.ToReference<BlueprintBuffReference>(),
                                                                            HlEX.CreateContextDuration(HlEX.CreateContextValue(AbilityRankType.Default), DurationRate.Rounds),
                                                                            false, false, false, false, false);

            var Apply_Haste_Buff = HlEX.CreateContextActionApplyBuff(Haste_Buff.ToReference<BlueprintBuffReference>(),
                                                                     HlEX.CreateContextDuration(HlEX.CreateContextValue(AbilityRankType.Default), DurationRate.Rounds),
                                                                     false, false, false, false, false);


            var AccelerationOfFormIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_AccelerationOfForm.png");


            var AccelerationOfFormAbility = Helpers.CreateBlueprint<BlueprintAbility>(ToDContext, "NinjaTrickAccelerationOfFormAbility", bp => {
                bp.SetName(ToDContext, AccelerationOfFormFeatureName);
                bp.SetDescription(ToDContext, AccelerationOfFormFeatureDescription);
                bp.m_Icon = AccelerationOfFormIcon;
                bp.ResourceAssetIds = Array.Empty<string>();
                bp.Type = AbilityType.Supernatural;
                bp.ActionType = UnitCommand.CommandType.Standard;
                bp.Range = AbilityRange.Personal;
                bp.LocalizedDuration = Helpers.CreateString(ToDContext, "NinjaTrickAccelerationOfFormAbility.Duration", "1 round/ 2 levels");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AddComponent(HlEX.CreateRunActions(Apply_Displacement_Buff, Apply_Haste_Buff));
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.Default;
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Class = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };
                    c.m_Progression = ContextRankProgression.Div2;
                });
                bp.AddComponent(kiResource.CreateResourceLogic());
            });

            AccelerationOfFormAbility.SetMiscAbilityParametersSelfOnly();

            var acceleration_of_form_feature = HlEX.ConvertAbilityToFeature(AccelerationOfFormAbility, "", "", "Feature", "Ability", false);

            ToDContext.Logger.LogPatch("Created Accelleration of Form ninja trick.", acceleration_of_form_feature);


        }
    }
}
