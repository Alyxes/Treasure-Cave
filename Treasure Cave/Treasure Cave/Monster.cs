using System;
using System.Collections.Generic;

namespace TreasureCave
{
    public class Monster
    {
        // Declares a lot of variables for monsters.
        public string name;
        public string type;
        public string difficultyType;
        public string battlecry;
        public string enter_message;

        public int level;
        public int XPworth;

        public int maxHealth;
        public int healthpoints;
        public int maxStrength;
        public int strength;
        public int maxStamina;
        public int stamina;
        public int maxSpeed;
        public int speed;
        public int composure; // composure for monsters will affect their tendency to flee a fight, and their successfulness in battle.
        public int baseComposure;
        public int armor;
        public int criticalDice;
        public int surpriseAttackChance;

        public List<string> ailments = new List<string>();

        public bool premiere = true;
        public int roomPartyIndex;
        public int consecutiveMisses = 0;
        public int consecutiveBigDamages = 0;
        public int consecutiveDodges = 0;
        public int consecutiveCriticalHits = 0;

        public string weapon;
        public string weaponType;
        public int attack;

        public string specialty;
        public string weak_element;

        public int aggravatedAgainst;
        public int restTime;
        public int mobilityInhibitCounter = 0;
        public int stunCounter = 0;

        // These extra stats are to use for when the monsters are stronger at night.
        public int night_level = 0;
        public int night_health = 0;
        public int night_strength = 0;
        public int night_speed = 0;
        public int night_stamina = 0;
        public int night_critical = 0;

        // Method to generate monsters and put them in the list of a specific cave room.
        public static void GenerateMonsters(int amount, Cave room)
        {
            int oneOfThree = 0;

            for (var i = 0; i < amount; i++)
            {
                // Fantastic examples of difficulty levels:
                // "Easy";
                // "Medium";
                // "Pushy";
                // "Tough";
                // "Hard";
                // "Wild";
                // "Alarming";
                // "SuperHard";
                // "Insane";
                // "Lunacy";
                // "Ridiculous";
                // "GodMode";
                // There are no difficulties yet, so no more than three monsters exist.
                // There will be more than three different monsters on each difficulty.
                // Should some monsters be able to overlap difficulties? Definitely if there are as many difficulties as suggested above...
                if (room.difficulty == "Easy")
                    oneOfThree = Game.randomize.Next(3);
                else if (room.difficulty == "Medium")
                    oneOfThree = Game.randomize.Next(3, 6);
                else if (room.difficulty == "Pushy")
                    oneOfThree = Game.randomize.Next(6, 9);
                else if (room.difficulty == "Tough")
                    oneOfThree = Game.randomize.Next(9, 12);
                else if (room.difficulty == "Hard")
                    oneOfThree = Game.randomize.Next(12, 15);

                if (oneOfThree == 0)
                {
                    Monster ratKing = new Monster();
                    ratKing.name = "Rat King";
                    ratKing.type = "Scavenger";
                    ratKing.roomPartyIndex = i;
                    ratKing.difficultyType = room.difficulty;
                    ratKing.battlecry = "\"Shriiieeeek!!\"";
                    ratKing.enter_message = "A bear sized, rugged and distorted rat beast comes rushing into your torch lights from the darkness! Its' eyes,\nfangs and claws glimmering, ready to shred, slice and munch on your flesh!\n";
                    ratKing.level = 1;
                    ratKing.XPworth = 150;
                    ratKing.maxHealth = Game.randomize.Next(8, 11);
                    ratKing.healthpoints = ratKing.maxHealth;
                    ratKing.maxStrength = Game.randomize.Next(7, 9);
                    ratKing.strength = ratKing.maxStrength;
                    ratKing.maxStamina = Game.randomize.Next(8, 13) + (ratKing.strength / 3);
                    ratKing.stamina = ratKing.maxStamina;
                    ratKing.armor = Game.randomize.Next(2, 5);
                    ratKing.maxSpeed = Game.randomize.Next(9, 14) - (ratKing.armor / 2);
                    ratKing.speed = ratKing.maxSpeed;
                    ratKing.baseComposure = 9;
                    ratKing.composure = ratKing.baseComposure;
                    ratKing.weapon = "Claws and fangs";
                    ratKing.weaponType = "sharp";
                    ratKing.attack = Game.randomize.Next(2, 6);
                    ratKing.criticalDice = 1;
                    ratKing.specialty = "Disgusting";
                    ratKing.weak_element = "Fire";
                    ratKing.surpriseAttackChance = 12;
                    ratKing.aggravatedAgainst = -1;

                    room.MonsterList.Add(ratKing);
                }
                else if (oneOfThree == 1)
                {
                    Monster bigBlob = new Monster();
                    bigBlob.name = "Big Blob";
                    bigBlob.type = "Carnivorous Invertebrate";
                    bigBlob.roomPartyIndex = i;
                    bigBlob.difficultyType = room.difficulty;
                    bigBlob.battlecry = "\"BlubBLUbluBlUBlbBlbLhh\"";
                    bigBlob.enter_message = "The sticky floor is a brief warning of the big blob emerging from the shadows, welling over the bumpy floor but\nwith intent; to smother and devour you within its dirty, green brownish slime body with red veins!\n";
                    bigBlob.level = 1;
                    bigBlob.XPworth = 100;
                    bigBlob.maxHealth = Game.randomize.Next(10, 15);
                    bigBlob.healthpoints = bigBlob.maxHealth;
                    bigBlob.maxStrength = Game.randomize.Next(5, 8);
                    bigBlob.strength = bigBlob.maxStrength;
                    bigBlob.maxStamina = Game.randomize.Next(15, 20) + bigBlob.strength;
                    bigBlob.stamina = bigBlob.maxStamina;
                    bigBlob.armor = 0;
                    bigBlob.maxSpeed = Game.randomize.Next(6, 9);
                    bigBlob.speed = bigBlob.maxSpeed;
                    bigBlob.baseComposure = 16;
                    bigBlob.composure = bigBlob.baseComposure;
                    bigBlob.weapon = "Poisonous slime";
                    bigBlob.weaponType = "poison";
                    bigBlob.attack = Game.randomize.Next(2, 5);
                    bigBlob.criticalDice = 1;
                    bigBlob.specialty = "Poisonous";
                    bigBlob.weak_element = "Cold";
                    bigBlob.surpriseAttackChance = 8;
                    bigBlob.aggravatedAgainst = -1;

                    room.MonsterList.Add(bigBlob);
                }
                else if (oneOfThree == 2)
                {
                    Monster goblin = new Monster();
                    goblin.name = "Cave Goblin";
                    goblin.type = "Humanoid Grim Kin";
                    goblin.roomPartyIndex = i;
                    goblin.difficultyType = room.difficulty;
                    goblin.battlecry = "\"Umannns!! Kill! Kill! Kiiilll!\"";
                    goblin.enter_message = "Its shrieking and sound of quick foot steps gives it away before you can see it, a cave goblin running\ntowards you with its weapon swinging! It flinches at the light of your torches but still very much plans to\nkill you and steal your equipment and Branzen!\n";
                    goblin.level = 1;
                    goblin.XPworth = 120;
                    goblin.maxHealth = Game.randomize.Next(6, 10);
                    goblin.healthpoints = goblin.maxHealth;
                    goblin.maxStrength = Game.randomize.Next(5, 8);
                    goblin.strength = goblin.maxStrength;
                    goblin.maxStamina = Game.randomize.Next(7, 12) + (goblin.strength / 3);
                    goblin.stamina = goblin.maxStamina;
                    goblin.armor = Game.randomize.Next(3, 6);
                    goblin.maxSpeed = Game.randomize.Next(8, 12) - (goblin.armor / 2);
                    goblin.speed = goblin.maxSpeed;
                    goblin.baseComposure = 6;
                    goblin.composure = goblin.baseComposure;
                    goblin.weapon = "Long Dagger";
                    goblin.weaponType = "sharp";
                    goblin.attack = Game.randomize.Next(2, 6);
                    goblin.criticalDice = 1;
                    goblin.specialty = "Annoying";
                    goblin.weak_element = "Light";
                    goblin.surpriseAttackChance = 17;
                    goblin.aggravatedAgainst = -1;

                    room.MonsterList.Add(goblin);
                }
            }
        }
        public static void UpdateActiveMonsters()
        {
            // NOTE! Is going to be a method for updating all monsters that has been activated so that any monsters that fled a battle slowly can regain their health.
            // Will update every game hour.
        }
        // Method for increasing a monsters stats based on the average level of the party of warriors, and change it based on night or day-time.
        public static void AdaptMonsterLevel(Monster monster)
        {
            if (monster.premiere)
            {
                if (Game.averagePartyLevel > 2)
                {
                    monster.level = Game.randomize.Next(Game.averagePartyLevel - 1, Game.averagePartyLevel + 2);
                    if (monster.difficultyType == "Easy")
                    {
                        if (monster.level > 9)
                            monster.level = 9;
                    }
                }
                else if (Game.averagePartyLevel == 2)
                {
                    monster.level = Game.randomize.Next(1, Game.averagePartyLevel + 1);
                }

                double critPlus = monster.level / 4.1;
                monster.criticalDice += (int)critPlus;

                monster.attack += monster.level / 3;
                monster.armor += monster.level / 6;
                monster.XPworth += monster.level * 100;

                if (monster.level > 1)
                {
                    int[] stats = new int[4];

                    for (int i = 0; i < monster.level; i++)
                    {
                        // Upgrades one of the monsters stats per level.
                        int rand;
                        if (i % 4 == 0)
                        {
                            for (int j = 0; j < stats.Length; j++)
                            {
                                stats[j] = 0;
                            }
                        }
                        do
                        {
                            rand = Game.randomize.Next(4);
                        }
                        while (stats[rand] == 1);

                        stats[rand] = 1;

                        if (stats[0] == 1)
                        {
                            monster.maxHealth++;
                        }
                        else if (stats[1] == 1)
                        {
                            monster.strength++;
                        }
                        else if (stats[2] == 1)
                        {
                            monster.maxStamina++;
                        }
                        else if (stats[3] == 1)
                        {
                            monster.speed++;
                        }
                    }
                }

                monster.premiere = false; // This bool can never become true again.
            }

            if (Game.timeOfDay == "nighttime")
            {
                monster.night_level = 3;
                monster.night_health = 1 + monster.level;
                monster.night_strength = 1 + (monster.level / 5);
                monster.night_speed = 1 + (monster.level / 5);
                monster.night_stamina = 2 + monster.level;
                monster.night_critical = 1 + (monster.level / 9);

                Console.Clear();
                Game.WrtL("r", "", "Monster is stronger due to nighttime!", true);
                Game.AwaitKeyEnter();
            }
            else
            {
                monster.night_level = 0;
                monster.night_health = 0;
                monster.night_strength = 0;
                monster.night_speed = 0;
                monster.night_stamina = 0;
                monster.night_critical = 0;

                //Console.Clear();
                //Game.WrtL("c", "", "Monster is weaker due to daytime.", true);
                //Game.AwaitKeyEnter();
            }
        }
        // Method for a monsters attack. Quite similar to the heroes attack method.
        // In the future, parts that are very similar should be turned into specific methods used for both monsters and warriors.
        public static void Attack(Monster beast, Hero warrior)
        {
            if (beast.mobilityInhibitCounter > 0)
            {
                beast.mobilityInhibitCounter--;
                if (beast.composure > 0)
                    beast.composure--;

                if (beast.mobilityInhibitCounter <= 0)
                {
                    Game.WrtL("The " + beast.name + " just regained its full mobility!");
                    beast.mobilityInhibitCounter = 0;
                    beast.speed = beast.maxSpeed;
                    if (beast.composure < beast.baseComposure)
                        beast.composure++;
                }
                else
                {
                    Game.WrtL("The " + beast.name + "'s mobility is still impaired.");
                }
                Game.AwaitKeyEnter();
            }
            if (beast.stunCounter > 0)
            {
                Game.WrtL("The " + beast.name + " is still stunned.");
            }

            int XPgained = 0;

            int statAttack;
            int weaponAttack;
            int critical;

            if (beast.stunCounter > 0)
            {
                statAttack = beast.strength + beast.speed;
                weaponAttack = beast.attack/2;
                critical = 0;
                if (beast.attack < 1)
                    beast.attack = 1;
            }
            else
            {
                statAttack = beast.level + beast.strength + beast.speed + beast.restTime;
                statAttack += beast.night_level + beast.night_strength + beast.night_speed;
                weaponAttack = beast.attack + ((beast.level + beast.night_level) / 3); // Here will be added any weapon specific bonuses towards this warrior.
                double critPlus = 1 * (beast.level / 4.1);
                critical = beast.criticalDice += (int)critPlus + beast.night_critical;
            }

            int warriorDefence = warrior.speed + warrior.armor + warrior.level + warrior.restTime;

            if (beast.composure < 5)
            {
                weaponAttack--;
                if (beast.composure < 3)
                {
                    weaponAttack--;
                    critical = 0;
                }
                if (weaponAttack <= 0)
                    weaponAttack = 1;
            }

            int sumOfAttack = statAttack + weaponAttack;
            if (beast.restTime > 0)
                sumOfAttack += beast.restTime;

            if (beast.composure <= 1)
                sumOfAttack = sumOfAttack / 2;

            int rolledDie;
            int warriorSixes = 0;
            int monsterSixes = 0;
            int finalHits = 0;
            int monsterDiceSides = 6;
            int warriorDiceSides = 6;
            bool gotCritical = false;

            bool warriorCountered = false;
            int counterHits = 0;

            // Test purposes
            //Game.WrtL("Monsterlevel: " + beast.level);
            //Game.WrtL("Monster attack dice sum: " + sumOfAttack);
            //Game.WrtL("Monster critical: " + critical);
            //Game.WrtL("Warrior defence dice: " + warriorDefence + "\n");

            for (int i = 0; i < sumOfAttack; i++)
            {
                rolledDie = Game.randomize.Next(monsterDiceSides);

                if (rolledDie == 0)
                {
                    monsterSixes++;
                }
            }

            if (beast.stamina < 4)
            {
                // A tired beast is a weak beast.
                monsterSixes -= 4 - beast.stamina;
                if (monsterSixes <= 0)
                    monsterSixes = 1;

                Game.WrtL(beast.name + " is tired and weakened!\n");
            }

            for (int i = 0; i < warriorDefence; i++)
            {
                rolledDie = Game.randomize.Next(warriorDiceSides);

                if (rolledDie == 0)
                {
                    warriorSixes++;
                }
            }

            if (monsterSixes > warriorSixes)
            {
                finalHits = Game.randomize.Next(1, beast.attack+1);
                beast.consecutiveMisses = 0;
                warrior.consecutiveDodges = 0;

                // No critical chance if beast is too tired.
                if (beast.stamina > 3 && beast.stunCounter == 0)
                {
                    for (int i = 0; i < critical; i++)
                    {
                        rolledDie = Game.randomize.Next(6);

                        if (rolledDie == 0)
                        {
                            if (beast.composure == 1)
                                beast.composure++;

                            //Game.WrtL("critical");
                            if (beast.attack < 3)
                            {
                                //Game.WrtL("simple. " + finalHits);
                                //Game.WrtL("gotCritical = true;");
                                // No use doing any advanced adding.
                                finalHits += Game.randomize.Next(1, beast.attack+1);
                            }
                            else if (beast.attack == 3)
                            {
                                //Game.WrtL("advanced. " + finalHits);
                                finalHits += Game.randomize.Next(1, beast.attack);
                                //Game.WrtL("gotCritical = true;");
                            }
                            else
                            {
                                finalHits += Game.randomize.Next(1, beast.attack-1);
                            }
                            gotCritical = true;
                            break;
                        }
                    }
                    if (gotCritical)
                    {
                        beast.consecutiveCriticalHits++;
                        //Game.WrtL("new finalHits: " + finalHits);
                    }
                    else
                    {
                        beast.consecutiveCriticalHits = 0;
                    }
                }
            }
            else
            {
                beast.consecutiveMisses++;
                warrior.consecutiveBigDamages = 0;
                warrior.consecutiveDodges++;
                if (beast.composure == 1)
                    beast.composure = 0;

                // When the monster misses, the warrior has a chance to counter attack!
                counterHits = Hero.CounterAttack(warrior);

                if (counterHits > 0)
                {
                    warriorCountered = true;

                    beast.healthpoints -= counterHits;
                    if (beast.healthpoints <= 0)
                        beast.healthpoints = 0;
                }
            }
            if (beast.healthpoints > 0)
            {
                beast.stamina--;
                beast.restTime = 0;
                if (beast.stamina < 0)
                    beast.stamina = 0;

                if (beast.consecutiveMisses >= 3)
                {
                    if (beast.composure > 0)
                        beast.composure--;
                }
                if (beast.consecutiveCriticalHits == 2)
                {
                    beast.composure += 2;
                    if (beast.composure > beast.baseComposure)
                        beast.composure = beast.baseComposure;

                    beast.consecutiveCriticalHits = 0;
                }
            }
            
            if (finalHits > 0)
            {
                if (gotCritical)
                {
                    Game.WrtL("r", "", "A critical hit by the beast!", true);
                }

                if (beast.composure == 1)
                    beast.composure++;

                Game.Wrt(beast.name + " has inflicted ");
                Game.Wrt("r","",finalHits,true);
                Game.WrtL(" damage to the warrior!\n");

                warrior.healthpoints -= finalHits;

                if (gotCritical)
                {
                    int rand;
                    if (beast.weaponType == "sharp")
                    {
                        rand = Game.randomize.Next(3);

                        if (rand == 0)
                        {
                            if (!Game.HasAilment(warrior, Game.ailments[0]))
                            {
                                warrior.ailments.Add(Game.ailments[0]); // bleeding
                                Game.AddToPerilListIfAbsent(warrior.alltimePartyId);

                                Game.WrtL("The wound got deep and the warrior is bleeding heavily!");
                                Game.WrtL(Game.CapitalizeFirstLetter(warrior.pronoun[0]) + " needs some kind of medical care soon!\n");
                            } 
                        }
                    }
                    else if (beast.weaponType == "poison")
                    {
                        rand = Game.randomize.Next(2);

                        if (rand == 0)
                        {
                            if (!Game.HasAilment(warrior, Game.ailments[9]))
                            {
                                warrior.ailments.Add(Game.ailments[9]); // poisoned
                                Game.AddToPerilListIfAbsent(warrior.alltimePartyId);

                                Game.WrtL("The beast managed to poison the warrior! It will weaken " + warrior.pronoun[1] + " over time and finally kill " + warrior.pronoun[1] + ".");
                                Game.Wrt("A strong person resists longer... ");
                                Game.WrtL("But " + warrior.pronoun[0] + "'s gonna need an antidote soon.\n");
                            }
                        }
                    }
                }

                if (finalHits >= warrior.maxHealth / 2)
                {
                    warrior.consecutiveBigDamages++;
                    warrior.composure -= 2;
                    if (beast.composure < beast.baseComposure)
                        beast.composure++;
                }
                else if (finalHits > 3)
                {
                    warrior.consecutiveBigDamages++;
                    warrior.composure--;
                }
                else
                    warrior.consecutiveBigDamages = 0;

                if (warrior.consecutiveBigDamages == 3)
                {
                    warrior.composure--;
                    warrior.consecutiveBigDamages = 0;
                }

                if (warrior.composure <= 0)
                {
                    warrior.composure = 0;
                    if (!Game.HasAilment(warrior, Game.ailments[7]))
                    {
                        warrior.ailments.Add(Game.ailments[7]); // panicking
                        Game.AddToPerilListIfAbsent(warrior.alltimePartyId);
                    }
                }

                warrior.stamina -= finalHits/2;
                if (warrior.stamina < 0)
                    warrior.stamina = 0;

                if (warrior.healthpoints <= 0)
                {
                    warrior.healthpoints = 0;
                    // Dying again shouldn't be possible to happen, but I will have this question as a standard everywhere anyway.
                    if (!Game.HasAilment(warrior, Game.ailments[8]))
                    {
                        warrior.ailments.Add(Game.ailments[8]); // dying
                        
                        Game.AddToPerilListIfAbsent(warrior.alltimePartyId);
                        Game.AwaitKeyEnter();
                        return;
                    }
                }
                else if (warrior.composure == 0)
                {
                    Game.WrtL(warrior.name + " has lost " + warrior.pronoun[2] + " composure and is panicking! " + Game.CapitalizeFirstLetter(warrior.pronoun[0]) + " will shortly run away\nand be lost to the darkness if not held back in some way!\n");
                }
                else
                {
                    if (warrior.healthpoints < 5)
                        Game.WrtL(warrior.name + " has been taking a lot of damage, " + warrior.pronoun[0] + " should rest and heal soon.\n");
                    else if (warrior.stamina < 5)
                        Game.WrtL(warrior.name + " is getting weary, " + warrior.pronoun[0] + "'d do well to rest.\n");
                }
            }
            else if (warriorCountered)
            {
                Game.WrtL(beast.name + " couldn't touch the warrior.");
                Game.WrtL("w", "", "However, " + warrior.name + " counter attacked, and damaged the monster " + counterHits + " healthpoints!\n", true);

                XPgained += beast.XPworth / beast.maxHealth * counterHits;

                if (beast.healthpoints <= 0)
                {
                    beast.healthpoints = 0;
                    XPgained += beast.XPworth / 2 * beast.level;

                    Game.WrtL("yl", "", "The counter attack turned out lethal!\n", true);
                }
            }
            else
            {
                Game.WrtL(beast.name + " couldn't touch the warrior...\n");
                XPgained += 20 * beast.level;
            }

            if (beast.stunCounter > 0)
            {
                beast.stunCounter--;
                if (beast.stunCounter <= 0)
                {
                    beast.stunCounter = 0;
                    beast.speed = beast.maxSpeed;
                    beast.strength = beast.maxStrength;
                    if (beast.composure < beast.baseComposure)
                        beast.composure++;
                }
                else
                {
                    if (beast.composure > 0)
                        beast.composure--;
                }
            }

            warrior.experience += XPgained;
            if (warrior.isDualWielding)
                warrior.dualWieldExperience += XPgained;

            if (XPgained > 0)
            {
                Game.w();
                Game.WrtL("Experience points gained: " + XPgained);
                if (warrior.isDualWielding && warriorCountered)
                    Game.WrtL("Dual wield experience points gained: " + XPgained);
                Game.res();
            }

            Game.AwaitKeyEnter();
        }
        // Method for monsters rest choice, exactly the same as for heroes.
        public static void Rest(Monster beast)
        {
            beast.stamina += 3;
            beast.restTime++;

            if (beast.restTime > 0)
                beast.stamina += 2;

            if (beast.stamina > beast.maxStamina)
                beast.stamina = beast.maxStamina;
        }
        // Method for giving the monster a small amount of decision making, being able to rest, and attack someone else than active warrior.
        // Returns a number, the decision of whom to attack, or -1 (no one, thereby resting).
        public static int MonsterDecisions(Monster beast)
        {
            // Aggravation is yet simple. The last warrior to attack the monster is "it".
            // In later versions I would like to add the possibility for monsters to notice which warrior is stronger or weaker, and decide to attack them for strategic reasons.
            // Also, would like to add a variable for moral to decide if the monster gets afraid and decides to try to escape the battle. Can be affected of strong succesful attacks.
            if (beast.composure == 0)
            {
                // The monster is panicking and decides to try to flee!
                return -2;
            }
            if (beast.aggravatedAgainst == -1)
            {
                int whichToAttack;
                int amount = 0;
                do
                {
                    // This never stops if the sole warrior is walking dead, which isn't possible without a bug...
                    whichToAttack = Game.randomize.Next(Game.Party.Count);

                    if (amount == Game.Party.Count)
                    {
                        Console.Clear();
                        Game.WrtL("BUG! MonsterDecisions, no warrior has any healthpoints! WTF.");
                        Game.AwaitKeyEnter();
                        whichToAttack = 0;
                        break;
                    }
                    amount++;
                }
                while (Game.Party[whichToAttack].healthpoints == 0);

                beast.aggravatedAgainst = whichToAttack;
            }

            if (beast.stamina == 0 || (beast.stamina < 5 && beast.healthpoints > 5) || (beast.restTime == 1 && beast.stamina < 6))
            {
                // Beast decides to rest.
                return -1;
            }

            return beast.aggravatedAgainst;
        }
        // Method for collecting all the outcomes of the monster's decision. Returns -1 to 1.
        public static int Reaction(Monster beast, int currentWarrior)
        {
            int monsterChoice = MonsterDecisions(beast);
            int focus = currentWarrior;

            if (monsterChoice == -1)
            {
                Game.WrtL("The " + beast.name + " decides to rest!");
                Rest(beast);

                Game.AwaitKeyEnter();
                return 0;
            }
            else if (monsterChoice == -2)
            {
                Game.WrtL("The " + beast.name + " is panicking and tries to flee!\n");
                return -1;
            }
            else
            {
                // Is the chosen warrior the active one? 
                if (beast.aggravatedAgainst == focus)
                    Game.WrtL("The " + beast.name + " attacks!\n");
                else
                {
                    // No. Set the new target.
                    focus = beast.aggravatedAgainst;
                    Game.WrtL("The " + beast.name + " decides to attack " + Game.Party[focus].name + "!\n");
                }

                Game.AwaitKeyEnter();
                // Attacks!
                Attack(beast, Game.Party[focus]);

                if (beast.composure == 0)
                {
                    Game.WrtL("The " + beast.name + " is panicking and tries to flee!\n");
                    return -1;
                }
            }
            return DeathResults(focus, beast);
        }
        // Method for the monster thinking about wether to flee the battle or not.
        public static int TryToFlee()
        {
            int chanceToRun = 15 - Game.Party.Count;
            int rand = Game.randomize.Next(15);

            if (rand < chanceToRun)
            {
                Game.WrtL("It dashes away, out from your torch light!");
                // Fix later.
                // Game.WrtL("You have a slim chance to hit it with an arrow or throwing weapon before it's gone in the darkness!");
                return 1;
            }
            else
            {
                Game.WrtL("It fails to get away from you! It squirms and goes into defence position again!\n");
                return 0;
            }
        }
        // Method for if death has occurred to a warrior and/or the monster.
        public static int DeathResults(int focus, Monster beast)
        {
            if (Game.Party[focus].healthpoints == 0)
            {
                Console.Clear();

                if (Game.Party[focus].alltimePartyId == 1)
                {
                    // Oh no... The player character died!
                    if (Game.Party.Count > 1)
                    {
                        Game.WrtL("Oh my! You, the leader, was killed...");

                        bool emptyParty = true;

                        for (int i = 0; i < Game.Party.Count; i++)
                        {
                            if (Game.Party[i].healthpoints > 0)
                            {
                                emptyParty = false;
                                break;
                            }
                        }
                        if (emptyParty)
                        {
                            Game.WrtL("But you and your team fought bravely to the end.");
                        }
                        else
                        {
                            if (Game.Party.Count > 2)
                                Game.WrtL("Without you the party panics and scatters into the darkness, lost in the cave, to get killed one by one...");
                            else
                                Game.WrtL("Your last warrior panics and disappears into the darkness, stumbling blindly, to get eaten by something soon enough...");
                        }
                    }
                    else
                    {
                        Game.WrtL("Oh no... You were killed...");
                        Game.WrtL("But you fought bravely to the end.");
                    }

                    Game.AwaitKeyEnter();
                    Game.EndGameByDeath(0);
                }
                // Game.WrtL(Game.Party[focus].name + " is down! If " + Game.Party[focus].pronoun[0] + " doesn't get immediate help soon " + Game.Party[focus].pronoun[0] + " will die!\n");
                Game.WrtL("Oh no! " + Game.Party[focus].name + " was killed by the " + beast.name + "!");

                if (beast.healthpoints == 0)
                {
                    // victory = true; but not for the monster...
                    Game.WrtL("But " + Game.Party[focus].pronoun[0] + " got the monster with " + Game.Party[focus].pronoun[2] + " defensive moves!");
                    // Defensive moves are not yet a thing...
                    Game.WrtL(Game.Party[focus].name + " gave " + Game.Party[focus].pronoun[2] + " life for the party.");
                    Game.AwaitKeyEnter();
                    return 1;
                }
                Game.AwaitKeyEnter();
                return 0;
            }
            if (beast.healthpoints == 0)
            {
                // victory = true;
                Game.WrtL(Game.Party[focus].name + " got the monster with " + Game.Party[focus].pronoun[2] + " defensive moves!");
                Game.AwaitKeyEnter();
                return 1;
            }
            return 0;
        }
    }
}
