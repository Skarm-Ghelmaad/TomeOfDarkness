using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomeOfDarkness.NewComponents
{
    // Holic75_SC
    [ComponentName("Attack Dice Reroll")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    [TypeId("47B9AD42D7264BC99040C683407C9054")]
    public class ReduceConcealmentByOneStep : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (this.m_Attack != null)
            {
                return;
            }
            evt.Initiator.Ensure<UnitPartConcealment>().IgnorePartial = true;
            evt.Initiator.Ensure<UnitPartConcealment>().TreatTotalAsPartial = true;
            this.m_Attack = evt;
        }

        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (this.m_Attack == null)
            {
                return;
            }
            evt.Initiator.Ensure<UnitPartConcealment>().IgnorePartial = false;
            evt.Initiator.Ensure<UnitPartConcealment>().TreatTotalAsPartial = false;
            this.m_Attack = (RuleAttackRoll)null;
        }

        private RuleAttackRoll m_Attack;
    }
}
