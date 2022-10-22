using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Blueprints.Classes;
using System;
using System.Linq;
using JetBrains.Annotations;
using Kingmaker.Utility;
using System.Collections.Generic;
using Kingmaker.EntitySystem.Stats;

namespace TomeOfDarkness.NewComponents
{
    // Thìs new component adjusts a resource with a Stat bonus, but also allows the following:
    // - Use the highest Stat bonus among the attributes (Strength, Dexterity, Constitution, Intelligence, Wisdom and Charisma).
    // - Subtract the bonus (instead of adding it).
    // - Apply a resource multiplier to the modifier.
    // Used for the change in Ki Resource mechanics and in the Potential Ki resource.

    [TypeId("653F8009D29A478A8F7DD21C9E531782")]
    public class IncreaseResourceAmountBasedOnStat : UnitFactComponentDelegate, IResourceAmountBonusHandler, IUnitSubscriber, ISubscriber
    {

        public BlueprintAbilityResource Resource
        {
            get
            {
                BlueprintAbilityResourceReference resource = this.m_Resource;
                if (resource == null)
                {
                    return null;
                }
                return resource.Get();
            }
        }

        public ModifiableValueAttributeStat FindHighestAttributeStat(UnitDescriptor unit)
        {


            ModifiableValueAttributeStat strength = unit.Stats.GetStat(StatType.Strength) as ModifiableValueAttributeStat;
            ModifiableValueAttributeStat dexterity = unit.Stats.GetStat(StatType.Dexterity) as ModifiableValueAttributeStat;
            ModifiableValueAttributeStat constitution = unit.Stats.GetStat(StatType.Constitution) as ModifiableValueAttributeStat;
            ModifiableValueAttributeStat intelligence = unit.Stats.GetStat(StatType.Intelligence) as ModifiableValueAttributeStat;
            ModifiableValueAttributeStat wisdom = unit.Stats.GetStat(StatType.Wisdom) as ModifiableValueAttributeStat;
            ModifiableValueAttributeStat charisma = unit.Stats.GetStat(StatType.Charisma) as ModifiableValueAttributeStat;


            ModifiableValueAttributeStat[] unitAttributeStats = { strength, dexterity, constitution, intelligence, wisdom, charisma };

            ModifiableValueAttributeStat maxStat = unitAttributeStats.Max();

            return maxStat;

        }

        public int CalculateStatBonusAmount()
        {
            UnitDescriptor unit = base.Owner;

            int num = 0;

            ModifiableValueAttributeStat modifiableValueAttributeStat = null;

            if (this.NotUseHighestStat)
            {

                modifiableValueAttributeStat = unit.Stats.GetStat(this.ResourceBonusStat) as ModifiableValueAttributeStat;

            }
            else
            {

                modifiableValueAttributeStat = FindHighestAttributeStat(unit);

            }

            if (modifiableValueAttributeStat != null)
            {

                if ((this.UseResourceMultiplier == true) & (this.UseResourceDivisor == true))
                {
                    num += (int)((float)(modifiableValueAttributeStat.Bonus) * (this.ResourceMultiplier / this.ResourceDivisor));
                }
                if ((this.UseResourceMultiplier == true) & (this.UseResourceDivisor == false))
                {
                    num += (int)((float)(modifiableValueAttributeStat.Bonus) * this.ResourceMultiplier);
                }
                else if ((this.UseResourceMultiplier == false) & (this.UseResourceDivisor == true))
                {
                    num += (int)((float)(modifiableValueAttributeStat.Bonus) / this.ResourceDivisor);
                }
                else
                {
                    num += modifiableValueAttributeStat.Bonus;
                }
            }
            else
            {
                PFLog.Default.Error("Can't use stat {0} in ability resource's count formula", new object[]
                {
                        this.ResourceBonusStat
                });
            }

            return num;


        }


        public void CalculateMaxResourceAmount(BlueprintAbilityResource resource, ref int bonus)
        {



            if (base.Fact.Active && resource == this.Resource)
            {

                int resource_amount = CalculateStatBonusAmount();


                if (Subtract)
                {
                    bonus -= resource_amount;
                }
                else
                {
                    bonus += resource_amount;
                }


            }

        }

        public BlueprintAbilityResourceReference m_Resource;

        public bool UseResourceMultiplier = false;

        public bool UseResourceDivisor = false;

        public bool Subtract = false;

        public bool NotUseHighestStat = true;

        [UsedImplicitly]
        [ShowIf("NotUseHighestStat")]
        public StatType ResourceBonusStat;

        [UsedImplicitly]
        [ShowIf("UseResourceMultiplier")]
        public float ResourceMultiplier = 1.00f;        // This is a resource multiplier which is used to tweak the adjustment.

        [UsedImplicitly]
        [ShowIf("UseResourceDivisor")]
        public float ResourceDivisor = 1.00f;        // This is a resource divisor which is used to tweak the adjustment.

    }
}
