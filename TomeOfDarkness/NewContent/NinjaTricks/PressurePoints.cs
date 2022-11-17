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
using Kingmaker.Blueprints.Items.Ecnchantments;
using System.Linq;
using System;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.ElementsSystem;

namespace TomeOfDarkness.NewContent.NinjaTricks
{
    internal class PressurePoints
    {
        public static void ConfigurePressurePoints()
        {
            var PressurePointsIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_PressurePoints.png");
            var PressurePointsStrIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_PressurePointsStr.png");
            var PressurePointsDexIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_PressurePointsDex.png");
            var PressurePointsConIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_PressurePointsCon.png");
            var PressurePointsIntIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_PressurePointsInt.png");
            var PressurePointsWisIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_PressurePointsWis.png");
            var PressurePointsChaIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_PressurePointsCha.png");

            var Debilitating_Injury_Bewildered_Active_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("116ee72b2149f4d44a330296a7e42d13");
            var Debilitating_Injury_Bewildered_Activatable_Ability = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("c74a63e0d2fd08149bfcfff8ded43313");

            var Pressure_Points_Str_Action_Deal_Damage = HlEX.CreateContextActionDealDamage(StatType.Strength, HlEX.CreateContextDiceValue(DiceType.Zero, HlEX.CreateContextValue(0), HlEX.CreateContextValue(1)), false, false, false, false);
            var Pressure_Points_Dex_Action_Deal_Damage = HlEX.CreateContextActionDealDamage(StatType.Dexterity, HlEX.CreateContextDiceValue(DiceType.Zero, HlEX.CreateContextValue(0), HlEX.CreateContextValue(1)), false, false, false, false);
            var Pressure_Points_Con_Action_Deal_Damage = HlEX.CreateContextActionDealDamage(StatType.Constitution, HlEX.CreateContextDiceValue(DiceType.Zero, HlEX.CreateContextValue(0), HlEX.CreateContextValue(1)), false, false, false, false);
            var Pressure_Points_Int_Action_Deal_Damage = HlEX.CreateContextActionDealDamage(StatType.Intelligence, HlEX.CreateContextDiceValue(DiceType.Zero, HlEX.CreateContextValue(0), HlEX.CreateContextValue(1)), false, false, false, false);
            var Pressure_Points_Wis_Action_Deal_Damage = HlEX.CreateContextActionDealDamage(StatType.Wisdom, HlEX.CreateContextDiceValue(DiceType.Zero, HlEX.CreateContextValue(0), HlEX.CreateContextValue(1)), false, false, false, false);
            var Pressure_Points_Cha_Action_Deal_Damage = HlEX.CreateContextActionDealDamage(StatType.Charisma, HlEX.CreateContextDiceValue(DiceType.Zero, HlEX.CreateContextValue(0), HlEX.CreateContextValue(1)), false, false, false, false);

            var Pressure_Points_Str_Active_Buff = Debilitating_Injury_Bewildered_Active_Buff.CreateCopy(ToDContext, "NinjaTrickPressurePointsStrActiveBuff", bp => {
                bp.SetName(ToDContext, "Pressure Points (Strength)");
                bp.SetDescription(ToDContext, "Whenever a character with this trick deals sneak {g|Encyclopedia:Attack}attack{/g} {g|Encyclopedia:Damage}damage{/g} to a foe, he also deals 1 stat {g|Encyclopedia:Damage}damage{/g} to that foe's {g|Encyclopedia:Strength}Strength{/g}.");
                bp.m_Icon = PressurePointsStrIcon;
            });

            var Pressure_Points_Dex_Active_Buff = Debilitating_Injury_Bewildered_Active_Buff.CreateCopy(ToDContext, "NinjaTrickPressurePointsDexActiveBuff", bp => {
                bp.SetName(ToDContext, "Pressure Points (Dexterity)");
                bp.SetDescription(ToDContext, "Whenever a character with this trick deals sneak {g|Encyclopedia:Attack}attack{/g} {g|Encyclopedia:Damage}damage{/g} to a foe, he also deals 1 stat {g|Encyclopedia:Damage}damage{/g} to that foe's {g|Encyclopedia:Dexterity}Dexterity{/g}.");
                bp.m_Icon = PressurePointsDexIcon;
            });

            var Pressure_Points_Con_Active_Buff = Debilitating_Injury_Bewildered_Active_Buff.CreateCopy(ToDContext, "NinjaTrickPressurePointsConActiveBuff", bp => {
                bp.SetName(ToDContext, "Pressure Points (Constitution)");
                bp.SetDescription(ToDContext, "Whenever a character with this trick deals sneak {g|Encyclopedia:Attack}attack{/g} {g|Encyclopedia:Damage}damage{/g} to a foe, he also deals 1 stat {g|Encyclopedia:Damage}damage{/g} to that foe's {g|Encyclopedia:Constitution}Constitution{/g}.");
                bp.m_Icon = PressurePointsConIcon;
            });

            var Pressure_Points_Int_Active_Buff = Debilitating_Injury_Bewildered_Active_Buff.CreateCopy(ToDContext, "NinjaTrickPressurePointsIntActiveBuff", bp => {
                bp.SetName(ToDContext, "Pressure Points (Intelligence)");
                bp.SetDescription(ToDContext, "Whenever a character with this trick deals sneak {g|Encyclopedia:Attack}attack{/g} {g|Encyclopedia:Damage}damage{/g} to a foe, he also deals 1 stat {g|Encyclopedia:Damage}damage{/g} to that foe's {g|Encyclopedia:Intelligence}Intelligence{/g}.");
                bp.m_Icon = PressurePointsIntIcon;
            });

            var Pressure_Points_Wis_Active_Buff = Debilitating_Injury_Bewildered_Active_Buff.CreateCopy(ToDContext, "NinjaTrickPressurePointsWisActiveBuff", bp => {
                bp.SetName(ToDContext, "Pressure Points (Wisdom)");
                bp.SetDescription(ToDContext, "Whenever a character with this trick deals sneak {g|Encyclopedia:Attack}attack{/g} {g|Encyclopedia:Damage}damage{/g} to a foe, he also deals 1 stat {g|Encyclopedia:Damage}damage{/g} to that foe's {g|Encyclopedia:Wisdom}Wisdom{/g}.");
                bp.m_Icon = PressurePointsWisIcon;
            });

            var Pressure_Points_Cha_Active_Buff = Debilitating_Injury_Bewildered_Active_Buff.CreateCopy(ToDContext, "NinjaTrickPressurePointsChaActiveBuff", bp => {
                bp.SetName(ToDContext, "Pressure Points (Charisma)");
                bp.SetDescription(ToDContext, "Whenever a character with this trick deals sneak {g|Encyclopedia:Attack}attack{/g} {g|Encyclopedia:Damage}damage{/g} to a foe, he also deals 1 stat {g|Encyclopedia:Damage}damage{/g} to that foe's {g|Encyclopedia:Charisma}Charisma{/g}.");
                bp.m_Icon = PressurePointsChaIcon;
            });

            var Pressure_Points_Str_Effect_Buff = Debilitating_Injury_Bewildered_Active_Buff.CreateCopy(ToDContext, "NinjaTrickPressurePointsStrEffectBuff", bp => {
                bp.SetName(ToDContext, "Pressure Points (Strength)");
                bp.SetDescription(ToDContext, "Whenever a character with this trick deals sneak {g|Encyclopedia:Attack}attack{/g} {g|Encyclopedia:Damage}damage{/g} to a foe, he also deals 1 stat {g|Encyclopedia:Damage}damage{/g} to that foe's {g|Encyclopedia:Strength}Strength{/g}.");
                bp.m_Icon = PressurePointsStrIcon;
                bp.ReplaceComponents<AddInitiatorAttackRollTrigger>(HlEX.CreateAddInitiatorAttackRollTrigger(Helpers.CreateActionList(Pressure_Points_Str_Action_Deal_Damage, HlEX.CreateContextActionRemoveSelf()), true, false, true));
                bp.m_Flags = (BlueprintBuff.Flags)0;
            });

            var Pressure_Points_Dex_Effect_Buff = Debilitating_Injury_Bewildered_Active_Buff.CreateCopy(ToDContext, "NinjaTrickPressurePointsDexEffectBuff", bp => {
                bp.SetName(ToDContext, "Pressure Points (Dexterity)");
                bp.SetDescription(ToDContext, "Whenever a character with this trick deals sneak {g|Encyclopedia:Attack}attack{/g} {g|Encyclopedia:Damage}damage{/g} to a foe, he also deals 1 stat {g|Encyclopedia:Damage}damage{/g} to that foe's {g|Encyclopedia:Dexterity}Dexterity{/g}.");
                bp.m_Icon = PressurePointsDexIcon;
                bp.ReplaceComponents<AddInitiatorAttackRollTrigger>(HlEX.CreateAddInitiatorAttackRollTrigger(Helpers.CreateActionList(Pressure_Points_Dex_Action_Deal_Damage, HlEX.CreateContextActionRemoveSelf()), true, false, true));
                bp.m_Flags = (BlueprintBuff.Flags)0;
            });

            var Pressure_Points_Con_Effect_Buff = Debilitating_Injury_Bewildered_Active_Buff.CreateCopy(ToDContext, "NinjaTrickPressurePointsConEffectBuff", bp => {
                bp.SetName(ToDContext, "Pressure Points (Constitution)");
                bp.SetDescription(ToDContext, "Whenever a character with this trick deals sneak {g|Encyclopedia:Attack}attack{/g} {g|Encyclopedia:Damage}damage{/g} to a foe, he also deals 1 stat {g|Encyclopedia:Damage}damage{/g} to that foe's {g|Encyclopedia:Constitution}Constitution{/g}.");
                bp.m_Icon = PressurePointsConIcon;
                bp.ReplaceComponents<AddInitiatorAttackRollTrigger>(HlEX.CreateAddInitiatorAttackRollTrigger(Helpers.CreateActionList(Pressure_Points_Con_Action_Deal_Damage, HlEX.CreateContextActionRemoveSelf()), true, false, true));
                bp.m_Flags = (BlueprintBuff.Flags)0;
            });

            var Pressure_Points_Int_Effect_Buff = Debilitating_Injury_Bewildered_Active_Buff.CreateCopy(ToDContext, "NinjaTrickPressurePointsIntEffectBuff", bp => {
                bp.SetName(ToDContext, "Pressure Points (Intelligence)");
                bp.SetDescription(ToDContext, "Whenever a character with this trick deals sneak {g|Encyclopedia:Attack}attack{/g} {g|Encyclopedia:Damage}damage{/g} to a foe, he also deals 1 stat {g|Encyclopedia:Damage}damage{/g} to that foe's {g|Encyclopedia:Intelligence}Intelligence{/g}.");
                bp.m_Icon = PressurePointsIntIcon;
                bp.ReplaceComponents<AddInitiatorAttackRollTrigger>(HlEX.CreateAddInitiatorAttackRollTrigger(Helpers.CreateActionList(Pressure_Points_Int_Action_Deal_Damage, HlEX.CreateContextActionRemoveSelf()), true, false, true));
                bp.m_Flags = (BlueprintBuff.Flags)0;
            });

            var Pressure_Points_Wis_Effect_Buff = Debilitating_Injury_Bewildered_Active_Buff.CreateCopy(ToDContext, "NinjaTrickPressurePointsWisEffectBuff", bp => {
                bp.SetName(ToDContext, "Pressure Points (Wisdom)");
                bp.SetDescription(ToDContext, "Whenever a character with this trick deals sneak {g|Encyclopedia:Attack}attack{/g} {g|Encyclopedia:Damage}damage{/g} to a foe, he also deals 1 stat {g|Encyclopedia:Damage}damage{/g} to that foe's {g|Encyclopedia:Wisdom}Wisdom{/g}.");
                bp.m_Icon = PressurePointsWisIcon;
                bp.ReplaceComponents<AddInitiatorAttackRollTrigger>(HlEX.CreateAddInitiatorAttackRollTrigger(Helpers.CreateActionList(Pressure_Points_Wis_Action_Deal_Damage, HlEX.CreateContextActionRemoveSelf()), true, false, true));
                bp.m_Flags = (BlueprintBuff.Flags)0;
            });

            var Pressure_Points_Cha_Effect_Buff = Debilitating_Injury_Bewildered_Active_Buff.CreateCopy(ToDContext, "NinjaTrickPressurePointsChaEffectBuff", bp => {
                bp.SetName(ToDContext, "Pressure Points (Charisma)");
                bp.SetDescription(ToDContext, "Whenever a character with this trick deals sneak {g|Encyclopedia:Attack}attack{/g} {g|Encyclopedia:Damage}damage{/g} to a foe, he also deals 1 stat {g|Encyclopedia:Damage}damage{/g} to that foe's {g|Encyclopedia:Charisma}Charisma{/g}.");
                bp.m_Icon = PressurePointsChaIcon;
                bp.ReplaceComponents<AddInitiatorAttackRollTrigger>(HlEX.CreateAddInitiatorAttackRollTrigger(Helpers.CreateActionList(Pressure_Points_Cha_Action_Deal_Damage, HlEX.CreateContextActionRemoveSelf()), true, false, true));
                bp.m_Flags = (BlueprintBuff.Flags)0;
            });


            var Pressure_Points_Str_Active_Buff_Conditional_Action1 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Dex_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Dex_Effect_Buff), null);
            var Pressure_Points_Str_Active_Buff_Conditional_Action2 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Con_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Con_Effect_Buff), null);
            var Pressure_Points_Str_Active_Buff_Conditional_Action3 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Int_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Int_Effect_Buff), null);
            var Pressure_Points_Str_Active_Buff_Conditional_Action4 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Wis_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Wis_Effect_Buff), null);
            var Pressure_Points_Str_Active_Buff_Conditional_Action5 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Cha_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Cha_Effect_Buff), null);

            var Pressure_Points_Dex_Active_Buff_Conditional_Action1 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Str_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Str_Effect_Buff), null);
            var Pressure_Points_Dex_Active_Buff_Conditional_Action2 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Con_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Con_Effect_Buff), null);
            var Pressure_Points_Dex_Active_Buff_Conditional_Action3 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Int_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Int_Effect_Buff), null);
            var Pressure_Points_Dex_Active_Buff_Conditional_Action4 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Wis_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Wis_Effect_Buff), null);
            var Pressure_Points_Dex_Active_Buff_Conditional_Action5 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Cha_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Cha_Effect_Buff), null);

            var Pressure_Points_Con_Active_Buff_Conditional_Action1 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Str_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Str_Effect_Buff), null);
            var Pressure_Points_Con_Active_Buff_Conditional_Action2 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Dex_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Dex_Effect_Buff), null);
            var Pressure_Points_Con_Active_Buff_Conditional_Action3 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Int_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Int_Effect_Buff), null);
            var Pressure_Points_Con_Active_Buff_Conditional_Action4 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Wis_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Wis_Effect_Buff), null);
            var Pressure_Points_Con_Active_Buff_Conditional_Action5 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Cha_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Cha_Effect_Buff), null);

            var Pressure_Points_Int_Active_Buff_Conditional_Action1 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Str_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Str_Effect_Buff), null);
            var Pressure_Points_Int_Active_Buff_Conditional_Action2 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Dex_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Dex_Effect_Buff), null);
            var Pressure_Points_Int_Active_Buff_Conditional_Action3 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Con_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Con_Effect_Buff), null);
            var Pressure_Points_Int_Active_Buff_Conditional_Action4 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Wis_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Wis_Effect_Buff), null);
            var Pressure_Points_Int_Active_Buff_Conditional_Action5 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Cha_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Cha_Effect_Buff), null);

            var Pressure_Points_Wis_Active_Buff_Conditional_Action1 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Str_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Str_Effect_Buff), null);
            var Pressure_Points_Wis_Active_Buff_Conditional_Action2 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Dex_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Dex_Effect_Buff), null);
            var Pressure_Points_Wis_Active_Buff_Conditional_Action3 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Con_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Con_Effect_Buff), null);
            var Pressure_Points_Wis_Active_Buff_Conditional_Action4 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Int_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Int_Effect_Buff), null);
            var Pressure_Points_Wis_Active_Buff_Conditional_Action5 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Cha_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Cha_Effect_Buff), null);

            var Pressure_Points_Cha_Active_Buff_Conditional_Action1 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Str_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Str_Effect_Buff), null);
            var Pressure_Points_Cha_Active_Buff_Conditional_Action2 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Dex_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Dex_Effect_Buff), null);
            var Pressure_Points_Cha_Active_Buff_Conditional_Action3 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Con_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Con_Effect_Buff), null);
            var Pressure_Points_Cha_Active_Buff_Conditional_Action4 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Int_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Int_Effect_Buff), null);
            var Pressure_Points_Cha_Active_Buff_Conditional_Action5 = HlEX.CreateConditional( HlEX.CreateContextConditionCasterHasFact(Pressure_Points_Wis_Active_Buff, false), HlEX.CreateContextActionRemoveBuff(Pressure_Points_Wis_Effect_Buff), null);

            var Pressure_Points_Active_Apply_Buff_Duration = Debilitating_Injury_Bewildered_Active_Buff.GetComponent<AddInitiatorAttackRollTrigger>().Action.Actions.OfType<ContextActionApplyBuff>().FirstOrDefault().DurationValue;

            var Pressure_Points_Str_Active_Apply_Buff_Action = HlEX.CreateContextActionApplyBuff(Pressure_Points_Str_Effect_Buff.ToReference<BlueprintBuffReference>(), Pressure_Points_Active_Apply_Buff_Duration, false, false, true, false, false);
            var Pressure_Points_Dex_Active_Apply_Buff_Action = HlEX.CreateContextActionApplyBuff(Pressure_Points_Dex_Effect_Buff.ToReference<BlueprintBuffReference>(), Pressure_Points_Active_Apply_Buff_Duration, false, false, true, false, false);
            var Pressure_Points_Con_Active_Apply_Buff_Action = HlEX.CreateContextActionApplyBuff(Pressure_Points_Con_Effect_Buff.ToReference<BlueprintBuffReference>(), Pressure_Points_Active_Apply_Buff_Duration, false, false, true, false, false);
            var Pressure_Points_Int_Active_Apply_Buff_Action = HlEX.CreateContextActionApplyBuff(Pressure_Points_Int_Effect_Buff.ToReference<BlueprintBuffReference>(), Pressure_Points_Active_Apply_Buff_Duration, false, false, true, false, false);
            var Pressure_Points_Wis_Active_Apply_Buff_Action = HlEX.CreateContextActionApplyBuff(Pressure_Points_Wis_Effect_Buff.ToReference<BlueprintBuffReference>(), Pressure_Points_Active_Apply_Buff_Duration, false, false, true, false, false);
            var Pressure_Points_Cha_Active_Apply_Buff_Action = HlEX.CreateContextActionApplyBuff(Pressure_Points_Str_Effect_Buff.ToReference<BlueprintBuffReference>(), Pressure_Points_Active_Apply_Buff_Duration, false, false, true, false, false);

            Pressure_Points_Str_Active_Buff.ReplaceComponents<AddInitiatorAttackRollTrigger>(HlEX.CreateRunActions(Pressure_Points_Str_Active_Apply_Buff_Action, Pressure_Points_Str_Active_Buff_Conditional_Action1, Pressure_Points_Str_Active_Buff_Conditional_Action2, Pressure_Points_Str_Active_Buff_Conditional_Action3, Pressure_Points_Str_Active_Buff_Conditional_Action4, Pressure_Points_Str_Active_Buff_Conditional_Action5));
            Pressure_Points_Dex_Active_Buff.ReplaceComponents<AddInitiatorAttackRollTrigger>(HlEX.CreateRunActions(Pressure_Points_Dex_Active_Apply_Buff_Action, Pressure_Points_Dex_Active_Buff_Conditional_Action1, Pressure_Points_Dex_Active_Buff_Conditional_Action2, Pressure_Points_Dex_Active_Buff_Conditional_Action3, Pressure_Points_Dex_Active_Buff_Conditional_Action4, Pressure_Points_Dex_Active_Buff_Conditional_Action5));
            Pressure_Points_Con_Active_Buff.ReplaceComponents<AddInitiatorAttackRollTrigger>(HlEX.CreateRunActions(Pressure_Points_Con_Active_Apply_Buff_Action, Pressure_Points_Con_Active_Buff_Conditional_Action1, Pressure_Points_Con_Active_Buff_Conditional_Action2, Pressure_Points_Con_Active_Buff_Conditional_Action3, Pressure_Points_Con_Active_Buff_Conditional_Action4, Pressure_Points_Con_Active_Buff_Conditional_Action5));
            Pressure_Points_Int_Active_Buff.ReplaceComponents<AddInitiatorAttackRollTrigger>(HlEX.CreateRunActions(Pressure_Points_Int_Active_Apply_Buff_Action, Pressure_Points_Int_Active_Buff_Conditional_Action1, Pressure_Points_Int_Active_Buff_Conditional_Action2, Pressure_Points_Int_Active_Buff_Conditional_Action3, Pressure_Points_Int_Active_Buff_Conditional_Action4, Pressure_Points_Int_Active_Buff_Conditional_Action5));
            Pressure_Points_Wis_Active_Buff.ReplaceComponents<AddInitiatorAttackRollTrigger>(HlEX.CreateRunActions(Pressure_Points_Wis_Active_Apply_Buff_Action, Pressure_Points_Wis_Active_Buff_Conditional_Action1, Pressure_Points_Wis_Active_Buff_Conditional_Action2, Pressure_Points_Wis_Active_Buff_Conditional_Action3, Pressure_Points_Wis_Active_Buff_Conditional_Action4, Pressure_Points_Wis_Active_Buff_Conditional_Action5));
            Pressure_Points_Cha_Active_Buff.ReplaceComponents<AddInitiatorAttackRollTrigger>(HlEX.CreateRunActions(Pressure_Points_Cha_Active_Apply_Buff_Action, Pressure_Points_Cha_Active_Buff_Conditional_Action1, Pressure_Points_Cha_Active_Buff_Conditional_Action2, Pressure_Points_Cha_Active_Buff_Conditional_Action3, Pressure_Points_Cha_Active_Buff_Conditional_Action4, Pressure_Points_Cha_Active_Buff_Conditional_Action5));

            var Pressure_Points_Str_Activatable_Ability = Debilitating_Injury_Bewildered_Activatable_Ability.CreateCopy(ToDContext, "NinjaTrickPressurePointsStrToggleAbility", bp => {
                bp.SetName(ToDContext, Pressure_Points_Str_Active_Buff.Name);
                bp.SetDescription(ToDContext, Pressure_Points_Str_Active_Buff.Description);
                bp.m_Buff = Pressure_Points_Str_Active_Buff.ToReference<BlueprintBuffReference>();
                bp.m_Icon = Pressure_Points_Str_Active_Buff.m_Icon;

            });

            var Pressure_Points_Dex_Activatable_Ability = Debilitating_Injury_Bewildered_Activatable_Ability.CreateCopy(ToDContext, "NinjaTrickPressurePointsDexToggleAbility", bp => {
                bp.SetName(ToDContext, Pressure_Points_Dex_Active_Buff.Name);
                bp.SetDescription(ToDContext, Pressure_Points_Dex_Active_Buff.Description);
                bp.m_Buff = Pressure_Points_Dex_Active_Buff.ToReference<BlueprintBuffReference>();
                bp.m_Icon = Pressure_Points_Dex_Active_Buff.m_Icon;

            });

            var Pressure_Points_Con_Activatable_Ability = Debilitating_Injury_Bewildered_Activatable_Ability.CreateCopy(ToDContext, "NinjaTrickPressurePointsConToggleAbility", bp => {
                bp.SetName(ToDContext, Pressure_Points_Con_Active_Buff.Name);
                bp.SetDescription(ToDContext, Pressure_Points_Con_Active_Buff.Description);
                bp.m_Buff = Pressure_Points_Con_Active_Buff.ToReference<BlueprintBuffReference>();
                bp.m_Icon = Pressure_Points_Con_Active_Buff.m_Icon;

            });

            var Pressure_Points_Int_Activatable_Ability = Debilitating_Injury_Bewildered_Activatable_Ability.CreateCopy(ToDContext, "NinjaTrickPressurePointsIntToggleAbility", bp => {
                bp.SetName(ToDContext, Pressure_Points_Int_Active_Buff.Name);
                bp.SetDescription(ToDContext, Pressure_Points_Int_Active_Buff.Description);
                bp.m_Buff = Pressure_Points_Int_Active_Buff.ToReference<BlueprintBuffReference>();
                bp.m_Icon = Pressure_Points_Int_Active_Buff.m_Icon;

            });

            var Pressure_Points_Wis_Activatable_Ability = Debilitating_Injury_Bewildered_Activatable_Ability.CreateCopy(ToDContext, "NinjaTrickPressurePointsWisToggleAbility", bp => {
                bp.SetName(ToDContext, Pressure_Points_Wis_Active_Buff.Name);
                bp.SetDescription(ToDContext, Pressure_Points_Wis_Active_Buff.Description);
                bp.m_Buff = Pressure_Points_Wis_Active_Buff.ToReference<BlueprintBuffReference>();
                bp.m_Icon = Pressure_Points_Wis_Active_Buff.m_Icon;

            });

            var Pressure_Points_Cha_Activatable_Ability = Debilitating_Injury_Bewildered_Activatable_Ability.CreateCopy(ToDContext, "NinjaTrickPressurePointsChaToggleAbility", bp => {
                bp.SetName(ToDContext, Pressure_Points_Cha_Active_Buff.Name);
                bp.SetDescription(ToDContext, Pressure_Points_Cha_Active_Buff.Description);
                bp.m_Buff = Pressure_Points_Cha_Active_Buff.ToReference<BlueprintBuffReference>();
                bp.m_Icon = Pressure_Points_Cha_Active_Buff.m_Icon;

            });



            var Pressure_Points_Con_Unlock_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickPressurePointsConUnlock", bp => {
                bp.SetDescription(ToDContext, "The character learned new pressure points that allow him to deal {g|Encyclopedia:Constitution}Constitution{/g} {g|Encyclopedia:Damage}damage{/g} with his sneak attacks.");
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
            });

            var Pressure_Points_Int_Unlock_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickPressurePointsIntUnlock", bp => {
                bp.SetDescription(ToDContext, "The character learned new pressure points that allow him to deal {g|Encyclopedia:Intelligence}Intelligence{/g} {g|Encyclopedia:Damage}damage{/g} with his sneak attacks.");
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
            });

            var Pressure_Points_Wis_Unlock_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickPressurePointsWisUnlock", bp => {
                bp.SetDescription(ToDContext, "The character learned new pressure points that allow him to deal {g|Encyclopedia:Wisdom}Wisdom{/g} {g|Encyclopedia:Damage}damage{/g} with his sneak attacks.");
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
            });

            var Pressure_Points_Cha_Unlock_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickPressurePointsChaUnlock", bp => {
                bp.SetDescription(ToDContext, "The character learned new pressure points that allow him to deal {g|Encyclopedia:Charisma}Charisma{/g} {g|Encyclopedia:Damage}damage{/g} with his sneak attacks.");
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
            });

            var Pressure_Points_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickPressurePointsFeature", bp => {
                bp.SetName(ToDContext, "Pressure Points");
                bp.SetDescription(ToDContext, "A character with this trick can strike at an opponent's vital pressure points, causing weakness and intense pain. Whenever the character deals sneak attack damage, he also deals 1 point of {g|Encyclopedia:Strength}Strength{/g} or {g|Encyclopedia:Dexterity}Dexterity{/g} {g|Encyclopedia:Damage}damage{/g}, decided by the character.");
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
                bp.Ranks = 1;
                bp.m_Icon = PressurePointsIcon;
                bp.IsClassFeature = true;
                bp.AddComponent(HlEX.CreateAddFacts(Pressure_Points_Str_Activatable_Ability.ToReference<BlueprintUnitFactReference>(), Pressure_Points_Dex_Activatable_Ability.ToReference<BlueprintUnitFactReference>()));
                bp.AddComponent<AddFeatureIfHasFact>(c =>
                {
                    c.m_CheckedFact = Pressure_Points_Con_Unlock_Feature.ToReference<BlueprintUnitFactReference>();
                    c.m_Feature = Pressure_Points_Con_Activatable_Ability.ToReference<BlueprintUnitFactReference>();
                    c.Not = false;
                });
                bp.AddComponent<AddFeatureIfHasFact>(c =>
                {
                    c.m_CheckedFact = Pressure_Points_Int_Unlock_Feature.ToReference<BlueprintUnitFactReference>();
                    c.m_Feature = Pressure_Points_Int_Activatable_Ability.ToReference<BlueprintUnitFactReference>();
                    c.Not = false;
                });
                bp.AddComponent<AddFeatureIfHasFact>(c =>
                {
                    c.m_CheckedFact = Pressure_Points_Wis_Unlock_Feature.ToReference<BlueprintUnitFactReference>();
                    c.m_Feature = Pressure_Points_Wis_Activatable_Ability.ToReference<BlueprintUnitFactReference>();
                    c.Not = false;
                });
                bp.AddComponent<AddFeatureIfHasFact>(c =>
                {
                    c.m_CheckedFact = Pressure_Points_Cha_Unlock_Feature.ToReference<BlueprintUnitFactReference>();
                    c.m_Feature = Pressure_Points_Cha_Activatable_Ability.ToReference<BlueprintUnitFactReference>();
                    c.Not = false;
                });                
            });

            Pressure_Points_Feature.AddComponent(HlEX.CreatePrerequisiteNoFeature(Pressure_Points_Feature));

            ToDContext.Logger.LogPatch("Created Pressure Points trick.", Pressure_Points_Feature);

        }

    }
}
