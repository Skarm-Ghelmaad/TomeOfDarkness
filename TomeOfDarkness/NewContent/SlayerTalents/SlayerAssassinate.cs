using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TomeOfDarkness.NewComponents;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TomeOfDarkness.MechanicsChanges;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics;
using HlEX = TomeOfDarkness.Utilities.HelpersExtension;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Mechanics.Components;
using TomeOfDarkness.Utilities;
using Kingmaker.AreaLogic.Cutscenes;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using RootMotion.FinalIK;
using static TabletopTweaks.Core.Utilities.FeatTools;
using System.Runtime.ConstrainedExecution;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;
using TomeOfDarkness.NewComponents.OwlcatReplacements;
using Kingmaker.UnitLogic.Abilities;
using static Kingmaker.UnitLogic.Interaction.SpawnerInteractionPart;
using Kingmaker.Blueprints.Classes.Selection;
using TomeOfDarkness.NewContent.NinjaTricks;
using TomeOfDarkness.NewContent.Features;
using static UnityModManagerNet.UnityModManager;
using HarmonyLib;
using System.Text.RegularExpressions;


namespace TomeOfDarkness.NewContent.SlayerTalents
{
    internal class SlayerAssassinate
    {
        public static void ConfigureSlayerAssassinate()
        {
            var Executioner_Assassinate_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("bd7e70e98f9036f4ba27ef3e29a177c2");
            var Executioner_Archetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("8040e66d2724c754f85dd3026a1f0cee");


            var Slayer_Talent_Assassination_Feature = Executioner_Assassinate_Feature.CreateCopy(ToDContext, "SlayerTalentAssassinateFeature");
            Slayer_Talent_Assassination_Feature.SetDescription(ToDContext, "A slayer with this advanced talent can kill foes that are unable to defend themselves. To attempt to assassinate a target, the slayer must have studied that opponent, but thereafter he can attempt to instantly kill him (or any other studied opponent). This ability can only be used out of combat and the target must not see the slayer, but this special {g|Encyclopedia:Attack}attack{/g} automatically hits, scores a {g|Encyclopedia:Critical}critical hit{/g} and, if the victim survives, must make a {g|Encyclopedia:Saving_Throw}Fortitude save{/g} ({g|Encyclopedia:DC}DC{/g} 10 + slayer level + relevant stat modifier or die.");
            Slayer_Talent_Assassination_Feature.AddComponent(HlEX.CreatePrerequisiteClassLevel(ClassTools.Classes.SlayerClass, 1, false));
            Slayer_Talent_Assassination_Feature.AddComponent(HlEX.CreatePrerequisiteNoArchetype(ClassTools.Classes.SlayerClass, Executioner_Archetype));
            Slayer_Talent_Assassination_Feature.Groups = new FeatureGroup[] { FeatureGroup.SlayerTalent };

            if (ToDContext.NewContent.SlayerTalents.IsDisabled("SlayerAssassinate")) { return; }

            FeatToolsExtension.AddAsSlayerTalent(Slayer_Talent_Assassination_Feature, true, new BlueprintFeatureSelection[] { Selections.SlayerTalentSelection2, Selections.SlayerTalentSelection6 });

            ToDContext.Logger.LogPatch("Created Assassinate slayer avanced talent.", Slayer_Talent_Assassination_Feature);

        }
    }
}
