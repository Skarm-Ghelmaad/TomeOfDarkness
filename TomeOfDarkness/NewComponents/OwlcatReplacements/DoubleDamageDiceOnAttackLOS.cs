using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace TomeOfDarkness.NewComponents.OwlcatReplacements
{

    // This variant of DoubleDamageDiceOnAttack checks if the current weapon damage is greater than the base damage of the weapon and, if that is the case, uses the current damage dice instead of those of the base weapon.
    // Holic75_PT 

    [ComponentName("Roll weapon damage dice twice on attack")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("DA2CB2B27943452DB904E55A9F8C45D8")]
    public class DoubleDamageDiceOnAttackLOS : EntityFactComponentDelegate, IInitiatorRulebookHandler<RuleDealDamage>, IRulebookHandler<RuleDealDamage>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public BlueprintWeaponType WeaponType
        {
            get
            {
                BlueprintWeaponTypeReference weaponType = this.m_WeaponType;
                if (weaponType == null)
                {
                    return null;
                }
                return weaponType.Get();
            }
        }

        public void OnEventAboutToTrigger(RuleDealDamage evt)
        {
            RuleAttackWithWeapon ruleAttackWithWeapon = evt.Reason.Rule as RuleAttackWithWeapon;
            if (ruleAttackWithWeapon == null || evt.DamageBundle.WeaponDamage == null)
            {
                return;
            }
            if (this.CheckCondition(ruleAttackWithWeapon))
            {
                var current_weapon_damage_rolls = evt.DamageBundle.WeaponDamage.Dice.BaseFormula.Rolls;

                var current_weapon_damage_dice = evt.DamageBundle.WeaponDamage.Dice.BaseFormula.Dice;

                var current_weapon_damage = evt.DamageBundle.WeaponDamage;
                var base_weapon_damage = ruleAttackWithWeapon.Weapon.DamageDice;

                var current_weapon_damage_avg = current_weapon_damage.Dice.BaseFormula.MinValue(0, true) + current_weapon_damage.Dice.BaseFormula.MaxValue(0, true);
                int base_weapon_damage_avg = (base_weapon_damage.MaxValue(0, true) + base_weapon_damage.MinValue(0, true));

                if (current_weapon_damage_avg > base_weapon_damage_avg)
                {
                    BaseDamage extra_damage1 = evt.DamageBundle.WeaponDamage.CreateTypeDescription().CreateDamage(new DiceFormula(current_weapon_damage_rolls, current_weapon_damage_dice), 0);
                    evt.Add(extra_damage1);
                }
                else
                {
                    BaseDamage extra_damage2 = evt.DamageBundle.WeaponDamage.CreateTypeDescription().CreateDamage(ruleAttackWithWeapon.Weapon.DamageDice, 0);
                    evt.Add(extra_damage2);
                }
            }
        }

        public void OnEventDidTrigger(RuleDealDamage evt)
        {
        }

        private bool CheckCondition(RuleAttackWithWeapon evt)
        {
            ItemEnchantment itemEnchantment = base.Fact as ItemEnchantment;
            ItemEntity itemEntity = (itemEnchantment != null) ? itemEnchantment.Owner : null;
            return (itemEntity == null || itemEntity == evt.Weapon) && (!this.WeaponType || this.WeaponType == evt.Weapon.Blueprint.Type) && (!this.CriticalHit || (evt.AttackRoll.IsCriticalConfirmed && !evt.AttackRoll.FortificationNegatesCriticalHit)) && (!this.OnlyOnFullAttack || evt.IsFullAttack) && (!this.OnlyOnFirstAttack || evt.IsFirstAttack);
        }

        public bool OnlyOnFullAttack;

        [ShowIf("OnlyOnFullAttack")]
        public bool OnlyOnFirstAttack;

        public bool CriticalHit;

        [SerializeField]
        [FormerlySerializedAs("WeaponType")]
        public BlueprintWeaponTypeReference m_WeaponType;
    }
}
