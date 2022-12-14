using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;
using HlEX = TomeOfDarkness.Utilities.HelpersExtension;
using Kingmaker.Blueprints.Classes.Selection;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints.Classes.Prerequisites;
using TomeOfDarkness.Utilities;

namespace TomeOfDarkness.NewContent.NinjaTricks
{
    internal class UnarmedCombatMastery
    {
        public static void ConfigureUnarmedCombatMastery()
        {
            var RogueArray = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };
            var monk_1d6_unarmed_strike = BlueprintTools.GetBlueprint<BlueprintFeature>("c3fbeb2ffebaaa64aa38ce7a0bb18fb0");
            var improved_unarmed_strike = BlueprintTools.GetBlueprint<BlueprintFeature>("7812ad3672a4b9a4fb894ea402095167");

            var MartialArtsTrainingAzureIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_MartialArtsTrainingAzure.png");
            var MartialArtsTrainingBlackIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_MartialArtsTrainingBlack.png");
            var MartialArtsTrainingBlueIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_MartialArtsTrainingBlue.png");
            var MartialArtsTrainingBrownIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_MartialArtsTrainingBrown.png");
            var MartialArtsTrainingDarkGreenIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_MartialArtsTrainingDarkGreen.png");
            var MartialArtsTrainingGrayIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_MartialArtsTrainingGray.png");
            var MartialArtsTrainingLightGreenIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_MartialArtsTrainingLightGreen.png");
            var MartialArtsTrainingOrangeIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_MartialArtsTrainingOrange.png");
            var MartialArtsTrainingPurpleIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_MartialArtsTrainingPurple.png");
            var MartialArtsTrainingRedIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_MartialArtsTrainingRed.png");
            var MartialArtsTrainingYellowIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_MartialArtsTrainingYellow.png");
            var UnarmedCombatMasteryIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_UnarmedCombatMastery.png");

            var monk_fake_levels = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(ToDContext, "MartialArtsTrainingFakeLevel");

            var Universal_Unarmed_Strike = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "UniversalUnarmedStrike");

            var RogueMartialArtsTrainingProgression = Helpers.CreateBlueprint<BlueprintProgression>(ToDContext, "RogueMartialArtsTrainingProgression", bp => {
                bp.SetName(ToDContext, "Rogue Martial Training");
                bp.SetDescription(ToDContext, "The character treats his rogue level -4 as monk level for the purposes of unarmed strike damage.");
                bp.m_Icon = MartialArtsTrainingBlackIcon;
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
                            monk_fake_levels.Get().ToReference<BlueprintFeatureBaseReference>()
                        },
                    })
                    .ToArray();
                bp.AddClass(ClassTools.ClassReferences.RogueClass);
                bp.m_Classes[0].AdditionalLevel = -4;
            });

            var UnarmedCombatMasteryNinjaFeature = Helpers.CreateBlueprint<BlueprintFeatureSelection>(ToDContext, "NinjaTrickUnarmedCombatMasteryNinjaFeature", bp => {
                bp.SetName(ToDContext, "Unarmed Combat Mastery");
                bp.SetDescription(ToDContext, "A character who selects this trick deals damage with her unarmed strikes as if she were a monk of her ninja level –4. If the rogue has levels in monk (or other similar features), this ability stacks with monk levels to determine how much damage she can do with her unarmed strikes. A ninja must have the Improved Unarmed Strike feat before taking this trick.");
                bp.m_Icon = MartialArtsTrainingBlackIcon;
                bp.AddComponent(HlEX.CreateHasFactFeatureUnlock(monk_1d6_unarmed_strike.ToReference<BlueprintUnitFactReference>(), Universal_Unarmed_Strike.ToReference<BlueprintUnitFactReference>(), true));
                bp.AddComponent(Helpers.Create<AddFeatureOnApply>(c => {
                    c.m_Feature = RogueMartialArtsTrainingProgression.ToReference<BlueprintFeatureReference>();
                }));
                bp.AddPrerequisites(Helpers.Create<PrerequisiteFeature>(c => {
                    c.m_Feature = improved_unarmed_strike.ToReference<BlueprintFeatureReference>();
                    c.Group = Prerequisite.GroupType.All;
                }));
            });

            UnarmedCombatMasteryNinjaFeature.AddComponent(HlEX.CreatePrerequisiteNoFeature(UnarmedCombatMasteryNinjaFeature));

            ToDContext.Logger.LogPatch("Created Unarmed Combat Mastery ninja trick.", UnarmedCombatMasteryNinjaFeature);



        }
    }
}
