using System;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using UnityEngine;
using UnityEngine.Serialization;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.QA;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using Newtonsoft.Json;
using Kingmaker.Designers.Mechanics.Facts;
using static Kingmaker.GameModes.GameModeType;

namespace TomeOfDarkness.NewComponents
{

    // Thìs new component allows to use a context value (instead of an int) as bonus and is an hybridization inspired by "OutflankAttackBonus", "AddContextStatBonus" and "FlankedAttackBonus"
    // Used for Dispatchment class feature.

    [ComponentName("Context-based Bonus to attack against flanked opponents if has fact")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    [TypeId("E92634A9FA97465A85FF7BC3AE0E14D2")]
    public class AddContextFlankingAttackBonus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonus>, IRulebookHandler<RuleCalculateAttackBonus>, ISubscriber, IInitiatorRulebookSubscriber
    {


        public void OnEventAboutToTrigger(RuleCalculateAttackBonus evt)
        {
            bool isFlatFooted = Rulebook.Trigger<RuleCheckTargetFlatFooted>(new RuleCheckTargetFlatFooted(evt.Initiator, evt.Target)).IsFlatFooted;
            if (evt.Target.CombatState.IsFlanked || isFlatFooted || evt.TargetIsFlanked)
            {
                int ContextAttackBonus = this.CalculateContexFlankingAttackBonus(base.Fact.MaybeContext);
                if (this.HasMinimal)
                {
                    evt.AddModifier(Math.Max(ContextAttackBonus, this.Minimal), base.Fact, this.Descriptor);
                }
                else
                {
                    evt.AddModifier(ContextAttackBonus, base.Fact, this.Descriptor);
                }
            }
        }


        public int CalculateContexFlankingAttackBonus(MechanicsContext context)
        {
            if (context == null)
            {
                PFLog.Default.ErrorWithReport("Context is missing", Array.Empty<object>());
                return 0;
            }
            return this.Value.Calculate(context) * this.Multiplier;
        }


        public void OnEventDidTrigger(RuleCalculateAttackBonus evt)
        {
        }

        public ModifierDescriptor Descriptor = ModifierDescriptor.UntypedStackable;

        public int Multiplier = 1;

        public ContextValue Value;

        public bool HasMinimal = false;

        [ShowIf("HasMinimal")]
        public int Minimal;
    }
}
