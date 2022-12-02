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
using static TomeOfDarkness.NewGameLogs.SimpleCombatLogMessage;
using Kingmaker.Designers;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;





namespace TomeOfDarkness.NewComponents
{
    [TypeId("A73BDB5B66D446A4AFE2EC493653B8AB")]
    public class AddInoffensiveness : UnitBuffComponentDelegate<AddInoffensivenessData>, IEntityRevealedHandler, IUnitMakeOffensiveActionGlobalHandler, IGlobalSubscriber, IUnitSubscriber, ISubscriber
    {

        public BlueprintBuff MinorMarkingBuff
        {
            get
            {
                BlueprintBuffReference buff = this.m_MinorMarkingBuff;
                if (buff == null)
                {
                    return null;
                }
                return buff.Get();
            }
        }

        public BlueprintBuff ModerateMarkingBuff
        {
            get
            {
                BlueprintBuffReference buff = this.m_ModerateMarkingBuff;
                if (buff == null)
                {
                    return null;
                }
                return buff.Get();
            }
        }

        public BlueprintBuff MajorMarkingBuff
        {
            get
            {
                BlueprintBuffReference buff = this.m_MajorMarkingBuff;
                if (buff == null)
                {
                    return null;
                }
                return buff.Get();
            }
        }

        public override void OnTurnOn()
        {
            this.Owner.Ensure<UnitPartInoffensiveness>().AddUnitPartFlag();
            this.Owner.Get<UnitPartInoffensiveness>().AddEntry(this.Fact, this);
            if (this.IsEvaluationOnSavingThrow)
            {
                base.Data.Stored_DC = this.UseCustomDC ? this.CustomDC.Calculate(this.Context) : this.Context.Params.DC;
            }
            else if (this.IsEvaluationOnSkillCheck)
            {
                base.Data.Stored_DC = this.UseCustomDC ? this.CustomDC.Calculate(this.Context) : this.Context.Params.DC;
            }
            else if (this.IsEvaluationOnProperty || this.IsEvaluationCustomProperty)
            {
                base.Data.Stored_DC = this.UseCustomDC ? this.CustomDC.Calculate(this.Context) : 10;
            }
            var visible_enemy_units = this.Owner.Memory.Enemies;
            var total_enemy_units = new List<UnitGroupMemory.UnitInfo>();
            var checked_enemy_group = new List<string>();

            foreach (var enemy in visible_enemy_units)
            {
                total_enemy_units.Add(enemy);
                var enemy_group = enemy.UnitReference.Value.Memory.UnitsList;
                var enemy_group_ID = enemy.UnitReference.Value.Memory.GroupId;

                if (!checked_enemy_group.Any() && !checked_enemy_group.Contains(enemy_group_ID))
                {
                    foreach (var enemy_ally in enemy_group)
                    {
                        if (!visible_enemy_units.Contains(enemy_ally))
                        {
                            total_enemy_units.Add((UnitGroupMemory.UnitInfo)enemy_ally); 
                        }
                    }
                }
            }

            foreach (var enemy in total_enemy_units)
            {
                this.CanBeAttackedBy(enemy.UnitReference.Value);
            }
            visible_enemy_units.Clear();
            total_enemy_units.Clear();
            checked_enemy_group.Clear();

        }

        public override void OnTurnOff()
        {

            if (this.IsEvaluationOnSavingThrow || this.IsEvaluationOnSkillCheck || this.IsEvaluationOnProperty || this.IsEvaluationCustomProperty)
            {
                base.Data.Stored_DC = 0;
            }
            this.Owner.Ensure<UnitPartInoffensiveness>().RemoveEntry(this.Fact, this);
        }

        public void AddMarkingBuff(UnitEntityData unit, MarkingBuffType type)
        {
            if (type == MarkingBuffType.MajorBuff)
            {
                if ((!base.Data.Marked_Units_Major.Any() || !base.Data.Marked_Units_Major.Contains(unit)) && !unit.HasFact(this.MajorMarkingBuff))
                {
                    unit.Buffs.AddBuff(this.MajorMarkingBuff, null, null, null);
                    base.Data.Marked_Units_Major.Add(unit);
                    return;
                }
            }
            else if (type == MarkingBuffType.ModerateBuff)
            {
                if ((!base.Data.Marked_Units_Moderate.Any() || !base.Data.Marked_Units_Moderate.Contains(unit)) && !unit.HasFact(this.ModerateMarkingBuff))
                {
                    unit.Buffs.AddBuff(this.ModerateMarkingBuff, null, null, null);
                    base.Data.Marked_Units_Moderate.Add(unit);
                    return;
                } 
            }
            else 
            {
                if ((!base.Data.Marked_Units_Minor.Any() || !base.Data.Marked_Units_Minor.Contains(unit)) && !unit.HasFact(this.MinorMarkingBuff))
                {
                    unit.Buffs.AddBuff(this.MinorMarkingBuff, null, null, null);
                    base.Data.Marked_Units_Minor.Add(unit);
                    return;
                }
            }
        }

        public void RemoveMarkingBuff(UnitEntityData unit, MarkingBuffType type)
        {
            if (type == MarkingBuffType.MajorBuff)
            {
                if ((base.Data.Marked_Units_Major.Any() || base.Data.Marked_Units_Major.Contains(unit)) && unit.HasFact(this.MajorMarkingBuff))
                {
                    unit.Buffs.RemoveFact(this.MajorMarkingBuff);
                    base.Data.Marked_Units_Major.Remove(unit);
                    return;
                }
            }
            else if (type == MarkingBuffType.ModerateBuff)
            {
                if ((base.Data.Marked_Units_Moderate.Any() || base.Data.Marked_Units_Moderate.Contains(unit)) && unit.HasFact(this.ModerateMarkingBuff))
                {
                    unit.Buffs.RemoveFact(this.ModerateMarkingBuff);
                    base.Data.Marked_Units_Moderate.Remove(unit);
                    return;
                }
            }
            else
            {
                if ((base.Data.Marked_Units_Minor.Any() || base.Data.Marked_Units_Minor.Contains(unit)) && unit.HasFact(this.MinorMarkingBuff))
                {
                    unit.Buffs.RemoveFact(this.MinorMarkingBuff);
                    base.Data.Marked_Units_Minor.Remove(unit);
                    return;
                }
            }
        }

        public void RemoveAllMarkingBuffs()
        {
            if (this.HasAdvancedMarkingBuff && base.Data.Marked_Units_Major.Any())
            {
                foreach (var unit in base.Data.Marked_Units_Major)
                {
                    if (!unit.IsDisposed && unit.HasFact(this.MajorMarkingBuff))
                    {
                        unit.Buffs.RemoveFact(this.MajorMarkingBuff);
                        base.Data.Marked_Units_Major.Remove(unit);
                    }
                    else
                    {
                        base.Data.Marked_Units_Major.Remove(unit);
                    }
                }
            }
            else if (this.HasAdvancedMarkingBuff && base.Data.Marked_Units_Moderate.Any())
            {
                foreach (var unit in base.Data.Marked_Units_Moderate)
                {
                    if (!unit.IsDisposed && unit.HasFact(this.ModerateMarkingBuff))
                    {
                        unit.Buffs.RemoveFact(this.ModerateMarkingBuff);
                        base.Data.Marked_Units_Moderate.Remove(unit);
                    }
                    else
                    {
                        base.Data.Marked_Units_Moderate.Remove(unit);
                    }
                }
            }
            else
            {
                foreach (var unit in base.Data.Marked_Units_Minor)
                {
                    if (!unit.IsDisposed && unit.HasFact(this.MinorMarkingBuff))
                    {
                        unit.Buffs.RemoveFact(this.MinorMarkingBuff);
                        base.Data.Marked_Units_Minor.Remove(unit);
                    }
                    else
                    {
                        base.Data.Marked_Units_Minor.Remove(unit);
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
                if (this.HasAdvancedMarkingBuff)
                {
                    this.RemoveMarkingBuff(unit,MarkingBuffType.MajorBuff);
                    this.RemoveMarkingBuff(unit, MarkingBuffType.ModerateBuff);
                    this.RemoveMarkingBuff(unit, MarkingBuffType.MinorBuff);
                }
                else if (this.HasMarkingBuff)
                {
                    this.RemoveMarkingBuff(unit, MarkingBuffType.MinorBuff);
                }
                return true;
            }

            if (this.CanBypassInoffensiveness(unit) == true)
            {
                this.CreateInoffensivenessLogMessage(InoffensivenessMessageType.BypassSucceess, unit);
                base.Data.Can_Attack.Add(unit);
                if (this.HasAdvancedMarkingBuff)
                {
                    this.RemoveMarkingBuff(unit,MarkingBuffType.MajorBuff);
                    this.RemoveMarkingBuff(unit, MarkingBuffType.ModerateBuff);
                    this.RemoveMarkingBuff(unit, MarkingBuffType.MinorBuff);
                }
                else if (this.HasMarkingBuff)
                {
                    this.RemoveMarkingBuff(unit, MarkingBuffType.MinorBuff);
                }
                return true;
            }
            else
            {
                this.CreateInoffensivenessLogMessage(InoffensivenessMessageType.BypassFailure, unit);
                base.Data.Cannot_Attack.Add(unit);
                return true;
            }

        }

        public bool CanBypassInoffensiveness(UnitEntityData target)
        {
            int altered_DC = 0;

            switch (this.Type)
            {
                case InoffensivenessEvaluationType.SavingThrow:

                    altered_DC = base.Data.Stored_DC;
                    if (this.UseDCAdjustingFacts && this.m_FactDCModification != null)
                    {
                        foreach (AddInoffensiveness.FactDCModification conditionalDCIncreaseEntry in this.m_FactDCModification)
                        {
                            if (target.HasFact(conditionalDCIncreaseEntry.Fact))
                            {
                                altered_DC += conditionalDCIncreaseEntry.Value;
                            }
                        }
                    }
                    RuleSavingThrow ruleSavingThrow = this.Context.TriggerRule<RuleSavingThrow>(new RuleSavingThrow(target, this.m_SavingThrowType, altered_DC));

                    if (this.HasAdvancedMarkingBuff)
                    {
                        if (!this.ReverseCheck)
                        {
                            if (ruleSavingThrow.Success)
                            {
                                if (altered_DC - ruleSavingThrow.RollResult >= 0 && altered_DC - ruleSavingThrow.RollResult < 5)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                    return true;
                                }
                                if (altered_DC - ruleSavingThrow.RollResult >= 5 && altered_DC - ruleSavingThrow.RollResult < 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.ModerateBuff);
                                    return true;
                                }
                                if (altered_DC - ruleSavingThrow.RollResult >= 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MajorBuff);
                                    return true;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (!ruleSavingThrow.Success)
                            {
                                if (ruleSavingThrow.RollResult - altered_DC >= 0 && ruleSavingThrow.RollResult - altered_DC < 5)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                    return true;
                                }
                                if (ruleSavingThrow.RollResult - altered_DC >= 5 && ruleSavingThrow.RollResult - altered_DC < 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.ModerateBuff);
                                    return true;
                                }
                                if (ruleSavingThrow.RollResult - altered_DC >= 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MajorBuff);
                                    return true;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else if (this.HasMarkingBuff)
                    {
                        if (!this.ReverseCheck)
                        {
                            if (ruleSavingThrow.Success)
                            {
                                this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (!ruleSavingThrow.Success)
                            {
                                this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }

                    return ruleSavingThrow.Success && !this.ReverseCheck;

                case InoffensivenessEvaluationType.SkillCheck:
                    
                    altered_DC = base.Data.Stored_DC;
                    if (this.UseDCAdjustingFacts && this.m_FactDCModification != null)
                    {
                        foreach (AddInoffensiveness.FactDCModification conditionalDCIncreaseEntry in this.m_FactDCModification)
                        {
                            if (target.HasFact(conditionalDCIncreaseEntry.Fact))
                            {
                                altered_DC += conditionalDCIncreaseEntry.Value;
                            }
                        }
                    }
                    RuleStatCheck ruleStatCheck = RuleStatCheck.Create(target, this.m_StatStype, altered_DC);
                    ruleStatCheck.ShowAnyway = true;
                    RuleStatCheck ruleStatCheck2 = GameHelper.TriggerStatCheck(ruleStatCheck, base.Context, true);

                    if (this.HasAdvancedMarkingBuff)
                    {
                        if (!this.ReverseCheck)
                        {
                            if (ruleStatCheck2.Success)
                            {
                                if (altered_DC - ruleStatCheck2.RollResult >= 0 && altered_DC - ruleStatCheck2.RollResult < 5)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                    return true;
                                }
                                if (altered_DC - ruleStatCheck2.RollResult >= 5 && altered_DC - ruleStatCheck2.RollResult < 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.ModerateBuff);
                                    return true;
                                }
                                if (altered_DC - ruleStatCheck2.RollResult >= 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MajorBuff);
                                    return true;
                                }
                            } 
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (!ruleStatCheck2.Success)
                            {
                                if (ruleStatCheck2.RollResult - altered_DC >= 0 && ruleStatCheck2.RollResult - altered_DC < 5)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                    return true;
                                }
                                if (ruleStatCheck2.RollResult - altered_DC >= 5 && ruleStatCheck2.RollResult - altered_DC < 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.ModerateBuff);
                                    return true;
                                }
                                if (ruleStatCheck2.RollResult - altered_DC >= 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MajorBuff);
                                    return true;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else if (this.HasMarkingBuff)
                    {
                        if (!this.ReverseCheck)
                        {
                            if (ruleStatCheck2.Success)
                            {
                                this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (!ruleStatCheck2.Success)
                            {
                                this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }

                    return ruleStatCheck2.Success && !this.ReverseCheck;

                case InoffensivenessEvaluationType.OwnerBuff:

                    var owner_has_buff = target.Buffs.HasFact(this.m_Buff.Get());

                    if (this.HasMarkingBuff)
                    {
                        if (owner_has_buff && !this.ReverseCheck)
                        {
                            this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                            return true;
                        }
                        else 
                        { 
                            return false; 
                        }

                    }
                    else
                    {
                        if (owner_has_buff && !this.ReverseCheck)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }

                case InoffensivenessEvaluationType.TargetBuff:


                    var target_has_buff = target.Buffs.HasFact(this.m_Buff.Get());

                    if (this.HasMarkingBuff)
                    {
                        if (target_has_buff && !this.ReverseCheck)
                        {
                            this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                    else
                    {
                        if (target_has_buff && !this.ReverseCheck)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }

                case InoffensivenessEvaluationType.OwnerProperty:
                    altered_DC = base.Data.Stored_DC;
                    if (this.UseDCAdjustingFacts && this.m_FactDCModification != null)
                    {
                        foreach (AddInoffensiveness.FactDCModification conditionalDCIncreaseEntry in this.m_FactDCModification)
                        {
                            if (target.HasFact(conditionalDCIncreaseEntry.Fact))
                            {
                                altered_DC += conditionalDCIncreaseEntry.Value;
                            }
                        }
                    }
                    int owner_property_value = this.Property.GetInt(this.Owner);

                    if (this.HasAdvancedMarkingBuff)
                    {
                        if (!this.ReverseCheck)
                        {
                            if (owner_property_value >= altered_DC)
                            {
                                if (altered_DC - owner_property_value >= 0 && altered_DC - owner_property_value < 5)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                    return true;
                                }
                                if (altered_DC - owner_property_value >= 5 && altered_DC - altered_DC - owner_property_value < 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.ModerateBuff);
                                    return true;
                                }
                                if (altered_DC - owner_property_value >= 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MajorBuff);
                                    return true;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (owner_property_value < altered_DC)
                            {
                                if (owner_property_value - altered_DC > 0 && owner_property_value - altered_DC < 5)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                    return true;
                                }
                                if (owner_property_value - altered_DC >= 5 && altered_DC - owner_property_value - altered_DC < 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.ModerateBuff);
                                    return true;
                                }
                                if (owner_property_value - altered_DC >= 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MajorBuff);
                                    return true;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else if (this.HasMarkingBuff)
                    {
                        if (!this.ReverseCheck)
                        {
                            if (owner_property_value >= altered_DC)
                            {
                                this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (owner_property_value < altered_DC)
                            {
                                this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    return (owner_property_value >= altered_DC) && !this.ReverseCheck;

                case InoffensivenessEvaluationType.TargetProperty:
                    altered_DC = base.Data.Stored_DC;
                    if (this.UseDCAdjustingFacts && this.m_FactDCModification != null)
                    {
                        foreach (AddInoffensiveness.FactDCModification conditionalDCIncreaseEntry in this.m_FactDCModification)
                        {
                            if (target.HasFact(conditionalDCIncreaseEntry.Fact))
                            {
                                altered_DC += conditionalDCIncreaseEntry.Value;
                            }
                        }
                    }
                    int target_property_value = this.Property.GetInt(target);

                    if (this.HasAdvancedMarkingBuff)
                    {
                        if (!this.ReverseCheck)
                        {
                            if (target_property_value >= altered_DC)
                            {
                                if (altered_DC - target_property_value >= 0 && altered_DC - target_property_value < 5)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                    return true;
                                }
                                if (altered_DC - target_property_value >= 5 && altered_DC - altered_DC - target_property_value < 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.ModerateBuff);
                                    return true;
                                }
                                if (altered_DC - target_property_value >= 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MajorBuff);
                                    return true;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (target_property_value < altered_DC)
                            {
                                if (target_property_value - altered_DC > 0 && target_property_value - altered_DC < 5)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                    return true;
                                }
                                if (target_property_value - altered_DC >= 5 && altered_DC - target_property_value - altered_DC < 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.ModerateBuff);
                                    return true;
                                }
                                if (target_property_value - altered_DC >= 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MajorBuff);
                                    return true;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else if (this.HasMarkingBuff)
                    {
                        if (!this.ReverseCheck)
                        {
                            if (target_property_value >= altered_DC)
                            {
                                this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (target_property_value < altered_DC)
                            {
                                this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    return (target_property_value >= altered_DC) && !this.ReverseCheck;

                case InoffensivenessEvaluationType.OwnerCustomProperty:
                    altered_DC = base.Data.Stored_DC;
                    if (this.UseDCAdjustingFacts && this.m_FactDCModification != null)
                    {
                        foreach (AddInoffensiveness.FactDCModification conditionalDCIncreaseEntry in this.m_FactDCModification)
                        {
                            if (target.HasFact(conditionalDCIncreaseEntry.Fact))
                            {
                                altered_DC += conditionalDCIncreaseEntry.Value;
                            }
                        }
                    }
                    int owner_custom_property_value = this.CustomProperty.GetInt(this.Owner);

                    if (this.HasAdvancedMarkingBuff)
                    {
                        if (!this.ReverseCheck)
                        {
                            if (owner_custom_property_value >= altered_DC)
                            {
                                if (altered_DC - owner_custom_property_value >= 0 && altered_DC - owner_custom_property_value < 5)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                    return true;
                                }
                                if (altered_DC - owner_custom_property_value >= 5 && altered_DC - altered_DC - owner_custom_property_value < 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.ModerateBuff);
                                    return true;
                                }
                                if (altered_DC - owner_custom_property_value >= 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MajorBuff);
                                    return true;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (owner_custom_property_value < altered_DC)
                            {
                                if (owner_custom_property_value - altered_DC > 0 && owner_custom_property_value - altered_DC < 5)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                    return true;
                                }
                                if (owner_custom_property_value - altered_DC >= 5 && altered_DC - owner_custom_property_value - altered_DC < 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.ModerateBuff);
                                    return true;
                                }
                                if (owner_custom_property_value - altered_DC >= 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MajorBuff);
                                    return true;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else if (this.HasMarkingBuff)
                    {
                        if (!this.ReverseCheck)
                        {
                            if (owner_custom_property_value >= altered_DC)
                            {
                                this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (owner_custom_property_value < altered_DC)
                            {
                                this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    return (owner_custom_property_value >= altered_DC) && !this.ReverseCheck;

                case InoffensivenessEvaluationType.TargetCustomProperty:
                    altered_DC = base.Data.Stored_DC;
                    if (this.UseDCAdjustingFacts && this.m_FactDCModification != null)
                    {
                        foreach (AddInoffensiveness.FactDCModification conditionalDCIncreaseEntry in this.m_FactDCModification)
                        {
                            if (target.HasFact(conditionalDCIncreaseEntry.Fact))
                            {
                                altered_DC += conditionalDCIncreaseEntry.Value;
                            }
                        }
                    }
                    int target_custom_property_value = this.CustomProperty.GetInt(target);

                    if (this.HasAdvancedMarkingBuff)
                    {
                        if (!this.ReverseCheck)
                        {
                            if (target_custom_property_value >= altered_DC)
                            {
                                if (altered_DC - target_custom_property_value >= 0 && altered_DC - target_custom_property_value < 5)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                    return true;
                                }
                                if (altered_DC - target_custom_property_value >= 5 && altered_DC - altered_DC - target_custom_property_value < 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.ModerateBuff);
                                    return true;
                                }
                                if (altered_DC - target_custom_property_value >= 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MajorBuff);
                                    return true;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (target_custom_property_value < altered_DC)
                            {
                                if (target_custom_property_value - altered_DC > 0 && target_custom_property_value - altered_DC < 5)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                    return true;
                                }
                                if (target_custom_property_value - altered_DC >= 5 && altered_DC - target_custom_property_value - altered_DC < 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.ModerateBuff);
                                    return true;
                                }
                                if (target_custom_property_value - altered_DC >= 10)
                                {
                                    this.AddMarkingBuff(target, MarkingBuffType.MajorBuff);
                                    return true;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else if (this.HasMarkingBuff)
                    {
                        if (!this.ReverseCheck)
                        {
                            if (target_custom_property_value >= altered_DC)
                            {
                                this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (target_custom_property_value < altered_DC)
                            {
                                this.AddMarkingBuff(target, MarkingBuffType.MinorBuff);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    return (target_custom_property_value >= altered_DC) && !this.ReverseCheck;

                default:
                    return !this.ReverseCheck;
            }


        }

        void IEntityRevealedHandler.HandleEntityRevealed(EntityDataBase entity)
        {
            UnitEntityData revealed_entity = entity as UnitEntityData;
            UnitEntityData caster = this.Owner;

            if (entity != null && revealed_entity != caster && revealed_entity.Faction.AttackFactions.Contains(caster.Faction))
            {
                this.CanBeAttackedBy(revealed_entity);
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
                    if (this.HasAdvancedMarkingBuff)
                    {
                        this.RemoveMarkingBuff(unit, MarkingBuffType.MajorBuff);
                        this.RemoveMarkingBuff(unit, MarkingBuffType.ModerateBuff);
                        this.RemoveMarkingBuff(unit, MarkingBuffType.MinorBuff);
                    }
                    else if (this.HasMarkingBuff)
                    {
                        this.RemoveMarkingBuff(unit, MarkingBuffType.MinorBuff);
                    }
                }
            }


        }

        public void CreateInoffensivenessLogMessage(InoffensivenessMessageType message_type, UnitEntityData target)
        {
            var caster = this.Owner;
            var text = this.Fact.Name;
            var color = new Color32();
            LocalizedString message;
            LogChannelType channel_type;
            LogThreadBaseAttachment attachment_thread;

            switch (this.Type)
            {
                case InoffensivenessEvaluationType.SavingThrow:
                    channel_type = LogChannelType.AnyCombat;
                    attachment_thread = LogThreadBaseAttachment.SavingThrow;
                    break;
                case InoffensivenessEvaluationType.SkillCheck:
                    channel_type = LogChannelType.AnyCombat;
                    attachment_thread = LogThreadBaseAttachment.SkillCheck;
                    break;
                default:
                    channel_type = LogChannelType.Common;
                    attachment_thread = LogThreadBaseAttachment.None;
                    break;
            }

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
            SimpleCombatLogMessage.SendSimpleCombatLogMessage(custom_message, channel_type, attachment_thread);

            return;

        }



        //Evaluation checkers to define which parameters are needed.

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

        //check if other parameters are needed.

        [UsedImplicitly]
        public bool HasAdvancedMarkingBuff
        {
            get
            {
                return this.HasMarkingBuff && this.CalculateDCDifference;
            }
        }


        [UsedImplicitly]
        public bool NeedsCustomDCInput
        {
            get
            {
                return this.UseCustomDC && this.IsBasedOnRoll || this.IsEvaluationOnProperty || this.IsEvaluationCustomProperty;
            }
        }

        public OffensiveActionEffect Offensive_Action_Effect;

        public InoffensivenessEvaluationType Type = InoffensivenessEvaluationType.SavingThrow;

        [HideInInspector]
        [ShowIf("IsEvaluationOnSavingThrow")]
        public SavingThrowType m_SavingThrowType = SavingThrowType.Will;

        [HideInInspector]
        [ShowIf("IsEvaluationOnSkillCheck")]
        public StatType m_StatStype;

        [HideInInspector]
        [ShowIf("IsEvaluationOnBuff")]
        [SerializeField]
        public BlueprintBuffReference m_Buff;

        [HideInInspector]
        [ShowIf("IsEvaluationOnProperty")]
        public UnitProperty Property;

        [HideInInspector]
        [ShowIf("IsEvaluationCustomProperty")]
        public BlueprintUnitProperty CustomProperty;

        public bool UseCustomDC;

        public bool HasMarkingBuff = false;

        public bool CalculateDCDifference = false;

        public bool UseDCAdjustingFacts = false;

        [HideInInspector]
        [ShowIf("NeedsCustomDCInput")]
        public ContextValue CustomDC;

        [ShowIf("HasMarkingBuff")]
        [SerializeField]
        public BlueprintBuffReference m_MinorMarkingBuff;

        [ShowIf("HasAdvancedMarkingBuff")]
        [SerializeField]
        public BlueprintBuffReference m_ModerateMarkingBuff;

        [ShowIf("HasAdvancedMarkingBuff")]
        [SerializeField]
        public BlueprintBuffReference m_MajorMarkingBuff;

        [ShowIf("UseDCAdjustingFacts")]
        [SerializeField]
        private AddInoffensiveness.FactDCModification[] m_FactDCModification = new AddInoffensiveness.FactDCModification[0];

        public bool ReverseCheck = false;

        [Serializable]
        private struct FactDCModification
        {
            public BlueprintUnitFact Fact;

            public int Value;
        }


        public enum InoffensivenessMessageType
        {
            Immunity = 0,
            BypassSucceess = 1,
            BypassFailure = 2,
            Invalidation = 3

        }

        public enum MarkingBuffType
        {
            MinorBuff = 0,
            ModerateBuff = 1,
            MajorBuff = 2
        }

    }
}
