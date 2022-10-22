using System;
using Kingmaker.EntitySystem;
using Newtonsoft.Json;
using Kingmaker.Designers.Mechanics.Facts;

namespace TomeOfDarkness.NewComponents
{
    // This is the equivalent of UnitFactComponentDelegate<AddFeatureIfHasFactData> but works in blocks of facts (array) instead of individual facts.
    public class HasFactsFeaturesUnlockData
    {
        public EntityFact[] AppliedFacts;
    }
}
