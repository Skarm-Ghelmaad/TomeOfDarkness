using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;

namespace TomeOfDarkness.NewComponents
{
    [AllowedOn(typeof(BlueprintAbility))]
    [AllowMultipleComponents]
    [TypeId("18B6DD41526342798E7F2B51C965BF8B")]
    public class AbilityShowIfCasterHasResource : BlueprintComponent, IAbilityVisibilityProvider  // Holic75_SC 
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

        public bool IsAbilityVisible(AbilityData ability)
        {
            if (this.Resource == null)
            {
                return true;
            }
            if (ability.Caster.Resources.GetResourceAmount(this.Resource) < this.Amount)
            {
                return false;
            }

            return true;

        }


        public BlueprintAbilityResourceReference m_Resource;
        public int Amount = 1;

    }
}
