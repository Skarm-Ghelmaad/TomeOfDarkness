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

namespace TomeOfDarkness.MechanicsChanges
{
    internal class UniversalAssassination
    {

        public static void ConfigureUniversalAssassination()
        {

            AssassinationTraining.ConfigureAssassinationTraining();

            var Assassination_Training_FakeLevel = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "AssassinationTrainingFakeLevel");
            var Assassination_Training_Property = BlueprintTools.GetModBlueprint<BlueprintUnitProperty>(ToDContext, "AssassinationTrainingProperty");

            #region |----------------------------------------------------------| Create Generic Assassinte Stat |-------------------------------------------------------------|

            var AssassinationStatIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_AssassinationTraining.png");

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

            var Universal_Assassination_Mortality_Boost_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalAssassinationMortalityBoostBluff", bp => {
                bp.SetName(ToDContext, "Assassination - Mortality Boost");
                bp.SetDescription(ToDContext, "Each rank of this feature adds +1 to the Death Attack's and Assassination's DC.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
            });

            ToDContext.Logger.LogPatch("Added Assassination Mortality Boost buff.", Universal_Assassination_Mortality_Boost_Buff);

            var Universal_Assassination_Mortality_Reduction_Feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "UniversalAssassinationMortalityReductionFeature", bp => {
                bp.SetName(ToDContext, "Assassination - Mortality Reduction");
                bp.SetDescription(ToDContext, "Each rank of this feature adds -1 to the Death Attack's and Assassination's DC.");
                bp.Ranks = 100;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            ToDContext.Logger.LogPatch("Added Assassination Mortality Reduction feature.", Universal_Assassination_Mortality_Reduction_Feature);

            var Universal_Assassination_Mortality_Reduction_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalAssassinationMortalityBoostBluff", bp => {
                bp.SetName(ToDContext, "Assassination - Mortality Reduction");
                bp.SetDescription(ToDContext, "Each rank of this buff adds -1 to the Death Attack's and Assassination's DC.");
                bp.Ranks = 100;
                bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
            });

            ToDContext.Logger.LogPatch("Added Assassination Mortality Reduction feature.", Universal_Assassination_Mortality_Reduction_Buff);

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
                            Property = HlEX.CreateFactRankGetter(Universal_Assassination_Mortality_Reduction_Feature.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            },
                       new CompositeCustomPropertyGetter.ComplexCustomProperty(){
                            Property = HlEX.CreateFactRankGetter(Universal_Assassination_Mortality_Reduction_Buff.ToReference<BlueprintUnitFactReference>()),
                            Numerator = -1,
                            Denominator = 1
                            }
                        };
                });
                bp.BaseValue = 0;
            });

            ToDContext.Logger.LogPatch("Added Assassination Mortality property.", Assassination_Mortality_Adjustment_Property);

            #endregion

            #region |-------------------------------------------------| Change Assassin Death Attack to Use Custom Property |--------------------------------------------------|

            var Assassin_Death_Attack_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("d86609703c0b37445828a23140385721");
            var Assassin_Death_Attack_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("744e7c7fd58bd6040b40210cf0864692");
            var Assassin_Death_Attack_Ability_Kill = BlueprintTools.GetBlueprint<BlueprintAbility>("ca5575accdf8ee64cb32608a77aaf989");
            var Assassin_Death_Attack_Ability_Paralyze = BlueprintTools.GetBlueprint<BlueprintAbility>("452b64ffab80cff40bd27dc5f350d80c");

            var Assassin_Death_Attack_Standard_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("b2bcc7efc9d59af42836bc5ba9e1a5e0");
            var Assassin_Death_Attack_Standard_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("68a6086913b7cca4283c62be2295ce81");
            var Assassin_Death_Attack_Ability_Kill_Standard = BlueprintTools.GetBlueprint<BlueprintAbility>("02d129b799da92d40b6377bac27d843f");
            var Assassin_Death_Attack_Ability_Paralyze_Standard = BlueprintTools.GetBlueprint<BlueprintAbility>("614422dc5cdd3eb4ebf0f9900dd0e0ab");

            var Assassin_Death_Attack_Ability_Kill_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("3740fdf85036ec34bbd0b09f218a9cce");
            var Assassin_Death_Attack_Ability_Paralyze_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("b6ebb41266c137c4384b4b279a7f631c");

            var Assassin_Death_Attack_Ability_Property = BlueprintTools.GetBlueprint<BlueprintUnitProperty>("857ddbe8d4a742c49a933b893653649f");

            var Assassin_Death_Attack_Ability_Kill_Effect_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("fa7cf97ea4dfc4d4aa600e18fd7d419b");
            var Assassin_Death_Attack_Ability_Paralyze_Effect_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("b6ebb41266c137c4384b4b279a7f631c");

            Assassin_Death_Attack_Ability_Property.RemoveComponents<ClassLevelGetter>();
            Assassin_Death_Attack_Ability_Property.RemoveComponents<SimplePropertyGetter>();
            Assassin_Death_Attack_Ability_Property.AddComponent(HlEX.CreateCustomPropertyGetter(Assassination_Training_Property));
            Assassin_Death_Attack_Ability_Property.AddComponent(HlEX.CreateCustomPropertyGetter(AssassinationStatProperty));

            ToDContext.Logger.LogPatch("Changed Death Attack property to use custom properties.", Assassin_Death_Attack_Ability_Property);

            var dth_atk_ab_kil_comp = Assassin_Death_Attack_Ability_Kill.GetComponents<ContextRankConfig>().ToArray();
            var dth_atk_ab_par_comp = Assassin_Death_Attack_Ability_Paralyze.GetComponents<ContextRankConfig>().ToArray();
            var dth_atk_ab_kil_std_comp = Assassin_Death_Attack_Ability_Kill_Standard.GetComponents<ContextRankConfig>().ToArray();
            var dth_atk_ab_par_std_comp = Assassin_Death_Attack_Ability_Paralyze_Standard.GetComponents<ContextRankConfig>().ToArray();

            foreach (var cmp in dth_atk_ab_kil_comp)
            {
                if (cmp.m_BaseValueType == ContextRankBaseValueType.ClassLevel)
                {
                    cmp.TemporaryContext(c => {
                        c.m_Class = new BlueprintCharacterClassReference[0];
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = Assassination_Training_Property.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
                else
                {
                    cmp.TemporaryContext(c => {
                        c.m_Stat = StatType.Unknown;
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = AssassinationStatProperty.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
            }

            ToDContext.Logger.LogPatch("Changed Death Attack - Kill to use custom properties.", Assassin_Death_Attack_Ability_Kill);

            foreach (var cmp in dth_atk_ab_par_comp)
            {
                if (cmp.m_BaseValueType == ContextRankBaseValueType.ClassLevel)
                {
                    cmp.TemporaryContext(c => {
                        c.m_Class = new BlueprintCharacterClassReference[0];
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = Assassination_Training_Property.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
                else
                {
                    cmp.TemporaryContext(c => {
                        c.m_Stat = StatType.Unknown;
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = AssassinationStatProperty.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
            }

            ToDContext.Logger.LogPatch("Changed Death Attack - Paralyze to use custom properties.", Assassin_Death_Attack_Ability_Paralyze);

            foreach (var cmp in dth_atk_ab_kil_std_comp)
            {
                if (cmp.m_BaseValueType == ContextRankBaseValueType.ClassLevel)
                {
                    cmp.TemporaryContext(c => {
                        c.m_Class = new BlueprintCharacterClassReference[0];
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = Assassination_Training_Property.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
                else
                {
                    cmp.TemporaryContext(c => {
                        c.m_Stat = StatType.Unknown;
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = AssassinationStatProperty.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
            }

            ToDContext.Logger.LogPatch("Changed Death Attack - Kill (Standard) to use custom properties.", Assassin_Death_Attack_Ability_Kill_Standard);

            foreach (var cmp in dth_atk_ab_par_std_comp)
            {
                if (cmp.m_BaseValueType == ContextRankBaseValueType.ClassLevel)
                {
                    cmp.TemporaryContext(c => {
                        c.m_Class = new BlueprintCharacterClassReference[0];
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = Assassination_Training_Property.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
                else
                {
                    cmp.TemporaryContext(c => {
                        c.m_Stat = StatType.Unknown;
                        c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                        c.m_CustomProperty = AssassinationStatProperty.ToReference<BlueprintUnitPropertyReference>();
                    });
                }
            }

            ToDContext.Logger.LogPatch("Changed Death Attack - Paralyze (Standard) to use custom properties.", Assassin_Death_Attack_Ability_Paralyze_Standard);

            var dth_atk_kil_buf_comp = Assassin_Death_Attack_Ability_Kill_Buff.GetComponents<ContextRankConfig>().FirstOrDefault();
            var dth_atk_par_buf_comp = Assassin_Death_Attack_Ability_Paralyze_Buff.GetComponents<ContextRankConfig>().FirstOrDefault();

            dth_atk_kil_buf_comp.TemporaryContext(c => {
                c.m_Class = new BlueprintCharacterClassReference[0];
                c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                c.m_CustomProperty = Assassination_Training_Property.ToReference<BlueprintUnitPropertyReference>();
            });

            ToDContext.Logger.LogPatch("Changed Death Attack - Kill buff to use custom properties.", Assassin_Death_Attack_Ability_Kill_Buff);

            dth_atk_par_buf_comp.TemporaryContext(c => {
                c.m_Class = new BlueprintCharacterClassReference[0];
                c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                c.m_CustomProperty = Assassination_Training_Property.ToReference<BlueprintUnitPropertyReference>();
            });

            ToDContext.Logger.LogPatch("Changed Death Attack - Paralyze buff to use custom properties.", Assassin_Death_Attack_Ability_Paralyze_Buff);

            var dth_atk_kil_eff_buf_comp = Assassin_Death_Attack_Ability_Kill_Effect_Buff.GetComponents<ContextRankConfig>().FirstOrDefault();

            dth_atk_kil_eff_buf_comp.TemporaryContext(c => {
                c.m_Class = new BlueprintCharacterClassReference[0];
                c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                c.m_CustomProperty = Assassination_Training_Property.ToReference<BlueprintUnitPropertyReference>();
            });

            ToDContext.Logger.LogPatch("Changed Death Attack - Kill effect buff to use custom properties.", Assassin_Death_Attack_Ability_Kill_Effect_Buff);

            var dth_atk_par_eff_buf_comp = Assassin_Death_Attack_Ability_Paralyze_Effect_Buff.GetComponents<ContextRankConfig>().FirstOrDefault();

            dth_atk_par_eff_buf_comp.TemporaryContext(c => {
                c.m_Class = new BlueprintCharacterClassReference[0];
                c.m_BaseValueType = ContextRankBaseValueType.CustomProperty;
                c.m_CustomProperty = Assassination_Training_Property.ToReference<BlueprintUnitPropertyReference>();
            });

            ToDContext.Logger.LogPatch("Changed Death Attack - Paralyze effect buff to use custom properties.", Assassin_Death_Attack_Ability_Paralyze_Effect_Buff);


            #endregion

            #region |-----------------------------------------------------| Create Assassinate Custom Property |-------------------------------------------------------|

            // This property is a perfect clone of the Assassin Death Attack Ability copy.

            var Universal_Assassinate_Ability_Property = Assassin_Death_Attack_Ability_Property.CreateCopy(ToDContext, "UniversalAssassinateAbilityProperty");

            ToDContext.Logger.LogPatch("Added Assassination Mortality property.", Universal_Assassinate_Ability_Property);

            #endregion

            #region |-----------------------------------------------| Change Executioner's Assassination to Become Universal |-------------------------------------------------|


            var Executioner_Assassinate_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("bd7e70e98f9036f4ba27ef3e29a177c2");
            var ExecutionerAssassinateOldIcon = Executioner_Assassinate_Feature.m_Icon;
            var UniversalGoryAssassinationIcon = BlueprintTools.GetBlueprint<BlueprintAbility>("c3d2294a6740bc147870fff652f3ced5").m_Icon; // Death Clutch icon 
            var Executioner_Assassinate_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("3dad7f131aa884f4c972f2fb759d0df4");
            var Executioner_Assassinate19_Messy_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("c81f15dbbadbb3a48b37e95a5a7ef759");
            var Executioner_Assassinate14_Terroristic_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("c81f15dbbadbb3a48b37e95a5a7ef759");

            var Main_Weapon_Damage_Stat_Bonus_Property = BlueprintTools.GetBlueprint<BlueprintUnitProperty>("9955f9c72c350254daff5a029ee32712");
            var Main_Weapon_Critical_Modifier_Property = BlueprintTools.GetBlueprint<BlueprintUnitProperty>("6ac8613eca0083d438b48f9e1391f09b");

            var Universal_Terroristic_Execution_Active_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalTerroristicExecutionActiveBuff", bp => {
                bp.SetName(ToDContext, "Terroristic Execution");
                bp.SetDescription(ToDContext, "Whenever the character kills an enemy with assassinate, all other enemies in a 30-foot radius become demoralized.");
                bp.m_Icon = ExecutionerAssassinateOldIcon;
                bp.FxOnStart = new PrefabLink();
                bp.FxOnRemove = new PrefabLink();
            });

            ToDContext.Logger.LogPatch("Created Terroristic Execution buff.", Universal_Terroristic_Execution_Active_Buff);

            var Universal_Gory_Assassination_Active_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "UniversalGoryAssassinationActiveBuff", bp => {
                bp.SetName(ToDContext, "Gory Assassination");
                bp.SetDescription(ToDContext, "Whenever an enemy survives an assassinate attempt by the character, it suffers {g|Encyclopedia:Dice}2d6{/g} bleed {g|Encyclopedia:Damage}damage{/g}.");
                bp.m_Icon = UniversalGoryAssassinationIcon;
                bp.FxOnStart = new PrefabLink();
                bp.FxOnRemove = new PrefabLink();
            });

            ToDContext.Logger.LogPatch("Created Gory Assassination buff.", Universal_Gory_Assassination_Active_Buff);

            var Slayer_Terroristic_Execution_Activatable_Ability = HlEX.ConvertBuffToActivatableAbility(Universal_Terroristic_Execution_Active_Buff, UnitCommand.CommandType.Free, true, "Slayer", "Universal", "ToggleAbility", "ActiveBuff");
            Slayer_Terroristic_Execution_Activatable_Ability.ActivationType = AbilityActivationType.Immediately;
            Slayer_Terroristic_Execution_Activatable_Ability.m_ActivateOnUnitAction = AbilityActivateOnUnitActionType.Attack;
            Slayer_Terroristic_Execution_Activatable_Ability.SetDescription(ToDContext, "Whenever this ability is activated and the character kills an enemy with assassinate, all other enemies in a 30-foot radius become demoralized.");

            ToDContext.Logger.LogPatch("Created Terroristic Execution activatable ability.", Slayer_Terroristic_Execution_Activatable_Ability);

            var Slayer_Gory_Assassination_Activatable_Ability = HlEX.ConvertBuffToActivatableAbility(Universal_Gory_Assassination_Active_Buff, UnitCommand.CommandType.Free, true, "Slayer", "Universal", "ToggleAbility", "ActiveBuff");
            Slayer_Gory_Assassination_Activatable_Ability.ActivationType = AbilityActivationType.Immediately;
            Slayer_Gory_Assassination_Activatable_Ability.m_ActivateOnUnitAction = AbilityActivateOnUnitActionType.Attack;
            Slayer_Gory_Assassination_Activatable_Ability.SetDescription(ToDContext, "Whenever this ability is activated and an enemy survives an assassinate attempt by the character, it suffers {g|Encyclopedia:Dice}2d6{/g} bleed {g|Encyclopedia:Damage}damage{/g}.");

            ToDContext.Logger.LogPatch("Created Gory Assassination activatable ability.", Slayer_Gory_Assassination_Activatable_Ability);

            Executioner_Assassinate14_Terroristic_Feature.AddComponent(HlEX.CreateAddFacts(new BlueprintUnitFactReference[] { Slayer_Terroristic_Execution_Activatable_Ability.ToReference<BlueprintUnitFactReference>() }));
            Executioner_Assassinate14_Terroristic_Feature.HideInUI = false;
            Executioner_Assassinate14_Terroristic_Feature.HideInCharacterSheetAndLevelUp = false;
            Executioner_Assassinate14_Terroristic_Feature.SetDescription(ToDContext, "At 14th level, whenever an executioner kills an enemy with assassinate, all other enemies in a 30 - foot radius become demoralized.");

            ToDContext.Logger.LogPatch("Changed hidden Executioner's Assassinate for 14th level, making visble and renaming Terroristic Execution.", Executioner_Assassinate14_Terroristic_Feature);

            Executioner_Assassinate19_Messy_Feature.AddComponent(HlEX.CreateAddFacts(new BlueprintUnitFactReference[] { Slayer_Gory_Assassination_Activatable_Ability.ToReference<BlueprintUnitFactReference>()}));
            Executioner_Assassinate19_Messy_Feature.HideInUI = false;
            Executioner_Assassinate19_Messy_Feature.HideInCharacterSheetAndLevelUp = false;
            Executioner_Assassinate19_Messy_Feature.SetDescription(ToDContext, "At 19th level, whenever an enemy survives an assassinate attempt by an executioner, it suffers {g|Encyclopedia:Dice}2d6{/g} bleed {g|Encyclopedia:Damage}damage{/g}.");

            ToDContext.Logger.LogPatch("Changed hidden Executioner's Assassinate for 19th level, making visble and renaming Gory Assassination.", Executioner_Assassinate19_Messy_Feature);

            var Executioner_Assassinate_Ability_Conditionals = Executioner_Assassinate_Ability.FlattenAllActions().OfType<Conditional>().ToArray();

            foreach (var conditional in Executioner_Assassinate_Ability_Conditionals)
            {

                var caster_has_fact_conds = conditional.ConditionsChecker.Conditions.OfType<ContextConditionCasterHasFact>().ToArray();

                foreach (var cond in caster_has_fact_conds)
                {
                    if (cond.m_Fact == Executioner_Assassinate14_Terroristic_Feature.ToReference<BlueprintUnitFactReference>())
                    {
                        cond.TemporaryContext(c => {
                            c.m_Fact = Universal_Terroristic_Execution_Active_Buff.ToReference<BlueprintUnitFactReference>();
                        });
                    }
                    else if (cond.m_Fact == Executioner_Assassinate19_Messy_Feature.ToReference<BlueprintUnitFactReference>())
                    {
                        cond.TemporaryContext(c => {
                            c.m_Fact = Universal_Gory_Assassination_Active_Buff.ToReference<BlueprintUnitFactReference>();
                        });
                    }
                }

            }

            ToDContext.Logger.LogPatch("Changed Executioner's Assassinate ability to use buffs from activatable abilities instead of features", Executioner_Assassinate_Ability);


            //var Executioner_Assassinate_Ability_Conf_Rnk_Conf_Comps = Executioner_Assassinate_Ability.GetComponents<ContextRankConfig>().ToArray();

            Executioner_Assassinate_Ability.GetComponent<ContextSetAbilityParams>().DC.TemporaryContext(c => {
                c.ValueType = ContextValueType.CasterCustomProperty;
                c.m_CustomProperty = Universal_Assassinate_Ability_Property.ToReference<BlueprintUnitPropertyReference>();
            });

            var Executioner_Assassinate_Ability_Context_Save = Executioner_Assassinate_Ability.FlattenAllActions().OfType<ContextActionSavingThrow>().FirstOrDefault();

            var Executioner_Assassinate_Ability_Conditional_DC_Increase = new ContextActionSavingThrow.ConditionalDCIncrease();

            Executioner_Assassinate_Ability_Conditional_DC_Increase.Condition = HlEX.CreateConditionsCheckerAnd(new Condition[] { HlEX.CreateConditionTrue() });
            Executioner_Assassinate_Ability_Conditional_DC_Increase.Value = new ContextValue()
                                                                            {
                                                                                ValueType = ContextValueType.CasterCustomProperty,
                                                                                m_CustomProperty = Assassination_Mortality_Adjustment_Property.ToReference<BlueprintUnitPropertyReference>()
                                                                            };


            Executioner_Assassinate_Ability_Context_Save.TemporaryContext(c => {
                c.m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[] { Executioner_Assassinate_Ability_Conditional_DC_Increase };
            });

            Executioner_Assassinate_Ability.RemoveComponents<ContextRankConfig>();

            ToDContext.Logger.LogPatch("Changed Executioner's Assassinate ability to remove ContextRankConfig and use custom properties.", Executioner_Assassinate_Ability);


            #endregion

            #region |-----------------------------------------------| Change Assassin's Death Attack to include Mortality Adj. |-----------------------------------------------|

            var Death_Attack_Ability_Conditional_DC_Increase = new ContextActionSavingThrow.ConditionalDCIncrease();

            Death_Attack_Ability_Conditional_DC_Increase.Condition = HlEX.CreateConditionsCheckerAnd(new Condition[] {HlEX.CreateConditionTrue()});
            Death_Attack_Ability_Conditional_DC_Increase.Value = new ContextValue()
            {
                ValueType = ContextValueType.CasterCustomProperty,
                m_CustomProperty = Assassination_Mortality_Adjustment_Property.ToReference<BlueprintUnitPropertyReference>()
            };

            var Assassin_Death_Attack_Ability_Kill_Effect_Buff_Context_Save = Assassin_Death_Attack_Ability_Kill_Effect_Buff.FlattenAllActions().OfType<ContextActionSavingThrow>().FirstOrDefault();

            Assassin_Death_Attack_Ability_Kill_Effect_Buff_Context_Save.TemporaryContext(c => {
                c.m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[] { Executioner_Assassinate_Ability_Conditional_DC_Increase };
            });

            ToDContext.Logger.LogPatch("Changed Assassin's Death Attack Kill effect buff to have a custom conditional DC.", Assassin_Death_Attack_Ability_Kill_Effect_Buff);


            var Assassin_Death_Attack_Ability_Paralyze_Effect_Buff_Context_Save = Assassin_Death_Attack_Ability_Paralyze_Effect_Buff.FlattenAllActions().OfType<ContextActionSavingThrow>().FirstOrDefault();

            Assassin_Death_Attack_Ability_Paralyze_Effect_Buff_Context_Save.TemporaryContext(c => {
                c.m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[] { Executioner_Assassinate_Ability_Conditional_DC_Increase };
            });

            ToDContext.Logger.LogPatch("Changed Assassin's Death Attack Paralyze effect buff to have a custom conditional DC.", Assassin_Death_Attack_Ability_Paralyze_Effect_Buff);

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


            var ExecutionerAssassinateNewIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_AssassinateSlayer.png");


            Executioner_Assassinate_Ability.SetDescription(ToDContext, "The slayer can target studied opponents in attempt to instantly kill them. This ability can only be used out of combat and the target must not see the slayer, but this special {g|Encyclopedia:Attack}attack{/g} automatically hits, scores a {g|Encyclopedia:Critical}critical hit{/g} and, if the victim survives, must make a {g|Encyclopedia:Saving_Throw}Fortitude save{/g} ({g|Encyclopedia:DC}DC{/g} 10 + slayer level + relevant stat modifier or die. \n This special {g|Encyclopedia:Attack}attack{/g} is a full-{g|Encyclopedia:Combat_Round}round{/g} {g|Encyclopedia:CA_Types}action{/g} that provokes {g|Encyclopedia:Attack_Of_Opportunity}attacks of opportunity{/g} from {g|Encyclopedia:Threatened_Area}threatening{/g} opponents.");
            Executioner_Assassinate_Ability.m_Icon = ExecutionerAssassinateNewIcon;
            Executioner_Assassinate_Feature.m_Icon = ExecutionerAssassinateNewIcon;
            Executioner_Assassinate_Ability.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.CoupDeGrace;

            ToDContext.Logger.LogPatch("Changed Executioner's Assassinate ability's description.", Executioner_Assassinate_Ability);
            ToDContext.Logger.LogPatch("Changed Executioner's Assassinate feature's description.", Executioner_Assassinate_Feature);

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

            //#region |-------------------------------------------------------| Change Assassin Poison Use Descriptions |--------------------------------------------------------|
            //#endregion

        }

    }
}
