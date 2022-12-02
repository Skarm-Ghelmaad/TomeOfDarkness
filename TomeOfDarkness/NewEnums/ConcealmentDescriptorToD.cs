using Kingmaker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TabletopTweaks.Core.NewUnitParts.CustomStatTypes;

namespace TomeOfDarkness.NewEnums
{
    public class ConcealmentDescriptorToD
    {
        public static class NewConcealmentDescriptor
        {
            //The last ConcealmentDescriptor in Kingmaker.Enums.ConcealmentDescriptor is "WindsOfVengenance" [ 0x00000005 = 5 ]
            public const ConcealmentDescriptor LowLight = (ConcealmentDescriptor)12_200;
            public const ConcealmentDescriptor Darkness = (ConcealmentDescriptor)12_201;

        }

    }
}
