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
using Kingmaker.Blueprints.Items.Ecnchantments;
using System.Linq;
using System;
using Kingmaker.Craft;
using Kingmaker.RuleSystem;
using TomeOfDarkness.NewComponents.AreaEffects;

namespace TomeOfDarkness.NewContent.NinjaTricks
{
    internal class NinjaCanonBombs
    {
        public static void ConfigureNinjaCanonBombs()
        {
            // Note that, unlike Holic75's original design, I've decided to keep the same dark smoke fx for all three smoke bombs.

            var Rogue_Array = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };

            var SmokeBombIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_NinjaBombBlue.png");
            var ChokingBombIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_NinjaBombPurple.png");
            var BlindingBombIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_NinjaBombYellow.png");

            var obscuring_mist_spell = BlueprintTools.GetModBlueprint<BlueprintAbility>(ToDContext, "ObscuringMistAbility");
            var obscuring_mist_area = BlueprintTools.GetModBlueprint<BlueprintAbilityAreaEffect>(ToDContext, "ObscuringMistArea");
            var Plague_Storm_Fx_ID = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("b342e42d2ed58484c8dff9150d18f4e4").Fx.AssetId; // Use Plague Storm (Blinding Sickness) for smoke bomb as it resembles more dark smoke.
            var kiResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82");

            var Staggered_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("df3950af5a783bd4d91ab73eb8fa0fd3");
            var Blinded_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("187f88d96a0ef464280706b63635f2af");


            #region |---------------------------------------------------------------|  Create Smoke Bomb |-------------------------------------------------------------------|

            var Smoke_Bomb_Area = obscuring_mist_area.CreateCopy(ToDContext, "NinjaTrickSmokeBombArea", bp =>
            {
                bp.Fx = HlEX.CreatePrefabLink(Plague_Storm_Fx_ID);

            });

            var Smoke_Bomb_Spawn_Action = HlEX.CreateContextActionSpawnAreaEffect(Smoke_Bomb_Area.ToReference<BlueprintAbilityAreaEffectReference>(),
                                                    HlEX.CreateContextDuration(1, DurationRate.Minutes));


            var Smoke_Bomb_Ability = obscuring_mist_spell.CreateCopy(ToDContext, "NinjaTrickSmokeBombAbility", bp =>
            {
                bp.SetName(ToDContext, "Smoke Bomb");
                bp.SetDescription(ToDContext, "This ability allows the character to throw a smoke bomb that creates a cloud of smoke with a 20 - foot radius. This acts like a fog cloud spell with duration of 1 minute. Using this ability is a standard action. Each use of this ability uses up 1 ki point.");
                bp.m_Icon = SmokeBombIcon;
                bp.RemoveComponents<SpellListComponent>();
                bp.RemoveComponents<ContextRankConfig>();
                bp.RemoveComponents<CraftInfoComponent>();
                bp.Type = AbilityType.Extraordinary;
                bp.Range = AbilityRange.Close;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Thrown;
                bp.AvailableMetamagic = (Metamagic)0;
                bp.LocalizedDuration = Helpers.CreateString(ToDContext, $"{bp.name}.Duration", "1 minute");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.ReplaceComponents<AbilityEffectRunAction>(HlEX.CreateRunActions(Smoke_Bomb_Spawn_Action));
                bp.AddComponent(kiResource.CreateResourceLogic());
                bp.AddComponent(HlEX.CreateContextCalculateAbilityParamsBasedOnClass(ClassTools.ClassReferences.RogueClass, StatType.Charisma));

            });

            var Smoke_Bomb_Feature = HlEX.ConvertAbilityToFeature(Smoke_Bomb_Ability, "", "", "Feature", "Ability", false);

            ToDContext.Logger.LogPatch("Created Smoke Bomb Trick ninja trick.", Smoke_Bomb_Feature);


            #endregion

            #region |---------------------------------------------------------------|  Create Choking Bomb |-------------------------------------------------------------------|

            var Apply_Staggered_Buff = HlEX.CreateContextActionConditionalSaved(null, HlEX.CreateContextActionApplyBuff(Staggered_Buff, HlEX.CreateContextDuration(0, DurationRate.Rounds, DiceType.D4, 1), false));
            var Apply_Staggered_Buff_Saved = HlEX.CreateConditional(HlEX.CreateContextConditionHasFact(Staggered_Buff), HlEX.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(Apply_Staggered_Buff)));
            var Choking_Bomb_Action = Helpers.Create<AbilityAreaEffectRunActionWithDelay>(a => 
                                                                                                {
                                                                                                    a.UnitEnter = Helpers.CreateActionList(Apply_Staggered_Buff_Saved);
                                                                                                    a.Round = Helpers.CreateActionList(Apply_Staggered_Buff_Saved);
                                                                                                    a.DelayRound = Helpers.CreateActionList(Apply_Staggered_Buff_Saved);
                                                                                                    a.IsNotFirstRoundDelay = false;
                                                                                                });


            var Choking_Bomb_Area = Smoke_Bomb_Area.CreateCopy(ToDContext, "NinjaTrickChokingBombArea", bp =>
            {
                bp.AddComponent(Choking_Bomb_Action);
            });

            var Choking_Bomb_Spawn_Action = HlEX.CreateContextActionSpawnAreaEffect(Choking_Bomb_Area.ToReference<BlueprintAbilityAreaEffectReference>(),
                                        HlEX.CreateContextDuration(1, DurationRate.Minutes));

            var Choking_Bomb_Ability = Smoke_Bomb_Ability.CreateCopy(ToDContext, "NinjaTrickChokingBombAbility", bp =>
            {
                bp.SetName(ToDContext, "Choking Bomb");
                bp.SetDescription(ToDContext, "Whenever the character throws a smoke bomb, all living creatures in the resulting cloud must make a Fortitude save or become staggered by the choking black smoke for 1d4 rounds. The DC of this saving throw is equal to 10 + 1/2 the character relevant class’ level + the character’s Charisma modifier.");
                bp.m_Icon = ChokingBombIcon;
                bp.ReplaceComponents<AbilityEffectRunAction>(HlEX.CreateRunActions(Choking_Bomb_Spawn_Action));
            });

            var Choking_Bomb_Feature = HlEX.ConvertAbilityToFeature(Choking_Bomb_Ability, "", "", "Feature", "Ability", false);

            Choking_Bomb_Feature.AddPrerequisiteFeature(Smoke_Bomb_Feature);

            ToDContext.Logger.LogPatch("Created Choking Bomb Trick ninja trick.", Choking_Bomb_Feature);

            #endregion

            #region |---------------------------------------------------------------|  Create Blinding Bomb |------------------------------------------------------------------|

            var Apply_Blinded_Buff = HlEX.CreateContextActionConditionalSaved(null, HlEX.CreateContextActionApplyBuff(Blinded_Buff, HlEX.CreateContextDuration(0, DurationRate.Rounds, DiceType.D4, 1), false));
            var Apply_Blinded_Buff_Saved = HlEX.CreateConditional(HlEX.CreateContextConditionHasFact(Blinded_Buff), HlEX.CreateContextActionSavingThrow(SavingThrowType.Fortitude, Helpers.CreateActionList(Apply_Blinded_Buff)));
            var Blinding_Bomb_Action = Helpers.Create<AbilityAreaEffectRunActionWithDelay>(a =>
            {
                a.UnitEnter = Helpers.CreateActionList(Apply_Blinded_Buff_Saved);
                a.Round = Helpers.CreateActionList(Apply_Blinded_Buff_Saved);
                a.DelayRound = Helpers.CreateActionList(Apply_Blinded_Buff_Saved);
                a.IsNotFirstRoundDelay = false;
            });

            var Blinding_Bomb_Area = Smoke_Bomb_Area.CreateCopy(ToDContext, "NinjaTrickChokingBombArea", bp =>
            {
                bp.AddComponent(Blinding_Bomb_Action);
            });

            var Blinding_Bomb_Spawn_Action = HlEX.CreateContextActionSpawnAreaEffect(Blinding_Bomb_Area.ToReference<BlueprintAbilityAreaEffectReference>(),
                                        HlEX.CreateContextDuration(1, DurationRate.Minutes));

            var Blinding_Bomb_Ability = Smoke_Bomb_Ability.CreateCopy(ToDContext, "NinjaTrickBlindingBombAbility", bp =>
            {
                bp.SetName(ToDContext, "Blinding Bomb");
                bp.SetDescription(ToDContext, "Whenever the character throws a smoke bomb, all living creatures in the cloud must make a Fortitude save or be blinded by the black smoke for 1d4 rounds. The DC of this saving throw is equal to 10 + 1/2 the character relevant class’ level + the character’s Charisma modifier.");
                bp.m_Icon = BlindingBombIcon;
                bp.ReplaceComponents<AbilityEffectRunAction>(HlEX.CreateRunActions(Blinding_Bomb_Spawn_Action));
            });

            var Blinding_Bomb_Feature = HlEX.ConvertAbilityToFeature(Blinding_Bomb_Ability, "", "", "Feature", "Ability", false);

            Blinding_Bomb_Feature.AddPrerequisiteFeature(Smoke_Bomb_Feature);
            Blinding_Bomb_Feature.AddPrerequisiteFeature(Choking_Bomb_Feature);

            ToDContext.Logger.LogPatch("Created Blinding Bomb Trick ninja trick.", Blinding_Bomb_Feature);

            #endregion




        }
    }
}
