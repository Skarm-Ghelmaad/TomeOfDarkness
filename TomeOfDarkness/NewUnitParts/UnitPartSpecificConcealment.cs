using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using Kingmaker.Designers.Mechanics.Collections;
using Kingmaker.UnitLogic.Buffs;
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Root;
using Kingmaker.Controllers;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.Settings;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics;
using Newtonsoft.Json;
using Owlcat.Runtime.Core.Utils;
using TurnBased.Controllers;
using TomeOfDarkness.NewComponents;
using TabletopTweaks.Core.Config;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Parts;



namespace TomeOfDarkness.NewUnitParts
{
    // Holic75_PT
    public class UnitPartSpecificConcealment: AdditiveUnitPart
    {

        public override CountableFlag UnitPartFlag
        {
            get
            { 
                return SpecificConcealment;
            }
        }

        public void UpdateSpecificConcealmentBuffTracker()
        {
                List<BlueprintBuff> Tracker = new List<BlueprintBuff>();

                foreach (var buff in this.Owner.Buffs)
                {
                    if (buff.Blueprint.GetComponent<SpecificConcealmentBase>() != null)
                    {

                        Tracker.Add(buff.Blueprint);

                    }

                }

            this.SpecificConcealmentBuffTracker = Tracker.ToArray();
        }


        public UnitPartConcealment.ConcealmentEntry[] GetConcealments(UnitEntityData attacker)
        {
            var Concealments = new List<UnitPartConcealment.ConcealmentEntry>();

            if (this.SpecificConcealmentBuffTracker.Any())
            {
                foreach (var b in this.SpecificConcealmentBuffTracker)
                {
                    bool Works_On = false;

                    b.CallComponents<SpecificConcealmentBase>(s => Works_On = s.WorksAgainst(attacker));

                    if (Works_On)
                    {
                        b.CallComponents<SpecificConcealmentBase>(s => Concealments.Add(s.CreateConcealmentEntry()));
                    }
                }
            }

            return Concealments.ToArray();
        }


        public CountableFlag SpecificConcealment = new CountableFlag();
        public BlueprintBuff[] SpecificConcealmentBuffTracker = new BlueprintBuff[0];


    }
}
