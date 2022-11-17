using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;

namespace TomeOfDarkness.NewUIStrings
{
    public class CustomUIStrings
    {
        [InitializeStaticString]
        public static LocalizedString CustomFactImmunity = Helpers.CreateString(ToDContext, "CustomFactImmunity.CustomUIString", "{target} is immune to {text} of {source}.");

        [InitializeStaticString]
        public static LocalizedString CustomFactBypassSuccess = Helpers.CreateString(ToDContext, "CustomFactBypassSuccess.CustomUIString", "{target} successfuly overcomes {text} of {source}.");

        [InitializeStaticString]
        public static LocalizedString CustomFactBypassFailure = Helpers.CreateString(ToDContext, "CustomFactBypassFailure.CustomUIString", "{target} fails to overcome {text} of {source}.");

        [InitializeStaticString]
        public static LocalizedString CustomFactInvalidation = Helpers.CreateString(ToDContext, "CustomFactInvalidation.CustomUIString", "{target} invalidates {text} against {source}.");

    }
}
