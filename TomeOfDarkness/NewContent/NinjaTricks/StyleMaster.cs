using Kingmaker.Blueprints;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;
using TomeOfDarkness.Utilities;
using HlEX = TomeOfDarkness.Utilities.HelpersExtension;
using Kingmaker.Blueprints.Classes.Selection;

namespace TomeOfDarkness.NewContent.NinjaTricks
{
    internal class StyleMaster
    {
        public static void ConfigureStyleMaster()
        {
            var RogueArray = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };

            var current_style_feats = FeatToolsExtension.GetStyleFeats();

            var Style_Master_Feature_Selection = Helpers.CreateBlueprint<BlueprintFeatureSelection>(ToDContext, "NinjaStyleMasterFeatureSelection", bp => {
                bp.SetName(ToDContext, "Style Master");
                bp.SetDescription(ToDContext, "A chracter who selects this trick gains a style feat that she qualifies for as a bonus feat.");
                bp.m_Icon = null;
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                foreach (var current_style_feat in current_style_feats)
                {
                    bp.AddFeatures(current_style_feat);
                }


            });

            Style_Master_Feature_Selection.AddComponent(HlEX.CreatePrerequisiteNoFeature(Style_Master_Feature_Selection));

            ToDContext.Logger.LogPatch("Created Style Master ninja trick.", Style_Master_Feature_Selection);


        }

    }
}
