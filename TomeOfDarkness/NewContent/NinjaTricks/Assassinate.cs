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
using Kingmaker.Craft;

namespace TomeOfDarkness.NewContent.NinjaTricks
{
    internal class Assassinate
    {
        public static void ConfigureAssassinate()
        {
            var AssassinateNinjaIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_AssassinateNinja.png");
            var AssassinateSlayerIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_AssassinateSlayer.png");
            var Executioner_Assassinate_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("bd7e70e98f9036f4ba27ef3e29a177c2");
            var Executioner_Assassinate_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("3dad7f131aa884f4c972f2fb759d0df4");
            var Executioner_Assassinate14_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("c81f15dbbadbb3a48b37e95a5a7ef759");
            var Executioner_Assassinate19_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("a10abd69551134341a6f771d23535165");
            var Executioner_Assassinate_Demoralize_Action = BlueprintTools.GetBlueprint<BlueprintAbility>("3e7780219eb23b64f8dac5b29bb32e23");






        }
    }
}
