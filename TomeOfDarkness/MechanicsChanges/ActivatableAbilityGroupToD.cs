using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic.ActivatableAbilities;

namespace TomeOfDarkness.MechanicsChanges
{
    internal class ActivatableAbilityGroupToD
    {
        public static class NewActivatableAbilityGroup
        {
            //The last ActivatableAbilityGroup in Kingmaker.Blueprints.Classes is "ShifterAspect" [ 0x000000030 = 48 ]
            public const ActivatableAbilityGroup PressurePoints = (ActivatableAbilityGroup)10_000;

        }
    }
}

