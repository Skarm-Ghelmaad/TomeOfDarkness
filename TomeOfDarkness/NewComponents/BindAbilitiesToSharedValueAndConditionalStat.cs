using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using UnityEngine.Serialization;
using UnityEngine;
using Kingmaker.Utility;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Blueprints.Classes;

namespace TomeOfDarkness.NewComponents
{
    [AllowMultipleComponents]
    [ComponentName("Bind ability parameters to a Shared (Context) Value and Attribute bonuses defined by certain features.")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("A69263B978984CC3A627F9BD288B2B6B")]
    public class BindAbilitiesToSharedValueAndConditionalStat : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleDispelMagic>, IRulebookHandler<RuleDispelMagic>, IInitiatorRulebookHandler<RuleSpellResistanceCheck>, IRulebookHandler<RuleSpellResistanceCheck>
    {

        public ReferenceArrayProxy<BlueprintAbility, BlueprintAbilityReference> Abilites
        {
            get
            {
                return this.m_Abilites;
            }
        }

        public StatType FindApplicableAttributeStat(UnitDescriptor unit)
        {


            var resulting_stat = this.DefaultStat;

            foreach (var st_fea in this.m_StatFeature)
            {

                if ((st_fea.Key != null) && unit.HasFact(st_fea.Key))
                {
                    return resulting_stat = st_fea.Value;
                }
            }

            return resulting_stat;

        }

        public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            if (this.Abilites.Contains(evt.Spell))
            {
                int level = this.GetLevel(this.Context);
                evt.ReplaceStat = new StatType?(this.FindApplicableAttributeStat(evt.Initiator.Descriptor));
                evt.ReplaceCasterLevel = new int?(this.GetLevelBase(level));
                evt.ReplaceSpellLevel = new int?(this.Cantrip ? 0 : Math.Min(level / 2, 10));            // While this system is primarily meant to stack classes, I have capped the spell level to 10th to avoid troubles with actual spells.
            }
        }

        public void OnEventDidTrigger(RuleCalculateAbilityParams evt)
        {
        }

        public void OnEventAboutToTrigger(RuleDispelMagic evt)
        {
            IEnumerable<BlueprintAbility> source = this.Abilites;
            AbilityData ability = evt.Reason.Ability;
            if (source.Contains((ability != null) ? ability.Blueprint : null) && this.FullCasterChecks)
            {
                evt.Bonus += this.GetLevelDiff(this.Context);
            }
        }

        public void OnEventDidTrigger(RuleDispelMagic evt)
        {
        }

        public void OnEventAboutToTrigger(RuleSpellResistanceCheck evt)
        {
            if (this.Abilites.Contains(evt.Ability) && this.FullCasterChecks)
            {
                evt.AddSpellPenetration(this.GetLevelDiff(this.Context), ModifierDescriptor.UntypedStackable);
            }
        }

        public void OnEventDidTrigger(RuleSpellResistanceCheck evt)
        {
        }

        private int GetLevel(MechanicsContext context)
        {
            return this.ValueBasedLevel.Calculate(context);
        }

        private int GetLevelBase(int level)
        {
            return (level - (this.Odd ? 1 : 0)) / (this.Cantrip ? 1 : this.LevelStep);
        }

        private int GetLevelDiff(MechanicsContext context)
        {
            int level = this.GetLevel(context);
            int levelBase = this.GetLevelBase(level);
            return Math.Max(level - levelBase, 0);
        }

        [SerializeField]
        [FormerlySerializedAs("Abilites")]
        public BlueprintAbilityReference[] m_Abilites;

        public ContextValue ValueBasedLevel;

        public bool SetMinCasterLevel = false;

        public int m_MinCasterLevel = 1;

        public bool Cantrip;

        [HideIf("Cantrip")]
        public int LevelStep = 1;

        [HideIf("Cantrip")]
        public bool Odd;

        public StatType DefaultStat;

        public IDictionary<BlueprintFeatureReference, StatType> m_StatFeature = new Dictionary<BlueprintFeatureReference, StatType>();

        public bool FullCasterChecks = true;


    }
}
