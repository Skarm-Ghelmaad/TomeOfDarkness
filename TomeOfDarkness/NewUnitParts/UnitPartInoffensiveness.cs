using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Utility;
using System.Collections.Generic;
using TomeOfDarkness.NewComponents;

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

        // Inoffensiveness is a unique unit part which tracks and checks the requirements to attack the owner associated
        // with any effect that have this component, so it is necessary to check whether (or not) there are AddInoffensiveness components left
        // before deactivating the UnitPartInoffensiveness. 

        public bool  IsInoffensivenessBuffTrackerEmpty
        {
            get
            {
                return !this.InoffensivenessBuffTracker.Any();
            }

        }


        public void UpdateInoffensivenessBuffTracker()
        {
            List<BlueprintBuff> Tracker = new List<BlueprintBuff>();

            foreach (var buff in this.Owner.Buffs)
            {
                if (buff.Blueprint.GetComponent<AddInoffensiveness>() != null)
                {

                    Tracker.Add(buff.Blueprint);

                }

            }

            this.InoffensivenessBuffTracker = Tracker;
        }

        public bool CanBeAttackedBy(UnitEntityData unit)
        {
            if (!this.InoffensivenessBuffTracker.Any())
            {
                return true;
            }

            foreach (var b in this.InoffensivenessBuffTracker)
            {
                bool Can_Be_Attacked_By = false;

                b.CallComponents<AddInoffensiveness>(c => Can_Be_Attacked_By = c.CanBeAttackedBy(unit));

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

        public List<BlueprintBuff> InoffensivenessBuffTracker = new List<BlueprintBuff>();

    }
}


