using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TomeOfDarkness.NewActions
{
    // Vek17_SC
    // This is a variant of Vek17's "ContextActionApplyBuffRanks" that is specifically designed to work for poison buffs (which use a non-rank stacking method).
    public class ContextActionRepeatedApplyBuff : ContextAction
    {
        public BlueprintBuff Buff => m_Buff?.Get();

        public override string GetCaption()
        {
            string str = string.Concat(new string[]
            {
                "Apply",
                this.AsChild ? " child" : "",
                " Buff",
                this.ToCaster ? " to caster" : "",
                ": ",
                this.Buff.NameSafe() ?? "???"
            });
            if (this.Permanent)
            {
                return str + " (permanent)";
            }
            string str2 = this.SameDuration ? "same duration" : (this.UseDurationSeconds ? string.Format("{0} seconds", this.DurationSeconds) : this.DurationValue.ToString());
            return str + " (for " + str2 + ")";
        }

        public override void RunAction()
        {
            MechanicsContext.Data data = ContextData<MechanicsContext.Data>.Current;
            MechanicsContext mechanicsContext = data?.Context;
            if (mechanicsContext == null)
            {
                PFLog.Default.Error(this, "Unable to apply buff: no context found", Array.Empty<object>());
                return;
            }
            UnitEntityData buffTarget = this.GetBuffTarget(mechanicsContext);
            if (buffTarget == null)
            {
                PFLog.Default.Error(this, "Can't apply buff: target is null", Array.Empty<object>());
                return;
            }
            TimeSpan? duration = this.CalculateDuration(mechanicsContext);
            Buff buff = buffTarget.Descriptor.AddBuff(this.Buff, mechanicsContext, duration);
            int repeated_application = BuffApplications.Calculate(Context) - 1;
            if (buff == null)
            {
                return;
            }
            if (repeated_application >= 1)
            {
                for (int bf_appl = 0; bf_appl < repeated_application; bf_appl++)
                {
                    buffTarget.Descriptor.AddBuff(this.Buff, mechanicsContext, duration);
                }
            }
            AreaEffectContextData areaEffectContextData = ContextData<AreaEffectContextData>.Current;
            AreaEffectEntityData areaEffectEntityData = areaEffectContextData?.Entity;
            if (areaEffectEntityData != null && !this.NotLinkToAreaEffect)
            {
                buff.SourceAreaEffectId = areaEffectEntityData.UniqueId;
            }
            buff.IsFromSpell = (this.IsFromSpell || this.Buff.IsFromSpell);
            buff.IsNotDispelable |= this.IsNotDispelable;
            if (this.AsChild)
            {
                Buff.Data data2 = ContextData<Buff.Data>.Current;
                Buff buff2 = data2?.Buff;
                if (buff2 != null)
                {
                    if (buff2.Owner == buff.Owner)
                    {
                        buff2.StoreFact(buff);
                        return;
                    }
                    PFLog.Default.Error(mechanicsContext.AssociatedBlueprint, "Parent and child buff must have one owner (" + mechanicsContext.AssociatedBlueprint.name + ")", Array.Empty<object>());
                }
            }
        }

        public UnitEntityData GetBuffTarget(MechanicsContext context)
        {
            if (!this.ToCaster)
            {
                return base.Target.Unit;
            }
            return context.MaybeCaster;
        }

        private TimeSpan? CalculateDuration(MechanicsContext context)
        {
            if (this.Permanent)
            {
                return null;
            }
            if (!this.SameDuration)
            {
                TimeSpan value = this.UseDurationSeconds ? this.DurationSeconds.Seconds() : this.DurationValue.Calculate(context).Seconds;
                return new TimeSpan?(value);
            }
            Buff.Data data = ContextData<Buff.Data>.Current;
            Buff buff = data?.Buff;
            if (buff == null)
            {
                return null;
            }
            return new TimeSpan?(buff.TimeLeft);
        }

        [SerializeField]
        [FormerlySerializedAs("Buff")]
        public BlueprintBuffReference m_Buff;
        public bool Permanent;
        public ContextValue BuffApplications = 1;
        public bool UseDurationSeconds;
        [HideIf("UseDurationSeconds")]
        public ContextDurationValue DurationValue;
        [ShowIf("UseDurationSeconds")]
        public float DurationSeconds;
        public bool IsFromSpell;
        public bool IsNotDispelable;
        public bool ToCaster;
        public bool AsChild = true;
        public bool SameDuration;
        [InfoBox("By default all effects that were created from area effect will be destroyed after area effect ends.\nCheck it on if you want buff to live longer")]
        public bool NotLinkToAreaEffect;
    }
}
