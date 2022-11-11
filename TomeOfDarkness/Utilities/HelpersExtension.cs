using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Kingmaker.View;
using Kingmaker.ResourceLinks;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;
using TomeOfDarkness.NewComponents;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using static TomeOfDarkness.Main;
using System.Web;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.ElementsSystem;
using Kingmaker.Designers.EventConditionActionSystem.ContextData;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Class.Kineticist;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using TabletopTweaks.Core.NewComponents;
using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Core.NewComponents.Properties;
using Kingmaker.Assets.UnitLogic.Mechanics.Properties;
using Kingmaker.UnitLogic.Class.Kineticist.Properties;
using Kingmaker.Localization;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem;
using Owlcat.Runtime.UI.Tooltips;
using TabletopTweaks.Core.UMMTools.Utility;
using UnityEngine.UI.Extensions;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem.LogThreads.Common;
using static RootMotion.FinalIK.InteractionObject;
using JetBrains.Annotations;
using Kingmaker.Localization.Shared;
using System.Reflection;
using System.Text.RegularExpressions;
using TabletopTweaks.Core.Localization;
using TabletopTweaks.Core.ModLogic;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements.DamageResistance;
using TabletopTweaks.Core.UMMTools;
using TomeOfDarkness.NewComponents.Properties;
using TabletopTweaks.Core.NewActions;
using TomeOfDarkness.NewActions;



namespace TomeOfDarkness.Utilities
{
    public static class HelpersExtension
    {

        #region |----------------------------------------------------------------| ResourceLinks Creators |----------------------------------------------------------------|



        public static PrefabLink CreatePrefabLink(string asset_id) // Holic75_SC 
        {
            var link = new PrefabLink();
            link.AssetId = asset_id;
            return link;
        }

        public static ProjectileLink CreateProjectileLink(string asset_id)
        {
            var link = new ProjectileLink();
            link.AssetId = asset_id;
            return link;
        }

        public static UnitViewLink CreateUnitViewLink(string asset_id) // Holic75_SC 
        {
            var link = new UnitViewLink();
            link.AssetId = asset_id;
            return link;
        }

        #endregion

        #region |------------------------------------------------------------------| Blueprint Creators |------------------------------------------------------------------|


        public static BlueprintAbilityResource CreateAbilityResource(String name,
                                                              Sprite icon = null,
                                                              params BlueprintComponent[] components)

        {
            var resource = Helpers.CreateBlueprint<BlueprintAbilityResource>(ToDContext, name, bp =>
            {
                bp.m_Icon = icon;
                bp.SetComponents(components);
            });
            return resource;
        }

        // Holic75_PT
        public static BlueprintAbilityResource CreateAbilityResource(String name,
                                                              String display_name,
                                                              String description,
                                                              Sprite icon = null,
                                                              params BlueprintComponent[] components)

        {
            var resource = Helpers.CreateBlueprint<BlueprintAbilityResource>(ToDContext, name, bp =>
            {
                bp.LocalizedName = Helpers.CreateString(ToDContext, $"{bp.name}.Name", display_name);
                bp.LocalizedDescription = Helpers.CreateString(ToDContext, $"{bp.name}.Name", description);
                bp.m_Icon = icon;
                bp.SetComponents(components);
            });
            return resource;
        }

        #endregion

        #region |------------------------------------------------------------------| Elements Creators |-------------------------------------------------------------------|

        // Holic75_SC
        public static ConditionsChecker CreateConditionsCheckerAnd(params Condition[] conditions)
        {
            return new ConditionsChecker() { Conditions = conditions, Operation = Operation.And };
        }

        // Holic75_SC
        public static ConditionsChecker CreateConditionsCheckerOr(params Condition[] conditions)
        {
            return new ConditionsChecker() { Conditions = conditions, Operation = Operation.Or };
        }


        #endregion

        #region |------------------------------------------------------------------| Mechanics Creators |------------------------------------------------------------------|

        // Holic75_SC
        public static ContextValue CreateContextValueRank(AbilityRankType value = AbilityRankType.Default) => value.CreateContextValue();

        // Holic75_SC
        public static ContextValue CreateContextValue(this AbilityRankType value)
        {
            return new ContextValue() { ValueType = ContextValueType.Rank, ValueRank = value };
        }

        // Holic75_SC
        public static ContextValue CreateContextValue(this AbilitySharedValue value)
        {
            return new ContextValue() { ValueType = ContextValueType.Shared, ValueShared = value };
        }

        // Holic75_SC
        public static ContextDiceValue CreateContextDiceValue(this DiceType dice, ContextValue diceCount = null, ContextValue bonus = null)
        {
            return new ContextDiceValue()
            {
                DiceType = dice,
                DiceCountValue = diceCount ?? CreateContextValueRank(),
                BonusValue = bonus ?? 0
            };
        }

        // Holic75_SC
        public static ContextDurationValue CreateContextDuration(ContextValue bonus = null, DurationRate rate = DurationRate.Rounds, DiceType diceType = DiceType.Zero, ContextValue diceCount = null)
        {
            return new ContextDurationValue()
            {
                BonusValue = bonus ?? CreateContextValueRank(),
                Rate = rate,
                DiceCountValue = diceCount ?? 0,
                DiceType = diceType
            };
        }

        // Holic75_SC
        public static ContextDurationValue CreateContextDurationNonExtandable(ContextValue bonus = null, DurationRate rate = DurationRate.Rounds, DiceType diceType = DiceType.Zero, ContextValue diceCount = null)
        {
            var d = new ContextDurationValue()
            {
                BonusValue = bonus ?? CreateContextValueRank(),
                Rate = rate,
                DiceCountValue = diceCount ?? 0,
                DiceType = diceType
            };
            d.m_IsExtendable = false;

            return d;
        }

        #endregion

        #region |--------------------------------------------------------------| ( Event ) Actions Creators |---------------------------------------------------------------|

        // Holic75_SC
        public static Conditional CreateConditional(Condition condition, GameAction ifTrue, GameAction ifFalse = null)
        {
            var c = Helpers.Create<Conditional>();
            c.ConditionsChecker = CreateConditionsCheckerAnd(condition);
            c.IfTrue = Helpers.CreateActionList(ifTrue);
            c.IfFalse = Helpers.CreateActionList(ifFalse);
            return c;
        }

        // Holic75_SC
        public static Conditional CreateConditional(Condition[] condition, GameAction ifTrue, GameAction ifFalse = null)
        {
            var c = Helpers.Create<Conditional>();
            c.ConditionsChecker = CreateConditionsCheckerAnd(condition);
            c.IfTrue = Helpers.CreateActionList(ifTrue);
            c.IfFalse = Helpers.CreateActionList(ifFalse);
            return c;
        }

        // Holic75_SC
        public static Conditional CreateConditionalOr(Condition[] condition, GameAction ifTrue, GameAction ifFalse = null)
        {
            var c = Helpers.Create<Conditional>();
            c.ConditionsChecker = CreateConditionsCheckerOr(condition);
            c.IfTrue = Helpers.CreateActionList(ifTrue);
            c.IfFalse = Helpers.CreateActionList(ifFalse);
            return c;
        }

        // Holic75_SC
        public static Conditional CreateConditional(Condition[] condition, GameAction[] ifTrue, GameAction[] ifFalse = null)
        {
            var c = Helpers.Create<Conditional>();
            c.ConditionsChecker = CreateConditionsCheckerAnd(condition);
            c.IfTrue = Helpers.CreateActionList(ifTrue);
            c.IfFalse = Helpers.CreateActionList(ifFalse);
            return c;
        }

        // Holic75_SC
        public static Conditional CreateConditional(ConditionsChecker conditions, GameAction ifTrue, GameAction ifFalse = null)
        {
            var c = Helpers.Create<Conditional>();
            c.ConditionsChecker = conditions;
            c.IfTrue = Helpers.CreateActionList(ifTrue);
            c.IfFalse = Helpers.CreateActionList(ifFalse);
            return c;
        }

        // Holic75_SC
        public static Conditional CreateConditional(Condition condition, GameAction[] ifTrue, GameAction[] ifFalse = null)
        {
            var c = Helpers.Create<Conditional>();
            c.ConditionsChecker = new ConditionsChecker() { Conditions = new Condition[] { condition } };
            c.IfTrue = Helpers.CreateActionList(ifTrue);
            c.IfFalse = Helpers.CreateActionList(ifFalse ?? Array.Empty<GameAction>());
            return c;
        }

        // Holic75_SC
        public static Conditional CreateConditional(ConditionsChecker condition, GameAction[] ifTrue, GameAction[] ifFalse = null)
        {
            var c = Helpers.Create<Conditional>();
            c.ConditionsChecker = condition;
            c.IfTrue = Helpers.CreateActionList(ifTrue);
            c.IfFalse = Helpers.CreateActionList(ifFalse ?? Array.Empty<GameAction>());
            return c;
        }

        #endregion

        #region  |-----------------------------------------------------------| ( Mechanics ) Actions Creators |-------------------------------------------------------------|

        // Holic75_SC
        public static ContextActionApplyBuff CreateContextActionApplyBuff(this BlueprintBuff buff, ContextDurationValue duration, bool fromSpell, bool dispellable = true, bool toCaster = false, bool asChild = false, bool permanent = false)
        {
            var result = Helpers.Create<ContextActionApplyBuff>();
            result.m_Buff = buff.ToReference<BlueprintBuffReference>();
            result.DurationValue = duration;
            result.IsFromSpell = fromSpell;
            result.IsNotDispelable = !dispellable;
            result.ToCaster = toCaster;
            result.AsChild = asChild;
            result.Permanent = permanent;
            return result;
        }


        static public ContextActionApplyBuffRanks CreateContextActionApplyBuffRanks(this BlueprintBuff buff, ContextDurationValue duration, ContextValue applied_ranks, bool fromSpell, bool dispellable = true, bool toCaster = false, bool asChild = false, bool permanent = false, bool linked_to_area = true)
        {
            var result = Helpers.Create<ContextActionApplyBuffRanks>();
            result.m_Buff = buff.ToReference<BlueprintBuffReference>();
            result.Rank = applied_ranks;
            result.DurationValue = duration;
            result.IsFromSpell = fromSpell;
            result.IsNotDispelable = !dispellable;
            result.ToCaster = toCaster;
            result.AsChild = asChild;
            result.Permanent = permanent;
            result.NotLinkToAreaEffect = !linked_to_area;

            return result;
        }

        static public ContextActionRepeatedApplyBuff CreateContextActionRepeatedApplyBuff(this BlueprintBuff buff, ContextDurationValue duration, ContextValue buff_applications, bool fromSpell, bool dispellable = true, bool toCaster = false, bool asChild = false, bool permanent = false, bool linked_to_area = true)
        {
            var result = Helpers.Create<ContextActionRepeatedApplyBuff>();
            result.m_Buff = buff.ToReference<BlueprintBuffReference>();
            result.BuffApplications = buff_applications;
            result.DurationValue = duration;
            result.IsFromSpell = fromSpell;
            result.IsNotDispelable = !dispellable;
            result.ToCaster = toCaster;
            result.AsChild = asChild;
            result.Permanent = permanent;
            result.NotLinkToAreaEffect = !linked_to_area;

            return result;
        }

        // Holic75_SC
        static public ContextActionConditionalSaved CreateContextActionConditionalSaved(GameAction[] success, GameAction[] failed)
        {
            var r = Helpers.Create<ContextActionConditionalSaved>();
            r.Succeed = Helpers.CreateActionList(success);
            r.Failed = Helpers.CreateActionList(failed);
            return r;
        }

        // Holic75_SC
        static public ContextActionConditionalSaved CreateContextActionConditionalSaved(GameAction success, GameAction failed)
        {
            return CreateContextActionConditionalSaved( success == null ? new GameAction[0] : new GameAction[] { success }, failed == null ? new GameAction[0] : new GameAction[] { failed });
        }

        // Holic75_PT
        static public ContextActionRemoveBuff CreateContextActionRemoveBuff(BlueprintBuff buff)
        {
            var r = Helpers.Create<ContextActionRemoveBuff>();
            r.m_Buff = buff.ToReference<BlueprintBuffReference>();
            return r;
        }

        static public ContextActionRemoveBuff CreateContextActionRemoveBuffRank(BlueprintBuff buff)
        {
            var r = Helpers.Create<ContextActionRemoveBuff>();
            r.m_Buff = buff.ToReference<BlueprintBuffReference>();
            r.RemoveRank = true;
            return r;
        }

        // Holic75_SC
        static public ContextActionRemoveBuffsByDescriptor CreateContextActionRemoveBuffsByDescriptor(SpellDescriptor descriptor, bool not_self = true)
        {
            var r = Helpers.Create<ContextActionRemoveBuffsByDescriptor>();
            r.SpellDescriptor = descriptor;
            r.NotSelf = not_self;
            return r;
        }

        // Holic75_SC
        static public ContextActionSavingThrow CreateContextActionSavingThrow(SavingThrowType saving_throw, Kingmaker.ElementsSystem.ActionList action)
        {
            var r = Helpers.Create<ContextActionSavingThrow>();
            r.Type = saving_throw;
            r.Actions = action;
            return r;
        }

        // Holic75_SC
        static public ContextActionSkillCheck CreateContextActionSkillCheck(StatType skill, Kingmaker.ElementsSystem.ActionList success = null, Kingmaker.ElementsSystem.ActionList failure = null, ContextValue custom_dc = null)
        {
            var r = Helpers.Create<ContextActionSkillCheck>();
            r.UseCustomDC = custom_dc != null;
            r.CustomDC = custom_dc;
            r.Success = success == null ? Helpers.CreateActionList() : success;
            r.Failure = failure == null ? Helpers.CreateActionList() : failure;
            r.Stat = skill;
            return r;
        }

        // Holic75_SC
        static public ContextActionSkillCheck CreateContextActionSkillCheck(StatType skill, GameAction success = null, GameAction failure = null, ContextValue custom_dc = null)
        {
            return CreateContextActionSkillCheck(skill, success == null ? null : Helpers.CreateActionList(success) , failure == null ? null : Helpers.CreateActionList(failure), custom_dc);
        }

        static public ContextActionSpawnAreaEffect CreateContextActionSpawnAreaEffect(BlueprintAbilityAreaEffectReference area_effect, ContextDurationValue duration)
        {
            var r = Helpers.Create<ContextActionSpawnAreaEffect>();
            r.m_AreaEffect = area_effect;
            r.DurationValue = duration;
            return r;
        }

        #endregion

        #region |------------------------------------------------------------| ( Mechanics ) Buffs Creators |--------------------------------------------------------------|

        public static HasFactFeatureUnlock CreateHasFactFeatureUnlock(BlueprintUnitFactReference checked_fact, BlueprintUnitFactReference feature, bool not)
        {
            var c = Helpers.Create<HasFactFeatureUnlock>();
            c.m_CheckedFact = checked_fact;
            c.m_Feature = feature;
            c.Not = not;
            return c;
        }



        #endregion

        #region |----------------------------------------------------------| ( Abilities ) Components Creators |-----------------------------------------------------------|


        public static AbilityShowIfCasterHasFact CreateAbilityShowIfCasterHasFact(BlueprintUnitFactReference checked_fact, bool not = false)
        {
            var c = Helpers.Create<AbilityShowIfCasterHasFact>();
            c.m_UnitFact = checked_fact;
            c.Not = not;
            return c;
        }

        // Holic75_SC
        public static AbilityAreaEffectRunAction CreateAreaEffectRunAction(GameAction unitEnter = null, GameAction unitExit = null, GameAction unitMove = null, GameAction round = null)
        {
            var a = Helpers.Create<AbilityAreaEffectRunAction>();
            a.UnitEnter = Helpers.CreateActionList(unitEnter);
            a.UnitExit = Helpers.CreateActionList(unitExit);
            a.UnitMove = Helpers.CreateActionList(unitMove);
            a.Round = Helpers.CreateActionList(round);
            return a;
        }

        // Holic75_SC
        public static AbilityAreaEffectRunAction CreateAreaEffectRunAction(GameAction[] unitEnter = null, GameAction[] unitExit = null, GameAction[] unitMove = null, GameAction[] round = null)
        {
            var a = Helpers.Create<AbilityAreaEffectRunAction>();
            a.UnitEnter = Helpers.CreateActionList(unitEnter);
            a.UnitExit = Helpers.CreateActionList(unitExit);
            a.UnitMove = Helpers.CreateActionList(unitMove);
            a.Round = Helpers.CreateActionList(round);
            return a;
        }

        // Holic75_SC
        public static AbilitySpawnFx CreateAbilitySpawnFx(string asset_id,
                                                  AbilitySpawnFxAnchor position_anchor = AbilitySpawnFxAnchor.None,
                                                  AbilitySpawnFxAnchor orientation_anchor = AbilitySpawnFxAnchor.None,
                                                  AbilitySpawnFxAnchor anchor = AbilitySpawnFxAnchor.None)
        {
            var a = Helpers.Create<AbilitySpawnFx>();
            a.PrefabLink = CreatePrefabLink(asset_id);
            a.PositionAnchor = position_anchor;
            a.OrientationAnchor = orientation_anchor;
            a.Anchor = anchor;

            return a;
        }

        // Holic75_SC
        public static AbilitySpawnFx CreateAbilitySpawnFxTime(string asset_id, AbilitySpawnFxTime time,
                                                              AbilitySpawnFxAnchor position_anchor = AbilitySpawnFxAnchor.None,
                                                              AbilitySpawnFxAnchor orientation_anchor = AbilitySpawnFxAnchor.None,
                                                              AbilitySpawnFxAnchor anchor = AbilitySpawnFxAnchor.None)
        {
            var a = Helpers.Create<AbilitySpawnFx>();
            a.PrefabLink = CreatePrefabLink(asset_id);
            a.PositionAnchor = position_anchor;
            a.OrientationAnchor = orientation_anchor;
            a.Anchor = anchor;
            a.Time = time;
            return a;
        }

        // Holic75_SC
        public static AbilitySpawnFx CreateAbilitySpawnFxDestroyOnCast(string asset_id,
                                                                       AbilitySpawnFxAnchor position_anchor = AbilitySpawnFxAnchor.None,
                                                                       AbilitySpawnFxAnchor orientation_anchor = AbilitySpawnFxAnchor.None,
                                                                       AbilitySpawnFxAnchor anchor = AbilitySpawnFxAnchor.None)
        {
            var a = Helpers.Create<AbilitySpawnFx>();
            a.PrefabLink = CreatePrefabLink(asset_id);
            a.PositionAnchor = position_anchor;
            a.OrientationAnchor = orientation_anchor;
            a.Anchor = anchor;
            a.DestroyOnCast = true;
            return a;
        }

        // Holic75_PT
        public static AbilityTargetsAround CreateAbilityTargetsAround(Feet radius, TargetType target_type, ConditionsChecker conditions = null, Feet spread_speed = default(Feet), bool include_dead = false)
        {
            var around = Helpers.Create<AbilityTargetsAround>();
            around.m_Radius = radius;
            around.m_TargetType = target_type;
            around.m_IncludeDead = include_dead;
            around.m_Condition = conditions ?? new ConditionsChecker() { Conditions = Array.Empty<Condition>() };
            around.m_SpreadSpeed = spread_speed;
            return around;
        }

        // Holic75_SC
        // These creators create a list of variant of the parent BlueprintAbility from a given variants list and then adds the parent as such in each of these variants' Blueprints
        public static AbilityVariants CreateAbilityVariants(this BlueprintAbility parent, IEnumerable<BlueprintAbility> variants) => CreateAbilityVariants(parent, variants.ToArray());

        // Holic75_SC
        public static AbilityVariants CreateAbilityVariants(this BlueprintAbility parent, params BlueprintAbility[] variants)
        {
            var a = Helpers.Create<AbilityVariants>();

            BlueprintAbilityReference[] variants_reference = new BlueprintAbilityReference[variants.Length];

            for (int i = 0; i < variants_reference.Length; i++)
            {
                variants_reference[i] = variants[i].ToReference<BlueprintAbilityReference>();
            }

            a.m_Variants = variants_reference;
            foreach (var vr in variants)
            {
                vr.m_Parent = parent.ToReference<BlueprintAbilityReference>();
            }
            return a;
        }

        // Holic75_SC
        public static AbilityResourceLogic CreateResourceLogic(this BlueprintAbilityResource resource,
                                                        bool spend = true,
                                                        int amount = 1,
                                                        bool cost_is_custom = false)

        {
            var arl = Helpers.Create<AbilityResourceLogic>();
            arl.m_IsSpendResource = spend;
            arl.m_RequiredResource = resource.ToReference<BlueprintAbilityResourceReference>();
            arl.Amount = amount;
            arl.CostIsCustom = cost_is_custom;
            return arl;
        }

        // Holic75_SC
        public static AbilityEffectRunAction CreateRunActions(params GameAction[] actions)
        {
            var result = Helpers.Create<AbilityEffectRunAction>();
            result.Actions = Helpers.CreateActionList(actions);
            return result;
        }



        #endregion

        #region |----------------------------------------------------------| ( Mechanics ) Components Creators |-----------------------------------------------------------|

        // Holic75_SC
        public static AddFactContextActions CreateAddFactContextActions(GameAction[] onActivate = null, GameAction[] onDeactivate = null, GameAction[] onNewRound = null)
        {
            var a = Helpers.Create<AddFactContextActions>();
            a.Activated = Helpers.CreateActionList(onActivate);
            a.Deactivated = Helpers.CreateActionList(onDeactivate);
            a.NewRound = Helpers.CreateActionList(onNewRound);
            return a;
        }

        public static ContextCalculateAbilityParamsBasedOnClass CreateContextCalculateAbilityParamsBasedOnClass(BlueprintCharacterClassReference character_class,
                                                                                                         StatType stat,
                                                                                                         bool use_kineticist_main_stat = false)
        {
            var c = Helpers.Create<ContextCalculateAbilityParamsBasedOnClass>();
            c.m_CharacterClass = character_class;
            c.StatType = stat;
            c.UseKineticistMainStat = use_kineticist_main_stat;
            return c;
        }

        // Holic75_PT 
        public static ContextCalculateAbilityParamsBasedOnClasses CreateContextCalculateAbilityParamsBasedOnClasses(BlueprintCharacterClassReference[] character_classes,
                                                                                                                                   StatType stat)
        {
            var c = Helpers.Create<ContextCalculateAbilityParamsBasedOnClasses>();
            c.m_CharacterClasses = character_classes;
            c.m_StatType = stat;
            return c;
        }

        // Holic75_PT 
        public static ContextCalculateAbilityParamsBasedOnClasses CreateContextCalculateAbilityParamsBasedOnClassesWithProperty(BlueprintCharacterClassReference[] character_classes,
                                                                                                                                 BlueprintUnitPropertyReference property,
                                                                                                                                 StatType stat = StatType.Charisma)
        {
            var c = Helpers.Create<ContextCalculateAbilityParamsBasedOnClasses>();
            c.m_CharacterClasses = character_classes;
            c.m_StatType = stat;
            c.StatTypeFromCustomProperty = true;
            c.m_CustomProperty = property;
            return c;
        }

        // Holic75_PT 
        public static ContextCalculateAbilityParamsBasedOnClasses CreateContextCalculateAbilityParamsBasedOnClassesWithArchetypes(BlueprintCharacterClassReference[] character_classes,
                                                                                                                                     BlueprintArchetypeReference[] archetypes,
                                                                                                                                     StatType stat)
        {
            var c = Helpers.Create<ContextCalculateAbilityParamsBasedOnClasses>();
            c.m_CharacterClasses = character_classes;
            c.CheckArchetype = true;
            c.m_Archetypes = archetypes;
            c.m_StatType = stat;
            return c;
        }

        // Holic75_PT 
        public static ContextCalculateAbilityParamsBasedOnClasses CreateContextCalculateAbilityParamsBasedOnClassesWithArchetypesWithProperty(BlueprintCharacterClassReference[] character_classes,
                                                                                                                                               BlueprintArchetypeReference[] archetypes,
                                                                                                                                               BlueprintUnitPropertyReference property,
                                                                                                                                               StatType stat = StatType.Charisma)
        {
            var c = Helpers.Create<ContextCalculateAbilityParamsBasedOnClasses>();
            c.m_CharacterClasses = character_classes;
            c.CheckArchetype = true;
            c.m_Archetypes = archetypes;
            c.m_StatType = stat;
            c.StatTypeFromCustomProperty = true;
            c.m_CustomProperty = property;
            return c;
        }

        // Holic75_PT 
        public static ContextWeaponDamageDiceReplacementWeaponCategory CreateContextWeaponDamageDiceReplacementWeaponCategory(WeaponCategory[] categories, DiceFormula[] dice_formulas, ContextValue value)
        {
            var c = Helpers.Create<ContextWeaponDamageDiceReplacementWeaponCategory>();
            c.Categories = categories;
            c.Dice_Formulas = dice_formulas;
            c.Value = value;

            return c;
        }

        #endregion

        #region |-----------------------------------------------------------| ( Spells ) Components Creators |-------------------------------------------------------------|

        public static SpellComponent CreateSpellComponent(SpellSchool school)
        {
            var c = Helpers.Create<SpellComponent>();
            c.School = school;
            return c;
        }


        #endregion

        #region |-----------------------------------------------------| ( FactLogic ) Component Delegates Creators |-------------------------------------------------------|

        // Holic75_SC
        public static AddFacts CreateAddFacts(params BlueprintUnitFactReference[] facts)
        {
            var c = Helpers.Create<AddFacts>();
            c.m_Facts = facts;
            return c;
        }

        // Holic75_SC
        public static AddOutgoingConcealment CreateOutgoingConcealment(AddConcealment self_concealement)
        {
            var c = Helpers.Create<AddOutgoingConcealment>();
            c.Descriptor = self_concealement.Descriptor;
            c.Concealment = self_concealement.Concealment;
            c.CheckDistance = self_concealement.CheckDistance;
            c.CheckWeaponRangeType = self_concealement.CheckWeaponRangeType;
            c.DistanceGreater = self_concealement.DistanceGreater;
            c.OnlyForAttacks = self_concealement.OnlyForAttacks;
            c.RangeType = self_concealement.RangeType;
            return c;
        }


        #endregion

        #region |----------------------------------------------------------| ( Mechanics ) Conditions Creators |-----------------------------------------------------------|

        // Holic75_SC
        public static ContextConditionAlignment CreateContextConditionAlignment(AlignmentComponent alignment, bool check_caster = false, bool not = false)
        {
            var c = Helpers.Create<ContextConditionAlignment>();
            c.Alignment = alignment;
            c.Not = not;
            c.CheckCaster = check_caster;
            return c;
        }

        // Holic75_SC
        static public ContextConditionCasterHasFact CreateContextConditionCasterHasFact(BlueprintUnitFact fact, bool has = true)
        {
            var c = Helpers.Create<ContextConditionCasterHasFact>();
            c.m_Fact = fact.ToReference<BlueprintUnitFactReference>();
            c.Not = !has;
            return c;
        }

        // Holic75_SC
        public static ContextConditionHasBuff CreateConditionHasBuff(this BlueprintBuff buff)
        {
            var hasBuff = Helpers.Create<ContextConditionHasBuff>();
            hasBuff.m_Buff = buff.ToReference<BlueprintBuffReference>();
            return hasBuff;
        }

        // Holic75_SC
        public static ContextConditionHasBuff CreateConditionHasNoBuff(this BlueprintBuff buff)
        {
            var hasBuff = Helpers.Create<ContextConditionHasBuff>();
            hasBuff.m_Buff = buff.ToReference<BlueprintBuffReference>();
            hasBuff.Not = true;
            return hasBuff;
        }

        // Holic75_SC
        public static ContextConditionHasBuffFromCaster CreateContextConditionHasBuffFromCaster(BlueprintBuff buff, bool not = false)
        {
            var c = Helpers.Create<ContextConditionHasBuffFromCaster>();
            c.m_Buff = buff.ToReference<BlueprintBuffReference>();
            c.Not = not;
            return c;
        }

        // Holic75_SC
        static public ContextConditionHasFact CreateContextConditionHasFact(BlueprintUnitFact fact, bool has = true)
        {
            var c = Helpers.Create<ContextConditionHasFact>();
            c.m_Fact = fact.ToReference<BlueprintUnitFactReference>();
            c.Not = !has;
            return c;
        }

        // Holic75_SC
        static public ContextConditionIsCaster CreateContextConditionIsCaster(bool not = false)
        {
            var c = Helpers.Create<ContextConditionIsCaster>();
            c.Not = not;
            return c;
        }

        #endregion

        #region |------------------------------------------------------------| ( Mechanics ) Facts Creators |--------------------------------------------------------------|

        // Holic75_SC
        public static AddAbilityResources CreateAddAbilityResources(this BlueprintAbilityResourceReference resource)
        {
            var c = Helpers.Create<AddAbilityResources>();
            c.m_Resource = resource;
            c.RestoreAmount = true;
            return c;
        }

        // Holic75_SC
        public static AddAbilityResources CreateAddAbilityResourcesNoRestore(this BlueprintAbilityResourceReference resource)
        {
            var c = Helpers.Create<AddAbilityResources>();
            c.m_Resource = resource;
            c.RestoreAmount = false;
            return c;
        }

        // Holic75_SC
        public static AddFeatureOnClassLevel CreateAddFeatureOnClassLevel(BlueprintFeatureReference feature, BlueprintCharacterClassReference[] character_classes, int level, bool before_level = false)
        {
            var c = Helpers.Create<AddFeatureOnClassLevel>();
            c.name = $"AddFeatureOnClassLevel${feature.Get().name}";
            c.m_Feature = feature;
            c.m_Class = character_classes[0];
            c.m_AdditionalClasses = character_classes.Skip(1).ToArray();
            c.m_Archetypes = new BlueprintArchetypeReference[0];
            c.Level = level;
            c.BeforeThisLevel = before_level;

            return c;
        }



        #endregion

        #region |---------------------------------------------------------------| Prerequisite Creators |------------------------------------------------------------------|

        public static PrerequisiteNoFeature CreatePrerequisiteNoFeature(this BlueprintFeature feat, bool any = false)
        {
            var p = Helpers.Create<PrerequisiteNoFeature>();
            p.m_Feature = feat.ToReference<BlueprintFeatureReference>();
            p.Group = any ? Prerequisite.GroupType.Any : Prerequisite.GroupType.All;
            return p;
        }

        #endregion

        #region |----------------------------------------------------------| ( Kineticist ) Properties Creators |----------------------------------------------------------|

        public static BaseAtackGetter CreateBaseAtackGetter()
        {
            var ppv = Helpers.Create<BaseAtackGetter>();
            return ppv;
        }

        public static BaseAttackPropertyWithFeatureList CreateBaseAttackPropertyWithFeatureList(int base_value, int base_attack_divisor, int base_attack_zero, int feature_bonus, BlueprintFeatureReference[] features)
        {
            var ppv = Helpers.Create<BaseAttackPropertyWithFeatureList>();
            ppv.BaseValue = base_value;
            ppv.BaseAttackDiv = base_attack_divisor;
            ppv.BaseAttackZero = base_attack_zero;
            ppv.FeatureBonus = feature_bonus;
            ppv.m_Features = features;
            return ppv;
        }

        public static CurrentMeleeWeaponDamageStatGetter CreateCurrentMeleeWeaponDamageStatGetter()
        {
            var ppv = Helpers.Create<CurrentMeleeWeaponDamageStatGetter>();
            return ppv;
        }

        public static CurrentWeaponCriticalMultiplierGetter CreateCurrentWeaponCriticalMultiplierGetter()
        {
            var ppv = Helpers.Create<CurrentWeaponCriticalMultiplierGetter>();
            return ppv;
        }

        public static FightingDefensivelyACBonusProperty CreateFightingDefensivelyACBonusProperty()
        {
            var ppv = Helpers.Create<FightingDefensivelyACBonusProperty>();
            return ppv;
        }

        public static FightingDefensivelyAttackPenaltyProperty CreateFightingDefensivelyAttackPenaltyProperty()
        {
            var ppv = Helpers.Create<FightingDefensivelyAttackPenaltyProperty>();
            return ppv;
        }

        public static LevelBasedPropertyWithFeatureList CreateLevelBasedPropertyWithFeatureList(int base_value, int level_divisor, int level_zero, int feature_bonus, BlueprintFeatureReference[] features)
        {
            var ppv = Helpers.Create<LevelBasedPropertyWithFeatureList>();
            ppv.BaseValue = base_value;
            ppv.LevelDiv = level_divisor;
            ppv.LevelZero = level_zero;
            ppv.m_Features = features;
            ppv.FeatureBonus = feature_bonus;
            return ppv;
        }

        public static StatBonusIfHasFactProperty CreateStatBonusIfHasFactProperty(int multiplier, StatType stat, BlueprintUnitFactReference fact)
        {
            var ppv = Helpers.Create<StatBonusIfHasFactProperty>();
            ppv.Multiplier = multiplier;
            ppv.Stat = stat;
            ppv.m_RequiredFact = fact;
            return ppv;
        }

        #endregion

        #region |----------------------------------------------------------| ( Mechanics ) Properties Creators |-----------------------------------------------------------|


        public static CastingAttributeGetter CreateCastingAttributeGetter()
        {
            var ppv = Helpers.Create<CastingAttributeGetter>();
            return ppv;
        }
        public static CustomProgressionPropertyGetter CreateCustomProgressionPropertyGetter(UnitProperty property, int start = 1, int step = 1)
        {
            var ppv = Helpers.Create<CustomProgressionPropertyGetter>();
            ppv.Property = property;
            ppv.Start = start;
            ppv.Step = step;
            return ppv;
        }

        public static MaxAttributeBonusGetter CreateMaxAttributeBonusGetter()
        {
            var ppv = Helpers.Create<MaxAttributeBonusGetter>();
            return ppv;
        }

        public static MaxCastingAttributeGetter CreateMaxCastingAttributeGetter()
        {
            var ppv = Helpers.Create<MaxCastingAttributeGetter>();
            return ppv;
        }

        public static CompositeCustomPropertyGetter CreateCompositeCustomPropertyGetter(CompositeCustomPropertyGetter.Mode mode, CompositeCustomPropertyGetter.ComplexCustomProperty[] properties)
        {
            var cpg = Helpers.Create<CompositeCustomPropertyGetter>();
            cpg.CalculationMode = mode;
            cpg.Properties = properties;
            return cpg;

        }

        public static CompositePropertyGetter CreateCompositePropertyGetter(CompositePropertyGetter.Mode mode, CompositePropertyGetter.ComplexProperty[] properties)
        {
            var cpg = Helpers.Create<CompositePropertyGetter>();
            cpg.CalculationMode = mode;
            cpg.Properties = properties;
            return cpg;
        }

        public static CompositePropertyGetter.ComplexProperty CreateComplexProperty(UnitProperty property, int bonus = 0, float numerator = 1.0f, float denominator = 1.0f)
        {
            var cp = Helpers.Create<CompositePropertyGetter.ComplexProperty>();
            cp.Property = property;
            cp.Bonus = bonus;
            cp.Numerator = numerator;
            cp.Denominator = denominator;
            return cp;
        }

        public static CompositeCustomPropertyGetter.ComplexCustomProperty ComplexCustomProperty(PropertyValueGetter property, int bonus = 0, float numerator = 1.0f, float denominator = 1.0f)
        {
            var ccp = Helpers.Create<CompositeCustomPropertyGetter.ComplexCustomProperty>();
            ccp.Property = property;
            ccp.Bonus = bonus;
            ccp.Numerator = numerator;
            ccp.Denominator = denominator;
            return ccp;
        }


        #endregion

        #region |----------------------------------------------------| ( UnitLogic Mechanics ) Properties Creators |-------------------------------------------------------|

        public static AnimalPetOwnerRankGetter CreateAnimalPetOwnerRankGetter(UnitProperty property)
        {
            var ppv = Helpers.Create<AnimalPetOwnerRankGetter>();
            ppv.Property = property;
            return ppv;
        }

        public static ArcaneSpellFailureChanceGetter CreateArcaneSpellFailureChanceGetter(UnitProperty property)
        {
            var ppv = Helpers.Create<ArcaneSpellFailureChanceGetter>();
            return ppv;
        }

        public static AreaCrComplexGetter CreateAreaCrComplexGetter(int bonus, int multiplier = 1, int denominator = 1)
        {
            var ppv = Helpers.Create<AreaCrComplexGetter>();
            ppv.Bonus = bonus;
            ppv.Multiplier = multiplier;
            ppv.Denominator = denominator;
            return ppv;
        }

        public static ClassLevelGetter CreateClassLevelGetter(BlueprintCharacterClassReference character_class, BlueprintArchetypeReference archetype = null)
        {
            var ppv = Helpers.Create<ClassLevelGetter>();
            ppv.m_Class = character_class;
            ppv.m_Archetype = archetype;
            return ppv;
        }

        public static CustomPropertyGetter CreateCustomPropertyGetter(BlueprintUnitProperty property)
        {
            var ppv = Helpers.Create<CustomPropertyGetter>();
            ppv.m_Property = property.ToReference<BlueprintUnitPropertyReference>();
            return ppv;
        }


        public static CompanionsCountGetter CreateCompanionsCountGetter()
        {
            var ppv = Helpers.Create<CompanionsCountGetter>();
            return ppv;
        }

        public static FactRankGetter CreateFactRankGetter(BlueprintUnitFactReference fact)
        {
            var ppv = Helpers.Create<FactRankGetter>();
            ppv.m_Fact = fact;
            return ppv;
        }

        public static PropertyWithFactRankGetter CreatePropertyWithFactRankGetter(BlueprintUnitFactReference fact, int rank_multiplier = 1)
        {
            var ppv = Helpers.Create<PropertyWithFactRankGetter>();
            ppv.m_Fact = fact;
            ppv.m_RankMultiplier = rank_multiplier;
            return ppv;
        }

        public static ShieldBonusGetter CreateShieldBonusGetter()
        {
            var ppv = Helpers.Create<ShieldBonusGetter>();
            return ppv;
        }

        public static SimplePropertyGetter CreateSimplePropertyGetter(UnitProperty property)
        {
            var ppv = Helpers.Create<SimplePropertyGetter>();
            ppv.Property = property;
            return ppv;
        }

        public static SkillRankGetter CreateSkillRankGetter(StatType skill)
        {
            var ppv = Helpers.Create<SkillRankGetter>();
            ppv.Skill = skill;
            return ppv;
        }

        public static SkillValueGetter CreateSkillValueGetter(StatType skill)
        {
            var ppv = Helpers.Create<SkillValueGetter>();
            ppv.Skill = skill;
            return ppv;
        }

        public static SpellLevelGetter CreateSpellLevelGetter(bool from_cast_rule)
        {
            var ppv = Helpers.Create<SpellLevelGetter>();
            ppv.FromCastRule = from_cast_rule;
            return ppv;
        }

        public static StatValueGetter CreateStatValueGetter(StatValueGetter.ReturnType type)
        {
            var ppv = Helpers.Create<StatValueGetter>();
            ppv.ValueType = type;
            return ppv;
        }


        public static SummClassLevelGetter CreateSummClassLevelGetter(BlueprintCharacterClassReference[] character_classes, BlueprintArchetypeReference[] archetypes = null)
        {
            var ppv = Helpers.Create<SummClassLevelGetter>();
            ppv.m_Class = character_classes;
            ppv.m_Archetypes = archetypes;
            return ppv;
        }

        public static UnitWeaponEnhancementGetter CreateUnitWeaponEnhancementGetter()
        {
            var ppv = Helpers.Create<UnitWeaponEnhancementGetter>();
            return ppv;
        }

        public static PropertyMulipliedByFactRankGetter CreatePropertyMulipliedByFactRankGetter(UnitProperty property, BlueprintUnitFactReference fact, int bonus = 0, float numerator = 1, float denominator = 1)
        {
            var ppv = Helpers.Create<PropertyMulipliedByFactRankGetter>();
            ppv.m_Property = property;
            ppv.m_Fact = fact;
            ppv.Bonus = bonus;
            ppv.Numerator = numerator;
            ppv.Denominator = denominator;
            return ppv;
        }


        #endregion

        #region  |------------------------------------------------|  Converters - ActivatableAbility Creators (Simple) |----------------------------------------------------|

        // Holic75_PT
        // This converter creates a toggle that activates the buff. This is the full version, which adds all kind of name modification features.
        static public BlueprintActivatableAbility ConvertBuffToActivatableAbility(BlueprintBuff buff,
                                                                                  CommandType command,
                                                                                  bool deactivate_immediately,
                                                                                  string prefixAdd = "",
                                                                                  string prefixRemove = "",
                                                                                  string suffixAdd = "ToggleAbility",
                                                                                  string suffixRemove = "Buff",
                                                                                  string replaceOldText1 = "",
                                                                                  string replaceOldText2 = "",
                                                                                  string replaceOldText3 = "",
                                                                                  string replaceNewText1 = "",
                                                                                  string replaceNewText2 = "",
                                                                                 string replaceNewText3 = "",
                                                                                 params BlueprintComponent[] components
                                                        )
        {

            string toggleName = buff.name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                toggleName = toggleName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                toggleName = toggleName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                toggleName = prefixAdd + toggleName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                toggleName = toggleName + suffixAdd;
            }
            if (!String.IsNullOrEmpty(replaceOldText1))
            {
                toggleName = toggleName.Replace(replaceOldText1, replaceNewText1);
            }
            if (!String.IsNullOrEmpty(replaceOldText2))
            {
                toggleName = toggleName.Replace(replaceOldText2, replaceNewText2);
            }
            if (!String.IsNullOrEmpty(replaceOldText3))
            {
                toggleName = toggleName.Replace(replaceOldText3, replaceNewText3);
            }


            var toggle = Helpers.CreateBlueprint<BlueprintActivatableAbility>(ToDContext, toggleName, bp =>
            {
                bp.m_Buff = buff.ToReference<BlueprintBuffReference>();
                bp.SetName(ToDContext, buff.Name);
                bp.SetDescription(ToDContext, buff.Description);
                bp.m_Icon = buff.Icon;
                bp.ResourceAssetIds = new string[0];
                bp.ActivationType = (command == CommandType.Free) ? AbilityActivationType.Immediately : AbilityActivationType.WithUnitCommand;
                bp.m_ActivateWithUnitCommand = command;
                bp.SetComponents(components);
                bp.DeactivateImmediately = deactivate_immediately;

            });

            return toggle;

        }

        // Holic75_PT
        // This converter creates a toggle that activates the buff. This is the barebone version which lists only suffix to remove and to add.
        static public BlueprintActivatableAbility ConvertBuffToActivatableAbility(BlueprintBuff buff,
                                                                                  CommandType command,
                                                                                  bool deactivate_immediately,
                                                                                  string suffixAdd = "ToggleAbility",
                                                                                  string suffixRemove = "Buff",
                                                                                 params BlueprintComponent[] components
                                                        )
        {

            string toggleName = buff.name;

            if (!String.IsNullOrEmpty(suffixRemove))
            {
                toggleName = toggleName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                toggleName = toggleName + suffixAdd;
            }


            var toggle = Helpers.CreateBlueprint<BlueprintActivatableAbility>(ToDContext, toggleName, bp =>
            {
                bp.m_Buff = buff.ToReference<BlueprintBuffReference>();
                bp.SetName(ToDContext, buff.Name);
                bp.SetDescription(ToDContext, buff.Description);
                bp.m_Icon = buff.Icon;
                bp.ResourceAssetIds = new string[0];
                bp.ActivationType = (command == CommandType.Free) ? AbilityActivationType.Immediately : AbilityActivationType.WithUnitCommand;
                bp.m_ActivateWithUnitCommand = command;
                bp.SetComponents(components);
                bp.DeactivateImmediately = deactivate_immediately;

            });

            return toggle;

        }



        #endregion

        #region |-----------------------------------------------------|  Converters - Feature Creators (Simple) |----------------------------------------------------------|

        // Holic75_PT
        // This converter creates a feature that matches the ability from which has been created.
        // This was mostly used (in Holic75's mod) to create features that add bonus or required features (such as Iroran Paladin's auto-selection of Irori as Deity)
        static public BlueprintFeature ConvertFeatureToFeature(BlueprintFeature feature1,
                                                                    string prefixAdd = "",
                                                                    string prefixRemove = "",
                                                                    string suffixAdd = "",
                                                                    string suffixRemove = "",
                                                                    string replaceOldText1 = "",
                                                                    string replaceOldText2 = "",
                                                                    string replaceOldText3 = "",
                                                                    string replaceNewText1 = "",
                                                                    string replaceNewText2 = "",
                                                                    string replaceNewText3 = "",
                                                                    bool hide = true
                                                                )
        {

            string feature1AltName = feature1.name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                feature1AltName = feature1AltName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                feature1AltName = feature1AltName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                feature1AltName = prefixAdd + feature1AltName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                feature1AltName = feature1AltName + suffixAdd;
            }
            if (!String.IsNullOrEmpty(replaceOldText1))
            {
                feature1AltName = feature1AltName.Replace(replaceOldText1, replaceNewText1);
            }
            if (!String.IsNullOrEmpty(replaceOldText2))
            {
                feature1AltName = feature1AltName.Replace(replaceOldText2, replaceNewText2);
            }
            if (!String.IsNullOrEmpty(replaceOldText3))
            {
                feature1AltName = feature1AltName.Replace(replaceOldText3, replaceNewText3);
            }

            var feature2 = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, feature1AltName, bp =>
            {
                bp.SetName(ToDContext, feature1.Name);
                bp.SetDescription(ToDContext, feature1.Description);
                bp.m_Icon = feature1.Icon;
                bp.AddComponent<AddFeatureOnApply>(c =>
                {
                    c.m_Feature = feature1.ToReference<BlueprintFeatureReference>();
                });


                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
            });
            if (hide)
            {
                feature2.HideInCharacterSheetAndLevelUp = true;
            }

            return feature2;
        }

        // Holic75_PT
        // This converter creates a feature that matches the ability from which has been created.
        static public BlueprintFeature ConvertAbilityToFeature(BlueprintAbility ability,
                                                                    string prefixAdd = "",
                                                                    string prefixRemove = "",
                                                                    string suffixAdd = "Feature",
                                                                    string suffixRemove = "Ability",
                                                                    bool hide = true
                                                                )
        {

            string abilityAltName = ability.name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                abilityAltName = abilityAltName.Replace(prefixRemove, "");

            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                abilityAltName = abilityAltName.Replace(suffixRemove, "");

            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                abilityAltName = prefixAdd + abilityAltName;

            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                abilityAltName = abilityAltName + suffixAdd;

            }

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, abilityAltName, bp =>
            {
                bp.SetName(ToDContext, ability.Name);
                bp.SetDescription(ToDContext, ability.Description);
                bp.m_Icon = ability.Icon;
                bp.AddComponent<AddFeatureIfHasFact>(c =>
                {
                    c.m_CheckedFact = ability.ToReference<BlueprintUnitFactReference>();
                    c.m_Feature = ability.ToReference<BlueprintUnitFactReference>();
                    c.Not = true;
                });
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
            });
            if (hide)
            {
                feature.HideInCharacterSheetAndLevelUp = true;
                feature.HideInUI = true;
            }

            return feature;
        }

        // Holic75_PT
        // This converter creates a feature that matches the ability from which has been created (version without replacements)
        static public BlueprintFeature ConvertAbilityToFeature(BlueprintAbility ability,
                                                            bool hide = true
                                                        )
        {

            string abilityAltName = ability.name + "Feature";


            var feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, abilityAltName, bp =>
            {
                bp.SetName(ToDContext, ability.Name);
                bp.SetDescription(ToDContext, ability.Description);
                bp.m_Icon = ability.Icon;
                bp.AddComponent<AddFeatureIfHasFact>(c =>
                {
                    c.m_CheckedFact = ability.ToReference<BlueprintUnitFactReference>();
                    c.m_Feature = ability.ToReference<BlueprintUnitFactReference>();
                    c.Not = true;
                });
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
            });
            if (hide)
            {
                feature.HideInCharacterSheetAndLevelUp = true;
                feature.HideInUI = true;
            }

            return feature;
        }

        // Holic75_PT
        // This converter creates a feature that matches the ability from which has been created but adds without checking.
        static public BlueprintFeature ConvertAbilityToFeatureNoCheck(BlueprintAbility ability,
                                                                        string prefixAdd = "",
                                                                        string prefixRemove = "",
                                                                        string suffixAdd = "Feature",
                                                                        string suffixRemove = "Ability",
                                                                        bool hide = true
                                                                    )
        {

            string abilityAltName = ability.name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                abilityAltName = abilityAltName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                abilityAltName = abilityAltName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                abilityAltName = prefixAdd + abilityAltName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                abilityAltName = abilityAltName + suffixAdd;
            }

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, abilityAltName, bp =>
            {
                bp.SetName(ToDContext, ability.Name);
                bp.SetDescription(ToDContext, ability.Description);
                bp.m_Icon = ability.Icon;
                bp.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[] { ability.ToReference<BlueprintUnitFactReference>() };
                });
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
            });
            if (hide)
            {
                feature.HideInCharacterSheetAndLevelUp = true;
                feature.HideInUI = true;
            }

            return feature;
        }

        // Holic75_PT
        // This converter creates a feature that matches the activatable ability from which has been created and adds it without checking.
        static public BlueprintFeature ConvertActivatableAbilityToFeature(BlueprintActivatableAbility ability,
                                                                        string prefixAdd = "",
                                                                        string prefixRemove = "",
                                                                        string suffixAdd = "Feature",
                                                                        string suffixRemove = "ToggleAbility",
                                                                        bool hide = true
                                                                    )
        {

            string activatable_abilityAltName = ability.name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                activatable_abilityAltName = activatable_abilityAltName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                activatable_abilityAltName = activatable_abilityAltName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                activatable_abilityAltName = prefixAdd + activatable_abilityAltName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                activatable_abilityAltName = activatable_abilityAltName + suffixAdd;
            }

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, activatable_abilityAltName, bp =>
            {
                bp.SetName(ToDContext, ability.Name);
                bp.SetDescription(ToDContext, ability.Description);
                bp.m_Icon = ability.Icon;
                bp.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[] { ability.ToReference<BlueprintUnitFactReference>() };
                });
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
            });
            if (hide)
            {
                feature.HideInCharacterSheetAndLevelUp = true;
                feature.HideInUI = true;
            }

            return feature;
        }

        // Holic75_PT
        // This converter creates a feature that matches the buff from which has been created.
        static public BlueprintFeature ConvertBuffToFeature(BlueprintBuff buff,
                                                            string prefixAdd = "",
                                                            string prefixRemove = "",
                                                            string suffixAdd = "Feature",
                                                            string suffixRemove = "",
                                                            bool hide = true
                                                        )
        {

            string buffAltName = buff.name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                buffAltName = buffAltName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                buffAltName = buffAltName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                buffAltName = prefixAdd + buffAltName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                buffAltName = buffAltName + suffixAdd;
            }

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, buffAltName, bp =>
            {
                bp.SetName(ToDContext, buff.Name);
                bp.SetDescription(ToDContext, buff.Description);
                bp.m_Icon = buff.Icon;
                bp.SetComponents(buff.ComponentsArray);
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            return feature;
        }



        #endregion

        #region |-------------------------------------------------|  Converters - Specific Ability Creators (Simple) |-----------------------------------------------------|

        // Holic75_PT
        // This converter creates a spell-like ability from an existing spell.
        // Compared to the original Holic75's version, I have added more optional string parameters to allow to customize the name of the spell-like ability.
        static public BlueprintAbility ConvertSpellToSpellLike(BlueprintAbility spell,
                                                               BlueprintCharacterClassReference[] classes,
                                                               StatType stat,
                                                               BlueprintAbilityResource resource = null,
                                                               string prefixAdd = "",
                                                               string prefixRemove = "",
                                                               string suffixAdd = "",
                                                               string suffixRemove = "",
                                                               string replaceOldText1 = "",
                                                               string replaceOldText2 = "",
                                                               string replaceOldText3 = "",
                                                               string replaceNewText1 = "",
                                                               string replaceNewText2 = "",
                                                               string replaceNewText3 = "",
                                                               bool no_resource = false,
                                                               bool no_scaling = false,
                                                               BlueprintArchetypeReference[] archetypes = null,
                                                               int cost = 1

                                                              )
        {

            string spelllikeName = spell.name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                spelllikeName = spelllikeName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                spelllikeName = spelllikeName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                spelllikeName = prefixAdd + spelllikeName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                spelllikeName = spelllikeName + suffixAdd;
            }
            if (!String.IsNullOrEmpty(replaceOldText1))
            {
                spelllikeName = spelllikeName.Replace(replaceOldText1, replaceNewText1);
            }
            if (!String.IsNullOrEmpty(replaceOldText2))
            {
                spelllikeName = spelllikeName.Replace(replaceOldText2, replaceNewText2);
            }
            if (!String.IsNullOrEmpty(replaceOldText3))
            {
                spelllikeName = spelllikeName.Replace(replaceOldText3, replaceNewText3);
            }

            var ability = spell.CreateCopy(ToDContext, spelllikeName);

            if (!no_scaling)
            {
                ability.RemoveComponents<SpellListComponent>();
            }

            ability.Type = AbilityType.SpellLike;

            if (!no_scaling)
            {
                ability.AddComponent(CreateContextCalculateAbilityParamsBasedOnClassesWithArchetypes(classes, archetypes, stat));
            }

            ability.MaterialComponent = BlueprintTools.GetBlueprint<BlueprintAbility>("2d81362af43aeac4387a3d4fced489c3").MaterialComponent; // Fireball spell (no component)

            if (!no_resource)
            {
                var resource2 = resource;
                if (resource2 == null)
                {
                    resource2 = CreateAbilityResource(spelllikeName + "Resource", null);
                    resource2.SetFixedResource(cost);
                }
                ability.AddComponent(CreateResourceLogic(resource2, amount: cost));
            }

            ability.Parent = null;
            return ability;

        }

        // Holic75_PT
        // This converter variant converts a spell-like ability from an existing spell, but drops all the string alterations BUT the prefix.
        static public BlueprintAbility ConvertSpellToSpellLike(BlueprintAbility spell,
                                                               BlueprintCharacterClassReference[] classes,
                                                               StatType stat,
                                                               BlueprintAbilityResource resource = null,
                                                               string prefixAdd = "",
                                                               bool no_resource = false,
                                                               bool no_scaling = false,
                                                               BlueprintArchetypeReference[] archetypes = null,
                                                               int cost = 1

                                                              )
        {

            string spelllikeName = spell.name;

            if (!String.IsNullOrEmpty(prefixAdd))
            {
                spelllikeName = prefixAdd + spelllikeName;
            }

            var ability = spell.CreateCopy(ToDContext, spelllikeName);

            if (!no_scaling)
            {
                ability.RemoveComponents<SpellListComponent>();
            }

            ability.Type = AbilityType.SpellLike;

            if (!no_scaling)
            {
                ability.AddComponent(CreateContextCalculateAbilityParamsBasedOnClassesWithArchetypes(classes, archetypes, stat));
            }

            ability.MaterialComponent = BlueprintTools.GetBlueprint<BlueprintAbility>("2d81362af43aeac4387a3d4fced489c3").MaterialComponent; // Fireball spell (no component)

            if (!no_resource)
            {
                var resource2 = resource;
                if (resource2 == null)
                {
                    resource2 = CreateAbilityResource(spelllikeName + "Resource", null);
                    resource2.SetFixedResource(cost);
                }
                ability.AddComponent(CreateResourceLogic(resource2, amount: cost));
            }

            ability.Parent = null;
            return ability;

        }

        // Holic75_PT
        // This converter creates a supernatural ability from an existing spell.
        // Compared to the original Holic75's version, I have added more optional string parameters to allow to customize the name of the supernatural ability,
        // moreover I have completely redone the part of non-dispellable buffs to avoid to have to port the changeAction method, which seemed either impossible or (more likely)
        // too hard for me to port!
        static public BlueprintAbility ConvertSpellToSupernatural(BlueprintAbility spell,
                                                                   BlueprintCharacterClassReference[] classes,
                                                                   StatType stat,
                                                                   BlueprintAbilityResource resource = null,
                                                                   string prefixAdd = "",
                                                                   bool no_resource = false,
                                                                   bool no_scaling = false,
                                                                   BlueprintArchetypeReference[] archetypes = null,
                                                                   int cost = 1

                                                                  )
        {

            var ability = ConvertSpellToSpellLike(spell, classes, stat, resource, prefixAdd, no_resource, no_scaling, archetypes: archetypes, cost: cost);
            ability.Type = AbilityType.Supernatural;
            ability.SpellResistance = false;
            ability.RemoveComponents<SpellComponent>();
            ability.AvailableMetamagic = (Metamagic)0;

            GameAction[] action_storage = new GameAction[0];

            //make buffs non dispellable
            var actions = ability.GetComponent<AbilityEffectRunAction>();

            ability.FlattenAllActions()
                   .OfType<ContextActionApplyBuff>()
                        .ForEach(b =>
                        {
                            b.IsNotDispelable = true;
                            b.IsFromSpell = false;
                        });


            return ability;

        }

        // Holic75_PT
        // This converter creates a supernatural ability from an existing spell, but drops all the string alterations BUT the prefix.
        static public BlueprintAbility ConvertSpellToSupernatural(BlueprintAbility spell,
                                                                   BlueprintCharacterClassReference[] classes,
                                                                   StatType stat,
                                                                   BlueprintAbilityResource resource = null,
                                                                   string prefixAdd = "",
                                                                   string prefixRemove = "",
                                                                   string suffixAdd = "",
                                                                   string suffixRemove = "",
                                                                   string replaceOldText1 = "",
                                                                   string replaceOldText2 = "",
                                                                   string replaceOldText3 = "",
                                                                   string replaceNewText1 = "",
                                                                   string replaceNewText2 = "",
                                                                   string replaceNewText3 = "",
                                                                   bool no_resource = false,
                                                                   bool no_scaling = false,
                                                                   BlueprintArchetypeReference[] archetypes = null,
                                                                   int cost = 1

                                                                  )
        {

            var ability = ConvertSpellToSpellLike(spell, classes, stat, resource, prefixAdd, prefixRemove, suffixAdd, suffixRemove, replaceOldText1, replaceOldText2, replaceOldText3, replaceNewText1, replaceNewText2, replaceNewText3, no_resource, no_scaling, archetypes: archetypes, cost: cost);
            ability.Type = AbilityType.Supernatural;
            ability.SpellResistance = false;
            ability.RemoveComponents<SpellComponent>();
            ability.AvailableMetamagic = (Metamagic)0;

            GameAction[] action_storage = new GameAction[0];

            //make buffs non dispellable
            var actions = ability.GetComponent<AbilityEffectRunAction>();

            ability.FlattenAllActions()
                   .OfType<ContextActionApplyBuff>()
                        .ForEach(b =>
                        {
                            b.IsNotDispelable = true;
                            b.IsFromSpell = false;
                        });


            return ability;

        }

        // KINETIC TALENTS
        // This converts a spell to a kinetic talent, allowing for different name alterations.
        static public BlueprintAbility ConvertSpellToKineticistTalent(BlueprintAbility spell,
                                                                       string prefixAdd = "",
                                                                       string prefixRemove = "",
                                                                       string suffixAdd = "",
                                                                       string suffixRemove = "",
                                                                       string replaceOldText1 = "",
                                                                       string replaceOldText2 = "",
                                                                       string replaceOldText3 = "",
                                                                       string replaceNewText1 = "",
                                                                       string replaceNewText2 = "",
                                                                       string replaceNewText3 = "",
                                                                       int burn_cost = 0)
        {
            var kineticist = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("42a455d9ec1ad924d889272429eb8391").ToReference<BlueprintCharacterClassReference>();

            var ability = ConvertSpellToSpellLike(spell, new BlueprintCharacterClassReference[] { kineticist }, StatType.Unknown, null, prefixAdd, prefixRemove, suffixAdd, suffixRemove, replaceOldText1, replaceOldText2, replaceOldText3, replaceNewText1, replaceNewText2, replaceNewText3, no_resource: true, no_scaling: true, null, 0);

            ability.AddComponents(Helpers.Create<AbilityKineticist>(a => { a.Amount = burn_cost; a.WildTalentBurnCost = burn_cost; }),
                                  Helpers.Create<ContextCalculateAbilityParamsBasedOnClass>(c => { c.m_CharacterClass = kineticist; c.StatType = StatType.Constitution; c.UseKineticistMainStat = true; }));

            ability.RemoveComponents<SpellListComponent>();
            return ability;

        }

        // This converts a spell to a kinetic talent with just a simplified name change (prefix added).
        static public BlueprintAbility ConvertSpellToKineticistTalent(BlueprintAbility spell,
                                                               string prefixAdd = "",
                                                               int burn_cost = 0)
        {
            var kineticist = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("42a455d9ec1ad924d889272429eb8391").ToReference<BlueprintCharacterClassReference>();

            var ability = ConvertSpellToSpellLike(spell, new BlueprintCharacterClassReference[] { kineticist }, StatType.Unknown, null, prefixAdd, no_resource: true, no_scaling: true, null, 0);

            ability.AddComponents(Helpers.Create<AbilityKineticist>(a => { a.Amount = burn_cost; a.WildTalentBurnCost = burn_cost; }),
                                  Helpers.Create<ContextCalculateAbilityParamsBasedOnClass>(c => { c.m_CharacterClass = kineticist; c.StatType = StatType.Constitution; c.UseKineticistMainStat = true; }));

            ability.RemoveComponents<SpellListComponent>();
            return ability;

        }

        // KI POWERS
        // This converts a spell to a ki power, allowing for different name alterations.
        public static void ConvertSpellToMonkKiPower(BlueprintAbility spell,
                                                     int required_level,
                                                     bool personal_only,
                                                     int cost = 1)
        {
            var monk = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("e8f21e5b58e0569468e420ebea456124").ToReference<BlueprintCharacterClassReference>();

            var wis_resource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82").ToReference<BlueprintAbilityResourceReference>(); // standard Ki resource (based on Wis)
            var cha_resource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("7d002c1025fbfe2458f1509bf7a89ce1").ToReference<BlueprintAbilityResourceReference>(); // Scaled Fist's Ki resource (based on Cha)

            var monk_ki_power_selection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("3049386713ff04245a38b32483362551").ToReference<BlueprintFeatureSelectionReference>();
            var scaled_fist_ki_power_selection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("4694f6ac27eaed34abb7d09ab67b4541").ToReference<BlueprintFeatureSelectionReference>();

            var sensei_mystic_powers = BlueprintTools.GetBlueprint<BlueprintFeature>("d5f7bcde6e7e5ed498f430ebf5c29837").ToReference<BlueprintFeatureReference>(); // Sensei Mystic Powers
            var sensei_mystic_powers_mass = BlueprintTools.GetBlueprint<BlueprintFeature>("a316044187ec61344ba33535f42f6a4d").ToReference<BlueprintFeatureReference>(); // Sensei Mass Mystic Powers

            string action_type = "standard";
            if (spell.ActionType == UnitCommand.CommandType.Swift)
            {
                action_type = "swift";
            }
            else if (spell.ActionType == UnitCommand.CommandType.Move)
            {
                action_type = "move";
            }
            else if (spell.IsFullRoundAction)
            {
                action_type = "full-round";
            }

            var description = $"A monk with this ki power can spend {cost} point{(cost != 1 ? "s" : "")} from his ki pool to apply effect of the {spell.Name} spell to himself as a {action_type} action.\n"
            + spell.Name + ": " + spell.Description;

            var ab_name = "Ki Power: " + spell.Name;

            var monk_ability = ConvertSpellToSpellLikeVariants(spell, new BlueprintCharacterClassReference[] { monk }, StatType.Wisdom, wis_resource, "Base", "KiPower", false, false, personal_only, null, cost);

            var scaled_fist_ability = ConvertSpellToSpellLikeVariants(spell, new BlueprintCharacterClassReference[] { monk }, StatType.Charisma, cha_resource, "Base", "ScaledFistKiPower", false, false, personal_only, null, cost);

            monk_ability.SetNameDescription(ToDContext, ab_name, description);

            scaled_fist_ability.SetNameDescription(ToDContext, ab_name, description);

            if (monk_ability.HasVariants)
            {

                var monk_ability_variants_reference = monk_ability.GetComponent<AbilityVariants>().m_Variants;

                foreach (var v in monk_ability_variants_reference)
                {
                    var v_orig = v.Get();
                    v_orig.SetName(ToDContext, "Ki Power: " + v_orig.Name);
                }

            }

            if (scaled_fist_ability.HasVariants)
            {

                var scaled_fist_ability_variants_reference = scaled_fist_ability.GetComponent<AbilityVariants>().m_Variants;

                foreach (var v in scaled_fist_ability_variants_reference)
                {
                    var v_orig = v.Get();
                    v_orig.SetName(ToDContext, "Ki Power: " + v_orig.Name);
                }

            }

            var monk_feature = ConvertAbilityToFeature(monk_ability, "", "", "Feature", "Ability", false);

            var scaled_fist_feature = ConvertAbilityToFeature(scaled_fist_ability, "", "", "Feature", "Ability", false);

            monk_feature.AddPrerequisite<PrerequisiteClassLevel>(p => { p.m_CharacterClass = monk; p.Level = required_level; });

            scaled_fist_feature.AddPrerequisite<PrerequisiteClassLevel>(p => { p.m_CharacterClass = monk; p.Level = required_level; });

            FeatToolsExtension.AddAsMonkKiPower(monk_feature);

            FeatToolsExtension.AddAsScaledFistKiPower(scaled_fist_feature);

            var mystic_wisdom_ability = monk_ability.CreateCopy(ToDContext, "SenseiAdvice" + monk_ability.name, bp =>
            {

                bp.Range = AbilityRange.Close;
                bp.SetMiscAbilityParametersSingleTargetRangedFriendly();
                bp.SetName(ToDContext, bp.Name.Replace("Ki Power: ", "Sensei Advice: "));
                var cmp = bp.GetComponent<AbilityResourceLogic>();
                cmp.Amount = cmp.Amount + 1;

            });

            if (mystic_wisdom_ability.HasVariants)
            {
                var mystic_wisdom_ability_variants = mystic_wisdom_ability.GetComponent<AbilityVariants>().m_Variants;

                int num_variants = mystic_wisdom_ability_variants.Length;

                var mystic_wisdom_abilities = new BlueprintAbility[num_variants];

                var mystic_wisdom_abilities_reference = new BlueprintAbilityReference[num_variants];

                for (int i = 0; i < num_variants; i++)
                {
                    mystic_wisdom_abilities[i] = mystic_wisdom_abilities[i].CreateCopy(ToDContext, "SenseiAdvice" + mystic_wisdom_abilities[i].name, bp =>
                    {

                        bp.Range = AbilityRange.Close;
                        bp.SetMiscAbilityParametersSingleTargetRangedFriendly();
                        bp.SetName(ToDContext, bp.Name.Replace("Ki Power: ", "Sensei Advice: "));
                        var cmp = bp.GetComponent<AbilityResourceLogic>();
                        cmp.Amount = cmp.Amount + 1;
                        bp.Parent = mystic_wisdom_ability;

                    });

                    mystic_wisdom_abilities_reference[i] = mystic_wisdom_abilities[i].ToReference<BlueprintAbilityReference>();
                }
                mystic_wisdom_ability.GetComponent<AbilityVariants>().m_Variants = mystic_wisdom_abilities_reference;

                sensei_mystic_powers.Get().AddComponent<AddFeatureIfHasFact>(c =>
                {
                    c.m_CheckedFact = monk_feature.ToReference<BlueprintUnitFactReference>();
                    c.m_Feature = mystic_wisdom_ability.ToReference<BlueprintUnitFactReference>();
                });


            }


            var mystic_wisdom_ability_mass = monk_ability.CreateCopy(ToDContext, "SenseiAdviceMass" + monk_ability.name, bp =>
            {

                bp.SetName(ToDContext, bp.Name.Replace("Ki Power: ", "Sensei Advice: Mass "));
                var cmp = bp.GetComponent<AbilityResourceLogic>();
                cmp.Amount = cmp.Amount + 2;

            });

            if (mystic_wisdom_ability_mass.HasVariants)
            {

                var mystic_wisdom_ability_mass_variants = mystic_wisdom_ability_mass.GetComponent<AbilityVariants>().m_Variants;

                int num_variants = mystic_wisdom_ability_mass_variants.Length;

                var mystic_wisdom_abilities_mass = new BlueprintAbility[num_variants];

                var mystic_wisdom_abilities_mass_reference = new BlueprintAbilityReference[num_variants];

                for (int i = 0; i < num_variants; i++)
                {
                    mystic_wisdom_abilities_mass[i] = mystic_wisdom_abilities_mass[i].CreateCopy(ToDContext, "SenseiAdviceMass" + mystic_wisdom_abilities_mass[i].name, bp =>
                    {

                        bp.SetName(ToDContext, bp.Name.Replace("Ki Power: ", "Sensei Advice: Mass "));
                        bp.Parent = mystic_wisdom_ability_mass;
                        bp.AddComponent(CreateAbilityTargetsAround(30.Feet(), TargetType.Ally));
                        var cmp = bp.GetComponent<AbilityResourceLogic>();
                        cmp.Amount = cmp.Amount + 2;

                    });

                    mystic_wisdom_abilities_mass_reference[i] = mystic_wisdom_abilities_mass[i].ToReference<BlueprintAbilityReference>();
                }


            }
            else
            {

                mystic_wisdom_ability_mass.AddComponent(CreateAbilityTargetsAround(30.Feet(), TargetType.Ally));

            }

            sensei_mystic_powers_mass.Get().AddComponent<AddFeatureIfHasFact>(c =>
            {
                c.m_CheckedFact = monk_feature.ToReference<BlueprintUnitFactReference>();
                c.m_Feature = mystic_wisdom_ability_mass.ToReference<BlueprintUnitFactReference>();
            });


        }


        #endregion

        #region |-------------------------------------------------|  Converters - Specific Ability Creators (Variant) |----------------------------------------------------|

        // Holic75_PT
        // This method creates spell-like variants from a spell (with variants). This is the most complete version, which allows to add a specific prefix for the wrapper and a lot of text modifications.
        // Note that the typical suffix used for the wrapper ability is "Base" (as in the existing game wrappers).
        static public BlueprintAbility ConvertSpellToSpellLikeVariants(BlueprintAbility spell,
                                                                       BlueprintCharacterClassReference[] classes,
                                                                       StatType stat,
                                                                       BlueprintAbilityResource resource = null,
                                                                       string suffixforWrapperAdd = "Base",
                                                                       string prefixAdd = "",
                                                                       string prefixRemove = "",
                                                                       string suffixAdd = "",
                                                                       string suffixRemove = "",
                                                                       string replaceOldText1 = "",
                                                                       string replaceOldText2 = "",
                                                                       string replaceOldText3 = "",
                                                                       string replaceNewText1 = "",
                                                                       string replaceNewText2 = "",
                                                                       string replaceNewText3 = "",
                                                                       bool no_resource = false,
                                                                       bool no_scaling = false,
                                                                       bool self_only = false,
                                                                       BlueprintArchetypeReference[] archetypes = null,
                                                                       int cost = 1
                                                                       )
        {
            if (!spell.HasVariants)
            {
                var a = ConvertSpellToSpellLike(spell, classes, stat, resource, prefixAdd, prefixRemove, suffixAdd, suffixRemove, replaceOldText1, replaceOldText2, replaceOldText3, replaceNewText1, replaceNewText2, replaceNewText3, no_resource, no_scaling, archetypes, cost);
                if (self_only)
                {
                    a.Range = AbilityRange.Personal;
                }
                return a;
            }

            var spell_variants = spell.GetComponent<AbilityVariants>().m_Variants;

            int num_variants = spell_variants.Length;

            var abilities = new BlueprintAbility[num_variants];

            for (int i = 0; i < num_variants; i++)
            {
                abilities[i] = ConvertSpellToSpellLike(spell_variants[i], classes, stat, resource, prefixAdd, prefixRemove, suffixAdd, suffixRemove, replaceOldText1, replaceOldText2, replaceOldText3, replaceNewText1, replaceNewText2, replaceNewText3, no_resource, no_scaling, archetypes, cost);
                if (self_only)
                {
                    abilities[i].Range = AbilityRange.Personal;
                }
            }

            string spelllikeWrapperName = spell.name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                spelllikeWrapperName = spelllikeWrapperName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                spelllikeWrapperName = spelllikeWrapperName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                spelllikeWrapperName = prefixAdd + spelllikeWrapperName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                spelllikeWrapperName = spelllikeWrapperName + suffixAdd;
            }
            if (!String.IsNullOrEmpty(replaceOldText1))
            {
                spelllikeWrapperName = spelllikeWrapperName.Replace(replaceOldText1, replaceNewText1);
            }
            if (!String.IsNullOrEmpty(replaceOldText2))
            {
                spelllikeWrapperName = spelllikeWrapperName.Replace(replaceOldText2, replaceNewText2);
            }
            if (!String.IsNullOrEmpty(replaceOldText3))
            {
                spelllikeWrapperName = spelllikeWrapperName.Replace(replaceOldText3, replaceNewText3);
            }
            if (!String.IsNullOrEmpty(suffixforWrapperAdd))
            {
                spelllikeWrapperName = spelllikeWrapperName + suffixforWrapperAdd;
            }

            var wrapper = CreateVariantWrapper(spelllikeWrapperName, abilities);

            wrapper.SetName(ToDContext, spell.Name);
            wrapper.SetDescription(ToDContext, spell.Description);
            wrapper.m_Icon = spell.m_Icon;

            return wrapper;

        }

        // Holic75_PT
        // This method creates spell-like variants from a spell (with variants), but drops all the string alterations BUT the prefix AND the prefix for the wrapper.
        // Note that the typical suffix used for the wrapper ability is "Base" (as in the existing game wrappers).
        static public BlueprintAbility ConvertSpellToSpellLikeVariants(BlueprintAbility spell,
                                                                       BlueprintCharacterClassReference[] classes,
                                                                       StatType stat,
                                                                       BlueprintAbilityResource resource = null,
                                                                       string suffixforWrapperAdd = "Base",
                                                                       string prefixAdd = "",
                                                                       bool no_resource = false,
                                                                       bool no_scaling = false,
                                                                       bool self_only = false,
                                                                       BlueprintArchetypeReference[] archetypes = null,
                                                                       int cost = 1
                                                                       )
        {
            if (!spell.HasVariants)
            {
                var a = ConvertSpellToSpellLike(spell, classes, stat, resource, prefixAdd, no_resource, no_scaling, archetypes, cost);
                if (self_only)
                {
                    a.Range = AbilityRange.Personal;
                }
                return a;
            }

            var spell_variants = spell.GetComponent<AbilityVariants>().m_Variants;

            int num_variants = spell_variants.Length;

            var abilities = new BlueprintAbility[num_variants];

            for (int i = 0; i < num_variants; i++)
            {
                abilities[i] = ConvertSpellToSpellLike(spell_variants[i], classes, stat, resource, prefixAdd, no_resource, no_scaling, archetypes, cost);
                if (self_only)
                {
                    abilities[i].Range = AbilityRange.Personal;
                }
            }

            string spelllikeWrapperName = spell.name;

            if (!String.IsNullOrEmpty(prefixAdd))
            {
                spelllikeWrapperName = prefixAdd + spelllikeWrapperName;
            }
            if (!String.IsNullOrEmpty(suffixforWrapperAdd))
            {
                spelllikeWrapperName = spelllikeWrapperName + suffixforWrapperAdd;
            }

            var wrapper = CreateVariantWrapper(spelllikeWrapperName, abilities);

            wrapper.SetName(ToDContext, spell.Name);
            wrapper.SetDescription(ToDContext, spell.Description);
            wrapper.m_Icon = spell.m_Icon;

            return wrapper;

        }

        // Holic75_PT
        // This method creates supernatural variants from a spell (with variants).  This is the most complete version, which allows to add a specific prefix for the wrapper and a lot of text modifications.
        // Note that the typical suffix used for the wrapper ability is "Base" (as in the existing game wrappers).
        static public BlueprintAbility ConvertSpellToSupernaturalVariants(BlueprintAbility spell,
                                                                       BlueprintCharacterClassReference[] classes,
                                                                       StatType stat,
                                                                       BlueprintAbilityResource resource = null,
                                                                       string suffixforWrapperAdd = "Base",
                                                                       string prefixAdd = "",
                                                                       string prefixRemove = "",
                                                                       string suffixAdd = "",
                                                                       string suffixRemove = "",
                                                                       string replaceOldText1 = "",
                                                                       string replaceOldText2 = "",
                                                                       string replaceOldText3 = "",
                                                                       string replaceNewText1 = "",
                                                                       string replaceNewText2 = "",
                                                                       string replaceNewText3 = "",
                                                                       bool no_resource = false,
                                                                       bool no_scaling = false,
                                                                       bool self_only = false,
                                                                       BlueprintArchetypeReference[] archetypes = null,
                                                                       int cost = 1
                                                                       )
        {

            if (!spell.HasVariants)
            {
                var a = ConvertSpellToSupernatural(spell, classes, stat, resource, prefixAdd, prefixRemove, suffixAdd, suffixRemove, replaceOldText1, replaceOldText2, replaceOldText3, replaceNewText1, replaceNewText2, replaceNewText3, no_resource, no_scaling, archetypes, cost);
                if (self_only)
                {
                    a.Range = AbilityRange.Personal;
                }
                return a;

            }

            var spell_variants = spell.GetComponent<AbilityVariants>().m_Variants;

            int num_variants = spell_variants.Length;

            var abilities = new BlueprintAbility[num_variants];

            for (int i = 0; i < num_variants; i++)
            {
                abilities[i] = ConvertSpellToSupernatural(spell_variants[i], classes, stat, resource, prefixAdd, prefixRemove, suffixAdd, suffixRemove, replaceOldText1, replaceOldText2, replaceOldText3, replaceNewText1, replaceNewText2, replaceNewText3, no_resource, no_scaling, archetypes, cost);
                if (self_only)
                {
                    abilities[i].Range = AbilityRange.Personal;
                }
            }


            string supernaturalWrapperName = spell.name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                supernaturalWrapperName = supernaturalWrapperName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                supernaturalWrapperName = supernaturalWrapperName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                supernaturalWrapperName = prefixAdd + supernaturalWrapperName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                supernaturalWrapperName = supernaturalWrapperName + suffixAdd;
            }
            if (!String.IsNullOrEmpty(replaceOldText1))
            {
                supernaturalWrapperName = supernaturalWrapperName.Replace(replaceOldText1, replaceNewText1);
            }
            if (!String.IsNullOrEmpty(replaceOldText2))
            {
                supernaturalWrapperName = supernaturalWrapperName.Replace(replaceOldText2, replaceNewText2);
            }
            if (!String.IsNullOrEmpty(replaceOldText3))
            {
                supernaturalWrapperName = supernaturalWrapperName.Replace(replaceOldText3, replaceNewText3);
            }
            if (!String.IsNullOrEmpty(suffixforWrapperAdd))
            {
                supernaturalWrapperName = supernaturalWrapperName + suffixforWrapperAdd;
            }


            var wrapper = CreateVariantWrapper(supernaturalWrapperName, abilities);

            wrapper.SetName(ToDContext, spell.Name);
            wrapper.SetDescription(ToDContext, spell.Description);
            wrapper.m_Icon = spell.m_Icon;

            return wrapper;


        }

        // Holic75_PT
        // This method creates supernatural variants from a spell (with variants), but drops all the string alterations BUT the prefix AND the prefix for the wrapper.
        // Note that the typical suffix used for the wrapper ability is "Base" (as in the existing game wrappers).
        static public BlueprintAbility ConvertSpellToSupernaturalVariants(BlueprintAbility spell,
                                                                       BlueprintCharacterClassReference[] classes,
                                                                       StatType stat,
                                                                       BlueprintAbilityResource resource = null,
                                                                       string suffixforWrapperAdd = "Base",
                                                                       string prefixAdd = "",
                                                                       bool no_resource = false,
                                                                       bool no_scaling = false,
                                                                       bool self_only = false,
                                                                       BlueprintArchetypeReference[] archetypes = null,
                                                                       int cost = 1
                                                                       )
        {

            if (!spell.HasVariants)
            {
                var a = ConvertSpellToSupernatural(spell, classes, stat, resource, prefixAdd, no_resource, no_scaling, archetypes, cost);
                if (self_only)
                {
                    a.Range = AbilityRange.Personal;
                }
                return a;

            }

            var spell_variants = spell.GetComponent<AbilityVariants>().m_Variants;

            int num_variants = spell_variants.Length;

            var abilities = new BlueprintAbility[num_variants];

            for (int i = 0; i < num_variants; i++)
            {
                abilities[i] = ConvertSpellToSupernatural(spell_variants[i], classes, stat, resource, prefixAdd, no_resource, no_scaling, archetypes, cost);
                if (self_only)
                {
                    abilities[i].Range = AbilityRange.Personal;
                }
            }


            string supernaturalWrapperName = spell.name;

            if (!String.IsNullOrEmpty(prefixAdd))
            {
                supernaturalWrapperName = prefixAdd + supernaturalWrapperName;
            }
            if (!String.IsNullOrEmpty(suffixforWrapperAdd))
            {
                supernaturalWrapperName = supernaturalWrapperName + suffixforWrapperAdd;
            }

            var wrapper = CreateVariantWrapper(supernaturalWrapperName, abilities);

            wrapper.SetName(ToDContext, spell.Name);
            wrapper.SetDescription(ToDContext, spell.Description);
            wrapper.m_Icon = spell.m_Icon;

            return wrapper;


        }

        #endregion

        #region |-------------------------------------------------------|  ( Blueprint ) Miscellaneous Functions |---------------------------------------------------------|

        // Holic75_SC
        static public LevelEntry[] RemoveEntries(LevelEntry[] old_entries, Predicate<BlueprintFeatureBase> predicate, bool keep_empty_entries = false)
        {
            List<LevelEntry> level_entries = new List<LevelEntry>();

            for (int i = 0; i < old_entries.Length; i++)
            {
                var new_entry = new LevelEntry() { Level = old_entries[i].Level };

                foreach (var f in old_entries[i].Features.ToArray())
                {
                    if (!predicate(f))
                    {
                        new_entry.Features.Add(f);
                    }
                }

                if (!new_entry.Features.Empty() || keep_empty_entries)
                {
                    level_entries.Add(new_entry);
                }

            }

            return level_entries.ToArray();
        }

        // Holic75_PT
        public static void ReplaceDomainSpell (BlueprintProgression domain_progression, BlueprintAbility new_spell, int level)
        {
            var domain_base_feature = domain_progression.LevelEntries[0].m_Features[0]; // This is the base feature for the domain progression.
            string domain_base_feature_name = domain_base_feature.Get().Name;
            var domain_spell_list = domain_progression.GetComponent<LearnSpellList>().SpellList;
            var domain_spells = domain_spell_list.SpellsByLevel.First(sp => sp.SpellLevel == level).m_Spells;
            var old_spell = domain_spells[0];
            
            string new_description = "";
            string new_base_feature_description = "";
            string domain_progression_description = domain_progression.Description;
            string domain_progression_description_no_encyclopedia_tags = domain_progression_description.StripHTML().RemoveEncyclopediaTags();
            string old_spell_name = old_spell.Get().Name;
            string old_spell_name_lc = old_spell_name.ToLower();
            string new_spell_name = new_spell.Name;
            string new_spell_name_lc = new_spell_name.ToLower();
            string domain_base_feature_description = domain_base_feature.Get().Description;
            string domain_base_feature_description_no_encyclopedia_tags = domain_base_feature_description.StripHTML().RemoveEncyclopediaTags();

            if (level < 9)
            {
                new_description = ReplaceFirstOccurrence(domain_progression_description_no_encyclopedia_tags, old_spell_name_lc + ",", new_spell_name_lc + ",");
            }
            else
            {
                new_description = ReplaceFirstOccurrence(domain_progression_description_no_encyclopedia_tags, old_spell_name_lc + ".", new_spell_name_lc + ".");
            }

            var domain_blueprints = GetBlueprintsByDescription<BlueprintProgression>(domain_progression_description).ToArray();

            foreach (var bpt in domain_blueprints)
            {
                bpt.SetDescription(ToDContext, new_description);
            }

            if (level < 9)
            {
                new_base_feature_description = ReplaceFirstOccurrence(domain_base_feature_description_no_encyclopedia_tags, old_spell_name_lc + ",", new_spell_name_lc + ",");
            }
            else
            {
                new_base_feature_description = ReplaceFirstOccurrence(domain_base_feature_description_no_encyclopedia_tags, old_spell_name_lc + ".", new_spell_name_lc + ".");
            }

            domain_base_feature.Get().SetDescription(ToDContext, new_base_feature_description);

            domain_spells.Clear();
            domain_spells.Add(new_spell.ToReference<BlueprintAbilityReference>());

            new_spell.AddComponent(Helpers.Create<SpellListComponent>( c => { c.SpellLevel = level; c.m_SpellList = domain_spell_list.ToReference<BlueprintSpellListReference>(); }));

            var old_spelllist_components = old_spell.Get().GetComponents<SpellListComponent>();

            foreach (var slc in old_spelllist_components.ToArray())
            {
                if (slc.m_SpellList == domain_spell_list.ToReference<BlueprintSpellListReference>())
                {
                    old_spell.Get().RemoveComponent(slc);
                }
            }

        }

        // Holic75_PT
        public static void SetFixedResource(this BlueprintAbilityResource resource, int baseValue)
        {
            var amount = resource.m_MaxAmount;
            amount.BaseValue = baseValue;

            // Enusre arrays are at least initialized to empty.
            var emptyClasses = Array.Empty<BlueprintCharacterClassReference>();
            var emptyArchetypes = Array.Empty<BlueprintArchetypeReference>();


            if (amount.m_Class == null) amount.m_Class = emptyClasses;
            if (amount.m_ClassDiv == null) amount.m_ClassDiv = emptyClasses;
            if (amount.m_Archetypes == null) amount.m_Archetypes = emptyArchetypes;
            if (amount.m_ArchetypesDiv == null) amount.m_ArchetypesDiv = emptyArchetypes;

            resource.m_MaxAmount = amount;
        }


        #endregion

        #region |-------------------------------------------------------|  ( Abilities ) Miscellaneous Functions |---------------------------------------------------------|

        // Holic75_PT
        // This method adds variants to an existing ability variants list and then adds the parent as such in each of these new variants' Blueprints
        public static bool AddToAbilityVariants(this BlueprintAbility parent, params BlueprintAbility[] variants)
        {
            var cmp = parent.GetComponent<AbilityVariants>();

            BlueprintAbilityReference[] variants_reference = new BlueprintAbilityReference[variants.Length];

            for (int i = 0; i < variants_reference.Length; i++)
            {
                variants_reference[i] = variants[i].ToReference<BlueprintAbilityReference>();
            }

            cmp.m_Variants = cmp.m_Variants.AppendToArray(variants_reference);

            foreach (var vr in variants)
            {
                vr.m_Parent = parent.ToReference<BlueprintAbilityReference>();
            }

            return true;

        }

        // Holic75_PT
        // This method creates a wrapper for certain variants
        public static BlueprintAbility CreateVariantWrapper(string name, params BlueprintAbility[] variants)
        {
            var first_variant = variants[0];

            var wrapper = first_variant.CreateCopy(ToDContext, name, bp =>
            {

                List<BlueprintComponent> cmps = new List<BlueprintComponent>();
                cmps.Add(CreateAbilityVariants(bp, variants));
                bp.ComponentsArray = cmps.ToArray();

            });

            return wrapper;
        }

        // Holic75_SC
        public static void SetMiscAbilityParametersSingleTargetRangedHarmful(this BlueprintAbility ability,
                                                                     bool works_on_allies = false,
                                                                     Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Point)
        {
            ability.CanTargetFriends = works_on_allies;
            ability.CanTargetEnemies = true;
            ability.CanTargetSelf = false;
            ability.CanTargetPoint = false;
            ability.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
            ability.EffectOnAlly = works_on_allies ? AbilityEffectOnUnit.Harmful : AbilityEffectOnUnit.None;
            ability.Animation = animation;
        }


        // Holic75_SC
        public static void SetMiscAbilityParametersSingleTargetRangedFriendly(this BlueprintAbility ability,
                                                                              bool works_on_self = false,
                                                                              Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Point)
        {
            ability.CanTargetFriends = true;
            ability.CanTargetEnemies = false;
            ability.CanTargetSelf = works_on_self;
            ability.CanTargetPoint = false;
            ability.EffectOnEnemy = AbilityEffectOnUnit.None;
            ability.EffectOnAlly = AbilityEffectOnUnit.Helpful;
            ability.Animation = animation;
        }

        // Holic75_SC
        public static void SetMiscAbilityParametersTouchHarmful(this BlueprintAbility ability,
                                                                bool works_on_allies = true,
                                                                Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Touch)
        {
            ability.CanTargetFriends = works_on_allies;
            ability.CanTargetEnemies = true;
            ability.CanTargetSelf = works_on_allies;
            ability.CanTargetPoint = false;
            ability.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
            ability.EffectOnAlly = works_on_allies ? AbilityEffectOnUnit.Harmful : AbilityEffectOnUnit.None;
            ability.Animation = animation;

        }

        // Holic75_SC
        public static void SetMiscAbilityParametersTouchFriendly(this BlueprintAbility ability,
                                                                 bool works_on_self = true,
                                                                 Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Touch)
        {
            ability.CanTargetFriends = true;
            ability.CanTargetEnemies = false;
            ability.CanTargetSelf = works_on_self;
            ability.CanTargetPoint = false;
            ability.EffectOnEnemy = AbilityEffectOnUnit.None;
            ability.EffectOnAlly = AbilityEffectOnUnit.Helpful;
            ability.Animation = animation;

        }

        // Holic75_SC
        public static void SetMiscAbilityParametersRangedDirectional(this BlueprintAbility ability,
                                                                      bool works_on_units = true,
                                                                      AbilityEffectOnUnit effect_on_ally = AbilityEffectOnUnit.Harmful,
                                                                      AbilityEffectOnUnit effect_on_enemy = AbilityEffectOnUnit.Harmful,
                                                                      Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional)
        {
            ability.CanTargetFriends = works_on_units;
            ability.CanTargetEnemies = works_on_units;
            ability.CanTargetSelf = works_on_units;
            ability.CanTargetPoint = true;
            ability.EffectOnEnemy = effect_on_enemy;
            ability.EffectOnAlly = effect_on_ally;
            ability.Animation = animation;

        }

        // Holic75_SC
        public static void SetMiscAbilityParametersSelfOnly(this BlueprintAbility ability,
                                                            Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
        {
            ability.CanTargetFriends = false;
            ability.CanTargetEnemies = false;
            ability.CanTargetSelf = true;
            ability.CanTargetPoint = false;
            ability.EffectOnEnemy = AbilityEffectOnUnit.None;
            ability.EffectOnAlly = AbilityEffectOnUnit.Helpful;
            ability.Animation = animation;

        }


        #endregion

        #region |------------------------------------------------------|  ( Blueprints ) Miscellaneous Functions |---------------------------------------------------------|

        public static List<T> GetBlueprintsByDescription<T>(string description) where T : BlueprintUnitFact
        {
            return Kingmaker.Cheats.Utilities.GetScriptableObjects<T>().Where(b => b.Description == description).ToList();

        }

        public static List<T> GetBlueprintsByDisplayName<T>(string display_name) where T : BlueprintUnitFact
        {
            return Kingmaker.Cheats.Utilities.GetScriptableObjects<T>().Where(b => b.Name == display_name).ToList();
        }


        // Holic75_SC
        static public string ReplaceFirstOccurrence(string target_string, string old_token, string new_token)
        {
            int indx = target_string.IndexOf(old_token);
            if (indx >= 0)
            {
                return target_string.Remove(indx, old_token.Length).Insert(indx, new_token);
            }

            return target_string;

        }

        #endregion

        #region |------------------------------------------------------|  ( Combat Log ) Miscellaneous Functions |---------------------------------------------------------|

        // Bubbles_PC
        // The part on the LogThreadService was copied straight-away from BubbleBuffs, while the rest of this method is original.
        public static void CreateCustomLogMessage(LocalizedString base_text, Dictionary<string, LocalizedString> replacement_substrings, Color color, PrefixIcon icon, TooltipBaseTemplate template = null, bool hasTooltip = true)
        {
            string custom_message_text = "";

            foreach (var dictionary_item in replacement_substrings)
            {
                custom_message_text = custom_message_text.Replace(dictionary_item.Key, dictionary_item.Value.ToString());
            }
            var custom_message = new CombatLogMessage(custom_message_text, color, icon, template, hasTooltip);
            var messageLog = LogThreadService.Instance.m_Logs[LogChannelType.Common].First(x => x is MessageLogThread);
            messageLog.AddMessage(custom_message);

        }

        public static Dictionary<String, LocalizedString> CreateCustomReplacementStringDictionary(string[] replacement_keys, LocalizedString[] replacement_values)
        {
            Math.Min(replacement_keys.Length, replacement_values.Length);

            var new_replacement_dictionary = new Dictionary<String, LocalizedString>();

            foreach (var rk in replacement_keys)
            {
                new_replacement_dictionary.Add(rk, replacement_values[rk.IndexOf(rk)]);
            }
            return new_replacement_dictionary;
        }

        #endregion

        #region |-------------------------------------------------------|  ( Strings ) Miscellaneous Functions |----------------------------------------------------------|

        //Vek17_SC
        // THis was copied straight-away from Vek's TabletopTweaks Core because access issues made the original method unusable.
        public static string RemoveEncyclopediaTags(this string str)
        {
            return Regex.Replace(str, "{.*?}", string.Empty);
        }


        #endregion

    }
}
