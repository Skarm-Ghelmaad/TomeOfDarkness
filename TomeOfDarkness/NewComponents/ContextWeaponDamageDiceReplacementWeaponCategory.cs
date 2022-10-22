using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Utility;
using System.Linq;
using Kingmaker.Blueprints.Items.Weapons;

namespace TomeOfDarkness.NewComponents
{
    // Holic75_SC 
    [AllowedOn(typeof(BlueprintUnitFact))]
    [TypeId("1A8A4697EE79498FBFBFFC09E255A9BC")]
    public class ContextWeaponDamageDiceReplacementWeaponCategory : EntityFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public WeaponCategory[] Categories;
        public DiceFormula[] Dice_Formulas;
        public ContextValue Value;



        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {

            if (!this.Categories.Contains(evt.Weapon.Blueprint.Category))
            {
                return;

            }

            var dice_index = Value.Calculate(this.Context);                             //This allows to select which dice set is used among those listed in the array.

            if (dice_index < 0)                                                         //This sets the value within the lower bound of the array.
            {
                dice_index = 0;

            }
            else if (dice_index >= Dice_Formulas.Length)                                //This sets the value within the upper bound of the array.
            {
                dice_index = Dice_Formulas.Length - 1;
            }

            var wielder_size = evt.Initiator.Descriptor.State.Size;                     // This causes the weapon to scale based on size change.

            var base_damage = evt.WeaponDamageDice.HasModifications ? evt.WeaponDamageDice.ModifiedValue : evt.Weapon.Blueprint.BaseDamage;
            //var base_damage = evt.WeaponDamageDiceOverride.HasValue ? evt.WeaponDamageDiceOverride.Value : evt.Weapon.Blueprint.BaseDamage;
            var base_dice = evt.Initiator.Body.IsPolymorphed ? base_damage : WeaponDamageScaleTable.Scale(base_damage, wielder_size);
            var new_dice = WeaponDamageScaleTable.Scale(this.Dice_Formulas[dice_index], wielder_size);

            var new_dmg_avg = new_dice.MinValue(0, true) + new_dice.MaxValue(0, true);
            int current_dmg_avg = base_dice.MaxValue(0, true) + base_dice.MinValue(0, true);

            if (new_dmg_avg > current_dmg_avg)
            {
                //evt.WeaponDamageDiceOverride = Dice_Formulas[dice_index];

                evt.WeaponDamageDice.Modify(Dice_Formulas[dice_index], base.Fact);
            }



        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
        }
    }
}
