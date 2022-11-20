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

        public List<UnitEntityData> Marked_Units = new List<UnitEntityData>();

        public int Stored_DC = 0;

        public int Stored_PropertyThreshold = 0;

        public int Stored_CustomPropertyThreshold = 0;

    }
}
