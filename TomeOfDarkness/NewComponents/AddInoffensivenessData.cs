using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomeOfDarkness.NewComponents
{
    public class AddInoffensivenessData
    {
        public List<UnitEntityData> Cannot_Attack = new List<UnitEntityData>();

        public List<UnitEntityData> Can_Attack = new List<UnitEntityData>();

        public List<UnitEntityData> Marked_Units_Minor = new List<UnitEntityData>();

        public List<UnitEntityData> Marked_Units_Moderate = new List<UnitEntityData>();

        public List<UnitEntityData> Marked_Units_Major = new List<UnitEntityData>();

        public int Stored_DC = 0;

    }
}
