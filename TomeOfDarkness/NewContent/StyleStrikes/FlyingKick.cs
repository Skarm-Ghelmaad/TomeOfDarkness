using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using static TomeOfDarkness.Main;
using TomeOfDarkness.Utilities;
using TabletopTweaks.Core.Utilities;

namespace TomeOfDarkness.NewContent.StyleStrikes
{
    internal class FlyingKick
    {

        public static void ConfigureFlyingKick()
        {
            var FlyingKickIcon = AssetLoader.LoadInternal(ToDContext, folder: "assets/icons", file: "FlyingKick.png");

            var FlyingKickBuff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "MonkFlyingKickBuff", bp => {
                bp.SetName(ToDContext, "");
                bp.SetDescription(ToDContext, "");
                bp.m_Icon = null;
                bp.AddComponent<AddMechanicsFeature>(c => {
                    c.m_Feature = Kingmaker.UnitLogic.FactLogic.AddMechanicsFeature.MechanicsFeatureType.Pounce;
                });
                bp.m_Flags = BlueprintBuff.Flags.StayOnDeath | BlueprintBuff.Flags.HiddenInUi;
            });

            var FlyingKickToggle = HelpersExtension.ConvertBuffToActivatableAbility(FlyingKickBuff, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free, false, "ToggleAbility", "Buff");

            FlyingKickToggle.Group = Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbilityGroup.StyleStrike;
            FlyingKickToggle.SetName(ToDContext, "Flying Kick");
            FlyingKickToggle.SetDescription(ToDContext, "The monk leaps through the air to strike a foe with a kick. The monk can make a full-attack at the end of charge action.");
            FlyingKickToggle.m_Icon = FlyingKickIcon;


            var FlyingKickFeature = HelpersExtension.ConvertActivatableAbilityToFeature(FlyingKickToggle, "", "", "Feature", "ToggleAbility", false);

            FeatToolsExtension.AddAsStyleStrike(FlyingKickFeature);

            ToDContext.Logger.LogPatch("Created Flying Kick style strike.", FlyingKickFeature);

        }

    }
}
