using Kingmaker.Blueprints.Classes;
using Kingmaker.Utility;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using static TomeOfDarkness.Main;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using HlEX = TomeOfDarkness.Utilities.HelpersExtension;
using Kingmaker.ResourceLinks;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using TomeOfDarkness.Utilities;

namespace TomeOfDarkness.NewContent.NinjaTricks
{
    public class NinjaAssassinate
    {
        public static void ConfigureNinjaAssassinate()
        {
  
            var Executioner_Assassinate_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("bd7e70e98f9036f4ba27ef3e29a177c2");                                        //[CHECKED]
            var Executioner_Assassinate_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("3dad7f131aa884f4c972f2fb759d0df4");                                        //[CHECKED]
            var Slayer_Study_Target_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("b96d810ceb1708b4e895b695ddbb1813");                                            //[CHECKED]
            var AssassinateStudyIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_AssassinateStudyNinja.png");
            var NinjaTrickAssassinateMeleeIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_AssassinateNinja.png");
            var Rogue_Assassination_Training_Progression = BlueprintTools.GetModBlueprint<BlueprintProgression>(ToDContext, "RogueAssassinationTrainingProgression");
            var Assassination_Charisma_Feature = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "AssassinationCharismaStatFeature");



            var Ninja_Trick_Assassination_Marking_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "NinjaTrickAssassinateMarkingBuff", bp => {
                bp.SetName(ToDContext, "Marked for Assassination");
                bp.SetDescription(ToDContext, "This creature has been marked for assassination.");
                bp.m_Icon = AssassinateStudyIcon;
                bp.FxOnStart = new PrefabLink();
                bp.FxOnRemove = new PrefabLink();
            });

            var Ninja_Trick_Assassination_Study_Ability = Slayer_Study_Target_Ability.CreateCopy(ToDContext, "NinjaTrickAssassinateStudyTargetAbility", bp => {
                bp.SetName(ToDContext, "Study Assassination Target");
                bp.SetDescription(ToDContext, "A character can study an opponent he can see as a {g|Encyclopedia:Move_Action}move action{/g} to mark him for assassination. Subsequently, as long as the character is out of combat and the marked target cannot see him, he can attempt to instantly kill him.");
                bp.m_Icon = AssassinateStudyIcon;
            });

            var Ninja_Trick_Assassination_Study_Ability_Conditional = Ninja_Trick_Assassination_Study_Ability.FlattenAllActions().OfType<Conditional>().FirstOrDefault();
            var Ninja_Trick_Assassination_Study_Ability_Context_Action_Apply_Buff = Ninja_Trick_Assassination_Study_Ability.FlattenAllActions().OfType<ContextActionApplyBuff>().FirstOrDefault();

            var Ninja_Trick_Assassination_Study_Ability_Context_Condition_Has_Buff_From_Caster = Ninja_Trick_Assassination_Study_Ability_Conditional.ConditionsChecker.Conditions.OfType<ContextConditionHasBuffFromCaster>().FirstOrDefault();

            Ninja_Trick_Assassination_Study_Ability_Context_Condition_Has_Buff_From_Caster.TemporaryContext(c => {
                c.m_Buff = Ninja_Trick_Assassination_Marking_Buff.ToReference<BlueprintBuffReference>();
            });

            Ninja_Trick_Assassination_Study_Ability_Context_Action_Apply_Buff.TemporaryContext(c => {
                c.m_Buff = Ninja_Trick_Assassination_Marking_Buff.ToReference<BlueprintBuffReference>();
            });


            var Ninja_Trick_Assassinate_Ability = Executioner_Assassinate_Ability.CreateCopy(ToDContext, "NinjaTrickAssassinateAbility", bp => {
                bp.SetDescription(ToDContext, "The character can attempt to instantly kill any target marked for assassination. This ability can only be used out of combat and the target must not see the character, but this special {g|Encyclopedia:Attack}attack{/g} automatically hits, scores a {g|Encyclopedia:Critical}critical hit{/g} and, if the victim survives, must make a {g|Encyclopedia:Saving_Throw}Fortitude save{/g} ({g|Encyclopedia:DC}DC{/g} 10 + relevant class level + relevant stat modifier or die. \n This special {g|Encyclopedia:Attack}attack{/g} is a full-{g|Encyclopedia:Combat_Round}round{/g} {g|Encyclopedia:CA_Types}action{/g} that provokes {g|Encyclopedia:Attack_Of_Opportunity}attacks of opportunity{/g} from {g|Encyclopedia:Threatened_Area}threatening{/g} opponents.");
                bp.m_Icon = NinjaTrickAssassinateMeleeIcon;
            });

            Ninja_Trick_Assassinate_Ability.GetComponent<AbilityTargetHasConditionOrBuff>().TemporaryContext(c => {
                c.m_Buffs = new BlueprintBuffReference[] { Ninja_Trick_Assassination_Marking_Buff.ToReference<BlueprintBuffReference>() };
            });

            var Ninja_Trick_Assassinate_Ability_Variants = new BlueprintAbility[] { Ninja_Trick_Assassinate_Ability, Ninja_Trick_Assassination_Study_Ability };

            var Ninja_Trick_Assassinate_Wrapper_Ability = HlEX.CreateVariantWrapper("NinjaTrickAssassinateBaseAbility", Ninja_Trick_Assassinate_Ability_Variants);

            Ninja_Trick_Assassinate_Wrapper_Ability.TemporaryContext(ab => {
                ab.SetName(ToDContext, "Assassinate");
                ab.SetDescription(ToDContext, "A character with this master trick can kill foes that are unable to defend themselves. To attempt to assassinate a target, the character must first study his target as a {g|Encyclopedia:Move_Action}move action{/g} to mark him for assassination. Subsequently he can attempt to instantly kill him (or any other marked target). This ability can only be used out of combat and the target must not see the character, but this special {g|Encyclopedia:Attack}attack{/g} automatically hits, scores a {g|Encyclopedia:Critical}critical hit{/g} and, if the victim survives, must make a {g|Encyclopedia:Saving_Throw}Fortitude save{/g} ({g|Encyclopedia:DC}DC{/g} 10 + relevant class level + relevant stat modifier or die. \n This special {g|Encyclopedia:Attack}attack{/g} is a full-{g|Encyclopedia:Combat_Round}round{/g} {g|Encyclopedia:CA_Types}action{/g} that provokes {g|Encyclopedia:Attack_Of_Opportunity}attacks of opportunity{/g} from {g|Encyclopedia:Threatened_Area}threatening{/g} opponents.");
                ab.m_Icon = NinjaTrickAssassinateMeleeIcon;
            });

            var Ninja_Trick_Assassinate_Feature = HlEX.ConvertAbilityToFeature(Ninja_Trick_Assassinate_Wrapper_Ability, "", "", "Feature", "BaseAbility", false);

            Ninja_Trick_Assassinate_Feature.TemporaryContext(bp => {
                bp.AddComponent(Helpers.Create<AddFeatureOnApply>(c => {
                    c.m_Feature = Rogue_Assassination_Training_Progression.ToReference<BlueprintFeatureReference>();
                }));
                bp.AddComponent(HlEX.CreateAddFacts(new BlueprintUnitFactReference[] { Assassination_Charisma_Feature.ToReference<BlueprintUnitFactReference>() }));
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.AddComponent(HlEX.CreatePrerequisiteNoFeature(Ninja_Trick_Assassinate_Feature));
            });

            ToDContext.Logger.LogPatch("Created Assassinate ninja trick.", Ninja_Trick_Assassinate_Feature);
        }

    }
}
