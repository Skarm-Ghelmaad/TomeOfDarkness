using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;
using static Kingmaker.Localization.Shared.Locale;

namespace TomeOfDarkness.NewUIStrings
{
    public class CustomUIStrings
    {
        [InitializeStaticString]
        public static LocalizedString CustomFactImmunity = Helpers.CreateString(ToDContext, "CustomFactImmunity.CustomUIString", "<b>{target}</b> is immune to <b>{text}</b>.", enGB, false);

        [InitializeStaticString]
        public static LocalizedString CustomFactBypassSuccess = Helpers.CreateString(ToDContext, "InoffensivenessBypassSuccess.CustomUIString", "<b>{target}</b>: Overcomes <b>{text}</b> effect.", enGB, false);

        [InitializeStaticString]
        public static LocalizedString CustomFactBypassFailure = Helpers.CreateString(ToDContext, "CustomFactBypassFailure.CustomUIString", "<b>{target}</b>: Unable to overcome <b>{text}</b> effect.", enGB, false);

        [InitializeStaticString]
        public static LocalizedString CustomFactInvalidation = Helpers.CreateString(ToDContext, "CustomFactInvalidation.CustomUIString", "<b>{source}</b>: Breaks the <b>{text}</b> effect for <b>{target}</b>.", enGB, false);


    }
}
