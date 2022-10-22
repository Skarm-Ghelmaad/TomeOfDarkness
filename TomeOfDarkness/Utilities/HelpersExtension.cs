using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Kingmaker.View;
using Kingmaker.ResourceLinks;

namespace TomeOfDarkness.Utilities
{
    public static class HelpersExtension
    {

        #region |----------------------------------------------------------------| ResourceLinks Creators |----------------------------------------------------------------|

              

        public static PrefabLink CreatePrefabLink(string asset_id) // Holic75_SC 
        {
            var link = new PrefabLink();
            link.AssetId = asset_id;
            return link;
        }

        public static ProjectileLink CreateProjectileLink(string asset_id) 
        {
            var link = new ProjectileLink();
            link.AssetId = asset_id;
            return link;
        }

        public static UnitViewLink CreateUnitViewLink(string asset_id) // Holic75_SC 
        {
            var link = new UnitViewLink();
            link.AssetId = asset_id;
            return link;
        }

        #endregion


    }
}
