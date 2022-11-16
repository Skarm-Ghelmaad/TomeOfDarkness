using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;
using Kingmaker.UnitLogic.Commands.Base;
using HlEX = TomeOfDarkness.Utilities.HelpersExtension;

namespace TomeOfDarkness.NewContent.NinjaTricks
{
    internal class SeeTheUnseen
    {

        public static void ConfigureSeeTheUnseen()
        {
            var RogueArray = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };

            var kiResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82");

            var SeeTheUnseenIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_SeeTheUnseen.png");

            var See_Invisibility_Spell = BlueprintTools.GetBlueprint<BlueprintAbility>("30e5dc243f937fc4b95d2f8f4e1b7ff3");

            var SeeTheUnseenAbility = HlEX.ConvertSpellToSupernatural(See_Invisibility_Spell, RogueArray, StatType.Charisma, kiResource, "NinjaTrick", "Ability", "SeeTheUnseen", "SeeInvisibility", "", "", "", "", "", "", false, false, null, 1);

            SeeTheUnseenAbility.SetName(ToDContext, "See the Unseen");
            SeeTheUnseenAbility.SetDescription(ToDContext, "A character with this trick learns how to see that which cannot be seen. As a swift action, the character can cast See Invisibility, using her relevant class level as the caster level. Each use of this ability uses up 1 ki point.");
            SeeTheUnseenAbility.m_Icon = SeeTheUnseenIcon;
            SeeTheUnseenAbility.ActionType = UnitCommand.CommandType.Swift;

            var see_the_unseen_feature = HlEX.ConvertAbilityToFeature(SeeTheUnseenAbility, "", "", "Feature", "Ability", false);
            
            see_the_unseen_feature.IsClassFeature = true;
            see_the_unseen_feature.Ranks = 1;

            ToDContext.Logger.LogPatch("Created See The Unseen ninja trick.", see_the_unseen_feature);

        }

    }
}
