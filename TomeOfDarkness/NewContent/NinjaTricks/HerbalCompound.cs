using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Commands.Base;
using TomeOfDarkness.Utilities;
using HlEX = TomeOfDarkness.Utilities.HelpersExtension;
using System;
using Kingmaker.UnitLogic.Abilities.Components.Base;

namespace TomeOfDarkness.NewContent.NinjaTricks
{
    internal class HerbalCompound
    {

        public static void ConfigureHerbalCompound()
        {
            var RogueArray = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };

            var kiResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82");

            var Owls_Wisdom = BlueprintTools.GetBlueprint<BlueprintAbility>("f0455c9295b53904f9e02fc571dd2ce1");

            var Owls_Wisdom_Fx_Asset_ID = Owls_Wisdom.GetComponent<AbilitySpawnFx>().PrefabLink.AssetId;

            var HerbalCompoundIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_HerbalCompound.png");


            var Herb_Comp_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "HerbalCompoundBuff", bp => {
                bp.SetName(ToDContext, "Herbal Compound");
                bp.SetDescription(ToDContext, "A character with this trick can consume specially prepared herbs to strengthen her mind. The character can spend 1 ki point and smoke, eat, or inject an herbal compound as a move action. The character takes a –2 penalty to AC and on Reflex saves but gains a +4 alchemical bonus on Will saves for 10 minutes per relevant class level.");
                bp.m_Icon = HerbalCompoundIcon;
                bp.FxOnStart = new PrefabLink();
                bp.FxOnRemove = new PrefabLink();
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Stat = StatType.AC;
                    c.Descriptor = ModifierDescriptor.Penalty;
                    c.Value = -2;
                }));
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Stat = StatType.SaveReflex;
                    c.Descriptor = ModifierDescriptor.Penalty;
                    c.Value = -2;
                }));
                bp.AddComponent(Helpers.Create<AddStatBonus>(c => {
                    c.Stat = StatType.SaveWill;
                    c.Descriptor = ModifierDescriptor.Alchemical;
                    c.Value = 4;
                }));

            });


            var Apply_Herb_Comp_Buff = HlEX.CreateContextActionApplyBuff(Herb_Comp_Buff.ToReference<BlueprintBuffReference>(),
                                                                            HlEX.CreateContextDuration(HlEX.CreateContextValue(AbilityRankType.Default), DurationRate.TenMinutes),
                                                                            false, false, false, false, false);

            var HerbalCompoundAbility = Helpers.CreateBlueprint<BlueprintAbility>(ToDContext, "NinjaTrickHerbalCompoundAbility", bp => {
                bp.SetName(ToDContext, "Herbal Compound");
                bp.SetDescription(ToDContext, "A character with this trick can consume specially prepared herbs to strengthen her mind. The character can spend 1 ki point and smoke, eat, or inject an herbal compound as a move action. The character takes a –2 penalty to AC and on Reflex saves but gains a +4 alchemical bonus on Will saves for 10 minutes per relevant class level.");
                bp.m_Icon = HerbalCompoundIcon;
                bp.ResourceAssetIds = Array.Empty<string>();
                bp.Type = AbilityType.Extraordinary;
                bp.ActionType = UnitCommand.CommandType.Move;
                bp.Range = AbilityRange.Personal;
                bp.LocalizedDuration = Helpers.CreateString(ToDContext, "NinjaTrickHerbalCompoundAbility.Duration", "10 minutes/level");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AddComponent(HlEX.CreateRunActions(Apply_Herb_Comp_Buff));
                bp.AddComponent(HlEX.CreateAbilitySpawnFx(Owls_Wisdom_Fx_Asset_ID, anchor: AbilitySpawnFxAnchor.SelectedTarget));
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.Default;
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Class = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };
                    c.m_Progression = ContextRankProgression.AsIs;
                });
                bp.AddComponent(kiResource.CreateResourceLogic());
            });

            HerbalCompoundAbility.SetMiscAbilityParametersSelfOnly();

            var herbal_compound_feature = HlEX.ConvertAbilityToFeature(HerbalCompoundAbility, "", "", "Feature", "Ability", false);

            herbal_compound_feature.IsClassFeature = true;
            herbal_compound_feature.Ranks = 1;

            ToDContext.Logger.LogPatch("Created Herbal Compound ninja trick.", herbal_compound_feature);

        }

    }
}
