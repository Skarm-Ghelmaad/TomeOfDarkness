using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.UMMTools;
using UnityModManagerNet;

namespace TomeOfDarkness
{
    public static class UMMSettingsUI
    {
        private static int selectedTab;
        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            UI.AutoWidth();
            UI.TabBar(ref selectedTab,
                    () => UI.Label("SETTINGS WILL NOT BE UPDATED UNTIL YOU RESTART YOUR GAME.".yellow().bold()),
                    new NamedAction("Fixes", () => SettingsTabs.Fixes()),
                    new NamedAction("Added Content", () => SettingsTabs.NewContent())
            );
        }
    }



    static class SettingsTabs
    {
        public static void Fixes()
        {
            var TabLevel = SetttingUI.TabLevel.Zero;
            var Fixes = Main.ToDContext.Fixes;
            UI.Div(0, 15);
            using (UI.VerticalScope())
            {
                UI.Toggle("New Settings Off By Default".bold(), ref Fixes.NewSettingsOffByDefault);
                UI.Space(25);

                SetttingUI.SettingGroup("Base Fixes", TabLevel, Fixes.BaseFixes);
                SetttingUI.SettingGroup("Spells", TabLevel, Fixes.Spells);
            }
        }
        public static void NewContent()
        {
            var TabLevel = SetttingUI.TabLevel.Zero;
            var AddedContent = Main.ToDContext.NewContent;
            UI.Div(0, 15);
            using (UI.VerticalScope())
            {
                UI.Toggle("New Settings Off By Default".bold(), ref AddedContent.NewSettingsOffByDefault);
                UI.Space(25);

                SetttingUI.SettingGroup("Archetypes", TabLevel, AddedContent.Archetypes);
                SetttingUI.SettingGroup("Feats", TabLevel, AddedContent.Feats);
                SetttingUI.SettingGroup("Ninja Tricks", TabLevel, AddedContent.NinjaTricks);
                SetttingUI.SettingGroup("Rogue Talents", TabLevel, AddedContent.RogueTalents);
                SetttingUI.SettingGroup("Slayer Talents", TabLevel, AddedContent.SlayerTalents);
                SetttingUI.SettingGroup("Wild Talents", TabLevel, AddedContent.WildTalents);
                SetttingUI.SettingGroup("Items", TabLevel, AddedContent.Items);
                SetttingUI.SettingGroup("Spells", TabLevel, AddedContent.Spells);



            }
        }
    }
}
