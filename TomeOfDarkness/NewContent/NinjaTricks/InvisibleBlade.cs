using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;


namespace TomeOfDarkness.NewContent.NinjaTricks
{
    internal class InvisibleBlade
    {
        public static void ConfigureInvisibleBlade()
        {
            var InvisibleBladeIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_InvisibleBlade.png");

            var InvisibleBladeFeature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickInvisibleBladeFeature", bp => {
                bp.SetName(ToDContext, "Invisible Blade");
                bp.SetDescription(ToDContext, "Whenever the character uses the Vanishing Trick ninja trick, she is treated as if she were under the effects of Greater Invisibility.");
                bp.m_Icon = InvisibleBladeIcon;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
            });

            VanishingTrick.ConfigureVanishingTrick();

            ToDContext.Logger.LogPatch("Created Invisible Blade ninja trick.", InvisibleBladeFeature);

        }
    }
}
