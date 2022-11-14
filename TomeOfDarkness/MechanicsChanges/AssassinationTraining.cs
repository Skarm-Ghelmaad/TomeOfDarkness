using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using System.Diagnostics.Tracing;
using Kingmaker.EntitySystem.Stats;
using static TomeOfDarkness.Main;
using Kingmaker.UnitLogic.Mechanics.Components;
using TomeOfDarkness.NewComponents;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Abilities;
using TomeOfDarkness.Utilities;
using Kingmaker.UnitLogic.Abilities.Components;
using HlEX = TomeOfDarkness.Utilities.HelpersExtension;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem;
using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements.DamageResistance;
using TabletopTweaks.Core.NewComponents.Properties;
using TabletopTweaks.Base.NewContent.Features;
using System.Security.AccessControl;
using TomeOfDarkness.NewComponents.OwlcatReplacements;
using Kingmaker.RuleSystem;

namespace TomeOfDarkness.MechanicsChanges
{
    internal class AssassinationTraining
    {

        public static void ConfigureAssassinationTraining()
        {
            ConfigureAssassinationTrainingFakeLevelFeature();

            var Assassination_Training_FakeLevel = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "AssassinationTrainingFakeLevel");

            var AssassinationTrainingIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_AssassinationTraining.png");

            #region |-----------------------------------------------------/ CREATED ASSASSINATION TRAINING PROPERTY /-------------------------------------------------------------|

            var AssassinationTrainingProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>(ToDContext, "AssassinationTrainingProperty", bp =>
            {
                bp.AddComponent<CompositeCustomPropertyGetter>(c =>
                {
                    c.CalculationMode = CompositeCustomPropertyGetter.Mode.Sum;
                    c.Properties = new CompositeCustomPropertyGetter.ComplexCustomProperty[] {
                        new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = new FactRankGetter(){
                                m_Fact = Assassination_Training_FakeLevel.ToReference<BlueprintUnitFactReference>()
                            }
                        },
                        new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = new ClassLevelGetter(){
                                m_Class = ClassTools.ClassReferences.AssassinClass
                            }
                        }
                    };
                });
            });

            ToDContext.Logger.LogPatch("Created Assassination Training property.", AssassinationTrainingProperty);

            #endregion


        }

        public static void ConfigureAssassinationTrainingFakeLevelFeature()
        {

            var AssassinationTrainingIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_AssassinationTraining.png");


            var AssassinationTrainingFakeLevel = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "AssassinationTrainingFakeLevel", bp => {
                bp.IsClassFeature = true;
                bp.HideInUI = true;
                bp.Ranks = 40;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.SetName(ToDContext, "Assassination Training Fake Level");
                bp.SetDescription(ToDContext, "This feature grant fake levels that are considered to calculate his class level for purpose of the Assassinate feature.");
                bp.m_Icon = AssassinationTrainingIcon;
            });

            ToDContext.Logger.LogPatch("Created Assassination Training Fake Level.", AssassinationTrainingFakeLevel);

        }
    }
}
