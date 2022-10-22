using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using Kingmaker.Blueprints.JsonSystem;

namespace TomeOfDarkness.NewComponents
{
    // This new component allows to add a block of features if the unit has (or hasn't) a block of features.
    [TypeId("CEB162D2E3AC4C61B6D415AE77F5A587")]
    public class HasFactsFeaturesUnlock : UnitFactComponentDelegate<HasFactsFeaturesUnlockData>, IUnitGainFactHandler, IUnitLostFactHandler, IUnitSubscriber, ISubscriber
    {
        public ReferenceArrayProxy<BlueprintUnitFact, BlueprintUnitFactReference> CheckedFacts
        {
            get
            {
                BlueprintUnitFactReference[] checkedFacts = m_CheckedFacts;
                if (checkedFacts == null)
                {
                    return null;
                }
                return checkedFacts;
            }
        }

        public ReferenceArrayProxy<BlueprintUnitFact, BlueprintUnitFactReference> Features
        {
            get
            {
                BlueprintUnitFactReference[] features = m_Features;
                if (features == null)
                {
                    return null;
                }
                return features;
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
            if (base.Data.AppliedFacts != null)
            {
                return;
            }

            bool hasfacts = false;
            int foundFacts = 0;

            for (int i = 0; i < this.CheckedFacts.Length; i++)
            {
                if (base.Owner.HasFact(this.CheckedFacts[i]))
                {
                    foundFacts += 1;
                }

            }

            hasfacts = (foundFacts == this.CheckedFacts.Length);

            if ((hasfacts && !Not) || (!hasfacts && Not))
            {

                base.Data.AppliedFacts = new EntityFact[0];

                for (int i1 = 0; i1 < this.Features.Length; i1++)
                {
                    if (!(base.Owner.HasFact(this.Features[i1])))
                    {

                        base.Data.AppliedFacts.AppendToArray(Owner.AddFact(Features[i1], null, null));


                    }

                }

            }


        }


        private void RemoveFact()
        {
            if (base.Data.AppliedFacts != null)
            {
                for (int i = 0; i < base.Data.AppliedFacts.Length; i++)
                {

                    if (Array.IndexOf(this.Features, base.Data.AppliedFacts[i]) != -1)
                    {

                        Owner.RemoveFact(base.Data.AppliedFacts[i]);

                    }

                }
                base.Data.AppliedFacts = null;
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
            bool hasfacts = false;
            int foundFacts = 0;

            for (int i = 0; i < this.CheckedFacts.Length; i++)
            {
                if (base.Owner.HasFact(this.CheckedFacts[i]))
                {
                    foundFacts += 1;
                }

            }

            hasfacts = (foundFacts == this.CheckedFacts.Length);

            return base.Data.AppliedFacts == null && (hasfacts && !Not) || (!hasfacts && Not);
        }

        public void HandleUnitGainFact(EntityFact fact)
        {
            Update();
        }

        public void HandleUnitLostFact(EntityFact fact)
        {
            Update();
        }


        [SerializeField]
        public BlueprintUnitFactReference[] m_CheckedFacts;

        [SerializeField]
        public BlueprintUnitFactReference[] m_Features;

        public bool Not;

    }
}
