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
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using static RootMotion.FinalIK.GrounderQuadruped;
using System.Collections.Generic;
using Kingmaker.Armies.Blueprints;
using static Kingmaker.UI.Context.MenuItem;
using Kingmaker.UnitLogic;

namespace TomeOfDarkness.MechanicsChanges
{
    internal class UniversalAssassination
    {

        public static void ConfigureUniversalAssassination()
        {

            AssassinationTraining.ConfigureAssassinationTraining();

            #region |-----------------------------------------------------------| initialize Existing Variables |--------------------------------------------------------------|

            //Vanilla Blueprints
            var Assassin_Death_Attack_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("d86609703c0b37445828a23140385721");                                   
            var Assassin_Death_Attack_Standard_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("b2bcc7efc9d59af42836bc5ba9e1a5e0");
            var Executioner_Assassinate_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("bd7e70e98f9036f4ba27ef3e29a177c2");
            var Executioner_Assassinate19_Messy_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("a10abd69551134341a6f771d23535165");
            var Executioner_Assassinate14_Terroristic_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("c81f15dbbadbb3a48b37e95a5a7ef759");

            var Immunity_To_Critical = BlueprintTools.GetBlueprint<BlueprintFeature>("ced0f4e5d02d5914a9f9ff74acacf26d");
            var Bloodline_Elemental_Air_Elemental_Body_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("7c3be22702ee39a418a5fba0e85e68de");
            var Bloodline_Elemental_Earth_Elemental_Body_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("6541fc1423987a341b30ea68a54f0327");
            var Bloodline_Elemental_Fire_Elemental_Body_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("5d974328297021a479b4e3a1de749126");
            var Bloodline_Elemental_Water_Elemental_Body_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("c459fcee6baabd149ac79acb0cb1d40e");
            var Subtype_Elemental = BlueprintTools.GetBlueprint<BlueprintFeature>("198fd8924dabcb5478d0f78bd453c586");
            var Robe_Of_False_Death_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("3e8617be47ba5d846bd5f06b2eeac1b3");
            var Evasive_Round_Helmet_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("125af9ed1b5e7874fb1c3de10f339708");
            var Helmet_Armag_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("095b64f04a46d5d4c802562a9719b9f0");
            var Forewarning_Shield_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("f0c2ed741b77d28459100eb7546b2621");
            var Mimic_Immunes_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("72c7c3b95972c554b9302c537ae3f73f");
            var Swarm_Diminutive_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("2e3e840ab458ce04c92064489f87ecc2");
            var Swarm_Tiny_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("5a04735fd0e952142bfc8ecf995e2361");


            var Assassin_Death_Attack_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("744e7c7fd58bd6040b40210cf0864692");                                   
            var Assassin_Death_Attack_Ability_Kill = BlueprintTools.GetBlueprint<BlueprintAbility>("ca5575accdf8ee64cb32608a77aaf989");                              
            var Assassin_Death_Attack_Ability_Paralyze = BlueprintTools.GetBlueprint<BlueprintAbility>("452b64ffab80cff40bd27dc5f350d80c");                          

            var Assassin_Death_Attack_Standard_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("68a6086913b7cca4283c62be2295ce81");                          
            var Assassin_Death_Attack_Ability_Kill_Standard = BlueprintTools.GetBlueprint<BlueprintAbility>("02d129b799da92d40b6377bac27d843f");                     
            var Assassin_Death_Attack_Ability_Paralyze_Standard = BlueprintTools.GetBlueprint<BlueprintAbility>("614422dc5cdd3eb4ebf0f9900dd0e0ab");

            var Executioner_Assassinate_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("3dad7f131aa884f4c972f2fb759d0df4");
            var Executioner_Assassinate_Terroristic_Demoralize_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("3e7780219eb23b64f8dac5b29bb32e23");

            var Assassin_Death_Attack_Ability_Kill_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("3740fdf85036ec34bbd0b09f218a9cce");                            
            var Assassin_Death_Attack_Ability_Paralyze_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("b6ebb41266c137c4384b4b279a7f631c");
            var Assassin_Death_Attack_Ability_Kill_Effect_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("fa7cf97ea4dfc4d4aa600e18fd7d419b");
            var Assassin_Death_Attack_Ability_Paralyze_Effect_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("af1e2d232ebbb334aaf25e2a46a92591");
            var Executioner_Assassinate_Messy_Feature_Bleed_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("16249b8075ab8684ca105a78a047a5ef");
            var Slayer_Study_Target_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("45548967b714e254aa83f23354f174b0");

            var Knowledge_Domain_Base_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("35fa55fe2c60e4442b670a88a70c06c3");
            var Wild_Shape_Elemental_Air_Huge_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("eb52d24d6f60fc742b32fe943b919180");
            var Wild_Shape_Elemental_Air_Large_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("814bc75e74f969641bf110addf076ff9");
            var Wild_Shape_Elemental_Earth_Huge_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("f0826c3794c158c4cbbe9ceb4210d6d6");
            var Wild_Shape_Elemental_Earth_Large_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("bf145574939845d43b68e3f4335986b4");
            var Wild_Shape_Elemental_Fire_Huge_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("e85abd773dbce30498efa8da745d7ca7");
            var Wild_Shape_Elemental_Fire_Large_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("7f30b0f7f3c4b6748a2819611fb236f8");
            var Wild_Shape_Elemental_Water_Huge_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("ea2cd08bdf2ca1c4f8a8870804790cd7");
            var Wild_Shape_Elemental_Water_Large_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("5993b78c793667e45bf0380e9275fab7");
            var Elemental_Body_III_Air_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("db00581a03e6947419648dfba6aa03b2");
            var Elemental_Body_III_Earth_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("5d2f3863ead92824ab47b11ef949c611");
            var Elemental_Body_III_Fire_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("56b844a14fcce03429e3e8a2a26cf595");
            var Elemental_Body_III_Water_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("e24ea1f5005649846b798318b5238e34");
            var Elemental_Body_IV_Air_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("ba06b8cff52da9e4d8432144ed6a6d19");
            var Elemental_Body_IV_Earth_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("3c7c12df25d21b344b7cbe12a60038d8");
            var Elemental_Body_IV_Fire_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("6be582eb1f6df4f41875c16d919e3b12");
            var Elemental_Body_IV_Water_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("f0abf98bb3bce4f4e877a8e8c2eccf41");
            var Polymorph_Greater_Elemental_Air_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("2641f73f8d7864f4bba0bd6134018094");
            var Polymorph_Greater_Elemental_Earth_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("c3fad2e285b70664c80fc63f4de1c7e9");
            var Polymorph_Greater_Elemental_Fire_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("02a143c601ee28b479dd409012779056");
            var Polymorph_Greater_Elemental_Water_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("8431e0229c76af74b9b517fdfeb87766");
            var Fiery_Body_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("b574e1583768798468335d8cdb77e94c");


            var Assassin_Death_Attack_Ability_Property = BlueprintTools.GetBlueprint<BlueprintUnitProperty>("857ddbe8d4a742c49a933b893653649f");
            var Main_Weapon_Damage_Stat_Bonus_Property = BlueprintTools.GetBlueprint<BlueprintUnitProperty>("9955f9c72c350254daff5a029ee32712");
            var Main_Weapon_Critical_Modifier_Property = BlueprintTools.GetBlueprint<BlueprintUnitProperty>("6ac8613eca0083d438b48f9e1391f09b");



            //New Blueprints
            var Assassination_Training_FakeLevel = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "AssassinationTrainingFakeLevel");
            var Assassination_Training_Property = BlueprintTools.GetModBlueprint<BlueprintUnitProperty>(ToDContext, "AssassinationTrainingProperty");

            //Existing Icons
            var ExecutionerAssassinateOldIcon = Executioner_Assassinate_Feature.m_Icon;
            var UniversalGoryAssassinationIcon = BlueprintTools.GetBlueprint<BlueprintAbility>("c3d2294a6740bc147870fff652f3ced5").m_Icon; // Death Clutch icon            
            var ExecutionerAssassinateNewIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_AssassinateSlayer.png");

            //Custom Icons
            var AssassinationStatIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_AssassinationTraining.png");

            #endregion


            #region |----------------------------------------------------------| Create Generic Assassinte Stat |-------------------------------------------------------------|

            var Assassination_Strength_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "AssassinationStrengthStatFeature", bp => {
                bp.SetName(ToDContext, "Assassination - Strength");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Strength}Str{/g} modifier to his Assassinate's and Death Attack's DC.");
                bp.m_Icon = AssassinationStatIcon;
                bp.Ranks = 1;
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
            });

            ToDContext.Logger.LogPatch("Created Assassination - Strength feature.", Assassination_Strength_Feature);

            var Assassination_Dexterity_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "AssassinationDexterityStatFeature", bp => {
                bp.SetName(ToDContext, "Assassination - Dexterity");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Dexterity}Dex{/g} modifier to his Assassinate's and Death Attack's DC.");
                bp.m_Icon = AssassinationStatIcon;
                bp.Ranks = 1;
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
            });

            ToDContext.Logger.LogPatch("Created Assassination - Dexterity feature.", Assassination_Dexterity_Feature);

            var Assassination_Constitution_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "AssassinationConstitutionStatFeature", bp => {
                bp.SetName(ToDContext, "Assassination - Constitution");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Constitution}Con{/g} modifier to his Assassinate's and Death Attack's DC.");
                bp.m_Icon = AssassinationStatIcon;
                bp.Ranks = 1;
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
            });

            ToDContext.Logger.LogPatch("Created Assassination - Constitution feature.", Assassination_Constitution_Feature);

            var Assassination_Intelligence_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "AssassinationIntelligenceStatFeature", bp => {
                bp.SetName(ToDContext, "Assassination - Intelligence");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Intelligence}Int{/g} modifier to his Assassinate's and Death Attack's DC.");
                bp.m_Icon = AssassinationStatIcon;
                bp.Ranks = 1;
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
            });

            ToDContext.Logger.LogPatch("Created Assassination - Intelligence feature.", Assassination_Intelligence_Feature);

            var Assassination_Wisdom_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "AssassinationWisdomStatFeature", bp => {
                bp.SetName(ToDContext, "Assassination - Wisdom");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Wisdom}Wis{/g} modifier to his Assassinate's and Death Attack's DC.");
                bp.m_Icon = AssassinationStatIcon;
                bp.Ranks = 1;
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
            });

            ToDContext.Logger.LogPatch("Created Assassination - Wisdom feature.", Assassination_Wisdom_Feature);

            var Assassination_Charisma_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "AssassinationCharismaStatFeature", bp => {
                bp.SetName(ToDContext, "Assassination - Charisma");
                bp.SetDescription(ToDContext, "The character adds his {g|Encyclopedia:Charisma}Cha{/g} modifier to his Assassinate's and Death Attack's DC.");
                bp.m_Icon = AssassinationStatIcon;
                bp.Ranks = 1;
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
            });

            ToDContext.Logger.LogPatch("Created Assassination - Charisma feature.", Assassination_Charisma_Feature);

            var AssassinationStatProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>(ToDContext, "AssassinationStatProperty", bp =>
            {
                bp.AddComponent<CompositeCustomPropertyGetter>(c =>
                {
                    c.CalculationMode = CompositeCustomPropertyGetter.Mode.Highest;
                    c.Properties = new CompositeCustomPropertyGetter.ComplexCustomProperty[] {
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreatePropertyMulipliedByFactRankGetter(UnitProperty.StatStrength, Assassination_Strength_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreatePropertyMulipliedByFactRankGetter(UnitProperty.StatDexterity, Assassination_Dexterity_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreatePropertyMulipliedByFactRankGetter(UnitProperty.StatConstitution, Assassination_Constitution_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreatePropertyMulipliedByFactRankGetter(UnitProperty.StatIntelligence, Assassination_Intelligence_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreatePropertyMulipliedByFactRankGetter(UnitProperty.StatWisdom, Assassination_Wisdom_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreatePropertyMulipliedByFactRankGetter(UnitProperty.StatCharisma, Assassination_Charisma_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                        };
                });
                bp.BaseValue = 0;
            });

            ToDContext.Logger.LogPatch("Created Assassination Stat property.", AssassinationStatProperty);

            #endregion

            #region |-----------------------------------------------------| Assassination Mortality Adjustment Property |-------------------------------------------------------|

            // This property is used to raise or lower the DC of Death Attack and Assassination based on features or buffs possessed.

            var Universal_Assassination_Mortality_Boost_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalAssassinationMortalityBoostFeature", bp => {
                bp.SetName(ToDContext, "Assassination - Mortality Boost");
                bp.SetDescription(ToDContext, "Each rank of this feature adds +1 to the Death Attack's and Assassination's DC.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Assassination Mortality Boost feature.", Universal_Assassination_Mortality_Boost_Feature);

            var Universal_Assassination_Mortality_Boost_Class_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalAssassinationMortalityBoostClassFeature", bp => {
                bp.SetName(ToDContext, "Assassination - Mortality Boost (Class)");
                bp.SetDescription(ToDContext, "Each rank of this feature adds +1 to the Death Attack's and Assassination's DC.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Assassination Mortality Boost (Class) feature.", Universal_Assassination_Mortality_Boost_Class_Feature);

            var Universal_Assassination_Mortality_Boost_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalAssassinationMortalityBoostBluff", bp => {
                bp.SetName(ToDContext, "Assassination - Mortality Boost");
                bp.SetDescription(ToDContext, "Each rank of this feature adds +1 to the Death Attack's and Assassination's DC.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
            });

            ToDContext.Logger.LogPatch("Added Assassination Mortality Boost buff.", Universal_Assassination_Mortality_Boost_Buff);

            var Universal_Assassination_Mortality_Boost_Class_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalAssassinationMortalityBoostClassBluff", bp => {
                bp.SetName(ToDContext, "Assassination - Mortality Boost (Class)");
                bp.SetDescription(ToDContext, "Each rank of this feature adds +1 to the Death Attack's and Assassination's DC.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Assassination Mortality Boost (Class) buff.", Universal_Assassination_Mortality_Boost_Class_Buff);
   
            var Universal_Assassination_Mortality_Reduction_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalAssassinationMortalityReductionFeature", bp => {
                bp.SetName(ToDContext, "Assassination - Mortality Reduction");
                bp.SetDescription(ToDContext, "Each rank of this feature adds -1 to the Death Attack's and Assassination's DC.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Assassination Mortality Reduction feature.", Universal_Assassination_Mortality_Reduction_Feature);
            
            var Universal_Assassination_Mortality_Reduction_Class_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalAssassinationMortalityReductionClassFeature", bp => {
                bp.SetName(ToDContext, "Assassination - Mortality Reduction (Class)");
                bp.SetDescription(ToDContext, "Each rank of this feature adds -1 to the Death Attack's and Assassination's DC.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Assassination Mortality Reduction (Class) feature.", Universal_Assassination_Mortality_Reduction_Class_Feature);

            var Universal_Assassination_Mortality_Reduction_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalAssassinationMortalityReductionBluff", bp => {
                bp.SetName(ToDContext, "Assassination - Mortality Reduction");
                bp.SetDescription(ToDContext, "Each rank of this buff adds -1 to the Death Attack's and Assassination's DC.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
            });

            ToDContext.Logger.LogPatch("Added Assassination Mortality Reduction buff.", Universal_Assassination_Mortality_Reduction_Buff);

            var Universal_Assassination_Mortality_Reduction_Class_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalAssassinationMortalityReductionClassBluff", bp => {
                bp.SetName(ToDContext, "Assassination - Mortality Reduction");
                bp.SetDescription(ToDContext, "Each rank of this buff adds -1 to the Death Attack's and Assassination's DC.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Assassination Mortality Reduction (Class) buff.", Universal_Assassination_Mortality_Reduction_Class_Buff);

            var Assassination_Mortality_Adjustment_Property = Helpers.CreateBlueprint<BlueprintUnitProperty>(ToDContext, "AssassinationMortalityAdjustmentProperty", bp =>
            {
                bp.AddComponent<CompositeCustomPropertyGetter>(c =>
                {
                    c.CalculationMode = CompositeCustomPropertyGetter.Mode.Sum;
                    c.Properties = new CompositeCustomPropertyGetter.ComplexCustomProperty[] {
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Assassination_Mortality_Boost_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Assassination_Mortality_Boost_Buff.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Assassination_Mortality_Boost_Class_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Assassination_Mortality_Boost_Class_Buff.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Assassination_Mortality_Reduction_Feature.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Assassination_Mortality_Reduction_Buff.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Assassination_Mortality_Reduction_Class_Feature.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Assassination_Mortality_Reduction_Class_Buff.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            }
                        };
                });
                bp.BaseValue = 0;
            });

            ToDContext.Logger.LogPatch("Added Assassination Mortality property.", Assassination_Mortality_Adjustment_Property);

            #endregion

            #region |------------------------------------------------| Change Death Attack to use Custom Properties |--------------------------------------------------|

            // This property is changed to use custom properties.

            Assassin_Death_Attack_Ability_Property.TemporaryContext(bp => {
                bp.RemoveComponents<ClassLevelGetter>();
                bp.RemoveComponents<SimplePropertyGetter>();
                bp.AddComponent(HlEX.CreateCustomPropertyGetter(Assassination_Training_Property));
                bp.AddComponent(HlEX.CreateCustomPropertyGetter(AssassinationStatProperty));
                bp.AddComponent(HlEX.CreateCustomPropertyGetter(Assassination_Mortality_Adjustment_Property));
            });

            ToDContext.Logger.LogPatch("Changed Death Attack property to use custom properties.", Assassin_Death_Attack_Ability_Property);

            #endregion

            #region |-----------------------------------------------------| Create Assassinate Custom Property |-------------------------------------------------------|

            // This property is a perfect clone of the Assassin Death Attack Ability copy.

            var Universal_Assassinate_Ability_Property = Assassin_Death_Attack_Ability_Property.CreateCopy(ToDContext, "UniversalAssassinateAbilityProperty");

            ToDContext.Logger.LogPatch("Added Assassination Mortality property.", Universal_Assassinate_Ability_Property);

            #endregion

            #region |-------------------------------------------| Assassination Paralysis Duration Adjustment Property |-----------------------------------------------|

            // This property is used to raise or lower the duration of non-death assassination effects.

            var Universal_Assassination_Paralysis_Duration_Boost_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalAssassinationParalysisDurationBoostFeature", bp => {
                bp.SetName(ToDContext, "Assassination - Paralysis Duration Boost");
                bp.SetDescription(ToDContext, "Each rank of this feature adds +1 to the duration of Death Attack's and Assassination's non-lethal effects.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Assassination Paralysis Duration Boost feature.", Universal_Assassination_Paralysis_Duration_Boost_Feature);

            var Universal_Assassination_Paralysis_Duration_Boost_Class_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalAssassinationParalysisDurationBoostClassFeature", bp => {
                bp.SetName(ToDContext, "Assassination - Paralysis Duration Boost (Class)");
                bp.SetDescription(ToDContext, "Each rank of this feature adds +1 to the duration of Death Attack's and Assassination's non-lethal effects.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Assassination Paralysis Duration Boost (Class) feature.", Universal_Assassination_Paralysis_Duration_Boost_Class_Feature);

            var Universal_Assassination_Paralysis_Duration_Boost_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalAssassinationParalysisDurationBoostBluff", bp => {
                bp.SetName(ToDContext, "Assassination - Paralysis Duration Boost");
                bp.SetDescription(ToDContext, "Each rank of this feature adds +1 to the duration of Death Attack's and Assassination's non-lethal effects.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
            });

            ToDContext.Logger.LogPatch("Added Assassination Paralysis Duration Boost buff.", Universal_Assassination_Paralysis_Duration_Boost_Buff);

            var Universal_Assassination_Paralysis_Duration_Boost_Class_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalAssassinationParalysisDurationBoostClassBluff", bp => {
                bp.SetName(ToDContext, "Assassination - Paralysis Duration Boost (Class)");
                bp.SetDescription(ToDContext, "Each rank of this feature adds +1 to the duration of Death Attack's and Assassination's non-lethal effects.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
                bp.Stacking = StackingType.Rank;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Added Assassination Paralysis Duration Boost (Class) buff.", Universal_Assassination_Paralysis_Duration_Boost_Class_Buff);

            var Assassination_Paralysis_Duration_Boost_Property = Helpers.CreateBlueprint<BlueprintUnitProperty>(ToDContext, "AssassinationParalysisDurationBoostProperty", bp =>
            {
                bp.AddComponent<CompositeCustomPropertyGetter>(c =>
                {
                    c.CalculationMode = CompositeCustomPropertyGetter.Mode.Sum;
                    c.Properties = new CompositeCustomPropertyGetter.ComplexCustomProperty[] {
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Assassination_Mortality_Boost_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Assassination_Mortality_Boost_Buff.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Assassination_Mortality_Boost_Class_Feature.ToReference<BlueprintUnitFactReference>())
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Assassination_Mortality_Boost_Class_Buff.ToReference<BlueprintUnitFactReference>())
                            }
                        };
                });
                bp.BaseValue = 0;
            });

            ToDContext.Logger.LogPatch("Added Assassination Paralysis Duration property.", Assassination_Paralysis_Duration_Boost_Property);

            #endregion

            #region |-----------------------------------------------------| Clean Death Attack Kill Effect Buff |------------------------------------------------------|

            // Remove all obsolete components

            Assassin_Death_Attack_Ability_Kill_Effect_Buff.TemporaryContext(bp => {
                bp.RemoveComponents<ContextSetAbilityParams>();
                bp.RemoveComponents<ContextRankConfig>();
            });

            ToDContext.Logger.LogPatch("Removed obsolete elements from Kill effect buff.", Assassin_Death_Attack_Ability_Kill_Effect_Buff);

            #endregion

            #region |-------------------------------------------------| Change Death Attack Paralysis-Applying Buff |--------------------------------------------------|

            // Remove all obsolete components

            var Assassin_Death_Attack_Ability_Paralysis_Buff_Apply_Action = HlEX.CreateContextActionApplyBuff(Assassin_Death_Attack_Ability_Paralyze_Effect_Buff, HlEX.CreateContextDuration(HlEX.CreateContextValueFromCustomCasterProperty(Assassination_Paralysis_Duration_Boost_Property), DurationRate.Rounds, DiceType.D6, HlEX.CreateContextValue(1)), false, false, false, false, false);

            var Assassin_Death_Attack_Ability_Paralysis_Buff_Cond_Save = HlEX.CreateContextActionConditionalSaved(null, Assassin_Death_Attack_Ability_Paralysis_Buff_Apply_Action);

            var Assassin_Death_Attack_Ability_Paralysis_Buff_Saving_Throw_Action = HlEX.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(Assassin_Death_Attack_Ability_Paralysis_Buff_Cond_Save));

            var Assassin_Death_Attack_Ability_Paralysis_Buff_Attack_With_Weapon_Trigger_Paralyze = HlEX.CreateAddInitiatorAttackWithWeaponTrigger(Helpers.CreateActionList(Assassin_Death_Attack_Ability_Paralysis_Buff_Saving_Throw_Action), true, wait_for_attack_to_resolve: true);

            var Assassin_Death_Attack_Ability_Paralysis_Buff_Attack_With_Weapon_Trigger_Remove_Buff = HlEX.CreateAddInitiatorAttackWithWeaponTrigger(Helpers.CreateActionList(HlEX.CreateContextActionRemoveSelf()), false, wait_for_attack_to_resolve: true);


            Assassin_Death_Attack_Ability_Paralyze_Buff.TemporaryContext(bp => {
                bp.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                bp.RemoveComponents<ContextSetAbilityParams>();
                bp.RemoveComponents<ContextRankConfig>();
                bp.AddComponent(Assassin_Death_Attack_Ability_Paralysis_Buff_Attack_With_Weapon_Trigger_Paralyze);
                bp.AddComponent(Assassin_Death_Attack_Ability_Paralysis_Buff_Attack_With_Weapon_Trigger_Remove_Buff);
            });

            ToDContext.Logger.LogPatch("Removed obsolete elements from Paralyze effect-applying buff.", Assassin_Death_Attack_Ability_Paralyze_Buff);

            #endregion

            #region |---------------------------------------------------| Change Death Attack Death-Applying Buff |----------------------------------------------------|

            // Remove all obsolete components

            var Assassin_Death_Attack_Ability_Kill_Buff_Apply_Action = HlEX.CreateContextActionApplyBuff(Assassin_Death_Attack_Ability_Kill_Effect_Buff, HlEX.CreateContextDuration(HlEX.CreateContextValue(1), DurationRate.Rounds, DiceType.Zero, HlEX.CreateContextValue(0)), false, false, false, false, false);

            var Assassin_Death_Attack_Ability_Kill_Buff_Attack_With_Weapon_Trigger_Kill = HlEX.CreateAddInitiatorAttackWithWeaponTrigger(Helpers.CreateActionList(Assassin_Death_Attack_Ability_Kill_Buff_Apply_Action), true, wait_for_attack_to_resolve: true);

            var Assassin_Death_Attack_Ability_Kill_Buff_Attack_With_Weapon_Trigger_Remove_Buff = HlEX.CreateAddInitiatorAttackWithWeaponTrigger(Helpers.CreateActionList(HlEX.CreateContextActionRemoveSelf()), false, wait_for_attack_to_resolve: true);

            Assassin_Death_Attack_Ability_Kill_Buff.TemporaryContext(bp => {
                bp.RemoveComponents<AddInitiatorAttackWithWeaponTrigger>();
                bp.RemoveComponents<ContextRankConfig>();
                bp.AddComponent(Assassin_Death_Attack_Ability_Kill_Buff_Attack_With_Weapon_Trigger_Kill);
                bp.AddComponent(Assassin_Death_Attack_Ability_Kill_Buff_Attack_With_Weapon_Trigger_Remove_Buff);
            });

            ToDContext.Logger.LogPatch("Removed obsolete elements from Kill effect-applying buff.", Assassin_Death_Attack_Ability_Kill_Buff);

            #endregion


            #region |-------------------------------------------------| Create Death Attack Parameters |--------------------------------------------------|

            // Remove all obsolete components

            var Assassin_Death_Attack_Ability_Any_Context_Value_DC = HlEX.CreateContextValueFromCustomCasterProperty(Assassin_Death_Attack_Ability_Property);
            var Assassin_Death_Attack_Ability_Any_Context_Value_CL = HlEX.CreateContextValue(0);
            var Assassin_Death_Attack_Ability_Any_Context_Value_Concentration = HlEX.CreateContextValue(0);
            var Assassin_Death_Attack_Ability_Any_Context_Value_Spell_Level = HlEX.CreateContextValue(0);

            var Assassin_Death_Attack_Ability_Any_Context_Set_Ability_Params = HlEX.CreateContextSetAbilityParams(Assassin_Death_Attack_Ability_Any_Context_Value_DC, Assassin_Death_Attack_Ability_Any_Context_Value_CL, Assassin_Death_Attack_Ability_Any_Context_Value_Concentration, Assassin_Death_Attack_Ability_Any_Context_Value_Spell_Level, true);

            #endregion

            #region |---------------------------------------------------| Change Death Attack Abilities to Make Them Universal |----------------------------------------------------|



            var Assassin_Death_Attack_Ability_Kill_Execute_Action_On_Cast_Action = HlEX.CreateAbilityExecuteActionOnCast(HlEX.CreateConditionTrue(), HlEX.CreateContextActionOnContextCaster(HlEX.CreateContextActionApplyBuff(Assassin_Death_Attack_Ability_Kill_Buff, HlEX.CreateContextDuration(HlEX.CreateContextValue(1), DurationRate.Rounds, DiceType.Zero, HlEX.CreateContextValue(0)), false, true, false, true, false)));

            Assassin_Death_Attack_Ability_Kill.TemporaryContext(bp => {
                bp.RemoveComponents<ContextRankConfig>();
                bp.RemoveComponents<ContextCalculateSharedValue>();
                bp.ReplaceComponents<AbilityExecuteActionOnCast>(Assassin_Death_Attack_Ability_Kill_Execute_Action_On_Cast_Action);
                bp.ReplaceComponents<ContextSetAbilityParams>(Assassin_Death_Attack_Ability_Any_Context_Set_Ability_Params);
            });

            ToDContext.Logger.LogPatch("Changed Death Attack Kill to take Custom Properties.", Assassin_Death_Attack_Ability_Kill);

            Assassin_Death_Attack_Ability_Kill_Standard.TemporaryContext(bp => {
                bp.RemoveComponents<ContextRankConfig>();
                bp.RemoveComponents<ContextCalculateSharedValue>();
                bp.ReplaceComponents<AbilityExecuteActionOnCast>(Assassin_Death_Attack_Ability_Kill_Execute_Action_On_Cast_Action);
                bp.ReplaceComponents<ContextSetAbilityParams>(Assassin_Death_Attack_Ability_Any_Context_Set_Ability_Params);
            });

            ToDContext.Logger.LogPatch("Changed Death Attack Kill (Standard) to take Custom Properties.", Assassin_Death_Attack_Ability_Kill_Standard);

            Assassin_Death_Attack_Ability_Paralyze.TemporaryContext(bp => {
                bp.RemoveComponents<ContextRankConfig>();
                bp.RemoveComponents<ContextCalculateSharedValue>();
                bp.ReplaceComponents<AbilityExecuteActionOnCast>(Assassin_Death_Attack_Ability_Kill_Execute_Action_On_Cast_Action);
                bp.ReplaceComponents<ContextSetAbilityParams>(Assassin_Death_Attack_Ability_Any_Context_Set_Ability_Params);
            });

            ToDContext.Logger.LogPatch("Changed Death Attack Paralyze to take Custom Properties.", Assassin_Death_Attack_Ability_Paralyze);

            Assassin_Death_Attack_Ability_Paralyze_Standard.TemporaryContext(bp => {
                bp.RemoveComponents<ContextRankConfig>();
                bp.RemoveComponents<ContextCalculateSharedValue>();
                bp.ReplaceComponents<AbilityExecuteActionOnCast>(Assassin_Death_Attack_Ability_Kill_Execute_Action_On_Cast_Action);
                bp.ReplaceComponents<ContextSetAbilityParams>(Assassin_Death_Attack_Ability_Any_Context_Set_Ability_Params);
            });

            ToDContext.Logger.LogPatch("Changed Death Attack Paralyze (Standard) to take Custom Properties.", Assassin_Death_Attack_Ability_Paralyze_Standard);

            #endregion

            #region |-------------------------| Change Executioner Asssassinate Upgrade Features to Work As Activatable Abilities |------------------------------------|

            var Universal_Terroristic_Execution_Active_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalTerroristicExecutionActiveBuff", bp => {
                bp.SetName(ToDContext, "Terroristic Execution");
                bp.SetDescription(ToDContext, "Whenever the character kills an enemy with assassinate, all other enemies in a 30-foot radius become demoralized.");
                bp.m_Icon = ExecutionerAssassinateOldIcon;
                bp.FxOnStart = new PrefabLink();
                bp.FxOnRemove = new PrefabLink();
                bp.Ranks = 1;
                bp.Stacking = StackingType.Replace;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Created Terroristic Execution buff.", Universal_Terroristic_Execution_Active_Buff);

            var Universal_Gory_Assassination_Active_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalGoryAssassinationActiveBuff", bp => {
                bp.SetName(ToDContext, "Gory Assassination");
                bp.SetDescription(ToDContext, "Whenever an enemy survives an assassinate attempt by the character, it suffers {g|Encyclopedia:Dice}2d6{/g} bleed {g|Encyclopedia:Damage}damage{/g}.");
                bp.m_Icon = UniversalGoryAssassinationIcon;
                bp.FxOnStart = new PrefabLink();
                bp.FxOnRemove = new PrefabLink();
                bp.Stacking = StackingType.Replace;
                bp.IsClassFeature = true;
            });

            ToDContext.Logger.LogPatch("Created Gory Assassination buff.", Universal_Gory_Assassination_Active_Buff);

            var Slayer_Terroristic_Execution_Activatable_Ability = HlEX.ConvertBuffToActivatableAbility(Universal_Terroristic_Execution_Active_Buff, UnitCommand.CommandType.Free, true, "Slayer", "Universal", "ToggleAbility", "ActiveBuff");
            Slayer_Terroristic_Execution_Activatable_Ability.TemporaryContext(bp => {
                 bp.ActivationType = AbilityActivationType.Immediately;
                 bp.m_ActivateOnUnitAction = AbilityActivateOnUnitActionType.Attack;
                 bp.SetDescription(ToDContext, "Whenever this ability is activated and the character kills an enemy with assassinate, all other enemies in a 30-foot radius become demoralized.");
            });

            ToDContext.Logger.LogPatch("Created Terroristic Execution activatable ability.", Slayer_Terroristic_Execution_Activatable_Ability);

            var Slayer_Gory_Assassination_Activatable_Ability = HlEX.ConvertBuffToActivatableAbility(Universal_Gory_Assassination_Active_Buff, UnitCommand.CommandType.Free, true, "Slayer", "Universal", "ToggleAbility", "ActiveBuff");
            Slayer_Gory_Assassination_Activatable_Ability.TemporaryContext(bp => {
                bp.ActivationType = AbilityActivationType.Immediately;
                bp.m_ActivateOnUnitAction = AbilityActivateOnUnitActionType.Attack;
                bp.SetDescription(ToDContext, "Whenever this ability is activated and an enemy survives an assassinate attempt by the character, it suffers {g|Encyclopedia:Dice}2d6{/g} bleed {g|Encyclopedia:Damage}damage{/g}.");

            });

            ToDContext.Logger.LogPatch("Created Gory Assassination activatable ability.", Slayer_Gory_Assassination_Activatable_Ability);

            Executioner_Assassinate14_Terroristic_Feature.TemporaryContext(bp => {
                bp.AddComponent(HlEX.CreateAddFacts(new BlueprintUnitFactReference[] { Slayer_Terroristic_Execution_Activatable_Ability.ToReference<BlueprintUnitFactReference>() }));
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
                bp.SetDescription(ToDContext, "At 14th level, whenever an executioner kills an enemy with assassinate, all other enemies in a 30 - foot radius become demoralized.");
            });

            ToDContext.Logger.LogPatch("Changed hidden Executioner's Assassinate for 14th level, making visble and renaming Terroristic Execution.", Executioner_Assassinate14_Terroristic_Feature);

            Executioner_Assassinate19_Messy_Feature.TemporaryContext(bp => {
                bp.AddComponent(HlEX.CreateAddFacts(new BlueprintUnitFactReference[] { Slayer_Gory_Assassination_Activatable_Ability.ToReference<BlueprintUnitFactReference>() }));
                bp.HideInUI = false;
                bp.HideInCharacterSheetAndLevelUp = false;
                bp.SetDescription(ToDContext, "At 19th level, whenever an enemy survives an assassinate attempt by an executioner, it suffers {g|Encyclopedia:Dice}2d6{/g} bleed {g|Encyclopedia:Damage}damage{/g}.");
            });
                
            ToDContext.Logger.LogPatch("Changed hidden Executioner's Assassinate for 19th level, making visble and renaming Gory Assassination.", Executioner_Assassinate19_Messy_Feature);

            ToDContext.Logger.LogPatch("Changed Executioner's Assassinate ability to use buffs from activatable abilities instead of features", Executioner_Assassinate_Ability);

            #endregion

            #region |-----------------------------------------| Setup Executioner Assassinate Ability Components |------------------------------------|

            var Executioner_Assassinate_Ability_Any_Context_Value_DC = HlEX.CreateContextValueFromCustomCasterProperty(Universal_Assassinate_Ability_Property);

            UnityModManagerNet.UnityModManager.Logger.Log("Created Executioner Assassinate Ability's Context Value DC.");

            var Executioner_Assassinate_Ability_Any_Context_Value_CL = HlEX.CreateContextValue(0);

            UnityModManagerNet.UnityModManager.Logger.Log("Created Executioner Assassinate Ability's Context Value CL.");

            var Executioner_Assassinate_Ability_Any_Context_Value_Concentration = HlEX.CreateContextValue(0);

            UnityModManagerNet.UnityModManager.Logger.Log("Created Executioner Assassinate Ability's Context Value Concentration.");

            var Executioner_Assassinate_Ability_Any_Context_Value_Spell_Level = HlEX.CreateContextValue(0);

            UnityModManagerNet.UnityModManager.Logger.Log("Created Executioner Assassinate Ability's Context Value Spell Level.");

            ToDContext.Logger.LogPatch("Changed Executioner Assassinate ability to take Custom Properties.", Executioner_Assassinate_Ability);

            #endregion

            #region |---------------------------------------------| Create New Components for Executioner's Assassinate |---------------------------------------------------|

            var New_Assassinate_Component_Array = new BlueprintComponent[0];

            // Component 0

            var Executioner_Assassinate_Ability_Has_Item_In_Hands = HlEX.CreateAbilityRequirementHasItemInHands(AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon);

            var Executioner_Assassinate_Ability_Caster_Has_Fact_Gory_Assassination = HlEX.CreateContextConditionCasterHasFact(Universal_Gory_Assassination_Active_Buff, true);
            var Executioner_Assassinate_Ability_Caster_Has_Fact_Terroristic_Execution = HlEX.CreateContextConditionCasterHasFact(Universal_Terroristic_Execution_Active_Buff, true);
            
            var Executioner_Assassinate_Ability_Apply_Bleed = HlEX.CreateContextActionApplyBuff(Executioner_Assassinate_Messy_Feature_Bleed_Buff, HlEX.CreateContextDuration(HlEX.CreateContextValue(0), DurationRate.Rounds, DiceType.Zero, HlEX.CreateContextValue(0)), false, true, false, false, true);
            var Executioner_Assassinate_Ability_Cast_Spell_Demoralize = HlEX.CreateContextActionCastSpell(Executioner_Assassinate_Terroristic_Demoralize_Ability, false, false, false, false, false, HlEX.CreateContextValue(0), HlEX.CreateContextValue(0));

            var Executioner_Assassinate_Ability_Conditional_Gory_Assassination = HlEX.CreateConditional(Executioner_Assassinate_Ability_Caster_Has_Fact_Gory_Assassination, Executioner_Assassinate_Ability_Apply_Bleed, null);
            var Executioner_Assassinate_Ability_Conditional_Terroristic_Execution = HlEX.CreateConditional(Executioner_Assassinate_Ability_Caster_Has_Fact_Terroristic_Execution, Executioner_Assassinate_Ability_Cast_Spell_Demoralize, null);

            var Executioner_Assassinate_Ability_Conditional_Saved = HlEX.CreateContextActionConditionalSaved(new GameAction[] { Executioner_Assassinate_Ability_Conditional_Gory_Assassination }, new GameAction[] { HlEX.CreateContextActionKill(UnitState.DismemberType.None), Executioner_Assassinate_Ability_Conditional_Terroristic_Execution });

            UnityModManagerNet.UnityModManager.Logger.Log("Created Executioner Assassinate Ability's Conditional Saved.");

            var Executioner_Assassinate_Ability_Saving_Throw = HlEX.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(Executioner_Assassinate_Ability_Conditional_Saved));

            UnityModManagerNet.UnityModManager.Logger.Log("Created Executioner Assassinate Ability's Context Action Saving Throw.");

            var Executioner_Assassinate_Ability_Melee_Attack = HlEX.CreateContextActionMeleeAttack(true, true, true, false, false, false, false);

            UnityModManagerNet.UnityModManager.Logger.Log("Created Executioner Assassinate Ability's Context Action Melee Attack.");

            var Executioner_Assassinate_Ability_Provoke_Attack_Of_Opportunity = HlEX.CreateContextActionProvokeAttackOfOpportunity(true);

            UnityModManagerNet.UnityModManager.Logger.Log("Created Executioner Assassinate Ability's Context Action Provoke Attack of Opportunity.");

            // Component 1

            var Executioner_Assassinate_Ability_Run_Actions = HlEX.CreateRunActions(Executioner_Assassinate_Ability_Melee_Attack, Executioner_Assassinate_Ability_Saving_Throw, Executioner_Assassinate_Ability_Provoke_Attack_Of_Opportunity);

            UnityModManagerNet.UnityModManager.Logger.Log("Created Executioner Assassinate Ability's Ability Run Actions.");

            var Assassination_Immunity_Facts = new BlueprintUnitFactReference[]
                                               {
                                                    Immunity_To_Critical.ToReference<BlueprintUnitFactReference>(),
                                                    Knowledge_Domain_Base_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Wild_Shape_Elemental_Air_Huge_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Wild_Shape_Elemental_Air_Large_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Wild_Shape_Elemental_Earth_Huge_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Wild_Shape_Elemental_Earth_Large_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Wild_Shape_Elemental_Fire_Huge_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Wild_Shape_Elemental_Fire_Large_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Wild_Shape_Elemental_Water_Huge_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Wild_Shape_Elemental_Water_Large_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Bloodline_Elemental_Air_Elemental_Body_Feature.ToReference<BlueprintUnitFactReference>(),
                                                    Bloodline_Elemental_Earth_Elemental_Body_Feature.ToReference<BlueprintUnitFactReference>(),
                                                    Bloodline_Elemental_Fire_Elemental_Body_Feature.ToReference<BlueprintUnitFactReference>(),
                                                    Bloodline_Elemental_Water_Elemental_Body_Feature.ToReference<BlueprintUnitFactReference>(),
                                                    Subtype_Elemental.ToReference<BlueprintUnitFactReference>(),
                                                    Robe_Of_False_Death_Feature.ToReference<BlueprintUnitFactReference>(),
                                                    Evasive_Round_Helmet_Feature.ToReference<BlueprintUnitFactReference>(),
                                                    Helmet_Armag_Feature.ToReference<BlueprintUnitFactReference>(),
                                                    Forewarning_Shield_Feature.ToReference<BlueprintUnitFactReference>(),
                                                    Elemental_Body_III_Air_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Elemental_Body_III_Earth_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Elemental_Body_III_Fire_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Elemental_Body_III_Water_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Elemental_Body_IV_Air_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Elemental_Body_IV_Earth_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Elemental_Body_IV_Fire_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Elemental_Body_IV_Water_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Polymorph_Greater_Elemental_Air_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Polymorph_Greater_Elemental_Earth_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Polymorph_Greater_Elemental_Fire_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Polymorph_Greater_Elemental_Water_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Fiery_Body_Buff.ToReference<BlueprintUnitFactReference>(),
                                                    Mimic_Immunes_Feature.ToReference<BlueprintUnitFactReference>(),
                                                    Swarm_Diminutive_Feature.ToReference<BlueprintUnitFactReference>(),
                                                    Swarm_Tiny_Feature.ToReference<BlueprintUnitFactReference>()

                                               };

            // Component 2

            var Executioner_Assassinate_Ability_Target_Has_Not_Facts = HlEX.CreateAbilityTargetHasFact(Assassination_Immunity_Facts, true);

            UnityModManagerNet.UnityModManager.Logger.Log("Created Executioner Assassinate Ability's Ability Target Has Fact.");

            // Component 3

            var Executioner_Assassinate_Ability_Any_Context_Set_Ability_Params = HlEX.CreateContextSetAbilityParams(Executioner_Assassinate_Ability_Any_Context_Value_DC, Executioner_Assassinate_Ability_Any_Context_Value_CL, Executioner_Assassinate_Ability_Any_Context_Value_Concentration, Executioner_Assassinate_Ability_Any_Context_Value_Spell_Level, true);

            UnityModManagerNet.UnityModManager.Logger.Log("Created Executioner Assassinate Ability's Set Ability Params.");

            // Component 4

            var Executioner_Assassinate_Dismemberment = HlEX.CreateInPowerDismemberComponent();


            UnityModManagerNet.UnityModManager.Logger.Log("Created Executioner Assassinate Ability's In Power Dismemberment.");

            // Component 5

            var Executioner_Assassinate_Target_Cannot_See = HlEX.CreateAbilityTargetCanSeeCaster(true);

            UnityModManagerNet.UnityModManager.Logger.Log("Created Executioner Assassinate Ability's Cannot See The Target.");

            // Component 6

            var Executioner_Assassinate_Has_Condition_Or_Buff = HlEX.CreateAbilityTargetHasConditionOrBuff(Slayer_Study_Target_Buff);

            UnityModManagerNet.UnityModManager.Logger.Log("Created Executioner Assassinate Ability's Has Condition or Buffs.");

            // Component 7

            var Executioner_Assassinate_Out_Of_Combat = HlEX.CreateAbilityCasterInCombat(true);

            UnityModManagerNet.UnityModManager.Logger.Log("Created Executioner Assassinate Ability's Out of Combat.");


            #endregion

            #region |----------------------------------------------| Clean Assassinate Ability and Add Components  |---------------------------------------------------|

            Assassin_Death_Attack_Ability.ComponentsArray = New_Assassinate_Component_Array;

            Assassin_Death_Attack_Ability.TemporaryContext(ab => {
                ab.AddComponent(Executioner_Assassinate_Ability_Has_Item_In_Hands);
                ab.AddComponent(Executioner_Assassinate_Ability_Run_Actions);
                ab.AddComponent(Executioner_Assassinate_Ability_Target_Has_Not_Facts);
                ab.AddComponent(Executioner_Assassinate_Ability_Any_Context_Set_Ability_Params);
                ab.AddComponent(Executioner_Assassinate_Dismemberment);
                ab.AddComponent(Executioner_Assassinate_Target_Cannot_See);
                ab.AddComponent(Executioner_Assassinate_Has_Condition_Or_Buff);
                ab.AddComponent(Executioner_Assassinate_Out_Of_Combat);
            });


            #endregion

            #region |------------------------------------------| Create Assassination Training Progressions for Rogue and Slayer |---------------------------------------------|

            var SlayerAssassinationTrainingProgression = Helpers.CreateBlueprint<BlueprintProgression>(ToDContext, "SlayerAssassinationTrainingProgression", bp => {
                bp.SetName(ToDContext, "Slayer Assassination Training");
                bp.SetDescription(ToDContext, "The character uses his slayer level for purposes of assassinate ability and treats it as assassin level for purposes of death attack.");
                bp.m_Icon = AssassinationStatIcon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.ReapplyOnLevelUp = true;
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
                bp.LevelEntries = Enumerable.Range(1, 20)
                    .Select(i => new LevelEntry
                    {
                        Level = i,
                        m_Features = new List<BlueprintFeatureBaseReference> {
                            Assassination_Training_FakeLevel.ToReference<BlueprintFeatureBaseReference>()
                        },
                    })
                    .ToArray();
                bp.AddClass(ClassTools.ClassReferences.SlayerClass);
                bp.m_Classes[0].AdditionalLevel = 0;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Created Slayer Assassination Training.", SlayerAssassinationTrainingProgression);

            var RogueAssassinationTrainingProgression = Helpers.CreateBlueprint<BlueprintProgression>(ToDContext, "RogueAssassinationTrainingProgression", bp => {
                bp.SetName(ToDContext, "Rogue Assassination Training");
                bp.SetDescription(ToDContext, "The character uses his rogue level as slayer level for purposes of assassinate ability and treats it as assassin level for purposes of death attack.");
                bp.m_Icon = AssassinationStatIcon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.ReapplyOnLevelUp = true;
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
                bp.LevelEntries = Enumerable.Range(1, 20)
                    .Select(i => new LevelEntry
                    {
                        Level = i,
                        m_Features = new List<BlueprintFeatureBaseReference> {
                            Assassination_Training_FakeLevel.ToReference<BlueprintFeatureBaseReference>()
                        },
                    })
                    .ToArray();
                bp.AddClass(ClassTools.ClassReferences.RogueClass);
                bp.m_Classes[0].AdditionalLevel = 0;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Created Rogue Assassination Training.", RogueAssassinationTrainingProgression);

            #endregion

            #region |---------------------------------------------| Change Executioner's Assassinate Ability's Description |---------------------------------------------------|



            Executioner_Assassinate_Ability.SetDescription(ToDContext, "The slayer can target studied opponents in attempt to instantly kill them. This ability can only be used out of combat and the target must not see the slayer, but this special {g|Encyclopedia:Attack}attack{/g} automatically hits, scores a {g|Encyclopedia:Critical}critical hit{/g} and, if the victim survives, must make a {g|Encyclopedia:Saving_Throw}Fortitude save{/g} ({g|Encyclopedia:DC}DC{/g} 10 + slayer level + relevant stat modifier or die. \n This special {g|Encyclopedia:Attack}attack{/g} is a full-{g|Encyclopedia:Combat_Round}round{/g} {g|Encyclopedia:CA_Types}action{/g} that provokes {g|Encyclopedia:Attack_Of_Opportunity}attacks of opportunity{/g} from {g|Encyclopedia:Threatened_Area}threatening{/g} opponents.");
            Executioner_Assassinate_Ability.m_Icon = ExecutionerAssassinateNewIcon;
            Executioner_Assassinate_Ability.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.CoupDeGrace;

            ToDContext.Logger.LogPatch("Changed Executioner's Assassinate ability's description.", Executioner_Assassinate_Ability);

            Executioner_Assassinate_Feature.m_Icon = ExecutionerAssassinateNewIcon;

            ToDContext.Logger.LogPatch("Changed Executioner's Assassinate feature's icon.", Executioner_Assassinate_Feature);

            #endregion

            #region |------------------------------------------------| Add Fake Assassin Levels to Executioner's Assassinate Feature |-------------------------------------------------|

            Executioner_Assassinate_Feature.AddComponent(Helpers.Create<AddFeatureOnApply>(c => {
                c.m_Feature = SlayerAssassinationTrainingProgression.ToReference<BlueprintFeatureReference>();
            }));

            #endregion

            #region |------------------------------------------------------------| Add Assassinate Stat to Slayer |-----------------------------------------------------------|

            Executioner_Assassinate_Feature.AddComponent(HlEX.CreateAddFacts(new BlueprintUnitFactReference[] { Assassination_Intelligence_Feature.ToReference<BlueprintUnitFactReference>() }));

            ToDContext.Logger.LogPatch("Added Assassination - Intelligence to Executioner's Assassinate feature.", Executioner_Assassinate_Feature);

            #endregion


            #region |-----------------------------------------------------| Create Assassinate Custom Property |-------------------------------------------------------|


            #endregion

        }

    }
}
