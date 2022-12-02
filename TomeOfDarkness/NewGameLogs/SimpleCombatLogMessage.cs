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
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem.LogThreads.Combat;

namespace TomeOfDarkness.NewGameLogs
{

    public class SimpleCombatLogMessage
    {
        public static CombatLogMessage GenerateSimpleCombatLogMessage(LocalizedString message, Color color, UnitEntityData source_entity = null, UnitEntityData target = null, string text = "", string description = "", string text_with_tags = "")
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

        // Generally the following are used:
        // a) LogChannelType.AnyCombat [if you want to send a message alongside skill checks and saving throws]
        // b) LogChannelType.AnyCombat [if you just want to send the message]

        public static void SendSimpleCombatLogMessage(CombatLogMessage message, LogChannelType channel_type, LogThreadBaseAttachment log_threat_type)
        {
            switch(log_threat_type)
            {
                case LogThreadBaseAttachment.SavingThrow:
                    var messageLog1 = LogThreadService.Instance.m_Logs[channel_type].Last(x => x is RollSkillCheckLogThread);
                    messageLog1.AddMessage(message);
                    return;

                case LogThreadBaseAttachment.SkillCheck:
                    var messageLog2 = LogThreadService.Instance.m_Logs[channel_type].Last(x => x is RollSkillCheckLogThread);
                    messageLog2.AddMessage(message);
                    return;

                default:
                    var messageLog3 = LogThreadService.Instance.m_Logs[channel_type].Last(x => x is MessageLogThread);
                    messageLog3.AddMessage(message);
                    return;
            }    
        }

        public static Color32 ValidateColor(Color color)
        {
            if (color.r <= 0 && color.g <= 0 && color.b <= 0 && color.a <= 0)
            {
                return GameLogStrings.Instance.DefaultColor;
            }
            return color;
        }

        public enum LogThreadBaseAttachment
        {
            None = 0,
            SavingThrow = 1,
            SkillCheck = 2
        }


    }
}
