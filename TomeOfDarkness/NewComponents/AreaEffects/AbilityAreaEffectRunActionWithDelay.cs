//using Kingmaker;
//using Kingmaker.ElementsSystem;
//using Kingmaker.EntitySystem.Entities;
//using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
//using Kingmaker.UnitLogic.Abilities.Components.Base;
//using Kingmaker.UnitLogic.Mechanics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.Remoting.Contexts;
//using System.Text;
//using System.Threading.Tasks;
//using TabletopTweaks.Core.Utilities;

//namespace TomeOfDarkness.NewComponents.AreaEffects
//{
//    // The original component from Holic75 was "AbilityAreaEffectRunActionWithFirstRound", but I wanted a more versatile component that allowed for more fine-tuned
//    // (and longer) delay.
//    public class AbilityAreaEffectRunActionWithDelay : AbilityAreaEffectLogic
//    {
//        public override void OnUnitEnter(MechanicsContext context, AreaEffectEntityData areaEffect, UnitEntityData unit)
//        {
//            if (this.OnsetTimeSet == false)
//            {
//                this.SetOnsetTime(areaEffect);
//                this.OnsetTimeSet = true;
//            }
//            if (!this.UnitEnter.HasActions || !this.IsOnsetTimeExpired() )
//            {
//                return;
//            }
//            else
//            {
//                if (!this.UnitEnter.HasActions)
//                {
//                    return;
//                }
//                using (ContextData<AreaEffectContextData>.Request().Setup(areaEffect))
//                {
//                    using (context.GetDataScope(unit))
//                    {
//                        this.UnitEnter.Run();
//                    }
//                }
//            }
//        }

//        public override void OnUnitExit(MechanicsContext context, AreaEffectEntityData areaEffect, UnitEntityData unit)
//        {
//            if (this.OnsetTimeSet == false)
//            {
//                this.SetOnsetTime(areaEffect);
//                this.OnsetTimeSet = true;
//            }
//            if (!this.UnitExit.HasActions || !this.IsOnsetTimeExpired())
//            {
//                return;
//            }
//            else
//            {
//                if (!this.UnitExit.HasActions)
//                {
//                    return;
//                }
//                using (ContextData<AreaEffectContextData>.Request().Setup(areaEffect))
//                {
//                    using (context.GetDataScope(unit))
//                    {
//                        this.UnitExit.Run();
//                    }
//                }
//            }

//        }

//        public override void OnUnitMove(MechanicsContext context, AreaEffectEntityData areaEffect, UnitEntityData unit)
//        {
//            if (this.OnsetTimeSet == false)
//            {
//                this.SetOnsetTime(areaEffect);
//                this.OnsetTimeSet = true;
//            }
//            if (!this.UnitMove.HasActions || !this.IsOnsetTimeExpired())
//            {
//                return;
//            }
//            else
//            {
//                if (!this.UnitMove.HasActions)
//                {
//                    return;
//                }
//                using (ContextData<AreaEffectContextData>.Request().Setup(areaEffect))
//                {
//                    using (context.GetDataScope(unit))
//                    {
//                        this.UnitMove.Run();
//                    }
//                }
//            }
//        }

//        public override void OnRound(MechanicsContext context, AreaEffectEntityData areaEffect)
//        {
//            if (this.OnsetTimeSet == false)
//            {
//                this.SetOnsetTime(areaEffect);
//                this.OnsetTimeSet = true;
//            }
//            if (!this.Round.HasActions && !this.PreOnsetRound.HasActions)
//            {
//                return;
//            }
//            else
//            {
//                using (ContextData<AreaEffectContextData>.Request().Setup(areaEffect))
//                {
//                    foreach (UnitEntityData unit in areaEffect.InGameUnitsInside)
//                    {
//                        using (context.GetDataScope(unit))
//                        {
//                            if (this.IsOnsetTimeExpired())
//                            {
//                                if (this.Round.HasActions)
//                                {
//                                    this.Round.Run();
//                                }
//                            }
//                            else
//                            {
//                                if (this.PreOnsetRound.HasActions)
//                                {
//                                    this.PreOnsetRound.Run();
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//        }


//        public bool IsOnsetTimeExpired()
//        {
//            return !(OnsetTime > Game.Instance.TimeController.GameTime);

//        }

//        public void SetOnsetTime(AreaEffectEntityData areaEffect)
//        {

//            this.OnsetTime = areaEffect.m_CreationTime + this.Delay.Calculate(areaEffect.Context).Seconds;

//        }

//        public ActionList UnitEnter = Helpers.CreateActionList(null);

//        public ActionList UnitExit = Helpers.CreateActionList(null);

//        public ActionList UnitMove = Helpers.CreateActionList(null);

//        public ActionList Round = Helpers.CreateActionList(null);

//        public ActionList PreOnsetRound = Helpers.CreateActionList(null);

//        public ContextDurationValue Delay;

//        private TimeSpan OnsetTime;

//        private bool OnsetTimeSet = false;
//    }
//}
