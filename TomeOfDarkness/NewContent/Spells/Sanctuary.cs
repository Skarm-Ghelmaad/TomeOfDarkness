using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using HlEX = TomeOfDarkness.Utilities.HelpersExtension;
using TabletopTweaks.Core.Utilities;
using Kingmaker.Blueprints.Items.Equipment;
using TomeOfDarkness.NewComponents;
using Kingmaker.EntitySystem.Stats;
using static TomeOfDarkness.Main;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using TomeOfDarkness.NewEnums;


namespace TomeOfDarkness.NewContent.Spells
{
    internal class Sanctuary
    {
        public static void ConfigureSanctuary()
        {
            // Sanctuary uses Lesser Restoration spell and scroll icon.
            var SanctuaryIcon = BlueprintTools.GetBlueprint<BlueprintAbility>("e84fc922ccf952943b5240293669b171").m_Icon; 
            var SanctuaryScrollIcon = BlueprintTools.GetBlueprint<BlueprintItemEquipmentUsable>("0d9349f009aeaed429ca5c59382e8212").m_Icon;

            // Sanctuary uses the Invisibility buff as base.
            var Invisibility_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("525f980cb29bc2240b93e953974cb325");

            var protection_domain_progression = BlueprintTools.GetBlueprint<BlueprintProgression>("b750650400d9d554b880dbf4c8347b24");

            var SactuaryBuffLogic = Helpers.Create<AddInoffensiveness>(c =>{
                c.Type = InoffensivenessEvaluationType.SavingThrow;
                c.m_SavingThrowType = SavingThrowType.Will;
                c.ReverseCheck = false;
                c.HasMarkingBuff = false;
                c.Offensive_Action_Effect = OffensiveActionEffect.REMOVE_FROM_OWNER;
            });

            var Sanctuary_Buff = Invisibility_Buff.CreateCopy(ToDContext, "SanctuaryBuff", bp => {
                bp.SetName(ToDContext, "Sanctuary");
                bp.m_Icon = SanctuaryIcon;
            });

            Sanctuary_Buff.ComponentsArray = new BlueprintComponent[] { SactuaryBuffLogic };

            var Apply_Sanctuary_Buff = HlEX.CreateContextActionApplyBuff(Sanctuary_Buff, HlEX.CreateContextDuration(HlEX.CreateContextValue(AbilityRankType.Default), DurationRate.Rounds), true);

            var Sanctuary_Spell = Helpers.CreateBlueprint<BlueprintAbility>(ToDContext, "SanctuaryAbility", bp => {
                bp.SetName(ToDContext, "Sanctuary");
                bp.SetDescription(ToDContext, "Any opponent attempting to directly attack the warded creature, even with a targeted spell, must attempt a Will save. If the save succeeds, the opponent can attack normally and is unaffected by that casting of the spell. If the save fails, the opponent can’t follow through with the attack, that part of its action is lost, and it can’t directly attack the warded creature for the duration of the spell. Those not attempting to attack the subject remain unaffected. This spell does not prevent the warded creature from being attacked or affected by area of effect spells. The subject cannot attack without breaking the spell but may use non-attack spells or otherwise act.");
                bp.m_Icon = SanctuaryIcon;
                bp.SetLocalizedDuration(ToDContext, "1 round/level");
                bp.SetLocalizedSavingThrow(ToDContext, "Will negates");
                bp.Type = AbilityType.Spell;
                bp.Range = AbilityRange.Touch;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
                bp.CanTargetSelf = true;
                bp.CanTargetPoint = false;
                bp.CanTargetFriends = true;
                bp.CanTargetEnemies = false;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Touch;
                bp.AvailableMetamagic = Metamagic.Extend | Metamagic.Heighten | Metamagic.Quicken | Metamagic.CompletelyNormal | Metamagic.Persistent | Metamagic.Reach;
                bp.ResourceAssetIds = new string[0];
                bp.AddComponent(HlEX.CreateSpellComponent(SpellSchool.Abjuration));
                bp.AddComponent(HlEX.CreateRunActions(Apply_Sanctuary_Buff));
                bp.AddComponent(Helpers.Create<CraftInfoComponent>(c => { c.OwnerBlueprint = bp; c.SpellType = CraftSpellType.Buff; c.SavingThrow = CraftSavingThrow.Will; c.AOEType = CraftAOE.None; }));
                bp.SpellResistance = false;
            });

            var SanctuaryScroll = ItemTools.CreateScroll(ToDContext, "ScrollOfSanctuary", SanctuaryScrollIcon, Sanctuary_Spell, 1, 1);
            var SanctuaryPotion = ItemTools.CreatePotion(ToDContext, "PotionOfSanctuary", ItemTools.PotionColor.Cyan, Sanctuary_Spell, 1, 1);

            if (ToDContext.NewContent.Spells.IsDisabled("Sanctuary")) { return; }

            VenderTools.AddScrollToLeveledVenders(SanctuaryScroll);
            VenderTools.AddPotionToLeveledVenders(SanctuaryPotion);

            Sanctuary_Spell.AddToSpellList(SpellTools.SpellList.ClericSpellList, 1);
            Sanctuary_Spell.AddToSpellList(SpellTools.SpellList.InquisitorSpellList, 1);
            Sanctuary_Spell.AddToSpellList(SpellTools.SpellList.WarpriestSpelllist, 1);

            HlEX.ReplaceDomainSpell(protection_domain_progression, Sanctuary_Spell, 1);

            ToDContext.Logger.LogPatch("Created Sanctuary spell.", Sanctuary_Spell);
            ToDContext.Logger.LogPatch("Created Sanctuary scroll and added to vendors.", SanctuaryScroll);
            ToDContext.Logger.LogPatch("Created Sanctuary potion and added to vendors.", SanctuaryPotion);
            ToDContext.Logger.LogPatch("Added Sanctuary spell to Protection Domain.", protection_domain_progression);


        }

    }
}
