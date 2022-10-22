using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using UnityEngine;
using Kingmaker.Utility;
using Kingmaker.Blueprints.Classes;
using UnityEngine.Serialization;
using Kingmaker.Blueprints.JsonSystem;

namespace TomeOfDarkness.NewComponents
{
    // This new component allows to add a specific feature if the unit has a certain feat.
    // Note that this component is designed to allow for mutually-exclusionary conditional selection, so, if multiple conditions are met, only the first feature found will be added.

    [TypeId("E4A2CF7131BF45099D50E3115D6EB1D8")]
    public class ConditionalFactsFeaturesUnlock : UnitFactComponentDelegate<ConditionalFactsFeaturesUnlockData>, IUnitGainFactHandler, IUnitLostFactHandler, IUnitSubscriber, ISubscriber
    {

        public BlueprintFeature ConditionalFeature
        {
            get
            {

                UnitDescriptor unit = base.Owner;

                BlueprintFeature result = new BlueprintFeature();

                foreach (var condfeat in m_ConditionalFeatures)
                {

                    if ((condfeat.Key != null) && ((unit.HasFact(condfeat.Key) && (!Not)) || (!unit.HasFact(condfeat.Key) && (Not))))
                    {
                        result = condfeat.Value;
                        goto endResult;
                    }

                }

            endResult:

                return result;

            }
        }


        public override void OnTurnOn()
        {
            Update();
        }

        public override void OnTurnOff()
        {
            RemoveFact();
        }

        public override void OnActivate()
        {
            Update();
        }

        public override void OnDeactivate()
        {
            RemoveFact();
        }

        public override void OnPostLoad()
        {
            Update();
        }

        private void Apply()
        {
            if (base.Data.AppliedFact != null)
            {
                return;
            }
            if ((this.ConditionalFeature != null))
            {
                base.Data.AppliedFact = Owner.AddFact(this.ConditionalFeature, null, null);
            }
        }

        private void RemoveFact()
        {
            if (Data.AppliedFact != null)
            {
                Owner.RemoveFact(Data.AppliedFact);
                Data.AppliedFact = null;
            }
        }

        private void Update()
        {
            if (ShouldApply())
            {
                Apply();
            }
            else
            {
                RemoveFact();
            }
        }

        private bool ShouldApply()
        {
            return (base.Data.AppliedFact == null) && (this.ConditionalFeature != null);
        }

        public void HandleUnitGainFact(EntityFact fact)
        {
            Update();
        }

        public void HandleUnitLostFact(EntityFact fact)
        {
            Update();
        }


        public IDictionary<BlueprintUnitFactReference, BlueprintFeatureReference> m_ConditionalFeatures = new Dictionary<BlueprintUnitFactReference, BlueprintFeatureReference>();

        public bool Not;
    }
}
