using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System.Diagnostics.Tracing;
using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Configuration;
using TomeOfDarkness.Utilities;
using TomeOfDarkness.NewComponents;


namespace TomeOfDarkness.NewContent.Features
{
    internal class Dispatchment
    {
        // This feature required the creation of the "AddContextFlankingAttackBonus" component.

        private static BlueprintCharacterClassReference[] RogueClassArray()
        {
            return new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };
        }



        public static void ConfigureDispatchment()
        {



            var DispatchmentIcon = BlueprintTools.GetBlueprint<BlueprintFeature>("422dab7309e1ad343935f33a4d6e9f11").Icon; // Dispatchment uses Outflank icon

            var DispatchmentFeature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "NinjaDispatchmentFeature", bp => {
                bp.SetName(ToDContext, "Dispatchment");
                bp.SetDescription(ToDContext, "At 4th level, whenever a ninja attacks an opponent that would be denied a Dexterity bonus to AC or when she flanks her target, she gains a +2 bonus on her attack roll. At 9th level and every 5 levels thereafter, this bonus increases by +1 (to a total maximum of +5).");
                bp.m_Icon = DispatchmentIcon;
                bp.AddComponent<AddContextFlankingAttackBonus>(c => {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Multiplier = 1;
                    c.Value = new ContextValue() { ValueType = ContextValueType.Rank };
                });
                bp.AddContextRankConfig(c => {
                    c.m_Type = AbilityRankType.Default;
                    c.m_Class = RogueClassArray();
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Progression = ContextRankProgression.StartPlusDivStep;
                    c.m_StartLevel = -1;
                    c.m_StepLevel = 5;
                });
                bp.IsClassFeature = true;
                bp.ReapplyOnLevelUp = true;
                bp.Ranks = 1;

            });

            ToDContext.Logger.LogPatch("Created Dispatchment.", DispatchmentFeature);


        }

    }
}
