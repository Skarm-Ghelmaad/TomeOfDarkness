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
using System;

namespace TomeOfDarkness.NewContent.NinjaTricks
{
    internal class ShadowClone
    {

        public static void ConfigureShadowClone()
        {
            var RogueArray = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };

            var kiResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82");

            var ShadowCloneIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_ShadowClone.png");

            var Mirror_Image_Spell = BlueprintTools.GetBlueprint<BlueprintAbility>("3e4ab69ada402d145a5e0ad3ad4b8564");

            var ShadowCloneAbility = HlEX.ConvertSpellToSupernatural(Mirror_Image_Spell, RogueArray, StatType.Charisma, kiResource, "NinjaTrick", "Ability", "ShadowClone", "MirrorImage", "", "", "", "", "", "", false, false, null, 1);

            ShadowCloneAbility.SetName(ToDContext, "Shadow Clone");
            ShadowCloneAbility.SetDescription(ToDContext, "A character with this trick can create shadowy duplicates of herself that conceal her true location. This ability functions as Mirror Image, using his class level as her caster level. Using this ability is a standard action that uses up 1 ki point. The charater creates 1d4 duplicates plus one duplicates per three caster levels (maximum eight duplicates total) are created. [LONGSTART] These images remain in your space and move with you, mimicking your movements, sounds, and {g|Encyclopedia:CA_Types}actions{/g} exactly.[LONGEND] Whenever you are attacked or are the target of a spell that requires an attack roll, there is a possibility that the attack targets one of your images instead.[LONGSTART] If the attack is a hit, roll randomly to see whether the selected target is real or a figment. If it is a figment, the figment is destroyed. If the attack misses by 5 or less, one of your figments is destroyed by the near miss. Area spells affect you normally and do not destroy any of your figments. Spells and effects that do not require an attack roll affect you normally and do not destroy any of your figments. Spells that require a {g|Encyclopedia:TouchAttack}touch attack{/g} are harmlessly discharged if used to destroy a figment.\nAn attacker must be able to see the figments to be fooled. If you are invisible or the attacker is blind, the spell has no effect (although the normal miss chances still apply).[LONGEND] ");
            ShadowCloneAbility.m_Icon = ShadowCloneIcon;

            var shadow_clone_feature = HlEX.ConvertAbilityToFeature(ShadowCloneAbility, "", "", "Feature", "Ability", false);

            ToDContext.Logger.LogPatch("Created Shadow Clone ninja trick.", shadow_clone_feature);


        }

    }
}
