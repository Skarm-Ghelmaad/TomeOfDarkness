using Kingmaker;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace TomeOfDarkness.NewComponents.AreaEffects
{
    // The original component from Holic75 was "AbilityAreaEffectRunActionWithFirstRound", but I wanted a more versatile component that allowed for more fine-tuned
    // (and longer) delay.
    public class AbilityAreaEffectRunActionWithDelay : AbilityAreaEffectLogic
    {
        public override void OnUnitEnter(MechanicsContext context, AreaEffectEntityData areaEffect, UnitEntityData unit)
        {

            if (!this.UnitEnter.HasActions || this.IsDelayRound(areaEffect))
            {
                return;
            }
            if (this.UnitEnter.HasActions && (!this.IsDelayRound(areaEffect)))
            {
                using (ContextData<AreaEffectContextData>.Request().Setup(areaEffect))
                {
                    using (context.GetDataScope(unit))
                    {
                        this.UnitEnter.Run();
                    }
                }
            }

        }

        public override void OnUnitExit(MechanicsContext context, AreaEffectEntityData areaEffect, UnitEntityData unit)
        {
            if (!this.UnitExit.HasActions || this.IsDelayRound(areaEffect))
            {
                return;
            }
            if (this.UnitExit.HasActions && (!this.IsDelayRound(areaEffect)))
            {
                using (ContextData<AreaEffectContextData>.Request().Setup(areaEffect))
                {
                    using (context.GetDataScope(unit))
                    {
                        this.UnitExit.Run();
                    }
                }
            }

        }

        public override void OnUnitMove(MechanicsContext context, AreaEffectEntityData areaEffect, UnitEntityData unit)
        {
            if (!this.UnitMove.HasActions || this.IsDelayRound(areaEffect))
            {
                return;
            }
            if (this.UnitMove.HasActions && (!this.IsDelayRound(areaEffect)))
            {
                using (ContextData<AreaEffectContextData>.Request().Setup(areaEffect))
                {
                    using (context.GetDataScope(unit))
                    {
                        this.UnitMove.Run();
                    }
                }
            }
        }

        public override void OnRound(MechanicsContext context, AreaEffectEntityData areaEffect)
        {
            if (!this.Round.HasActions && !this.DelayRound.HasActions)
            {
                return;
            }
            using (ContextData<AreaEffectContextData>.Request().Setup(areaEffect))
            {
                foreach (UnitEntityData unit in areaEffect.InGameUnitsInside)
                {
                    using (context.GetDataScope(unit))
                    {
                        if (!this.IsDelayRound(areaEffect))
                        {
                            if (this.Round != null)
                            {
                                this.Round.Run();

                            }
                        }
                        else
                        {
                            if (this.DelayRound != null)
                            {
                                this.DelayRound.Run();

                            }
                        }

                    }
                }
            }
            
        }


        public bool IsDelayRound(AreaEffectEntityData areaEffect)
        {
            return this.OnsetTime(areaEffect) > Game.Instance.TimeController.GameTime;

        }


        public TimeSpan OnsetTime(AreaEffectEntityData areaEffect)
        {
            var delay = this.Delay.Calculate(areaEffect.Context);

            if (!this.IsNotFirstRoundDelay)
            {
                return areaEffect.m_CreationTime.Add(new TimeSpan(0, 0, 0, 0, 100));
            }
            else if (delay <= 1.Rounds())
            {
                return areaEffect.m_CreationTime.Add(new TimeSpan(0, 0, 0, 0, 100));
            }
            else
            {
                return areaEffect.m_CreationTime.Add(delay.Seconds).Add(new TimeSpan(0, 0, 0, 0, 100));
            }                      

        }

        public ActionList UnitEnter = Helpers.CreateActionList(null);

        public ActionList UnitExit = Helpers.CreateActionList(null);

        public ActionList UnitMove = Helpers.CreateActionList(null);

        public ActionList Round = Helpers.CreateActionList(null);

        public ActionList DelayRound = Helpers.CreateActionList(null);

        public bool IsNotFirstRoundDelay = false;

        [HideInInspector]
        [ShowIf("IsNotFirstRoundDelay")]
        public ContextDurationValue Delay;

    }
}
