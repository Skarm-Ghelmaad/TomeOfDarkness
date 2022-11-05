using Kingmaker.Utility;
using System.Collections.Generic;
using System.Web.Routing;
using TabletopTweaks.Core.Config;
using static TabletopTweaks.Base.Config.Bugfixes;
using static TabletopTweaks.Core.Utilities.BloodlineTools;

namespace TomeOfDarkness.Config
{
    public class Bugfixes : IUpdatableSettings
    {

        public bool NewSettingsOffByDefault = false;
        public SettingGroup BaseFixes = new SettingGroup();
        public SettingGroup Spells = new SettingGroup();

        public void Init()
        {
        }

        public void OverrideSettings(IUpdatableSettings userSettings)
        {
            var loadedSettings = userSettings as Bugfixes;
            NewSettingsOffByDefault = loadedSettings.NewSettingsOffByDefault;

            BaseFixes.LoadSettingGroup(loadedSettings.BaseFixes, NewSettingsOffByDefault);
            Spells.LoadSettingGroup(loadedSettings.Spells, NewSettingsOffByDefault);

        }

    }
}
