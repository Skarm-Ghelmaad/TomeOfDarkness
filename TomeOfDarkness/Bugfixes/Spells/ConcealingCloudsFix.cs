using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;

namespace TomeOfDarkness.Bugfixes.Spells
{
    internal class ConcealingCloudsFix
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;

            static void Postfix()
            {
                if (Initialized) return;
                Initialized = true;
                ToDContext.Logger.LogHeader("Patching cloud spells");

                PatchConcealingClouds();

            }

        }

        static void PatchConcealingClouds()
        {
            if (ToDContext.Fixes.Spells.IsDisabled("ConcealingCloudsFix")) { return; }

            var Obscuring_Mist_Components = BlueprintTools.GetModBlueprint<BlueprintAbilityAreaEffect>(ToDContext, "ObscuringMistArea").ComponentsArray.ToArray();
            var Obscuring_Generic_Component_Array = new BlueprintComponent[0];
            Obscuring_Generic_Component_Array.AppendToArray(Obscuring_Generic_Component_Array);

            PatchStinkingCloud();
            PatchCloudkill();
            PatchMindFog();
            PatchAcidFog();
            PatchPlagueStorm();
            PatchVolcanicStorm();
            PatchHallucinogenicCloud();
            PatchKineticistCloudInfusions();

            void PatchStinkingCloud()
            {

                var Stinking_Cloud_Area = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("aa2e0a0fe89693f4e9205fd52c5ba3e5");
                Stinking_Cloud_Area.AddComponents(Obscuring_Generic_Component_Array);

                ToDContext.Logger.LogPatch("Stinking Cloud patched to provide concealment.", Stinking_Cloud_Area);

            }

            void PatchCloudkill()
            {

                var Cloudkill_Area = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("6df1ac314d4e6e9418e34470b79f90d8");
                Cloudkill_Area.AddComponents(Obscuring_Generic_Component_Array);

                ToDContext.Logger.LogPatch("Cloudkill patched to provide concealment.", Cloudkill_Area);

            }

            void PatchMindFog()
            {

                var Mind_Fog_Area = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("fe5102d734382b74586f56980086e5e8");
                Mind_Fog_Area.AddComponents(Obscuring_Generic_Component_Array);

                ToDContext.Logger.LogPatch("Mind Fog patched to provide concealment.", Mind_Fog_Area);

            }

            void PatchAcidFog()
            {

                var Acid_Fog_Area = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("f4dc3f53090627945b83f16ebf3146a6");
                Acid_Fog_Area.AddComponents(Obscuring_Generic_Component_Array);

                ToDContext.Logger.LogPatch("Acid Fog patched to provide concealment.", Acid_Fog_Area);

            }

            void PatchPlagueStorm()
            {

                var Plague_Storm_Blinding_Sickness_Area = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("b342e42d2ed58484c8dff9150d18f4e4");
                var Plague_Storm_Bubonic_Plague_Area = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("ba09d51375db5f34790184443416d84b");
                var Plague_Storm_Cackle_Fever_Area = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("6cae3c64989f3684bb078efcfa9021a1");
                var Plague_Storm_Mind_Fire_Area = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("6fa0adacca8d00f4aaba1e8df77a318f");
                var Plague_Storm_Shakes_Area = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("706df636208b2864aa80032b72e0aabd");

                Plague_Storm_Blinding_Sickness_Area.AddComponents(Obscuring_Generic_Component_Array);
                Plague_Storm_Bubonic_Plague_Area.AddComponents(Obscuring_Generic_Component_Array);
                Plague_Storm_Cackle_Fever_Area.AddComponents(Obscuring_Generic_Component_Array);
                Plague_Storm_Mind_Fire_Area.AddComponents(Obscuring_Generic_Component_Array);
                Plague_Storm_Shakes_Area.AddComponents(Obscuring_Generic_Component_Array);

                ToDContext.Logger.LogPatch("Plague Storm (Blinding Sickness) patched to provide concealment.", Plague_Storm_Blinding_Sickness_Area);
                ToDContext.Logger.LogPatch("Plague Storm (Bubonic Plague) patched to provide concealment.", Plague_Storm_Bubonic_Plague_Area);
                ToDContext.Logger.LogPatch("Plague Storm (Cackle Fever) patched to provide concealment.", Plague_Storm_Cackle_Fever_Area);
                ToDContext.Logger.LogPatch("Plague Storm (Mind Fire) patched to provide concealment.", Plague_Storm_Mind_Fire_Area);
                ToDContext.Logger.LogPatch("Plague Storm (Shakes) patched to provide concealment.", Plague_Storm_Shakes_Area);
            }


            void PatchVolcanicStorm()
            {

                var Volcanic_Storm_Area = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("1d649d8859b25024888966ba1cc291d1");
                Volcanic_Storm_Area.AddComponents(Obscuring_Generic_Component_Array);

                ToDContext.Logger.LogPatch("Volcanic Storm patched to provide concealment.", Volcanic_Storm_Area);

            }

            void PatchHallucinogenicCloud()
            {

                var Hallucinogenic_Cloud_Area = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("053c8fd966bbcb94bb8db6582d990650");
                Hallucinogenic_Cloud_Area.AddComponents(Obscuring_Generic_Component_Array);

                ToDContext.Logger.LogPatch("Hallucinogenic Cloud patched to provide concealment.", Hallucinogenic_Cloud_Area);

            }

            void PatchKineticistCloudInfusions()
            {

                var Cloud_Blizzard_Blast_Area = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("6ea87a0ff5df41c459d641326f9973d5");
                var Cloud_Sandstorm_Blast_Area = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("48aa66d1a15515e40b07bc1f5fb80f64");
                var Cloud_Steam_Blast_Area = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("35a62ad81dd5ae3478956c61d6cd2d2e");
                var Cloud_Thunderstorm_Blast_Area = BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("3659ce23ae102ca47a7bf3a30dd98609");
                Cloud_Blizzard_Blast_Area.AddComponents(Obscuring_Generic_Component_Array);
                Cloud_Sandstorm_Blast_Area.AddComponents(Obscuring_Generic_Component_Array);
                Cloud_Steam_Blast_Area.AddComponents(Obscuring_Generic_Component_Array);
                Cloud_Thunderstorm_Blast_Area.AddComponents(Obscuring_Generic_Component_Array);

                ToDContext.Logger.LogPatch("Cloud Infusion (Blizzard Blast) patched to provide concealment.", Cloud_Blizzard_Blast_Area);
                ToDContext.Logger.LogPatch("Cloud Infusion (Sandstorm Blast) patched to provide concealment.", Cloud_Sandstorm_Blast_Area);
                ToDContext.Logger.LogPatch("Cloud Infusion (Steam Blast) patched to provide concealment.", Cloud_Steam_Blast_Area);
                ToDContext.Logger.LogPatch("Cloud Infusion (Thunderstorm Blast) patched to provide concealment.", Cloud_Thunderstorm_Blast_Area);

            }

        }
    }

}
