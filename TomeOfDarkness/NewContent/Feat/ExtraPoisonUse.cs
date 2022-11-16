using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;


namespace TomeOfDarkness.NewContent.Feat
{
    internal class ExtraPoisonUse
    {
        public static void ConfigureExtraPoisonUse()
        {
            // This feat is homebrew, but has been balanced against the Extra Arcane Reservoir feat.

            var Assassin_Create_Poison_Icon = BlueprintTools.GetBlueprint<BlueprintFeature>("8dd826513ba857645b38e918f17b59e6").m_Icon;
            var Assassin_Create_Poison_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("8dd826513ba857645b38e918f17b59e6");
            var Assassin_Create_Poison_Resource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("d54b614eb42da7d48b927b57de337b95");

            var ExtraPoisonUseFeat = FeatTools.CreateExtraResourceFeat(ToDContext, "ExtraPoisonUse", Assassin_Create_Poison_Resource, 3, bp => {
                bp.SetName(ToDContext, "Extra Poison Use");
                bp.SetDescription(ToDContext, "You can use your poison use ability three additional times per day." +
                    "\nYou can take this feat multiple times. Its effects stack.");
                bp.AddPrerequisiteFeature(Assassin_Create_Poison_Feature);
            });


            if (ToDContext.NewContent.Feats.IsDisabled("ExtraPoisonUse")) { return; }

            FeatTools.AddAsFeat(ExtraPoisonUseFeat);

            ToDContext.Logger.LogPatch("Created Extra Poison Use feat.", ExtraPoisonUseFeat);

        }

    }
}
