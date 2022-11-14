using Kingmaker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Classes;

namespace TomeOfDarkness.MechanicsChanges
{
    public class FeatureGroupToD
    {
        public static class NewFeatureGroup
        {
            //The last FeatureGroup in Kingmaker.Blueprints.Classes is "HalfOrcHeritage" [ 0x0000005B = 91 ]
            public const FeatureGroup NinjaTrick = (FeatureGroup)10_000;

        }
    }
}
