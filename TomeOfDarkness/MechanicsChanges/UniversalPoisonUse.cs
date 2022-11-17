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
using TomeOfDarkness.NewComponents;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Abilities;
using TomeOfDarkness.Utilities;
using Kingmaker.UnitLogic.Abilities.Components;
using HlEX = TomeOfDarkness.Utilities.HelpersExtension;
using System.Drawing;
using Kingmaker.UnitLogic.Buffs;
using System;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System.Linq;
using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Core.NewComponents.Properties;
using TomeOfDarkness.NewComponents.Properties;
using TomeOfDarkness.MechanicsChanges;
using Kingmaker.UI.ServiceWindow.CharacterScreen;
using Kingmaker.ElementsSystem;
using TomeOfDarkness.NewActions;

namespace TomeOfDarkness.NewContent.Features
{
    internal class UniversalPoisonUse
    {

        public static void ConfigureUniversalPoisonUse()
        {

            PoisonCraftTraining.ConfigurePoisonCraftTraining();

            var Poison_Craft_Training_FakeLevel = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "PoisonCraftTrainingFakeLevel");
            var Poison_Craft_Training_Property = BlueprintTools.GetModBlueprint<BlueprintUnitProperty>(ToDContext, "PoisonCraftTrainingProperty");

            #region |----------------------------------------------------------| Create Generic PoisonCraft Stat |-------------------------------------------------------------|

            var PoisonCraftStrengthIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_PoisonUseRed.png");
            var PoisonCraftDexterityIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_PoisonUseYellow.png");
            var PoisonCraftConstitutionIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_PoisonUseGreen.png");
            var PoisonCraftIntelligenceIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_PoisonUseBlue.png");
            var PoisonCraftWisdomIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_PoisonUseWhite.png");
            var PoisonCraftCharismaIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_PoisonUsePurple.png");

            var Poisoncraft_Strength_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "PoisonCraftStrengthStatFeature", bp => {
                bp.SetName(ToDContext, "Poison Use - Strength");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Strength}Str{/g} modifier to his Poison Use's DC.");
                bp.m_Icon = PoisonCraftStrengthIcon;
                bp.Ranks = 1;
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
            });

            ToDContext.Logger.LogPatch("Created Poison Use - Strength feature.", Poisoncraft_Strength_Feature);

            var Poisoncraft_Dexterity_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "PoisonCraftDexterityStatFeature", bp => {
                bp.SetName(ToDContext, "Poison Use - Dexterity");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Dexterity}Dex{/g} modifier to his Poison Use's DC.");
                bp.m_Icon = PoisonCraftDexterityIcon;
                bp.Ranks = 1;
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
            });

            ToDContext.Logger.LogPatch("Created Poison Use - Dexterity feature.", Poisoncraft_Dexterity_Feature);

            var Poisoncraft_Constitution_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "PoisonCraftConstitutionStatFeature", bp => {
                bp.SetName(ToDContext, "Poison Use - Constitution");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Constitution}Con{/g} modifier to his Poison Use's DC.");
                bp.m_Icon = PoisonCraftConstitutionIcon;
                bp.Ranks = 1;
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
            });

            ToDContext.Logger.LogPatch("Created Poison Use - Constitution feature.", Poisoncraft_Constitution_Feature);

            var Poisoncraft_Intelligence_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "PoisonCraftIntelligenceStatFeature", bp => {
                bp.SetName(ToDContext, "Poison Use - Intelligence");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Intelligence}Int{/g} modifier to his Poison Use's DC.");
                bp.m_Icon = PoisonCraftIntelligenceIcon;
                bp.Ranks = 1;
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
            });

            ToDContext.Logger.LogPatch("Created Poison Use - Intelligence feature.", Poisoncraft_Intelligence_Feature);

            var Poisoncraft_Wisdom_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "PoisonCraftWisdomStatFeature", bp => {
                bp.SetName(ToDContext, "Poison Use - Wisdom");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Wisdom}Wis{/g} modifier to his Poison Use's DC.");
                bp.m_Icon = PoisonCraftWisdomIcon;
                bp.Ranks = 1;
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
            });

            ToDContext.Logger.LogPatch("Created Poison Use - Wisdom feature.", Poisoncraft_Wisdom_Feature);

            var Poisoncraft_Charisma_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "PoisonCraftCharismaStatFeature", bp => {
                bp.SetName(ToDContext, "Poison Use - Charisma");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Charisma}Cha{/g} modifier to his Poison Use's DC.");
                bp.m_Icon = PoisonCraftCharismaIcon;
                bp.Ranks = 1;
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
            });

            ToDContext.Logger.LogPatch("Created Poison Use - Charisma feature.", Poisoncraft_Charisma_Feature);

            var PoisonCraftStatProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>(ToDContext, "PoisonCraftStatProperty", bp =>
            {
                bp.AddComponent<CompositeCustomPropertyGetter>(c =>
                {
                    c.CalculationMode = CompositeCustomPropertyGetter.Mode.Highest;
                    c.Properties = new CompositeCustomPropertyGetter.ComplexCustomProperty[] {
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreatePropertyMulipliedByFactRankGetter(UnitProperty.StatStrength, Poisoncraft_Strength_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreatePropertyMulipliedByFactRankGetter(UnitProperty.StatDexterity, Poisoncraft_Dexterity_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreatePropertyMulipliedByFactRankGetter(UnitProperty.StatConstitution, Poisoncraft_Constitution_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreatePropertyMulipliedByFactRankGetter(UnitProperty.StatIntelligence, Poisoncraft_Intelligence_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreatePropertyMulipliedByFactRankGetter(UnitProperty.StatWisdom, Poisoncraft_Wisdom_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreatePropertyMulipliedByFactRankGetter(UnitProperty.StatCharisma, Poisoncraft_Charisma_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                        };
                    });
                bp.BaseValue = 0;
            });

            ToDContext.Logger.LogPatch("Created Poisoncraft Stat property.", PoisonCraftStatProperty);

            #endregion

            #region |-----------------------------------------------------| Create Poison Toxicity Adjustment Property |-------------------------------------------------------|

            // This property is used to raise or lower the DC of Poison Use based on features or buffs possessed.

            var Universal_Create_Poison_Toxicity_Boost_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonToxicityBoostFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Toxicity Boost");
                bp.SetDescription(ToDContext, "Each rank of this feature add +1 to the Poison Use's DC.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Toxicity Boost feature.", Universal_Create_Poison_Toxicity_Boost_Feature);

            var Universal_Create_Poison_Toxicity_Boost_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonToxicityBoostBluff", bp => {
                bp.SetName(ToDContext, "Create Poison - Toxicity Boost");
                bp.SetDescription(ToDContext, "Each rank of this buff add +1 to the Poison Use's DC.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Toxicity Boost buff.", Universal_Create_Poison_Toxicity_Boost_Buff);

            var Universal_Create_Poison_Toxicity_Reduction_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonToxicityReductionFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Toxicity Reduction");
                bp.SetDescription(ToDContext, "Each rank of this feature adds -1 to the Poison Use's DC.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Toxicity Reduction feature.", Universal_Create_Poison_Toxicity_Reduction_Feature);

            var Universal_Create_Poison_Toxicity_Reduction_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonToxicityReductionBluff", bp => {
                bp.SetName(ToDContext, "Create Poison - Toxicity Reduction");
                bp.SetDescription(ToDContext, "Each rank of this buff adds -1 to the Poison Use's DC.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Toxicity Reduction feature.", Universal_Create_Poison_Toxicity_Reduction_Buff);

            var Create_Poison_Toxicity_Adjustment_Property = Helpers.CreateBlueprint<BlueprintUnitProperty>(ToDContext, "CreatePoisonToxicityAdjustmentProperty", bp =>
            {
                bp.AddComponent<CompositeCustomPropertyGetter>(c =>
                {
                    c.CalculationMode = CompositeCustomPropertyGetter.Mode.Sum;
                    c.Properties = new CompositeCustomPropertyGetter.ComplexCustomProperty[] {
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Toxicity_Boost_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Toxicity_Boost_Buff.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Toxicity_Reduction_Feature.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Toxicity_Reduction_Buff.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            }
                        };
                });
                bp.BaseValue = 0;
            });

            ToDContext.Logger.LogPatch("Added Create Poison Toxicity Adjustment property.", Create_Poison_Toxicity_Adjustment_Property);

            #endregion

            #region |------------------------------------------------------| Create Poison Duration Adjustment Property |------------------------------------------------------|

            // This property is used to raise or lower the frequency of Poison Use based on features or buffs possessed.

            var Universal_Create_Poison_Duration_Boost_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonDurationBoostFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Duration Boost");
                bp.SetDescription(ToDContext, "Each rank of this feature add +1 to the Poison Use's frequency (duration).");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Duration Boost feature.", Universal_Create_Poison_Duration_Boost_Feature);

            var Universal_Create_Poison_Duration_Boost_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonDurationyBoostBluff", bp => {
                bp.SetName(ToDContext, "Create Poison - Duration Boost");
                bp.SetDescription(ToDContext, "Each rank of this feature add +1 to the Poison Use's frequency (duration).");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Duration Boost buff.", Universal_Create_Poison_Duration_Boost_Buff);

            var Universal_Create_Poison_Duration_Reduction_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonDurationyReductionFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Duration Reduction");
                bp.SetDescription(ToDContext, "Each rank of this feature add -1 to the Poison Use's frequency (duration).");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Duration Reduction feature.", Universal_Create_Poison_Duration_Reduction_Feature);

            var Universal_Create_Poison_Duration_Reduction_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonDurationyBoostBluff", bp => {
                bp.SetName(ToDContext, "Create Poison - Duration Reduction");
                bp.SetDescription(ToDContext, "Each rank of this feature add -1 to the Poison Use's frequency (duration).");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Duration Reduction buff.", Universal_Create_Poison_Duration_Reduction_Buff);

            var Create_Poison_Duration_Adjustment_Property = Helpers.CreateBlueprint<BlueprintUnitProperty>(ToDContext, "CreatePoisonDurationAdjustmentProperty", bp =>
            {
                bp.AddComponent<CompositeCustomPropertyGetter>(c =>
                {
                    c.CalculationMode = CompositeCustomPropertyGetter.Mode.Sum;
                    c.Properties = new CompositeCustomPropertyGetter.ComplexCustomProperty[] {
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Duration_Boost_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Duration_Boost_Buff.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Duration_Reduction_Feature.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Duration_Reduction_Buff.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            }
                        };
                });
                bp.BaseValue = 0;
            });

            ToDContext.Logger.LogPatch("Added Create Poison Duration Adjustment property.", Create_Poison_Duration_Adjustment_Property);

            #endregion

            #region |---------------------------------------------------| Create Poison Recovery Adjustment Property |---------------------------------------------------------|

            // This property is used to raise or lower the number of saves of Poison Use based on features or buffs possessed.

            var Universal_Create_Poison_Recovery_Worsen_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonRecoveryWorsenFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Worsen Recovery");
                bp.SetDescription(ToDContext, "Each rank of this feature adds +1 save to recover from Poison Use.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Worsen Recovery feature.", Universal_Create_Poison_Recovery_Worsen_Feature);

            var Universal_Create_Poison_Recovery_Worsen_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonRecoveryWorsenBluff", bp => {
                bp.SetName(ToDContext, "Create Poison - Worsen Recovery");
                bp.SetDescription(ToDContext, "Each rank of this buff adds +1 save to recover from Poison Use.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Worsen Recovery buff.", Universal_Create_Poison_Recovery_Worsen_Buff);

            var Universal_Create_Poison_Recovery_Improve_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonRecoveryImproveFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Improve Recovery");
                bp.SetDescription(ToDContext, "Each rank of this feature adds -1 save to recover from Poison Use.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Worsen Recovery buff.", Universal_Create_Poison_Recovery_Worsen_Buff);

            var Universal_Create_Poison_Recovery_Improve_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonRecoveryImproveBuff", bp => {
                bp.SetName(ToDContext, "Create Poison - Improve Recovery");
                bp.SetDescription(ToDContext, "Each rank of this buff adds -1 save to recover from Poison Use.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Improve Recovery buff.", Universal_Create_Poison_Recovery_Improve_Buff);

            var Create_Poison_Recovery_Adjustment_Property = Helpers.CreateBlueprint<BlueprintUnitProperty>(ToDContext, "CreatePoisonRecoveryAdjustmentProperty", bp =>
            {
                bp.AddComponent<CompositeCustomPropertyGetter>(c =>
                {
                    c.CalculationMode = CompositeCustomPropertyGetter.Mode.Sum;
                    c.Properties = new CompositeCustomPropertyGetter.ComplexCustomProperty[] {
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Recovery_Worsen_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Recovery_Worsen_Buff.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Recovery_Improve_Feature.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Recovery_Improve_Buff.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            }
                        };
                });
                bp.BaseValue = 0;
            });

            ToDContext.Logger.LogPatch("Added Create Poison Recovery Adjustment property.", Create_Poison_Recovery_Adjustment_Property);

            #endregion

            #region |------------------------------------------------| Create Poison Stickiness Adjustment Property |----------------------------------------------------------|


            var Universal_Create_Poison_Stickiness_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonStickinessFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Stickiness");
                bp.SetDescription(ToDContext, "Each rank of this feature adds +1 attack infused with Poison Use.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Stickiness feature.", Universal_Create_Poison_Stickiness_Feature);

            var Universal_Create_Poison_Stickiness_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonStickinessBuff", bp => {
                bp.SetName(ToDContext, "Create Poison - Stickiness");
                bp.SetDescription(ToDContext, "Each rank of this buff adds +1 attack infused with Poison Use.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Stickiness buff.", Universal_Create_Poison_Stickiness_Buff);

            var Universal_Create_Poison_Stickiness_Boost_Strength_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonStickinessBoostStrengthFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Stickiness Boost (Strength)");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Strength}Str{/g} modifier to attacks infused with Poison Use.");
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Stickiness Boost (Strength) feature.", Universal_Create_Poison_Stickiness_Boost_Strength_Feature);

            var Universal_Create_Poison_Stickiness_Boost_Dexterity_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonStickinessBoostDexterityFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Stickiness Boost (Dexterity)");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Dexterity}Dex{/g} modifier to attacks infused with Poison Use.");
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Stickiness Boost (Dexterity) feature.", Universal_Create_Poison_Stickiness_Boost_Dexterity_Feature);

            var Universal_Create_Poison_Stickiness_Boost_Constitution_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonStickinessBoostConstitutionFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Stickiness Boost (Constitution)");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Constitution}Con{/g} modifier to attacks infused with Poison Use.");
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });



            var Universal_Create_Poison_Stickiness_Boost_Intelligence_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonStickinessBoostIntelligenceFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Stickiness Boost (Intelligence)");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Intelligence}Int{/g} modifier to attacks infused with Poison Use.");
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Stickiness Boost (Intelligence) feature.", Universal_Create_Poison_Stickiness_Boost_Intelligence_Feature);

            var Universal_Create_Poison_Stickiness_Boost_Wisdom_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonStickinessBoostWisdomFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Stickiness Boost (Wisdom)");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Wisdom}Wis{/g} modifier to attacks infused with Poison Use.");
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Stickiness Boost (Wisdom) feature.", Universal_Create_Poison_Stickiness_Boost_Wisdom_Feature);



            var Universal_Create_Poison_Stickiness_Boost_Charisma_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonStickinessBoostCharismaFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Stickiness Boost (Charisma)");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Charisma}Cha{/g} modifier to attacks infused with Poison Use.");
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Stickiness Boost (Charisma) feature.", Universal_Create_Poison_Stickiness_Boost_Charisma_Feature);

            var CreatePoisonStickinessProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>(ToDContext, "CreatePoisonStickinessProperty", bp =>
            {
                bp.AddComponent<CompositeCustomPropertyGetter>(c =>
                {
                    c.CalculationMode = CompositeCustomPropertyGetter.Mode.Sum;
                    c.Properties = new CompositeCustomPropertyGetter.ComplexCustomProperty[] {
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreatePropertyMulipliedByFactRankGetter(UnitProperty.StatStrength, Universal_Create_Poison_Stickiness_Boost_Strength_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreatePropertyMulipliedByFactRankGetter(UnitProperty.StatDexterity, Universal_Create_Poison_Stickiness_Boost_Dexterity_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreatePropertyMulipliedByFactRankGetter(UnitProperty.StatConstitution, Universal_Create_Poison_Stickiness_Boost_Constitution_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreatePropertyMulipliedByFactRankGetter(UnitProperty.StatIntelligence, Universal_Create_Poison_Stickiness_Boost_Intelligence_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreatePropertyMulipliedByFactRankGetter(UnitProperty.StatWisdom, Universal_Create_Poison_Stickiness_Boost_Wisdom_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreatePropertyMulipliedByFactRankGetter(UnitProperty.StatCharisma, Universal_Create_Poison_Stickiness_Boost_Charisma_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Stickiness_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Stickiness_Buff.ToReference<BlueprintUnitFactReference>())
                            }
                        };
                    bp.BaseValue = 1;
                });
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Stickiness property.", CreatePoisonStickinessProperty);

            #endregion

            #region |-----------------------------------------------| Create Poison Concentration Adjustment Property |--------------------------------------------------------|

            var Universal_Create_Poison_Concentration_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonConcentrationFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Concentration");
                bp.SetDescription(ToDContext, "Each rank of this feature adds +1 dose infused with Poison Use.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Concentration feature.", Universal_Create_Poison_Concentration_Feature);

            var Universal_Create_Poison_Concentration_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonConcentrationBuff", bp => {
                bp.SetName(ToDContext, "Create Poison - Concentration");
                bp.SetDescription(ToDContext, "Each rank of this buff adds +1 dose infused with Poison Use.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Concentration feature.", Universal_Create_Poison_Concentration_Buff);

            var CreatePoisonConcentrationProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>(ToDContext, "CreatePoisonConcentrationProperty", bp =>
            {
                bp.AddComponent<CompositeCustomPropertyGetter>(c =>
                {
                    c.CalculationMode = CompositeCustomPropertyGetter.Mode.Sum;
                    c.Properties = new CompositeCustomPropertyGetter.ComplexCustomProperty[] {
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Concentration_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Concentration_Buff.ToReference<BlueprintUnitFactReference>())
                            }
                        };
                    bp.BaseValue = 1;
                });
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Concentration property.", CreatePoisonConcentrationProperty);

            #endregion

            #region |----------------------------------------------------| Make Assassin's Poison Use Based On Properties |----------------------------------------------------|

            //[CHECKED]

            var Assassin_Create_Poison_Increased_Duration = BlueprintTools.GetBlueprint<BlueprintFeature>("953e47bbeb7c55145884de118f812b28");                       //[CHECKED]
            var Assassin_Create_Poison_Increased_Saving_Throws = BlueprintTools.GetBlueprint<BlueprintFeature>("0cbb2201a65cb374fb4d73d6c9830b01");                  //[CHECKED]

            var Assassin_Create_Poison_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("8dd826513ba857645b38e918f17b59e6");                                  //[CHECKED]
            var Assassin_Create_Poison_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("46660d0da7797124aa221818778edc9d");                                  //[CHECKED]
            var Assassin_Create_Poison_Ability_Str = BlueprintTools.GetBlueprint<BlueprintAbility>("67aa1843adeed0346a30125c29d8df8b");                              //[CHECKED]
            var Assassin_Create_Poison_Ability_Dex = BlueprintTools.GetBlueprint<BlueprintAbility>("3fbbc2843598d8146b1ca3415df6ecdd");                              //[CHECKED]
            var Assassin_Create_Poison_Ability_Con = BlueprintTools.GetBlueprint<BlueprintAbility>("5a72db7750919864ba9cc2afa335fd2f");                              //[CHECKED]

            var Assassin_Create_Poison_Swift_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("bb7b571cadb6cc147a52431385a40a0d");                            //[CHECKED]
            var Assassin_Create_Poison_Swift_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("eadfd2e82d4e3684893543668aa55312");                            //[CHECKED]
            var Assassin_Create_Poison_Ability_Swift_Str = BlueprintTools.GetBlueprint<BlueprintAbility>("ca95feaf38593534f91becc31d097756");                        //[CHECKED]
            var Assassin_Create_Poison_Ability_Swift_Dex = BlueprintTools.GetBlueprint<BlueprintAbility>("e7730af415f91c44e9306e1aa15c5a85");                        //[CHECKED]
            var Assassin_Create_Poison_Ability_Swift_Con = BlueprintTools.GetBlueprint<BlueprintAbility>("eff246d4ceae9b14e8bc4a99fe508808");                        //[CHECKED]

            var Assassin_Create_Poison_Ability_Property = BlueprintTools.GetBlueprint<BlueprintUnitProperty>("0482fffc039d46fc86a86bda03e00f1a");                    //[CHECKED]

            var Assassin_Create_Poison_Ability_Str_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("c219da8e56fb30045bb69247c695b8c8");                            //[CHECKED]
            var Assassin_Create_Poison_Ability_Dex_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("385c07aa91442094f9385510504dde3c");                            //[CHECKED]
            var Assassin_Create_Poison_Ability_Con_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("ac4d4b3f14f2b6e41a19a3d8e13e7ee7");                            //[CHECKED]
            var Assassin_Create_Poison_Ability_Str_Buff_Effect = BlueprintTools.GetBlueprint<BlueprintBuff>("285290cc80941bc4c97461d6f50aaad9");                     //[CHECKED]
            var Assassin_Create_Poison_Ability_Dex_Buff_Effect = BlueprintTools.GetBlueprint<BlueprintBuff>("c766f0606ac12e24e8a9fdb8beabc6c2");                     //[CHECKED]
            var Assassin_Create_Poison_Ability_Con_Buff_Effect = BlueprintTools.GetBlueprint<BlueprintBuff>("6542e025d84501a41b652bcdc57f6901");                     //[CHECKED]

            Assassin_Create_Poison_Increased_Duration.AddComponent(HlEX.CreateAddFacts(new BlueprintUnitFactReference[] { Universal_Create_Poison_Duration_Boost_Feature.ToReference<BlueprintUnitFactReference>() }));

            ToDContext.Logger.LogPatch("Changed Assassin Create Poison Increased Duration to add a generic duration boost.", Assassin_Create_Poison_Increased_Duration);

            Assassin_Create_Poison_Increased_Saving_Throws.AddComponent(HlEX.CreateAddFacts(new BlueprintUnitFactReference[] { Universal_Create_Poison_Recovery_Worsen_Feature.ToReference<BlueprintUnitFactReference>() }));

            ToDContext.Logger.LogPatch("Changed Assassin Create Poison Increased Saving Throws to add a generic increase of saving throws.", Assassin_Create_Poison_Increased_Saving_Throws);

            Assassin_Create_Poison_Ability_Property.RemoveComponents<ClassLevelGetter>();
            Assassin_Create_Poison_Ability_Property.RemoveComponents<SimplePropertyGetter>();
            Assassin_Create_Poison_Ability_Property.AddComponent(HlEX.CreateCustomPropertyGetter(Poison_Craft_Training_Property));
            Assassin_Create_Poison_Ability_Property.AddComponent(HlEX.CreateCustomPropertyGetter(PoisonCraftStatProperty));

            ToDContext.Logger.LogPatch("Altered Assassin Create Poison Ability Property to use Custom Property Getters instead of the original Class Level Getter and Simple Property Getter", Assassin_Create_Poison_Ability_Property);

            var cr_po_ab_st_comp = Assassin_Create_Poison_Ability_Str.GetComponents<ContextRankConfig>().ToArray();
            var cr_po_ab_dx_comp = Assassin_Create_Poison_Ability_Dex.GetComponents<ContextRankConfig>().ToArray();
            var cr_po_ab_co_comp = Assassin_Create_Poison_Ability_Con.GetComponents<ContextRankConfig>().ToArray();
            var cr_po_ab_st_comp2 = Assassin_Create_Poison_Ability_Swift_Str.GetComponents<ContextRankConfig>().ToArray();
            var cr_po_ab_dx_comp2 = Assassin_Create_Poison_Ability_Swift_Dex.GetComponents<ContextRankConfig>().ToArray();
            var cr_po_ab_co_comp2 = Assassin_Create_Poison_Ability_Swift_Con.GetComponents<ContextRankConfig>().ToArray();

            foreach (var cmp in cr_po_ab_st_comp)
            {
                if (cmp.m_BaseValueType == ContextRankBaseValueType.ClassLevel)
                {
                    cmp.TemporaryContext(c => {
                        c.m_Class = new BlueprintCharacterClassReference[0];
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = Poison_Craft_Training_Property.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
                else
                {
                    cmp.TemporaryContext(c => {
                        c.m_Stat = StatType.Unknown;
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = PoisonCraftStatProperty.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Strength Ability to use Custom Properties", Assassin_Create_Poison_Ability_Str);

            foreach (var cmp in cr_po_ab_st_comp2)
            {
                if (cmp.m_BaseValueType == ContextRankBaseValueType.ClassLevel)
                {
                    cmp.TemporaryContext(c => {
                        c.m_Class = new BlueprintCharacterClassReference[0];
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = Poison_Craft_Training_Property.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
                else
                {
                    cmp.TemporaryContext(c => {
                        c.m_Stat = StatType.Unknown;
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = PoisonCraftStatProperty.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Strength (Swift) Ability to use Custom Properties", Assassin_Create_Poison_Ability_Swift_Str);
            
            foreach (var cmp in cr_po_ab_dx_comp)
            {
                if (cmp.m_BaseValueType == ContextRankBaseValueType.ClassLevel)
                {
                    cmp.TemporaryContext(c => {
                        c.m_Class = new BlueprintCharacterClassReference[0];
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = Poison_Craft_Training_Property.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
                else
                {
                    cmp.TemporaryContext(c => {
                        c.m_Stat = StatType.Unknown;
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = PoisonCraftStatProperty.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Dexterity Ability to use Custom Properties", Assassin_Create_Poison_Ability_Dex);

            foreach (var cmp in cr_po_ab_dx_comp2)
            {
                if (cmp.m_BaseValueType == ContextRankBaseValueType.ClassLevel)
                {
                    cmp.TemporaryContext(c => {
                        c.m_Class = new BlueprintCharacterClassReference[0];
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = Poison_Craft_Training_Property.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
                else
                {
                    cmp.TemporaryContext(c => {
                        c.m_Stat = StatType.Unknown;
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = PoisonCraftStatProperty.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Dexterity Ability (Swift) to use Custom Properties", Assassin_Create_Poison_Ability_Swift_Dex);

            foreach (var cmp in cr_po_ab_co_comp)
            {
                if (cmp.m_BaseValueType == ContextRankBaseValueType.ClassLevel)
                {
                    cmp.TemporaryContext(c => {
                        c.m_Class = new BlueprintCharacterClassReference[0];
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = Poison_Craft_Training_Property.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
                else
                {
                    cmp.TemporaryContext(c => {
                        c.m_Stat = StatType.Unknown;
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = PoisonCraftStatProperty.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Constitution Ability to use Custom Properties", Assassin_Create_Poison_Ability_Con);

            foreach (var cmp in cr_po_ab_co_comp2)
            {
                if (cmp.m_BaseValueType == ContextRankBaseValueType.ClassLevel)
                {
                    cmp.TemporaryContext(c => {
                        c.m_Class = new BlueprintCharacterClassReference[0];
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = Poison_Craft_Training_Property.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
                else
                {
                    cmp.TemporaryContext(c => {
                        c.m_Stat = StatType.Unknown;
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = PoisonCraftStatProperty.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Constitution (Swift) Ability to use Custom Properties", Assassin_Create_Poison_Ability_Swift_Con);

            Assassin_Create_Poison_Ability_Str_Buff.GetComponent<ContextRankConfig>().TemporaryContext(c => {
                c.m_Feature = null;
                c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                c.m_CustomProperty = Create_Poison_Duration_Adjustment_Property.ToReference<BlueprintUnitPropertyReference>();
            });

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Strength Ability Buff to use Custom Properties", Assassin_Create_Poison_Ability_Str_Buff);

            Assassin_Create_Poison_Ability_Dex_Buff.GetComponent<ContextRankConfig>().TemporaryContext(c => {
                c.m_Feature = null;
                c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                c.m_CustomProperty = Create_Poison_Duration_Adjustment_Property.ToReference<BlueprintUnitPropertyReference>();
            });

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Dexterity Ability Buff to use Custom Properties", Assassin_Create_Poison_Ability_Dex_Buff);

            Assassin_Create_Poison_Ability_Con_Buff.GetComponent<ContextRankConfig>().TemporaryContext(c => {
                c.m_Feature = null;
                c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                c.m_CustomProperty = Create_Poison_Duration_Adjustment_Property.ToReference<BlueprintUnitPropertyReference>();
            });

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Constitution Ability Buff to use Custom Properties", Assassin_Create_Poison_Ability_Con_Buff);


            var cr_po_ab_ef_bf_st_comp = Assassin_Create_Poison_Ability_Str_Buff_Effect.GetComponents<ContextRankConfig>().ToArray();
            var cr_po_ab_ef_bf_dx_comp = Assassin_Create_Poison_Ability_Dex_Buff_Effect.GetComponents<ContextRankConfig>().ToArray();
            var cr_po_ab_ef_bf_co_comp = Assassin_Create_Poison_Ability_Con_Buff_Effect.GetComponents<ContextRankConfig>().ToArray();

            foreach (var cmp in cr_po_ab_ef_bf_st_comp)
            {
                if (cmp.m_Type == AbilityRankType.Default)
                {
                    cmp.TemporaryContext(c => {
                        c.m_Feature = null;
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = Create_Poison_Duration_Adjustment_Property.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
                else
                {
                    cmp.TemporaryContext(c => {
                        c.m_Feature = null;
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = Create_Poison_Recovery_Adjustment_Property.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Strength Ability Effect Buff to use Custom Properties", Assassin_Create_Poison_Ability_Str_Buff_Effect);

            foreach (var cmp in cr_po_ab_ef_bf_dx_comp)
            {
                if (cmp.m_Type == AbilityRankType.Default)
                {
                    cmp.TemporaryContext(c => {
                        c.m_Feature = null;
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = Create_Poison_Duration_Adjustment_Property.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
                else
                {
                    cmp.TemporaryContext(c => {
                        c.m_Feature = null;
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = Create_Poison_Recovery_Adjustment_Property.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Dexterity Ability Effect Buff to use Custom Properties", Assassin_Create_Poison_Ability_Dex_Buff_Effect);

            foreach (var cmp in cr_po_ab_ef_bf_co_comp)
            {
                if (cmp.m_Type == AbilityRankType.Default)
                {
                    cmp.TemporaryContext(c => {
                        c.m_Feature = null;
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = Create_Poison_Duration_Adjustment_Property.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
                else
                {
                    cmp.TemporaryContext(c => {
                        c.m_Feature = null;
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = Create_Poison_Recovery_Adjustment_Property.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Constitution Ability Effect Buff to use Custom Properties", Assassin_Create_Poison_Ability_Con_Buff_Effect);


            #endregion

            #region |----------------------------------------------------|  Make Assassin's Poison Usable on Any Weapon |------------------------------------------------------|

            var cr_po_ab_bf_st_comp = Assassin_Create_Poison_Ability_Str_Buff.GetComponents<AddInitiatorAttackWithWeaponTrigger>().ToArray();
            var cr_po_ab_bf_dx_comp = Assassin_Create_Poison_Ability_Dex_Buff.GetComponents<AddInitiatorAttackWithWeaponTrigger>().ToArray();
            var cr_po_ab_bf_co_comp = Assassin_Create_Poison_Ability_Con_Buff.GetComponents<AddInitiatorAttackWithWeaponTrigger>().ToArray();

            foreach (var cmp in cr_po_ab_bf_st_comp)
            {
                cmp.TemporaryContext(c => {
                    c.RangeType = (WeaponRangeType)0;
                    c.CheckWeaponRangeType = false;
                });

            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Strength Ability Buff to allow for any weapon range (not just melee).", Assassin_Create_Poison_Ability_Str_Buff);


            foreach (var cmp in cr_po_ab_bf_dx_comp)
            {
                cmp.TemporaryContext(c => {
                    c.RangeType = (WeaponRangeType)0;
                    c.CheckWeaponRangeType = false;
                });

            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Dexterity Ability Buff to allow for any weapon range (not just melee).", Assassin_Create_Poison_Ability_Dex_Buff);

            foreach (var cmp in cr_po_ab_bf_co_comp)
            {
                cmp.TemporaryContext(c => {
                    c.RangeType = (WeaponRangeType)0;
                    c.CheckWeaponRangeType = false;
                });

            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Constitution Ability Buff to allow for any weapon range (not just melee).", Assassin_Create_Poison_Ability_Con_Buff);

            #endregion

            #region |-------------------------------------------|  Make Assassin's Poison Variable "Stickiness" Based On Property |--------------------------------------------|

            // Used factors: m_Type = "Default" (Duration)
            // Use "ProjectilesCount" for Stickiness.

            Assassin_Create_Poison_Ability_Str_Buff.Stacking = StackingType.Rank;
            Assassin_Create_Poison_Ability_Dex_Buff.Stacking = StackingType.Rank;
            Assassin_Create_Poison_Ability_Con_Buff.Stacking = StackingType.Rank;

            // Replace Apply Buff Action with Apply Buff Ranks Action [Poison Use — Strength Ability]

            var cr_po_ab_str_appl_bf_action = Assassin_Create_Poison_Ability_Str.FlattenAllActions().OfType<ContextActionApplyBuff>().FirstOrDefault();
            var cr_po_ab_str_cont_dur_comp = cr_po_ab_str_appl_bf_action.DurationValue;
            var Assassin_Create_Poison_Ability_Str_Stickiness = Helpers.CreateContextRankConfig(ContextRankBaseValueType.CustomProperty, ContextRankProgression.AsIs, AbilityRankType.ProjectilesCount, 1, null, 0, 0, false, StatType.Unknown, CreatePoisonStickinessProperty);
            Assassin_Create_Poison_Ability_Str.AddComponent(Assassin_Create_Poison_Ability_Str_Stickiness);
            var Action_Apply_Buff_Ranks_Assassin_Create_Poison_Ability_Str = HlEX.CreateContextActionApplyBuffRanks(cr_po_ab_str_appl_bf_action.Buff, cr_po_ab_str_cont_dur_comp, HlEX.CreateContextValue(AbilityRankType.ProjectilesCount), cr_po_ab_str_appl_bf_action.IsFromSpell, !cr_po_ab_str_appl_bf_action.IsNotDispelable, cr_po_ab_str_appl_bf_action.ToCaster, cr_po_ab_str_appl_bf_action.AsChild, cr_po_ab_str_appl_bf_action.Permanent, !cr_po_ab_str_appl_bf_action.NotLinkToAreaEffect);
            Assassin_Create_Poison_Ability_Str.ReplaceComponents<AbilityEffectRunAction>(HlEX.CreateRunActions(Action_Apply_Buff_Ranks_Assassin_Create_Poison_Ability_Str));

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Strength Ability to apply buff ranks instead of apply buff.", Assassin_Create_Poison_Ability_Str);

            // Replace Apply Buff Action with Apply Buff Ranks Action [Poison Use — Strength Ability (Swift)]

            var cr_po_ab_str_swf_appl_bf_action = Assassin_Create_Poison_Ability_Swift_Str.FlattenAllActions().OfType<ContextActionApplyBuff>().FirstOrDefault();
            var cr_po_ab_str_swf_cont_dur_comp = cr_po_ab_str_appl_bf_action.DurationValue;
            var Assassin_Create_Poison_Ability_Swift_Str_Stickiness = Helpers.CreateContextRankConfig(ContextRankBaseValueType.CustomProperty, ContextRankProgression.AsIs, AbilityRankType.ProjectilesCount, 1, null, 0, 0, false, StatType.Unknown, CreatePoisonStickinessProperty);
            Assassin_Create_Poison_Ability_Swift_Str.AddComponent(Assassin_Create_Poison_Ability_Swift_Str_Stickiness);
            var Action_Apply_Buff_Ranks_Assassin_Create_Poison_Ability_Swift_Str = HlEX.CreateContextActionApplyBuffRanks(cr_po_ab_str_swf_appl_bf_action.Buff, cr_po_ab_str_swf_cont_dur_comp, HlEX.CreateContextValue(AbilityRankType.ProjectilesCount), cr_po_ab_str_swf_appl_bf_action.IsFromSpell, !cr_po_ab_str_swf_appl_bf_action.IsNotDispelable, cr_po_ab_str_swf_appl_bf_action.ToCaster, cr_po_ab_str_swf_appl_bf_action.AsChild, cr_po_ab_str_swf_appl_bf_action.Permanent, !cr_po_ab_str_swf_appl_bf_action.NotLinkToAreaEffect);
            Assassin_Create_Poison_Ability_Swift_Str.ReplaceComponents<AbilityEffectRunAction>(HlEX.CreateRunActions(Action_Apply_Buff_Ranks_Assassin_Create_Poison_Ability_Swift_Str));

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Strength (Swift) Ability to apply buff ranks instead of apply buff.", Assassin_Create_Poison_Ability_Swift_Str);

            // Replace ContextActionRemoveSelf with ContextActionRemoveBuff (with Remove Ranks)

            var Remove_Assassin_Create_Poison_Ability_Str_Buff_Rank_Action = HlEX.CreateContextActionRemoveBuffRank(Assassin_Create_Poison_Ability_Str_Buff);

            foreach (var cmp in cr_po_ab_bf_st_comp)
            {
                var actions_to_alter = cmp.Action;
                var actions_to_remove = actions_to_alter.Actions.OfType<ContextActionRemoveSelf>().ToArray();
                foreach (var action in actions_to_remove)
                {
                     actions_to_alter.Actions = actions_to_alter.Actions.RemoveFromArray(action);
                }
                actions_to_alter.AddAction(Remove_Assassin_Create_Poison_Ability_Str_Buff_Rank_Action);
            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Strength Ability Buff to remove ranks instead all the buff.", Assassin_Create_Poison_Ability_Str_Buff);

            // Replace Apply Buff Action with Apply Buff Ranks Action [Poison Use — Dexterity Ability]

            var cr_po_ab_dex_appl_bf_action = Assassin_Create_Poison_Ability_Dex.FlattenAllActions().OfType<ContextActionApplyBuff>().FirstOrDefault();
            var cr_po_ab_dex_cont_dur_comp = cr_po_ab_dex_appl_bf_action.DurationValue;
            var Assassin_Create_Poison_Ability_Dex_Stickiness = Helpers.CreateContextRankConfig(ContextRankBaseValueType.CustomProperty, ContextRankProgression.AsIs, AbilityRankType.ProjectilesCount, 1, null, 0, 0, false, StatType.Unknown, CreatePoisonStickinessProperty);
            Assassin_Create_Poison_Ability_Dex.AddComponent(Assassin_Create_Poison_Ability_Dex_Stickiness);
            var Action_Apply_Buff_Ranks_Assassin_Create_Poison_Ability_Dex = HlEX.CreateContextActionApplyBuffRanks(cr_po_ab_dex_appl_bf_action.Buff, cr_po_ab_dex_cont_dur_comp, HlEX.CreateContextValue(AbilityRankType.ProjectilesCount), cr_po_ab_dex_appl_bf_action.IsFromSpell, !cr_po_ab_dex_appl_bf_action.IsNotDispelable, cr_po_ab_dex_appl_bf_action.ToCaster, cr_po_ab_dex_appl_bf_action.AsChild, cr_po_ab_dex_appl_bf_action.Permanent, !cr_po_ab_dex_appl_bf_action.NotLinkToAreaEffect);
            Assassin_Create_Poison_Ability_Dex.ReplaceComponents<AbilityEffectRunAction>(HlEX.CreateRunActions(Action_Apply_Buff_Ranks_Assassin_Create_Poison_Ability_Dex));

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Dexterity Ability to apply buff ranks instead of apply buff.", Assassin_Create_Poison_Ability_Dex);

            // Replace Apply Buff Action with Apply Buff Ranks Action [Poison Use — Dexterity Ability (Swift)]

            var cr_po_ab_dex_swf_appl_bf_action = Assassin_Create_Poison_Ability_Swift_Dex.FlattenAllActions().OfType<ContextActionApplyBuff>().FirstOrDefault();
            var cr_po_ab_dex_swf_cont_dur_comp = cr_po_ab_dex_swf_appl_bf_action.DurationValue;
            var Assassin_Create_Poison_Ability_Swift_Dex_Stickiness = Helpers.CreateContextRankConfig(ContextRankBaseValueType.CustomProperty, ContextRankProgression.AsIs, AbilityRankType.ProjectilesCount, 1, null, 0, 0, false, StatType.Unknown, CreatePoisonStickinessProperty);
            Assassin_Create_Poison_Ability_Swift_Dex.AddComponent(Assassin_Create_Poison_Ability_Swift_Dex_Stickiness);
            var Action_Apply_Buff_Ranks_Assassin_Create_Poison_Ability_Swift_Dex = HlEX.CreateContextActionApplyBuffRanks(cr_po_ab_dex_swf_appl_bf_action.Buff, cr_po_ab_dex_swf_cont_dur_comp, HlEX.CreateContextValue(AbilityRankType.ProjectilesCount), cr_po_ab_dex_swf_appl_bf_action.IsFromSpell, !cr_po_ab_dex_swf_appl_bf_action.IsNotDispelable, cr_po_ab_dex_swf_appl_bf_action.ToCaster, cr_po_ab_dex_swf_appl_bf_action.AsChild, cr_po_ab_dex_swf_appl_bf_action.Permanent, !cr_po_ab_dex_swf_appl_bf_action.NotLinkToAreaEffect);
            Assassin_Create_Poison_Ability_Swift_Dex.ReplaceComponents<AbilityEffectRunAction>(HlEX.CreateRunActions(Action_Apply_Buff_Ranks_Assassin_Create_Poison_Ability_Swift_Dex));

            // Replace ContextActionRemoveSelf with ContextActionRemoveBuff (with Remove Ranks)

            var Remove_Assassin_Create_Poison_Ability_Dex_Buff_Rank_Action = HlEX.CreateContextActionRemoveBuffRank(Assassin_Create_Poison_Ability_Dex_Buff);

            foreach (var cmp in cr_po_ab_bf_dx_comp)
            {
                var actions_to_alter = cmp.Action;
                var actions_to_remove = actions_to_alter.Actions.OfType<ContextActionRemoveSelf>().ToArray();
                foreach (var action in actions_to_remove)
                {
                    actions_to_alter.Actions = actions_to_alter.Actions.RemoveFromArray(action);
                }
                actions_to_alter.AddAction(Remove_Assassin_Create_Poison_Ability_Dex_Buff_Rank_Action);
            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Dexterity Ability Buff to remove ranks instead all the buff.", Assassin_Create_Poison_Ability_Dex_Buff);

            // Replace Apply Buff Action with Apply Buff Ranks Action [Poison Use — Constitution Ability]

            var cr_po_ab_con_appl_bf_action = Assassin_Create_Poison_Ability_Con.FlattenAllActions().OfType<ContextActionApplyBuff>().FirstOrDefault();
            var cr_po_ab_con_cont_dur_comp = cr_po_ab_con_appl_bf_action.DurationValue;
            var Assassin_Create_Poison_Ability_Con_Stickiness = Helpers.CreateContextRankConfig(ContextRankBaseValueType.CustomProperty, ContextRankProgression.AsIs, AbilityRankType.ProjectilesCount, 1, null, 0, 0, false, StatType.Unknown, CreatePoisonStickinessProperty);
            Assassin_Create_Poison_Ability_Con.AddComponent(Assassin_Create_Poison_Ability_Con_Stickiness);
            var Action_Apply_Buff_Ranks_Assassin_Create_Poison_Ability_Con = HlEX.CreateContextActionApplyBuffRanks(cr_po_ab_con_appl_bf_action.Buff, cr_po_ab_con_cont_dur_comp, HlEX.CreateContextValue(AbilityRankType.ProjectilesCount), cr_po_ab_con_appl_bf_action.IsFromSpell, !cr_po_ab_con_appl_bf_action.IsNotDispelable, cr_po_ab_con_appl_bf_action.ToCaster, cr_po_ab_con_appl_bf_action.AsChild, cr_po_ab_con_appl_bf_action.Permanent, !cr_po_ab_con_appl_bf_action.NotLinkToAreaEffect);
            Assassin_Create_Poison_Ability_Con.ReplaceComponents<AbilityEffectRunAction>(HlEX.CreateRunActions(Action_Apply_Buff_Ranks_Assassin_Create_Poison_Ability_Con));

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Constitution Ability to apply buff ranks instead of apply buff.", Assassin_Create_Poison_Ability_Con);

            // Replace Apply Buff Action with Apply Buff Ranks Action [Poison Use — Constitution Ability (Swift)]


            var cr_po_ab_con_swf_appl_bf_action = Assassin_Create_Poison_Ability_Swift_Con.FlattenAllActions().OfType<ContextActionApplyBuff>().FirstOrDefault();
            var cr_po_ab_con_swf_cont_dur_comp = cr_po_ab_con_swf_appl_bf_action.DurationValue;
            var Assassin_Create_Poison_Ability_Swift_Con_Stickiness = Helpers.CreateContextRankConfig(ContextRankBaseValueType.CustomProperty, ContextRankProgression.AsIs, AbilityRankType.ProjectilesCount, 1, null, 0, 0, false, StatType.Unknown, CreatePoisonStickinessProperty);
            Assassin_Create_Poison_Ability_Swift_Con.AddComponent(Assassin_Create_Poison_Ability_Swift_Con_Stickiness);
            var Action_Apply_Buff_Ranks_Assassin_Create_Poison_Ability_Swift_Con = HlEX.CreateContextActionApplyBuffRanks(cr_po_ab_con_swf_appl_bf_action.Buff, cr_po_ab_con_swf_cont_dur_comp, HlEX.CreateContextValue(AbilityRankType.ProjectilesCount), cr_po_ab_con_swf_appl_bf_action.IsFromSpell, !cr_po_ab_con_swf_appl_bf_action.IsNotDispelable, cr_po_ab_con_swf_appl_bf_action.ToCaster, cr_po_ab_con_swf_appl_bf_action.AsChild, cr_po_ab_con_swf_appl_bf_action.Permanent, !cr_po_ab_con_swf_appl_bf_action.NotLinkToAreaEffect);
            Assassin_Create_Poison_Ability_Swift_Con.ReplaceComponents<AbilityEffectRunAction>(HlEX.CreateRunActions(Action_Apply_Buff_Ranks_Assassin_Create_Poison_Ability_Swift_Con));

            // Replace ContextActionRemoveSelf with ContextActionRemoveBuff (with Remove Ranks)

            var Remove_Assassin_Create_Poison_Ability_Con_Buff_Rank_Action = HlEX.CreateContextActionRemoveBuffRank(Assassin_Create_Poison_Ability_Con_Buff);


            foreach (var cmp in cr_po_ab_bf_co_comp)
            {
                var actions_to_alter = cmp.Action;
                var actions_to_remove = actions_to_alter.Actions.OfType<ContextActionRemoveSelf>().ToArray();
                foreach (var action in actions_to_remove)
                {
                    actions_to_alter.Actions = actions_to_alter.Actions.RemoveFromArray(action);
                }
                actions_to_alter.AddAction(Remove_Assassin_Create_Poison_Ability_Con_Buff_Rank_Action);
            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Constitution Ability Buff to remove ranks instead all the buff.", Assassin_Create_Poison_Ability_Con_Buff);


            #endregion

            #region |-------------------------------------------|  Make Assassin's Poison Variable Concetration Based On Property |--------------------------------------------|

            // Used factors: m_Type = "Default" (Duration) / "DamageDice" (Recovery Saves)
            // Use "SpeedBonus" for Concentration.

            Assassin_Create_Poison_Ability_Str_Buff_Effect.Stacking = StackingType.Poison;
            Assassin_Create_Poison_Ability_Dex_Buff_Effect.Stacking = StackingType.Poison;
            Assassin_Create_Poison_Ability_Con_Buff_Effect.Stacking = StackingType.Poison;

            var Assassin_Create_Poison_Ability_Str_Buff_Concentration = Helpers.CreateContextRankConfig(ContextRankBaseValueType.CustomProperty, ContextRankProgression.AsIs, AbilityRankType.SpeedBonus, 1, null, 0, 0, false, StatType.Unknown, CreatePoisonStickinessProperty);
            Assassin_Create_Poison_Ability_Str_Buff.AddComponent(Assassin_Create_Poison_Ability_Str_Buff_Concentration);

            var Assassin_Create_Poison_Ability_Str_Buff_Apply_Buff_Action_ContextDuration = Assassin_Create_Poison_Ability_Str_Buff.FlattenAllActions().OfType<ContextActionApplyBuff>().FirstOrDefault().DurationValue;
            var Assassin_Create_Poison_Ability_Str_Buff_Actions_Saved = Assassin_Create_Poison_Ability_Str_Buff.FlattenAllActions().OfType<ContextActionConditionalSaved>().ToArray();
            var Assassin_Create_Poison_Ability_Str_Repeated_Buff_Application_Action = HlEX.CreateContextActionRepeatedApplyBuff(Assassin_Create_Poison_Ability_Str_Buff_Effect, Assassin_Create_Poison_Ability_Str_Buff_Apply_Buff_Action_ContextDuration, HlEX.CreateContextValue(AbilityRankType.SpeedBonus), false, false, false, false, false, true);


            foreach (var action in Assassin_Create_Poison_Ability_Str_Buff_Actions_Saved)
            {
                action.Failed = Helpers.CreateActionList(Assassin_Create_Poison_Ability_Str_Repeated_Buff_Application_Action);
            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Strength Ability Buff to apply effect buffs based on Concentration property.", Assassin_Create_Poison_Ability_Str_Buff);

            var Assassin_Create_Poison_Ability_Dex_Buff_Concentration = Helpers.CreateContextRankConfig(ContextRankBaseValueType.CustomProperty, ContextRankProgression.AsIs, AbilityRankType.SpeedBonus, 1, null, 0, 0, false, StatType.Unknown, CreatePoisonStickinessProperty);
            Assassin_Create_Poison_Ability_Dex_Buff.AddComponent(Assassin_Create_Poison_Ability_Dex_Buff_Concentration);

            var Assassin_Create_Poison_Ability_Dex_Buff_Apply_Buff_Action_ContextDuration = Assassin_Create_Poison_Ability_Dex_Buff.FlattenAllActions().OfType<ContextActionApplyBuff>().FirstOrDefault().DurationValue;
            var Assassin_Create_Poison_Ability_Dex_Buff_Actions_Saved = Assassin_Create_Poison_Ability_Dex_Buff.FlattenAllActions().OfType<ContextActionConditionalSaved>().ToArray();
            var Assassin_Create_Poison_Ability_Dex_Repeated_Buff_Application_Action = HlEX.CreateContextActionRepeatedApplyBuff(Assassin_Create_Poison_Ability_Dex_Buff_Effect, Assassin_Create_Poison_Ability_Dex_Buff_Apply_Buff_Action_ContextDuration, HlEX.CreateContextValue(AbilityRankType.SpeedBonus), false, false, false, false, false, true);


            foreach (var action in Assassin_Create_Poison_Ability_Dex_Buff_Actions_Saved)
            {
                action.Failed = Helpers.CreateActionList(Assassin_Create_Poison_Ability_Dex_Repeated_Buff_Application_Action);
            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Dexterity Ability Buff to apply effect buffs based on Concentration property.", Assassin_Create_Poison_Ability_Dex_Buff);

            var Assassin_Create_Poison_Ability_Con_Buff_Concentration = Helpers.CreateContextRankConfig(ContextRankBaseValueType.CustomProperty, ContextRankProgression.AsIs, AbilityRankType.SpeedBonus, 1, null, 0, 0, false, StatType.Unknown, CreatePoisonStickinessProperty);
            Assassin_Create_Poison_Ability_Con_Buff.AddComponent(Assassin_Create_Poison_Ability_Con_Buff_Concentration);


            var Assassin_Create_Poison_Ability_Con_Buff_Apply_Buff_Action_ContextDuration = Assassin_Create_Poison_Ability_Con_Buff.FlattenAllActions().OfType<ContextActionApplyBuff>().FirstOrDefault().DurationValue;
            var Assassin_Create_Poison_Ability_Con_Buff_Actions_Saved = Assassin_Create_Poison_Ability_Dex_Buff.FlattenAllActions().OfType<ContextActionConditionalSaved>().ToArray();
            var Assassin_Create_Poison_Ability_Con_Repeated_Buff_Application_Action = HlEX.CreateContextActionRepeatedApplyBuff(Assassin_Create_Poison_Ability_Con_Buff_Effect, Assassin_Create_Poison_Ability_Con_Buff_Apply_Buff_Action_ContextDuration, HlEX.CreateContextValue(AbilityRankType.SpeedBonus), false, false, false, false, false, true);


            foreach (var action in Assassin_Create_Poison_Ability_Con_Buff_Actions_Saved)
            {
                action.Failed = Helpers.CreateActionList(Assassin_Create_Poison_Ability_Con_Repeated_Buff_Application_Action);
            }


            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Constitution Ability Buff to apply effect buffs based on Concentration property.", Assassin_Create_Poison_Ability_Con_Buff);

            #endregion

            #region |------------------------------------------------------------| Add Poison Use Stat to Assassin |-----------------------------------------------------------|

            var Assassin_Progression = BlueprintTools.GetBlueprint<BlueprintProgression>("a02e1f0e13baa8c43b758425eda9e973");                                               //[CHECKED]

            var AssassinLevelEntry1 = Assassin_Progression.LevelEntries.Where(entry => entry.Level == 1).FirstOrDefault();

            AssassinLevelEntry1.m_Features.Add(Poisoncraft_Intelligence_Feature.ToReference<BlueprintFeatureBaseReference>());

            ToDContext.Logger.LogPatch("Added Poison Use - Intelligence to 1st level Assassin.", Assassin_Progression);

            #endregion


            #region |---------------------------------------------------|  Make Assassin's Poison Use Con-Damage Universal |---------------------------------------------------|

            var Assassin_Create_Poison_Con_Unlock_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("4b28d5c267df88743afb272a2a874220");                              //[CHECKED]

            var Universal_Create_Poison_Con_Unlock_Feature = Assassin_Create_Poison_Con_Unlock_Feature.CreateCopy(ToDContext, "UniversalCreatePoisonConUnlock", bp => {
                bp.SetDescription(ToDContext, "The character gains access to poison that deals {g|Encyclopedia:Constitution}Constitution{/g} {g|Encyclopedia:Damage}damage{/g}.");
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
            });

            Assassin_Create_Poison_Ability_Con.GetComponent<AbilityShowIfCasterHasFact>().TemporaryContext(c => {
                c.m_UnitFact = Universal_Create_Poison_Con_Unlock_Feature.ToReference<BlueprintUnitFactReference>();
            });

            Assassin_Create_Poison_Ability_Swift_Con.GetComponent<AbilityShowIfCasterHasFact>().TemporaryContext(c => {
                c.m_UnitFact = Universal_Create_Poison_Con_Unlock_Feature.ToReference<BlueprintUnitFactReference>();
            });

            Assassin_Create_Poison_Con_Unlock_Feature.AddComponent(HlEX.CreateAddFacts(new BlueprintUnitFactReference[] { Universal_Create_Poison_Con_Unlock_Feature.ToReference<BlueprintUnitFactReference>() }));

            ToDContext.Logger.LogPatch("Changed Assassin Poison Con Damage unlock to be universal.", Assassin_Create_Poison_Con_Unlock_Feature);

            #endregion

            #region |----------------------------------------------|  Make Assassin's Poison Use Buffs to include Toxicity Adj. |----------------------------------------------|

            var Assassin_Create_Poison_Ability_Str_Buff_Context_Action_Saving_Throws = Assassin_Create_Poison_Ability_Str_Buff.FlattenAllActions().OfType<ContextActionSavingThrow>().ToArray();
            var Assassin_Create_Poison_Ability_Dex_Buff_Context_Action_Saving_Throws = Assassin_Create_Poison_Ability_Dex_Buff.FlattenAllActions().OfType<ContextActionSavingThrow>().ToArray();
            var Assassin_Create_Poison_Ability_Con_Buff_Context_Action_Saving_Throws = Assassin_Create_Poison_Ability_Con_Buff.FlattenAllActions().OfType<ContextActionSavingThrow>().ToArray();

            var Assassin_Create_Poison_Ability_Buff_Toxicity_Conditional_DC_Increase = new ContextActionSavingThrow.ConditionalDCIncrease();

            Assassin_Create_Poison_Ability_Buff_Toxicity_Conditional_DC_Increase.Condition = HlEX.CreateConditionsCheckerAnd(new Condition[] { HlEX.CreateConditionTrue() });
            Assassin_Create_Poison_Ability_Buff_Toxicity_Conditional_DC_Increase.Value = new ContextValue()
            {
                ValueType = ContextValueType.CasterCustomProperty,
                m_CustomProperty = Create_Poison_Toxicity_Adjustment_Property.ToReference<BlueprintUnitPropertyReference>()
            };


            foreach (var cont_act_saving_throw in Assassin_Create_Poison_Ability_Str_Buff_Context_Action_Saving_Throws)
            {
                cont_act_saving_throw.TemporaryContext(c => {
                    c.m_ConditionalDCIncrease.AppendToArray(Assassin_Create_Poison_Ability_Buff_Toxicity_Conditional_DC_Increase); 
                });
            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Strength Ability Buff to adjust the buff's DC based on the Toxicity property.", Assassin_Create_Poison_Ability_Str_Buff);

            foreach (var cont_act_saving_throw in Assassin_Create_Poison_Ability_Dex_Buff_Context_Action_Saving_Throws)
            {
                cont_act_saving_throw.TemporaryContext(c => {
                    c.m_ConditionalDCIncrease.AppendToArray(Assassin_Create_Poison_Ability_Buff_Toxicity_Conditional_DC_Increase);
                });
            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Dexterity Ability Buff to adjust the buff's DC based on the Toxicity property.", Assassin_Create_Poison_Ability_Dex_Buff);

            foreach (var cont_act_saving_throw in Assassin_Create_Poison_Ability_Con_Buff_Context_Action_Saving_Throws)
            {
                cont_act_saving_throw.TemporaryContext(c => {
                    c.m_ConditionalDCIncrease.AppendToArray(Assassin_Create_Poison_Ability_Buff_Toxicity_Conditional_DC_Increase);
                });
            }

            ToDContext.Logger.LogPatch("Altered Assassin Poison Use — Constitution Ability Buff to adjust the buff's DC based on the Toxicity property.", Assassin_Create_Poison_Ability_Con_Buff);

            #endregion


            #region |-------------------------------------------------------| Change Assassin Poison Use Descriptions |--------------------------------------------------------|

            string Assassin_Create_Poison_Ability_New_Description = "Assassins specialize in poison use. Every day they create special combat poisons, the number of their doses equal to 3 + assassin level, which they can use to envenom their weapons (both melee and ranged). As a {g|Encyclopedia:Move_Action}move action{/g}, an assassin can apply one of the poisons to their weapons (both melee and ranged), and the next successful {g|Encyclopedia:Attack}attack{/g} applies it to the target. {g|Encyclopedia:Saving_Throw}Save{/g} {g|Encyclopedia:DC}DC{/g} of all poisons is 10 + assassin level + relevant stat modifier. If the attack that applied the poison was a sneak attack, the DC is increased by 2.\nAt 1st level, an assassin has access to poisons that deal {g|Encyclopedia:Dice}1d4{/g} stat {g|Encyclopedia:Damage}damage{/g} to {g|Encyclopedia:Strength}Strength{/g} or {g|Encyclopedia:Dexterity}Dexterity{/g} for 4 {g|Encyclopedia:Combat_Round}rounds{/g}.";
            string Assassin_Create_Poison_Swift_Ability_New_Description = "Assassins specialize in poison use. Every day they create special combat poisons, the number of their doses equal to 3 + assassin level, which they can use to envenom their weapons (both melee and ranged). As a {g|Encyclopedia:Swift_Action}swift action{/g}, an assassin can apply one of the poisons to their weapons (both melee and ranged), and the next successful {g|Encyclopedia:Attack}attack{/g} applies it to the target. {g|Encyclopedia:Saving_Throw}Save{/g} {g|Encyclopedia:DC}DC{/g} of all poisons is 10 + assassin level + relevant stat modifier. If the attack that applied the poison was a sneak attack, the DC is increased by 2.\nThe assassin has access to all special combat poisons currently known to him and to any feature that would affect the base Poison Use ability (with {g|Encyclopedia:Move_Action}move action{/g} activation).";


            Assassin_Create_Poison_Ability.SetDescription(ToDContext, Assassin_Create_Poison_Ability_New_Description);
            Assassin_Create_Poison_Ability_Str.SetDescription(ToDContext, Assassin_Create_Poison_Ability_New_Description + "\n When produced with its basic formula, this specific combat poison deals {g|Encyclopedia:Dice}1d4{/g} stat {g|Encyclopedia:Damage}damage{/g} to {g|Encyclopedia:Strength}Strength{/g} for 4 {g|Encyclopedia:Combat_Round}rounds{/g}.");
            Assassin_Create_Poison_Ability_Dex.SetDescription(ToDContext, Assassin_Create_Poison_Ability_New_Description  + "\n When produced with its basic formula, this specific combat poison deals {g|Encyclopedia:Dice}1d4{/g} stat {g|Encyclopedia:Damage}damage{/g} to {g|Encyclopedia:Dexterity}Dexterity{/g} for 4 {g|Encyclopedia:Combat_Round}rounds{/g}.");
            Assassin_Create_Poison_Ability_Con.SetDescription(ToDContext, Assassin_Create_Poison_Ability_New_Description + "\n When produced with its basic formula, this specific combat poison deals {g|Encyclopedia:Dice}1d4{/g} stat {g|Encyclopedia:Damage}damage{/g} to {g|Encyclopedia:Constitution}Constitution{/g} for 4 {g|Encyclopedia:Combat_Round}rounds{/g}.");

            Assassin_Create_Poison_Swift_Ability.SetDescription(ToDContext, Assassin_Create_Poison_Swift_Ability_New_Description);
            Assassin_Create_Poison_Ability_Swift_Str.SetDescription(ToDContext, Assassin_Create_Poison_Swift_Ability_New_Description + "\n When produced with its basic formula, this specific combat poison deals {g|Encyclopedia:Dice}1d4{/g} stat {g|Encyclopedia:Damage}damage{/g} to {g|Encyclopedia:Strength}Strength{/g} for 4 {g|Encyclopedia:Combat_Round}rounds{/g}.");
            Assassin_Create_Poison_Ability_Swift_Dex.SetDescription(ToDContext, Assassin_Create_Poison_Swift_Ability_New_Description + "\n When produced with its basic formula, this specific combat poison deals {g|Encyclopedia:Dice}1d4{/g} stat {g|Encyclopedia:Damage}damage{/g} to {g|Encyclopedia:Dexterity}Dexterity{/g} for 4 {g|Encyclopedia:Combat_Round}rounds{/g}.");
            Assassin_Create_Poison_Ability_Swift_Con.SetDescription(ToDContext, Assassin_Create_Poison_Swift_Ability_New_Description + "\n When produced with its basic formula, this specific combat poison deals {g|Encyclopedia:Dice}1d4{/g} stat {g|Encyclopedia:Damage}damage{/g} to {g|Encyclopedia:Constitution}Constitution{/g} for 4 {g|Encyclopedia:Combat_Round}rounds{/g}.");


            UnityModManagerNet.UnityModManager.Logger.Log("Changed description of the Poison Use feature.");

            #endregion

        }


    }
}
