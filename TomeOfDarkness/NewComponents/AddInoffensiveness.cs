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
    public class AddInoffensiveness : UnitBuffComponentDelegate, IUnitMakeOffensiveActionHandler, IUnitSubscriber
    {

        public override void OnTurnOn()
        {
            this.Owner.Ensure<UnitPartInoffensiveness>().AddUnitPartFlag();

        }

        public override void OnTurnOff()
        {
            this.Owner.Ensure<UnitPartInoffensiveness>().RemoveUnitPartFlag();

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
            if ((!this.Marked_Units.Any() || !this.Marked_Units.Contains(unit)) && !unit.HasFact(this.MarkingBuff))
            {
                unit.Buffs.AddBuff(this.MarkingBuff, null, null, null);
                this.Marked_Units.Add(unit);
                return;
            }
        }

        public void RemoveMarkingBuff(UnitEntityData unit)
        {
            if ((this.Marked_Units.Any() || this.Marked_Units.Contains(unit)) && (unit.HasFact(this.MarkingBuff)))
            {
                unit.Buffs.RemoveFact(this.MarkingBuff);
                this.Marked_Units.Remove(unit);
                return;
            }
        }

        public void RemoveAllMarkingBuffs()
        {
            if (this.Marked_Units.Any())
            {
                foreach (var unit in this.Marked_Units)
                {
                    if (!unit.IsDisposed && unit.HasFact(this.MarkingBuff))
                    {
                        unit.Buffs.RemoveFact(this.MarkingBuff);
                        this.Marked_Units.Remove(unit);
                    }
                    else
                    {
                        this.Marked_Units.Remove(unit);
                    }
                }
            }
        }

        public bool CanBeAttackedBy(UnitEntityData unit)
        {

            if (this.Can_Attack.Contains(unit))
            {
                return true;
            }
            if (this.Cannot_Attack.Contains(unit))
            {
                return false;
            }
            var buff_spell_descriptor = this.Buff.Blueprint.GetComponent<SpellDescriptorComponent>();
            if (buff_spell_descriptor != null && (UnitDescriptionHelper.GetDescription(unit.Blueprint).Immunities.SpellDescriptorImmunity.Value & buff_spell_descriptor.Descriptor.Value) != 0)
            {
                this.CreateInoffensivenessLogMessage(InoffensivenessMessageType.Immunity, unit);
                this.Can_Attack.Add(unit);
                if (this.HasMarkingBuff)
                {
                    this.RemoveMarkingBuff(unit);
                }
                return true;
            }
      
            if (this.CanBypassInoffensiveness(unit) == true)
            {
                this.Can_Attack.Add(unit);
                if (this.HasMarkingBuff)
                {
                    this.RemoveMarkingBuff(unit);
                }
                this.CreateInoffensivenessLogMessage(InoffensivenessMessageType.BypassSucceess, unit);
                return true;
            }
            else
            {
                this.Cannot_Attack.Add(unit);
                if (this.HasMarkingBuff)
                {
                    this.AddMarkingBuff(unit);
                }
                this.CreateInoffensivenessLogMessage(InoffensivenessMessageType.BypassFailure, unit);
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
                    RuleSavingThrow ruleSavingThrow = this.Context.TriggerRule<RuleSavingThrow>(new RuleSavingThrow(target, this.m_SavingThrowType, this.DC.Calculate(this.Context)));
                    return ( ruleSavingThrow.IsPassed && !this.ReverseCheck );

                case InoffensivenessEvaluationType.SkillCheck:
                    RuleSkillCheck ruleSkillCheck = this.Context.TriggerRule<RuleSkillCheck>(new RuleSkillCheck(target, this.m_StatStype, this.DC.Calculate(this.Context)));
                    return ( ruleSkillCheck.Success && !this.ReverseCheck);

                case InoffensivenessEvaluationType.OwnerBuff:
                    return ( this.Owner.Buffs.HasFact(this.m_Buff.Get()) && !this.ReverseCheck );

                case InoffensivenessEvaluationType.TargetBuff:
                    return ( target.Buffs.HasFact(this.m_Buff.Get()) && !this.ReverseCheck);

                case InoffensivenessEvaluationType.OwnerProperty:
                    int op_threshold = this.PropertyThreshold.Calculate(this.Context);
                    return ((this.Property.GetInt(this.Owner) >= op_threshold) && !this.ReverseCheck);

                case InoffensivenessEvaluationType.TargetProperty:
                    int tp_threshold = this.PropertyThreshold.Calculate(this.Context);
                    return ((this.Property.GetInt(target) >= tp_threshold) && !this.ReverseCheck);

                case InoffensivenessEvaluationType.OwnerCustomProperty:
                    int cop_threshold = this.CustomPropertyThreshold.Calculate(this.Context);
                    return ((this.CustomProperty.GetInt(this.Owner) >= cop_threshold) && !this.ReverseCheck);

                case InoffensivenessEvaluationType.TargetCustomProperty:
                    int ctp_threshold = this.CustomPropertyThreshold.Calculate(this.Context);
                    return ((this.CustomProperty.GetInt(target) >= ctp_threshold) && !this.ReverseCheck);

                default:
                    return !this.ReverseCheck;
            }


        }

        public void HandleUnitMakeOffensiveAction(UnitEntityData target)
        {
            if (this.Offensive_Action_Effect == OffensiveActionEffect.REMOVE_FROM_OWNER)
            {
                if (this.HasMarkingBuff)
                {
                    this.RemoveAllMarkingBuffs();
                }
                this.Buff.Remove();

            }
            else if (this.Offensive_Action_Effect == OffensiveActionEffect.REMOVE_FROM_TARGET && !this.Can_Attack.Contains(target))
            {

                this.Can_Attack.Add(target);
                this.Cannot_Attack.Remove(target);
                if (this.HasMarkingBuff)
                {
                    this.RemoveMarkingBuff(target);
                }
                this.CreateInoffensivenessLogMessage(InoffensivenessMessageType.Invalidation, target);
            }
        }

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
                    color = Color.red;
                    break;
                case InoffensivenessMessageType.BypassSucceess:
                    message = CustomFactBypassSuccess;
                    color = Color.red;
                    break;
                case InoffensivenessMessageType.BypassFailure:
                    message = CustomFactBypassFailure;
                    color = Color.green;
                    break;
                case InoffensivenessMessageType.Invalidation:
                    message = CustomFactInvalidation;
                    color = Color.magenta;
                    break;
                default:
                    message = CustomFactBypassFailure;
                    color = Color.green;
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

        private List<UnitEntityData> Cannot_Attack = new List<UnitEntityData>();

        private List<UnitEntityData> Can_Attack = new List<UnitEntityData>();

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
        [ShowIf("IsBasedOnRoll")]
        public ContextValue DC;

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

        [ShowIf("HasMarkingBuff")]
        private List<UnitEntityData> Marked_Units = new List<UnitEntityData>();

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
