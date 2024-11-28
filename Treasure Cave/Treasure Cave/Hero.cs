using System;
using System.Collections.Generic;
using System.Linq;

namespace TreasureCave
{
    public class Hero
    {
        // Variables of a hero.
        public int alltimePartyId;
        public int currentPartyId;

        public string description;
        public string warriorCry;
        public string[] warriorCries;
        public int warriorTypeIndex;

        public string name;
        public string type;
        public string gender;
        public string[] pronoun = {"","","",""};
        public string battlecry;

        public int baseHealth;
        public int baseStrength;
        public int baseStamina;
        public int baseSpeed;

        public int extraPoints;

        public int level;
        public int dualWieldLevel;
        public int experience;
        public int dualWieldExperience;

        public int maximumLevel = 42;
        public int maximumWieldLevel = 20;

        public int maxHealth;
        public int healthpoints;
        public int maxStrength;
        public int strength;
        public int maxStamina;
        public int stamina;
        public int armor;
        public int maxSpeed;
        public int speed;
        public List<string> choiceOfWeapon = new List<string>();

        public int composure; // Affects if warrior will attack the right target, and when reaching zero they will run away from the party, being lost. High composure grants bonus dice when attacking.
        public int baseComposure; // It can be less and more, this is just the composure that a warrior type will regain when rested and healthy, and/or enhancements wear off.
        // Composure will have some steps, whereas the warriors functions and stats will be affected. It's not gradual.
        // 0 = panic, will run off and forever be lost from the party. < 3 very scared, confused or affected by ailment, will risk attacking your own, attack -2. < 6 scared or having some impairing ailment, decreases attack -1.  5 - 10, good, regular, not scared, not impaired, no sense heightened. > 10 a focused person, will have +1 attack, will easier withstand confuse attacks, can use some magic attacks from specific weapons. > 13, a very focused person, will have +2 attack, are immune to weak confuse attacks, can use extra attack options for specific weapons, can use all magic attacks from specific weapons.
        // Being damaged a lot from one attack or a lot from consecutive attacks without successfully attacking back can decrease it. Defeating an enemy or damage it a lot will increase it, but not above the base value. Magical potions, artifacts, or even armor or weapons can increase it. Magical attacks can decrease it. Death of party members decrease it. Ailments can decrease it. Rest and food will increase it, but not above the base value.

        public List<string> ailments = new List<string>();

        public int berserkValue;
        public int recoverFromPoisoningCounter;

        public int consecutiveMisses = 0;
        public int consecutiveBigDamages = 0;
        public int consecutiveDodges = 0;
        public int consecutiveCriticalHits = 0;

        public Gear equippedWeapon;
        public Gear equippedSecondaryWeapon;
        public bool isDualWielding = false;
        public bool isUsingDoubleHandedWeapon = false;
        
        public int attackDice = 0;
        public int bonusAttackDice = 0;
        public int bonusDefenceDice = 0;
        public int bonusCriticalDice = 0;
        public int dualWieldDice = 0;
        public int weaponSafeDice = 0;
        public int bonusSafeDice = 0;

        public int maxChanceToCounter = 1;
        public int chanceToCounterAttack;

        // Save specific variables.
        public string armorName;
        public string weaponName;
        public string secondaryWeaponName;
        public string shieldName;
        public int branzen;

        // Specifically armor.
        public Gear equippedArmor;
        public Gear equippedShield;

        // Can be armor or granting other bonuses.
        public Gear equippedHelmet;
        public Gear equippedGauntlet;
        public Gear equippedLegWear;

        // These can also grant armor, but often it is for other bonuses.
        public Gear wornClothes1;
        public Gear wornClothes2;

        // These will grant magical abilities and other bonuses.
        public Gear equippedJewelry1;
        public Gear equippedJewelry2;
        public Gear equippedJewelry3;

        // These will grant magical abilities and other bonuses.
        public Gear equippedArtifact1;
        public Gear equippedArtifact2;

        // Order of gear in array: helmet, armor, weapon, second_weapon, shield, gauntlet, legwear, clothes1, clothes2, jewelry1, jewelry2, jewelry3, artifact1, artifact2
        public Gear[] warriorGear = new Gear[14];

        public int weightOfEquipment;

        public int cost;
        public int restTime;

        // Used for points added in the beginning and from leveling up.
        public int addedHealthPoints = 0;
        public int addedStrengthPoints = 0;
        public int addedStaminaPoints = 0;
        public int addedSpeedPoints = 0;

        // Used for points added by equipment or used items.
        public int extraHealthPoints = 0;
        public int extraStrengthPoints = 0;
        public int extraStaminaPoints = 0;
        public int extraSpeedPoints = 0;
        public int extraComposurePoints = 0;
        // Kanske behöver fler varianter här...

        public static void AddIdStuff(Hero warrior)
        {
            warrior.alltimePartyId = Game.maximumPartyEnlisting+1;
            warrior.currentPartyId = Game.Party.Count+1;
        }
        public static void ReCalculateCurrentPartyId()
        {
            // This is needed after a battle where warriors might have been lost.
            for (int i = 0;i < Game.Party.Count;i++)
            {
                Game.Party[i].currentPartyId = i + 1;
            }
        }
        // Method for presenting a warrior somewhat more concise.
        public static void Present(Hero warrior)
        {
            Game.Wrt("This warrior's name is ");
            Game.WrtL("w","",warrior.name, true);
            Game.WrtL("Type: " + warrior.type);
            Game.WrtL("Health: " + warrior.healthpoints);
            Game.WrtL("Strength: " + warrior.strength);
            Game.WrtL("Stamina: " + warrior.stamina);
            Game.WrtL("Speed: " + warrior.speed);
            Game.WrtL("Armor: " + warrior.armor);
            Game.WrtL("Attack: " + warrior.attackDice);
        }
        // Method for getting a better look at a warrior, used in tavern and pause menu in the cave.
        public static void TakeAlookAt(Hero warrior)
        {
            Game.Wrt("w", "", warrior.name, true);
            Game.WrtL(" the " + warrior.type);
            Game.Wrt("Level: " + warrior.level);
            Game.WrtL(" - Dual Wield Level: " + warrior.dualWieldLevel);
            Game.Wrt("Experience: " + warrior.experience);
            Game.WrtL(" - Dual Wield Experience: " + warrior.dualWieldExperience);

            // Following two might be for test purpose only. I don't think end users need this info...
            Game.Wrt("Entered party as number: " + warrior.alltimePartyId);
            Game.WrtL(" - Current party position: " + warrior.currentPartyId);

            Game.Wrt("Health: ");
            if (warrior.healthpoints != warrior.maxHealth)
            {
                if (warrior.healthpoints < 4)
                    Game.Wrt("r", "", warrior.healthpoints, true);
                else
                    Game.Wrt("yl", "", warrior.healthpoints, true);
                Game.Wrt(" of ");
                Game.WrtL("w", "", warrior.maxHealth, true);
            }
            else
                Game.WrtL("w", "", " " + warrior.healthpoints + " ", true); // max

            Game.Wrt("Strength: ");
            if (warrior.strength != warrior.maxStrength)
            {
                Game.Wrt("yl", "", warrior.strength, true);
                Game.Wrt(" of ");
                Game.WrtL("w", "", warrior.maxStrength, true);
            }
            else
                Game.WrtL("w", "", " " + warrior.strength + " ", true); // max

            Game.Wrt("Stamina: ");
            if (warrior.stamina != warrior.maxStamina)
            {
                if (warrior.stamina < 4)
                    Game.Wrt("r", "", warrior.stamina, true);
                else
                    Game.Wrt("yl", "", warrior.stamina, true);
                Game.Wrt(" of ");
                Game.WrtL("w", "", warrior.maxStamina, true);
            }
            else
                Game.WrtL("w", "", " " + warrior.stamina + " ", true); // max

            Game.Wrt("Speed: ");
            if (warrior.speed != warrior.maxSpeed)
            {
                Game.Wrt("yl", "", warrior.speed, true);
                Game.Wrt(" of ");
                Game.Wrt("w", "", warrior.maxSpeed, true);
                if (warrior.weightOfEquipment > 0)
                    Game.WrtL(" - Equipment affects speed");
                else
                    Game.WrtL("");
            }
            else
                Game.WrtL("w", "", " " + warrior.speed + " ", true); // max

            Game.Wrt("Composure: ");
            if (warrior.composure != warrior.baseComposure)
            {
                if (warrior.composure < 4)
                    Game.Wrt("r", "", warrior.composure, true);
                else
                    Game.Wrt("yl", "", warrior.composure, true);
                Game.Wrt(" of ");
                Game.WrtL("w", "", warrior.baseComposure, true);
            }
            else
                Game.WrtL("w", "", " " + warrior.composure + " ", true); // max

            Game.Wrt("Armor: ");
            Game.Wrt("yl","",warrior.armor,true);
            if (warrior.equippedArmor != null || warrior.equippedShield != null)
            {
                Game.Wrt(" by ");
                if (warrior.equippedArmor == null)
                {
                    Game.Wrt("w", "", warrior.equippedShield.name, true);
                }
                else if (warrior.equippedShield == null)
                {
                    Game.Wrt("w", "", warrior.equippedArmor.name, true);
                }
                else
                {
                    Game.Wrt("w", "", warrior.equippedArmor.name, true);
                    Game.Wrt(" and ");
                    Game.Wrt("w", "", warrior.equippedShield.name, true);
                }
            }
            Game.Wrt("\n");

            Game.Wrt("Attack: ");
            Game.WrtL("yl", "", warrior.attackDice, true);
            if (warrior.isDualWielding)
            {
                Game.Wrt("Weapons: ");
                Game.Wrt("yl", "", warrior.equippedWeapon.name, true);
                Game.Wrt(" and ");
                Game.WrtL("yl", "", warrior.equippedSecondaryWeapon.name, true);
            }
            else
            {
                Game.Wrt("Weapon: ");
                if (warrior.equippedWeapon != null)
                {
                    Game.WrtL("yl", "", warrior.equippedWeapon.name, true);
                }
                else if (warrior.equippedSecondaryWeapon != null)
                {
                    Game.WrtL("yl", "", warrior.equippedSecondaryWeapon.name, true);
                }
                else
                {
                    Game.WrtL("yl", "", "None...", true);
                }
            }
            if (warrior.ailments.Count > 0)
            {
                // Need to ask to add the empty line...
                Game.WrtL("");
                PresentAilments(warrior);
            }
        }
        // Method that presents the party's vital stats in menus.
        public static void SeePartyVitals()
        {
            Game.WrtL("\nQuick look at the team:");

            for (int i = 0; i < Game.Party.Count; i++)
            {
                Hero warrior = Game.Party[i];

                if (Game.Party.Count < 4 || (Game.Party.Count < 6 && Game.partyMembersInPeril.Count < 2))
                {
                    Game.WrtL("w", "", "\n" + warrior.name + " the " + warrior.type, true);
                    SeeVitalStatsCompact(warrior);
                    PresentAilments(warrior);
                }
                else
                {
                    Game.Wrt("w", "", "\n" + warrior.name + " - " + Game.CapitalizeFirstLetter(warrior.type), true);
                    Game.Wrt(" - ");
                    SeeVitalsSuperCompact(warrior);
                    PresentAilmentsCompact(warrior);
                    Game.WrtL("");
                }
            }
        }
        // Method that presents a specific warriors vital stats.
        public static void SeeVitalStats(Hero warrior)
        {
            Game.Wrt("Health: ");
            Game.Wrt("yl", "", warrior.healthpoints, true);
            Game.Wrt(" of ");
            Game.Wrt("w", "", warrior.maxHealth, true);
            if (warrior.healthpoints < 4)
                Game.Wrt("r", "", " - Dangerously low! Next hit could be lethal.", true);
            Game.Wrt("\n");

            Game.Wrt("Stamina: ");
            Game.Wrt("yl", "", warrior.stamina, true);
            Game.Wrt(" of ");
            Game.Wrt("w", "", warrior.maxStamina, true);
            if (warrior.stamina < 4)
                Game.Wrt("r", "", " - Tired! Will affect attack badly.", true);
            Game.Wrt("\n");

            Game.Wrt("Composure: ");
            Game.Wrt("yl", "", warrior.composure, true);
            Game.Wrt(" of ");
            Game.Wrt("w", "", warrior.baseComposure, true);
            if (warrior.composure < 3)
                Game.Wrt("r", "", " - Loosing it soon!", true);
            Game.Wrt("\n");

            Game.Wrt("Experience: ");
            Game.WrtL("w", "", warrior.experience, true);
            Game.Wrt("Dual Wield Experience: ");
            Game.WrtL("w", "", warrior.dualWieldExperience, true);
            Game.Wrt("\n");
        }
        // Method that presents a specific warriors vital stats, but even more compact and shortened.
        public static void SeeVitalStatsCompact(Hero warrior)
        {
            Game.Wrt("Health: ");
            if (warrior.healthpoints < 4)
                Game.Wrt("r", "", warrior.healthpoints, true);
            else
                Game.Wrt("yl", "", warrior.healthpoints, true);
            Game.Wrt(" of ");
            Game.Wrt("w", "", warrior.maxHealth, true);

            Game.Wrt(" -- ");

            Game.Wrt("Stamina: ");
            if (warrior.stamina < 4)
                Game.Wrt("r", "", warrior.stamina, true);
            else
                Game.Wrt("yl", "", warrior.stamina, true);
            Game.Wrt(" of ");
            Game.Wrt("w", "", warrior.maxStamina, true);

            Game.Wrt(" -- ");

            Game.Wrt("Composure: ");
            if (warrior.composure < 3)
                Game.Wrt("r", "", warrior.composure, true);
            else
                Game.Wrt("yl", "", warrior.composure, true);
            Game.Wrt(" of ");
            Game.Wrt("w", "", warrior.baseComposure, true);

            Game.Wrt(" -- ");

            Game.Wrt("Exp: ");
            Game.Wrt("w", "", warrior.experience, true);
            Game.Wrt(" -- Dual Wield Exp: ");
            Game.Wrt("w", "", warrior.dualWieldExperience, true);

            Game.Wrt("\n");
        }
        // Method that presents a specific warriors vital stats, but super compact and on a single line.
        public static void SeeVitalsSuperCompact(Hero warrior)
        {
            Game.Wrt("H: ");
            if (warrior.healthpoints < 4)
                Game.Wrt("r", "", warrior.healthpoints, true);
            else
                Game.Wrt("yl", "", warrior.healthpoints, true);
            Game.Wrt("/");
            Game.Wrt("w", "", warrior.maxHealth, true);

            Game.Wrt(" - ");

            Game.Wrt("S: ");
            if (warrior.stamina < 4)
                Game.Wrt("r", "", warrior.stamina, true);
            else
                Game.Wrt("yl", "", warrior.stamina, true);
            Game.Wrt("/");
            Game.Wrt("w", "", warrior.maxStamina, true);

            Game.Wrt(" - ");

            Game.Wrt("C: ");
            if (warrior.composure < 3)
                Game.Wrt("r", "", warrior.composure, true);
            else
                Game.Wrt("yl", "", warrior.composure, true);
            Game.Wrt("/");
            Game.Wrt("w", "", warrior.baseComposure, true);

            Game.Wrt("XP: ");
            Game.Wrt("w", "", warrior.experience, true);
            Game.Wrt(" - DuWiXP: ");
            Game.Wrt("w", "", warrior.dualWieldExperience, true);
        }
        // Method that presents ailments of a specific warrior.
        public static void PresentAilments(Hero warrior)
        {
            if (warrior.ailments.Count > 0)
            {
                if (Game.HasAilment(warrior, Game.ailments[8])) // dying
                {
                    Game.WrtL("r", "", "Warrior is down and dying!", true);
                    if (Game.HasAilment(warrior, Game.ailments[0])) // bleeding
                    {
                        Game.WrtL(Game.CapitalizeFirstLetter(warrior.pronoun[0]) + "'s bleeding heavily, only magical resurrection will work.");
                    }
                }
                else
                {
                    Game.Wrt("r", "", "Suffers from ", false);

                    if (warrior.ailments.Count == 1)
                    {
                        Game.WrtL(warrior.ailments[0] + ".");
                    }
                    else
                    {
                        for (int i = 0; i < warrior.ailments.Count; i++)
                        {
                            Game.Wrt(warrior.ailments[i]);
                            if (warrior.ailments.Count > 2 && i < warrior.ailments.Count - 1)
                            {
                                Game.Wrt(", ");
                            }
                            else if (i != warrior.ailments.Count - 1)
                            {
                                Game.Wrt(" and ");
                            }
                        }
                        Game.WrtL(".");
                    }
                    Game.res();
                }
            }
        }
        // Method that presents ailments of a specific warrior, more compact.
        public static void PresentAilmentsCompact(Hero warrior)
        {
            if (warrior.ailments.Count > 0)
            {
                if (Game.HasAilment(warrior, Game.ailments[8])) // dying
                {
                    Game.Wrt("r", "", "Dying. ", false);
                    if (Game.HasAilment(warrior, Game.ailments[0])) // bleeding
                    {
                        Game.Wrt("Bleeding heavily, only magical resurrection.");
                    }
                    Game.res();
                }
                else
                {
                    Game.Wrt("Ailments: ");
                    Game.r();

                    if (warrior.ailments.Count == 1)
                    {
                        Game.Wrt(Game.CapitalizeFirstLetter(warrior.ailments[0]) + ".");
                    }
                    else
                    {
                        for (int i = 0; i < warrior.ailments.Count; i++)
                        {
                            if (i == 0)
                                Game.Wrt(Game.CapitalizeFirstLetter(warrior.ailments[i]));
                            else
                                Game.Wrt(warrior.ailments[i]);
                            if (warrior.ailments.Count > 2 && i < warrior.ailments.Count - 1)
                            {
                                Game.Wrt(", ");
                            }
                            else if (i != warrior.ailments.Count - 1)
                            {
                                Game.Wrt(" and ");
                            }
                        }
                        Game.Wrt(".");
                    }
                    Game.res();
                }
            }
        }
        // Method used in the initial creation of all the warrior types. Used to reduce warrior.extraPoints with points amount, and check that points doesn't use more than the remaining
        // warrior.extraPoints are.
        public static int UsePoints(int points, Hero warrior)
        {
            if (warrior.extraPoints - points < 0)
            {
                if (warrior.extraPoints <= 0)
                    warrior.extraPoints -= points;
                else
                {
                    points = warrior.extraPoints;
                    warrior.extraPoints = 0;
                }
            }
            else
                warrior.extraPoints -= points;

            return points;
        }
        // Method for updating stats that need to be recalculated based on weapons and armor, used only in the beginning when creating a character.
        public static void UpdateWarriorStatsBeginning(Hero warrior)
        {
            warrior.attackDice = 0;
            warrior.bonusAttackDice = 0;
            warrior.bonusDefenceDice = 0;
            warrior.bonusCriticalDice = 0;
            warrior.weaponSafeDice = 0;
            warrior.bonusSafeDice = 0;

            int ArmorDef = 0;
            int ArmorWeight = 0;

            if (warrior.equippedArmor != null)
            {
                ArmorDef = warrior.equippedArmor.defence;
                ArmorWeight = warrior.equippedArmor.weight;
                warrior.warriorGear[1] = warrior.equippedArmor;
                warrior.armorName = warrior.equippedArmor.name;
            }

            int ShieldDef = 0;
            int ShieldWeight = 0;

            if (warrior.equippedShield != null)
            {
                ShieldDef = warrior.equippedShield.defence;
                ShieldWeight = warrior.equippedShield.weight;
                warrior.warriorGear[4] = warrior.equippedShield;
                warrior.shieldName = warrior.equippedShield.name;
            }

            Gear currentWpn;

            int wpn1Atk = 0;
            int wpn1Def = 0;
            int wpnWght = 0;
            if (warrior.equippedWeapon != null)
            {
                currentWpn = warrior.equippedWeapon;
                warrior.weaponName = warrior.equippedWeapon.name;
                warrior.warriorGear[2] = warrior.equippedWeapon;
                warrior.weaponSafeDice += warrior.equippedWeapon.safeDice;
                wpn1Atk = warrior.equippedWeapon.attack;
                wpn1Def = currentWpn.defenceDice;
                for (var i = 0;i < currentWpn.types.Count;i++)
                {
                    if (currentWpn.types[i] == "heavy")
                    {
                        wpnWght += 1;
                    }
                    else if (currentWpn.types[i] == warrior.choiceOfWeapon[0] || currentWpn.types[i] == warrior.choiceOfWeapon[1])
                    {
                        warrior.bonusAttackDice += 1;
                    }
                }
            }
            int wpn2Atk = 0;
            int wpn2Def = 0;
            if (warrior.equippedSecondaryWeapon != null)
            {
                currentWpn = warrior.equippedSecondaryWeapon;
                warrior.secondaryWeaponName = warrior.equippedSecondaryWeapon.name;
                warrior.warriorGear[3] = warrior.equippedSecondaryWeapon;
                warrior.weaponSafeDice += warrior.equippedSecondaryWeapon.safeDice;
                wpn2Atk = warrior.equippedSecondaryWeapon.attack;
                wpn2Def = currentWpn.defenceDice;
                for (var i = 0; i < currentWpn.types.Count; i++)
                {
                    if (currentWpn.types[i] == "heavy")
                    {
                        wpnWght += 1;
                    }
                    else if (currentWpn.types[i] == warrior.choiceOfWeapon[0] || currentWpn.types[i] == warrior.choiceOfWeapon[1])
                    {
                        warrior.bonusAttackDice += 1;
                    }
                }
            }

            //double weightCalc = Math.Round((ArmorWeight + ShieldWeight + wpnWght) / 1.9);
            int weightCalc = ArmorWeight + ShieldWeight + wpnWght;
            warrior.weightOfEquipment = weightCalc; // (int)weightCalc

            warrior.armor = 0 + ArmorDef + ShieldDef + wpn1Def + wpn2Def;
            warrior.attackDice = wpn1Atk + wpn2Atk + warrior.bonusAttackDice;

            if (warrior.isDualWielding)
            {
                warrior.attackDice += warrior.dualWieldDice;
                if (warrior.attackDice < 1)
                    warrior.attackDice = 1;

                if (warrior.chanceToCounterAttack == 1)
                    warrior.chanceToCounterAttack++;
            }
            else
            {
                if (warrior.chanceToCounterAttack == 2)
                    warrior.chanceToCounterAttack--;
            }

            warrior.maxHealth = warrior.baseHealth + warrior.addedHealthPoints;
            warrior.healthpoints = warrior.maxHealth;
            warrior.maxStrength = warrior.baseStrength + warrior.addedStrengthPoints;
            warrior.strength = warrior.maxStrength;
            warrior.maxStamina = warrior.baseStamina + warrior.addedStaminaPoints + (warrior.strength / 3);
            warrior.stamina = warrior.maxStamina;
            warrior.maxSpeed = warrior.baseSpeed + warrior.addedSpeedPoints;
            warrior.speed = warrior.maxSpeed - warrior.weightOfEquipment;
        }
        // Method for updating stats that need to be recalculated based on weapons and armor, this is the version used in the rest of the game, after the beginning.
        public static void UpdateWarriorStats(Hero warrior)
        {
            // Resets these to let the method fill them up. That will reset item effects after battles, but equipped gear will put their points back here below.
            warrior.extraHealthPoints = 0;
            warrior.extraStrengthPoints = 0;
            warrior.extraStaminaPoints = 0;
            warrior.extraSpeedPoints = 0;
            warrior.extraComposurePoints = 0;

            warrior.attackDice = 0;
            warrior.bonusAttackDice = 0;
            warrior.armor = 0;
            warrior.bonusDefenceDice = 0;
            warrior.bonusCriticalDice = 0;
            warrior.weaponSafeDice = 0;
            warrior.bonusSafeDice = 0;

            // This is an attempt to work around a problem with diminishing speed when re-equipping things...
            warrior.speed += warrior.weightOfEquipment;
            if (warrior.speed > warrior.maxSpeed)
            {
                warrior.speed = warrior.maxSpeed;
                Game.WrtL("r", "", "\nSomething's wrong! speed was greater than max speed in UpdateWarriorStats!\n", true);
            }

            warrior.weightOfEquipment = 0;

            int currentComposure = warrior.composure;
            int currentChanceToCounterAttack = warrior.chanceToCounterAttack;

            int ArmorDef = 0;
            int ArmorWeight = 0;

            if (warrior.equippedArmor != null)
            {
                ArmorDef = warrior.equippedArmor.defence;
                ArmorWeight = warrior.equippedArmor.weight;
            }

            int ShieldDef = 0;
            int ShieldWeight = 0;

            if (warrior.equippedShield != null)
            {
                ShieldDef = warrior.equippedShield.defence;
                ShieldWeight = warrior.equippedShield.weight;
            }

            Gear currentWpn;

            int wpn1Atk = 0;
            int wpn1Def = 0;
            int wpnWght = 0;
            if (warrior.equippedWeapon != null)
            {
                warrior.weaponSafeDice += warrior.equippedWeapon.safeDice;
                currentWpn = warrior.equippedWeapon;
                wpn1Atk = warrior.equippedWeapon.attack;
                wpn1Def = currentWpn.defenceDice;
                for (var i = 0; i < currentWpn.types.Count; i++)
                {
                    if (currentWpn.types[i] == "heavy")
                    {
                        wpnWght += 1;
                    }
                    else if (currentWpn.types[i] == warrior.choiceOfWeapon[0] || currentWpn.types[i] == warrior.choiceOfWeapon[1])
                    {
                        warrior.bonusAttackDice += 1;
                    }
                }
            }
            int wpn2Atk = 0;
            int wpn2Def = 0;
            if (warrior.equippedSecondaryWeapon != null)
            {
                warrior.weaponSafeDice += warrior.equippedSecondaryWeapon.safeDice;
                currentWpn = warrior.equippedSecondaryWeapon;
                wpn2Atk = warrior.equippedSecondaryWeapon.attack;
                wpn2Def = currentWpn.defenceDice;
                for (var i = 0; i < currentWpn.types.Count; i++)
                {
                    if (currentWpn.types[i] == "heavy")
                    {
                        wpnWght += 1;
                    }
                    else if (currentWpn.types[i] == warrior.choiceOfWeapon[0] || currentWpn.types[i] == warrior.choiceOfWeapon[1])
                    {
                        warrior.bonusAttackDice += 1;
                    }
                }
            }

            for (int i = 0;i < warrior.warriorGear.Length;i++)
            {
                if (warrior.warriorGear[i] != null && warrior.warriorGear[i].specialPower1 != "None")
                {
                    Gear.EquippedGearFunction(warrior, warrior.warriorGear[i].specialPower1, warrior.warriorGear[i].specialPower1Value, true);
                    // Runs console clear to get rid of comments that is printed by these methods that is used at other places but not needed here.
                    Console.Clear();
                }
                if (warrior.warriorGear[i] != null && warrior.warriorGear[i].specialPower2 != "None")
                {
                    Gear.EquippedGearFunction(warrior, warrior.warriorGear[i].specialPower2, warrior.warriorGear[i].specialPower2Value, true);
                    // Same.
                    Console.Clear();
                }
            }

            // double weightCalc = Math.Round((warrior.weightOfEquipment + ArmorWeight + ShieldWeight + wpnWght) / 1.9);
            int weightCalc = warrior.weightOfEquipment + ArmorWeight + ShieldWeight + wpnWght;
            warrior.weightOfEquipment = weightCalc; // (int)weightCalc

            warrior.armor += ArmorDef + ShieldDef + wpn1Def + wpn2Def + warrior.bonusDefenceDice;
            warrior.attackDice += wpn1Atk + wpn2Atk + warrior.bonusAttackDice;

            if (warrior.isDualWielding)
            {
                warrior.attackDice += warrior.dualWieldDice;
                if (warrior.attackDice < 1)
                    warrior.attackDice = 1;

                // NOTE! It feels like one wants a extraChanceToCounter as well, enabling weapons and other gear to add to this. If implemented, this math has to be changed.

                if (currentChanceToCounterAttack == warrior.maxChanceToCounter)
                    warrior.chanceToCounterAttack += 1 + (warrior.dualWieldLevel/4);
            }
            else
            {
                if (currentChanceToCounterAttack > warrior.maxChanceToCounter)
                    warrior.chanceToCounterAttack = warrior.maxChanceToCounter;
            }

            // NOTE! This might need some rethinking. extraSOMETHINGPoints maybe shouldn't be added on to maxSOMETHING...
            // If changed it will affect the function of the leveling up method.
            warrior.maxHealth = warrior.baseHealth + warrior.addedHealthPoints + warrior.extraHealthPoints;
            if (warrior.healthpoints == warrior.baseHealth + warrior.addedHealthPoints)
                warrior.healthpoints = warrior.maxHealth;
            else
                warrior.healthpoints += warrior.extraHealthPoints;

            warrior.maxStrength = warrior.baseStrength + warrior.addedStrengthPoints + warrior.extraStrengthPoints;
            if (warrior.strength == warrior.baseStrength + warrior.addedStrengthPoints) // if stat skill is full.
                warrior.strength = warrior.maxStrength;
            else // stat skill is not full, add only extraPoints.
                warrior.strength += warrior.extraStrengthPoints;

            warrior.maxStamina = warrior.baseStamina + warrior.addedStaminaPoints + (warrior.strength / 3) + warrior.extraStaminaPoints;
            if (warrior.stamina == warrior.baseStamina + warrior.addedStaminaPoints) // if stat skill is full.
                warrior.stamina = warrior.maxStamina;
            else // stat skill is not full, add only extraPoints.
                warrior.stamina += warrior.extraStaminaPoints;

            warrior.maxSpeed = warrior.baseSpeed + warrior.addedSpeedPoints + warrior.extraSpeedPoints;
            if (warrior.speed == warrior.baseSpeed + warrior.addedSpeedPoints) // if stat skill is full.
                warrior.speed = warrior.maxSpeed;
            else // stat skill is not full, add only extraPoints.
                warrior.speed += warrior.extraSpeedPoints;

            warrior.speed -= warrior.weightOfEquipment;

            if (warrior.speed < 0)
                warrior.speed = 0; // Maybe there should be a system for detecting and warning speed becoming 0...?

            warrior.composure = currentComposure + warrior.extraComposurePoints;
        }
        // Method for leveling up a specific warrior, and sending the player to the menu for choosing which stat to upgrade.
        public static void LevelUp(Hero warrior, int newLevel)
        {
            warrior.extraPoints = 1;

            if (newLevel % 6 == 0)
            {
                warrior.chanceToCounterAttack++;
            }

            warrior.level++;

            Menu.AddNewLevelPointMenu(warrior);
        }
        // Method for leveling up dual wielding for a specific warrior.
        public static void LevelUpDualWielding(Hero warrior, int newLevel)
        {
            //dualWieldLevelStepData = { -3, 0, -2, 1, -1, 0, 0, 1, 1, 0, 1, 1, 2, 0, 2, 1, 3, 0,
            //                            3, 1, 3, 1, 4, 0, 4, 1, 4, 2, 5, 0, 5, 2, 6, 1, 6, 2, 7, 3 };
            warrior.dualWieldLevel++;

            int oldDWDice = warrior.dualWieldDice;
            warrior.dualWieldDice = Game.dualWieldLevelStepData[(newLevel-2)*2];
            if (warrior.dualWieldDice != oldDWDice)
            {
                Game.WrtL("Warrior's bonus dice for dual wielding increased by one.");
            }

            // Game.WrtL("old chanceToCounterAttack: " + warrior.chanceToCounterAttack);

            warrior.maxChanceToCounter += Game.dualWieldLevelStepData[((newLevel-2)*2) + 1];
            warrior.chanceToCounterAttack = warrior.maxChanceToCounter;
            int newCounterDice = Game.dualWieldLevelStepData[((newLevel - 2) * 2) + 1];
            if (newCounterDice > 0)
            {
                Game.WrtL("Warrior's chance to counter attack increased by " + newCounterDice + ".");
            }

            UpdateWarriorStats(warrior);

            //Game.WrtL("counter points to add: " + Game.dualWieldLevelStepData[((newLevel - 2) * 2) + 1]);
            //Game.WrtL("new chanceToCounterAttack: " + warrior.chanceToCounterAttack);
            //Game.WrtL("dualWieldDice: " + warrior.dualWieldDice);
        }
        // Method for presenting base skills of the players chosen warrior type.
        public static void PresentPlayerBaseSkills(Hero Player)
        {
            Game.WrtL("This is now your base skills:\n");
            Game.WrtL("Health: " + Player.healthpoints);
            Game.WrtL("Strength: " + Player.strength);
            Game.WrtL("Stamina: " + Player.stamina);
            Game.WrtL("Speed: " + Player.speed);
        }
        // Method for printing a specific warrior's battle cry.
        public static void BattleCry(Hero warrior)
        {
            Game.Wrt(warrior.battlecry);
        }
        // Method for randomizing the gender of a warrior, and assigning them a list of pronouns.
        public static void randGender(Hero warrior)
        {
            int genderNr = Game.randomize.Next(3);

            setGender(warrior, genderNr);
        }
        // Method for assigning the correct list of pronouns based on chosen gender to a specific warrior.
        public static void setGender(Hero warrior, int genderNr)
        {
            switch (genderNr)
            {
                case 0:
                    warrior.gender = "male";
                    warrior.pronoun[0] = "he";
                    warrior.pronoun[1] = "him";
                    warrior.pronoun[2] = "his";
                    warrior.pronoun[3] = "guy";
                    break;
                case 1:
                    warrior.gender = "female";
                    warrior.pronoun[0] = "she";
                    warrior.pronoun[1] = "her";
                    warrior.pronoun[2] = "her";
                    warrior.pronoun[3] = "gal";
                    break;
                case 2:
                    warrior.gender = "non binary";
                    warrior.pronoun[0] = "they";
                    warrior.pronoun[1] = "them";
                    warrior.pronoun[2] = "their";
                    warrior.pronoun[3] = "person";
                    break;
            }
        }
        // Method for getting the index number of the gender by string name. Not used right now, might come in handy...
        public static int getGenderNr(string genderType)
        {
            int genderNr = -1;
            switch (genderType)
            {
                case "male":
                    genderNr = 0;
                    break;
                case "female":
                    genderNr = 1;
                    break;
                case "non binary":
                    genderNr = 2;
                    break;
            }
            return genderNr;
        }

        // Method for randomizing names for warriors. Important stuff.
        public static string NameGenerator(string gend)
        {
            string firstname = "";
            string lastname;
            string newName;

            string slumpname = "";

            int slump1;
            int slump2;

            // I should eventually load the names from a txt-file instead...
            string[] mNames = { "Thor", "Loki", "Rahst", "Pippin", "Sven", "Jaan", "Rohan", "Grom", "Yurigan", "Conan", "Sam", "Aragorn", "Boyd", "Serdarr", "Jauk", "Jonn", "Yoann", "Petrik",
                                "Drake", "Zindahr", "Xion", "Hank", "Biuras", "Dante", "Worf", "Kim", "Casheem", "Brunte", "Jorr", "Gimli", "Ceasar", "Tom", "Ash", "Marduk", "Zahr", "Pomm",
                                "Woire", "Gant", "Bennek", "Kale", "Qirak", "Eric", "Obi", "Ben", "Luke", "Han", "Sharuk", "Hrithik", "Link", "Dorf", "Freddy", "Omar", "Simeon", "Odd", "Lahr",
                                "Spike", "Giles","Angelos", "Lorne", "Rupert", "Snake", "Baccus", "Toby", "Gideon", "Poe", "Edgar", "Rauk", "Tikon", "Yohann", "Ameer", "Connor", "Spock", "Rohit",
                                "Roy", "Clay", "Rayleigh", "Gash", "Jack", "Slade", "Oak", "Stiles", "Scott", "Dalton", "Gonn", "Prince", "Raide", "Zuko", "Aang", "Sokka", "Iroh", "Cid", "Finn",
                                "Marco", "Zack", "Vernon", "Borat", "Zigyon", "Rufus", "Ranveer", "Ravesh", "Bucky", "Budley", "Roydas", "Tang", "Derick", "Beat", "Timothee", "Ryle", "Rex", "Bo",
                                "Rocky", "Dean", "Bazz", "Freid", "Bill", "Bjohrn", "Dainehl", "Muahn", "Derias", "Mathies", "Henrique", "Caspian", "Callisto", "Corben", "Stefán", "Tyke", "Chad",
                                "Buzz", "Boon", "Effin", "Effamor", "Bahr", "Clyde", "Eufreed", "Freyd", "Fried", "Perath", "Jiro", "Todd", "Glyde", "Sennek", "Baronn", "Mark", "Noah", "Bayne",
                                "Dom", "Dominic", "Max", "Locke", "Buhga", "Eodal", "Sark", "Ponthe", "Brock", "Boris", "Hector", "Sanoak", "Javier", "Igor", "Kwade", "Njord", "Kye", "Yondu",
                                "Hawke", "Chopper", "Victor", "Robh", "Rode", "Bahrd", "Horatio", "Gohan", "Pudh", "Gero", "Chrono", "Goku", "Muad'Dib", "Eldemar", "Byron", "Geralt", "Baltu",
                                "Derris", "Dirk", "Ivan", "Durra", "Mico", "Bob", "Seth", "Higgs", "Nobu", "Ranma", "Juran", "Dorn", "Darth", "Zod", "Fyonn", "Birk", "Furn", "Gebralto", "Jarren",
                                "Odney"};
            string[] fNames = { "Freya", "Jaana", "Becca", "Teera", "Ilveereh", "Ylva", "Diina", "Ana", "Opiana", "Terra", "Donna", "Oscilla", "Hildegard", "Viola", "Buffy", "Liana", "Helioma",
                                "Cordelia", "Brunhilde", "Girelle", "Kastania", "Quineira", "Leia", "Padme", "Cortana", "Yisme", "Katrina", "Alia", "Alita", "Zelda", "Farore", "Nayru", "Epona",
                                "Tanya", "Astra", "Faye", "Lyca", "Phenicia", "Shira", "Shakira", "Ghani", "Xena", "Enya", "Winifred", "Ellie", "Simone", "Ciara", "Emilia", "Astrid", "Thalia",
                                "Dana", "Lara", "Laurice", "Veera", "Pam", "Lu Lu", "Lois", "Peña", "Lena", "Carolynn", "Paru", "Hanneke", "Joanna", "Eliza", "Erica", "Rosanne", "Gia", "Wanda",
                                "Fray", "Yuki","Pyrinne", "Rei", "Beema", "Tori", "Inica", "Erin", "Tylde", "Marle", "Ayla", "Schala", "Nikka", "Katara", "Suki", "Mae", "Ezra", "Jyn", "Ethel",
                                "Marci", "Zee-zee", "Durma", "Contradictoria", "Governia", "Zandrah", "Kat", "Haylee", "Ru-Weena", "Reena", "Catniss", "Violynn", "Theia", "Gaia", "Elsbeth",
                                "Alice", "Inez", "Iridia", "Lyra", "Uhma", "Jenna", "Uruba", "Ehryn", "Callista", "Rona", "Rudeena", "Kay-Lin", "Correena", "Kanni", "Korrowyn", "Windelia", "Bai",
                                "Boona", "Otra", "Iktaria", "Rangeela", "Effámi", "Effinea", "Goonam", "Eufrat", "Freeda", "Gina", "Tani", "Stephania", "Ireena", "Senni", "Myra", "Nova", "Nour",
                                "Sarah", "Minique", "Mia", "Elena", "Ahsoka", "Tuhla", "Zuki", "Binha", "Hekla", "Sanooki", "Hectoveline", "Crystal", "Rynnei", "Katla", "Joo-nya", "Kazine", "Zoe",
                                "Diamanda", "Shurica", "Cassie", "Victoria", "Faura", "Farine", "Mania", "Vereis", "Chi Chi", "Aniana", "Aniara", "Bulma", "Uma", "Chani", "Zendaya", "Taki", "Lin",
                                "Idun", "Ezmeralda", "Hellandra", "Arwen", "Serpentra", "Meeri", "Zophia", "Qenna", "Yalena", "Inka", "Juneya", "June", "Juleya", "July", "Julia", "Zhuli", "Rani",
                                "Akane", "Jurya", "Dolores", "Dornea", "Pollee", "Ponea", "Dores", "Violet", "Sequoia", "Sigourney", "Dráni", "Kaycee", "Kei-Lee", "Fiona", "Fiora", "Ronya",
                                "Ilia"};
            string[] aNames = { "Kim", "Din", "Tiger", "Yuri", "Gil", "Lee", "Shyle", "Silver", "Paye", "Yiga", "Peru", "Eliot", "Angel", "Lucca", "Cyril", "Cinder", "Gilden", "Viper", "Gynn",
                                "Kalifa", "Akira", "Yanni", "Eid", "Zeer", "Shiigo", "Page", "Tau", "Lou", "Wu", "Seraf", "Yale", "Gura", "Spark", "Shadda", "Innotah", "Gyudan", "Kaisle",
                                "Rue", "Woe", "Toni", "Marine", "Azure", "Cyann", "Ivy", "Gendra", "Scaar", "Platine", "Row", "Raiden", "Freed", "Treen", "Gizmoe", "Gandel", "Coco", "Ryhs",
                                "Cassio", "Samus", "Ripley", "Arnie", "Valian", "Que", "Jayne", "Shade", "Shane", "Willow", "Eisle", "Thorn", "Guy", "Heeda", "Cannif", "Tryn", "Spyre", "Raki",
                                "Zenuhl", "Quint", "Cleo", "Wynde", "Logoe", "Quip", "Dash", "Singe", "Toph", "Rune", "Zeke", "Gal", "Roe", "Raven", "Reese", "Ray", "Teal", "Cricket", "Taren",
                                "Riley", "Lake", "Fang", "Torque", "Grue", "Waka", "Iohm", "Ohmu", "Mathines", "Yu-Ehl", "Cal", "Wynne", "Blaze", "Effios", "Fuego", "Urun", "Ouros", "Eyren",
                                "Ghouma", "Mako", "Kunei", "Kona", "Kanno", "Toubo", "Caspe", "Callyste", "Freyn", "Gylde", "Sennoe", "Mazeda", "Miriun", "Nouinn", "Saren", "Niqué", "Ocra",
                                "Brass", "Brynn", "Juno", "Yuno", "Timmy", "Cercei", "Tundra", "Sun", "Syllek", "Mace", "Lynx", "Sankei", "Cascade", "Vailuh", "Cush", "Ionith", "Ion", "Jai",
                                "Malachi", "Zompa", "Skye", "Perilou", "Xoriel", "Victoree", "Vic", "Robin", "Bon", "Bonée", "Bokka", "Sai", "Zeta", "Zeire", "Lasham", "Rye", "Zakune", "Ico",
                                "Araki", "Sora", "Takei", "Verse", "Echo", "Ywen", "Egeele", "Hirra", "Gaara", "Mieko", "Mieke", "Mikko", "Gorna", "Voile", "Rugele", "Vígill", "Hayste", "Zeko",
                                "Sephy", "Ran", "Ghala", "Juréi", "Doréi", "Pon", "Jinx", "Vye", "Kylo", "Ace", "Eldraan", "Draneis", "Oraga", "Nimago", "Fionne"};

            string[] lNames = { "Wylde", "Mortifer", "Wick", "Parandas", "Khan", "Roshan", "Tu Meri", "the Adventurer", "Kenobi", "Sohl", "Sonda", "Girandyr", "Azrael", "Bond", "Pond", "Tiberius",
                                "Picard", "Wellefair", "Joust", "Sturdwall", "Pocayne", "Kain", "Rodhapagos", "Zhul", "Sirren", "Omnicos", "Nightglow", "SteelBarer", "Vox", "Mazenda", "Hick", "",
                                "Garnell", "Ton Girann", "Kuh Yorenn", "Ahk Uthirion", "Suleiman", "Vei Korra", "Corals", "Pikku", "Orakazmir", "Gunnday", "Simmba", "Kuh Illdahr", "Leafling", "",
                                "Barrudharos", "Qurrado", "Amenos", "Thanatos", "Vei Deneryan", "Ahk Aderith", "Masraik", "Ton Errigo", "the Seeker", "Bold", "Stark", "Frank", "Grove", "Groove", "",
                                "Vos Kevvon", "Madaraaki", "Kaneda", "Aran", "San Dooze", "Ton Waruk", "Ahk Zideian", "Zidane", "Strife", "San Gyun", "Barreidas", "Port", "EldsGate", "RensGate",
                                "Lyons", "Smithsman", "BladeForger", "PionGate", "Kuh Arkoss", "Andross", "Bannerman", "Weavers", "Moontide", "Crescentia", "Dyhvel", "Vekkio", "Meadow", "Jinn Salu",
                                "Jinn Eiko", "Vos Ikkyca", "Jinn Toro", "Jinn Fey", "Jadoo", "Mehra", "Auroras", "Kakariko", "Fern", "River", "Cliff", "Kuh Sentall", "Ton Kaska", "Ihl", "Leandre",
                                "Rook", "the Vagabond", "Ell", "Arrowheart", "Edgewalker", "Tu Taradi", "Woolfe", "Snowe", "Ganonda", "Fowes", "Wex", "Fallahan", "Thylde", "Hencman", "Sparrow", "",
                                "Burrden", "Ganze", "Mohraniss", "Hyde", "Peer", "Scout", "Tahr", "Venndoer", "Quinn", "Cirrus", "Brakken", "Iridius", "Enigmicus", "Gamma", "Qurios", "Magus", "Kog",
                                "Ashtear", "Paile", "Amidala", "Dameron", "Calrissian", "Erso", "Imwe", "Omega", "Omnicus", "Sayden", "Yallohr", "Ironclad", "Ironheart", "Woodes", "Salios", "Fogg",
                                "Kane", "Zhuda", "StormGate", "Turrican", "Ganymedes", "Cattarol", "Hegemonium", "Backer", "Raghav", "Wavebraker", "Daywalker", "Hoardahl", "Daybringer", "Byhr", "",
                                "Nightfender", "Gander", "Nightwatcher", "Irrifyos", "Zonai", "Larza", "Ellix", "Lon Alyxes", "Ahk Farathe", "Lon Belleida", "Lon Cernéis", "Lon Dúrnes", "Vos Rudahk",
                                "Lon Effigho", "Dihrael", "Cosmo", "Slinger", "Eldrin", "Loom", "San Kyoh", "Sprygg", "Ahk Terrenios", "Tyrannicus", "San Chun", "Linger", "Jinn Gau", "Guhlu", "Zun",
                                "Ton Obahn", "Vei Liryos", "Languale", "Mahogny", "Vei Uriceic", "BrimsGate", "Brimston", "Brynes", "Gahrdian", "Neverfall", "Neidren", "Othellon", "StarsGate", "",
                                "Portis", "Porter", "Ahk Ytremhenek", "Ythis", "Inagis", "Ahk Ithiliar", "Vos Entarr", "Vos Polluq", "the Redeemed", "Achero", "Vos Hehryf", "Waka", "Reeves", "Kye",
                                "Aroborous", "Fuagamimos", "Fuerente", "Renzei", "Weyan", "Yi-Tzai", "Gibraltar", "Sodderton", "Adderlash", "Arrowtip", "Modura", "Godjira", "Kinora", "Zenida", "",
                                "Carano", "Illyad", "Illuminos", "Illyriahn", "Abel", "Lon Furios", "Lon Gemeira", "Gamora", "Kantarr", "Beam", "Kruehd", "Govvernoh", "Admirrahl", "Mynnist'r", "",
                                "Boke", "Stoak", "Menoak", "Novak", "Torrente", "Toretto", "Melchior", "Shippe", "Toppler", "Sa'gawak", "Chelege", "Kan'maguhr", "Breache", "FloodGate", "Bourne", "",
                                "Sollemen", "Hidwell", "Treadwell", "Deepwell", "Winn Dew", "Tano", "Hondo", "Soare", "Strack", "Strucker", "Girrakos", "Hoode", "Cape", "Cloake", "Hyder", "Veile",
                                "Robes", "Bokum", "Barras", "Bendon", "Bakeeros", "Boson", "Bennitou", "Bisolli", "Biggun", "Lobard", "Tashkaruga", "Ikaruga", "Verdeste", "Torres", "Carduic", "Zhou",
                                "Torne", "Thorn", "Reikaruga", "Anekaruga", "Zenkaruga", "Kakarott", "Sainouda", "Toriyama", "Dune", "Fremen", "Bhala", "Ekydohr", "Zanoga", "Zanida", "Zaneka", "",
                                "Zakuhra", "Zarashi", "Zafraihm", "Wynthell", "Bazra", "Vei Tarra", "Riot", "Ehnder", "Kopernikus", "Madalena", "Thodeo", "Hertz", "Quayke", "Winn Kouw", "Illydain",
                                "Winn Gon", "Barthas", "Etherat", "Ward", "Wyrde", "Tu Azadi", "Black", "Tu Ami", "Tu Asali", "Harrk", "Tu Tegori", "Tu Dori", "Gohrz", "Winn Pow", "Ohmb", "Winn Bao",
                                "Roth", "Nadellar", "Wielde", "Waile", "Weiroh", "Winn Tzu", "the Enlightened", "Kaleido", "San Zee", "Bidahr", "Tykonik", "Gallehk", "Logio", "Ruínde", "Zeneyro", "",
                                "Sagabadoth", "Damalcon", "Castanye", "Caste", "Dorontes", "Tenyand", "Tuu-Po", "Serengheti", "Cordi'ahl", "Gymelith", "Karn'agios", "Vander", "Vader", "Sidi-Ooce",
                                "Ren", "Astahla", "Iok'muhn", "Iso'tohp", "Eeki'sol", "Perangi", "Sagadimm", "Helmuth", "Hillbore", "Mat'astrad", "Uhnweily", "Immestra", "Hypolus", "Ashalloum", "",
                                "B'ko", "N'gao", "Tch'ree", "J'dei", "U'tao", "K'moor"};

            if (gend == "male")
            {
                // Kolla upp igen hur man sätter ihop listor till en.
                slump1 = Game.randomize.Next(2);

                if (slump1 == 0)
                {
                    slump2 = Game.randomize.Next(mNames.Length);
                    slumpname = mNames[slump2];
                }
                else if (slump1 == 1)
                {
                    slump2 = Game.randomize.Next(aNames.Length);
                    slumpname = aNames[slump2];
                }

                firstname = slumpname;
            }
            else if (gend == "female")
            {
                slump1 = Game.randomize.Next(2);

                if (slump1 == 0)
                {
                    slump2 = Game.randomize.Next(fNames.Length);
                    slumpname = fNames[slump2];
                }
                else if (slump1 == 1)
                {
                    slump2 = Game.randomize.Next(aNames.Length);
                    slumpname = aNames[slump2];
                }

                firstname = slumpname;
            }
            else if (gend == "non binary")
            {
                slump1 = Game.randomize.Next(3);

                if (slump1 == 0)
                {
                    slump2 = Game.randomize.Next(mNames.Length);
                    slumpname = mNames[slump2];
                }
                else if (slump1 == 1)
                {
                    slump2 = Game.randomize.Next(fNames.Length);
                    slumpname = fNames[slump2];
                }
                else if (slump1 == 2)
                {
                    slump2 = Game.randomize.Next(aNames.Length);
                    slumpname = aNames[slump2];
                }

                firstname = slumpname;
            }

            slump1 = Game.randomize.Next(lNames.Length);

            lastname = lNames[slump1];

            if (lastname == "")
                newName = firstname;
            else
                newName = firstname + " " + lastname;

            return newName;
        }
        // Method for randomizing a battle cry to a randomized warrior.
        public static string randCry(int warriorType)
        {
            string battleCry;

            int rand = Game.randomize.Next(9);

            battleCry = Game.warriorBattlecries[warriorType, rand];

            return battleCry;
        }
        // Method for randomizing armor to a randomized warrior.
        public static Gear randArmor(int warriorLvl, string armorType, string optionalSearch) // optionalSearch is an attempt to make this method future proof... Isn't used yet.
        {
            Gear armor;

            List<Gear> armorAvailable = Gear.allGear.Where(armor => armor.types[0] == armorType && armor.wieldLvl <= warriorLvl && armor.specialPower1 == optionalSearch).ToList();

            int randArm = Game.randomize.Next(armorAvailable.Count);
            armor = armorAvailable[randArm];

            return armor;
        }
        // Method for randomizing a weapon or secondary weapon to a randomized warrior.
        public static Gear randWeapon(Hero warrior, int warriorLvl, string weaponType1, string weaponType2, string whichHand, string optionalSearch)
        {
            Gear wpn;

            List<Gear> weaponsAvailable = new List<Gear>();

            for (int i = 0;i < Gear.allGear.Count; i++)
            {
                for (int j = 0; j < Gear.allGear[i].types.Count; j++)
                {
                    if (whichHand == "first")
                    {
                        if (warrior.isDualWielding)
                        {
                            if (Gear.allGear[i].isDoubleHandheld == false && Gear.allGear[i].wieldLvl <= warriorLvl && Gear.allGear[i].dualWieldLvl <= warriorLvl && 
                                Gear.allGear[i].specialPower1 == optionalSearch && (Gear.allGear[i].types[j] == weaponType1 || Gear.allGear[i].types[j] == weaponType2))
                                weaponsAvailable.Add(Gear.allGear[i]);
                        }
                        else
                        {
                            if (Gear.allGear[i].wieldLvl <= warriorLvl && Gear.allGear[i].specialPower1 == optionalSearch &&
                               (Gear.allGear[i].types[j] == weaponType1 || Gear.allGear[i].types[j] == weaponType2))
                                weaponsAvailable.Add(Gear.allGear[i]);
                        }
                    }
                    else
                    {
                        if (Gear.allGear[i].isDoubleHandheld == false && Gear.allGear[i].wieldLvl <= warriorLvl && Gear.allGear[i].dualWieldLvl <= warriorLvl && Gear.allGear[i].specialPower1 == optionalSearch && (Gear.allGear[i].types[j] == weaponType1 || Gear.allGear[i].types[j] == weaponType2))
                            weaponsAvailable.Add(Gear.allGear[i]);
                    }
                }
            }
            int randWpn = Game.randomize.Next(weaponsAvailable.Count);
            wpn = weaponsAvailable[randWpn];

            if (wpn.isDoubleHandheld)
                warrior.isUsingDoubleHandedWeapon = true;

            return wpn;
        }
        // Method for slowly regaining stats lost from poisoning for specific warrior.
        public static void RecoverFromPoisoning(Hero warrior)
        {
            switch (warrior.recoverFromPoisoningCounter)
            {
                case 4:
                    warrior.strength += 2;
                    if (warrior.strength >= warrior.maxStrength)
                    {
                        warrior.strength = warrior.maxStrength;
                        warrior.recoverFromPoisoningCounter = 0;
                    }
                    break;
                case 1:
                    warrior.strength = warrior.maxStrength;
                    break;
                default:
                    warrior.strength += warrior.maxStrength / 3;
                    if (warrior.strength >= warrior.maxStrength)
                    {
                        warrior.strength = warrior.maxStrength;
                        warrior.recoverFromPoisoningCounter = 0;
                    }
                    break;
            }
            if (warrior.recoverFromPoisoningCounter > 0)
                warrior.recoverFromPoisoningCounter--;
        }
        // Method for when resting in battle.
        public static void Rest(Hero warrior)
        {
            Game.WrtL(warrior.name + " decides to rest and stand " + warrior.pronoun[2] + " ground.\n");

            warrior.stamina += 3;
            warrior.restTime++;

            if (warrior.restTime > 1)
            {
                warrior.stamina += 2;

                Game.WrtL("dy", "", "stamina + 5", true);

                if (warrior.composure < warrior.baseComposure && warrior.consecutiveBigDamages == 0)
                {
                    warrior.composure++;
                    Game.WrtL("dy", "", "composure + 1", true);
                }
            }
            else
            {
                Game.WrtL("dy", "", "stamina + 3", false);
                Game.WrtL("defence + 3");
                Game.res();
            }

            if (warrior.stamina > warrior.maxStamina)
                warrior.stamina = warrior.maxStamina;
        }
        // Method for attacking in battle.
        public static void Attack(Hero warrior, Monster beast)
        {
            // Checks whether the warrior is up for it really.
            if (warrior.stamina == 0)
            {
                Game.WrtL(warrior.name + "'s stamina is completely drained, " + warrior.pronoun[0] + " can't attack!");
                return;
            }
            else
            {
                // They are. Best yell at the beast.
                Game.Wrt("\"");
                BattleCry(warrior);
                Game.WrtL("\"\n");
                Game.WrtL(warrior.name + " attacks the beast with all " + warrior.pronoun[2] + " might!\n");
            }

            // warrior.consecutiveMisses = 0;
            // warrior.consecutiveBigDamages = 0;
            // warrior.consecutiveDodges = 0;
            // warrior.consecutiveCriticalHits = 0;

            int XPgained = 0;

            int statAttack = warrior.strength + warrior.speed + warrior.level;
            int weaponAttack = warrior.attackDice; // Here will be added any weapon specific bonuses towards this monster.

            int critical = warrior.equippedWeapon.criticalChance + warrior.bonusCriticalDice;

            if (warrior.equippedSecondaryWeapon != null)
                critical += warrior.equippedSecondaryWeapon.criticalChance;

            if (warrior.composure > 10)
            {
                weaponAttack++;
                if (warrior.composure > 13)
                {
                    weaponAttack++;
                    critical++;
                }
            }
            else if (warrior.composure < 6)
            {
                weaponAttack--;
                if (warrior.composure < 3)
                {
                    weaponAttack--;
                    if (critical > 1)
                        critical = 1;
                }
            }

            int monsterDefence; 
            
            if (beast.stunCounter > 0)
            {
                monsterDefence = beast.speed + beast.armor;
            }
            else
            {
                monsterDefence = beast.level + beast.speed + beast.armor + beast.restTime;
                monsterDefence += beast.night_level + beast.night_speed;
            }

            if (beast.composure == 0)
            {
                monsterDefence = monsterDefence/2;
            }

            int sumOfAttack = statAttack + weaponAttack;
            if (warrior.restTime > 0)
                sumOfAttack += warrior.restTime;

            int rolledDie;
            int warriorSixes = 0;
            int monsterSixes = 0;
            int finalHits = 0;
            int warriorDiceSides = 6;
            int monsterDiceSides = 6;
            bool gotCritical = false;

            // Test purposes
            //Game.WrtL("Warriorlevel: " + warrior.level);
            //Game.WrtL("Warrior attack dice sum: " + sumOfAttack);
            //Game.WrtL("Warrior critical: " + critical);
            //Game.WrtL("Monster defence dice: " + monsterDefence + "\n");

            for (int i = 0; i < sumOfAttack; i++)
            {
                rolledDie = Game.randomize.Next(warriorDiceSides);

                if (rolledDie == 0)
                {
                    warriorSixes++;
                }
            }

            if (warrior.stamina < 4)
            {
                // A tired warrior is a weak warrior.
                warriorSixes -= 4 - warrior.stamina;
                if (warriorSixes <= 0)
                    warriorSixes = 1;

                Game.WrtL(Game.CapitalizeFirstLetter(warrior.pronoun[2]) + " attackpower is diminished because of low stamina!\n");
            }

            for (int i = 0; i < monsterDefence; i++)
            {
                rolledDie = Game.randomize.Next(monsterDiceSides);

                if (rolledDie == 0)
                {
                    monsterSixes++;
                }
            }

            if (warriorSixes > monsterSixes)
            {
                finalHits = Game.randomize.Next(1, 1 + warrior.attackDice);
                warrior.consecutiveMisses = 0;
                beast.consecutiveDodges = 0;

                // No critical chance if warrior is too tired.
                if (warrior.stamina > 3)
                {
                    for (int i = 0; i < critical; i++)
                    {
                        rolledDie = Game.randomize.Next(6);

                        if (rolledDie == 0)
                        {
                            //Game.WrtL("critical");
                            if (warrior.attackDice < 4)
                            {
                                // Rare. One must have chosen one of the small and weakest weapons outside ones preferred weapon types.
                                //Game.WrtL("simple. " + finalHits);
                                //Game.WrtL("gotCritical = true;");
                                // No use doing any advanced adding.
                                finalHits += Game.randomize.Next(2, 5);
                                gotCritical = true;
                                break;
                            }
                            else
                            {
                                //Game.WrtL("advanced. " + finalHits);

                                if (warrior.isDualWielding)
                                {
                                    if (gotCritical == false)
                                    {
                                        finalHits += 2 + Game.randomize.Next(1, 1 + (warrior.attackDice / 2));
                                        //Game.WrtL("isDualWielding = true;");
                                        //Game.WrtL("gotCritical = true;");
                                        gotCritical = true;
                                        continue;
                                    }
                                    else
                                    {
                                        finalHits += Game.randomize.Next(1, 1 + (warrior.attackDice / 2));
                                        break;
                                    }  
                                }
                                else
                                {
                                    finalHits += 2 + Game.randomize.Next(1, 1 + (warrior.attackDice / 2));
                                    //Game.WrtL("isDualWielding = false;");
                                    //Game.WrtL("gotCritical = true;");
                                    gotCritical = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (gotCritical)
                    {
                        warrior.consecutiveCriticalHits++;
                        //Game.WrtL("new finalHits: " + finalHits);
                    }
                    else
                    {
                        warrior.consecutiveCriticalHits = 0;
                    }
                }
            }
            else
            {
                // The warrior's safe dice get them an extra chance to hit the monster, this time it affects the finalHits directly.
                // However, hitting this way ignores critical dice.
                for (int i = 0; i < (warrior.weaponSafeDice + beast.stunCounter); i++)
                {
                    rolledDie = Game.randomize.Next(warriorDiceSides);

                    if (rolledDie == 0)
                    {
                        finalHits++;
                    }
                }

                if (finalHits == 0)
                {
                    warrior.consecutiveMisses++;
                    beast.consecutiveBigDamages = 0;
                    beast.consecutiveDodges++;
                    if (beast.composure == 0)
                        beast.composure = 1;
                }
                else
                {
                    // Test info.
                    Game.WrtL("yl", "", "Safe Dice saved you this time!", true);
                }
            }

            // Battle takes its toll.
            warrior.stamina--;
            warrior.restTime = 0;
            if (warrior.stamina < 0)
                warrior.stamina = 0;

            if (warrior.consecutiveMisses >= 2)
            {
                if (warrior.composure > 0)
                    warrior.composure--;
            }
            if (warrior.consecutiveCriticalHits == 2)
            {
                warrior.composure += 2;
                if (warrior.composure > warrior.baseComposure)
                    warrior.composure = warrior.baseComposure;

                warrior.consecutiveCriticalHits = 0;
            }

            // Checks if there were any hits and delivers appropriate message.
            if (finalHits > 0)
            {
                if (gotCritical)
                {
                    Game.WrtL("yl", "", "A critical hit!", true);
                }  

                Game.Wrt(Game.CapitalizeFirstLetter(warrior.pronoun[0]) + " has inflicted ");
                Game.Wrt("yl", "", finalHits, true);
                Game.WrtL(" damage to the " + beast.name + "!\n");

                beast.healthpoints -= finalHits;

                if (finalHits > 2)
                {
                    beast.composure--;
                }
                else if (finalHits > 3)
                {
                    beast.consecutiveBigDamages++;
                }
                else if (finalHits >= beast.maxHealth / 2)
                {
                    beast.consecutiveBigDamages++;
                    beast.composure -= 2;
                    if (warrior.composure < warrior.baseComposure)
                        warrior.composure++;
                }
                else
                    beast.consecutiveBigDamages = 0;

                if (beast.consecutiveBigDamages == 3)
                {
                    beast.composure--;
                    beast.consecutiveBigDamages = 0;
                }

                XPgained += beast.XPworth / beast.maxHealth * finalHits;

                // Also affects the beasts values.
                beast.stamina -= finalHits/2;
                beast.restTime = 0;
                if (beast.stamina < 0)
                    beast.stamina = 0;

                if (beast.healthpoints <= 0)
                {
                    beast.healthpoints = 0;
                    XPgained += beast.XPworth / beast.maxHealth * beast.level;
                }
                if (beast.composure <= 0)
                    beast.composure = 0;
            }
            else
            {
                Game.WrtL(Game.CapitalizeFirstLetter(warrior.pronoun[0]) + " couldn't touch the " + beast.name + "...\n");
            }

            warrior.experience += XPgained;
            if (warrior.isDualWielding)
            {
                if (finalHits > 0)
                    warrior.dualWieldExperience += XPgained;
            }
            
            if (XPgained > 0)
            {
                Game.w();
                Game.WrtL("Experience points gained: " + XPgained);
                if (warrior.isDualWielding && finalHits > 0)
                    Game.WrtL("Dual wield experience points gained: " + XPgained);
                Game.res();
            }

            Game.AwaitKeyEnter();
        }
        // Method for when a warrior manages to counter attack, directly after a monster attack failed.
        public static int CounterAttack(Hero warrior)
        {
            int diceSides = 12;
            int rand;
            bool hit = false;
            int finalHits = 0;
            int critical = warrior.equippedWeapon.criticalChance + warrior.bonusCriticalDice;

            if (warrior.equippedSecondaryWeapon != null)
                critical += warrior.equippedSecondaryWeapon.criticalChance;

            for (int i = 0; i < warrior.chanceToCounterAttack; i++)
            {
                rand = Game.randomize.Next(diceSides);

                if (rand == 0)
                {
                    hit = true;
                    break;
                }
            }

            if (hit)
            {
                finalHits = Game.randomize.Next(1, warrior.attackDice+1);
                int crits = 0;

                for (int i = 0; i < critical; i++)
                {
                    rand = Game.randomize.Next(6);

                    if (rand == 0)
                    {
                        crits++;
                        if (crits < 3)
                            finalHits += 2;
                        else
                            finalHits++;
                    }
                }
            }
            
            return finalHits;
        }
    }
}
