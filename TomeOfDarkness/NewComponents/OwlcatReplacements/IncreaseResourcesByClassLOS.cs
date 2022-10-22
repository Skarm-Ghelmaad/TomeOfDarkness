using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Blueprints.Classes;
using System;
using System.Linq;
using JetBrains.Annotations;
using Kingmaker.Utility;
using System.Collections.Generic;
using Kingmaker.EntitySystem.Stats;
using UnityEngine;

namespace TomeOfDarkness.NewComponents.OwlcatReplacements
{

    // This is a variant of IncreaseResourcesByClass which allows for:
    // - A more detailed calculation (not just by Level but also by LevelStartPlusDivStep, just like the normal resource calculation)
    // - A resource multiplier and a resource divider (to further tweak the calculation).
    // - The subtraction of the calculated amount (which allows to reduce a resource)

    [TypeId("EAD249FB3E5E47EFB780017C86FD9BE8")]
    public class IncreaseResourcesByClassLOS : UnitFactComponentDelegate, IResourceAmountBonusHandler, IUnitSubscriber, ISubscriber
    {

        [CanBeNull]
        public BlueprintCharacterClass CharacterClass
        {
            get
            {
                BlueprintCharacterClassReference characterClass = this.m_CharacterClass;
                if (characterClass == null)
                {
                    return null;
                }
                return characterClass.Get();
            }
        }

        [CanBeNull]
        public BlueprintArchetype Archetype
        {
            get
            {
                BlueprintArchetypeReference archetype = this.m_Archetype;
                if (archetype == null)
                {
                    return null;
                }
                return archetype.Get();
            }
        }

        public int CalculateBonusAmount(int classLevel)
        {
            int num = 0;

            float final_Resource_Multiplier = 1.00f;

            if ((this.UseResourceMultiplier == true) & (this.UseResourceMultiplier = true))
            {
                final_Resource_Multiplier = this.ResourceMultiplier / this.ResourceDivisor;
            }
            else if ((this.UseResourceMultiplier == false) & (this.UseResourceMultiplier = true))
            {
                final_Resource_Multiplier = 1.00f / this.ResourceDivisor;
            }
            else if ((this.UseResourceMultiplier == true) & (this.UseResourceMultiplier = false))
            {
                final_Resource_Multiplier = this.ResourceMultiplier;
            }

            if (this.IncreasedByLevel)
            {
                num = (int)((float)(this.BaseValue + (classLevel * this.LevelIncrease)) * final_Resource_Multiplier);

                goto end_result;

            }
            else if (this.IncreasedByLevelStartPlusDivStep)
            {
                if (this.StartingLevel <= classLevel)
                {
                    if (this.LevelStep == 0)
                    {
                        PFLog.Default.Error("LevelStep is 0. Can't divide by 0", Array.Empty<object>());
                        goto end_result;

                    }
                    else
                    {

                        num += Math.Max((int)((float)(this.StartingIncrease + this.PerStepIncrease * (classLevel - this.StartingLevel) / this.LevelStep) * final_Resource_Multiplier), this.MinClassLevelIncrease);
                        goto end_result;
                    }

                }

            }

        end_result:

            return num;


        }


        public void CalculateMaxResourceAmount(BlueprintAbilityResource resource, ref int bonus)
        {

            if (base.Fact.Active && resource == this.m_Resource.Get())
            {
                UnitDescriptor unit = base.Owner;

                int classlevel = 0;

                if (this.CharacterClass != null)
                {
                    classlevel = unit.Progression.GetClassLevel(this.CharacterClass);
                }
                if (this.Archetype != null)
                {
                    foreach (ClassData classData in base.Owner.Progression.Classes)
                    {
                        if (classData.CharacterClass != this.CharacterClass && classData.Archetypes.HasItem(this.Archetype))
                        {
                            classlevel += classData.Level;
                            break;
                        }
                    }
                }


                int resource_amount = CalculateBonusAmount(classlevel);


                if (Subtract)
                {
                    bonus -= resource_amount;
                }
                else
                {
                    bonus += resource_amount;
                }
            }


        }



        public BlueprintAbilityResourceReference m_Resource;

        public BlueprintCharacterClassReference m_CharacterClass;

        public BlueprintArchetypeReference m_Archetype;

        public bool UseResourceMultiplier = false;

        public bool UseResourceDivisor = false;

        [UsedImplicitly]
        public int BaseValue = 0;

        [UsedImplicitly]
        public bool IncreasedByLevel;

        [UsedImplicitly]
        [ShowIf("IncreasedByLevel")]
        public int LevelIncrease;

        [UsedImplicitly]
        public bool IncreasedByLevelStartPlusDivStep;

        [UsedImplicitly]
        [ShowIf("IncreasedByLevelStartPlusDivStep")]
        public int StartingLevel;

        [UsedImplicitly]
        [ShowIf("IncreasedByLevelStartPlusDivStep")]
        public int StartingIncrease;

        [UsedImplicitly]
        [ShowIf("IncreasedByLevelStartPlusDivStep")]
        public int LevelStep;

        [UsedImplicitly]
        [ShowIf("IncreasedByLevelStartPlusDivStep")]
        public int PerStepIncrease;


        [UsedImplicitly]
        [ShowIf("IncreasedByLevelStartPlusDivStep")]
        public int MinClassLevelIncrease;

        [UsedImplicitly]
        [ShowIf("UseResourceMultiplier")]
        public float ResourceMultiplier = 1.00f;        // This is a resource multiplier which is used to tweak the adjustment.

        [UsedImplicitly]
        [ShowIf("UseResourceDivisor")]
        public float ResourceDivisor = 1.00f;        // This is a resource divisor which is used to tweak the adjustment.

        public bool Subtract = false;

    }
}
