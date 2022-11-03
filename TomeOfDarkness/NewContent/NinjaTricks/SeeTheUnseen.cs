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
using System.Drawing;
using Kingmaker.UnitLogic.Buffs;
using System;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System.Linq;

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

            ToDContext.Logger.LogPatch("Created See The Unseen ninja trick.", see_the_unseen_feature);

        }

    }
}
