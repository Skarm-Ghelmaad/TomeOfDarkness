using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using Owlcat.QA.Validation;
using UnityEngine;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System.Linq;
using Kingmaker.UnitLogic;


namespace TomeOfDarkness.NewComponents.Properties
{
    public class PropertyMulipliedByFactRankGetter : PropertyValueGetter
    {
        public BlueprintUnitFact Fact
        {
            get
            {
                BlueprintUnitFactReference fact = this.m_Fact;
                if (fact == null)
                {
                    return null;
                }
                return fact.Get();
            }
        }

        public override int GetBaseValue(UnitEntityData unit)
        {
            EntityFact fact = unit.GetFact(this.Fact);
            int base_property = this.m_Property.GetInt(unit);
            int bonus = this.Bonus;
            int propertyMultiplier = Mathf.FloorToInt((this.Numerator / this.Denominator));
            Buff buff = fact as Buff;
            int num;
            if (buff == null)
            {
                Feature feature = fact as Feature;
                num = ((feature != null) ? feature.GetRank() : 0);
            }
            else
            {
                num = buff.GetRank();
            }
            return bonus + base_property * propertyMultiplier * num;
        }

        public override void ApplyValidation(ValidationContext context, int parentIndex)
        {
            base.ApplyValidation(context, parentIndex);
            if (this.Fact != null && !(this.Fact is BlueprintFeature) && !(this.Fact is BlueprintBuff))
            {
                context.AddError("Fact must be Feature or Buff", Array.Empty<object>());
            }
        }

        public UnitProperty m_Property;

        [SerializeField]
        [ValidateNotNull]
        public BlueprintUnitFactReference m_Fact;

        public int Bonus = 0;

        public float Numerator = 1;
        public float Denominator = 1;



    }
}
