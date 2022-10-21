using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.ModLogic;
using UnityModManagerNet;

namespace TomeOfDarkness.ModLogic
{
    class ModContextTomeOfDarkness : ModContextBase
    {

        public Config.NewContent NewContent;

        public ModContextTomeOfDarkness(UnityModManager.ModEntry modEntry) : base(modEntry)
        {
#if DEBUG   
            Debug = true;
#endif
            LoadAllSettings();

        }

        public override void LoadAllSettings()
        {
            LoadBlueprints("TomeOfDarkness.Config", this);
            LoadSettings("NewContent.json", "TomeOfDarkness.Config", ref NewContent);

            LoadLocalization("TomeOfDarkness.Localization");

        }

        public override void AfterBlueprintCachePatches()
        {
            base.AfterBlueprintCachePatches();
            if (Debug)
            {
                Blueprints.RemoveUnused();
                SaveSettings(BlueprintsFile, Blueprints);
                ModLocalizationPack.RemoveUnused();
                SaveLocalization(ModLocalizationPack);
            }
        }
        public override void SaveAllSettings()
        {
            base.SaveAllSettings();
            SaveSettings("NewContent.json", NewContent);
        }








    }
}
