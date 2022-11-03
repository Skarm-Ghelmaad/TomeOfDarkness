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
using Kingmaker.Blueprints.Classes.Selection;
using HlEX = TomeOfDarkness.Utilities.HelpersExtension;
using static TabletopTweaks.Core.Utilities.FeatTools;
using static UnityModManagerNet.UnityModManager;

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

                foreach (var current_style_feat in current_style_feats)
                {
                    bp.AddFeatures(current_style_feat);
                }


            });

            Style_Master_Feature_Selection.CreatePrerequisiteNoFeature(Style_Master_Feature_Selection);

            ToDContext.Logger.LogPatch("Created Style Master ninja trick.", Style_Master_Feature_Selection);


        }

    }
}
