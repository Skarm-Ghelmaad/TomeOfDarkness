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
using TomeOfDarkness.MechanicsChanges;
using TomeOfDarkness.NewContent.Features;
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

                // Basic setup needed for new content

                KiResourceChanges.ConfigureBasicKiResourceChanges();        //Changes to the Monk's Ki Pool feature to make it universal. 
                MartialArtsTraining.ConfigureMonkMartialArtsTraining();     //Changes to the Monk's unarmed attack feature to make it universal. 
                UniversalPoisonUse.ConfigureUniversalPoisonUse();           //Changes to the Assassin's Poison Use feature to make it universal.


                //New archetypes
                Archetypes.Ninja.ConfigureNinjaArchetype();

                //New spells
                Spells.FxTester.ConfigureFxTester();
                Spells.ObscuringMist.ConfigureObscuringMist();
                Spells.Sanctuary.ConfigureSanctuary();

            }


       }
    }
}
