using System.Collections.Generic;

namespace TreasureCave
{
    public class Gear
    {
        public int Id;
        public int cost;
        public string name;
        public string description;
        public List<string> types;
        public static List<Gear> allGear = new List<Gear>();
        public static List<int> GearToRandomize = new List<int>();
        public static List<int> WeaponToRandomize = new List<int>();
        public static List<int> ArmorToRandomize = new List<int>();

        public int wieldLvl; // The level needed from the warrior to wield this gear. Is used in weapons and armor mostly.
        public string specialPower1 = "None";
        public int specialPower1Value;
        public string specialPower2 = "None";
        public int specialPower2Value;
        public bool addsBattleOption;
        public string battleOptionPrompt;
        public int commonness;
        public string specialDataDescription = null;

        // weapon variables
        public static string[] sizes = { "small", "medium", "big" };
        public static string[] weaponClasses = { "sword", "knife", "axe", "blunt", "bow", "crossbow", "spear", "sickle", "whip" };
        public string weaponClass;
        public string size;
        public int dualWieldLvl; // The level needed from the warrior to double wield this weapon.
        public bool isDoubleHandheld;
        public int safeDice;
        public int skillDice;
        public int defenceDice;
        public int criticalChance;
        public int attack;

        // armor variables
        public static string[] armorClasses = { "armor", "shield" };
        public int defence;
        public int weight;

        // item variables
        public static string[] itemClasses = { "food", "potion", "jewelry", "clothes", "gadget", "artifact", "disposable weapon", "helmet", "gauntlet", "leg wear" };
        public string itemClass;
        public bool isEquippable;
        public bool isPartyEquippable;
        public int amountOfUses;

        // Method for putting the ID of items in certain amounts in a list, thereby creating a kind of probability list. The more spots in the list a specific item takes, the bigger
        // the chance it is that it's chosen when randomizing a spot from the list.
        public static void BuildProbabilityGearLists()
        {
            for (int i = 0; i < allGear.Count; i++)
            {
                // All the weapons come first, so I create a separate weapon list first.
                if (allGear[i].types[0] == "weapon")
                {
                    for (int j = 0; j < allGear[i].commonness; j++)
                    {
                        // I add the Id-number of a weapon to this list as many times as its commonness says. Higher commonness increases that weapons' chance of being randomized through.
                        WeaponToRandomize.Add(allGear[i].Id);
                    }
                }
                else if (allGear[i].types[0] == "armor" || allGear[i].types[0] == "shield")
                {
                    for (int j = 0; j < allGear[i].commonness; j++)
                    {
                        // I add the Id-number of a weapon to this list as many times as its commonness says. Higher commonness increases that weapons' chance of being randomized through.
                        ArmorToRandomize.Add(allGear[i].Id);
                    }
                }
                else
                {
                    // Now, only the items remain.
                    if (i % 10 == 1)
                    {
                        // Each 20th place in the list there should be some more chances that a weapon is randomized.
                        for (int k = 0; k < 6; k++)
                            GearToRandomize.Add(0); // 0 means weapon. The first gear that is created is a weapon, so it has the nr 0 Id, and no other gear can have that.
                    }
                    if (i % 15 == 1)
                    {
                        // Each 30th place in the list there should be some more chances that armor is randomized.
                        for (int k = 0; k < 8; k++)
                            GearToRandomize.Add(1); // 1 means armor. The second gear that is created is a weapon, so it has the nr 1 Id, and no other gear can have that.
                    }
                    for (int j = 0; j < allGear[i].commonness; j++)
                    {
                        // I add the Id-number of a certain gear to this list as many times as its commonness says. Higher commonness increases that gears' chance of being randomized through.
                        GearToRandomize.Add(allGear[i].Id);
                    }
                }
            }
        }
        // Method for printing data about a specific gear.
        public static void PresentGearData(Gear gear)
        {
            if (gear.types[0] == "weapon")
            {
                if (gear.types[1] != "blunt")
                    Game.Wrt(Game.CapitalizeFirstLetter(gear.types[1]));
                else
                    Game.Wrt(Game.CapitalizeFirstLetter(gear.types[1]) + " weapon");
                Game.Wrt(" that can be wielded at level: " + gear.wieldLvl);
                if (gear.dualWieldLvl < 99)
                    Game.WrtL(" - And dual wielded at level: " + gear.dualWieldLvl);
                else
                    Game.WrtL(" - Cannot be dual wielded.");
                Game.Wrt("Safe Dice: " + gear.safeDice);
                Game.WrtL(" - Skill Dice: " + gear.skillDice);  
                Game.Wrt("Critical Dice: " + gear.criticalChance);
                Game.WrtL(" - Defence: " + gear.defenceDice);
                if (gear.isDoubleHandheld)
                {
                    if (gear.dualWieldLvl < 99)
                        Game.Wrt("Requires the use of both hands until you have reached its Dual Wield level.");
                    else
                        Game.Wrt("Requires the use of both hands.");
                }
                for (int i = 0; i < gear.types.Count; i++)
                {
                    if (gear.types[i] == "heavy")
                    {
                        Game.WrtL("Heavy, adds 1 weight, and subtracts 1 skill dice.");
                        Game.WrtL("But when you reach strength 18 the bad things go away and turns into +2 attack."); // Nope, not yet!
                    }
                }
                if (gear.specialDataDescription != null)
                    Game.WrtL(gear.specialDataDescription);
            }
            else if (gear.types[0] == "armor" || gear.types[0] == "shield")
            {
                Game.WrtL(Game.CapitalizeFirstLetter(gear.types[0]) + " that can be wielded at level: " + gear.wieldLvl);
                Game.Wrt("Defence: " + gear.defence);
                Game.WrtL(" - Weight: " + gear.weight);
                if (gear.specialDataDescription != null)
                    Game.WrtL(gear.specialDataDescription);
            }
            else if (gear.types[0] == "item")
            {
                Game.WrtL("This is a " + gear.types[1] + " item.");
                if (gear.specialDataDescription != null)
                    Game.WrtL(gear.specialDataDescription);
            }
        }
        // Method for randomizing a gear and returning the name of that gear.
        public static string RandomizeLootGear()
        {
            Gear newItem;

            int randGear = Game.randomize.Next(GearToRandomize.Count);

            if (GearToRandomize[randGear] == 0)
            {
                // 0 means weapon, lets randomize a weapon instead then.
                randGear = Game.randomize.Next(WeaponToRandomize.Count);

                newItem = allGear[WeaponToRandomize[randGear]];
                Game.partyGear.Add(newItem);
                Game.SortPartyGear();
            }
            else if (GearToRandomize[randGear] == 1)
            {
                // 1 means armor, lets randomize armor instead then.
                randGear = Game.randomize.Next(ArmorToRandomize.Count);

                newItem = allGear[ArmorToRandomize[randGear]];
                Game.partyGear.Add(newItem);
                Game.SortPartyGear();
            }
            else
            {
                // It's not a weapon, or armor. Just an item.
                newItem = allGear[GearToRandomize[randGear]];
                Game.partyGear.Add(newItem);
                Game.SortPartyGear();
            }

            return newItem.name;
        }
        // Method for finding an object in the gear list from the string name of it (attribute 'specificGearName').
        public static Gear GearWithName(string specificGearName)
        {
            Gear gear = null;

            for (int i = 0; i < allGear.Count; i++)
            {
                if (allGear[i].name == specificGearName)
                {
                    gear = allGear[i];
                    break;
                }
            }

            return gear;
        }
        // Method for picking out a string from a specific method and returning it.
        public static string ItemSpecialPowerDescription(string specialPower, int value)
        {
            string specialPowerDescription = null;
            bool isEquipped = false;
            
            switch (specialPower)
            {
                case "HealthUp":
                    specialPowerDescription = HealthUp(null, value);
                    break;
                case "HealthDown":
                    specialPowerDescription = HealthDown(null, value);
                    break;
                case "StaminaUp":
                    specialPowerDescription = StaminaUp(null, value);
                    break;
                case "StaminaDown":
                    specialPowerDescription = StaminaDown(null, value);
                    break;
                case "ComposureUp":
                    specialPowerDescription = ComposureUp(null, value);
                    break;
                case "ComposureDown":
                    specialPowerDescription = ComposureDown(null, value);
                    break;
                case "HealthUpHalf":
                    specialPowerDescription = HealthUpHalf(null, value);
                    break;
                case "HealthUpFull":
                    specialPowerDescription = HealthUpFull(null);
                    break;
                case "StaminaUpHalf":
                    specialPowerDescription = StaminaUpHalf(null, value);
                    break;
                case "StaminaUpFull":
                    specialPowerDescription = StaminaUpFull(null);
                    break;
                case "StrengthPlus":
                    specialPowerDescription = StrengthPlus(null, value, isEquipped);
                    break;
                case "SpeedPlus":
                    specialPowerDescription = SpeedPlus(null, value, isEquipped);
                    break;
                case "ComposurePlus":
                    specialPowerDescription = ComposurePlus(null, value, isEquipped);
                    break;
                case "ArmorPlus":
                    specialPowerDescription = ArmorPlus(null, value);
                    break;
                case "AttackDicePlus":
                    specialPowerDescription = AttackDicePlus(null, value);
                    break;
                case "CriticalDicePlus":
                    specialPowerDescription = CriticalDicePlus(null, value);
                    break;
                case "WeightPlus":
                    specialPowerDescription = WeightPlus(null, value);
                    break;
                case "Intoxication":
                    specialPowerDescription = Intoxication(null, value);
                    break;
                case "Tiring":
                    specialPowerDescription = Tiring(null, value);
                    break;
                case "DefenceDown":
                    specialPowerDescription = DefenceDown(null, value);
                    break;
                case "Berserk":
                    specialPowerDescription = Berserk(null, value);
                    break;
                case "Hyper":
                    specialPowerDescription = Hyper(null, value);
                    break;
                case "RandomEffect":
                    specialPowerDescription = RandomEffect(null, value);
                    break;
                case "CureBleeding":
                    specialPowerDescription = CureBleeding(null);
                    break;
                case "CurePoisoned":
                    specialPowerDescription = CurePoisoned(null, value);
                    break;
                case "Resurrection":
                    specialPowerDescription = Resurrection(null, value);
                    break;
                case "PhysicalMobilityInhibitor":
                    specialPowerDescription = PhysicalMobilityInhibitor(null, null, value);
                    break;
                case "Stun":
                    specialPowerDescription = Stun(null, null, value);
                    break;



            }
            return specialPowerDescription;
        }
        // Method for printing different messages depending on which option is chosen by the player. Returns true if the option works on a monster, false if not.
        public static bool RunBattleMenuOptionMessage(Hero warrior, Monster beast, string optionMessage)
        {
            switch (optionMessage)
            {
                case "Throw Fishing Net":
                    Game.WrtL(warrior.name + " throws a fishing net at the " + beast.name + "!");
                    if (beast.name == "Big Blob")
                    {
                        Game.WrtL("It completely absorbs the net within seconds, the Big Blobs movement isn't affected at all...");
                        return false;
                    }
                    return true;
                case "Throw Blinding Cracker":
                    Game.WrtL(warrior.name + " threw a cracker on the floor to blind the " + beast.name + "!");
                    if (beast.name == "Big Blob")
                    {
                        Game.WrtL("It doesn't have eyes and only shivers slightly from the sound wave...");
                        return false;
                    }
                    return true;
                // There will be a lot more options here.

            }
            return false;
        }
        // Method for running an item's special power by finding its method through the string with the same name.
        public static void RunBattleMenuFunction(Hero warrior, Monster beast, string specialPower, int value)
        {
            switch (specialPower)
            {
                case "PhysicalMobilityInhibitor":
                    PhysicalMobilityInhibitor(warrior, beast, value);
                    break;
                case "Stun":
                    Stun(warrior, beast, value);
                    break;
                case "ElementalFireAttack":
                    // ElementalFireAttack(warrior, beast, value);
                    break;
                case "DemonicWinAndFaint":
                    // DemonicWinAndFaint(warrior, beast, value);
                    break;


            }
        }
        // Method for applying specific gears different stat boosts.
        public static void EquippedGearFunction(Hero warrior, string specialPower, int value, bool isEquipped)
        {
            switch (specialPower)
            {
                case "StrengthPlus":
                    StrengthPlus(warrior, value, isEquipped);
                    break;
                case "SpeedPlus":
                    SpeedPlus(warrior, value, isEquipped);
                    break;
                case "ComposurePlus":
                    ComposurePlus(warrior, value, isEquipped);
                    break;
                case "ArmorPlus":
                    ArmorPlus(warrior, value);
                    break;
                case "AttackDicePlus":
                    AttackDicePlus(warrior, value);
                    break;
                case "CriticalDicePlus":
                    CriticalDicePlus(warrior, value);
                    break;

            }
        }
        // Method for applying different items stat boosts or nerfs.
        public static void UseItemFunction(Hero warrior, string specialPower, int value)
        {
            bool isEquipped = false;
            switch (specialPower)
            {
                case "None":
                    break;
                case "HealthUp":
                    HealthUp(warrior, value);
                    break;
                case "HealthDown":
                    HealthDown(warrior, value);
                    break;
                case "StaminaUp":
                    StaminaUp(warrior, value);
                    break;
                case "StaminaDown":
                    StaminaDown(warrior, value);
                    break;
                case "ComposureUp":
                    ComposureUp(warrior, value);
                    break;
                case "ComposureDown":
                    ComposureDown(warrior, value);
                    break;
                case "HealthUpHalf":
                    HealthUpHalf(warrior, value);
                    break;
                case "HealthUpFull":
                    HealthUpFull(warrior);
                    break;
                case "StaminaUpHalf":
                    StaminaUpHalf(warrior, value);
                    break;
                case "StaminaUpFull":
                    StaminaUpFull(warrior);
                    break;
                case "StrengthPlus":
                    StrengthPlus(warrior, value, isEquipped);
                    break;
                case "SpeedPlus":
                    SpeedPlus(warrior, value, isEquipped);
                    break;
                case "ComposurePlus":
                    ComposurePlus(warrior, value, isEquipped);
                    break;
                case "ArmorPlus":
                    ArmorPlus(warrior, value);
                    break;
                case "AttackDicePlus":
                    AttackDicePlus(warrior, value);
                    break;
                case "Intoxication":
                    Intoxication(warrior, value);
                    break;
                case "Tiring":
                    Tiring(warrior, value);
                    break;
                case "DefenceDown":
                    DefenceDown(warrior, value);
                    break;
                case "Berserk":
                    Berserk(warrior, value);
                    break;
                case "Hyper":
                    Hyper(warrior, value);
                    break;
                case "RandomEffect":
                    RandomEffect(warrior, value);
                    break;
                case "CureBleeding":
                    CureBleeding(warrior);
                    break;
                case "CurePoisoned":
                    CurePoisoned(warrior, value);
                    break;
                case "Resurrection":
                    Resurrection(warrior, value);
                    break;


                default:
                    break;
            }
        }
        // Method for adding points to specific stat of specific warrior after using an item, and printing a message about it.
        private static int IncreaseStat(int statNow, int maxStat, int value, string theStatString)
        {
            int actualChange = value;
            int result;

            if ((statNow + value) > maxStat)
            {
                actualChange = maxStat - statNow;
                result = maxStat;
            }
            else
                result = statNow + value;

            Game.WrtL("Warrior got " + actualChange + " " + theStatString + " back!\n");

            return result;
        }
        // Method for subtracting points from specific stat of specific warrior after using an item, and printing a message about it.
        private static int DecreaseStat(int statNow, int value, string theStatString)
        {
            int actualChange = value;
            int result;

            if ((statNow - value) < 0)
            {
                actualChange = statNow;
                result = 0;
            }
            else
                result = statNow - value;

            Game.WrtL("Warrior lost " + actualChange + " " + theStatString + "!\n");

            return result;
        }
        // Method for increasing health on specific warrior and print an appropriate message.
        public static string HealthUp(Hero warrior, int value)
        {
            if (warrior == null)
                return "Increases health by " + value + ", but not above maximum health.";

            warrior.healthpoints = IncreaseStat(warrior.healthpoints, warrior.maxHealth, value, "health points");
            return "";
        }
        // Method for decreasing health on specific warrior and print an appropriate message.
        public static string HealthDown(Hero warrior, int value)
        {
            if (warrior == null)
                return "Decreases health by " + value + ".";

            warrior.healthpoints = DecreaseStat(warrior.healthpoints, value, "health points");
            return "";
        }
        // Method for increasing stamina on specific warrior and print an appropriate message.
        public static string StaminaUp(Hero warrior, int value)
        {
            if (warrior == null)
                return "Increases stamina by " + value + ", but not above maximum stamina.";

            warrior.stamina = IncreaseStat(warrior.stamina, warrior.maxStamina, value, "stamina points");
            return "";
        }
        // Method for decreasing stamina on specific warrior and print an appropriate message.
        public static string StaminaDown(Hero warrior, int value)
        {
            if (warrior == null)
                return "Decreases stamina by " + value + ".";

            warrior.stamina = DecreaseStat(warrior.stamina, value, "stamina points");
            return "";
        }
        // Method for increasing composure on specific warrior and print an appropriate message.
        public static string ComposureUp(Hero warrior, int value)
        {
            if (warrior == null)
                return "Increases composure by " + value + ", but not above maximum composure.";

            warrior.composure = IncreaseStat(warrior.composure, warrior.baseComposure, value, "composure");

            if (Game.HasAilment(warrior, Game.ailments[7])) // panicking
            {
                warrior.ailments.Remove(Game.ailments[7]);
                if (warrior.ailments.Count == 0)
                {
                    Game.partyMembersInPeril.Remove(warrior.alltimePartyId);
                }
            }
            return "";
        }
        // Method for decreasing composure on specific warrior and print an appropriate message.
        public static string ComposureDown(Hero warrior, int value)
        {
            if (warrior == null)
                return "Decreases composure by " + value + ".";

            warrior.composure = DecreaseStat(warrior.composure, value, "composure");
            return "";
        }
        // Method for increasing health by half on specific warrior and print an appropriate message.
        public static string HealthUpHalf(Hero warrior, int value)
        {
            if (warrior == null)
                return "Increases your health by half your regular health or " + value + " points, whichever is higher, but not above maximum health.";

            int actualChange = value;
            int oldPoints = warrior.healthpoints;

            if (warrior.maxHealth / 2 < value)
                warrior.healthpoints += value;
            else
            {
                actualChange = warrior.maxHealth / 2;
                warrior.healthpoints += warrior.maxHealth / 2;
            }

            if (warrior.healthpoints > warrior.maxHealth)
            {
                actualChange = warrior.maxHealth - oldPoints;
                warrior.healthpoints = warrior.maxHealth;
            }

            Game.WrtL("Warrior got " + actualChange + " health points back!\n");
            return "";
        }
        // Method for giving full health to specific warrior and print an appropriate message.
        public static string HealthUpFull(Hero warrior)
        {
            if (warrior == null)
                return "Recovers your health fully.";

            warrior.healthpoints = warrior.maxHealth;

            Game.WrtL("Warrior got all of " + warrior.pronoun[2] + " health points back!\n");
            return "";
        }
        // Method for increasing stamina by half on specific warrior and print an appropriate message.
        public static string StaminaUpHalf(Hero warrior, int value)
        {
            if (warrior == null)
                return "Increases your stamina by half your regular stamina or " + value + " points, whichever is higher, but not above maximum stamina.";

            int actualChange = value;
            int oldPoints = warrior.stamina;

            if (warrior.stamina / 2 < value)
                warrior.stamina += value;
            else
            {
                actualChange = warrior.maxStamina / 2;
                warrior.stamina += warrior.maxStamina / 2;
            }

            if (warrior.stamina > warrior.maxStamina)
            {
                actualChange = warrior.maxStamina - oldPoints;
                warrior.stamina = warrior.maxStamina;
            }

            Game.WrtL("Warrior got " + actualChange + " stamina points back!\n");
            return "";
        }
        // Method for giving full stamina to specific warrior and print an appropriate message.
        public static string StaminaUpFull(Hero warrior)
        {
            if (warrior == null)
                return "Recovers your stamina fully.";

            warrior.stamina = warrior.maxStamina;

            Game.WrtL("Warrior got all of " + warrior.pronoun[2] + " stamina points back!\n");
            return "";
        }
        // Method for increasing strength on specific warrior and print an appropriate message.
        public static string StrengthPlus(Hero warrior, int value, bool isEquipped)
        {
            if (warrior == null)
                return "Increases strength by " + value + ".";

            warrior.extraStrengthPoints += value;
            if (!isEquipped)
                warrior.strength += warrior.extraStrengthPoints;

            Game.WrtL("Warrior got " + value + " more strength points!\n");
            return "";
        }
        // Method for increasing speed on specific warrior and print an appropriate message.
        public static string SpeedPlus(Hero warrior, int value, bool isEquipped)
        {
            if (warrior == null)
                return "Increases speed by " + value + ".";

            warrior.extraSpeedPoints += value;
            if (!isEquipped)
                warrior.speed += warrior.extraSpeedPoints;

            Game.WrtL("Warrior got " + value + " more speed points!\n");
            return "";
        }
        // Method for increasing composure on specific warrior and print an appropriate message.
        public static string ComposurePlus(Hero warrior, int value, bool isEquipped)
        {
            if (warrior == null)
                return "Increases composure by " + value + ". Might cure \"berserk\" if this is not an equippable gear.";

            bool wasBerserking = false;

            warrior.extraComposurePoints += value;
            if (!isEquipped)
            {
                warrior.composure += warrior.extraComposurePoints;

                Game.RemoveIfHasAilment(warrior, Game.ailments[7]);

                if (Game.HasAilment(warrior, Game.ailments[2])) // berserk
                {
                    int rand = Game.randomize.Next(2);

                    if (rand == 0)
                    {
                        Game.RemoveAilment(warrior, Game.ailments[2]);

                        warrior.strength -= warrior.berserkValue;
                        if (warrior.strength <= 0)
                            warrior.strength = 1;

                        warrior.speed -= warrior.berserkValue;
                        if (warrior.speed <= 0)
                            warrior.speed = 1;

                        warrior.berserkValue = 0;
                        wasBerserking = true;

                        Game.WrtL("Warrior was brought out of berserk!");
                    }
                }
            }

            Game.Wrt("Warrior got " + value + " more points of composure");
            if (wasBerserking)
            {
                Game.Wrt(", and it brought " + warrior.pronoun[1] + " back from berserking");
            }
            Game.WrtL("!\n");
            return "";
        }
        // Method for increasing armor on specific warrior and print an appropriate message.
        public static string ArmorPlus(Hero warrior, int value)
        {
            if (warrior == null)
                return "Increases armor by " + value + ".";

            warrior.armor += value;

            Game.WrtL("Warrior gets " + value + " more dice in defence!\n");
            return "";
        }
        // Method for increasing attack on specific warrior and print an appropriate message.
        public static string AttackDicePlus(Hero warrior, int value)
        {
            if (warrior == null)
                return "Increases attack by " + value + ".";

            warrior.attackDice += value;

            Game.WrtL("Warrior gets " + value + " more dice in attack!\n");
            return "";
        }
        // Method for increasing the chance of getting a critical hit for specific warrior and print an appropriate message.
        public static string CriticalDicePlus(Hero warrior, int value)
        {
            if (warrior == null)
                return "Increases critical chance by " + value + ".";

            warrior.bonusCriticalDice += value;

            Game.WrtL("Warrior gets " + value + " more critical dice!\n");
            return "";
        }
        // Method for adding equipment weight to specific warrior and print an appropriate message.
        public static string WeightPlus(Hero warrior, int value)
        {
            if (warrior == null)
                return "Adds to equipment weight by " + value + ".";

            warrior.weightOfEquipment += value;

            Game.WrtL("This adds " + value + " points of weight to your equipment.\n");
            return "";
        }
        // Method for decreasing several specific stats on specific warrior and print an appropriate message.
        public static string Intoxication(Hero warrior, int value)
        {
            if (warrior == null)
                return "Decreases composure and stamina by " + value + ".";

            int compDown = value;
            int staminaDown = value;

            if ((warrior.composure - value) < 0)
            {
                compDown = warrior.composure;
                warrior.composure = 0;
            }
            else
                warrior.composure -= value;

            if ((warrior.stamina - value) < 0)
            {
                staminaDown = warrior.stamina;
                warrior.stamina = 0;
            }
            else
                warrior.stamina -= value;

            Game.WrtL("Warrior is a bit drunk.");

            if (compDown == staminaDown)
            {
                value = compDown;
                Game.WrtL("It strikes composure and stamina, both are " + value + " points down!");
            }
            else
            {
                Game.WrtL("It strikes composure and stamina!");
                Game.WrtL("Stamina is down " + staminaDown + " points and composure is down " + compDown + " points!");
            }

            if (warrior.stamina == 0)
            {
                Game.WrtL("Stamina is all gone! Warrior is weakened and cannot attack!");
            }
            if (warrior.composure == 0)
            {
                Game.WrtL("Oh no! This warrior has lost " + warrior.pronoun[2] + " composure completely!");
            }
            Game.WrtL("");
            return "";
        }
        // Method for decreasing stamina on specific warrior and print an appropriate message.
        public static string Tiring(Hero warrior, int value)
        {
            if (warrior == null)
                return "Causes \"fatigue\". Makes you lose stamina faster and makes it hard to get stamina back up.";

            warrior.stamina -= value;

            if (!Game.HasAilment(warrior, Game.ailments[1]))
            {
                warrior.ailments.Add(Game.ailments[1]); // fatigue
                Game.AddToPerilListIfAbsent(warrior.alltimePartyId);
            }
            return "";
        }
        // Method for decreasing armor/defence on specific warrior and print an appropriate message.
        public static string DefenceDown(Hero warrior, int value)
        {
            if (warrior == null)
                return "Decreases defence by " + value + ".";

            warrior.armor -= value;
            if (warrior.armor < 0)
                warrior.armor = 0;

            Game.WrtL("Warrior gets " + value + " less dice in defence!\n");
            return "";
        }
        // Method for decreasing composure on specific warrior and print an appropriate message.
        public static string Berserk(Hero warrior, int value)
        {
            if (warrior == null)
                return "Causes \"berserk\". Increases strength and speed by " + value + ", but also decreases composure by 1.";

            warrior.composure--;

            if (warrior.composure <= 0)
            {
                warrior.composure = 0;

                Game.WrtL("Oh no... " + warrior.name + " had way too low composure already, and it's completely lost now!");
                Game.WrtL("The positive effects of going berserk is lost.");

                if (!Game.HasAilment(warrior, Game.ailments[7]))
                {
                    warrior.ailments.Add(Game.ailments[7]); // panicking
                    Game.AddToPerilListIfAbsent(warrior.alltimePartyId);
                }
            }
            else
            {
                if (!Game.HasAilment(warrior, Game.ailments[2]))
                {
                    warrior.ailments.Add(Game.ailments[2]); // berserk
                    Game.AddToPerilListIfAbsent(warrior.alltimePartyId);

                    warrior.strength += value;
                    warrior.speed += value;

                    warrior.berserkValue = value;

                    Game.WrtL(Game.CapitalizeFirstLetter(warrior.pronoun[0]) + "'s got " + value + " more points of strength and speed but it also strikes");
                    Game.WrtL("composure with 1 point down!");
                }
            }

            Game.WrtL("");
            return "";
        }
        // Method for decreasing and/or increasing several stats on specific warrior and print an appropriate message.
        public static string Hyper(Hero warrior, int value)
        {
            if (warrior == null)
                return "Increases speed by around " + value + " points, and armor will decrease by 1.\nOther effects is quite random and can be good or bad. Also cures \"fatigue\".";
                //return "Increases speed by around " + value + " points, will increase OR decrease stamina and strength by around " + value + ",\ndecreases composure by around " + value + " and decreases armor by 1.";

            Game.WrtL("Boldly, " + warrior.name + " gulps the entire purple potion!");

            warrior.extraSpeedPoints += Game.randomize.Next(value - 1, value + 2);
            Game.WrtL(Game.CapitalizeFirstLetter(warrior.pronoun[2]) + " speed has increased with " + value + " points!");

            int rand = Game.randomize.Next(3);
            if (rand == 0)
            {
                rand = Game.randomize.Next(value - 1, value + 2);
                warrior.stamina -= rand;
                Game.Wrt(Game.CapitalizeFirstLetter(warrior.pronoun[2]) + " stamina has decreased with " + rand + " points!");
                if (warrior.stamina <= 0)
                {
                    warrior.stamina = 0;
                    Game.Wrt("It took all " + warrior.pronoun[0] + " had... ");
                }
                Game.Wrt("\n");
            }
            else
            {
                rand = Game.randomize.Next(value - 1, value + 1);
                warrior.stamina += rand;
                Game.WrtL(Game.CapitalizeFirstLetter(warrior.pronoun[2]) + " stamina has increased with " + rand + " points!");
            }

            rand = Game.randomize.Next(3);

            if (rand == 0)
            {
                rand = Game.randomize.Next(value - 1, value + 1);
                warrior.strength -= rand;
                Game.Wrt(Game.CapitalizeFirstLetter(warrior.pronoun[2]) + " strength has decreased with " + rand + " points!");
                if (warrior.strength <= 1)
                {
                    warrior.strength = 1;
                    Game.Wrt("It's at absolute lowest possible, just 1 strength point left...");
                }
                Game.Wrt("\n");
            }
            else if (rand == 1)
            {
                rand = Game.randomize.Next(value - 1, value + 1);
                warrior.extraStrengthPoints += rand;
                Game.WrtL(Game.CapitalizeFirstLetter(warrior.pronoun[2]) + " strength has increased with " + rand + " points!");
            }

            rand = Game.randomize.Next(0, value + 1);
            warrior.composure -= rand;
            warrior.armor -= 1;

            if (rand != 0)
                Game.Wrt(Game.CapitalizeFirstLetter(warrior.pronoun[2]) + " composure has decreased " + rand + " points, and " + warrior.pronoun[2] + " armor by one point!");
            else
                Game.Wrt(Game.CapitalizeFirstLetter(warrior.pronoun[2]) + " armor has decreased by one point!");

            // Hyper cures fatigue, because of course it does.
            Game.RemoveIfHasAilment(warrior, Game.ailments[1]); // fatigue

            if (warrior.composure <= 0)
            {
                warrior.composure = 0;

                if (!Game.HasAilment(warrior, Game.ailments[7]))
                {
                    warrior.ailments.Add(Game.ailments[7]); // panicking
                    Game.AddToPerilListIfAbsent(warrior.alltimePartyId);
                }

                Game.Wrt("\n" + Game.CapitalizeFirstLetter(warrior.pronoun[0]) + " lost all composure and is panicking! ");
            }

            if (warrior.armor <= 0)
            {
                warrior.armor = 0;
                Game.Wrt(Game.CapitalizeFirstLetter(warrior.pronoun[0]) + " has no armor left...");
            }
            Game.WrtL("\n");
            return "";
        }
        // Method for randomizing effect on specific warrior and print an appropriate message.
        public static string RandomEffect(Hero warrior, int value)
        {
            if (warrior == null)
                return "Can be any type of single effect potion, giving OR draining around " + value + " points in something.";

            int rand = Game.randomize.Next(14);
            int randValue = Game.randomize.Next(value -1, value +2);

            string[] randomTypes = new string[] { "HealthUp", "HealthDown", "StaminaUp", "StaminaDown", "ComposureUp", "ComposureDown",
                                                  "HealthUpHalf", "HealthUpFull", "StaminaUpHalf", "StaminaUpFull",
                                                  "StrengthPlus", "SpeedPlus", "ComposurePlus", "Intoxication" };

            // I tailor the amounts a bit for certain types of potions as too high numbers on them are quite too cruel or too bloody good.
            if (rand == 4 || rand == 5)
                randValue--;
            else if (rand > 9)
            {
                if (randValue > 2)
                    randValue--;
            }

            UseItemFunction(warrior, randomTypes[rand], randValue);

            return "";
        }
        // Method for removing ailment bleeding.
        public static string CureBleeding(Hero warrior)
        {
            if (warrior == null)
                return "Stops heavy bleeding.";

            Game.RemoveAilment(warrior, Game.ailments[0]);

            Game.WrtL(warrior.name + "'s bleeding has stopped!");
            return "";
        }
        // Method for removing ailment poisoned.
        public static string CurePoisoned(Hero warrior, int value)
        {
            if (warrior == null)
            {
                if (value == 1)
                    return "Cures non magical poisoning.";
                else
                    return "Might cure non magical poisoning.";
            }
            
            int rand = Game.randomize.Next(value);

            if (rand == 0)
            {
                Game.RemoveAilment(warrior, Game.ailments[9]);

                warrior.recoverFromPoisoningCounter = 4;

                Game.WrtL(warrior.name + " is cured from the poisoning. " + Game.CapitalizeFirstLetter(warrior.pronoun[0]) + " will recover steadily.");
            }
            else
            {
                Game.WrtL("The poison couldn't be purged... It continues to eat away at " + warrior.name + "'s strength and health.");
            }
            return "";
        }
        // Method for removing ailment dying.
        public static string Resurrection(Hero warrior, int value)
        {
            if (warrior == null)
            {
                if (value >= 8)
                    return "Resurrects a fallen warrior, and cures residual ailments.";
                else
                    return "Resurrects a fallen warrior if they haven't died from bleeding. Doesn't cure other ailments.";
            }

            if (value >= 8 || !Game.HasAilment(warrior, Game.ailments[0]))
                Game.RemoveAilment(warrior, Game.ailments[8]);
            else
            {
                Game.WrtL("Having bled to death, this method of resurrection doesn't work... " + warrior.name + " stays cold on the ground.");
                return "";
            }
            
            Game.WrtL(warrior.name + " was resurrected!");

            if (value >= 8) // Right now, this means you used the Rainbow Shard, and it's magical. Fixes everything.
            {
                if (Game.RemoveIfHasAilment(warrior, Game.ailments[0])) // bleeding
                {
                    Game.WrtL(Game.CapitalizeFirstLetter(warrior.pronoun[2]) + " bleeding wounds have closed magically and " + warrior.pronoun[0] + " suffers no bloodloss!");
                }
                if (Game.RemoveIfHasAilment(warrior, Game.ailments[9])) // poisoned
                {
                    warrior.strength = warrior.maxStrength;
                    Game.WrtL("The poison in " + warrior.pronoun[2] + " body has been vanquished completely!");
                }
            }

            warrior.healthpoints = value;
            if (warrior.healthpoints > warrior.maxHealth)
                warrior.healthpoints = warrior.maxHealth;

            warrior.composure = value;
            if (warrior.composure > warrior.baseComposure)
                warrior.composure = warrior.baseComposure;

            if (warrior.healthpoints > warrior.maxHealth / 2)
            {
                Game.WrtL(Game.CapitalizeFirstLetter(warrior.pronoun[0]) + "'s feeling ok.");
            }
            else
            {
                Game.WrtL(Game.CapitalizeFirstLetter(warrior.pronoun[0]) + "'s feeling a bit weak still, and could need some food.");
            }

            if (warrior.composure > 5)
            {
                Game.WrtL(Game.CapitalizeFirstLetter(warrior.pronoun[2]) + " mind is shook, but alright. Fit for fight.");
            }
            else
            {
                Game.WrtL(Game.CapitalizeFirstLetter(warrior.pronoun[2]) + " head is spinning, and " + warrior.pronoun[0] + " needs some rest. Fighting will be tough yet.");
            }

            if (Game.HasAilment(warrior, Game.ailments[9])) // poisoned
            {
                Game.WrtL("\n" + Game.CapitalizeFirstLetter(warrior.pronoun[0]) + " still has poison in " + warrior.pronoun[2] + " blood, and it need to be remedied.");
                if (warrior.strength < 2)
                {
                    warrior.strength = 2;
                    Game.WrtL("It's far gone, and will kill " + warrior.pronoun[1] + " again soon!");
                }
            }
            return "";
        }
        // Method for inhibiting speed on monster.
        public static string PhysicalMobilityInhibitor(Hero warrior, Monster beast, int value)
        {
            if (warrior == null)
                return "Will diminish or nullify a monsters speed if successful. Successfulness depends on warrior speed versus monster speed.";

            int randVal = Game.randomize.Next((warrior.speed/2)-1, warrior.speed+2);

            if (randVal >= beast.speed / 2)
            {
                value += randVal - (beast.speed / 2);

                beast.speed -= value;
                if (beast.speed < 0)
                    beast.speed = 0;

                if (beast.speed == 0)
                {
                    beast.mobilityInhibitCounter += 3;
                    Game.WrtL("It was very effective! The " + beast.name + "'s movement is completely impaired!");
                }   
                else if (beast.speed < 4)
                {
                    beast.mobilityInhibitCounter += 2;
                    Game.WrtL("It was somewhat effective. The " + beast.name + "'s movement is decently impaired.");
                }
                else if (beast.speed < 7)
                {
                    beast.mobilityInhibitCounter += 1;
                    Game.WrtL("It wasn't very effective. The " + beast.name + "'s movement is only slightly affected.");
                }
                else if (beast.speed >= 7)
                {
                    Game.WrtL("That was a waste of time. The " + beast.name + " is still moving rather effortlessly.");
                }

                if (beast.mobilityInhibitCounter > 5)
                {
                    beast.mobilityInhibitCounter = 5;
                }
            }
            else
            {
                Game.WrtL("It failed! The beast completely evaded.");
            }

            Game.AwaitKeyEnter();

            return "";
        }
        // Method for stunning monsters, diminishing many of their stats gravely.
        public static string Stun(Hero warrior, Monster beast, int value)
        {
            if (warrior == null)
                return "Will stun a monster for a short while if the attack is successful.";

            int randVal = Game.randomize.Next(5);

            if (randVal == 0)
            {
                // 20% chance of failure
                Game.WrtL("The " + beast.name + " were lucky to look away at the right moment!");
                Game.WrtL(warrior.name + " failed to stun the monster...");
            }
            else
            {
                beast.stunCounter = value;
                beast.speed = 2;
                beast.strength -= 3;
                beast.composure--;
                if (beast.strength <= 0)
                    beast.strength = 1;

                if (beast.composure < 0)
                    beast.composure = 0;

                Game.WrtL("The " + beast.name + " screeches and squirms with its' eyes closed, backing up a bit.");
                Game.WrtL(warrior.name + " successfully stunned the monster!");
            }

            Game.AwaitKeyEnter();

            return "";
        }
        // NOTE! After the use of disposable weapons we need to check how the monster is doing, should it die or try to run. These are things that could happen.

    }
}
