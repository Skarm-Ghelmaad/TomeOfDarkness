using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TomeOfDarkness.NewUnitParts;

namespace TomeOfDarkness.NewComponents
{
    // Holic75_PT
    [TypeId("307BABF54C87440CB54B50FED16F6F2D")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class IgnoreCasterConcealment : SpecificConcealmentBase
    {
        public override bool WorksAgainst(UnitEntityData attacker)
        {
            return (this.Context.MaybeCaster != attacker);
        }

    }
}
