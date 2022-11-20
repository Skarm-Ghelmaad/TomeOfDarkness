using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomeOfDarkness.NewUnitParts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Blueprints;
using static Kingmaker.UnitLogic.Parts.UnitPartConcealment;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using TomeOfDarkness.NewEnums;
using JetBrains.Annotations;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Properties;
using UnityEngine;
using Kingmaker.EntitySystem;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.EntitySystem.Stats;
using System.Linq.Expressions;
using Kingmaker.RuleSystem.Rules;
using static Kingmaker.EntitySystem.Persistence.SaveInfo;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Utility.UnitDescription;
using static System.Net.Mime.MediaTypeNames;
using Kingmaker.Blueprints.Root.Strings.GameLog;
using Kingmaker.Localization;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem;
using Kingmaker.UI.Models.Log;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using static TomeOfDarkness.NewUIStrings.CustomUIStrings;
using Kingmaker.Blueprints.Root.Strings;
using TabletopTweaks.Core.Config;
using HlEX = TomeOfDarkness.Utilities.HelpersExtension;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;
using TomeOfDarkness.NewGameLogs;




namespace TomeOfDarkness.NewComponents
{
    [TypeId("A73BDB5B66D446A4AFE2EC493653B8AB")]
    public class AddInoffensiveness : UnitBuffComponentDelegate<AddInoffensivenessData>, IUnitMakeOffensiveActionGlobalHandler, IGlobalSubscriber, IUnitSubscriber, ISubscriber
    {

        public override void OnTurnOn()
        {
            this.Owner.Ensure<UnitPartInoffensiveness>().AddUnitPartFlag();
            this.Owner.Get<UnitPartInoffensiveness>().UpdateInoffensivenessBuffTracker();
            if (this.IsEvaluationOnSavingThrow)
            {
                base.Data.Stored_DC = this.Context.Params.DC;
            }
            else if (this.IsEvaluationOnSkillCheck)
            {
                base.Data.Stored_DC = this.Check_DC.Calculate(this.Context);
            }
            else if (this.IsEvaluationOnProperty)
            {
                base.Data.Stored_PropertyThreshold = this.PropertyThreshold.Calculate(this.Context);
            }
            else if (this.IsEvaluationCustomProperty)
            {
                base.Data.Stored_CustomPropertyThreshold = this.CustomPropertyThreshold.Calculate(this.Context);
            }


        }

        public override void OnTurnOff()
        {
            if (this.IsEvaluationOnSavingThrow)
            {
                base.Data.Stored_DC = 0;
            }
            else if (this.IsEvaluationOnSkillCheck)
            {
                base.Data.Stored_DC = 0;
            }
            else if (this.IsEvaluationOnProperty)
            {
                base.Data.Stored_PropertyThreshold = 0;
            }
            else if (this.IsEvaluationCustomProperty)
            {
                base.Data.Stored_CustomPropertyThreshold = 0;
            }
            this.Owner.Ensure<UnitPartInoffensiveness>().UpdateInoffensivenessBuffTracker();

            if (this.Owner.Get<UnitPartInoffensiveness>().IsInoffensivenessBuffTrackerEmpty)
            {
               this.Owner.Get<UnitPartInoffensiveness>().RemoveUnitPartFlag();
            }
 
        }

        public BlueprintBuff MarkingBuff
        {
            get
            {
                BlueprintBuffReference buff = this.m_MarkingBuff;
                if (buff == null)
                {
                    return null;
                }
                return buff.Get();
            }
        }

        public void AddMarkingBuff(UnitEntityData unit)
        {
            if ((!base.Data.Marked_Units.Any() || !base.Data.Marked_Units.Contains(unit)) && !unit.HasFact(this.MarkingBuff))
            {
                unit.Buffs.AddBuff(this.MarkingBuff, null, null, null);
                base.Data.Marked_Units.Add(unit);
                return;
            }
        }

        public void RemoveMarkingBuff(UnitEntityData unit)
        {
            if ((base.Data.Marked_Units.Any() || base.Data.Marked_Units.Contains(unit)) && unit.HasFact(this.MarkingBuff))
            {
                unit.Buffs.RemoveFact(this.MarkingBuff);
                base.Data.Marked_Units.Remove(unit);
                return;
            }
        }

        public void RemoveAllMarkingBuffs()
        {
            if (base.Data.Marked_Units.Any())
            {
                foreach (var unit in base.Data.Marked_Units)
                {
                    if (!unit.IsDisposed && unit.HasFact(this.MarkingBuff))
                    {
                        unit.Buffs.RemoveFact(this.MarkingBuff);
                        base.Data.Marked_Units.Remove(unit);
                    }
                    else
                    {
                        base.Data.Marked_Units.Remove(unit);
                    }
                }
            }
        }

        public bool CanBeAttackedBy(UnitEntityData unit)
        {

            if (base.Data.Can_Attack.Contains(unit))
            {
                return true;
            }
            if (base.Data.Cannot_Attack.Contains(unit))
            {
                return false;
            }
            var buff_spell_descriptor = this.Buff.Blueprint.GetComponent<SpellDescriptorComponent>();
            if (buff_spell_descriptor != null && (UnitDescriptionHelper.GetDescription(unit.Blueprint).Immunities.SpellDescriptorImmunity.Value & buff_spell_descriptor.Descriptor.Value) != 0)
            {
                this.CreateInoffensivenessLogMessage(InoffensivenessMessageType.Immunity, unit);
                base.Data.Can_Attack.Add(unit);
                if (this.HasMarkingBuff)
                {
                    this.RemoveMarkingBuff(unit);
                }
                return true;
            }
      
            if (this.CanBypassInoffensiveness(unit) == true)
            {
                this.CreateInoffensivenessLogMessage(InoffensivenessMessageType.BypassSucceess, unit);
                base.Data.Can_Attack.Add(unit);
                if (this.HasMarkingBuff)
                {
                    this.RemoveMarkingBuff(unit);
                }
                return true;
            }
            else 
            {
                this.CreateInoffensivenessLogMessage(InoffensivenessMessageType.BypassFailure, unit);
                base.Data.Cannot_Attack.Add(unit);
                if (this.HasMarkingBuff)
                {
                    this.AddMarkingBuff(unit);
                }

                return true;
            }

        }

        // If the ReverseCheck is set to "false", the inoffensiveness effect cannot be bypassed if the saving throw or skill check is passed,
        // if the buff is either on the owner of this buff or on the "target" of this buff or if the chosen property is higher or equal to the threshold. 
        // As a safety measure, the default result if no InoffensivenessEvaluationType was set is "true", so the unit can be attacked.
        public bool CanBypassInoffensiveness(UnitEntityData target)
        {
            switch (this.Type)
            {
                case InoffensivenessEvaluationType.SavingThrow:
                    RuleSavingThrow ruleSavingThrow = this.Context.TriggerRule<RuleSavingThrow>(new RuleSavingThrow(target, this.m_SavingThrowType, base.Data.Stored_DC));
                    return  ruleSavingThrow.IsPassed && !this.ReverseCheck ;

                case InoffensivenessEvaluationType.SkillCheck:
                    RuleSkillCheck ruleSkillCheck = this.Context.TriggerRule<RuleSkillCheck>(new RuleSkillCheck(target, this.m_StatStype, base.Data.Stored_DC));
                    return  ruleSkillCheck.Success && !this.ReverseCheck;

                case InoffensivenessEvaluationType.OwnerBuff:
                    return  this.Owner.Buffs.HasFact(this.m_Buff.Get()) && !this.ReverseCheck ;

                case InoffensivenessEvaluationType.TargetBuff:
                    return  target.Buffs.HasFact(this.m_Buff.Get()) && !this.ReverseCheck;

                case InoffensivenessEvaluationType.OwnerProperty:
                    return (this.Property.GetInt(this.Owner) >= base.Data.Stored_PropertyThreshold) && !this.ReverseCheck;

                case InoffensivenessEvaluationType.TargetProperty:
                    return (this.Property.GetInt(target) >= base.Data.Stored_PropertyThreshold) && !this.ReverseCheck;

                case InoffensivenessEvaluationType.OwnerCustomProperty:
                    return (this.CustomProperty.GetInt(this.Owner) >= base.Data.Stored_CustomPropertyThreshold) && !this.ReverseCheck;

                case InoffensivenessEvaluationType.TargetCustomProperty:
                    return (this.CustomProperty.GetInt(target) >= base.Data.Stored_CustomPropertyThreshold) && !this.ReverseCheck;

                default:
                    return !this.ReverseCheck;
            }


        }

        public void HandleUnitMakeOffensiveAction(UnitEntityData unit, UnitEntityData target)
        {
            if (unit == this.Owner)
            {
                if (this.Offensive_Action_Effect == OffensiveActionEffect.REMOVE_FROM_OWNER)
                {
                    if (this.HasMarkingBuff)
                    {
                        this.RemoveAllMarkingBuffs();
                    }
                    this.Buff.Remove();

                }
                else if (this.Offensive_Action_Effect == OffensiveActionEffect.REMOVE_FROM_TARGET && !base.Data.Can_Attack.Contains(target))
                {
                    this.CreateInoffensivenessLogMessage(InoffensivenessMessageType.Invalidation, target);
                    base.Data.Can_Attack.Add(target);
                    base.Data.Cannot_Attack.Remove(target);
                    if (this.HasMarkingBuff)
                    {
                        this.RemoveMarkingBuff(target);
                    }
                }
            }
            

        }

        // IEntityRevealedHandler,
        //public void HandleEntityRevealed(EntityDataBase entity)
        //{
        //    UnitEntityData revealed_entity = entity as UnitEntityData;
        //    UnitEntityData caster = this.Owner;

        //    if (entity != null && revealed_entity != caster && revealed_entity.IsPlayersEnemy)
        //    {
        //        this.CanBeAttackedBy(revealed_entity);
        //    }
        //}

        public void CreateInoffensivenessLogMessage(InoffensivenessMessageType message_type, UnitEntityData target)
        {
            var caster = this.Owner;
            var text = this.Fact.Name;
            var color = new Color32();
            LocalizedString message;

            switch (message_type)
            {
                case InoffensivenessMessageType.Immunity:
                    message = CustomFactImmunity;
                    color = GameLogStrings.Instance.DefaultColor;
                    break;
                case InoffensivenessMessageType.BypassSucceess:
                    message = CustomFactBypassSuccess;
                    color = GameLogStrings.Instance.DefaultColor;
                    break;
                case InoffensivenessMessageType.BypassFailure:
                    message = CustomFactBypassFailure;
                    color = GameLogStrings.Instance.DefaultColor;
                    break;
                case InoffensivenessMessageType.Invalidation:
                    message = CustomFactInvalidation;
                    color = GameLogStrings.Instance.DefaultColor;
                    break;
                default:
                    message = CustomFactBypassFailure;
                    color = GameLogStrings.Instance.DefaultColor;
                    break;
            }

            var custom_message = SimpleCombatLogMessage.GenerateSimpleCombatLogMessage(message, color, caster, target, text, "", "");
            SimpleCombatLogMessage.SendSimpleCombatLogMessage(custom_message);

            return;

        }

        [UsedImplicitly]
        public bool IsEvaluationOnSavingThrow
        {
            get
            {
                return this.Type == InoffensivenessEvaluationType.SavingThrow;
            }
        }

        [UsedImplicitly]
        public bool IsEvaluationOnSkillCheck
        {
            get
            {
                return this.Type == InoffensivenessEvaluationType.SkillCheck;
            }
        }

        [UsedImplicitly]
        public bool IsEvaluationOnBuff
        {
            get
            {
                return this.Type == InoffensivenessEvaluationType.OwnerBuff || this.Type == InoffensivenessEvaluationType.TargetBuff;
            }
        }

        [UsedImplicitly]
        public bool IsEvaluationOnProperty
        {
            get
            {
                return this.Type == InoffensivenessEvaluationType.OwnerProperty || this.Type == InoffensivenessEvaluationType.TargetProperty;
            }
        }

        [UsedImplicitly]
        public bool IsEvaluationCustomProperty
        {
            get
            {
                return this.Type == InoffensivenessEvaluationType.OwnerCustomProperty || this.Type == InoffensivenessEvaluationType.TargetCustomProperty;
            }
        }

        [UsedImplicitly]
        public bool IsBasedOnRoll
        {
            get
            {
                return this.Type == InoffensivenessEvaluationType.SavingThrow || this.Type == InoffensivenessEvaluationType.SkillCheck;
            }
        }

        public OffensiveActionEffect Offensive_Action_Effect;

        public InoffensivenessEvaluationType Type = InoffensivenessEvaluationType.SavingThrow;

        public bool HasMarkingBuff = false;

        [HideInInspector]
        [ShowIf("IsEvaluationOnSavingThrow")]
        public SavingThrowType m_SavingThrowType = SavingThrowType.Will;

        [HideInInspector]
        [ShowIf("IsEvaluationOnSkillCheck")]
        public StatType m_StatStype;

        [HideInInspector]
        [ShowIf("IsEvaluationOnSkillCheck")]
        public ContextValue Check_DC;

        [HideInInspector]
        [ShowIf("IsEvaluationOnBuff")]
        [SerializeField]
        public BlueprintBuffReference m_Buff;

        [HideInInspector]
        [ShowIf("IsEvaluationOnProperty")]
        public UnitProperty Property;

        [ShowIf("IsEvaluationOnProperty")]
        public ContextValue PropertyThreshold;

        [HideInInspector]
        [ShowIf("IsEvaluationCustomProperty")]
        public BlueprintUnitProperty CustomProperty;

        [ShowIf("IsEvaluationCustomProperty")]
        public ContextValue CustomPropertyThreshold;

        [ShowIf("HasMarkingBuff")]
        [SerializeField]
        public BlueprintBuffReference m_MarkingBuff;

        public bool ReverseCheck = false;

        public enum InoffensivenessMessageType
        {
            Immunity = 0,
            BypassSucceess = 1,
            BypassFailure = 2,
            Invalidation = 3

        }



    }
}
