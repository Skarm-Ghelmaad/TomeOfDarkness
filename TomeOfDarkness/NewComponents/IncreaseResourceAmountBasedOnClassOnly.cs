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

namespace TomeOfDarkness.NewComponents
{
    [TypeId("085B6F3B06B84D73AB94514BCF1FB168")]
    public class IncreaseResourceAmountBasedOnClassOnly : UnitFactComponentDelegate, IResourceAmountBonusHandler, IUnitSubscriber, ISubscriber
    {
        public BlueprintAbilityResource Resource
        {
            get
            {
                BlueprintAbilityResourceReference resource = this.m_Resource;
                if (resource == null)
                {
                    return null;
                }
                return resource.Get();
            }
        }

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

        public int CalculateBonusAmount(int classLevel)
        {
            int num = 0;

            float final_Resource_Multiplier = (this.UseResourceMultiplier ? this.ResourceMultiplier : 1.00f) / (this.UseResourceDivisor ? this.ResourceDivisor : 1.00f);


            if (IncreasedByLevel)
            {
                num = (int)((float)this.BaseValue + (classLevel * LevelIncrease) * final_Resource_Multiplier);

                goto end_result;

            }
            else if (IncreasedByLevelStartPlusDivStep)
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
                        num += Math.Max((int)((float)((this.StartingIncrease + this.PerStepIncrease * (classLevel - this.StartingLevel) / this.LevelStep)) * final_Resource_Multiplier), this.MinClassLevelIncrease);
                        goto end_result;
                    }

                }

            }

        end_result:

            return num;


        }

        public void CalculateMaxResourceAmount(BlueprintAbilityResource resource, ref int bonus)
        {

            if (base.Fact.Active && resource == m_Resource.Get())
            {
                UnitDescriptor unit = base.Owner;
                int classlevel = unit.Progression.GetClassLevel(CharacterClass);
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

        public bool UseResourceMultiplier = false;

        public bool UseResourceDivisor = false;

        [UsedImplicitly]
        [ShowIf("UseResourceMultiplier")]
        public float ResourceMultiplier = 1.00f;      // This is a resource multiplier which is used to tweak the adjustment.

        [UsedImplicitly]
        [ShowIf("UseResourceDivisor")]
        public float ResourceDivisor = 1.00f;        // This is a resource divisor which is used to tweak the adjustment.

        public bool Subtract = false;

    }
}
