using System.Collections.Generic;

namespace TreasureCave
{
    public class Weapon:Gear
    {
        /*
        public string[] sizes = { "small", "medium", "big" };
        // Think about making this list 2-dimensional to give every weapon type base stats...
        public static string[] weaponClasses = { "sword", "knife", "axe", "blunt", "bow", "crossbow", "spear", "sickle", "whip" };
        public string weaponClass;
        public string size;
        public int wieldLvl; // The level needed from the warrior to wield this weapon.
        public int dualWieldLvl; // The level needed from the warrior to double wield this weapon.
        public int safeDice;
        public int skillDice;
        public int defenceDice;
        public int criticalChance;
        public int attack;
        */

        // Constructor
        public Weapon(string wName, int wClass, int wSize, string type1, string type2, string type3, int wieldLVL, int dualLVL, bool _isDoubleHandheld,
                      string specPow, int specPowVal, int cash, string wDescription)
        {
            types = new List<string>();

            Id = allGear.Count;
            name = wName;
            types.Add("weapon");
            weaponClass = weaponClasses[wClass];
            types.Add(weaponClass);
            isDoubleHandheld = _isDoubleHandheld;
            size = sizes[wSize];
            types.Add(size);

            int worth = 0;
            string des = "A ";
            int desCount = 0;

            if (isDoubleHandheld)
            {
                types.Add("twohanded");
                des += "twohanded ";
            }

            if (weaponClass == weaponClasses[0] || weaponClass == weaponClasses[1] || weaponClass == weaponClasses[2] || weaponClass == weaponClasses[3] || weaponClass == weaponClasses[7])
            {
                types.Add("melee");
                des += size + " melee weapon. ";
                desCount++;
            }
            else if (weaponClass == weaponClasses[4] || weaponClass == weaponClasses[5] || weaponClass == weaponClasses[6] || weaponClass == weaponClasses[8])
            {
                types.Add("ranged");
                des += size + " ranged weapon. ";
                desCount++;
            }

            wieldLvl = wieldLVL;
            dualWieldLvl = dualLVL;

            safeDice = 2;
            criticalChance = 1;

            if (wSize == 0)
            {
                skillDice = 1;
                defenceDice = 0;
                commonness = 4;
                worth = 2;
            } 
            else if (wSize == 1)
            {
                skillDice = 2;
                defenceDice = 1;
                commonness = 3;
                worth = 4;
            }
                
            else if (wSize == 2)
            {
                skillDice = 3;
                defenceDice = 2;
                commonness = 2;
                worth = 5;
            }

            if (weaponClass == "knife")
            {
                safeDice = 1;
                if (defenceDice > 0)
                    defenceDice -= 1;

                worth -= 1 + (1 * (wSize + 1));
            }

            if (specPow != "None")
            {
                specialDataDescription = ItemSpecialPowerDescription(specPow, specPowVal);
                specialPower1 = specPow;
                commonness -= 1;
                worth += 2;
                des += "Special power: " + specialPower1 + " ";
                desCount++;
            }
            specialPower1Value = specPowVal;

            if (specialPower1Value > 3)
            {
                commonness -= 1;
                worth += 2;
            }

            if (type1 != "")
                types.Add(type1);
            if (type2 != "")
                types.Add(type2);
            if (type3 != "")
                types.Add(type3);

            if ((type1 != "" || type1 == "") && type2 == "" && type3 == "")
            {
                commonness += 1;
                worth -= 1;
                if (type1 == "" && specPow == "None")
                {
                    if (weaponClass == "blunt")
                        des += "Nothing special really, just an average " + size + " " + weaponClass + " weapon.";
                    else
                        des += "Nothing special really, just an average " + size + " " + weaponClass + ".";
                }
            }
                
            if (type3 != "")
            {
                commonness -= 1;
                if (type3 != "weak" && type3 != "fragile" && type3 != "dull" && type3 != "non-weapon")
                    worth += 1;
            }  
            if (wieldLvl == 1)
                commonness += 1;
            if (dualWieldLvl == 1)
                commonness += 1;
            if (wieldLvl > 4)
            {
                commonness -= 1;
                worth += 2;
            }

            foreach (string type in types)
            {
                if (desCount > 2)
                {
                    des += "\n";
                    desCount = 0;
                }
                if (type == "sharper" || type == "quicker")
                {
                    safeDice++;
                    des += Game.CapitalizeFirstLetter(type) + ", increases safe dice to attack. ";
                    desCount++;
                }
                else if (type == "light" || type == "steadier")
                {
                    skillDice++;
                    des += Game.CapitalizeFirstLetter(type) + ", increases skill dice to attack. ";
                    desCount++;
                } 
                else if (type == "durable" || type == "longer")
                {
                    defenceDice++;
                    worth += 2;
                    des += Game.CapitalizeFirstLetter(type) + ", increases defence. ";
                    desCount++;
                }
                // I keep the different names that grants the same bonus because I think about using them differently depending on warriors abilities.
                else if (type == "cleaver" || type == "jabby" || type == "spiky" || type == "mauling" || type == "impaler" || type == "scourger")
                {
                    criticalChance++;
                    commonness -= 1;
                    worth += 2;
                    des += Game.CapitalizeFirstLetter(type) + ", increases the critical chance. ";
                    desCount++;
                }
                else if (type == "fragile")
                {
                    if (defenceDice > 0)
                        defenceDice--;

                    commonness += 1;
                    worth -= 1;
                    des += "It's " + type + ", which decreases defence. ";
                    desCount++;
                }
                else if (type == "heavy")
                {
                    if (skillDice > 0)
                        skillDice--;
                    
                    worth += 1;

                    if (desCount != 0)
                    {
                        des += "\n";
                    }

                    des += "It's " + type + ", which decreases skill dice and adds to the weight; until you're strong enough, then all bad things\ngo away and turns into extra attack power.\n";
                    desCount = 0;
                }
                else if (type == "dull" || type == "weak")
                {
                    if (criticalChance > 0)
                        criticalChance--;

                    commonness += 1;
                    worth -= 2;
                    des += "A " + type + " weapon, diminishing critical chance";
                    desCount++;

                    if (weaponClass == "blunt")
                    {
                        if (safeDice > 1)
                            safeDice--;

                        des += " and might even affect the safe dice";
                        desCount++;
                    }
                    des += ". ";
                }
                else if (type == "non-weapon")
                {
                    if (safeDice > 1)
                        safeDice--;

                    commonness += 1;
                    worth -= 1;

                    if (desCount != 0)
                    {
                        des += "\n";
                    }

                    des += "This is not really meant to be a weapon, so the safe dice, affecting the attack, is diminished.\n";
                    desCount = 0;
                }
            }

            attack = safeDice + skillDice;

            worth += attack;

            if (worth <= 0)
                worth = 2;

            if (attack > 5)
                commonness -= 1;

            if (commonness <= 0)
                commonness = 1;

            // cost = cash;

            cost = 40 + (worth * 60);

            description = des;
        }
        static void CreateNewWeapon(string wName, int wClass, int wSize, string type1, string type2, string type3,
                                    int wieldLVL, int dualLVL, bool isDoubleHandheld, string specPow, int specPowVal, int cash, string wDescription)
        {
            Gear wpn;
            wpn = new Weapon(wName, wClass, wSize, type1, type2, type3, wieldLVL, dualLVL, isDoubleHandheld, specPow, specPowVal, cash, wDescription);
            allGear.Add(wpn);
        }
        // double handed is overrided by dual wield level. Once one has reached that level one can carry the weapon with one hand anyway.
        public static void CreateAllWeapons()
        {
            // small swords
            CreateNewWeapon("Shortsword", 0, 0, "", "", "", 1, 1, false, "None", 0, 350, "weapon");
            CreateNewWeapon("Dwarf Sword", 0, 0, "durable", "", "", 1, 1, false, "None", 0, 350, "weapon");
            CreateNewWeapon("Court Sword", 0, 0, "jabby", "fragile", "light", 1, 2, false, "None", 0, 300, "weapon");
            CreateNewWeapon("Wide Shortsword", 0, 0, "cleaver", "", "", 1, 1, false, "None", 0, 300, "weapon");
            CreateNewWeapon("Short Scimitar", 0, 0, "fragile", "light", "", 1, 2, false, "None", 0, 350, "weapon");
            CreateNewWeapon("Short Sabre", 0, 0, "", "", "", 1, 2, false, "None", 0, 350, "weapon");
            CreateNewWeapon("Gladius", 0, 0, "durable", "", "", 1, 2, false, "None", 0, 300, "weapon");
            CreateNewWeapon("Small Cutlass", 0, 0, "cleaver", "", "", 1, 1, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Thin Blade", 0, 0, "sharper", "fragile", "", 1, 1, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Wakizashi", 0, 0, "sharper", "", "", 1, 2, false, "None", 0, 350, "weapon");
            // medium swords
            CreateNewWeapon("Soldier Sword", 0, 1, "", "", "", 1, 1, false, "None", 0, 450, "weapon");
            CreateNewWeapon("Grass Blade", 0, 1, "sharper", "fragile", "light", 2, 3, false, "None", 0, 500, "weapon");
            CreateNewWeapon("Guerilla Sword", 0, 1, "fragile", "", "", 1, 1, false, "None", 0, 350, "weapon");
            CreateNewWeapon("Savage Sword", 0, 1, "durable", "", "", 1, 1, false, "None", 0, 400, "weapon");
            CreateNewWeapon("Wide Sword", 0, 1, "cleaver", "", "", 2, 3, false, "None", 0, 450, "weapon");
            CreateNewWeapon("Rapier", 0, 1, "jabby", "light", "", 2, 3, false, "None", 0, 450, "weapon");
            CreateNewWeapon("Katana", 0, 1, "sharper", "cleaver", "", 3, 4, true, "None", 0, 600, "weapon");
            CreateNewWeapon("Falchion", 0, 1, "cleaver", "", "", 2, 3, false, "None", 0, 450, "weapon");
            CreateNewWeapon("Longsword", 0, 1, "durable", "", "", 2, 3, true, "None", 0, 500, "weapon");
            CreateNewWeapon("Scimitar", 0, 1, "light", "", "", 2, 3, false, "None", 0, 450, "weapon");
            CreateNewWeapon("Sabre", 0, 1, "cleaver", "", "", 1, 2, false, "None", 0, 400, "weapon");
            CreateNewWeapon("Cutlass", 0, 1, "cleaver", "durable", "", 1, 2, false, "None", 0, 400, "weapon");
            CreateNewWeapon("Spadroon", 0, 1, "jabby", "cleaver", "fragile", 2, 3, false, "None", 0, 500, "weapon");
            // big swords
            CreateNewWeapon("Barbarian Sword", 0, 2, "durable", "heavy", "", 1, 3, true, "None", 0, 600, "weapon");
            CreateNewWeapon("Greatsword", 0, 2, "durable", "heavy", "cleaver", 1, 4, true, "None", 0, 650, "weapon");
            CreateNewWeapon("Claymore", 0, 2, "cleaver", "", "", 2, 4, true, "None", 0, 700, "weapon");
            CreateNewWeapon("Odatchi", 0, 2, "sharper", "cleaver", "", 3, 5, true, "None", 0, 750, "weapon");
            CreateNewWeapon("Giant Sword", 0, 2, "heavy", "cleaver", "", 3, 4, true, "None", 0, 680, "weapon");
            CreateNewWeapon("Iron Slab", 0, 2, "heavy", "", "", 3, 4, true, "None", 0, 600, "weapon");

            // small knives
            CreateNewWeapon("Pocket Knife", 1, 0, "light", "non-weapon", "", 1, 1, false, "None", 0, 150, "weapon");
            CreateNewWeapon("Soldier Knife", 1, 0, "durable", "", "", 1, 1, false, "None", 0, 180, "weapon");
            CreateNewWeapon("Gutting Knife", 1, 0, "sharper", "", "", 1, 1, false, "None", 0, 180, "weapon");
            CreateNewWeapon("Kozuka", 1, 0, "sharper", "fragile", "", 1, 2, false, "None", 0, 150, "weapon");
            CreateNewWeapon("Short Dagger", 1, 0, "durable", "", "", 1, 1, false, "None", 0, 180, "weapon");
            CreateNewWeapon("Switchblade", 1, 0, "light", "fragile", "", 2, 3, false, "None", 0, 150, "weapon");
            CreateNewWeapon("Stiletto", 1, 0, "light", "fragile", "jabby", 2, 3, false, "None", 0, 180, "weapon");
            CreateNewWeapon("Throwing Knife", 1, 0, "light", "", "", 2, 4, false, "None", 0, 100, "weapon");  // Throwing weapons is hard to make functional as a regular weapon... They will eventually be downgraded to disposable weapons.
            CreateNewWeapon("Kunai", 1, 0, "light", "sharper", "", 2, 4, false, "None", 0, 120, "weapon");
            CreateNewWeapon("Throwing Dagger", 1, 0, "", "", "", 2, 4, false, "None", 0, 130, "weapon");
            CreateNewWeapon("Jambiya", 1, 0, "sharper", "durable", "", 1, 2, false, "None", 0, 180, "weapon");
            // medium knives
            CreateNewWeapon("Tanto", 1, 1, "sharper", "", "", 1, 2, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Machete", 1, 1, "cleaver", "", "", 1, 1, false, "None", 0, 200, "weapon");
            CreateNewWeapon("Dagger", 1, 1, "", "", "", 1, 2, false, "None", 0, 200, "weapon");
            CreateNewWeapon("Combat Knife", 1, 1, "jabby", "", "", 1, 2, false, "None", 0, 220, "weapon");
            CreateNewWeapon("Thief Knife", 1, 1, "jabby", "light", "", 1, 1, false, "None", 0, 200, "weapon");
            CreateNewWeapon("Goblin Knife", 1, 1, "fragile", "light", "", 1, 1, false, "None", 0, 180, "weapon");
            CreateNewWeapon("Guerilla Knife", 1, 1, "fragile", "light", "cleaver", 1, 1, false, "None", 0, 180, "weapon");
            CreateNewWeapon("Sai", 1, 1, "light", "jabby", "durable", 2, 2, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Butcher's Knife", 1, 1, "cleaver", "non-weapon", "", 1, 1, false, "None", 0, 200, "weapon");
            CreateNewWeapon("Jile", 1, 1, "cleaver", "", "", 1, 2, false, "None", 0, 200, "weapon");
            // big knives
            CreateNewWeapon("Big Machete", 1, 2, "cleaver", "", "", 1, 3, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Long Dagger", 1, 2, "jabby", "", "", 1, 3, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Billhook", 1, 2, "durable", "cleaver", "", 1, 4, false, "None", 0, 300, "weapon");
            CreateNewWeapon("Kukri", 1, 2, "durable", "cleaver", "", 2, 3, false, "None", 0, 320, "weapon");
            CreateNewWeapon("Battle Knife", 1, 2, "jabby", "durable", "", 1, 3, false, "None", 0, 300, "weapon");
            CreateNewWeapon("Skeleton Cleaver", 1, 2, "sharper", "cleaver", "", 2, 3, false, "None", 0, 350, "weapon");

            // small axes
            CreateNewWeapon("Hatchett", 2, 0, "", "", "", 1, 1, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Throwing Axe", 2, 0, "light", "", "", 2, 3, false, "None", 0, 200, "weapon");
            CreateNewWeapon("Carpenter Axe", 2, 0, "light", "non-weapon", "", 1, 1, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Hammer-Axe", 2, 0, "durable", "", "", 1, 2, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Ono", 2, 0, "light", "cleaver", "", 1, 3, false, "None", 0, 300, "weapon");
            CreateNewWeapon("Francisca", 2, 0, "light", "fragile", "", 1, 2, false, "None", 0, 280, "weapon");
            CreateNewWeapon("Hurlbat", 2, 0, "light", "", "", 2, 3, false, "None", 0, 180, "weapon");
            CreateNewWeapon("Sagaris", 2, 0, "light", "", "", 2, 3, false, "None", 0, 280, "weapon");
            CreateNewWeapon("Boarding Axe", 2, 0, "durable", "", "", 1, 1, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Tribe Axe", 2, 0, "sharper", "", "", 2, 2, false, "None", 0, 280, "weapon");
            CreateNewWeapon("Stone Axe", 2, 0, "heavy", "cleaver", "", 2, 2, false, "None", 0, 220, "weapon");
            CreateNewWeapon("Light Pick", 2, 0, "light", "sharper", "", 1, 2, false, "None", 0, 280, "weapon");
            // medium axes
            CreateNewWeapon("Felling Axe", 2, 1, "", "", "", 1, 3, false, "None", 0, 350, "weapon");
            CreateNewWeapon("Broad Axe", 2, 1, "cleaver", "", "", 2, 3, false, "None", 0, 380, "weapon");
            CreateNewWeapon("Pickaxe", 2, 1, "durable", "jabby", "", 1, 3, false, "None", 0, 320, "weapon");
            CreateNewWeapon("Mattock", 2, 1, "durable", "cleaver", "", 1, 3, false, "None", 0, 350, "weapon");
            CreateNewWeapon("Hooked Axe", 2, 1, "sharper", "cleaver", "heavy", 2, 4, false, "None", 0, 420, "weapon");
            CreateNewWeapon("Alpenstock", 2, 1, "light", "fragile", "non-weapon", 1, 3, false, "None", 0, 280, "weapon");
            CreateNewWeapon("Battle Axe", 2, 1, "durable", "sharper", "", 2, 3, true, "None", 0, 420, "weapon");
            CreateNewWeapon("Dwarven Axe", 2, 1, "durable", "heavy", "", 1, 3, false, "None", 0, 380, "weapon");
            CreateNewWeapon("Pirate Axe", 2, 1, "durable", "", "", 1, 2, false, "None", 0, 320, "weapon");
            CreateNewWeapon("Norse Axe", 2, 1, "sharper", "", "", 2, 3, false, "None", 0, 400, "weapon");
            CreateNewWeapon("Bardiche", 2, 1, "cleaver", "jabby", "fragile", 2, 4, true, "None", 0, 400, "weapon");
            CreateNewWeapon("Sparth", 2, 1, "jabby", "light", "fragile", 2, 3, true, "None", 0, 380, "weapon");
            CreateNewWeapon("War Pick", 2, 1, "longer", "light", "", 1, 3, false, "None", 0, 350, "weapon");
            // big axes
            CreateNewWeapon("Greataxe", 2, 2, "durable", "cleaver", "", 2, 5, true, "None", 0, 650, "weapon");
            CreateNewWeapon("Barbarian Axe", 2, 2, "durable", "heavy", "", 1, 4, true, "None", 0, 620, "weapon");
            CreateNewWeapon("Long Battle Axe", 2, 2, "durable", "sharper", "", 3, 6, true, "None", 0, 700, "weapon");
            CreateNewWeapon("Long Broad Axe", 2, 2, "cleaver", "fragile", "", 2, 5, true, "None", 0, 600, "weapon");
            CreateNewWeapon("Great Norse Axe", 2, 2, "sharper", "durable", "", 3, 4, true, "None", 0, 680, "weapon");
            CreateNewWeapon("Double Headed Axe", 2, 2, "durable", "heavy", "", 2, 6, true, "None", 0, 650, "weapon");
            CreateNewWeapon("Double Headed Greataxe", 2, 2, "durable", "cleaver", "heavy", 3, 7, true, "None", 0, 700, "weapon");
            CreateNewWeapon("Double Headed Norse Axe", 2, 2, "sharper", "durable", "heavy", 3, 7, true, "None", 0, 700, "weapon");
            CreateNewWeapon("Great Bardiche", 2, 2, "cleaver", "jabby", "", 2, 5, true, "None", 0, 700, "weapon");
            CreateNewWeapon("Double Headed Barbarian Axe", 2, 2, "durable", "cleaver", "heavy", 2, 6, true, "None", 0, 680, "weapon");
            CreateNewWeapon("Double Headed Dwarven Axe", 2, 2, "durable", "cleaver", "heavy", 2, 4, true, "None", 0, 650, "weapon");
            CreateNewWeapon("Great Sparth", 2, 2, "cleaver", "jabby", "durable", 3, 5, true, "None", 0, 700, "weapon");

            // small blunt weapons
            CreateNewWeapon("Wooden Club", 3, 0, "light", "fragile", "weak", 1, 1, false, "None", 0, 100, "weapon");
            CreateNewWeapon("Wooden Spike Club", 3, 0, "spiky", "fragile", "weak", 1, 1, false, "None", 0, 120, "weapon");
            CreateNewWeapon("Wooden Nunchaku", 3, 0, "light", "fragile", "", 1, 4, true, "None", 0, 180, "weapon");
            CreateNewWeapon("Wooden Baton", 3, 0, "light", "weak", "", 1, 1, false, "None", 0, 120, "weapon");
            CreateNewWeapon("Iron Baton", 3, 0, "light", "durable", "", 1, 1, false, "None", 0, 200, "weapon");
            CreateNewWeapon("Iron Nunchaku", 3, 0, "mauling", "light", "", 2, 5, true, "None", 0, 250, "weapon");
            CreateNewWeapon("Light Mace", 3, 0, "light", "", "", 1, 1, false, "None", 0, 180, "weapon");
            CreateNewWeapon("Short Mace", 3, 0, "mauling", "", "", 1, 1, false, "None", 0, 200, "weapon");
            CreateNewWeapon("Morning Star", 3, 0, "spiky", "mauling", "", 2, 2, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Flail", 3, 0, "light", "spiky", "", 1, 2, false, "None", 0, 280, "weapon");
            CreateNewWeapon("Combat Hammer", 3, 0, "", "", "", 1, 1, false, "None", 0, 180, "weapon");
            CreateNewWeapon("Stone Hammer", 3, 0, "mauling", "heavy", "", 2, 3, false, "None", 0, 200, "weapon");
            CreateNewWeapon("Masonry Hammer", 3, 0, "light", "jabby", "non-weapon", 1, 1, false, "None", 0, 220, "weapon");
            // medium blunt weapons
            CreateNewWeapon("Wooden Long Club", 3, 1, "mauling", "light", "fragile", 1, 1, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Bone Club", 3, 1, "light", "weak", "", 1, 1, false, "None", 0, 200, "weapon");
            CreateNewWeapon("Barbarian Mace", 3, 1, "mauling", "spiky", "", 1, 1, false, "None", 0, 320, "weapon");
            CreateNewWeapon("Long Iron Nunchaku", 3, 1, "mauling", "longer", "", 3, 6, true, "None", 0, 380, "weapon");
            CreateNewWeapon("San Setsu Kon", 3, 1, "durable", "light", "", 3, 7, true, "None", 0, 350, "weapon"); // three section staff
            CreateNewWeapon("Ashwood Staff", 3, 1, "longer", "light", "", 1, 5, true, "None", 0, 400, "weapon");
            CreateNewWeapon("Red Oak Staff", 3, 1, "longer", "mauling", "", 1, 5, true, "None", 0, 450, "weapon");
            CreateNewWeapon("Iron Mace Staff", 3, 1, "longer", "mauling", "heavy", 2, 6, true, "None", 0, 480, "weapon");
            CreateNewWeapon("Long Morning Star", 3, 1, "longer", "spiky", "heavy", 2, 3, true, "None", 0, 500, "weapon");
            CreateNewWeapon("Dwarven Hammer", 3, 1, "mauling", "heavy", "durable", 1, 2, false, "None", 0, 430, "weapon");
            CreateNewWeapon("War Hammer", 3, 1, "mauling", "spiky", "", 1, 2, false, "None", 0, 420, "weapon");
            CreateNewWeapon("Norse Hammer", 3, 1, "mauling", "heavy", "", 2, 3, false, "None", 0, 380, "weapon");
            CreateNewWeapon("Bladed Mace", 3, 1, "mauling", "durable", "", 3, 4, false, "None", 0, 450, "weapon");
            // big blunt weapons
            CreateNewWeapon("Sledge Hammer", 3, 2, "mauling", "heavy", "", 1, 4, true, "None", 0, 480, "weapon");
            CreateNewWeapon("Battle Hammer", 3, 2, "mauling", "durable", "heavy", 2, 4, false, "None", 0, 500, "weapon");
            CreateNewWeapon("Spiked Battle Hammer", 3, 2, "mauling", "jabby", "heavy", 2, 5, false, "None", 0, 600, "weapon");
            CreateNewWeapon("Great Barbarian Mace", 3, 2, "mauling", "durable", "heavy", 2, 3, true, "None", 0, 450, "weapon");
            CreateNewWeapon("Great Bone Club", 3, 2, "mauling", "durable", "", 1, 3, true, "None", 0, 480, "weapon");
            CreateNewWeapon("Long Battle Mace", 3, 2, "mauling", "longer", "", 2, 6, true, "None", 0, 500, "weapon");
            CreateNewWeapon("Dwarven War Hammer", 3, 2, "mauling", "durable", "heavy", 1, 3, true, "None", 0, 600, "weapon");
            CreateNewWeapon("Great War Hammer", 3, 2, "mauling", "durable", "longer", 1, 6, true, "None", 0, 700, "weapon");
            CreateNewWeapon("Great Morning Star", 3, 2, "mauling", "spiky", "heavy", 3, 5, true, "None", 0, 650, "weapon");
            CreateNewWeapon("Giant Spike Club", 3, 2, "spiky", "heavy", "", 1, 4, true, "None", 0, 480, "weapon");

            // small bows
            CreateNewWeapon("Branch Bow", 4, 0, "light", "fragile", "weak", 1, 99, true, "None", 0, 180, "weapon"); // 99 means it's impossible to dual wield.
            CreateNewWeapon("Tribe Bow", 4, 0, "light", "fragile", "", 1, 99, true, "None", 0, 280, "weapon");
            CreateNewWeapon("Carved Bow", 4, 0, "light", "", "", 1, 99, true, "None", 0, 200, "weapon");
            CreateNewWeapon("Wooden Composite Bow", 4, 0, "quicker", "", "", 1, 99, true, "None", 0, 380, "weapon");
            CreateNewWeapon("Sneaky Bow", 4, 0, "light", "quicker", "fragile", 1, 99, true, "None", 0, 220, "weapon");
            CreateNewWeapon("Guard Bow", 4, 0, "steadier", "", "", 1, 99, true, "None", 0, 350, "weapon");
            CreateNewWeapon("Dwarven Bow", 4, 0, "durable", "", "", 1, 99, true, "None", 0, 320, "weapon");
            CreateNewWeapon("Southern Shore Bow", 4, 0, "light", "quicker", "fragile", 2, 99, true, "None", 0, 250, "weapon");
            // medium bows
            CreateNewWeapon("Flatbow", 4, 1, "steadier", "fragile", "", 1, 99, true, "None", 0, 450, "weapon");
            CreateNewWeapon("Finely Carved Bow", 4, 1, "quicker", "", "", 1, 99, true, "None", 0, 480, "weapon");
            CreateNewWeapon("Soldier Bow", 4, 1, "durable", "quicker", "", 1, 99, true, "None", 0, 580, "weapon");
            CreateNewWeapon("Composite Bow", 4, 1, "quicker", "steadier", "", 1, 99, true, "None", 0, 500, "weapon");
            CreateNewWeapon("East Empire Bow", 4, 1, "light", "quicker", "", 2, 99, true, "None", 0, 550, "weapon");
            CreateNewWeapon("Cavallery Bow", 4, 1, "steadier", "", "", 2, 99, true, "None", 0, 500, "weapon");
            CreateNewWeapon("Hunter Bow", 4, 1, "durable", "steadier", "", 1, 99, true, "None", 0, 580, "weapon");
            CreateNewWeapon("Tribe Warrior Bow", 4, 1, "quicker", "fragile", "", 1, 99, true, "None", 0, 450, "weapon");
            CreateNewWeapon("Horse Bow", 4, 1, "light", "weak", "", 1, 99, true, "None", 0, 450, "weapon");
            CreateNewWeapon("Hankyu Bow", 4, 1, "light", "steadier", "fragile", 2, 99, true, "None", 0, 550, "weapon");
            CreateNewWeapon("Combat Bow", 4, 1, "durable", "impaler", "", 1, 99, true, "None", 0, 480, "weapon");
            CreateNewWeapon("Southern Ocean Bow", 4, 1, "steadier", "light", "", 2, 99, true, "None", 0, 500, "weapon");
            // big bows
            CreateNewWeapon("Longbow", 4, 2, "steadier", "", "", 1, 99, true, "None", 0, 700, "weapon");
            CreateNewWeapon("Bear Bow", 4, 2, "durable", "impaler", "", 3, 99, true, "None", 0, 750, "weapon");
            CreateNewWeapon("Daikyu Bow", 4, 2, "light", "steadier", "", 3, 99, true, "None", 0, 800, "weapon");
            CreateNewWeapon("Norse Bow", 4, 2, "", "durable", "", 2, 99, true, "None", 0, 700, "weapon");
            CreateNewWeapon("Northern Shark Bow", 4, 2, "heavy", "impaler", "", 4, 99, true, "None", 0, 780, "weapon");
            CreateNewWeapon("Knight Bow", 4, 2, "steadier", "durable", "", 2, 99, true, "None", 0, 780, "weapon");
            CreateNewWeapon("Warrior Bow", 4, 2, "durable", "quicker", "impaler", 2, 99, true, "None", 0, 830, "weapon");
            CreateNewWeapon("Bladed Bow", 4, 2, "durable", "cleaver", "heavy", 3, 99, true, "None", 0, 780, "weapon");
            CreateNewWeapon("Heavy Hunter Bow", 4, 2, "durable", "impaler", "heavy", 3, 99, true, "None", 0, 780, "weapon");
            
            // small crossbows
            CreateNewWeapon("Wrist Crossbow", 5, 0, "light", "weak", "", 1, 5, false, "None", 0, 350, "weapon");
            CreateNewWeapon("One-handed Crossbow", 5, 0, "light", "weak", "fragile", 1, 2, false, "None", 0, 300, "weapon");
            CreateNewWeapon("Sneaky Crossbow", 5, 0, "light", "quicker", "weak", 1, 6, true, "None", 0, 480, "weapon");
            CreateNewWeapon("Dwarven Crossbow", 5, 0, "impaler", "", "", 1, 10, true, "None", 0, 420, "weapon");
            CreateNewWeapon("Quail Crossbow", 5, 0, "steadier", "", "", 1, 10, true, "None", 0, 450, "weapon");
            CreateNewWeapon("Latchet Crossbow", 5, 0, "quicker", "", "", 1, 8, true, "None", 0, 480, "weapon");
            // medium crossbows
            CreateNewWeapon("Soldier Crossbow", 5, 1, "quicker", "", "", 1, 15, true, "None", 0, 620, "weapon");
            CreateNewWeapon("Arbalest", 5, 1, "heavy", "impaler", "", 1, 15, true, "None", 0, 650, "weapon");
            CreateNewWeapon("War Crossbow", 5, 1, "quicker", "durable", "", 2, 15, true, "None", 0, 700, "weapon");
            CreateNewWeapon("Hunting Crossbow", 5, 1, "steadier", "", "", 1, 15, true, "None", 0, 650, "weapon");
            CreateNewWeapon("Bullet Crossbow", 5, 1, "light", "", "", 1, 15, true, "None", 0, 620, "weapon");
            CreateNewWeapon("Dwarven War Crossbow", 5, 1, "impaler", "durable", "", 2, 15, true, "None", 0, 700, "weapon");
            // big crossbows
            CreateNewWeapon("Heavy Crossbow", 5, 2, "heavy", "durable", "", 1, 99, true, "None", 0, 850, "weapon");
            CreateNewWeapon("Shoulder Ballista", 5, 2, "heavy", "impaler", "durable", 3, 99, true, "None", 0, 850, "weapon");
            CreateNewWeapon("Bear Crossbow", 5, 2, "impaler", "quicker", "", 2, 99, true, "None", 0, 900, "weapon");
            CreateNewWeapon("Boar Crossbow", 5, 2, "impaler", "steadier", "", 2, 99, true, "None", 0, 900, "weapon");
            CreateNewWeapon("Stone Shooter", 5, 2, "durable", "steadier", "", 3, 99, true, "None", 0, 880, "weapon");

            // small spears
            CreateNewWeapon("Wooden Spear", 6, 0, "light", "weak", "fragile", 1, 2, false, "None", 0, 180, "weapon");
            CreateNewWeapon("Javelin", 6, 0, "light", "fragile", "", 1, 3, false, "None", 0, 220, "weapon");
            CreateNewWeapon("Tribe Spear", 6, 0, "quicker", "", "", 1, 3, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Bamboo Spear", 6, 0, "light", "", "", 1, 3, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Spontoon", 6, 0, "durable", "", "", 1, 4, false, "None", 0, 280, "weapon");
            CreateNewWeapon("Harpoon", 6, 0, "impaler", "", "", 1, 3, false, "None", 0, 230, "weapon");
            // medium spears
            CreateNewWeapon("Long Spear", 6, 1, "longer", "jabby", "", 1, 4, true, "None", 0, 480, "weapon");
            CreateNewWeapon("Soldier Spear", 6, 1, "steadier", "", "", 1, 4, false, "None", 0, 450, "weapon");
            CreateNewWeapon("Throwing Spear", 6, 1, "steadier", "light", "", 1, 5, false, "None", 0, 450, "weapon");
            CreateNewWeapon("Thick Spear", 6, 1, "durable", "impaler", "", 1, 5, true, "None", 0, 500, "weapon");
            CreateNewWeapon("Fauchard", 6, 1, "longer", "sharper", "", 2, 5, true, "None", 0, 480, "weapon");
            CreateNewWeapon("Glaive", 6, 1, "cleaver", "", "", 2, 5, true, "None", 0, 500, "weapon");
            CreateNewWeapon("Pike", 6, 1, "impaler", "light", "", 1, 4, false, "None", 0, 450, "weapon");
            CreateNewWeapon("Long Spontoon", 6, 1, "longer", "jabby", "", 1, 5, true, "None", 0, 480, "weapon");
            CreateNewWeapon("Naginata", 6, 1, "sharper", "fragile", "", 2, 6, true, "None", 0, 500, "weapon");
            CreateNewWeapon("Shark Harpoon", 6, 1, "impaler", "durable", "", 3, 4, false, "None", 0, 480, "weapon");
            // big spears
            CreateNewWeapon("Heavy Spear", 6, 2, "durable", "impaler", "heavy", 2, 6, true, "None", 0, 650, "weapon");
            CreateNewWeapon("Great Spear", 6, 2, "steadier", "longer", "", 2, 7, true, "None", 0, 700, "weapon");
            CreateNewWeapon("Halberd", 6, 2, "cleaver", "durable", "", 3, 10, true, "None", 0, 680, "weapon");
            CreateNewWeapon("Great Fauchard", 6, 2, "longer", "sharper", "", 2, 8, true, "None", 0, 680, "weapon");
            CreateNewWeapon("Massive Glaive", 6, 2, "cleaver", "heavy", "", 3, 12, true, "None", 0, 700, "weapon");
            CreateNewWeapon("Lance", 6, 2, "longer", "impaler", "heavy", 2, 11, true, "None", 0, 700, "weapon");
            
            // small sickles
            CreateNewWeapon("Druid Sickle", 7, 0, "sharper", "fragile", "non-weapon", 1, 1, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Thief Sickle", 7, 0, "fragile", "light", "", 1, 1, false, "None", 0, 200, "weapon");
            CreateNewWeapon("Combat Sickle", 7, 0, "durable", "", "", 1, 1, false, "None", 0, 220, "weapon");
            CreateNewWeapon("Sickle Knife", 7, 0, "light", "", "", 1, 1, false, "None", 0, 220, "weapon");
            CreateNewWeapon("Chicken Sickle", 7, 0, "longer", "", "", 2, 3, false, "None", 0, 250, "weapon");
            CreateNewWeapon("New Moon Sickle", 7, 0, "sharper", "light", "fragile", 1, 2, false, "None", 0, 300, "weapon");
            CreateNewWeapon("Saw Teeth Sickle", 7, 0, "sharper", "", "", 1, 2, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Light Chakram", 7, 0, "light", "", "", 1, 1, false, "None", 0, 220, "weapon");
            CreateNewWeapon("Saw Teeth Chakram", 7, 0, "sharper", "", "", 1, 2, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Half Moon Chakram", 7, 0, "", "", "", 2, 2, false, "None", 0, 280, "weapon");
            CreateNewWeapon("Yin and Yang Chakram", 7, 0, "quicker", "sharper", "fragile", 2, 99, true, "None", 0, 300, "weapon");
            CreateNewWeapon("Shuriken", 7, 0, "quicker", "", "", 2, 2, false, "None", 0, 100, "weapon");
            // medium sickles
            CreateNewWeapon("Short Scythe", 7, 1, "light", "", "", 1, 6, true, "None", 0, 420, "weapon");
            CreateNewWeapon("Carved Scythe", 7, 1, "", "", "", 1, 6, true, "None", 0, 450, "weapon");
            CreateNewWeapon("Curved Scythe", 7, 1, "sharper", "", "", 1, 6, true, "None", 0, 450, "weapon");
            CreateNewWeapon("Crescent Blade", 7, 1, "sharper", "quicker", "", 2, 3, false, "None", 0, 480, "weapon");
            CreateNewWeapon("Savage Sickle", 7, 1, "durable", "", "", 1, 1, false, "None", 0, 380, "weapon");
            CreateNewWeapon("Bone Carver Sickle", 7, 1, "durable", "cleaver", "", 2, 3, false, "None", 0, 420, "weapon");
            CreateNewWeapon("Sickle Sword", 7, 1, "cleaver", "", "", 1, 2, false, "None", 0, 400, "weapon");
            CreateNewWeapon("Great Combat Sickle", 7, 0, "steadier", "durable", "", 2, 3, true, "None", 0, 400, "weapon");
            CreateNewWeapon("Khopesh", 7, 1, "cleaver", "sharper", "", 1, 2, false, "None", 0, 420, "weapon");
            CreateNewWeapon("Shotel", 7, 1, "light", "fragile", "", 1, 2, false, "None", 0, 380, "weapon");
            CreateNewWeapon("Kusarigama", 7, 1, "longer", "quicker", "", 3, 99, true, "None", 0, 450, "weapon"); // Chain sickle
            CreateNewWeapon("Great Chakram", 7, 1, "", "", "", 1, 2, false, "None", 0, 380, "weapon");
            CreateNewWeapon("Shark Teeth Chakram", 7, 1, "sharper", "", "", 3, 5, false, "None", 0, 420, "weapon");
            CreateNewWeapon("Wave Edge Chakram", 7, 1, "sharper", "steadier", "", 2, 3, false, "None", 0, 450, "weapon");
            // big sickles
            CreateNewWeapon("Reaper's Shadow", 7, 2, "cleaver", "sharper", "", 3, 15, true, "None", 0, 700, "weapon");
            CreateNewWeapon("War Scythe", 7, 2, "durable", "cleaver", "", 2, 15, true, "None", 0, 680, "weapon");
            CreateNewWeapon("Wide Blade Scythe", 7, 2, "cleaver", "heavy", "", 3, 15, true, "None", 0, 650, "weapon");
            CreateNewWeapon("Full Moon Chakram", 7, 2, "sharper", "steadier", "", 2, 10, true, "None", 0, 750, "weapon");
            CreateNewWeapon("Harvest Scythe", 7, 2, "steadier", "non-weapon", "", 1, 12, true, "None", 0, 580, "weapon");
            CreateNewWeapon("Giant Sickle", 7, 2, "sharper", "heavy", "", 1, 4, false, "None", 0, 650, "weapon");
            CreateNewWeapon("Blood Stone Sickle", 7, 2, "cleaver", "steadier", "", 2, 3, false, "None", 0, 750, "weapon");
            CreateNewWeapon("Shield Chakram", 7, 2, "durable", "steadier", "", 2, 3, false, "None", 0, 680, "weapon");
            CreateNewWeapon("Capoeira Chakram", 7, 2, "sharper", "quicker", "light", 4, 12, true, "None", 0, 750, "weapon");

            // small whips
            CreateNewWeapon("Light Leather Whip", 8, 0, "light", "quicker", "", 1, 2, false, "None", 0, 320, "weapon");
            CreateNewWeapon("Hemp Whip", 8, 0, "light", "fragile", "", 1, 2, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Root Whip", 8, 0, "quicker", "fragile", "", 2, 3, false, "None", 0, 220, "weapon");
            CreateNewWeapon("Yard Whip", 8, 0, "", "", "", 1, 2, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Snake Whip", 8, 0, "light", "", "", 2, 4, false, "None", 0, 250, "weapon");
            CreateNewWeapon("Tamer Whip", 8, 0, "steadier", "non-weapon", "", 2, 2, false, "None", 0, 200, "weapon");
            CreateNewWeapon("Horse Whip", 8, 0, "light", "weak", "fragile", 1, 2, false, "None", 0, 180, "weapon");
            // medium whips
            CreateNewWeapon("Leather Whip", 8, 1, "light", "steadier", "", 1, 3, false, "None", 0, 450, "weapon");
            CreateNewWeapon("Bat Leather Whip", 8, 1, "light", "sharper", "fragile", 1, 3, false, "None", 0, 500, "weapon");
            CreateNewWeapon("Bull Whip", 8, 1, "scourger", "durable", "", 2, 4, false, "None", 0, 480, "weapon");
            CreateNewWeapon("Redhide Whip", 8, 1, "steadier", "quicker", "", 1, 4, false, "None", 0, 520, "weapon");
            CreateNewWeapon("Twintip Whip", 8, 1, "scourger", "quicker", "fragile", 2, 5, false, "None", 0, 480, "weapon");
            CreateNewWeapon("Fang Whip", 8, 1, "scourger", "durable", "", 3, 4, false, "None", 0, 500, "weapon");
            CreateNewWeapon("Snakebite Whip", 8, 1, "sharper", "quicker", "fragile", 2, 5, false, "None", 0, 500, "weapon");
            CreateNewWeapon("Shark Skin Whip", 8, 1, "scourger", "sharper", "", 1, 4, false, "None", 0, 600, "weapon");
            CreateNewWeapon("Leather Flail Whip", 8, 1, "scourger", "steadier", "", 2, 6, false, "None", 0, 550, "weapon");
            // big whips
            CreateNewWeapon("Long Leather Whip", 8, 2, "scourger", "longer", "steadier", 2, 5, false, "None", 0, 700, "weapon");
            CreateNewWeapon("Black Whip", 8, 2, "scourger", "durable", "steadier", 1, 5, false, "None", 0, 680, "weapon");
            CreateNewWeapon("Hook Whip", 8, 2, "scourger", "sharper", "", 3, 7, false, "None", 0, 650, "weapon");
            CreateNewWeapon("Monsterhide Whip", 8, 2, "durable", "mauling", "", 1, 5, false, "None", 0, 680, "weapon");
            CreateNewWeapon("Morning Star Whip", 8, 2, "scourger", "mauling", "durable", 3, 9, true, "None", 0, 750, "weapon");
            CreateNewWeapon("Chain Whip", 8, 2, "mauling", "durable", "heavy", 2, 8, true, "None", 0, 700, "weapon");
            CreateNewWeapon("Dragon Scale Whip", 8, 2, "scourger", "longer", "sharper", 2, 6, false, "None", 0, 850, "weapon");
            CreateNewWeapon("Iron Flail Whip", 8, 2, "scourger", "durable", "heavy", 3, 8, true, "None", 0, 730, "weapon");
        }
    }
}
