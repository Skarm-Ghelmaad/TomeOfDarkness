using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using TomeOfDarkness.NewUnitParts;
using TabletopTweaks.Core.NewUnitParts;
using static TabletopTweaks.Core.NewUnitParts.UnitPartCustomMechanicsFeatures;
using static TomeOfDarkness.NewUnitParts.UnitPartCustomMechanicsFeaturesToD;

namespace TomeOfDarkness.NewComponents
{
    [TypeId("8295DE67810E4705A10FEFC48DB61244")]
    public abstract class ConsiderWeaponCategoryAsLightWeapon : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber
    {
        // This is used for the Flurry Of Stars ninja trick.
        // Holic75_PT

        public override void OnTurnOn()
        {
            base.Owner.Ensure<UnitPartCustomMechanicsFeatures>().AddMechanicsFeature(UseWeaponAsLightWeapon);
            WeaponCategory? category = base.Param?.WeaponCategory ?? Category;

        }

        public override void OnTurnOff()
        {
            base.Owner.Ensure<UnitPartCustomMechanicsFeatures>().RemoveMechanicsFeature(UseWeaponAsLightWeapon);

        }

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {

        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
        }

        public WeaponCategory? Category;
    }
}
