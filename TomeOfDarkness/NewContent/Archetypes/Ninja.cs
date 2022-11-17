using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TomeOfDarkness.NewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using static TomeOfDarkness.Main;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker.UnitLogic.Mechanics;
using HlEX = TomeOfDarkness.Utilities.HelpersExtension;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics.Components;
using TomeOfDarkness.Utilities;
using static TabletopTweaks.Core.Utilities.FeatTools;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;
using Kingmaker.UnitLogic.ActivatableAbilities;
using TomeOfDarkness.NewComponents.OwlcatReplacements;
using static Kingmaker.UnitLogic.Interaction.SpawnerInteractionPart;
using Kingmaker.Blueprints.Classes.Selection;
using TomeOfDarkness.NewContent.NinjaTricks;
using TomeOfDarkness.NewContent.Features;
using HarmonyLib;



namespace TomeOfDarkness.NewContent.Archetypes
{
    internal class Ninja
    {
        public static void ConfigureNinjaArchetype()
        {
            var Rogue_Array = new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass };

            var RogueClass = ClassTools.Classes.RogueClass;

            var Ninja_Archetype = Helpers.CreateBlueprint<BlueprintArchetype>(ToDContext, "NinjaArchetype", arc => {
                arc.SetName(ToDContext, "Ninja");
                arc.SetDescription(ToDContext, "When the wealthy and the powerful need an enemy eliminated quietly and without fail, they call upon the ninja. When a general needs to sabotage the siege engines of his foes before they can reach the castle walls, he calls upon the ninja. And when fools dare to move against a ninja or her companions, they will find the ninja waiting for them while they sleep, ready to strike. These shadowy killers are masters of infiltration, sabotage, and assassination, using a wide variety of weapons, practiced skills, and mystical powers to achieve their goals.");
                arc.m_ParentClass = RogueClass.ToReference<BlueprintCharacterClassReference>();
            });

            var rogue_proficiencies = BlueprintTools.GetBlueprint<BlueprintFeature>("33e2a7e4ad9daa54eaf808e1483bb43c");
            var weapon_finesse = BlueprintTools.GetBlueprint<BlueprintFeature>("90e54424d682d104ab36436bd527af09");
            var trapfinding = BlueprintTools.GetBlueprint<BlueprintFeature>("dbb6b3bffe6db3547b31c3711653838e");
            var evasion = BlueprintTools.GetBlueprint<BlueprintFeature>("576933720c440aa4d8d42b0c54b77e80");
            var rogue_talent = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("c074a5d615200494b8f2a9c845799d93");
            var finesse_training_selection = BlueprintTools.GetBlueprint<BlueprintFeature>("b78d146cea711a84598f0acef69462ea");
            var danger_sense = BlueprintTools.GetBlueprint<BlueprintFeature>("0bcbe9e450b0e7b428f08f66c53c5136");
            var debilitating_injury = BlueprintTools.GetBlueprint<BlueprintFeature>("def114eb566dfca448e998969bf51586");
            var uncanny_dodge = BlueprintTools.GetBlueprint<BlueprintFeature>("3c08d842e802c3e4eb19d15496145709");
            var improved_uncanny_dodge = BlueprintTools.GetBlueprint<BlueprintFeature>("485a18c05792521459c7d06c63128c79");
            var advanced_talents = BlueprintTools.GetBlueprint<BlueprintFeature>("a33b99f95322d6741af83e9381b2391c");
            var master_strike = BlueprintTools.GetBlueprint<BlueprintFeature>("72dcf1fb106d5054a81fd804fdc168d3");

            ConfigureNinjaProficiencies();
            NoTrace.ConfigureNoTrace();
            ConfigureNinjaKiPool();
            LightSteps.ConfigureLightSteps();
            Dispatchment.ConfigureDispatchment();
            ConfigureNinjaStyleStrikes();
            ConfigureNinjaPoisonUse();
            ConfigureNinjaTrick();

            var ninja_prof = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(ToDContext, "NinjaProficiencies");
            var ninja_ki_pool = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(ToDContext, "KiPoolNinjaFeature");
            var ninja_trick = BlueprintTools.GetModBlueprintReference<BlueprintFeatureSelectionReference>(ToDContext, "NinjaTrickSelection");
            var ninja_no_trace = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(ToDContext, "NinjaNoTraceFeature");
            var ninja_dispatchment = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(ToDContext, "NinjaDispatchmentFeature");
            var ninja_light_steps = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(ToDContext, "NinjaLightStepsFeature");
            var ninja_style_strikes = BlueprintTools.GetModBlueprintReference<BlueprintFeatureSelectionReference>(ToDContext, "NinjaStyleStrike");
            var ninja_poison_use = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(ToDContext, "NinjaCreatePoison");

            Ninja_Archetype.RemoveFeatures = new LevelEntry[] {
                                                                Helpers.CreateLevelEntry(1, rogue_proficiencies, trapfinding ),
                                                                Helpers.CreateLevelEntry(2, evasion, rogue_talent ),
                                                                Helpers.CreateLevelEntry(3, danger_sense ),
                                                                Helpers.CreateLevelEntry(4, rogue_talent, debilitating_injury),
                                                                Helpers.CreateLevelEntry(6, rogue_talent, danger_sense),
                                                                Helpers.CreateLevelEntry(8, rogue_talent),
                                                                Helpers.CreateLevelEntry(9, danger_sense),
                                                                Helpers.CreateLevelEntry(10, rogue_talent),
                                                                Helpers.CreateLevelEntry(12, rogue_talent, danger_sense),
                                                                Helpers.CreateLevelEntry(14, rogue_talent),
                                                                Helpers.CreateLevelEntry(15, danger_sense),
                                                                Helpers.CreateLevelEntry(16, rogue_talent),
                                                                Helpers.CreateLevelEntry(18, rogue_talent, danger_sense),
                                                                Helpers.CreateLevelEntry(20, rogue_talent),

                                                                };

            Ninja_Archetype.AddFeatures = new LevelEntry[] {
                                                                Helpers.CreateLevelEntry(1, ninja_prof, ninja_poison_use ),
                                                                Helpers.CreateLevelEntry(2, ninja_ki_pool, ninja_trick  ),
                                                                Helpers.CreateLevelEntry(3, ninja_no_trace ),
                                                                Helpers.CreateLevelEntry(4, ninja_trick, ninja_dispatchment),
                                                                Helpers.CreateLevelEntry(5, ninja_style_strikes ),
                                                                Helpers.CreateLevelEntry(6, ninja_trick, ninja_no_trace, ninja_light_steps ),
                                                                Helpers.CreateLevelEntry(8, ninja_trick),
                                                                Helpers.CreateLevelEntry(9, ninja_no_trace),
                                                                Helpers.CreateLevelEntry(10, ninja_trick, ninja_style_strikes),
                                                                Helpers.CreateLevelEntry(12, ninja_trick, ninja_no_trace),
                                                                Helpers.CreateLevelEntry(14, ninja_trick),
                                                                Helpers.CreateLevelEntry(15, ninja_no_trace, ninja_style_strikes ),
                                                                Helpers.CreateLevelEntry(16, ninja_trick),
                                                                Helpers.CreateLevelEntry(18, ninja_trick, ninja_no_trace),
                                                                Helpers.CreateLevelEntry(20, ninja_trick, ninja_style_strikes ),

                                                                };


            if (ToDContext.NewContent.Archetypes.IsDisabled("Ninja")) { return; }

            RogueClass.TemporaryContext(bp => {

                bp.Progression.m_UIDeterminatorsGroup = bp.Progression.m_UIDeterminatorsGroup.AppendToArray(ninja_prof.Get().ToReference<BlueprintFeatureBaseReference>());
                bp.Progression.UIGroups = bp.Progression.UIGroups.AppendToArray(Helpers.CreateUIGroup(ninja_ki_pool, ninja_light_steps, ninja_dispatchment));
                bp.m_Archetypes = bp.m_Archetypes.AddItem(Ninja_Archetype.ToReference<BlueprintArchetypeReference>()).ToArray();

            });


            ToDContext.Logger.LogPatch("Added Ninja Rogue archetype.", Ninja_Archetype);

        }

        static void ConfigureNinjaProficiencies()
        {
            var Rogue_Proficiencies = BlueprintTools.GetBlueprint<BlueprintFeature>("33e2a7e4ad9daa54eaf808e1483bb43c");
            var Dueling_Sword_Proficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("9c37279588fd9e34e9c4cb234857492c");

            #region | Create Ninja Proficiencies |

            var NinjaProficienciesFeature = Rogue_Proficiencies.CreateCopy(ToDContext, "NinjaProficiencies", bp =>
            {
                bp.ReplaceComponents<AddProficiencies>(Helpers.Create<AddProficiencies>(c =>
                {
                    c.WeaponProficiencies = new WeaponCategory[] {
                                                                    WeaponCategory.Kama,
                                                                    WeaponCategory.Nunchaku,
                                                                    WeaponCategory.Sai,
                                                                    WeaponCategory.Shortbow,
                                                                    WeaponCategory.Shortsword,
                                                                    WeaponCategory.Shuriken,
                                                                    WeaponCategory.Scimitar
                                                                };

                }));
                bp.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[] { Dueling_Sword_Proficiency.ToReference<BlueprintUnitFactReference>() };
                });
                bp.SetName(ToDContext, "Ninja Proficiencies");
                bp.SetDescription(ToDContext, "Ninja are proficient with all simple weapons, plus the kama, dueling sword, nunchaku, sai, shortbow, short sword, shuriken, and scimitar. Ninjas are proficient with light armor but not with shields.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
            });
            #endregion

            ToDContext.Logger.LogPatch("Created Ninja Proficiencies.", NinjaProficienciesFeature);

        }

        static void ConfigureNinjaKiPool()
        {
            var InstinctiveStealthIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_InstinctiveStealth.png");
            var kiResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82");
            var Abundant_Ki_Pool = BlueprintTools.GetBlueprint<BlueprintFeature>("e8752f9126d986748b10d0bdac693264");
            var Ki_Extra_Attack_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("cadf8a5c42002494cabfc6c1196b514a");
            var Monk_Ki_Extra_Attack_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("7f6ea312f5dad364fa4a896d7db39fdd");
            var Monk_Ki_Sudden_Speed_Ability = BlueprintTools.GetBlueprint<BlueprintAbility>("8c98b8f3ac90fa245afe14116e48c7da");
            var Expeditious_Retreat_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("9ea4ec3dc30cd7940a372a4d699032e7");
            var Shadow_Veil_Buff = BlueprintTools.GetBlueprint<BlueprintBuff>("5ceedff361efd4c4eb8e8369c13b03ea");
            var Shadow_Veil_Buff_Fx_Asset_ID = Shadow_Veil_Buff.FxOnStart.AssetId;

            #region | Create Ninja Ki abilities |

            var NinjaKiSpeedBuff = Expeditious_Retreat_Buff.CreateCopy(ToDContext, "NinjaTrickKiSpeedBuff", bp =>
            {
                bp.SetName(ToDContext, "Speed Burst");
                bp.SetDescription(ToDContext, "You grant yourself a sudden burst of speed, increasing your base land speed by 20 feet for 1 round.");
                bp.ReplaceComponents<BuffMovementSpeed>(Helpers.Create<BuffMovementSpeed>(c =>
                {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Value = 20;
                }));

            });


            var Instinctive_Stealth_Buff = Helpers.CreateBlueprint<BlueprintBuff>(ToDContext, "NinjaTrickInstinctiveStealthBuff", bp =>
            {
                bp.SetName(ToDContext, "Instinctive Stealth");
                bp.SetDescription(ToDContext, "You grant yourself an insight on which place and ways are the best to be stealthy in the current circumstances. This grants you a +4 insight bonus on Stealth checks for 1 round.");
                bp.m_Icon = InstinctiveStealthIcon;
                bp.FxOnStart = HlEX.CreatePrefabLink(Shadow_Veil_Buff_Fx_Asset_ID);
                bp.FxOnRemove = new PrefabLink();
                bp.AddComponent(Helpers.Create<AddStatBonus>(c =>
                {
                    c.Stat = StatType.SkillStealth;
                    c.Descriptor = ModifierDescriptor.Insight;
                    c.Value = 4;
                }));
            });


            var Apply_Instinctive_Stealth_Buff = HlEX.CreateContextActionApplyBuff(Instinctive_Stealth_Buff.ToReference<BlueprintBuffReference>(),
                                                                                   HlEX.CreateContextDuration(1, DurationRate.Rounds),
                                                                                   false, false, false, false, false);

            var NinjaExtraAttackAbility = Monk_Ki_Extra_Attack_Ability.CreateCopy(ToDContext, "NinjaTrickKiExtraAttackAbility", bp =>
            {
                bp.SetName(ToDContext, "Ninja Trick: Extra Attack");
                bp.SetDescription(ToDContext, "By spending 1 point from his ki pool as a swift action, a ninja can make one additional attack at his highest attack bonus when making a full attack. This bonus attack stacks with haste and similar effects.");

            });

            ToDContext.Logger.LogPatch("Created Extra Attack (minor) ninja trick.", Monk_Ki_Extra_Attack_Ability);

            var NinjaKiSpeedBoostAbility = Monk_Ki_Sudden_Speed_Ability.CreateCopy(ToDContext, "NinjaTrickKiSpeedBoostAbility", bp =>
            {
                bp.SetName(ToDContext, "Ninja Trick: Speed Burst");
                bp.SetDescription(ToDContext, "A ninja with this ki power can spend 1 point from his ki pool as a swift action to grant himself a sudden burst of speed. This increases the ninja's base land speed by 20 feet for 1 round.");
            });

            NinjaKiSpeedBoostAbility.FlattenAllActions()
                                    .OfType<ContextActionApplyBuff>()
                                    .ForEach(a =>
                                    {
                                        a.m_Buff = NinjaKiSpeedBuff.ToReference<BlueprintBuffReference>();
                                        a.DurationValue = HlEX.CreateContextDuration(1, DurationRate.Rounds);
                                    });

            ToDContext.Logger.LogPatch("Created Ninja Speed Burst (minor) ninja trick.", NinjaKiSpeedBoostAbility);

            var InstinctiveStealthAbility = Helpers.CreateBlueprint<BlueprintAbility>(ToDContext, "NinjaTrickInstinctiveStealthAbility", bp =>
            {
                bp.SetName(ToDContext, "Ninja Trick: Instinctive Stealth");
                bp.SetDescription(ToDContext, "A ninja with this ki power can spend 1 point from his ki pool as a swift action to grant himself an insight on which place and ways are the best to be stealthy in the current circumstances. This grants him a +4 insight bonus on Stealth checks for 1 round.");
                bp.m_Icon = InstinctiveStealthIcon;
                bp.ResourceAssetIds = Array.Empty<string>();
                bp.Type = AbilityType.Supernatural;
                bp.ActionType = UnitCommand.CommandType.Swift;
                bp.Range = AbilityRange.Personal;
                bp.LocalizedDuration = Helpers.CreateString(ToDContext, "NinjaTrickInstinctiveStealthAbility.Duration", "1 round");
                bp.LocalizedSavingThrow = new Kingmaker.Localization.LocalizedString();
                bp.AddComponent(HlEX.CreateRunActions(Apply_Instinctive_Stealth_Buff));
                bp.AddComponent(kiResource.CreateResourceLogic());
            });

            InstinctiveStealthAbility.SetMiscAbilityParametersSelfOnly();

            ToDContext.Logger.LogPatch("Created Instinctive Stealth (minor) ninja trick.", InstinctiveStealthAbility);

            var Ninja_Extra_Attack_feature = HlEX.ConvertAbilityToFeature(NinjaExtraAttackAbility, "", "", "Feature", "Ability", false);
            Ninja_Extra_Attack_feature.IsClassFeature = true;
            Ninja_Extra_Attack_feature.Ranks = 1;



            #endregion

            #region | Create Ninja Ki Pool |

            var wis_ki_points_bonus = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(ToDContext, "WisdomKiPoolCanonFeature");
            var cha_ki_points_bonus = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(ToDContext, "CharismaKiPoolCanonFeature");

            var canon_ki_modifier_exclusions = new BlueprintUnitFactReference[] { wis_ki_points_bonus.Get().ToReference<BlueprintUnitFactReference>(), cha_ki_points_bonus.Get().ToReference<BlueprintUnitFactReference>() };

            var KiPool = Helpers.CreateBlueprint<BlueprintFeature>(ToDContext, "KiPoolNinjaFeature", bp =>
            {
                bp.SetName(ToDContext, "Ki Pool");
                bp.SetDescription(ToDContext, "At 2nd level, a ninja gains a pool of ki points, supernatural energy she can use to accomplish amazing feats. The number of points in the ninja’s ki pool is equal to 1/2 her ninja level + her Charisma modifier.\n"
                                            + "By spending 1 point from her ki pool, a ninja can make one additional attack at her highest attack bonus, but she can do so only when making a full attack. In addition, she can spend 1 point to increase her speed by 20 feet for 1 round.\n"
                                            + "Moreover, she can also spend 1 point to gain a +4 insight bonus on her Stealth checks for 1 round. Each of these powers is activated as a swift action. A ninja can gain additional powers that consume points from her ki pool by selecting certain ninja tricks.");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.Groups = new FeatureGroup[] { FeatureGroup.None };
                bp.IsPrerequisiteFor = new List<BlueprintFeatureReference> { Abundant_Ki_Pool.ToReference<BlueprintFeatureReference>() };
                bp.AddComponent(HlEX.CreateAddAbilityResources(kiResource.ToReference<BlueprintAbilityResourceReference>()));
                bp.AddComponent(Helpers.Create<IncreaseResourceAmountBasedOnClassOnly>(c => {
                    c.m_Resource = kiResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_CharacterClass = ClassTools.ClassReferences.RogueClass;
                    c.Subtract = false;
                    c.IncreasedByLevel = false;
                    c.IncreasedByLevelStartPlusDivStep = true;
                    c.StartingLevel = 0;
                    c.StartingIncrease = 0;
                    c.LevelStep = 2;
                    c.PerStepIncrease = 1;
                    c.Subtract = false;
                }));
                bp.AddComponent<HasFactsFeaturesUnlock>(c => {
                    c.m_CheckedFacts = canon_ki_modifier_exclusions;
                    c.m_Features = new BlueprintUnitFactReference[] { cha_ki_points_bonus.Get().ToReference<BlueprintUnitFactReference>() };
                    c.Not = true;
                });
                bp.AddComponent<AddFacts>(c =>
                {
                    c.m_Facts = new BlueprintUnitFactReference[] { NinjaKiSpeedBoostAbility.ToReference<BlueprintUnitFactReference>(), InstinctiveStealthAbility.ToReference<BlueprintUnitFactReference>() };
                });
                bp.AddComponent(HlEX.CreateAddFeatureOnClassLevel(Ninja_Extra_Attack_feature.ToReference<BlueprintFeatureReference>(), new BlueprintCharacterClassReference[] { ClassTools.ClassReferences.RogueClass }, 5, true));
            });

            ToDContext.Logger.LogPatch("Created Ninja Ki Pool.", KiPool);

            //Adjust ki pool by adding ki points.


            #endregion

            #region | Add Ninja Ki Pool to Abundant Ki Pool feat|

            Abundant_Ki_Pool.AddPrerequisiteFeature(KiPool, GroupType.Any);  // Added Ninja Ki Pool to Abundant Ki Prerequisites. 

            ToDContext.Logger.LogPatch("Updated Abundant Ki Pool feat.", Abundant_Ki_Pool);

            #endregion

        }

        static void ConfigureNinjaStyleStrikes()
        {
            var NinjaStyleStrikeIcon = AssetLoader.LoadInternal(ToDContext, folder: "Abilities", file: "Icon_NinjaStyleStrike.png");
            var monk_style_strikes = Selections.MonkStyleStrike;
            var NinjaTrickKiExtraAttackAbility = BlueprintTools.GetModBlueprint<BlueprintAbility>(ToDContext, "NinjaTrickKiExtraAttackAbility");

            #region | Create Ninja Style Strikes from Monk Style Strike |

            var ninja_style_strikes = monk_style_strikes.CreateCopy(ToDContext, "NinjaStyleStrike", bp =>
            {
                bp.SetDescription(ToDContext, "At 5th level, a ninja can learn one type of style strike, as the monk class feature. Whenever she spends ki from her ki pool to make an additional attack, she can designate that additional attack as a style strike, regardless of the weapon she uses to make the attack. The attack is resolved as normal, but it has a different effect depending upon the type of strike chosen. At 10th level and every 5 levels thereafter, a ninja learns an additional style strike. She must choose which style strike to apply before the attack roll is made. Unlike a monk, a ninja does not gain the ability to designate more than one attack as a style strike per round.");
                bp.m_Features = new BlueprintFeatureReference[0];
            });

            var ninja_style_strikes_old_features = ninja_style_strikes.m_AllFeatures;

            var new_abilities = new List<BlueprintAbility>();

            foreach (var stl_strk in ninja_style_strikes_old_features)
            {
                var stl_strk_toggle = stl_strk.Get().GetComponent<AddFacts>().m_Facts[0].Get() as BlueprintActivatableAbility;
                var stl_strk_buff = stl_strk_toggle.m_Buff.Get().CreateCopy(ToDContext, "Ninja" + stl_strk_toggle.m_Buff.Get().name);

                if (stl_strk_buff.GetComponent<DoubleDamageDiceOnAttackLOS>() != null)
                {
                    stl_strk_buff.GetComponent<DoubleDamageDiceOnAttackLOS>().TemporaryContext(c =>
                    {
                        c.m_WeaponType = null;
                    });
                }

                if (stl_strk_buff.GetComponent<AddInitiatorAttackWithWeaponTrigger>() != null)
                {
                    stl_strk_buff.GetComponent<AddInitiatorAttackWithWeaponTrigger>().TemporaryContext(c =>
                    {
                        c.m_WeaponType = null;
                    });
                }

                if (stl_strk_buff.GetComponent<IgnoreDamageReductionOnAttack>() != null)
                {
                    stl_strk_buff.GetComponent<IgnoreDamageReductionOnAttack>().TemporaryContext(c =>
                    {
                        c.m_WeaponType = null;
                    });
                }

                stl_strk_buff.m_Flags = (BlueprintBuff.Flags)0;
                stl_strk_buff.SetName(ToDContext, stl_strk_toggle.m_DisplayName);
                stl_strk_buff.SetDescription(ToDContext, stl_strk_toggle.m_Description);
                stl_strk_buff.m_Icon = stl_strk_toggle.m_Icon;

                var apply_stl_strk_buff = HlEX.CreateContextActionApplyBuff(stl_strk_buff.ToReference<BlueprintBuffReference>(),
                                                                            HlEX.CreateContextDuration(1),
                                                                            false, false, false, false, false);
                var stl_strk_ability = NinjaTrickKiExtraAttackAbility.CreateCopy(ToDContext, "Ninja" + stl_strk_toggle.name);

                stl_strk_ability.GetComponent<AbilityEffectRunAction>().Actions.Actions.AppendToArray(apply_stl_strk_buff);

                new_abilities.Add(stl_strk_ability);

                var stl_strk_new_feature = stl_strk.Get().CreateCopy(ToDContext, "Ninja" + stl_strk.Get().name);

                stl_strk_new_feature.ComponentsArray = new BlueprintComponent[0];

                ninja_style_strikes.m_AllFeatures.AppendToArray(stl_strk_new_feature.ToReference<BlueprintFeatureReference>());

                stl_strk_ability.AddComponent(HlEX.CreateAbilityShowIfCasterHasFact(stl_strk_new_feature.ToReference<BlueprintUnitFactReference>()));

                stl_strk_ability.SetName(ToDContext, NinjaTrickKiExtraAttackAbility.m_DisplayName + " (" + stl_strk_toggle.m_DisplayName + ")");

                stl_strk_ability.SetDescription(ToDContext, NinjaTrickKiExtraAttackAbility.m_Description + "\n" + stl_strk_toggle.m_DisplayName + ": " + stl_strk_toggle.m_Description);

                stl_strk_ability.m_Icon = stl_strk_toggle.m_Icon;

            }

            #endregion

            #region | Create Ninja Style Strike wrapper |

            var stl_strk_wrapper = HlEX.CreateVariantWrapper("NinjaStyleStrikesAbilityBase", new_abilities.ToArray());

            stl_strk_wrapper.SetName(ToDContext, NinjaTrickKiExtraAttackAbility.m_DisplayName + " (" + ninja_style_strikes.m_DisplayName + ")");

            stl_strk_wrapper.SetDescription(ToDContext, NinjaTrickKiExtraAttackAbility.m_Description + " (" + ninja_style_strikes.m_DisplayName + ": " + ninja_style_strikes.m_Description + ")");

            stl_strk_wrapper.m_Icon = NinjaStyleStrikeIcon;

            ninja_style_strikes.ComponentsArray = new BlueprintComponent[] { HlEX.CreateAddFacts(stl_strk_wrapper.ToReference<BlueprintUnitFactReference>()) };

            ToDContext.Logger.LogPatch("Created Ninja Style Strikes.", ninja_style_strikes);

            #endregion


        }

        static void ConfigureNinjaTrick()
        {
            var rogue_talent_selection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("c074a5d615200494b8f2a9c845799d93");
            var improved_unarmed_strike = BlueprintTools.GetBlueprint<BlueprintFeature>("7812ad3672a4b9a4fb894ea402095167");
            var evasion = BlueprintTools.GetBlueprint<BlueprintFeature>("576933720c440aa4d8d42b0c54b77e80");
            var NinjaTrickSelectionIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_NinjaTrickSelection.png");

            var ninja_trick = rogue_talent_selection.CreateCopy(ToDContext, "NinjaTrickSelection");

            ninja_trick.SetName(ToDContext, "Ninja Tricks");
            ninja_trick.SetDescription(ToDContext, "As a ninja continues her training, she learns a number of tricks that allow her to confuse her foes and grant her supernatural abilities. Starting at 2nd level, a ninja gains one ninja trick. She gains one additional ninja trick for every 2 levels attained after 2nd. Unless otherwise noted, a ninja cannot select an individual ninja trick more than once.");
            ninja_trick.m_Icon = NinjaTrickSelectionIcon;



            StyleMaster.ConfigureStyleMaster();
            AccelerationOfForm.ConfigureAccelerationOfForm();
            ShadowClone.ConfigureShadowClone();
            InvisibleBlade.ConfigureInvisibleBlade();       // This also triggers the creation of Vanishing Trick.
            SeeTheUnseen.ConfigureSeeTheUnseen();
            HerbalCompound.ConfigureHerbalCompound();
            Kamikaze.ConfigureKamikaze();
            UnarmedCombatMastery.ConfigureUnarmedCombatMastery();
            FlurryOfStars.ConfigureFlurryOfStars();
            NinjaCanonBombs.ConfigureNinjaCanonBombs();
            PressurePoints.ConfigurePressurePoints();
            NinjaAssassinate.ConfigureNinjaAssassinate();


            var ninja_style_master = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(ToDContext, "NinjaStyleMasterFeatureSelection");
            var n_trick_accelleration_form = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickAccelerationOfFormFeature");
            var n_trick_shadow_clone = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickShadowCloneFeature");
            var n_trick_vanishing_trick = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickVanishingTrickFeature");
            var n_trick_invisible_blade = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickInvisibleBladeFeature");
            var n_trick_see_unseen = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickSeeTheUnseenFeature");
            var n_trick_herbal_compound = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickHerbalCompoundFeature");
            var n_trick_kamikaze = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickKamikazeFeature");
            var n_trick_unarmed_combat_mastery = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickUnarmedCombatMasteryNinjaFeature");
            var n_trick_flurry_stars = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickFlurryOfStarsFeature");
            var n_trick_smoke_bomb = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickSmokeBombFeature");
            var n_trick_choking_bomb = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickChokingBombFeature");
            var n_trick_blinding_bomb = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickBlindingBombFeature");
            var n_trick_pressure_points = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickPressurePointsFeature");
            var n_trick_assassinate = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "NinjaTrickAssassinateFeature");


            FeatToolsExtension.AddAsNinjaTrick(improved_unarmed_strike, false);
            FeatToolsExtension.AddAsNinjaTrick(ninja_style_master, false);
            FeatToolsExtension.AddAsNinjaTrick(n_trick_accelleration_form, false);
            FeatToolsExtension.AddAsNinjaTrick(n_trick_shadow_clone, false);
            FeatToolsExtension.AddAsNinjaTrick(n_trick_vanishing_trick, false);
            FeatToolsExtension.AddAsNinjaTrick(n_trick_invisible_blade, true);
            FeatToolsExtension.AddAsNinjaTrick(n_trick_see_unseen, true);
            FeatToolsExtension.AddAsNinjaTrick(n_trick_herbal_compound, false);
            FeatToolsExtension.AddAsNinjaTrick(n_trick_kamikaze, false);
            FeatToolsExtension.AddAsNinjaTrick(n_trick_unarmed_combat_mastery, true);
            FeatToolsExtension.AddAsNinjaTrick(evasion, true);
            FeatToolsExtension.AddAsNinjaTrick(n_trick_flurry_stars, false);
            FeatToolsExtension.AddAsNinjaTrick(n_trick_smoke_bomb, false);
            FeatToolsExtension.AddAsNinjaTrick(n_trick_choking_bomb, false);
            FeatToolsExtension.AddAsNinjaTrick(n_trick_blinding_bomb, true);
            FeatToolsExtension.AddAsNinjaTrick(n_trick_pressure_points, false);
            FeatToolsExtension.AddAsNinjaTrick(n_trick_assassinate, true);

            ToDContext.Logger.LogPatch("Created (canon) Ninja Tricks.", ninja_trick);


        }


        static void ConfigureNinjaPoisonUse()
        {
            var Assassin_Create_Poison_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("8dd826513ba857645b38e918f17b59e6");
            var Assassin_Create_Poison_Swift_Feature = BlueprintTools.GetBlueprint<BlueprintFeature>("bb7b571cadb6cc147a52431385a40a0d");
            var Assassin_Create_Poison_Resource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("d54b614eb42da7d48b927b57de337b95");

            var assassin_fake_levels = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(ToDContext, "PoisonCraftTrainingFakeLevel");
            var Poison_Craft_Charisma_Feature = BlueprintTools.GetModBlueprint<BlueprintFeature>(ToDContext, "PoisonCraftCharismaStatFeature");

            var PoisonCraftTrainingIcon = AssetLoader.LoadInternal(ToDContext, folder: "Features", file: "Icon_PoisonCraftTraining.png");

            var RoguePoisonCraftTrainingProgression = Helpers.CreateBlueprint<BlueprintProgression>(ToDContext, "RoguePoisonCraftTrainingProgression", bp => {
                bp.SetName(ToDContext, "Rogue Poison Craft Training");
                bp.SetDescription(ToDContext, "The character treats his rogue level as assassin level for the purposes of poison use.");
                bp.m_Icon = PoisonCraftTrainingIcon;
                bp.Ranks = 1;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.ReapplyOnLevelUp = true;
                bp.m_ExclusiveProgression = new BlueprintCharacterClassReference();
                bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
                bp.LevelEntries = Enumerable.Range(1, 20)
                    .Select(i => new LevelEntry
                    {
                        Level = i,
                        m_Features = new List<BlueprintFeatureBaseReference> {
                            assassin_fake_levels.Get().ToReference<BlueprintFeatureBaseReference>()
                        },
                    })
                    .ToArray();
                bp.AddClass(ClassTools.ClassReferences.RogueClass);
                bp.m_Classes[0].AdditionalLevel = 0;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
            });

            var Ninja_Create_Poison_Feature = HlEX.ConvertFeatureToFeature(Assassin_Create_Poison_Feature, "", "", "", "", "Assassin", "", "", "Ninja", "", "", false);
            Ninja_Create_Poison_Feature.AddComponent(Helpers.Create<AddFeatureOnApply>(c => {
                c.m_Feature = RoguePoisonCraftTrainingProgression.ToReference<BlueprintFeatureReference>();
            }));
            Ninja_Create_Poison_Feature.AddComponent(HlEX.CreateAddFacts(new BlueprintUnitFactReference[] { Poison_Craft_Charisma_Feature.ToReference<BlueprintUnitFactReference>() }));
            Ninja_Create_Poison_Feature.SetDescription(ToDContext, "At 1st level, a ninja gains the assassin’s poison use class feature.  He uses his rogue level as his effective rogue level to determine the effects of poison use.");
            Ninja_Create_Poison_Feature.AddComponent(Helpers.Create<IncreaseResourceAmountBasedOnClassOnly>(c => {
                c.m_Resource = Assassin_Create_Poison_Resource.ToReference<BlueprintAbilityResourceReference>();
                c.m_CharacterClass = ClassTools.ClassReferences.RogueClass;
                c.Subtract = false;
                c.IncreasedByLevel = true;
                c.IncreasedByLevelStartPlusDivStep = false;
                c.StartingLevel = 0;
                c.StartingIncrease = 0;
                c.LevelIncrease = 1;
                c.Subtract = false;
            }));

            Ninja_Create_Poison_Feature.IsClassFeature = true;
            Ninja_Create_Poison_Feature.Ranks = 1;



        }

    }
}
