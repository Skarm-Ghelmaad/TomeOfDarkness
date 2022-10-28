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

namespace TomeOfDarkness.NewContent.NinjaTricks
{
    internal class InvisibleBlade
    {
        private static readonly string InvisibleBladeFeatureName = "NinjaTrickInvisibleBladeFeature.Name";
        private static readonly string InvisibleBladeFeatureDescription = "NinjaTrickInvisibleBladeFeature.Description";

        public static void ConfigureInvisibleBlade()
        {
            var InvisibleBladeIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_InvisibleBlade.png");

            var InvisibleBladeFeature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickInvisibleBladeFeature", bp => {
                bp.SetName(ToDContext, InvisibleBladeFeatureName);
                bp.SetDescription(ToDContext, InvisibleBladeFeatureDescription);
                bp.m_Icon = InvisibleBladeIcon;
            });

            VanishingTrick.ConfigureVanishingTrick();

            ToDContext.Logger.LogPatch("Created Invisible Blade ninja trick.", InvisibleBladeFeature);

        }
    }
}
