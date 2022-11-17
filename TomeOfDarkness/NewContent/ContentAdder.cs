using HarmonyLib;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.JsonSystem;
using TabletopTweaks.Core.Utilities;
using TomeOfDarkness.MechanicsChanges;
using TomeOfDarkness.NewContent.Feat;
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
                UniversalAssassination.ConfigureUniversalAssassination();   //Changes to the Assassin's Death Attack and Executioner's Assassinate features to make it universal.


                //New spells (added first to be used for archetypes and other things)
                Spells.FxTester.ConfigureFxTester();
                Spells.ObscuringMist.ConfigureObscuringMist();
                Spells.Sanctuary.ConfigureSanctuary();

                //New archetypes
                Archetypes.Ninja.ConfigureNinjaArchetype();


                //New slayer talents
                SlayerTalents.SlayerAssassinate.ConfigureSlayerAssassinate();

                //New feats
                ExtraNinjaTrick.ConfigureExtraNinjaTrick();
                ExtraPoisonUse.ConfigureExtraPoisonUse();




            }


        }
    }
}
