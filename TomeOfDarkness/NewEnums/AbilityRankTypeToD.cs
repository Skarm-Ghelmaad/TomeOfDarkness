using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomeOfDarkness.NewEnums
{
    internal class AbilityRankTypeToD
    {
        public static class NewAbilityRankType
        {
            //The last AbilitySharedValue in Kingmaker.UnitLogic.Abilities is "SpeedBonus" [ 0x000000006 = 6 ]
            public const AbilityRankType PoisonFrequencyBonus = (AbilityRankType)12_200;
            public const AbilityRankType PoisonRecoveryBonus = (AbilityRankType)12_201;


        }
    }
}
