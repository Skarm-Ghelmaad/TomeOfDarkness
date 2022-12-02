using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using Owlcat.Runtime.Core.Logging;
using System;
using System.Collections.Generic;
using TomeOfDarkness.NewComponents;
using static TomeOfDarkness.Main;

namespace TomeOfDarkness.NewUnitParts
{
    // The original UnitPart was called "UnitPartSanctuary" but I wanted to implement this class in a way that allowed for a more versatile 
    // use than employing it for the "Sanctuary" spell, so I have given it a more generic name.
    // Note that, in order to retain vanilla coding convention, I've used the concept of "Caster" and "Target", which in this context have the following meaning:

    public class UnitPartInoffensiveness : AdditiveUnitPart
    {
        public override CountableFlag UnitPartFlag
        {
            get
            {
                return Inoffensiveness;
            }
        }



        public void AddEntry(EntityFact fact, AddInoffensiveness component)
        {
            if (this.InoffensivenessEntries.HasItem((UnitPartInoffensiveness.InoffensivenessEntry i) => i.Fact == fact && i.Component == component))
            {
                string messageFormat = "UnitPartInoffensiveness.AddEntry: item already exists (fact: {0}, component: {1})";
                object[] array = new object[2];
                array[0] = fact;
                int num = 1;
                AddInoffensiveness addEffectInoffensiveness = component.Or(null);
                array[num] = ((addEffectInoffensiveness != null) ? addEffectInoffensiveness.name : null);
                string error_message = String.Format(messageFormat, array[0], array[1]);
                ToDContext.Logger.LogError(error_message);
                return;
            }
            this.InoffensivenessEntries.Add(new UnitPartInoffensiveness.InoffensivenessEntry(fact, component));
        }

        public void RemoveEntry(EntityFact fact, AddInoffensiveness component)
        {
            this.InoffensivenessEntries.RemoveAll((UnitPartInoffensiveness.InoffensivenessEntry i) => i.Fact == fact && i.Component == component);
            if (this.InoffensivenessEntries.Count < 1)
            {
                this.RemoveUnitPartFlag();
            }
        }

        public bool CanBeAttackedBy(UnitEntityData unit)
        {
            if (this.InoffensivenessEntries.Count < 1)
            {
                return true;
            }

            foreach (var entry in this.InoffensivenessEntries)
            {
                bool Can_Be_Attacked_By = false;

                entry.Fact.CallComponents<AddInoffensiveness>(c => Can_Be_Attacked_By = c.CanBeAttackedBy(unit));

                if (Can_Be_Attacked_By)
                {
                    return true;
                }
            }

            return false;
        }

        [HarmonyPatch(typeof(UnitCommand))]
        [HarmonyPatch("CommandTargetUntargetable", MethodType.Normal)]
        static class UnitCommand__CommandTargetUntargetable__Patch
        {
            static void Postfix(EntityDataBase sourceEntity, UnitEntityData targetUnit, RulebookEvent evt, ref bool __result)
            {
                if (__result)
                {
                    return;
                }
                UnitEntityData unit = sourceEntity as UnitEntityData;
                if (unit == null || unit.IsAlly(targetUnit) || evt != null)
                {
                    return;
                }

                __result = !targetUnit.Ensure<UnitPartInoffensiveness>().CanBeAttackedBy(unit);
            }

        }

        public CountableFlag Inoffensiveness = new CountableFlag();

        public readonly List<UnitPartInoffensiveness.InoffensivenessEntry> InoffensivenessEntries = new List<UnitPartInoffensiveness.InoffensivenessEntry>();

        public struct InoffensivenessEntry
        {
            public InoffensivenessEntry(EntityFact fact, AddInoffensiveness component)
            {
                this.Fact = fact;
                this.Component = component;
            }

            public readonly EntityFact Fact;

            public readonly AddInoffensiveness Component;
        }

    }
}


