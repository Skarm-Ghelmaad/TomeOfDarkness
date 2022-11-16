using HarmonyLib;
using Kingmaker.Utility;
using Kingmaker;
using System;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using UnityModManagerNet;
using TomeOfDarkness.ModLogic;
using static UnityModManagerNet.UnityModManager;

namespace TomeOfDarkness
{
    static class Main
    {

        public static bool IsInGame => Game.Instance.Player?.Party.Any() ?? false;
        public static bool Enabled;
        public static ModContextTomeOfDarkness ToDContext;


        static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                Enabled = true;

                var harmony = new Harmony(modEntry.Info.Id);
                ToDContext = new ModContextTomeOfDarkness(modEntry);
                ToDContext.ModEntry.OnSaveGUI = OnSaveGUI;
                ToDContext.ModEntry.OnGUI = UMMSettingsUI.OnGUI;

#if DEBUG
                ToDContext.Debug = true;
                ToDContext.Blueprints.Debug = true;
#endif
                harmony.PatchAll();
                Logger.Log("Finished patching.");

                PostPatchInitializer.Initialize(ToDContext);
                return true;

            }

            catch (Exception e)
            {
                Main.ToDContext.Logger.LogError(e, e.Message);
                return false;
            }
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            ToDContext.SaveAllSettings();
        }



    }
}
