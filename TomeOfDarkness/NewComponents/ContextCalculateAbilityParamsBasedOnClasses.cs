using System;
using System.Linq;
using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Class.Kineticist;
using Kingmaker.Utility;
using Owlcat.QA.Validation;
using UnityEngine;
using UnityEngine.Serialization;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace TomeOfDarkness.NewComponents
{
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Note: This code was inspired by from Holic75's KingmakerRebalance/CotW Kingmaker mod, however, since porting proved too much for me,
    // I have drawn inspiration from "ContextCalculateAbilityParamsBasedOnClasses" and "ContextCalculateAbilityParams" to create one tweaked variant that should work like Holic75's code.
    // -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    [TypeId("88B8419961F84FC5B9560E7F67DD236C")]
    public class ContextCalculateAbilityParamsBasedOnClasses : ContextAbilityParamsCalculator
    {

        public ReferenceArrayProxy<BlueprintCharacterClass, BlueprintCharacterClassReference> CharacterClasses
        {
            get
            {
                return new ReferenceArrayProxy<BlueprintCharacterClass, BlueprintCharacterClassReference>(this.m_CharacterClasses);
            }
        }

        public ReferenceArrayProxy<BlueprintArchetype, BlueprintArchetypeReference> Archetypes
        {
            get
            {
                return new ReferenceArrayProxy<BlueprintArchetype, BlueprintArchetypeReference>(this.m_Archetypes);
            }
        }


        public override AbilityParams Calculate(MechanicsContext context)
        {
            UnitEntityData maybeCaster = context.MaybeCaster;
            if (maybeCaster == null)
            {
                PFLog.Default.Error(this, "Caster is missing", Array.Empty<object>());
                return context.Params;
            }
            BlueprintScriptableObject associatedBlueprint = context.AssociatedBlueprint;
            UnitEntityData caster = maybeCaster;
            AbilityExecutionContext sourceAbilityContext = context.SourceAbilityContext;
            return this.Calculate(context, associatedBlueprint, caster, (sourceAbilityContext != null) ? sourceAbilityContext.Ability : null);
        }

        public AbilityParams Calculate([NotNull] AbilityData ability)
        {
            return this.Calculate(null, ability.Blueprint, ability.Caster, ability);
        }

        private AbilityParams Calculate([CanBeNull] MechanicsContext context, [NotNull] BlueprintScriptableObject blueprint, [NotNull] UnitEntityData caster, [CanBeNull] AbilityData ability)
        {
            StatType value = this.m_StatType;
            if (this.UseKineticistMainStat)
            {
                UnitPartKineticist unitPartKineticist = (caster != null) ? caster.Get<UnitPartKineticist>() : null;
                if (unitPartKineticist == null)
                {
                    PFLog.Default.Error(blueprint, string.Format("Caster is not kineticist: {0} ({1})", caster, blueprint.NameSafe()), Array.Empty<object>());
                }
                value = ((unitPartKineticist != null) ? unitPartKineticist.MainStatType : this.m_StatType);
            }


            int class_level = 0;
            foreach (var c in this.CharacterClasses)
            {
                if (!CheckArchetype)
                {
                    class_level += caster.Descriptor.Progression.GetClassLevel(c);
                }
                if (CheckArchetype)
                {
                    var class_archetypes = Archetypes.Where(a => a.GetParentClass() == c);

                    if (class_archetypes.Empty() || class_archetypes.Any(a => caster.Descriptor.Progression.IsArchetype(a)))
                    {
                        class_level += caster.Descriptor.Progression.GetClassLevel(c);
                    }
                }

            }


            RuleCalculateAbilityParams ruleCalculateAbilityParams = (ability != null) ? new RuleCalculateAbilityParams(caster, ability) : new RuleCalculateAbilityParams(caster, blueprint, null);
            ruleCalculateAbilityParams.ReplaceStat = new StatType?(value);

            if (this.StatTypeFromCustomProperty)
            {
                ruleCalculateAbilityParams.ReplaceStatBonusModifier = new int?(this.m_CustomProperty.Get().GetInt(caster));
            }

            ruleCalculateAbilityParams.ReplaceCasterLevel = class_level;
            ruleCalculateAbilityParams.ReplaceSpellLevel = new int?(class_level / 2);
            if (context != null)
            {
                return context.TriggerRule<RuleCalculateAbilityParams>(ruleCalculateAbilityParams).Result;
            }
            return Rulebook.Trigger<RuleCalculateAbilityParams>(ruleCalculateAbilityParams).Result;
        }

        public override void ApplyValidation(ValidationContext context, int parentIndex)
        {
            base.ApplyValidation(context, parentIndex);
            if (!this.m_StatType.IsAttribute() && this.m_StatType != StatType.BaseAttackBonus)
            {
                string errorFormat = string.Join(", ", from s in StatTypeHelper.Attributes
                                                       select s.ToString());
                context.AddError("StatType must be Base Attack Bonus or an attribute: {0}", errorFormat, Array.Empty<object>());
            }
        }


        public bool UseKineticistMainStat = false;

        public bool CheckArchetype = false;

        public bool StatTypeFromCustomProperty = false;

        [HideIf("UseKineticistMainStat")]
        public StatType m_StatType = StatType.Charisma;

        public BlueprintCharacterClassReference[] m_CharacterClasses = new BlueprintCharacterClassReference[0];

        [ShowIf("CheckArchetype")]
        public BlueprintArchetypeReference[] m_Archetypes = new BlueprintArchetypeReference[0];

        [ShowIf("StatTypeFromCustomProperty")]
        public BlueprintUnitPropertyReference m_CustomProperty = null;

    }
}
