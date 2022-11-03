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

namespace TomeOfDarkness.NewContent.Features
{
    internal class NoTrace
    {

        public static void ConfigureNoTrace()
        {
            var NoTraceIcon = BlueprintTools.GetBlueprint<BlueprintFeature>("97a6aa2b64dd21a4fac67658a91067d7").Icon; // No Trace uses Fast Stealth icon

            var NoTraceFeature = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "NinjaNoTraceFeature", bp => {
                bp.SetName(ToDContext, "No Trace");
                bp.SetDescription(ToDContext, "At 3rd level, a ninja gains a +1 bonus on Stealth checks. This bonus increases by 1 every 3 ninja levels thereafter.");
                bp.m_Icon = NoTraceIcon;
                bp.AddComponent<AddContextStatBonus>(c => {
                    c.Stat = StatType.SkillStealth;
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = new ContextValue() { ValueType = ContextValueType.Rank };
                    c.Value.ValueRank = AbilityRankType.Default;
                    c.Multiplier = 1;
                });
                bp.IsClassFeature = true;
                bp.Ranks = 6;
                bp.ReapplyOnLevelUp = true;
                bp.AddComponent<ContextRankConfig>(c => {
                    c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
                    c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
                });
            });


            ToDContext.Logger.LogPatch("Created No Trace.", NoTraceFeature);

        }
    }
}
