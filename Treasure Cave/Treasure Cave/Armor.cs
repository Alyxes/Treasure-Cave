using System.Collections.Generic;

namespace TreasureCave
{
    public class Armor:Gear
    {
        // public static string[] armorClasses = { "armor", "shield" };
        public Armor(string aName, int aClass, string aDescription, int aDef, int aWeight, string type1, string type2, int wieldLVL, string specPow, int specPowVal, int cash)
        {
            types = new List<string>();

            Id = allGear.Count;
            name = aName;
            description = aDescription;
            defence = aDef;
            weight = aWeight;
            wieldLvl = wieldLVL;

            types.Add(armorClasses[aClass]);
            types.Add(type1);

            if (type2 != "")
                types.Add(type2);

            if (specPow != "None")
            {
                specialDataDescription = ItemSpecialPowerDescription(specPow, specPowVal);
                specialPower1 = specPow;
            }
            specialPower1Value = specPowVal;

            commonness = 5 - (defence/2);
            if (commonness < 1)
                commonness = 1;

            cost = cash;
        }
        static void CreateNewArmor(string aName, int aClass, string aDescription, int aDef, int aWeight, string type1, string type2, int wieldLVL, string specPow, int specPowVal, int cash)
        {
            Gear armor;
            armor = new Armor(aName, aClass, aDescription, aDef, aWeight, type1, type2, wieldLVL, specPow, specPowVal, cash);
            allGear.Add(armor);
        }
        public static void CreateAllArmor()
        {
            // Add some more of each type later, as well as some with magical attributes when that works. Until then this is enough here.
            // Body armor
            CreateNewArmor("Gambeson", 0, "A thick cloth tunic with long sleeves, protecting slightly against light strikes and cuts",
                1, 0, "cloth", "", 1, "None", 0, 200);
            CreateNewArmor("Poncho", 0, "A thick cloth poncho, covering only arms and chest, protecting slightly against light strikes and cuts.\nIts loose nature adds difficulty to land strikes beneath it.",
                1, 0, "cloth", "", 1, "None", 0, 200);
            CreateNewArmor("Hemp Shroud", 0, "A thick hemp cloth veil to wrap your body in, covering waist, torso, head and shoulders, protecting\nslightly against light strikes and cuts.",
                1, 0, "cloth", "", 1, "None", 0, 200);
            CreateNewArmor("Light Leather Tunic", 0, "A light tunic made of thin leather, covering waist, torso and shoulders, protecting slightly against\nstrikes and cuts.",
                2, 0, "leather", "", 1, "None", 0, 350);
            CreateNewArmor("Thick Leather Vest", 0, "A vest made of thick leather, covering only the chest; it's defensive function is limited.\nProtecting moderately against strikes and cuts.",
                2, 1, "leather", "", 1, "None", 0, 300);
            CreateNewArmor("Birch-bark Tunic", 0, "A regular length tunic without sleeves, made of birch-bark and thick cloth, giving decent coverage.\nProtecting decently against strikes and cuts.",
                3, 1, "wood", "cloth", 1, "None", 0, 370);
            CreateNewArmor("Leather Gambeson", 0, "A long tunic with sleeves, made of sturdy leather, giving good coverage.\nProtecting decently against strikes and cuts.",
                3, 2, "leather", "", 1, "None", 0, 350);
            CreateNewArmor("Wood Armor", 0, "Leather tunic equipped with hard wood plates, covering arms, shoulders, torso and waist. Slightly stumps mobility.\nProtecting decently against strikes and cuts.",
                3, 3, "wood", "leather", 1, "None", 0, 320);
            CreateNewArmor("Bone Armor", 0, "Monster bone jacket without sleeves put together by chains, strong against slashing attacks, weak against stabbing.\nProtecting decently against strikes and cuts.",
                3, 3, "bone", "", 1, "None", 0, 320);
            CreateNewArmor("Leather and Bone Armor", 0, "Leather tunic equipped with monster bone plates. Protecting well against strikes and cuts.",
                4, 3, "bone", "leather", 1, "None", 0, 400);
            CreateNewArmor("Chain Mail", 0, "Tunic made of small metal rings linked together, granting mobility with good defence, but quite heavy.\nProtecting well against tougher strikes and cuts.",
                4, 3, "chain", "", 1, "None", 0, 400);
            CreateNewArmor("Plated Chain Mail", 0, "Like a regular chain mail but with added thin metal plates covering the torso, granting some mobility with very good\ndefence, but quite heavy. Protecting well against tougher strikes and cuts.",
                5, 4, "chain", "plate", 1, "None", 0, 470);
            CreateNewArmor("Light Plate Armor", 0, "Thin plates of steel forming a torso, shoulders and faulds. Rather heavy, and stumps mobility a bit, but protects well\nagainst tough strikes and cuts.",
                6, 4, "plate", "", 2, "None", 0, 550);
            CreateNewArmor("Thick Plate Armor", 0, "Thick plates of steel forming a torso, shoulders, neck protection, arms and faulds. Very heavy, and stumps mobility,\nbut protects greatly against tough strikes and cuts.",
                7, 5, "plate", "", 2, "None", 0, 650);
            CreateNewArmor("Light Stone Plate Armor", 0, "Thin plates of stone put together with leather, forming a tunic like armor with sleeves.\nVery heavy, and stumps mobility quite a bit, but protects greatly against tough strikes and cuts.",
                7, 6, "stone", "leather", 3, "None", 0, 500);
            CreateNewArmor("Heavy Stone Plate Armor", 0, "Thick plates of stone put together with chain mail, forming a body armor with arms, shoulders and faulds.\nReally heavy, and remarkably stumps mobility, but protects greatly against all kinds\nof tougher strikes and cuts.",
                8, 7, "stone", "chain", 4, "None", 0, 550);
            CreateNewArmor("Crystal Armor", 0, "Leather armor equipped with crystals, protecting torso, shoulders and arms. Lighter than stone armor, but\nstill very heavy, stumping mobility. Protects superbly against all kinds of tougher strikes and cuts.",
                9, 5, "crystal", "leather", 5, "None", 0, 800);
            CreateNewArmor("Plate and Crystal Armor", 0, "Thin plate armor equipped with crystals, protecting chest, shoulders and arms. Lighter than stone armor, but\nstill very heavy, stumping mobility. Protects superbly against all kinds of the\ntoughest strikes and cuts.",
                10, 6, "crystal", "leather", 5, "None", 0, 950);

            // Shields
            CreateNewArmor("Wood Buckler", 1, "A small wooden shield, very light.",
                1, 0, "wood", "", 1, "None", 0, 150);
            CreateNewArmor("Wood Shield", 1, "A wooden shield, with steel edges.",
                2, 1, "wood", "", 1, "None", 0, 200);
            CreateNewArmor("Hard Wood Shield", 1, "A sturdy shield of harder and heavier wood, with steel edges and details.",
                3, 2, "wood", "", 1, "None", 0, 300);
            CreateNewArmor("Iron Buckler", 1, "A small iron shield, slightly curved.",
                3, 3, "steel", "", 1, "None", 0, 260);
            CreateNewArmor("Thin Steel Shield", 1, "A square, curved steel shield. Thin steel plate with leather handles.",
                4, 3, "steel", "", 1, "None", 0, 380);
            CreateNewArmor("Steel and Wood Shield", 1, "A sturdy wood shield with a steel plate front, ornamented.",
                5, 3, "steel", "wood", 2, "None", 0, 480);
            CreateNewArmor("Steel Plate Shield", 1, "A heavy shield, made of one thick steel plate, with fancy details and sturdy leather handles.",
                6, 5, "steel", "wood", 2, "None", 0, 550);

        }
    }
}
