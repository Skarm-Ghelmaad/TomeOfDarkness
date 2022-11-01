using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.PubSubSystem;
using TomeOfDarkness.NewUnitParts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using Kingmaker.Blueprints.JsonSystem;

namespace TomeOfDarkness.NewComponents
{

    [AllowedOn(typeof(BlueprintUnitFact))]
    [TypeId("4FDD2BDCECC149B68D391D631682BA00")]
    public class SetVisibilityLimit : UnitFactComponentDelegate, IUnitSubscriber
    {

        public override void OnTurnOn()
        {
            this.Owner.Ensure<UnitPartVisibilityLimit>().AddUnitPartFlag();
            if (this.Owner.Get<UnitPartVisibilityLimit>() != null)
            {
                this.Owner.Get<UnitPartVisibilityLimit>().AddVisibilityLimit(this.CreateVisibilityLimitEntry());
            }

        }

        public override void OnTurnOff()
        {
            this.Owner.Ensure<UnitPartVisibilityLimit>().RemoveUnitPartFlag();
            if (this.Owner.Get<UnitPartVisibilityLimit>() != null)
            {
                this.Owner.Get<UnitPartVisibilityLimit>().RemoveVisibilityLimit(this.CreateVisibilityLimitEntry());
            }

        }

        public UnitPartVisibilityLimit.VisibilityLimitEntry CreateVisibilityLimitEntry()
        {
            UnitPartVisibilityLimit.VisibilityLimitEntry vl_entry = new UnitPartVisibilityLimit.VisibilityLimitEntry
            {
                VisibilityCap = this.m_VisibilityCap
            };

            return vl_entry;
        }

        public Feet m_VisibilityCap;

     }
}
