using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Owlcat.QA.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;
using Kingmaker.UnitLogic;
using Kingmaker.Blueprints.JsonSystem;

namespace TomeOfDarkness.NewComponents
{
    [AllowedOn(typeof(BlueprintAbility), false)]
    [TypeId("A4B3D7455BC341C28A98F5B154BB4B07")]
    public class AbilityShowIfCasterHasFacts : BlueprintComponent, IAbilityVisibilityProvider  // Holic75_SC 
    {
        public bool IsAbilityVisible(AbilityData ability)
        {

            foreach (var fact in this.m_UnitFacts)
            {
                if (ability.Caster.HasFact(fact) == this.Any)
                {
                    return this.Any;
                }
            }

            return !this.Any;

        }

        // Token: 0x040087CE RID: 34766
        [ValidateNotNull]
        [SerializeField]
        public BlueprintUnitFactReference[] m_UnitFacts;

        public bool Any;
    }
}
