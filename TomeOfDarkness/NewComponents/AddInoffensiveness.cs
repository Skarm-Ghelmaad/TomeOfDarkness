//using Kingmaker.UnitLogic.Buffs.Components;
//using Kingmaker.Designers.Mechanics.Facts;
//using Kingmaker.EntitySystem.Entities;
//using Kingmaker.Enums;
//using Kingmaker.Utility;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TomeOfDarkness.NewUnitParts;
//using Kingmaker.UnitLogic.Buffs.Blueprints;
//using Kingmaker.UnitLogic.Parts;
//using Kingmaker.Blueprints;
//using static Kingmaker.UnitLogic.Parts.UnitPartConcealment;
//using Kingmaker.Blueprints.Facts;
//using Kingmaker.Blueprints.JsonSystem;

//namespace TomeOfDarkness.NewComponents
//{
//    public class AddInoffensiveness : UnitBuffComponentDelegate
//    {

//        public override void OnTurnOn()
//        {
//            this.Owner.Ensure<UnitPartInoffensiveness>().AddUnitPartFlag();

//        }

//        public override void OnTurnOff()
//        {
//            this.Owner.Ensure<UnitPartInoffensiveness>().RemoveUnitPartFlag();

//        }

//    }
//}
