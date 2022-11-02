using HarmonyLib;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;


namespace TomeOfDarkness.NewContent
{
    class ContentAdder
    {

        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;

            [HarmonyPriority(Priority.First)]
            [HarmonyPostfix]
            static void CreateNewBlueprints()
            {
                var test = BlueprintTools.GetBlueprint<BlueprintSharedVendorTable>("c773973cd73d4cd7aa4ccf3868dfeba9");
                test.TemporaryContext(bp => {
                    bp.SetComponents();
                    ToDContext.Logger.LogPatch(bp);
                });
                if (Initialized) return;
                Initialized = true;
                ToDContext.Logger.LogHeader("Loading New Content");


                //New archetypes
                Archetypes.Ninja.ConfigureNinjaArchetype();

                //New spells
                Spells.FxTester.ConfigureFxTester();
                Spells.ObscuringMist.ConfigureObscuringMist();

            }


       }
    }
}
