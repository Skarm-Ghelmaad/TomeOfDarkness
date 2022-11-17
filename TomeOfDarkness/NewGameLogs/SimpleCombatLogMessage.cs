using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Localization;
using Kingmaker.UI.Models.Log;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem;
using UnityEngine;
using Kingmaker.Blueprints.Root.Strings.GameLog;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.Utility;
using Kingmaker.EntitySystem;
using Kingmaker.Blueprints;
using static Kingmaker.Blueprints.Root.Strings.GameLog.GameLogStrings;
using Owlcat.Runtime.UI.Tooltips;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem.LogThreads.Common;
using System.Runtime.CompilerServices;

namespace TomeOfDarkness.NewGameLogs
{

    public class SimpleCombatLogMessage
    {
        public static CombatLogMessage GenerateSimpleCombatLogMessage(LocalizedString message, Color color, UnitEntityData source_entity = null, UnitEntityData target = null, EntityFact source_fact = null, string text = "", string description = "", string text_with_tags = "")
        {
            CombatLogMessage result;
                using (ProfileScope.New("Build Simple Combat Log Message", (SimpleBlueprint)null))
                {
                    using (GameLogContext.Scope)
                    {
                        if (source_entity != null)
                        {
                            GameLogContext.SourceUnit = source_entity;
                        }
                        else
                        {
                            GameLogContext.SourceFact = source_fact;
                        }

                        GameLogContext.Target = target;
                        GameLogContext.Message = message;
                        GameLogContext.Text = text;
                        GameLogContext.TextWithTags = text_with_tags;
                        GameLogContext.Description = description;
                        string message_text = GameLogContext.Message.ToString();
                        TooltipTemplateCombatLogMessage template = null;
                        Color32 actual_color = ValidateColor(color);

                        result = new CombatLogMessage(message_text, actual_color, GameLogContext.GetIcon(), template, true);
                    }
                }
            
            return result;
        }

        public static void SendSimpleCombatLogMessage(CombatLogMessage message)
        {
            var messageLog = LogThreadService.Instance.m_Logs[LogChannelType.Common].First(x => x is MessageLogThread);
            messageLog.AddMessage(message);
        }

        public static Color32 ValidateColor(Color color)
        {
            if (color.r <= 0 && color.g <= 0 && color.b <= 0 && color.a <= 0)
            {
                return GameLogStrings.Instance.DefaultColor;
            }
            return color;
        }


    }
}
