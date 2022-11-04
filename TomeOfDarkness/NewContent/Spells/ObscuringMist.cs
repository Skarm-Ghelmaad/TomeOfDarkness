using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using System.Linq;
using HlEX = TomeOfDarkness.Utilities.HelpersExtension;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using System.Web;
using static UnityModManagerNet.UnityModManager;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;


namespace TomeOfDarkness.NewContent.Spells
{
    internal class ObscuringMist
    {
        public static void ConfigureObscuringMist()
        {

            var ObscuringMistIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_ObscuringMist.png");
            var Icon_ScrollOfObscuringMist = AssetLoader.LoadInternal(ToDContext, folder: "Equipment", file: "Icon_ScrollObscuringMist.png");
            var Stinking_Cloud_Spell = BlueprintTools.GetBlueprint<BlueprintAbility>("68a9e6d7256f1354289a39003a46d826");
            var Stinking_Cloud_Area = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("aa2e0a0fe89693f4e9205fd52c5ba3e5");
            var Stinking_Cloud_Spawn_Area_Context_Value = Stinking_Cloud_Spell.GetComponent<AbilityEffectRunAction>().Actions.Actions.OfType<ContextActionSpawnAreaEffect>().FirstOrDefault().DurationValue.BonusValue;
            var Mind_Fog_Area = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("fe5102d734382b74586f56980086e5e8");
            var Stinking_Cloud_Fx_ID = Stinking_Cloud_Area.Fx.AssetId;
            string ObscuringMistName = "Obscuring Mist";
            string ObscuringMistDescription = "A misty vapor arises around you. It is stationary. The vapor obscures all sight, including darkvision, beyond 5 feet. A creature 5 feet away has concealment (attacks have a 20% miss chance). Creatures farther away have total concealment (50% miss chance, and the attacker cannot use sight to locate the target).";

            var air_domain_progression = BlueprintTools.GetBlueprint<BlueprintProgression>("750bfcd133cd52f42acbd4f7bc9cc365");
            var darkness_domain_progression = BlueprintTools.GetBlueprint<BlueprintProgression>("1e1b4128290b11a41ba55280ede90d7d");
            var weather_domain_progression = BlueprintTools.GetBlueprint<BlueprintProgression>("c18a821ee662db0439fb873165da25be");
            var water_domain_progression = BlueprintTools.GetBlueprint<BlueprintProgression>("e63d9133cebf2cf4788e61432a939084");

            var Obscuring_Mist_Area = Mind_Fog_Area.CreateCopy(ToDContext, "ObscuringMistArea", bp => {
                bp.Fx = HlEX.CreatePrefabLink(Stinking_Cloud_Fx_ID);
                bp.Size = 20.Feet();
                bp.SpellResistance = false;
            });

            var Obscuring_Mist_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "ObscuringMistBuff", bp => {
                bp.SetName(ToDContext, ObscuringMistName);
                bp.SetDescription(ToDContext, ObscuringMistDescription);
                bp.m_Icon = ObscuringMistIcon;
                bp.FxOnStart = new PrefabLink();
                bp.FxOnRemove = new PrefabLink();
                bp.AddComponent(Helpers.Create<AddConcealment>(c => {
                    c.CheckDistance = false;
                    c.Concealment = Concealment.Partial;
                    c.Descriptor = ConcealmentDescriptor.Fog;
                }));
                bp.AddComponent(Helpers.Create<AddConcealment>(c => {
                    c.CheckDistance = true;
                    c.DistanceGreater = 5.Feet();
                    c.Concealment = Concealment.Total;
                    c.Descriptor = ConcealmentDescriptor.Fog;
                }));
            });

            foreach (var cmp in Obscuring_Mist_Buff.GetComponents<AddConcealment>().ToArray())
            {
                Obscuring_Mist_Buff.AddComponent(HlEX.CreateOutgoingConcealment(cmp));
            }

            Obscuring_Mist_Area.ComponentsArray = new BlueprintComponent[] { Helpers.Create<AbilityAreaEffectBuff>(a => { a.m_Buff = Obscuring_Mist_Buff.ToReference<BlueprintBuffReference>(); a.Condition = HlEX.CreateConditionsCheckerAnd(); }) };

            var Obscuring_Mist_Spawn_Action = HlEX.CreateContextActionSpawnAreaEffect(Obscuring_Mist_Area.ToReference<BlueprintAbilityAreaEffectReference>(),
                                                                HlEX.CreateContextDuration(Stinking_Cloud_Spawn_Area_Context_Value, DurationRate.Minutes));

            Obscuring_Mist_Spawn_Action.DurationValue.m_IsExtendable = true;

            var Obscuring_Mist_Spell = Stinking_Cloud_Spell.CreateCopy(ToDContext, "ObscuringMistAbility", bp => {
                bp.SetName(ToDContext, ObscuringMistName);
                bp.SetDescription(ToDContext, ObscuringMistDescription);
                bp.m_Icon = ObscuringMistIcon;
                bp.SpellResistance = false;
                bp.RemoveComponents<SpellListComponent>();
                bp.RemoveComponents<SpellDescriptorComponent>();
                bp.GetComponent<AbilityAoERadius>().TemporaryContext(c => {
                    c.m_Radius = 20.Feet();
                });
                bp.AvailableMetamagic = Metamagic.Heighten | Metamagic.Quicken | Metamagic.Reach | Metamagic.Extend;
                bp.LocalizedDuration = Helpers.CreateString(ToDContext, $"{bp.name}.Duration", "1 minute/level");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.ReplaceComponents<AbilityEffectRunAction>(HlEX.CreateRunActions(Obscuring_Mist_Spawn_Action));
                bp.ReplaceComponents<CraftInfoComponent>(Helpers.Create<CraftInfoComponent>(c => { c.OwnerBlueprint = bp; c.SpellType = CraftSpellType.Debuff; c.SavingThrow = CraftSavingThrow.None; c.AOEType = CraftAOE.AOE; }));
            });

            var ObscuringMistScroll = ItemTools.CreateScroll(ToDContext, "ScrollOfObscuringMist", Icon_ScrollOfObscuringMist, Obscuring_Mist_Spell, 1, 1);

            if (ToDContext.NewContent.Spells.IsDisabled("ObscuringMist")) { return; }

            VenderTools.AddScrollToLeveledVenders(ObscuringMistScroll);

            Obscuring_Mist_Spell.AddToSpellList(SpellTools.SpellList.BloodragerSpellList, 1);
            Obscuring_Mist_Spell.AddToSpellList(SpellTools.SpellList.ClericSpellList, 1);
            Obscuring_Mist_Spell.AddToSpellList(SpellTools.SpellList.DruidSpellList, 1);
            Obscuring_Mist_Spell.AddToSpellList(SpellTools.SpellList.MagusSpellList, 1);
            Obscuring_Mist_Spell.AddToSpellList(SpellTools.SpellList.RangerSpellList, 1);
            Obscuring_Mist_Spell.AddToSpellList(SpellTools.SpellList.ShamanSpelllist, 1);
            Obscuring_Mist_Spell.AddToSpellList(SpellTools.SpellList.WizardSpellList, 1);
            Obscuring_Mist_Spell.AddToSpellList(SpellTools.SpellList.WitchSpellList, 1);

            HlEX.ReplaceDomainSpell(air_domain_progression, Obscuring_Mist_Spell, 1);
            HlEX.ReplaceDomainSpell(darkness_domain_progression, Obscuring_Mist_Spell, 1);
            HlEX.ReplaceDomainSpell(weather_domain_progression, Obscuring_Mist_Spell, 1);
            HlEX.ReplaceDomainSpell(water_domain_progression, Obscuring_Mist_Spell, 1);

            ToDContext.Logger.LogPatch("Created Obscuring Mist spell.", Obscuring_Mist_Spell);
            ToDContext.Logger.LogPatch("Created Obscuring Mist scroll and added to vendors.", ObscuringMistScroll);
            ToDContext.Logger.LogPatch("Added Obscuring Mist spell to Air Domain.", air_domain_progression);
            ToDContext.Logger.LogPatch("Added Obscuring Mist spell to Darkness Domain.", darkness_domain_progression);
            ToDContext.Logger.LogPatch("Added Obscuring Mist spell to Weather Domain.", weather_domain_progression);
            ToDContext.Logger.LogPatch("Added Obscuring Mist spell to Water Domain.", water_domain_progression);

        }
    }
}
