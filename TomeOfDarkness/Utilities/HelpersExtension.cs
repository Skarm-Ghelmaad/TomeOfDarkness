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



    }
}
