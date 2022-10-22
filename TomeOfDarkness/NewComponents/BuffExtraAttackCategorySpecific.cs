using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.Designers.Mechanics.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.RuleSystem;

namespace TomeOfDarkness.NewComponents
{
    // This is used for the Flurry Of Stars ninja trick.
    // Holic75_SC 

    [TypeId("4B4C289264634B8FB879491D1826BCA0")]
    public class BuffExtraAttackCategorySpecific : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttacksCount>, IRulebookHandler<RuleCalculateAttacksCount>, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public WeaponCategory[] Categories;
        public ContextValue Extra_Attacks = 1;
        public int Attack_Bonus = 0;


        public void OnEventAboutToTrigger(RuleCalculateAttacksCount evt)
        {
            if (!this.Owner.Body.PrimaryHand.HasWeapon || !this.Categories.Contains(this.Owner.Body.PrimaryHand.Weapon.Blueprint.Category))
                return;
            var attacks = this.Extra_Attacks.Calculate(this.Fact.MaybeContext);
            evt.AddExtraAttacks(attacks, false, false, (ItemEntity)this.Owner.Body.PrimaryHand.Weapon);
        }

        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            if (evt.Weapon == null)
                return;
            RulebookEvent rule = evt.Reason.Rule;
            if (rule != null && rule is RuleAttackWithWeapon && !(rule as RuleAttackWithWeapon).IsFullAttack)
                return;
            if (!this.Categories.Contains(evt.Weapon.Blueprint.Category))
            {
                return;
            }
            evt.AddModifier(Attack_Bonus, this.Fact, ModifierDescriptor.Penalty);
        }

        public void OnEventDidTrigger(RuleCalculateAttacksCount evt)
        {

        }

        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {

        }
    }
}
