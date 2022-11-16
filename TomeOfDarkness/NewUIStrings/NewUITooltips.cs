using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;

namespace TomeOfDarkness.NewUIStrings
{
    public class NewUITooltips
    {
        [InitializeStaticString]
        public static LocalizedString CustomFactImmunity = Helpers.CreateString(ToDContext, "CustomFactImmunity.UIString", "{target} is immune to {custom_fact} of {caster}.");

        [InitializeStaticString]
        public static LocalizedString CustomFactBypassSuccess = Helpers.CreateString(ToDContext, "CustomFactBypassSuccess.UIString", "{target} successfuly overcomes {custom_fact} of {caster}.");

        [InitializeStaticString]
        public static LocalizedString CustomFactBypassFailure = Helpers.CreateString(ToDContext, "CustomFactBypassFailure.UIString", "{target} fails to overcome {custom_fact} of {caster}.");

        [InitializeStaticString]
        public static LocalizedString CustomFactInvalidation = Helpers.CreateString(ToDContext, "CustomFactInvalidation.UIString", "{target} invalidates {custom_fact} against {caster}.");


    }
}
