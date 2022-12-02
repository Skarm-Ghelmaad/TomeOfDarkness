using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomeOfDarkness.NewEnums
{
    internal class AbilitySharedValueToD
    {
        public static class NewAbilitySharedValue
        {
            //The last AbilitySharedValue in Kingmaker.UnitLogic.Abilities is "DungeonBoonValue" [ 0x000000006 = 6 ]
            public const AbilitySharedValue PoisonFrequency = (AbilitySharedValue)12_200;
            public const AbilitySharedValue PoisonRecovery = (AbilitySharedValue)12_201;
            public const AbilitySharedValue PoisonStickiness = (AbilitySharedValue)12_202;
            public const AbilitySharedValue PoisonConcentration = (AbilitySharedValue)12_203;

        }
    }
}
