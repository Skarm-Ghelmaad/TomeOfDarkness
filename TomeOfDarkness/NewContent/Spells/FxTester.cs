using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.ResourceLinks;

namespace TomeOfDarkness.NewContent.Spells
{
    internal class FxTester
    {
        public static void ConfigureFxTester()
        {
            var FxTester1Icon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_KiPoolCharisma.png");
            var FxTester2Icon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_KiPoolConstitution.png");
            var FxTester3Icon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_KiPoolDexterity.png");
            var FxTester4Icon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_KiPoolIntelligence.png");
            var FxTester5Icon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_KiPoolStrength.png");
            var FxTester6Icon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_KiPoolWisdom.png");

            var Misty_Form_Toggle_Ability = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("84530fbd9c2a45d4b96247a228e54384");
            var Misty_Form_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("8de8e078992d7a4479f3d76e21aa1195");


            var Demon_Teleport_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("b3e8e307811b2a24387c2c9226fb4c10");

            var Demon_Charge_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("1b677ed598d47a048a0f6b4b671b8f84");

            var Waves_of_Fatigue_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("8878d0c46dfbd564e9d5756349d5e439");

            var Ray_of_Enfeebleement_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("450af0402422b0b4980d9c2175869612");

            var Ray_of_Exhaustion_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("8eead52509987034ea9025d60cc05985");

            var Chain_Spear_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("2516409ac6d08764d9dfe7a460251443");

            var Abyssal_Chain_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("3b86de15c18e4b44bb7315fc6c116b4d");

            var Fear_Spell_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("d2aeac47450c76347aebbc02e4f463e0");

            #region |-------------------------------------------| Personal Buff Fx Toggeable Ability |-------------------------------------------|

            var PersonalBuffTester1 = Misty_Form_Buff.CreateCopy(ToDContext, "ShadowyVFXBuff", bp => {
                bp.SetName(ToDContext, "Cloaked in Shadows Fx Buff");
                bp.SetDescription(ToDContext, "Visual effect from Stygian Slayer's Shadowy Mist form.");
                bp.RemoveComponents<AddFacts>();
            });

            //var Ray_of_Enfeebleement_Projectile_ViewFX_ID = Ray_of_Enfeebleement_Ability.GetComponent<AbilityDeliverProjectile>().m_Projectiles[0].Get().View.AssetId;
            //var Ray_of_Enfeebleement_Projectile_CastFX_ID = Ray_of_Enfeebleement_Ability.GetComponent<AbilityDeliverProjectile>().m_Projectiles[0].Get().CastFx.AssetId;

            #region |-------------------------------------------| Create Empty Fx Buff and Basic Buff Toggle |-------------------------------------------|
            var PersonalEmptyBuff = PersonalBuffTester1.CreateCopy(ToDContext, "EmptyVFXBuff", bp =>
            {
                bp.SetName(ToDContext, "Empty Fx Buff");
                bp.SetDescription(ToDContext, "This buff contains nothing and is ready to have a visual effect attached to it.");
                bp.FxOnStart = new PrefabLink();
            });

            //var RoE_ViewFX_Buff = PersonalEmptyBuff.CreateCopy(ToDContext, "RoEViewVFXBuff", bp =>
            //{
            //    bp.SetName(ToDContext, "Ray of Enfeebleement View Fx Buff");
            //    bp.SetDescription(ToDContext, "This buff contains nothing and is ready to have a visual effect attached to it.");
            //});

            //var RoE_CastFX_Buff = PersonalEmptyBuff.CreateCopy(ToDContext, "RoECastVFXBuff", bp =>
            //{
            //    bp.SetName(ToDContext, "Ray of Enfeebleement Cast Fx Buff");
            //    bp.SetDescription(ToDContext, "This buff contains nothing and is ready to have a visual effect attached to it.");
            //});

            var PersonalBuff1_Testing_Ability = Misty_Form_Toggle_Ability.CreateCopy(ToDContext, "PersonalBuff1TestingAbility", bp =>
            {
                bp.SetName(ToDContext, "Personal Buff Fx Testing 1");
                bp.SetDescription(ToDContext, "This toggle allow to activate a personal Fx buff.");
                bp.RemoveComponents<ActivatableAbilityResourceLogic>();
            }); 

            #endregion

            var PersonalBuff2_Testing_Ability = PersonalBuff1_Testing_Ability.CreateCopy(ToDContext, "PersonalBuff2TestingAbility");
            var PersonalBuff3_Testing_Ability = PersonalBuff1_Testing_Ability.CreateCopy(ToDContext, "PersonalBuff3TestingAbility");
            var PersonalBuff4_Testing_Ability = PersonalBuff1_Testing_Ability.CreateCopy(ToDContext, "PersonalBuff4TestingAbility");
            var PersonalBuff5_Testing_Ability = PersonalBuff1_Testing_Ability.CreateCopy(ToDContext, "PersonalBuff5TestingAbility");
            var PersonalBuff6_Testing_Ability = PersonalBuff1_Testing_Ability.CreateCopy(ToDContext, "PersonalBuff6TestingAbility");

            PersonalBuff1_Testing_Ability.m_Icon = FxTester1Icon;
            //PersonalBuff2_Testing_Ability.m_Icon = FxTester2Icon;
            //PersonalBuff3_Testing_Ability.m_Icon = FxTester3Icon;
            //PersonalBuff4_Testing_Ability.m_Icon = FxTester4Icon;
            //PersonalBuff5_Testing_Ability.m_Icon = FxTester5Icon;
            //PersonalBuff6_Testing_Ability.m_Icon = FxTester6Icon;

            PersonalBuff1_Testing_Ability.m_Buff = PersonalBuffTester1.ToReference<BlueprintBuffReference>();



            #endregion

            #region |------------------------------------------------------| Projectile Ability |--------------------------------------------------------|



            #endregion

            #region |--------------------------------------------------| Teleport Fx Ability |---------------------------------------------------|



            #endregion

            if (ToDContext.NewContent.Spells.IsDisabled("FxTester")) { return; }


            //ToDContext.Logger.LogPatch("Added", "x");

        }

    }
}
