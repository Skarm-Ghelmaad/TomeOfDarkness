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
using Kingmaker.UI.MVVM._ConsoleView.Vendor;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;

// Holic75_PT
// This part is a port from Holic, but owes a lot to Vek17's extraordinary CustomMechanics code and to Kurofivne's help with the suggestions on how to set the accessibilty parameters!!
namespace TomeOfDarkness.NewUnitParts
{
    public abstract class AdditiveUnitPart: OldStyleUnitPart
    {
        public virtual void AddUnitPartFlag()
        {
            this.UnitPartFlag.Retain();
        }


        public virtual void RemoveUnitPartFlag()
        {
            this.UnitPartFlag.Release();
        }

        public virtual void ClearUnitPartFlag()
        {
            this.UnitPartFlag.ReleaseAll();
        }

        public abstract CountableFlag UnitPartFlag
        {
            get;
        }

    }
}
