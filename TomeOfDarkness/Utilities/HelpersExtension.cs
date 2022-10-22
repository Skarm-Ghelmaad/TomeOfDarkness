using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Kingmaker.View;
using Kingmaker.ResourceLinks;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;
using TomeOfDarkness.NewComponents;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using static TomeOfDarkness.Main;
using System.Web;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.ElementsSystem;
using Kingmaker.Designers.EventConditionActionSystem.ContextData;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;

namespace TomeOfDarkness.Utilities
{
    public static class HelpersExtension
    {

        #region |----------------------------------------------------------------| ResourceLinks Creators |----------------------------------------------------------------|

              

        public static PrefabLink CreatePrefabLink(string asset_id) // Holic75_SC 
        {
            var link = new PrefabLink();
            link.AssetId = asset_id;
            return link;
        }

        public static ProjectileLink CreateProjectileLink(string asset_id) 
        {
            var link = new ProjectileLink();
            link.AssetId = asset_id;
            return link;
        }

        public static UnitViewLink CreateUnitViewLink(string asset_id) // Holic75_SC 
        {
            var link = new UnitViewLink();
            link.AssetId = asset_id;
            return link;
        }

        #endregion

        #region |------------------------------------------------------------------| Blueprint Creators |------------------------------------------------------------------|


        public static BlueprintAbilityResource CreateAbilityResource(String name,
                                                              Sprite icon = null,
                                                              params BlueprintComponent[] components)

        {
            var resource = Helpers.CreateBlueprint<BlueprintAbilityResource>(ToDContext, name, bp =>
            {
                bp.m_Icon = icon;
                bp.SetComponents(components);
            });
            return resource;
        }

        // Holic75_PT
        public static BlueprintAbilityResource CreateAbilityResource(String name,
                                                              String display_name,
                                                              String description,         
                                                              Sprite icon = null,
                                                              params BlueprintComponent[] components)

        {
            var resource = Helpers.CreateBlueprint<BlueprintAbilityResource>(ToDContext, name, bp =>
            {
                bp.LocalizedName = Helpers.CreateString(ToDContext, $"{bp.name}.Name", display_name);
                bp.LocalizedDescription = Helpers.CreateString(ToDContext, $"{bp.name}.Name", description);
                bp.m_Icon = icon;
                bp.SetComponents(components);
            });
            return resource;
        }

        #endregion

        #region |----------------------------------------------------------| ( Abilities ) Components Creators |-----------------------------------------------------------|

        // Holic75_SC
        public static AbilityResourceLogic CreateResourceLogic(this BlueprintAbilityResource resource,
                                                        bool spend = true,
                                                        int amount = 1,
                                                        bool cost_is_custom = false)

        {
            var arl = Helpers.Create<AbilityResourceLogic>();
            arl.m_IsSpendResource = spend;
            arl.m_RequiredResource = resource.ToReference<BlueprintAbilityResourceReference>();
            arl.Amount = amount;
            arl.CostIsCustom = cost_is_custom;
            return arl;
        }

        // Holic75_PT
        public static AbilityTargetsAround CreateAbilityTargetsAround(Feet radius, TargetType target_type, ConditionsChecker conditions = null, Feet spread_speed = default(Feet), bool include_dead = false)
        {
            var around = Helpers.Create<AbilityTargetsAround>();
            around.m_Radius = radius;
            around.m_TargetType = target_type;
            around.m_IncludeDead = include_dead;
            around.m_Condition = conditions ?? new ConditionsChecker() { Conditions = Array.Empty<Condition>() };
            around.m_SpreadSpeed = spread_speed;
            return around;
        }

        #endregion

        #region |----------------------------------------------------------| ( Mechanics ) Components Creators |-----------------------------------------------------------|

        public static ContextCalculateAbilityParamsBasedOnClass CreateContextCalculateAbilityParamsBasedOnClass(BlueprintCharacterClassReference character_class,
                                                                                                         StatType stat,
                                                                                                         bool use_kineticist_main_stat = false)
        {
            var c = Helpers.Create<ContextCalculateAbilityParamsBasedOnClass>();
            c.m_CharacterClass = character_class;
            c.StatType = stat;
            c.UseKineticistMainStat = use_kineticist_main_stat;
            return c;
        }

        // Holic75_PT 
        public static ContextCalculateAbilityParamsBasedOnClasses CreateContextCalculateAbilityParamsBasedOnClasses(BlueprintCharacterClassReference[] character_classes,
                                                                                                                                   StatType stat)
        {
            var c = Helpers.Create<ContextCalculateAbilityParamsBasedOnClasses>();
            c.m_CharacterClasses = character_classes;
            c.m_StatType = stat;
            return c;
        }

        // Holic75_PT 
        public static ContextCalculateAbilityParamsBasedOnClasses CreateContextCalculateAbilityParamsBasedOnClassesWithProperty(BlueprintCharacterClassReference[] character_classes,
                                                                                                                                 BlueprintUnitPropertyReference property,
                                                                                                                                 StatType stat = StatType.Charisma)
        {
            var c = Helpers.Create<ContextCalculateAbilityParamsBasedOnClasses>();
            c.m_CharacterClasses = character_classes;
            c.m_StatType = stat;
            c.StatTypeFromCustomProperty = true;
            c.m_CustomProperty = property;
            return c;
        }

        // Holic75_PT 
        public static ContextCalculateAbilityParamsBasedOnClasses CreateContextCalculateAbilityParamsBasedOnClassesWithArchetypes(BlueprintCharacterClassReference[] character_classes,
                                                                                                                                     BlueprintArchetypeReference[] archetypes,
                                                                                                                                     StatType stat)
        {
            var c = Helpers.Create<ContextCalculateAbilityParamsBasedOnClasses>();
            c.m_CharacterClasses = character_classes;
            c.CheckArchetype = true;
            c.m_Archetypes = archetypes;
            c.m_StatType = stat;
            return c;
        }

        // Holic75_PT 
        public static ContextCalculateAbilityParamsBasedOnClasses CreateContextCalculateAbilityParamsBasedOnClassesWithArchetypesWithProperty(BlueprintCharacterClassReference[] character_classes,
                                                                                                                                               BlueprintArchetypeReference[] archetypes,
                                                                                                                                               BlueprintUnitPropertyReference property,
                                                                                                                                               StatType stat = StatType.Charisma)
        {
            var c = Helpers.Create<ContextCalculateAbilityParamsBasedOnClasses>();
            c.m_CharacterClasses = character_classes;
            c.CheckArchetype = true;
            c.m_Archetypes = archetypes;
            c.m_StatType = stat;
            c.StatTypeFromCustomProperty = true;
            c.m_CustomProperty = property;
            return c;
        }

        // Holic75_PT 
        public static ContextWeaponDamageDiceReplacementWeaponCategory CreateContextWeaponDamageDiceReplacementWeaponCategory(WeaponCategory[] categories, DiceFormula[] dice_formulas, ContextValue value)
        {
            var c = Helpers.Create<ContextWeaponDamageDiceReplacementWeaponCategory>();
            c.Categories = categories;
            c.Dice_Formulas = dice_formulas;
            c.Value = value;

            return c;
        }

        #endregion


        #region  |------------------------------------------------|  Converters - ActivatableAbility Creators (Simple) |----------------------------------------------------|

        // Holic75_PT
        // This converter creates a toggle that activates the buff. This is the full version, which adds all kind of name modification features.
        static public BlueprintActivatableAbility ConvertBuffToActivatableAbility(BlueprintBuff buff,
                                                                                  CommandType command,
                                                                                  bool deactivate_immediately,
                                                                                  string prefixAdd = "",
                                                                                  string prefixRemove = "",
                                                                                  string suffixAdd = "ToggleAbility",
                                                                                  string suffixRemove = "Buff",
                                                                                  string replaceOldText1 = "",
                                                                                  string replaceOldText2 = "",
                                                                                  string replaceOldText3 = "",
                                                                                  string replaceNewText1 = "",
                                                                                  string replaceNewText2 = "",
                                                                                 string replaceNewText3 = "",
                                                                                 params BlueprintComponent[] components
                                                        )
        {

            string toggleName = buff.Name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                toggleName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                toggleName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                toggleName = prefixAdd + toggleName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                toggleName = toggleName + suffixAdd;
            }
            if (!String.IsNullOrEmpty(replaceOldText1))
            {
                toggleName.Replace(replaceOldText1, replaceNewText1);
            }
            if (!String.IsNullOrEmpty(replaceOldText2))
            {
                toggleName.Replace(replaceOldText2, replaceNewText2);
            }
            if (!String.IsNullOrEmpty(replaceOldText3))
            {
                toggleName.Replace(replaceOldText3, replaceNewText3);
            }


            var toggle = Helpers.CreateBlueprint<BlueprintActivatableAbility>(ToDContext, toggleName, bp =>
            {
                bp.m_Buff = buff.ToReference<BlueprintBuffReference>();
                bp.SetName(ToDContext, buff.Name);
                bp.SetDescription(ToDContext, buff.Description);
                bp.m_Icon = buff.Icon;
                bp.ResourceAssetIds = new string[0];
                bp.ActivationType = (command == CommandType.Free) ? AbilityActivationType.Immediately : AbilityActivationType.WithUnitCommand;
                bp.m_ActivateWithUnitCommand = command;
                bp.SetComponents(components);
                bp.DeactivateImmediately = deactivate_immediately;

            });

            return toggle;

        }

        // Holic75_PT
        // This converter creates a toggle that activates the buff. This is the barebone version which lists only suffix to remove and to add.
        static public BlueprintActivatableAbility ConvertBuffToActivatableAbility(BlueprintBuff buff,
                                                                                  CommandType command,
                                                                                  bool deactivate_immediately,
                                                                                  string suffixAdd = "ToggleAbility",
                                                                                  string suffixRemove = "Buff",
                                                                                 params BlueprintComponent[] components
                                                        )
        {

            string toggleName = buff.Name;

            if (!String.IsNullOrEmpty(suffixRemove))
            {
                toggleName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                toggleName = toggleName + suffixAdd;
            }


            var toggle = Helpers.CreateBlueprint<BlueprintActivatableAbility>(ToDContext, toggleName, bp =>
            {
                bp.m_Buff = buff.ToReference<BlueprintBuffReference>();
                bp.SetName(ToDContext, buff.Name);
                bp.SetDescription(ToDContext, buff.Description);
                bp.m_Icon = buff.Icon;
                bp.ResourceAssetIds = new string[0];
                bp.ActivationType = (command == CommandType.Free) ? AbilityActivationType.Immediately : AbilityActivationType.WithUnitCommand;
                bp.m_ActivateWithUnitCommand = command;
                bp.SetComponents(components);
                bp.DeactivateImmediately = deactivate_immediately;

            });

            return toggle;

        }



        #endregion

        #region |-----------------------------------------------------|  Converters - Feature Creators (Simple) |----------------------------------------------------------|

        // Holic75_PT
        // This converter creates a feature that matches the ability from which has been created.
        // This was mostly used (in Holic75's mod) to create features that add bonus or required features (such as Iroran Paladin's auto-selection of Irori as Deity)
        static public BlueprintFeature ConvertFeatureToFeature(BlueprintFeature feature1,
                                                                    string prefixAdd = "",
                                                                    string prefixRemove = "",
                                                                    string suffixAdd = "",
                                                                    string suffixRemove = "",
                                                                    string replaceOldText1 = "",
                                                                    string replaceOldText2 = "",
                                                                    string replaceOldText3 = "",
                                                                    string replaceNewText1 = "",
                                                                    string replaceNewText2 = "",
                                                                    string replaceNewText3 = "",
                                                                    bool hide = true
                                                                )
        {

            string feature1AltName = feature1.Name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                feature1AltName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                feature1AltName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                feature1AltName = prefixAdd + feature1AltName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                feature1AltName = feature1AltName + suffixAdd;
            }
            if (!String.IsNullOrEmpty(replaceOldText1))
            {
                feature1AltName.Replace(replaceOldText1, replaceNewText1);
            }
            if (!String.IsNullOrEmpty(replaceOldText2))
            {
                feature1AltName.Replace(replaceOldText2, replaceNewText2);
            }
            if (!String.IsNullOrEmpty(replaceOldText3))
            {
                feature1AltName.Replace(replaceOldText3, replaceNewText3);
            }

            var feature2 = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, feature1AltName, bp =>
            {
                bp.SetName(ToDContext, feature1.Name);
                bp.SetDescription(ToDContext, feature1.Description);
                bp.m_Icon = feature1.Icon;
                bp.AddComponent<AddFeatureOnApply>(c =>
                {
                    c.m_Feature = feature1.ToReference<BlueprintFeatureReference>();
                });


                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
            });
            if (hide)
            {
                feature2.HideInCharacterSheetAndLevelUp = true;
            }

            return feature2;
        }

        // Holic75_PT
        // This converter creates a feature that matches the ability from which has been created.
        static public BlueprintFeature ConvertAbilityToFeature(BlueprintAbility ability,
                                                                    string prefixAdd = "",
                                                                    string prefixRemove = "",
                                                                    string suffixAdd = "Feature",
                                                                    string suffixRemove = "Ability",
                                                                    bool hide = true
                                                                )
        {

            string abilityAltName = ability.Name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                abilityAltName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                abilityAltName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                abilityAltName = prefixAdd + abilityAltName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                abilityAltName = abilityAltName + suffixAdd;
            }

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, abilityAltName, bp =>
            {
                bp.SetName(ToDContext, ability.Name);
                bp.SetDescription(ToDContext, ability.Description);
                bp.m_Icon = ability.Icon;
                bp.AddComponent<AddFeatureIfHasFact>(c =>
                {
                    c.m_CheckedFact = ability.ToReference<BlueprintUnitFactReference>();
                    c.m_Feature = ability.ToReference<BlueprintUnitFactReference>();
                    c.Not = true;
                });
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
            });
            if (hide)
            {
                feature.HideInCharacterSheetAndLevelUp = true;
                feature.HideInUI = true;
            }

            return feature;
        }

        // Holic75_PT
        // This converter creates a feature that matches the ability from which has been created (version without replacements)
        static public BlueprintFeature ConvertAbilityToFeature(BlueprintAbility ability,
                                                            bool hide = true
                                                        )
        {

            string abilityAltName = ability.Name + "Feature";


            var feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, abilityAltName, bp =>
            {
                bp.SetName(ToDContext, ability.Name);
                bp.SetDescription(ToDContext, ability.Description);
                bp.m_Icon = ability.Icon;
                bp.AddComponent<AddFeatureIfHasFact>(c =>
                {
                    c.m_CheckedFact = ability.ToReference<BlueprintUnitFactReference>();
                    c.m_Feature = ability.ToReference<BlueprintUnitFactReference>();
                    c.Not = true;
                });
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
            });
            if (hide)
            {
                feature.HideInCharacterSheetAndLevelUp = true;
                feature.HideInUI = true;
            }

            return feature;
        }

        // Holic75_PT
        // This converter creates a feature that matches the ability from which has been created but adds without checking.
        static public BlueprintFeature ConvertAbilityToFeatureNoCheck(BlueprintAbility ability,
                                                                        string prefixAdd = "",
                                                                        string prefixRemove = "",
                                                                        string suffixAdd = "Feature",
                                                                        string suffixRemove = "Ability",
                                                                        bool hide = true
                                                                    )
        {

            string abilityAltName = ability.Name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                abilityAltName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                abilityAltName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                abilityAltName = prefixAdd + abilityAltName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                abilityAltName = abilityAltName + suffixAdd;
            }

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, abilityAltName, bp =>
            {
                bp.SetName(ToDContext, ability.Name);
                bp.SetDescription(ToDContext, ability.Description);
                bp.m_Icon = ability.Icon;
                bp.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[] { ability.ToReference<BlueprintUnitFactReference>() };
                });
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
            });
            if (hide)
            {
                feature.HideInCharacterSheetAndLevelUp = true;
                feature.HideInUI = true;
            }

            return feature;
        }

        // Holic75_PT
        // This converter creates a feature that matches the activatable ability from which has been created and adds it without checking.
        static public BlueprintFeature ConvertActivatableAbilityToFeature(BlueprintActivatableAbility ability,
                                                                        string prefixAdd = "",
                                                                        string prefixRemove = "",
                                                                        string suffixAdd = "Feature",
                                                                        string suffixRemove = "ToggleAbility",
                                                                        bool hide = true
                                                                    )
        {

            string activatable_abilityAltName = ability.Name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                activatable_abilityAltName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                activatable_abilityAltName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                activatable_abilityAltName = prefixAdd + activatable_abilityAltName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                activatable_abilityAltName = activatable_abilityAltName + suffixAdd;
            }

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, activatable_abilityAltName, bp =>
            {
                bp.SetName(ToDContext, ability.Name);
                bp.SetDescription(ToDContext, ability.Description);
                bp.m_Icon = ability.Icon;
                bp.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[] { ability.ToReference<BlueprintUnitFactReference>() };
                });
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
            });
            if (hide)
            {
                feature.HideInCharacterSheetAndLevelUp = true;
                feature.HideInUI = true;
            }

            return feature;
        }

        // Holic75_PT
        // This converter creates a feature that matches the buff from which has been created.
        static public BlueprintFeature ConvertBuffToFeature(BlueprintBuff buff,
                                                            string prefixAdd = "",
                                                            string prefixRemove = "",
                                                            string suffixAdd = "Feature",
                                                            string suffixRemove = "",
                                                            bool hide = true
                                                        )
        {

            string buffAltName = buff.Name;

            if (!String.IsNullOrEmpty(prefixRemove))
            {
                buffAltName.Replace(prefixRemove, "");
            }
            if (!String.IsNullOrEmpty(suffixRemove))
            {
                buffAltName.Replace(suffixRemove, "");
            }
            if (!String.IsNullOrEmpty(prefixAdd))
            {
                buffAltName = prefixAdd + buffAltName;
            }
            if (!String.IsNullOrEmpty(suffixAdd))
            {
                buffAltName = buffAltName + suffixAdd;
            }

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, buffAltName, bp =>
            {
                bp.SetName(ToDContext, buff.Name);
                bp.SetDescription(ToDContext, buff.Description);
                bp.m_Icon = buff.Icon;
                bp.SetComponents(buff.ComponentsArray);
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            return feature;
        }



        #endregion

        #region |-------------------------------------------------------|  ( Blueprint ) Miscellaneous Functions |---------------------------------------------------------|

        // Holic75_PT
        public static void SetFixedResource(this BlueprintAbilityResource resource, int baseValue)
        {
            var amount = resource.m_MaxAmount;
            amount.BaseValue = baseValue;

            // Enusre arrays are at least initialized to empty.
            var emptyClasses = Array.Empty<BlueprintCharacterClassReference>();
            var emptyArchetypes = Array.Empty<BlueprintArchetypeReference>();


            if (amount.m_Class == null) amount.m_Class = emptyClasses;
            if (amount.m_ClassDiv == null) amount.m_ClassDiv = emptyClasses;
            if (amount.m_Archetypes == null) amount.m_Archetypes = emptyArchetypes;
            if (amount.m_ArchetypesDiv == null) amount.m_ArchetypesDiv = emptyArchetypes;

            resource.m_MaxAmount = amount;
        }


        #endregion

    }
}
