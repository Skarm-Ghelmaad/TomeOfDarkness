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
using static TomeOfDarkness.NewEnums.AbilitySharedValueToD;
using static TomeOfDarkness.NewEnums.AbilityRankTypeToD;
using Kingmaker.Designers.Mechanics.Buffs;
using System.Security.Cryptography;

namespace TomeOfDarkness.MechanicsChanges
{
    internal class UniversalPoisonUse
    {

        public static void ConfigureUniversalPoisonUse()
        {

            PoisonCraftTraining.ConfigurePoisonCraftTraining();

            #region |-----------------------------------------------------------| initialize Existing Variables |--------------------------------------------------------------|

            //Vanilla Blueprints
            var Assassin_Create_Poison_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("8dd826513ba857645b38e918f17b59e6");                                  
            
            var Assassin_Create_Poison_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("46660d0da7797124aa221818778edc9d");                                 //Base Ability                                
            var Assassin_Create_Poison_Ability_Str = BlueprintTools.GetBlueprint<BlueprintAbility>("67aa1843adeed0346a30125c29d8df8b");                             //Poison-Applying Abilities                               
            var Assassin_Create_Poison_Ability_Dex = BlueprintTools.GetBlueprint<BlueprintAbility>("3fbbc2843598d8146b1ca3415df6ecdd");                              
            var Assassin_Create_Poison_Ability_Con = BlueprintTools.GetBlueprint<BlueprintAbility>("5a72db7750919864ba9cc2afa335fd2f");


            var Assassin_Create_Poison_Swift_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("bb7b571cadb6cc147a52431385a40a0d");  

            var Assassin_Create_Poison_Swift_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("eadfd2e82d4e3684893543668aa55312");                            //Base Swift Ability   
            var Assassin_Create_Poison_Ability_Swift_Str = BlueprintTools.GetBlueprint<BlueprintAbility>("ca95feaf38593534f91becc31d097756");                        //Swift Poison-Applying Abilities   
            var Assassin_Create_Poison_Ability_Swift_Dex = BlueprintTools.GetBlueprint<BlueprintAbility>("e7730af415f91c44e9306e1aa15c5a85");                        
            var Assassin_Create_Poison_Ability_Swift_Con = BlueprintTools.GetBlueprint<BlueprintAbility>("eff246d4ceae9b14e8bc4a99fe508808");

            var Assassin_Create_Poison_Ability_Str_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("c219da8e56fb30045bb69247c695b8c8");                            //Poison-Storing Buffs
            var Assassin_Create_Poison_Ability_Dex_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("385c07aa91442094f9385510504dde3c");                            
            var Assassin_Create_Poison_Ability_Con_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("ac4d4b3f14f2b6e41a19a3d8e13e7ee7");

            var Assassin_Create_Poison_Ability_Str_Buff_Effect = BlueprintTools.GetBlueprint<BlueprintBuff>("285290cc80941bc4c97461d6f50aaad9");                     //Poison-Effect Buffs
            var Assassin_Create_Poison_Ability_Dex_Buff_Effect = BlueprintTools.GetBlueprint<BlueprintBuff>("c766f0606ac12e24e8a9fdb8beabc6c2");                     
            var Assassin_Create_Poison_Ability_Con_Buff_Effect = BlueprintTools.GetBlueprint<BlueprintBuff>("6542e025d84501a41b652bcdc57f6901");                     

            var Assassin_Create_Poison_Ability_Property = BlueprintTools.GetBlueprint<BlueprintUnitProperty>("0482fffc039d46fc86a86bda03e00f1a");

            var Assassin_Create_Poison_Ability_Ability_Resource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("d54b614eb42da7d48b927b57de337b95");

            var Assassin_Create_Poison_Increased_Duration = BlueprintTools.GetBlueprint<BlueprintFeature>("953e47bbeb7c55145884de118f812b28");                       //Poison duration-enhancing feature                       
            var Assassin_Create_Poison_Increased_Saving_Throws = BlueprintTools.GetBlueprint<BlueprintFeature>("0cbb2201a65cb374fb4d73d6c9830b01");                  //Poison save-increasing feature 
            var Assassin_Create_Poison_Con_Unlock_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("4b28d5c267df88743afb272a2a874220");                       //Poison Con-unlock feature

            var Assassin_Proficiencies = BlueprintTools.GetBlueprint<BlueprintFeature>("4f5ba283da1232a458234d228fab5d48");

            //New Blueprints
            var Poison_Craft_Training_FakeLevel = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "PoisonCraftTrainingFakeLevel");
            var Poison_Craft_Training_Property = BlueprintTools.GetModBlueprint<BlueprintUnitProperty>(ToDContext, "PoisonCraftTrainingProperty");


            //Custom Icons
            var PoisonCraftStrengthIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_PoisonUseRed.png");
            var PoisonCraftDexterityIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_PoisonUseYellow.png");
            var PoisonCraftConstitutionIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_PoisonUseGreen.png");
            var PoisonCraftIntelligenceIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_PoisonUseBlue.png");
            var PoisonCraftWisdomIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_PoisonUseWhite.png");
            var PoisonCraftCharismaIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_PoisonUsePurple.png");

            #endregion



            #region |----------------------------------------------------------| Create Generic PoisonCraft Stat |-------------------------------------------------------------|



            var Poisoncraft_Strength_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "PoisonCraftStrengthStatFeature", bp => {
                bp.SetName(ToDContext, "Poison Use - Strength");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Strength}Str{/g} modifier to his Poison Use's DC.");
                bp.m_Icon = PoisonCraftStrengthIcon;
                bp.Ranks = 1;
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Created Poison Use - Strength feature.", Poisoncraft_Strength_Feature);

            var Poisoncraft_Dexterity_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "PoisonCraftDexterityStatFeature", bp => {
                bp.SetName(ToDContext, "Poison Use - Dexterity");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Dexterity}Dex{/g} modifier to his Poison Use's DC.");
                bp.m_Icon = PoisonCraftDexterityIcon;
                bp.Ranks = 1;
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Created Poison Use - Dexterity feature.", Poisoncraft_Dexterity_Feature);

            var Poisoncraft_Constitution_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "PoisonCraftConstitutionStatFeature", bp => {
                bp.SetName(ToDContext, "Poison Use - Constitution");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Constitution}Con{/g} modifier to his Poison Use's DC.");
                bp.m_Icon = PoisonCraftConstitutionIcon;
                bp.Ranks = 1;
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Created Poison Use - Constitution feature.", Poisoncraft_Constitution_Feature);

            var Poisoncraft_Intelligence_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "PoisonCraftIntelligenceStatFeature", bp => {
                bp.SetName(ToDContext, "Poison Use - Intelligence");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Intelligence}Int{/g} modifier to his Poison Use's DC.");
                bp.m_Icon = PoisonCraftIntelligenceIcon;
                bp.Ranks = 1;
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Created Poison Use - Intelligence feature.", Poisoncraft_Intelligence_Feature);

            var Poisoncraft_Wisdom_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "PoisonCraftWisdomStatFeature", bp => {
                bp.SetName(ToDContext, "Poison Use - Wisdom");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Wisdom}Wis{/g} modifier to his Poison Use's DC.");
                bp.m_Icon = PoisonCraftWisdomIcon;
                bp.Ranks = 1;
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Created Poison Use - Wisdom feature.", Poisoncraft_Wisdom_Feature);

            var Poisoncraft_Charisma_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "PoisonCraftCharismaStatFeature", bp => {
                bp.SetName(ToDContext, "Poison Use - Charisma");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Charisma}Cha{/g} modifier to his Poison Use's DC.");
                bp.m_Icon = PoisonCraftCharismaIcon;
                bp.Ranks = 1;
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
                bp.IsClassFeature = true;
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

            var Universal_Create_Poison_Toxicity_Boost_Class_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonToxicityBoostClassFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Toxicity Boost (Class)");
                bp.SetDescription(ToDContext, "Each rank of this feature add +1 to the Poison Use's DC.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Toxicity Boost (Class) feature.", Universal_Create_Poison_Toxicity_Boost_Class_Feature);

            var Universal_Create_Poison_Toxicity_Boost_Class_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonToxicityBoostClassBluff", bp => {
                bp.SetName(ToDContext, "Create Poison - Toxicity Boost (Class)");
                bp.SetDescription(ToDContext, "Each rank of this buff add +1 to the Poison Use's DC.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Toxicity Boost (Class) buff.", Universal_Create_Poison_Toxicity_Boost_Class_Buff);

            var Universal_Create_Poison_Toxicity_Reduction_Class_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonToxicityReductionClassFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Toxicity Reduction (Class)");
                bp.SetDescription(ToDContext, "Each rank of this feature adds -1 to the Poison Use's DC.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Toxicity Reduction (Class) feature.", Universal_Create_Poison_Toxicity_Reduction_Class_Feature);

            var Universal_Create_Poison_Toxicity_Reduction_Class_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonToxicityReductionClassBluff", bp => {
                bp.SetName(ToDContext, "Create Poison - Toxicity Reduction (Class)");
                bp.SetDescription(ToDContext, "Each rank of this buff adds -1 to the Poison Use's DC.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Toxicity Reduction (Class) buff.", Universal_Create_Poison_Toxicity_Reduction_Class_Buff);

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
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Toxicity_Boost_Class_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Toxicity_Boost_Buff.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Toxicity_Boost_Class_Buff.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Toxicity_Reduction_Feature.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Toxicity_Reduction_Class_Feature.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Toxicity_Reduction_Buff.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Toxicity_Reduction_Class_Buff.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            }
                        };
                });
                bp.BaseValue = 0;
            });

            ToDContext.Logger.LogPatch("Added Create Poison Toxicity Adjustment property.", Create_Poison_Toxicity_Adjustment_Property);

            #endregion

            #region |-------------------------------------------------| Create Poison Sneak Attack Toxicity Boost Property |---------------------------------------------------|

            // Note that Sneak Attack adds a base +2 bonus to DC, but I need to have this property to further adjust for specific features and enhancements.

            var Universal_Create_Poison_Sneak_Attack_Toxicity_Boost_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreateSneakAttackPoisonToxicityBoostFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Sneak Attack Toxicity Boost");
                bp.SetDescription(ToDContext, "Each rank of this feature add +1 to the Poison Use's DC when used with sneak attacks.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Sneak Attack Toxicity Boost feature.", Universal_Create_Poison_Sneak_Attack_Toxicity_Boost_Feature);

            var Universal_Create_Poison_Sneak_Attack_Toxicity_Boost_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreateSneakAttackPoisonToxicityBoostBluff", bp => {
                bp.SetName(ToDContext, "Create Poison - Sneak Attack Toxicity Boost");
                bp.SetDescription(ToDContext, "Each rank of this buff add +1 to the Poison Use's DC when used with sneak attacks.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Sneak Attack Toxicity Boost buff.", Universal_Create_Poison_Sneak_Attack_Toxicity_Boost_Buff);

            var Universal_Create_Poison_Sneak_Attack_Toxicity_Boost_Class_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonSneakAttackToxicityBoostClassFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Sneak Attack Toxicity Boost (Class)");
                bp.SetDescription(ToDContext, "Each rank of this feature add +1 to the Poison Use's DC when used with sneak attacks.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Sneak Attack Toxicity Boost (Class) feature.", Universal_Create_Poison_Sneak_Attack_Toxicity_Boost_Class_Feature);

            var Universal_Create_Poison_Sneak_Attack_Toxicity_Boost_Class_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonSneakAttackToxicityBoostClassBluff", bp => {
                bp.SetName(ToDContext, "Create Poison - Sneak Attack Toxicity Boost (Class)");
                bp.SetDescription(ToDContext, "Each rank of this buff add +1 to the Poison Use's DC when used with sneak attacks.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Sneak Attack Toxicity Boost (Class) buff.", Universal_Create_Poison_Sneak_Attack_Toxicity_Boost_Class_Buff);

            var Create_Poison_Sneak_Attack_Toxicity_Boost_Property = Helpers.CreateBlueprint<BlueprintUnitProperty>(ToDContext, "CreatePoisonSneakAttackToxicityBoostProperty", bp =>
            {
                bp.AddComponent<CompositeCustomPropertyGetter>(c =>
                {
                    c.CalculationMode = CompositeCustomPropertyGetter.Mode.Sum;
                    c.Properties = new CompositeCustomPropertyGetter.ComplexCustomProperty[] {
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Sneak_Attack_Toxicity_Boost_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Sneak_Attack_Toxicity_Boost_Class_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Sneak_Attack_Toxicity_Boost_Buff.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Sneak_Attack_Toxicity_Boost_Class_Buff.ToReference<BlueprintUnitFactReference>())
                            }

                        };
                });
                bp.BaseValue = 2;
            });

            ToDContext.Logger.LogPatch("Added Create Poison Sneak Attack Toxicity Adjustment property.", Create_Poison_Sneak_Attack_Toxicity_Boost_Property);

            #endregion

            #region |-------------------------------------------------| Create Poison Critical Hit Toxicity Boost Property |---------------------------------------------------|

            // Note that Critical Hits adds a base +0 bonus to DC, but I need to have this property to further adjust for specific features and enhancements.

            var Universal_Create_Poison_Critical_Hit_Toxicity_Boost_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonCriticalHitToxicityBoostFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Critical Hit Toxicity Boost");
                bp.SetDescription(ToDContext, "Each rank of this feature add +1 to the Poison Use's DC when used with critical hits.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Critical Hit Toxicity Boost feature.", Universal_Create_Poison_Critical_Hit_Toxicity_Boost_Feature);

            var Universal_Create_Poison_Critical_Hit_Toxicity_Boost_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonCriticalHitToxicityBoostBluff", bp => {
                bp.SetName(ToDContext, "Create Poison - Critical Hit Toxicity Boost");
                bp.SetDescription(ToDContext, "Each rank of this buff add +1 to the Poison Use's DC when used with critical hits.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Critical Hit Toxicity Boost buff.", Universal_Create_Poison_Critical_Hit_Toxicity_Boost_Buff);

            var Universal_Create_Poison_Critical_Hit_Toxicity_Boost_Class_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonCriticalHitToxicityBoostClassFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Critical Hit Toxicity Boost (Class)");
                bp.SetDescription(ToDContext, "Each rank of this feature add +1 to the Poison Use's DC when used with critical hits.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Critical Hit Toxicity Boost (Class) feature.", Universal_Create_Poison_Critical_Hit_Toxicity_Boost_Class_Feature);

            var Universal_Create_Poison_Critical_Hit_Toxicity_Boost_Class_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonCriticalHitToxicityBoostClassBluff", bp => {
                bp.SetName(ToDContext, "Create Poison - Critical Hit Toxicity Boost (Class)");
                bp.SetDescription(ToDContext, "Each rank of this buff add +1 to the Poison Use's DC when used with critical hits.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Critical Hit Toxicity Boost (Class) buff.", Universal_Create_Poison_Critical_Hit_Toxicity_Boost_Class_Buff);

            var Create_Poison_Critical_Hit_Toxicity_Boost_Property = Helpers.CreateBlueprint<BlueprintUnitProperty>(ToDContext, "CreatePoisonCriticalHitToxicityBoostProperty", bp =>
            {
                bp.AddComponent<CompositeCustomPropertyGetter>(c =>
                {
                    c.CalculationMode = CompositeCustomPropertyGetter.Mode.Sum;
                    c.Properties = new CompositeCustomPropertyGetter.ComplexCustomProperty[] {
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Critical_Hit_Toxicity_Boost_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Critical_Hit_Toxicity_Boost_Class_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Critical_Hit_Toxicity_Boost_Buff.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Critical_Hit_Toxicity_Boost_Class_Buff.ToReference<BlueprintUnitFactReference>())
                            }

                        };
                });
                bp.BaseValue = 0;
            });

            ToDContext.Logger.LogPatch("Added Create Poison Critical Hit Toxicity Adjustment property.", Create_Poison_Critical_Hit_Toxicity_Boost_Property);

            #endregion

            #region |-----------------------------------------------------| Create Poison Frequency Adjustment Property |------------------------------------------------------|

            // This property is used to raise or lower the frequency of Poison Use based on features or buffs possessed.

            var Universal_Create_Poison_Frequency_Boost_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonFrequencyBoostFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Frequency Boost");
                bp.SetDescription(ToDContext, "Each rank of this feature add +1 to the Poison Use's frequency (duration).");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Frequency Boost feature.", Universal_Create_Poison_Frequency_Boost_Feature);

            var Universal_Create_Poison_Frequency_Boost_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonFrequencyyBoostBluff", bp => {
                bp.SetName(ToDContext, "Create Poison - Frequency Boost");
                bp.SetDescription(ToDContext, "Each rank of this feature add +1 to the Poison Use's frequency (duration).");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Frequency Boost buff.", Universal_Create_Poison_Frequency_Boost_Buff);

            var Universal_Create_Poison_Frequency_Reduction_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonFrequencyyReductionFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Frequency Reduction");
                bp.SetDescription(ToDContext, "Each rank of this feature add -1 to the Poison Use's frequency (duration).");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Frequency Reduction feature.", Universal_Create_Poison_Frequency_Reduction_Feature);

            var Universal_Create_Poison_Frequency_Reduction_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonFrequencyyReductionBluff", bp => {
                bp.SetName(ToDContext, "Create Poison - Frequency Reduction");
                bp.SetDescription(ToDContext, "Each rank of this feature add -1 to the Poison Use's frequency (duration).");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Frequency Reduction buff.", Universal_Create_Poison_Frequency_Reduction_Buff);


            var Universal_Create_Poison_Frequency_Boost_Class_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonFrequencyBoostClassFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Frequency Boost (Class)");
                bp.SetDescription(ToDContext, "Each rank of this feature add +1 to the Poison Use's frequency (duration).");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Frequency Boost (Class) feature.", Universal_Create_Poison_Frequency_Boost_Class_Feature);

            var Universal_Create_Poison_Frequency_Boost_Class_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonFrequencyyBoostClassBluff", bp => {
                bp.SetName(ToDContext, "Create Poison - Frequency Boost (Class)");
                bp.SetDescription(ToDContext, "Each rank of this feature add +1 to the Poison Use's frequency (duration).");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Frequency Boost (Class) buff.", Universal_Create_Poison_Frequency_Boost_Class_Buff);

            var Universal_Create_Poison_Frequency_Reduction_Class_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonFrequencyyReductionClassFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Frequency Reduction (Class)");
                bp.SetDescription(ToDContext, "Each rank of this feature add -1 to the Poison Use's frequency (duration).");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Frequency Reduction (Class) feature.", Universal_Create_Poison_Frequency_Reduction_Class_Feature);

            var Universal_Create_Poison_Frequency_Reduction_Class_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonFrequencyyReductionClassBluff", bp => {
                bp.SetName(ToDContext, "Create Poison - Frequency Reduction (Class)");
                bp.SetDescription(ToDContext, "Each rank of this feature add -1 to the Poison Use's frequency (duration).");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Frequency Reduction (Class) buff.", Universal_Create_Poison_Frequency_Reduction_Class_Buff);

            var Create_Poison_Frequency_Adjustment_Property = Helpers.CreateBlueprint<BlueprintUnitProperty>(ToDContext, "CreatePoisonFrequencyAdjustmentProperty", bp =>
            {
                bp.AddComponent<CompositeCustomPropertyGetter>(c =>
                {
                    c.CalculationMode = CompositeCustomPropertyGetter.Mode.Sum;
                    c.Properties = new CompositeCustomPropertyGetter.ComplexCustomProperty[] {
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Frequency_Boost_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Frequency_Boost_Class_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Frequency_Boost_Buff.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Frequency_Boost_Class_Buff.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Frequency_Reduction_Feature.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Frequency_Reduction_Class_Feature.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Frequency_Reduction_Buff.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Frequency_Reduction_Class_Buff.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            }
                        };
                });
                bp.BaseValue = 0;
            });

            ToDContext.Logger.LogPatch("Added Create Poison Frequency Adjustment property.", Create_Poison_Frequency_Adjustment_Property);

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


            var Universal_Create_Poison_Recovery_Worsen_Class_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonRecoveryWorsenClassFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Worsen Recovery (Class)");
                bp.SetDescription(ToDContext, "Each rank of this feature adds +1 save to recover from Poison Use.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Worsen Recovery feature.", Universal_Create_Poison_Recovery_Worsen_Class_Feature);

            var Universal_Create_Poison_Recovery_Worsen_Class_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonRecoveryWorsenClassBluff", bp => {
                bp.SetName(ToDContext, "Create Poison - Worsen Recovery (Class)");
                bp.SetDescription(ToDContext, "Each rank of this buff adds +1 save to recover from Poison Use.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Worsen Recovery buff.", Universal_Create_Poison_Recovery_Worsen_Class_Buff);

            var Universal_Create_Poison_Recovery_Improve_Class_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonRecoveryImproveClassFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Improve Recovery (Class)");
                bp.SetDescription(ToDContext, "Each rank of this feature adds -1 save to recover from Poison Use.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Worsen Recovery buff.", Universal_Create_Poison_Recovery_Worsen_Class_Buff);

            var Universal_Create_Poison_Recovery_Improve_Class_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonRecoveryImproveClassBuff", bp => {
                bp.SetName(ToDContext, "Create Poison - Improve Recovery (Class)");
                bp.SetDescription(ToDContext, "Each rank of this buff adds -1 save to recover from Poison Use.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Improve Recovery buff.", Universal_Create_Poison_Recovery_Improve_Class_Buff);

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
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Recovery_Worsen_Class_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Recovery_Worsen_Buff.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Recovery_Worsen_Class_Buff.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Recovery_Improve_Feature.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Recovery_Improve_Class_Feature.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Recovery_Improve_Buff.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Recovery_Improve_Class_Buff.ToReference<BlueprintUnitFactReference>()),
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

            var Universal_Create_Poison_Stickiness_Class_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonStickinessClassFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Stickiness (Class)");
                bp.SetDescription(ToDContext, "Each rank of this feature adds +1 attack infused with Poison Use.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Stickiness (Class) feature.", Universal_Create_Poison_Stickiness_Class_Feature);

            var Universal_Create_Poison_Stickiness_Class_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonStickinessClassBuff", bp => {
                bp.SetName(ToDContext, "Create Poison - Stickiness (Class)");
                bp.SetDescription(ToDContext, "Each rank of this buff adds +1 attack infused with Poison Use.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Stickiness (Class) buff.", Universal_Create_Poison_Stickiness_Class_Buff);

            var Universal_Create_Poison_Stickiness_Boost_Strength_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonStickinessBoostStrengthFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Stickiness Boost (Strength)");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Strength}Str{/g} modifier to attacks infused with Poison Use.");
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Stickiness Boost (Strength) feature.", Universal_Create_Poison_Stickiness_Boost_Strength_Feature);

            var Universal_Create_Poison_Stickiness_Boost_Dexterity_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonStickinessBoostDexterityFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Stickiness Boost (Dexterity)");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Dexterity}Dex{/g} modifier to attacks infused with Poison Use.");
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Stickiness Boost (Dexterity) feature.", Universal_Create_Poison_Stickiness_Boost_Dexterity_Feature);

            var Universal_Create_Poison_Stickiness_Boost_Constitution_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonStickinessBoostConstitutionFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Stickiness Boost (Constitution)");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Constitution}Con{/g} modifier to attacks infused with Poison Use.");
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
            });

            var Universal_Create_Poison_Stickiness_Boost_Intelligence_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonStickinessBoostIntelligenceFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Stickiness Boost (Intelligence)");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Intelligence}Int{/g} modifier to attacks infused with Poison Use.");
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Stickiness Boost (Intelligence) feature.", Universal_Create_Poison_Stickiness_Boost_Intelligence_Feature);

            var Universal_Create_Poison_Stickiness_Boost_Wisdom_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonStickinessBoostWisdomFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Stickiness Boost (Wisdom)");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Wisdom}Wis{/g} modifier to attacks infused with Poison Use.");
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Stickiness Boost (Wisdom) feature.", Universal_Create_Poison_Stickiness_Boost_Wisdom_Feature);


            var Universal_Create_Poison_Stickiness_Boost_Charisma_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonStickinessBoostCharismaFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Stickiness Boost (Charisma)");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Charisma}Cha{/g} modifier to attacks infused with Poison Use.");
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
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
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Stickiness_Class_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Stickiness_Class_Buff.ToReference<BlueprintUnitFactReference>())
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


            var Universal_Create_Poison_Concentration_Class_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalCreatePoisonConcentrationClassFeature", bp => {
                bp.SetName(ToDContext, "Create Poison - Concentration (Class)");
                bp.SetDescription(ToDContext, "Each rank of this feature adds +1 dose infused with Poison Use.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Concentration feature.", Universal_Create_Poison_Concentration_Class_Feature);

            var Universal_Create_Poison_Concentration_Class_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalCreatePoisonConcentrationClassBuff", bp => {
                bp.SetName(ToDContext, "Create Poison - Concentration (Class)");
                bp.SetDescription(ToDContext, "Each rank of this buff adds +1 dose infused with Poison Use.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Concentration feature.", Universal_Create_Poison_Concentration_Class_Buff);

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
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Concentration_Class_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Create_Poison_Concentration_Class_Buff.ToReference<BlueprintUnitFactReference>())
                            }
                        };
                    bp.BaseValue = 1;
                });
            });

            ToDContext.Logger.LogPatch("Added Create Poison - Concentration property.", CreatePoisonConcentrationProperty);

            #endregion

            #region |----------------------------------------------| Changed Assassin's Poison Use Features to Become Universal |----------------------------------------------|


            Assassin_Create_Poison_Increased_Duration.TemporaryContext(bp => {
                bp.AddComponent(HlEX.CreateAddFacts(new BlueprintUnitFactReference[] { Universal_Create_Poison_Frequency_Boost_Class_Feature.ToReference<BlueprintUnitFactReference>() }));
            });

            ToDContext.Logger.LogPatch("Changed Assassin Create Poison Increased Duration to add a generic duration boost.", Assassin_Create_Poison_Increased_Duration);

            Assassin_Create_Poison_Increased_Saving_Throws.TemporaryContext(bp => {
                 bp.AddComponent(HlEX.CreateAddFacts(new BlueprintUnitFactReference[] { Universal_Create_Poison_Recovery_Worsen_Class_Feature.ToReference<BlueprintUnitFactReference>() }));
            });

            ToDContext.Logger.LogPatch("Changed Assassin Create Poison Increased Saving Throws to add a generic increase of saving throws.", Assassin_Create_Poison_Increased_Saving_Throws);

            var Universal_Create_Poison_Con_Unlock_Feature = Assassin_Create_Poison_Con_Unlock_Feature.CreateCopy(ToDContext, "UniversalCreatePoisonConUnlock", bp => {
                bp.SetDescription(ToDContext, "The character gains access to poison that deals {g|Encyclopedia:Constitution}Constitution{/g} {g|Encyclopedia:Damage}damage{/g}.");
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.Ranks = 1;
            });

            Assassin_Create_Poison_Con_Unlock_Feature.TemporaryContext(bp => {
                bp.AddComponent(HlEX.CreateAddFacts(new BlueprintUnitFactReference[] { Universal_Create_Poison_Con_Unlock_Feature.ToReference<BlueprintUnitFactReference>() }));
            });
            
            ToDContext.Logger.LogPatch("Changed Assassin Poison Con Damage unlock to be universal.", Assassin_Create_Poison_Con_Unlock_Feature);

            #endregion

            #region |------------------------------------------| Alter Assassin's Poison Use Ability Property to Be Universal |-----------------------------------------|

            // This changes Poison Use to take in account fake levels, conditional stats and DC (Toxicity) adjustments.
            
            Assassin_Create_Poison_Ability_Property.TemporaryContext(bp => {
                bp.RemoveComponents<ClassLevelGetter>();
                bp.RemoveComponents<SimplePropertyGetter>();
                bp.AddComponent(HlEX.CreateCustomPropertyGetter(Poison_Craft_Training_Property));
                bp.AddComponent(HlEX.CreateCustomPropertyGetter(PoisonCraftStatProperty));
                bp.AddComponent(HlEX.CreateCustomPropertyGetter(Create_Poison_Toxicity_Adjustment_Property));
            });
                       

            ToDContext.Logger.LogPatch("Altered Assassin Create Poison Ability Property to use Custom Property Getters instead of the original Class Level Getter and Simple Property Getter", Assassin_Create_Poison_Ability_Property);

            #endregion

            #region |----------------------------------------------| Create New Assassin's Poison Use Ability Parameters |----------------------------------------------|

            var Assassin_Create_Poison_Ability_Any_Context_Value_DC = HlEX.CreateContextValueFromCustomCasterProperty(Assassin_Create_Poison_Ability_Property);
            var Assassin_Create_Poison_Ability_Any_Context_Value_CL = HlEX.CreateContextValue(0);
            var Assassin_Create_Poison_Ability_Any_Context_Value_Concentration = HlEX.CreateContextValue(0);
            var Assassin_Create_Poison_Ability_Any_Context_Value_Spell_Level = HlEX.CreateContextValue(0);

            //The general idea behind the changes are the following:
            // - ContextRankConfigs are removed from all levels but the top-most level (specific ability).
            // - Frequency and Recovery use ContextRanks because they have a relatively high cap (2 and 1 respectively).
            // - Stickiness and Concentration properties can't be lower than 1 (as it is the base value) and the features add bonus to them.
            // - Stickiness, Concentration, Frequency and Recovery are associated with custom-made shared values. 
            // - Toxicity (DC) is calculated with a ContextCalculatSetAbilityParams and the associated property already includes adjustments.

            var Assassin_Create_Poison_Ability_Any_Context_Rank_Config_Frequency = Helpers.CreateContextRankConfig(ContextRankBaseValueType.CustomProperty, ContextRankProgression.BonusValue, NewAbilityRankType.PoisonFrequencyBonus, 2, null, 0, 4, false, StatType.Unknown, Create_Poison_Frequency_Adjustment_Property, null, null, null, null, null);
            var Assassin_Create_Poison_Ability_Any_Context_Rank_Config_Recovery = Helpers.CreateContextRankConfig(ContextRankBaseValueType.CustomProperty, ContextRankProgression.BonusValue, NewAbilityRankType.PoisonRecoveryBonus, 1, null, 0, 1, false, StatType.Unknown, Create_Poison_Recovery_Adjustment_Property, null, null, null, null, null);

            var Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Stickiness = HlEX.CreateContextCalculateSharedValue(HlEX.CreateContextDiceValue(DiceType.Zero, HlEX.CreateContextValue(0),HlEX.CreateContextValueFromCustomCasterProperty(CreatePoisonStickinessProperty)), NewAbilitySharedValue.PoisonStickiness, 1.0F);
            var Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Concentration = HlEX.CreateContextCalculateSharedValue(HlEX.CreateContextDiceValue(DiceType.Zero, HlEX.CreateContextValue(0), HlEX.CreateContextValueFromCustomCasterProperty(CreatePoisonConcentrationProperty)), NewAbilitySharedValue.PoisonConcentration, 1.0F);
            var Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Frequency = HlEX.CreateContextCalculateSharedValue(HlEX.CreateContextDiceValue(DiceType.Zero, HlEX.CreateContextValue(0), HlEX.CreateContextValue(NewAbilityRankType.PoisonFrequencyBonus)), NewAbilitySharedValue.PoisonFrequency, 1.0f);
            var Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Recovery = HlEX.CreateContextCalculateSharedValue(HlEX.CreateContextDiceValue(DiceType.Zero, HlEX.CreateContextValue(0), HlEX.CreateContextValue(NewAbilityRankType.PoisonRecoveryBonus)), NewAbilitySharedValue.PoisonRecovery, 1.0f);

            var Assassin_Create_Poison_Ability_Any_Context_Set_Ability_Params = HlEX.CreateContextSetAbilityParams(Assassin_Create_Poison_Ability_Any_Context_Value_DC, Assassin_Create_Poison_Ability_Any_Context_Value_CL, Assassin_Create_Poison_Ability_Any_Context_Value_Concentration, Assassin_Create_Poison_Ability_Any_Context_Value_Spell_Level, true);
            var Assassin_Create_Poison_Ability_Any_Ability_Resource_Logic = Assassin_Create_Poison_Ability_Ability_Resource.CreateResourceLogic();
            var Assassin_Create_Poison_Ability_Any_Ability_Caster_Has_No_Facts = HlEX.CreateAbilityCasterHasNoFacts(Assassin_Create_Poison_Ability_Str_Buff.ToReference<BlueprintUnitFactReference>(), Assassin_Create_Poison_Ability_Dex_Buff.ToReference<BlueprintUnitFactReference>(), Assassin_Create_Poison_Ability_Con_Buff.ToReference<BlueprintUnitFactReference>());

            #endregion

            #region |--------------------------------------------------| Clean Assassin's Poison Use Effect Buff Blueprint  |--------------------------------------------------|



            var Assassin_Create_Poison_Ability_Any_Buff_Effect_Context_Dice_Value = HlEX.CreateContextDiceValue(DiceType.D4, HlEX.CreateContextValue(1), HlEX.CreateContextValue(0));
            var Assassin_Create_Poison_Ability_Any_Buff_Effect_Frequency = HlEX.CreateContextValue(NewAbilitySharedValue.PoisonFrequency);
            var Assassin_Create_Poison_Ability_Any_Buff_Effect_Recovery = HlEX.CreateContextValue(NewAbilitySharedValue.PoisonRecovery);

            Assassin_Create_Poison_Ability_Str_Buff_Effect.TemporaryContext(bp => {
                bp.ReplaceComponents<BuffPoisonStatDamageContext>(HlEX.CreateBuffPoisonStatDamageContext(ModifierDescriptor.StatDamage, StatType.Strength, Assassin_Create_Poison_Ability_Any_Buff_Effect_Context_Dice_Value, HlEX.CreateContextValue(0), Assassin_Create_Poison_Ability_Any_Buff_Effect_Frequency, Assassin_Create_Poison_Ability_Any_Buff_Effect_Recovery, SavingThrowType.Fortitude, false));
                bp.RemoveComponents<ContextRankConfig>();
                bp.Stacking = StackingType.Poison;
             });

            ToDContext.Logger.LogPatch("Changed Assassin Poison (Strength) effect buff to use shared values and stack as poison.", Assassin_Create_Poison_Ability_Str_Buff_Effect);

            Assassin_Create_Poison_Ability_Dex_Buff_Effect.TemporaryContext(bp => {
                bp.ReplaceComponents<BuffPoisonStatDamageContext>(HlEX.CreateBuffPoisonStatDamageContext(ModifierDescriptor.StatDamage, StatType.Dexterity, Assassin_Create_Poison_Ability_Any_Buff_Effect_Context_Dice_Value, HlEX.CreateContextValue(0), Assassin_Create_Poison_Ability_Any_Buff_Effect_Frequency, Assassin_Create_Poison_Ability_Any_Buff_Effect_Recovery, SavingThrowType.Fortitude, false));
                bp.RemoveComponents<ContextRankConfig>();
                bp.Stacking = StackingType.Poison;
            });

            ToDContext.Logger.LogPatch("Changed Assassin Poison (Dexterity) effect buff to use shared values and stack as poison.", Assassin_Create_Poison_Ability_Dex_Buff_Effect);

            Assassin_Create_Poison_Ability_Con_Buff_Effect.TemporaryContext(bp => {
                bp.ReplaceComponents<BuffPoisonStatDamageContext>(HlEX.CreateBuffPoisonStatDamageContext(ModifierDescriptor.StatDamage, StatType.Constitution, Assassin_Create_Poison_Ability_Any_Buff_Effect_Context_Dice_Value, HlEX.CreateContextValue(0), Assassin_Create_Poison_Ability_Any_Buff_Effect_Frequency, Assassin_Create_Poison_Ability_Any_Buff_Effect_Recovery, SavingThrowType.Fortitude, false));
                bp.RemoveComponents<ContextRankConfig>();
                bp.Stacking = StackingType.Poison;
            });

            ToDContext.Logger.LogPatch("Changed Assassin Poison (Constitution) effect buff to use shared values and stack as poison.", Assassin_Create_Poison_Ability_Con_Buff_Effect);

            #endregion

            #region |------------------------------------------------| Create Assassin's Poison Use Applying Buff Parameters  |------------------------------------------------|

            // Creating the repeated action apply buff to benefit from exhanced concentration.

            var Assassin_Create_Poison_Ability_Str_Buff_Repeated_Apply_Action = HlEX.CreateContextActionRepeatedApplyBuff(Assassin_Create_Poison_Ability_Str_Buff_Effect, HlEX.CreateContextDuration(), HlEX.CreateContextValue(NewAbilitySharedValue.PoisonConcentration), false, true, false, false, false, false);
            var Assassin_Create_Poison_Ability_Dex_Buff_Repeated_Apply_Action = HlEX.CreateContextActionRepeatedApplyBuff(Assassin_Create_Poison_Ability_Dex_Buff_Effect, HlEX.CreateContextDuration(), HlEX.CreateContextValue(NewAbilitySharedValue.PoisonConcentration), false, true, false, false, false, false);
            var Assassin_Create_Poison_Ability_Con_Buff_Repeated_Apply_Action = HlEX.CreateContextActionRepeatedApplyBuff(Assassin_Create_Poison_Ability_Con_Buff_Effect, HlEX.CreateContextDuration(), HlEX.CreateContextValue(NewAbilitySharedValue.PoisonConcentration), false, true, false, false, false, false);

            // Creating the new condition saved.

            var Assassin_Create_Poison_Ability_Str_Buff_Cond_Save = HlEX.CreateContextActionConditionalSaved(null, Assassin_Create_Poison_Ability_Str_Buff_Repeated_Apply_Action);
            var Assassin_Create_Poison_Ability_Dex_Buff_Cond_Save = HlEX.CreateContextActionConditionalSaved(null, Assassin_Create_Poison_Ability_Dex_Buff_Repeated_Apply_Action);
            var Assassin_Create_Poison_Ability_Con_Buff_Cond_Save = HlEX.CreateContextActionConditionalSaved(null, Assassin_Create_Poison_Ability_Con_Buff_Repeated_Apply_Action);

            // Creating the Conditional DC increases

            var Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Array_Normal = new ContextActionSavingThrow.ConditionalDCIncrease[0];
            var Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Array_Sneak = new ContextActionSavingThrow.ConditionalDCIncrease[0];
            var Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Array_Critical = new ContextActionSavingThrow.ConditionalDCIncrease[0];
            var Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Array_Critical_Sneak = new ContextActionSavingThrow.ConditionalDCIncrease[0];

            // Creating the Conditional DC increase with (normal) sneak attack.

            var Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Sneak = new ContextActionSavingThrow.ConditionalDCIncrease();

            Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Sneak.TemporaryContext(bp => {
                bp.Condition = HlEX.CreateConditionsCheckerAnd(HlEX.CreateConditionTrue());
                bp.Value = HlEX.CreateContextValueFromCustomCasterProperty(Create_Poison_Sneak_Attack_Toxicity_Boost_Property);
            });

            Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Array_Sneak.TemporaryContext(cd => {
                cd.AppendToArray(Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Sneak);
            });

            // Creating the Conditional DC increase with (normal) critical hit.

            var Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Critical = new ContextActionSavingThrow.ConditionalDCIncrease();

            Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Critical.TemporaryContext(bp => {
                bp.Condition = HlEX.CreateConditionsCheckerAnd(HlEX.CreateConditionTrue());
                bp.Value = HlEX.CreateContextValueFromCustomCasterProperty(Create_Poison_Critical_Hit_Toxicity_Boost_Property);
            });

            Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Array_Critical.TemporaryContext(cd => {
                cd.AppendToArray(Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Critical);
            });

            // Creating the Conditional DC increase with critical hit with sneak attack.

            Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Array_Critical_Sneak.TemporaryContext(cd => {
                cd.AppendToArray(Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Sneak);
                cd.AppendToArray(Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Critical);
            });

            // Creating the new saving throw actions.

            // Strength

            var Assassin_Create_Poison_Ability_Str_Buff_Saving_Throw_Action_Normal = HlEX.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(Assassin_Create_Poison_Ability_Str_Buff_Cond_Save));
            Assassin_Create_Poison_Ability_Str_Buff_Saving_Throw_Action_Normal.TemporaryContext(a => {
                 a.m_ConditionalDCIncrease = Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Array_Normal;
             });

            var Assassin_Create_Poison_Ability_Str_Buff_Saving_Throw_Action_Sneak = HlEX.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(Assassin_Create_Poison_Ability_Str_Buff_Cond_Save));
            Assassin_Create_Poison_Ability_Str_Buff_Saving_Throw_Action_Sneak.TemporaryContext(a => {
                a.m_ConditionalDCIncrease = Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Array_Sneak;
            });

            var Assassin_Create_Poison_Ability_Str_Buff_Saving_Throw_Action_Critical = HlEX.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(Assassin_Create_Poison_Ability_Str_Buff_Cond_Save));
            Assassin_Create_Poison_Ability_Str_Buff_Saving_Throw_Action_Critical.TemporaryContext(a => {
                a.m_ConditionalDCIncrease = Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Array_Critical;
            });

            var Assassin_Create_Poison_Ability_Str_Buff_Saving_Throw_Action_Critical_Sneak = HlEX.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(Assassin_Create_Poison_Ability_Str_Buff_Cond_Save));
            Assassin_Create_Poison_Ability_Str_Buff_Saving_Throw_Action_Critical_Sneak.TemporaryContext(a => {
                a.m_ConditionalDCIncrease = Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Array_Critical_Sneak;
            });

            // Dexterity

            var Assassin_Create_Poison_Ability_Dex_Buff_Saving_Throw_Action_Normal = HlEX.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(Assassin_Create_Poison_Ability_Dex_Buff_Cond_Save));
            Assassin_Create_Poison_Ability_Dex_Buff_Saving_Throw_Action_Normal.TemporaryContext(a => {
                a.m_ConditionalDCIncrease = Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Array_Normal;
            });

            var Assassin_Create_Poison_Ability_Dex_Buff_Saving_Throw_Action_Sneak = HlEX.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(Assassin_Create_Poison_Ability_Dex_Buff_Cond_Save));
            Assassin_Create_Poison_Ability_Dex_Buff_Saving_Throw_Action_Sneak.TemporaryContext(a => {
                a.m_ConditionalDCIncrease = Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Array_Sneak;
            });

            var Assassin_Create_Poison_Ability_Dex_Buff_Saving_Throw_Action_Critical = HlEX.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(Assassin_Create_Poison_Ability_Dex_Buff_Cond_Save));
            Assassin_Create_Poison_Ability_Dex_Buff_Saving_Throw_Action_Critical.TemporaryContext(a => {
                a.m_ConditionalDCIncrease = Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Array_Critical;
            });

            var Assassin_Create_Poison_Ability_Dex_Buff_Saving_Throw_Action_Critical_Sneak = HlEX.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(Assassin_Create_Poison_Ability_Dex_Buff_Cond_Save));
            Assassin_Create_Poison_Ability_Dex_Buff_Saving_Throw_Action_Critical_Sneak.TemporaryContext(a => {
                a.m_ConditionalDCIncrease = Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Array_Critical_Sneak;
            });

            // Constitution

            var Assassin_Create_Poison_Ability_Con_Buff_Saving_Throw_Action_Normal = HlEX.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(Assassin_Create_Poison_Ability_Con_Buff_Cond_Save));
            Assassin_Create_Poison_Ability_Con_Buff_Saving_Throw_Action_Normal.TemporaryContext(a => {
                a.m_ConditionalDCIncrease = Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Array_Normal;
            });

            var Assassin_Create_Poison_Ability_Con_Buff_Saving_Throw_Action_Sneak = HlEX.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(Assassin_Create_Poison_Ability_Con_Buff_Cond_Save));
            Assassin_Create_Poison_Ability_Con_Buff_Saving_Throw_Action_Sneak.TemporaryContext(a => {
                a.m_ConditionalDCIncrease = Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Array_Sneak;
            });

            var Assassin_Create_Poison_Ability_Con_Buff_Saving_Throw_Action_Critical = HlEX.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(Assassin_Create_Poison_Ability_Con_Buff_Cond_Save));
            Assassin_Create_Poison_Ability_Con_Buff_Saving_Throw_Action_Critical.TemporaryContext(a => {
                a.m_ConditionalDCIncrease = Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Array_Critical;
            });

            var Assassin_Create_Poison_Ability_Con_Buff_Saving_Throw_Action_Critical_Sneak = HlEX.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(Assassin_Create_Poison_Ability_Con_Buff_Cond_Save));
            Assassin_Create_Poison_Ability_Con_Buff_Saving_Throw_Action_Critical_Sneak.TemporaryContext(a => {
                a.m_ConditionalDCIncrease = Assassin_Create_Poison_Ability_Any_Buff_Conditional_DC_Increase_Array_Critical_Sneak;
            });

            // Creating the new RankRemoving Action

            var Assassin_Create_Poison_Ability_Str_Buff_Remove_Rank = HlEX.CreateContextActionRemoveBuffRank(Assassin_Create_Poison_Ability_Str_Buff);
            var Assassin_Create_Poison_Ability_Dex_Buff_Remove_Rank = HlEX.CreateContextActionRemoveBuffRank(Assassin_Create_Poison_Ability_Dex_Buff);
            var Assassin_Create_Poison_Ability_Con_Buff_Remove_Rank = HlEX.CreateContextActionRemoveBuffRank(Assassin_Create_Poison_Ability_Con_Buff);


            // Creating the new AddInitiatorAttackWithWeaponTriggers

            // Strength

            var Assassin_Create_Poison_Ability_Str_Buff_Sneak_Attack_With_Weapon_Trigger = HlEX.CreateAddInitiatorAttackWithWeaponTrigger(Helpers.CreateActionList(Assassin_Create_Poison_Ability_Str_Buff_Saving_Throw_Action_Sneak, Assassin_Create_Poison_Ability_Str_Buff_Remove_Rank), true, false, true, false, false, false, WeaponRangeType.Melee);
            var Assassin_Create_Poison_Ability_Str_Buff_Normal_Attack_With_Weapon_Trigger = HlEX.CreateAddInitiatorAttackWithWeaponTrigger(Helpers.CreateActionList(Assassin_Create_Poison_Ability_Str_Buff_Saving_Throw_Action_Normal, Assassin_Create_Poison_Ability_Str_Buff_Remove_Rank), true, false, false, false, false, false, WeaponRangeType.Melee);
            Assassin_Create_Poison_Ability_Str_Buff_Normal_Attack_With_Weapon_Trigger.TemporaryContext(a => {
                a.NotSneakAttack = true;
                a.NotCriticalHit = true;
            });
            var Assassin_Create_Poison_Ability_Str_Buff_Critical_Attack_With_Weapon_Trigger = HlEX.CreateAddInitiatorAttackWithWeaponTrigger(Helpers.CreateActionList(Assassin_Create_Poison_Ability_Str_Buff_Saving_Throw_Action_Critical, Assassin_Create_Poison_Ability_Str_Buff_Remove_Rank), true, true, false, false, false, false, WeaponRangeType.Melee);
            Assassin_Create_Poison_Ability_Str_Buff_Critical_Attack_With_Weapon_Trigger.TemporaryContext(a => {
                a.NotSneakAttack = true;
            });
            var Assassin_Create_Poison_Ability_Str_Buff_Critical_Sneak_Attack_With_Weapon_Trigger = HlEX.CreateAddInitiatorAttackWithWeaponTrigger(Helpers.CreateActionList(Assassin_Create_Poison_Ability_Str_Buff_Saving_Throw_Action_Critical_Sneak, Assassin_Create_Poison_Ability_Str_Buff_Remove_Rank), true, true, true, false, false, false, WeaponRangeType.Melee);
            Assassin_Create_Poison_Ability_Str_Buff_Critical_Sneak_Attack_With_Weapon_Trigger.TemporaryContext(a => {
                a.NotSneakAttack = false;
            });

            Assassin_Create_Poison_Ability_Str_Buff_Sneak_Attack_With_Weapon_Trigger.WaitForAttackResolve = true;
            Assassin_Create_Poison_Ability_Str_Buff_Normal_Attack_With_Weapon_Trigger.WaitForAttackResolve = true;
            Assassin_Create_Poison_Ability_Str_Buff_Critical_Attack_With_Weapon_Trigger.WaitForAttackResolve = true;
            Assassin_Create_Poison_Ability_Str_Buff_Critical_Sneak_Attack_With_Weapon_Trigger.WaitForAttackResolve = true;

            // Dexterity

            var Assassin_Create_Poison_Ability_Dex_Buff_Sneak_Attack_With_Weapon_Trigger = HlEX.CreateAddInitiatorAttackWithWeaponTrigger(Helpers.CreateActionList(Assassin_Create_Poison_Ability_Dex_Buff_Saving_Throw_Action_Sneak, Assassin_Create_Poison_Ability_Dex_Buff_Remove_Rank), true, false, true, false, false, false, WeaponRangeType.Melee);
            var Assassin_Create_Poison_Ability_Dex_Buff_Normal_Attack_With_Weapon_Trigger = HlEX.CreateAddInitiatorAttackWithWeaponTrigger(Helpers.CreateActionList(Assassin_Create_Poison_Ability_Dex_Buff_Saving_Throw_Action_Normal, Assassin_Create_Poison_Ability_Dex_Buff_Remove_Rank), true, false, false, false, false, false, WeaponRangeType.Melee);
            Assassin_Create_Poison_Ability_Dex_Buff_Normal_Attack_With_Weapon_Trigger.TemporaryContext(a => {
                a.NotSneakAttack = true;
            });
            var Assassin_Create_Poison_Ability_Dex_Buff_Critical_Attack_With_Weapon_Trigger = HlEX.CreateAddInitiatorAttackWithWeaponTrigger(Helpers.CreateActionList(Assassin_Create_Poison_Ability_Dex_Buff_Saving_Throw_Action_Critical, Assassin_Create_Poison_Ability_Dex_Buff_Remove_Rank), true, true, false, false, false, false, WeaponRangeType.Melee);
            Assassin_Create_Poison_Ability_Dex_Buff_Critical_Attack_With_Weapon_Trigger.TemporaryContext(a => {
                a.NotSneakAttack = true;
            });
            var Assassin_Create_Poison_Ability_Dex_Buff_Critical_Sneak_Attack_With_Weapon_Trigger = HlEX.CreateAddInitiatorAttackWithWeaponTrigger(Helpers.CreateActionList(Assassin_Create_Poison_Ability_Dex_Buff_Saving_Throw_Action_Critical_Sneak, Assassin_Create_Poison_Ability_Dex_Buff_Remove_Rank), true, true, true, false, false, false, WeaponRangeType.Melee);
            Assassin_Create_Poison_Ability_Dex_Buff_Critical_Sneak_Attack_With_Weapon_Trigger.TemporaryContext(a => {
                a.NotSneakAttack = false;
            });

            Assassin_Create_Poison_Ability_Dex_Buff_Sneak_Attack_With_Weapon_Trigger.WaitForAttackResolve = true;
            Assassin_Create_Poison_Ability_Dex_Buff_Normal_Attack_With_Weapon_Trigger.WaitForAttackResolve = true;
            Assassin_Create_Poison_Ability_Dex_Buff_Critical_Attack_With_Weapon_Trigger.WaitForAttackResolve = true;
            Assassin_Create_Poison_Ability_Dex_Buff_Critical_Sneak_Attack_With_Weapon_Trigger.WaitForAttackResolve = true;

            // Constitution

            var Assassin_Create_Poison_Ability_Con_Buff_Sneak_Attack_With_Weapon_Trigger = HlEX.CreateAddInitiatorAttackWithWeaponTrigger(Helpers.CreateActionList(Assassin_Create_Poison_Ability_Con_Buff_Saving_Throw_Action_Sneak, Assassin_Create_Poison_Ability_Con_Buff_Remove_Rank), true, false, true, false, false, false, WeaponRangeType.Melee);
            var Assassin_Create_Poison_Ability_Con_Buff_Normal_Attack_With_Weapon_Trigger = HlEX.CreateAddInitiatorAttackWithWeaponTrigger(Helpers.CreateActionList(Assassin_Create_Poison_Ability_Con_Buff_Saving_Throw_Action_Normal, Assassin_Create_Poison_Ability_Con_Buff_Remove_Rank), true, false, false, false, false, false, WeaponRangeType.Melee);
            Assassin_Create_Poison_Ability_Con_Buff_Normal_Attack_With_Weapon_Trigger.TemporaryContext(a => {
                a.NotSneakAttack = true;
            });
            var Assassin_Create_Poison_Ability_Con_Buff_Critical_Attack_With_Weapon_Trigger = HlEX.CreateAddInitiatorAttackWithWeaponTrigger(Helpers.CreateActionList(Assassin_Create_Poison_Ability_Con_Buff_Saving_Throw_Action_Critical, Assassin_Create_Poison_Ability_Con_Buff_Remove_Rank), true, true, false, false, false, false, WeaponRangeType.Melee);
            Assassin_Create_Poison_Ability_Con_Buff_Critical_Attack_With_Weapon_Trigger.TemporaryContext(a => {
                a.NotSneakAttack = true;
            });
            var Assassin_Create_Poison_Ability_Con_Buff_Critical_Sneak_Attack_With_Weapon_Trigger = HlEX.CreateAddInitiatorAttackWithWeaponTrigger(Helpers.CreateActionList(Assassin_Create_Poison_Ability_Con_Buff_Saving_Throw_Action_Critical_Sneak, Assassin_Create_Poison_Ability_Con_Buff_Remove_Rank), true, true, true, false, false, false, WeaponRangeType.Melee);
            Assassin_Create_Poison_Ability_Con_Buff_Critical_Sneak_Attack_With_Weapon_Trigger.TemporaryContext(a => {
                a.NotSneakAttack = false;
            });

            Assassin_Create_Poison_Ability_Con_Buff_Sneak_Attack_With_Weapon_Trigger.WaitForAttackResolve = true;
            Assassin_Create_Poison_Ability_Con_Buff_Normal_Attack_With_Weapon_Trigger.WaitForAttackResolve = true;
            Assassin_Create_Poison_Ability_Con_Buff_Critical_Attack_With_Weapon_Trigger.WaitForAttackResolve = true;
            Assassin_Create_Poison_Ability_Con_Buff_Critical_Sneak_Attack_With_Weapon_Trigger.WaitForAttackResolve = true;

            // Creating the new AddInitiatorAttackWithWeaponTriggers to the Effect-applying Buffs

            Assassin_Create_Poison_Ability_Str_Buff.TemporaryContext(bp => {
                bp.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                bp.RemoveComponents<ContextRankConfig>();
                bp.RemoveComponents<ContextSetAbilityParams>();
                bp.AddComponent(Assassin_Create_Poison_Ability_Str_Buff_Critical_Sneak_Attack_With_Weapon_Trigger);
                bp.AddComponent(Assassin_Create_Poison_Ability_Str_Buff_Sneak_Attack_With_Weapon_Trigger);
                bp.AddComponent(Assassin_Create_Poison_Ability_Str_Buff_Critical_Attack_With_Weapon_Trigger);
                bp.AddComponent(Assassin_Create_Poison_Ability_Str_Buff_Normal_Attack_With_Weapon_Trigger);
                bp.m_Flags = (BlueprintBuff.Flags)0;
                bp.Ranks = 300;
                bp.Stacking = StackingType.Rank;
            });

            ToDContext.Logger.LogPatch("Changed Assassin Poison (Strength) effect-applying buff to remove ranks (not the whole buff) and applying multiple effect buffs.", Assassin_Create_Poison_Ability_Str_Buff);

            Assassin_Create_Poison_Ability_Dex_Buff.TemporaryContext(bp => {
                bp.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                bp.RemoveComponents<ContextRankConfig>();
                bp.RemoveComponents<ContextSetAbilityParams>();
                bp.AddComponent(Assassin_Create_Poison_Ability_Dex_Buff_Critical_Sneak_Attack_With_Weapon_Trigger);
                bp.AddComponent(Assassin_Create_Poison_Ability_Dex_Buff_Sneak_Attack_With_Weapon_Trigger);
                bp.AddComponent(Assassin_Create_Poison_Ability_Dex_Buff_Critical_Attack_With_Weapon_Trigger);
                bp.AddComponent(Assassin_Create_Poison_Ability_Dex_Buff_Normal_Attack_With_Weapon_Trigger);
                bp.m_Flags = (BlueprintBuff.Flags)0;
                bp.Ranks = 300;
                bp.Stacking = StackingType.Rank;
            });

            ToDContext.Logger.LogPatch("Changed Assassin Poison (Dexterity) effect-applying buff to remove ranks (not the whole buff) and applying multiple effect buffs.", Assassin_Create_Poison_Ability_Dex_Buff);

            Assassin_Create_Poison_Ability_Con_Buff.TemporaryContext(bp => {
                bp.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                bp.RemoveComponents<ContextRankConfig>();
                bp.RemoveComponents<ContextSetAbilityParams>();
                bp.AddComponent(Assassin_Create_Poison_Ability_Con_Buff_Critical_Sneak_Attack_With_Weapon_Trigger);
                bp.AddComponent(Assassin_Create_Poison_Ability_Con_Buff_Sneak_Attack_With_Weapon_Trigger);
                bp.AddComponent(Assassin_Create_Poison_Ability_Con_Buff_Critical_Attack_With_Weapon_Trigger);
                bp.AddComponent(Assassin_Create_Poison_Ability_Con_Buff_Normal_Attack_With_Weapon_Trigger);
                bp.m_Flags = (BlueprintBuff.Flags)0;
                bp.Ranks = 300;
                bp.Stacking = StackingType.Rank;
            });

            ToDContext.Logger.LogPatch("Changed Assassin Poison (Constitution) effect-applying buff to remove ranks (not the whole buff) and applying multiple effect buffs.", Assassin_Create_Poison_Ability_Con_Buff);


            #endregion

            #region |--------------------| Changed Assassin's Poison Use Abilities (Move and Swift) to Use Shared Values and Apply Multiple Buffs |----------------------------|

            // Creating the new CreateContextActionApplyBuffRanks

            var Assassin_Create_Poison_Ability_Str_Apply_Buff_Multi_Apply_Action = HlEX.CreateContextActionRepeatedApplyBuff(Assassin_Create_Poison_Ability_Str_Buff, HlEX.CreateContextDuration(1, DurationRate.Days), HlEX.CreateContextValue(NewAbilitySharedValue.PoisonStickiness), false, true, false, true, false, false);
            var Assassin_Create_Poison_Ability_Dex_Apply_Buff_Multi_Apply_Action = HlEX.CreateContextActionRepeatedApplyBuff(Assassin_Create_Poison_Ability_Dex_Buff, HlEX.CreateContextDuration(1, DurationRate.Days), HlEX.CreateContextValue(NewAbilitySharedValue.PoisonStickiness), false, true, false, true, false, false);
            var Assassin_Create_Poison_Ability_Con_Apply_Buff_Multi_Apply_Action = HlEX.CreateContextActionRepeatedApplyBuff(Assassin_Create_Poison_Ability_Con_Buff, HlEX.CreateContextDuration(1, DurationRate.Days), HlEX.CreateContextValue(NewAbilitySharedValue.PoisonStickiness), false, true, false, true, false, false);

            // Clear the abilities by setting them with the new components.

            Assassin_Create_Poison_Ability_Str.TemporaryContext(bp => {
                bp.SetComponents(new BlueprintComponent[] { HlEX.CreateRunActions(Assassin_Create_Poison_Ability_Str_Apply_Buff_Multi_Apply_Action), Assassin_Create_Poison_Ability_Any_Ability_Resource_Logic, Assassin_Create_Poison_Ability_Any_Ability_Caster_Has_No_Facts, Assassin_Create_Poison_Ability_Any_Context_Set_Ability_Params, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Stickiness, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Concentration, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Frequency, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Recovery });
            });

            ToDContext.Logger.LogPatch("Changed Assassin Poison (Strength) standard ability to use Shared Values and apply multiple Buff ranks.", Assassin_Create_Poison_Ability_Str);

            Assassin_Create_Poison_Ability_Swift_Str.TemporaryContext(bp => {
                bp.SetComponents(new BlueprintComponent[] {HlEX.CreateRunActions(Assassin_Create_Poison_Ability_Str_Apply_Buff_Multi_Apply_Action), Assassin_Create_Poison_Ability_Any_Ability_Resource_Logic, Assassin_Create_Poison_Ability_Any_Ability_Caster_Has_No_Facts, Assassin_Create_Poison_Ability_Any_Context_Set_Ability_Params, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Stickiness, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Concentration, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Frequency, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Recovery });
            });

            ToDContext.Logger.LogPatch("Changed Assassin Poison (Strength) swift ability to use Shared Values and apply multiple Buff ranks.", Assassin_Create_Poison_Ability_Swift_Str);

            Assassin_Create_Poison_Ability_Dex.TemporaryContext(bp => {
                bp.SetComponents(new BlueprintComponent[] { HlEX.CreateRunActions(Assassin_Create_Poison_Ability_Dex_Apply_Buff_Multi_Apply_Action), Assassin_Create_Poison_Ability_Any_Ability_Resource_Logic, Assassin_Create_Poison_Ability_Any_Ability_Caster_Has_No_Facts, Assassin_Create_Poison_Ability_Any_Context_Set_Ability_Params, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Stickiness, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Concentration, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Frequency, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Recovery });
            });

            ToDContext.Logger.LogPatch("Changed Assassin Poison (Dexterity) standard ability to use Shared Values and apply multiple Buff ranks.", Assassin_Create_Poison_Ability_Dex);

            Assassin_Create_Poison_Ability_Swift_Dex.TemporaryContext(bp => {
                bp.SetComponents(new BlueprintComponent[] { HlEX.CreateRunActions(Assassin_Create_Poison_Ability_Dex_Apply_Buff_Multi_Apply_Action), Assassin_Create_Poison_Ability_Any_Ability_Resource_Logic, Assassin_Create_Poison_Ability_Any_Ability_Caster_Has_No_Facts, Assassin_Create_Poison_Ability_Any_Context_Set_Ability_Params, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Stickiness, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Concentration, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Frequency, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Recovery });
            });

            ToDContext.Logger.LogPatch("Changed Assassin Poison (Dexterity) swift ability to use Shared Values and apply multiple Buff ranks.", Assassin_Create_Poison_Ability_Swift_Dex);

            Assassin_Create_Poison_Ability_Con.TemporaryContext(bp => {
                bp.SetComponents(new BlueprintComponent[] { HlEX.CreateRunActions(Assassin_Create_Poison_Ability_Con_Apply_Buff_Multi_Apply_Action), Assassin_Create_Poison_Ability_Any_Ability_Resource_Logic, Assassin_Create_Poison_Ability_Any_Ability_Caster_Has_No_Facts, Assassin_Create_Poison_Ability_Any_Context_Set_Ability_Params, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Stickiness, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Concentration, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Frequency, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Recovery });
            });

            ToDContext.Logger.LogPatch("Changed Assassin Poison (Constitution) standard ability to use Shared Values and apply multiple Buff ranks.", Assassin_Create_Poison_Ability_Con);

            Assassin_Create_Poison_Ability_Swift_Con.TemporaryContext(bp => {
                bp.SetComponents(new BlueprintComponent[] { HlEX.CreateRunActions(Assassin_Create_Poison_Ability_Con_Apply_Buff_Multi_Apply_Action), Assassin_Create_Poison_Ability_Any_Ability_Resource_Logic, Assassin_Create_Poison_Ability_Any_Ability_Caster_Has_No_Facts, Assassin_Create_Poison_Ability_Any_Context_Set_Ability_Params, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Stickiness, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Concentration, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Frequency, Assassin_Create_Poison_Ability_Any_Context_Calc_Shared_Value_Recovery });
            });

            ToDContext.Logger.LogPatch("Changed Assassin Poison (Constitution) swift ability to use Shared Values and apply multiple Buff ranks.", Assassin_Create_Poison_Ability_Swift_Con);

            Assassin_Create_Poison_Ability_Con.AddComponent(HlEX.CreateAbilityShowIfCasterHasFact(Universal_Create_Poison_Con_Unlock_Feature.ToReference<BlueprintUnitFactReference>(), false));

            ToDContext.Logger.LogPatch("Changed Assassin Poison Ability Constitution Damage to use universal unlock.", Assassin_Create_Poison_Ability_Con);

            Assassin_Create_Poison_Ability_Swift_Con.AddComponent(HlEX.CreateAbilityShowIfCasterHasFact(Universal_Create_Poison_Con_Unlock_Feature.ToReference<BlueprintUnitFactReference>(), false));

            ToDContext.Logger.LogPatch("Changed Assassin Poison Ability Constitution Damage (Swift) to use universal unlock.", Assassin_Create_Poison_Ability_Swift_Con);

            #endregion

            #region |----------------------------------------------------| Add Poison Use Stat to Assassin's Proficiencies |---------------------------------------------------|

            // Note that the Assassin's class is already existing, so, while I cannot add the Poison Use Stat to its Create Poison Ability (since I am using it for other classes too),
            // I still need to add it to an existing feature to ensure it is attached to all NPCs and to the automated progression.
            // The most obvious solution is to attach it to the Weapon Proficiency, since it's unlikely it would be added to someone who's not an assassin.

            Assassin_Proficiencies.TemporaryContext(bp => {
                bp.SetComponents(HlEX.CreateAddFacts(Poisoncraft_Intelligence_Feature.ToReference<BlueprintUnitFactReference>()));
            });

            ToDContext.Logger.LogPatch("Attached Poison Use - Intelligence to 1st level Assassin's proficiencies.", Assassin_Proficiencies);

            #endregion



            #region |-------------------------------------------------------| Change Assassin Poison Use Descriptions |--------------------------------------------------------|

            string Assassin_Create_Poison_Ability_New_Description = "Assassins specialize in poison use. Every day they create special combat poisons, the number of their doses equal to 3 + assassin level, which they can use to envenom their weapons (both melee and ranged). As a {g|Encyclopedia:Move_Action}move action{/g}, an assassin can apply one of the poisons to their weapons (both melee and ranged), and the next successful {g|Encyclopedia:Attack}attack{/g} applies it to the target. {g|Encyclopedia:Saving_Throw}Save{/g} {g|Encyclopedia:DC}DC{/g} of all poisons is 10 + assassin level + relevant stat modifier. If the attack that applied the poison was a sneak attack, the DC is increased by 2.\nAt 1st level, an assassin has access to poisons that deal {g|Encyclopedia:Dice}1d4{/g} stat {g|Encyclopedia:Damage}damage{/g} to {g|Encyclopedia:Strength}Strength{/g} or {g|Encyclopedia:Dexterity}Dexterity{/g} for 4 {g|Encyclopedia:Combat_Round}rounds{/g}.";
            string Assassin_Create_Poison_Swift_Ability_New_Description = "Assassins specialize in poison use. Every day they create special combat poisons, the number of their doses equal to 3 + assassin level, which they can use to envenom their weapons (both melee and ranged). As a {g|Encyclopedia:Swift_Action}swift action{/g}, an assassin can apply one of the poisons to their weapons (both melee and ranged), and the next successful {g|Encyclopedia:Attack}attack{/g} applies it to the target. {g|Encyclopedia:Saving_Throw}Save{/g} {g|Encyclopedia:DC}DC{/g} of all poisons is 10 + assassin level + relevant stat modifier. If the attack that applied the poison was a sneak attack, the DC is increased by 2.\nThe assassin has access to all special combat poisons currently known to him and to any feature that would affect the base Poison Use ability (with {g|Encyclopedia:Move_Action}move action{/g} activation).";

            Assassin_Create_Poison_Ability.TemporaryContext(bp => {
                bp.SetDescription(ToDContext, Assassin_Create_Poison_Ability_New_Description);
            });

            Assassin_Create_Poison_Ability_Str.TemporaryContext(bp => {
                bp.SetDescription(ToDContext, Assassin_Create_Poison_Ability_New_Description + "\n When produced with its basic formula, this specific combat poison deals {g|Encyclopedia:Dice}1d4{/g} stat {g|Encyclopedia:Damage}damage{/g} to {g|Encyclopedia:Strength}Strength{/g} for 4 {g|Encyclopedia:Combat_Round}rounds{/g}.");
            });

            Assassin_Create_Poison_Ability_Dex.TemporaryContext(bp => {
                bp.SetDescription(ToDContext, Assassin_Create_Poison_Ability_New_Description + "\n When produced with its basic formula, this specific combat poison deals {g|Encyclopedia:Dice}1d4{/g} stat {g|Encyclopedia:Damage}damage{/g} to {g|Encyclopedia:Dexterity}Dexterity{/g} for 4 {g|Encyclopedia:Combat_Round}rounds{/g}.");
            });


            Assassin_Create_Poison_Ability_Con.TemporaryContext(bp => {
                bp.SetDescription(ToDContext, Assassin_Create_Poison_Ability_New_Description + "\n When produced with its basic formula, this specific combat poison deals {g|Encyclopedia:Dice}1d4{/g} stat {g|Encyclopedia:Damage}damage{/g} to {g|Encyclopedia:Constitution}Constitution{/g} for 4 {g|Encyclopedia:Combat_Round}rounds{/g}.");
            });

            Assassin_Create_Poison_Swift_Ability.TemporaryContext(bp => {
                bp.SetDescription(ToDContext, Assassin_Create_Poison_Swift_Ability_New_Description);
            });

            Assassin_Create_Poison_Ability_Swift_Str.TemporaryContext(bp => {
                bp.SetDescription(ToDContext, Assassin_Create_Poison_Swift_Ability_New_Description + "\n When produced with its basic formula, this specific combat poison deals {g|Encyclopedia:Dice}1d4{/g} stat {g|Encyclopedia:Damage}damage{/g} to {g|Encyclopedia:Strength}Strength{/g} for 4 {g|Encyclopedia:Combat_Round}rounds{/g}.");
            });

            Assassin_Create_Poison_Ability_Swift_Dex.TemporaryContext(bp => {
                bp.SetDescription(ToDContext, Assassin_Create_Poison_Swift_Ability_New_Description + "\n When produced with its basic formula, this specific combat poison deals {g|Encyclopedia:Dice}1d4{/g} stat {g|Encyclopedia:Damage}damage{/g} to {g|Encyclopedia:Dexterity}Dexterity{/g} for 4 {g|Encyclopedia:Combat_Round}rounds{/g}.");
            });

            Assassin_Create_Poison_Ability_Swift_Con.TemporaryContext(bp => {
                bp.SetDescription(ToDContext, Assassin_Create_Poison_Swift_Ability_New_Description + "\n When produced with its basic formula, this specific combat poison deals {g|Encyclopedia:Dice}1d4{/g} stat {g|Encyclopedia:Damage}damage{/g} to {g|Encyclopedia:Constitution}Constitution{/g} for 4 {g|Encyclopedia:Combat_Round}rounds{/g}.");

            });

            UnityModManagerNet.UnityModManager.Logger.Log("Changed description of the Poison Use feature.");

            #endregion

        }


    }
}
