using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Parts;
using TomeOfDarkness.NewUnitParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.NewUnitParts;


namespace TomeOfDarkness.NewComponents
{
    [TypeId("04A76A01B243489D98F1635A0DB8D1C2")]
    public class ConsiderWeaponCategoriesAsLightWeapon : ConsiderWeaponCategoryAsLightWeapon
    {
        public WeaponCategory[] Categories;

        public override void OnTurnOn()
        {
            base.OnTurnOn();

            var unit_part_damage_grace = base.Owner.Ensure<UnitPartDamageGrace>();

            for (int i = 0; i < Categories.Length; i++)
            {
                Category = Categories[i];

                if (!unit_part_damage_grace.HasEntry((WeaponCategory)Category))
                {
                    unit_part_damage_grace.AddEntry(Category, base.Fact);
                }

            }



        }

        public override void OnTurnOff()
        {
            base.OnTurnOff();

            var unit_part_damage_grace = base.Owner.Ensure<UnitPartDamageGrace>();

            for (int i = 0; i < Categories.Length; i++)
            {
                Category = Categories[i];

                if (unit_part_damage_grace.HasEntry((WeaponCategory)Category))
                {
                    unit_part_damage_grace.RemoveEntry(base.Fact);
                }

            }


            base.Owner.Ensure<UnitPartDamageGrace>().RemoveEntry(base.Fact);
        }
    }
}
