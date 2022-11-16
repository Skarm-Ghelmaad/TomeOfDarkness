using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using HlEX = TomeOfDarkness.Utilities.HelpersExtension;

namespace TomeOfDarkness.NewContent.Features
{
    // Just like in Holic75's version of the Ninja Light Steps is implemented as a weakened version of Monk's Abundant Step, which takes a full-round action to activate.
    internal class LightSteps
    {
        private static BlueprintCharacterClassReference[] RogueClassArray()
        {
            return new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };
        }

        public static void ConfigureLightSteps()
        {



            var LightStepsIcon = BlueprintTools.GetBlueprint<BlueprintAbility>("f3c0b267dd17a2a45a40805e31fe3cd1").Icon; // LightSteps uses Feather Steps icon

            var MonkAbundantStepAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("336a841704b7e2341b51f89fc9491f54"); // Abundant Step is used as reference.
            var MonkAbundantStepCasterDisappearProjectile = BlueprintTools.GetBlueprint<BlueprintProjectile>("b9a3f1855ab08bf42a8b119818bcc6dd");
            var MonkAbundantStepCasterAppearProjectile = BlueprintTools.GetBlueprint<BlueprintProjectile>("4125f30c999bddc4492bf91d73c4cf64");
            var MonkAbundantSideDisappearProjectile = BlueprintTools.GetBlueprint<BlueprintProjectile>("cdaff4fd8665656409ddffe42fbc07c1");
            var MonkAbundantSideAppearProjectile = BlueprintTools.GetBlueprint<BlueprintProjectile>("6c72207ef86803543b4b13352bcc5cf6");


            var LightStepsAbility = Helpers.CreateBlueprint<BlueprintAbility>(ToDContext, "NinjaLightStepsAbility", bp => {
                bp.SetName(ToDContext, "Light Steps");
                bp.SetDescription(ToDContext, "At 6th level, a ninja learns to move while barely touching the surface underneath her. As a full-round action, she can move to any location within medium range, ignoring difficult terrain. While moving in this way, any surface will support her, no matter how much she weighs. This allows her to move across water, lava, or even the thinnest tree branches. She must end her move on a surface that can support her normally. She cannot move across air in this way, nor can she walk up walls or other vertical surfaces. When moving in this way, she does not take damage from surfaces or hazards that react to being touched, such as lava or caltrops, nor does she need to make Mobility checks to avoid falling on slippery or rough surfaces. Finally, when using light steps, the ninja ignores any mechanical traps that use a location-based trigger.");
                bp.m_Icon = LightStepsIcon;
                bp.AddComponent<AbilityCustomDimensionDoor>(c => {
                    c.Radius = 0.Feet();
                    c.PortalFromPrefab = HlEX.CreatePrefabLink("1886751171485164");
                    c.PortalToPrefab = HlEX.CreatePrefabLink("1886751171485164");
                    c.CasterDisappearFx = HlEX.CreatePrefabLink("ccd3a2dcada23c145b232501d105c55d"); // Uses the Invisibility Fx
                    c.SideDisappearFx = HlEX.CreatePrefabLink("ccd3a2dcada23c145b232501d105c55d"); // Uses the Invisibility Fx
                    c.m_CasterDisappearProjectile = MonkAbundantStepCasterDisappearProjectile.ToReference<BlueprintProjectileReference>();
                    c.m_CasterAppearProjectile = MonkAbundantStepCasterAppearProjectile.ToReference<BlueprintProjectileReference>();
                    c.m_SideDisappearProjectile = MonkAbundantSideDisappearProjectile.ToReference<BlueprintProjectileReference>();
                    c.m_SideAppearProjectile = MonkAbundantSideAppearProjectile.ToReference<BlueprintProjectileReference>();
                    c.m_CameraShouldFollow = false;
                });
                bp.Type = AbilityType.Supernatural;
                bp.Range = AbilityRange.DoubleMove;
                bp.IgnoreMinimalRangeLimit = false;
                bp.ShowNameForVariant = false;
                bp.CanTargetPoint = true;
                bp.CanTargetEnemies = false;
                bp.CanTargetFriends = false;
                bp.SpellResistance = false;
                bp.Hidden = false;
                bp.NeedEquipWeapons = false;
                bp.NotOffensive = false;
                bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = UnitCommand.CommandType.Move;
                bp.m_IsFullRoundAction = true;
                bp.AvailableMetamagic = Metamagic.Heighten | Metamagic.Quicken;
                bp.ResourceAssetIds = new string[0];


            });

            var LightStepsFeature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "NinjaLightStepsFeature", bp => {
                bp.SetName(ToDContext, "Light Steps");
                bp.SetDescription(ToDContext, "At 6th level, a ninja learns to move while barely touching the surface underneath her. As a full-round action, she can move to any location within medium range, ignoring difficult terrain. While moving in this way, any surface will support her, no matter how much she weighs. This allows her to move across water, lava, or even the thinnest tree branches. She must end her move on a surface that can support her normally. She cannot move across air in this way, nor can she walk up walls or other vertical surfaces. When moving in this way, she does not take damage from surfaces or hazards that react to being touched, such as lava or caltrops, nor does she need to make Mobility checks to avoid falling on slippery or rough surfaces. Finally, when using light steps, the ninja ignores any mechanical traps that use a location-based trigger.");
                bp.m_Icon = LightStepsIcon;
                bp.AddComponent<AddFacts>(c => {
                    c.m_Facts = new BlueprintUnitFactReference[] {
                        LightStepsAbility.ToReference<BlueprintUnitFactReference>(),
                    };


                });
            });

            ToDContext.Logger.LogPatch("Created Light Steps.", LightStepsFeature);


        }
    }
}
