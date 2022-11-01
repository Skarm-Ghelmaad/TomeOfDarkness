using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomeOfDarkness.NewUnitParts;
using Kingmaker.Enums;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;

namespace TomeOfDarkness.NewComponents
{
    // This was inspired by Holic75's "IgnoreFogConcelement" but I wanted something more versatile that could be used for any ConcealmentDescriptor.
    [TypeId("B859D8C200F947FDB384338050EA49BC")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class IgnoreConcealmentDescriptor : UnitFactComponentDelegate, IUnitSubscriber
    {

        public override void OnTurnOn()
        {
            this.Owner.Ensure<UnitPartIgnoreConcealmentDescriptor>().AddUnitPartFlag();
            if (this.Owner.Get<UnitPartIgnoreConcealmentDescriptor>() != null)
            {
                this.Owner.Get<UnitPartIgnoreConcealmentDescriptor>().AddIgnoreConcealmentDescriptor(this.CreateIgnoredConcealmentDescriptorEntry());
            }

        }

        public override void OnTurnOff()
        {
            this.Owner.Ensure<UnitPartIgnoreConcealmentDescriptor>().RemoveUnitPartFlag();
            if (this.Owner.Get<UnitPartIgnoreConcealmentDescriptor>() != null)
            {
                this.Owner.Get<UnitPartIgnoreConcealmentDescriptor>().RemoveIgnoreConcealmentDescriptor(this.CreateIgnoredConcealmentDescriptorEntry());
            }

        }


        public UnitPartIgnoreConcealmentDescriptor.IgnoredConcealmentDescriptorEntry CreateIgnoredConcealmentDescriptorEntry()
        {
            UnitPartIgnoreConcealmentDescriptor.IgnoredConcealmentDescriptorEntry icd_entry = new UnitPartIgnoreConcealmentDescriptor.IgnoredConcealmentDescriptorEntry
            {
                Type = this.m_IgnoredDescriptor
            };

            return icd_entry;
        }

        public ConcealmentDescriptor m_IgnoredDescriptor;

    }
}
