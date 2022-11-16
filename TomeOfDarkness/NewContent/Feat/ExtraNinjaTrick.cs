using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;
using Kingmaker.Blueprints.Classes.Selection;

namespace TomeOfDarkness.NewContent.Feat
{
    internal class ExtraNinjaTrick
    {
        public static void ConfigureExtraNinjaTrick()
        {
            var Ninja_Trick_Selection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(ToDContext, "NinjaTrickSelection");
            var NinjaTrickSelectionIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_NinjaTrickSelection.png");

            var ExtraNinjaTrickFeat = FeatTools.CreateExtraSelectionFeat(ToDContext, "ExtraNinjaTrick", Ninja_Trick_Selection, bp => {
                bp.SetName(ToDContext, "Extra Ninja Trick");
                bp.SetDescription(ToDContext, "You gain one additional ninja trick. You must meet all of the prerequisites for this ninja trick." +
                    "\nYou can gain Extra Ninja Trick multiple times.");
                bp.m_Icon = NinjaTrickSelectionIcon;
            });

            if (ToDContext.NewContent.Feats.IsDisabled("ExtraNinjaTrick")) { return; }

            FeatTools.AddAsFeat(ExtraNinjaTrickFeat);

            ToDContext.Logger.LogPatch("Created Extra Ninja Trick feat.", ExtraNinjaTrickFeat);

        }

    }
}
