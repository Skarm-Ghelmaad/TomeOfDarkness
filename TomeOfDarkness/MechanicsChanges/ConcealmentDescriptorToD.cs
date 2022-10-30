using Kingmaker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TabletopTweaks.Core.NewUnitParts.CustomStatTypes;

namespace TomeOfDarkness.MechanicsChanges
{
    internal class ConcealmentDescriptorToD
    {
        public static class NewConcealmentDescriptor
        {
            //The last ConcealmentDescriptor in Kingmaker.Enums.ConcealmentDescriptor is "WindsOfVengenance" [ 0x00000005 = 5 ]
            public const ConcealmentDescriptor LowLight = (ConcealmentDescriptor)10_000;
            public const ConcealmentDescriptor Darkness = (ConcealmentDescriptor)10_001;

        }

    }
}
