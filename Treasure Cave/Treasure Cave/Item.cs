using System.Collections.Generic;

namespace TreasureCave
{
    public class Item:Gear
    {
        //public static string[] itemClasses = { "food", "potion", "jewelry", "clothes", "gadget", "artifact", "disposable weapon", "helmet", "gauntlet", "leg wear" };
        //public bool isEquippable;
        //public bool isSingleUse;

        // Constructor
        public Item(string iName, int iClass, string type1, string type2, string type3, string specPow1, int specPow1Val, string specPow2, int specPow2Val,
                    bool iEquippable, bool iPartyEquippable, int uses, bool iBattleOption, string iOptionPrompt, int cash, int iCommonness, string iDescription)
        {
            types = new List<string>();

            Id = allGear.Count;
            name = iName;
            description = iDescription;
            commonness = iCommonness;

            types.Add("item");
            itemClass = itemClasses[iClass];
            types.Add(itemClass);

            if (specPow1 != "None")
            {
                specialDataDescription = ItemSpecialPowerDescription(specPow1, specPow1Val);
                specialPower1 = specPow1;
            } 
            specialPower1Value = specPow1Val;

            if (specPow2 != "None")
            {
                specialDataDescription += "\n" + ItemSpecialPowerDescription(specPow2, specPow2Val);
                specialPower2 = specPow2;
            }
            specialPower2Value = specPow2Val;

            if (type1 != "")
                types.Add(type1);
            if (type2 != "")
                types.Add(type2);
            if (type3 != "")
                types.Add(type3);

            isEquippable = iEquippable;
            isPartyEquippable = iPartyEquippable;
            amountOfUses = uses;

            addsBattleOption = iBattleOption;
            if (addsBattleOption)
                battleOptionPrompt = iOptionPrompt;
            
            cost = cash;
        }
        static void CreateNewItem(string iName, int iClass, string type1, string type2, string type3, string specPow1, int specPow1Val, string specPow2, int specPow2Val,
                                  bool iEquippable, bool iPartyEquippable, int uses, bool iBattleOption, string iOptionPrompt, int cash, int iCommonness, string iDescription)
        {
            Gear item;
            item = new Item(iName, iClass, type1, type2, type3, specPow1, specPow1Val, specPow2, specPow2Val, iEquippable, iPartyEquippable, uses, iBattleOption, iOptionPrompt, cash, iCommonness, iDescription);
            allGear.Add(item);
        }
        public static void CreateAllItems()
        {
            // food
            CreateNewItem("Stale Bread", 0, "bread", "health", "", "HealthUp", 1, "None", 0, false, false, 1, false, "", 15, 7, "A stale old bread that still could have a grain of nutrition left.");
            CreateNewItem("Soft Bread", 0, "bread", "health", "stamina", "HealthUp", 2, "StaminaUp", 1, false, false, 1, false, "", 35, 6, "A soft bread with all the taste a bread shall have.");
            CreateNewItem("Dried Meat", 0, "meat", "health", "stamina", "HealthUp", 3, "StaminaUp", 1, false, false, 1, false, "", 50, 6, "Dried venison, tough but tasty, and full of proteins.");
            CreateNewItem("Fruit", 0, "fruit", "health", "stamina", "HealthUp", 2, "StaminaUp", 2, false, false, 1, false, "", 50, 6, "Different kind of fruit, sweet and juicy, rejuvenates and energizes your body.");
            CreateNewItem("Grilled Meat", 0, "meat", "health", "stamina", "HealthUp", 4, "StaminaUp", 2, false, false, 1, false, "", 70, 4, "Tasty grilled meat, falls apart in your mouth, filling stomachs and raising spirits.");
            CreateNewItem("Gaia Fruit", 0, "fruit", "health", "stamina", "HealthUp", 5, "StaminaUp", 3, false, false, 1, false, "", 100, 1, "A rare fruit from a gaia tree, its sweetness and fullness rejuvenates body and soul.");
            CreateNewItem("Wine", 0, "spirit", "composure", "", "ComposureUp", 1, "StaminaDown", 2, false, false, 1, false, "", 75, 4, "A simple bottle of wine. Calms your nerves a bit, but make you a little bit tired too.");
            CreateNewItem("Rum", 0, "spirit", "strength", "risky", "StrengthPlus", 2, "Intoxication", 2, false, false, 1, false, "", 150, 2, "A bottle of rum. Makes you feel stronger, but affects your reaction and focus badly.");
            CreateNewItem("Biscuits", 0, "bread", "health", "stamina", "HealthUp", 1, "StaminaUp", 2, false, false, 1, false, "", 35, 4, "Sweet little biscuits in different, quite tasteful shapes. Mostly with jam, but some are chocolate.\nYummy and energizing.");
            CreateNewItem("Big Turnip", 0, "root-crop", "health", "", "HealthUp", 2, "None", 0, false, false, 1, false, "", 20, 7, "Root vegetable. Has a lot of nutrients and fibers.\nIsn't so much fun to chomp on raw, but sometimes it's all you got.");
            CreateNewItem("Southern Beans Brew", 0, "beverage", "stamina", "", "StaminaUp", 3, "None", 0, false, false, 1, false, "", 40, 2, "A black beverage made by brewing ground beans from the south. Quite bitter, but it will wake you up.");

            // potion
            CreateNewItem("Small Health Potion", 1, "health", "", "", "HealthUpHalf", 5, "None", 0, false, false, 1, false, "", 150, 4, "A small bottle of deep red potion, helping the body heal wounds.");
            CreateNewItem("Big Health Potion", 1, "health", "", "", "HealthUpFull", 1, "None", 0, false, false, 1, false, "", 300, 2, "A hefty bottle of deep red potion, healing all wounds except heavily bleeding ones.");
            CreateNewItem("Small Stamina Potion", 1, "stamina", "", "", "StaminaUpHalf", 5, "None", 0, false, false, 1, false, "", 120, 4, "A small bottle of bright green potion, granting an energy boost.");
            CreateNewItem("Big Stamina Potion", 1, "stamina", "", "", "StaminaUpFull", 8, "None", 0, false, false, 1, false, "", 240, 2, "A hefty bottle of bright green potion, reenergizing your body to its full potential.");
            CreateNewItem("Small Zen Potion", 1, "composure", "", "", "ComposureUp", 2, "None", 0, false, false, 1, false, "", 200, 3, "A small bottle of pale blue potion, calming you down, helping you focus again.\nCan only normalize you, not boost your senses.");
            CreateNewItem("Big Zen Potion", 1, "composure", "", "", "ComposureUp", 4, "None", 0, false, false, 1, false, "", 400, 1, "A hefty bottle of pale blue potion, calming you down good, helps you regain your focus.\nCan only normalize you, not boost your senses.");
            CreateNewItem("Small Steroid Potion", 1, "strength", "risky", "", "StrengthPlus", 3, "ComposureDown", 1, false, false, 1, false, "", 250, 2, "A small bottle of yellow potion, granting a strength boost for about as long as a regular fight lasts.\nLowers composure, so should be used with care.");
            CreateNewItem("Big Steroid Potion", 1, "strength", "risky", "", "StrengthPlus", 5, "ComposureDown", 2, false, false, 1, false, "", 400, 1, "A hefty bottle of yellow potion, granting a powerful strength boost for about as long as a regular fight lasts.\nTakes a toll off composure, so use carefully.");
            CreateNewItem("Berserk Potion", 1, "strength", "attack increaser", "risky", "Berserk", 3, "DefenceDown", 3, false, false, 1, false, "", 400, 3, "A bottle of ash black potion, quickly making the user go into berserk with all the ups and downs.\nWill automatically wear off when the battle ecstasy does.");
            CreateNewItem("Pink Weird Potion", 1, "risky", "stamina", "hyper", "StaminaUpHalf", 5, "Hyper", 3, false, false, 1, false, "", 200, 3, "A bottle of strong pink potion that will make you hyper... Can be a little unpredictable and sometimes do more damage than good. Use when there's little other choice.");
            CreateNewItem("Focus Potion", 1, "risky", "composure", "", "ComposurePlus", 5, "Tiring", 1, false, false, 1, false, "", 350, 1, "A white milky potion. Hightens your senses and makes you extremely focused, but tires your body.\nYour stamina will drain faster and be almost impossible to raise by any means. It wears off quickly when the adrenaline lowers.");
            CreateNewItem("Dubious Potion", 1, "risky", "randomeffect", "", "RandomEffect", 3, "None", 0, false, false, 1, false, "", 70, 4, "A potion of unclear colour, mostly grey-ish. Who made it is unknown, and for what purpose.\nThe only way to know it's not dirt water is its classical potion like sloshing sound and faint smell off the bottle.\nAnd the only way to know what it does... is to drink it.");
            CreateNewItem("Antidote", 1, "ailment fixer", "", "", "CurePoisoned", 1, "None", 0, false, false, 1, false, "", 180, 3, "A small bottle of dark green potion, a cure to all non magical poisons and venoms.");
            // Night Vision Potion?
            // Speed Potion?
            // Stone skin potion? Defence.

            // jewelry
            CreateNewItem("Ring of Skill", 2, "attack increaser", "ring", "magical", "AttackDicePlus", 1, "None", 2, true, false, 0, false, "", 300, 1, "A simple gold ring with blue stones, but with a magical effect that hightens your weapon skill slightly.");
            CreateNewItem("Ring of Fire", 2, "fire", "ring", "magical", "FireDamage", 2, "None", 0, true, false, 0, false, "", 650, 1, "A ring made of black intertwined metal strands, holding a small red stone.\nIt gives the bearer a flame around their weapon, scorching the enemy as well when striking them.");
            CreateNewItem("Infernal Ring", 2, "fire", "ring", "magical", "FireDamage", 3, "ElementalFireAttack", 5, true, false, 0, true, "Use Infernal Ring: Fire Storm", 1200, 1, "A ring made of glowing red stone.\nIt gives the bearer flames around their weapon for burning their enemies, but also grants the ability to\ncreate a fire storm at will, possibly incinerating an enemy.");
            CreateNewItem("Ring of Zen", 2, "composure", "ring", "magical", "ComposurePlus", 2, "None", 0, true, false, 0, false, "", 750, 1, "A ring of silver, with an oval, dark green stone. It's magical effect grants the wearer higher focus.");
            CreateNewItem("Black Ring", 2, "risky", "ring", "magical", "AttackDicePlus", 2, "DemonicWinAndFaint", 1, true, false, 0, true, "Use Black Ring: Demonic Attack", 500, 1, "A ring made of obsidian. It holds a powerful and dangerous magical ability, an attack with the power of a demon.\nWhen the bearer choose to use it, their attack will be lethal, no matter how successful, as long as they hit at all.\nImmediately after however, the bearer faints for a while. So if they didn't hit, the enemy can strike an unconscious target.");
            CreateNewItem("Rynce Herb Necklace", 2, "herbal", "necklace", "confuser", "Confuse", 1, "None", 0, true, false, 0, false, "", 300, 2, "A necklace made of simple silver chain, with a few small containers filled with rynce herbs, which\nwill disturb and confuse most monsters.");
            CreateNewItem("Storm Necklace", 2, "magical", "necklace", "wind", "Shield", 4, "VisionImpairer", 3, true, false, 1, true, "Use Necklace: Defensive Storm", 450, 1, "A magical necklace with a blue white stone. If tugged off by the bearer it explodes and creates a powerful, dusty\nstorm around them, making them very hard to attack.\nWill dissipate soon, but another warrior can intervene before.");
            CreateNewItem("Light Crystal Necklace", 2, "magical", "necklace", "light", "Stun", 1, "LightMagicFlash", 7, true, false, 0, true, "Use Necklace: Stunning Flash", 550, 1, "A magical necklace with a big clear rock crystal in a silver collet with a silver chain.\nIt can - when the bearer holds it and focus - deliver a blinding flash of magic light, stunning all unblind creatures,\nmay even make them faint.\nIf a creature is sensitive to light and/or light magic it will be damaged or even killed as well.");
            CreateNewItem("Shielding Ayzl Necklace", 2, "magical", "necklace", "", "PeriodicProtection", 1, "None", 0, true, false, 0, false, "", 400, 1, "A magical necklace of fine iron with runes and beautiful details.\nThe bearer will randomly, not always, be completely protected from a non magical attack.\nThere's no way to know when, or to force it to happen, but sometimes you're gonna be real lucky\nwith this around your neck.");

            // clothes
            CreateNewItem("Eastern Velvet Shirt", 3, "cloth", "", "", "ArmorPlus", 1, "None", 0, true, false, 0, false, "", 250, 2, "A shirt made of the rare eastern velvet known for its strength as much as its beauty.");
            CreateNewItem("Fake Beard", 3, "confuser", "", "", "Confuse", 3, "None", 0, true, false, 0, true, "Throw your beard!", 100, 3, "A fake beard! Throw it off in the middle of battle for a great chance to confuse the monster.");
            CreateNewItem("Colourful Silk Scarf", 3, "cloth", "magical", "", "SpeedPlus", 1, "None", 0, true, false, 0, false, "", 200, 2, "A silk scarf with vibrant colours. It has a light magical enhancement, making the wearer feel lighter.");
            CreateNewItem("Dress Of Success", 3, "cloth", "magical", "attack increaser", "AttackDicePlus", 4, "Destructible", 3, true, false, 0, false, "", 400, 1, "An elegant but effectively designed dress for battle. With the coat of arms of the warrior princess\nof Roxcetia embroidered on the chest, it is magically imbued with battle spirit.\nIf the dress is too torn, specifically the coat of arms, the magic dissipates.");
            CreateNewItem("Underwear of Luck", 3, "cloth", "magical", "", "CriticalDicePlus", 1, "None", 0, true, false, 0, false, "", 200, 2, "Underwear made of simple cloth, magically enhanced to increase your chance to land critical hits.");
            // Red Cape

            // gadget
            CreateNewItem("Bandage", 4, "ailment fixer", "", "", "CureBleeding", 1, "None", 0, false, false, 1, false, "", 50, 5, "Very handy when a battle leads to heavily bleeding wounds.");
            CreateNewItem("Heart Shot", 4, "resurrecter", "", "", "Resurrection", 4, "StaminaUp", 4, false, false, 1, false, "", 200, 2, "A shot with a fluid only the best druids can conjure. It can make a stopped heart beat again by being injected\nstraight into it. Some wounds of the hearts bearer will heal, but the person will need additional rest\nor potions to feel well again and fully heal.");
            CreateNewItem("Bag of Zen Herbs", 4, "composure", "", "", "ComposureUp", 1, "None", 0, false, false, 1, false, "", 100, 5, "A small bag with soothing and calming herbs, to breath in and out from when needing to calm down.");
            CreateNewItem("Herb Stone", 4, "ailment fixer", "", "", "CurePoisoned", 3, "None", 0, false, false, 1, false, "", 100, 5, "A small hard pill made of many strange herbs, said to draw out poison from blood. It works sometimes.\nWhen poisoned, put it on the wound and let it absorb your blood. Wait.\nif you don't feel better soon you should see a doctor.");
            // Smelling salt to wake unconscious warriors.
            // Something that bursts into a small hot flame and cauterizes a bleeding wound.

            // artifact
            CreateNewItem("Ayzl Rune Stone", 5, "stone", "magical", "", "PeriodicProtection", 5, "None", 0, true, false, 5, false, "", 750, 1, "A rectangular old stone with runes carved into it.\nIf a bearer reads the runes out loud it will protect them completely from each fifth non magical attack.\nAfter five uses it will break and crumble from the strikes it has absorbed.");
            CreateNewItem("Ruby of Blood", 5, "stone", "magical", "", "DamageEqualizer", 1, "None", 0, false, true, 10, false, "", 950, 1, "A cursed blood ruby, set in a detailed metal frame, that will make any blood holding enemy too suffer all the\ninjuries they inflict upon anyone that has offered a few drops of blood to the stone.\nAfter it has used its magic ten times, the ruby will turn to obsidian and have no more worth than a stone.");
            CreateNewItem("Rainbow Shard", 5, "glass", "magical", "resurrecter", "Resurrection", 10, "StaminaUp", 10, false, false, 1, false, "", 500, 3, "A piece of natural glass, clear as water, with rainbow effects when looking through it.\nIt's said to come from volcanic areas with strong magical powers, and then having been magically steered by mages\nto be a powerful healing tool, even able to bring back someone slayed in battle unless they've been dismemebered.");
            CreateNewItem("Skull of Screams", 5, "bone", "magical", "", "ChaseOff", 1, "None", 0, false, false, 1, true, "Blow the Skull Whistle", 400, 1, "A small skull, unknown from what animal, somehow made into a whistle.\nThe sound of it is soul pearcing and will scare off any monster that isn't deaf.\nIt's a brittle little thing and will crumble into uselessness after one blow.");
            // Ice ball/orb, more like a disposable weapon but too magical to be in that category.

            // disposable weapon
            CreateNewItem("Fire Bottle", 6, "fire", "", "", "FireDamage", 5, "None", 0, false, false, 1, true, "Throw Fire Bottle", 150, 3, "A glass bottle of flammable oil, with a thick shred of cloth stuffed in it, hanging out.\nLight it on fire and throw it at something to make sure it burns good.");
            CreateNewItem("Bomb", 6, "explosive", "", "", "ExplosiveDamage", 6, "None", 0, false, false, 1, true, "Throw Bomb", 200, 2, "A round ceramic or metallic orb filled with gun powder and a fuse sticking out.\nLight it up, and time it well before throwing it at an enemy, you don't want it thrown or bouncing back on you...");
            CreateNewItem("Fishing Net", 6, "disruption", "", "", "PhysicalMobilityInhibitor", 5, "None", 0, false, false, 1, true, "Throw Fishing Net", 250, 4, "Simply a net to fling over an enemy to disrupt their movement.\nBeing a fishing net it is made of thin, tough thread that's really hard to rip by hand.");
            CreateNewItem("Blinding Cracker", 6, "disruption", "", "", "Stun", 1, "None", 0, false, false, 1, true, "Throw Blinding Cracker", 100, 4, "A cracker that when hitting the floor will explode with a blinding flash, impairing any seeing creature's vision\nmomentarily as well as shocking it, making sure it's unable to attack for a short while.");
            // Sleeping powder or liquid that turns gaseous.
            // All throwing weapons maybe should be put here. Throwing knives, shuriken, and such. Only question is: does one expect to be able to pick certain weapons up again after they're thrown?

            // helmet
            CreateNewItem("Tough Hood", 7, "armor", "cloth", "", "ArmorPlus", 1, "None", 0, true, false, 0, false, "", 180, 5, "A hood made of tough, ropy cloth. Light with a little protection.");
            CreateNewItem("Leather Cap", 7, "armor", "leather", "", "ArmorPlus", 1, "None", 0, true, false, 0, false, "", 180, 4, "A sturdy leather cap covering the ears and neck somewhat, light with a little protection.");
            CreateNewItem("Steel Hat", 7, "armor", "steel", "", "ArmorPlus", 1, "None", 0, true, false, 0, false, "", 180, 4, "A hat made of thin steel with a leather strapper.\nAlthough metal, it is thin enough to weigh very little, but also small enough to grant quite little extra protection.\nMost of all, you will look a little funny wearing it.");
            CreateNewItem("Chain Mail Hood", 7, "armor", "steel", "", "ArmorPlus", 2, "WeightPlus", 1, true, false, 0, false, "", 300, 3, "A hood made of chain mail, giving good protection to head and neck from cuts.\nCovering the whole head and partly the shoulders, it's also a bit heavy.");
            CreateNewItem("Steel Helmet", 7, "armor", "steel", "", "ArmorPlus", 3, "WeightPlus", 2, true, false, 0, false, "", 400, 1, "A helmet of thin steel plates, covering head and neck well, and face decently. Rather heavy.");
            // Bark helmet
            // Horned demon skull

            // gauntlet
            CreateNewItem("Leather Gloves", 8, "leather", "attack increaser", "", "AttackDicePlus", 1, "None", 0, true, false, 0, false, "", 250, 3, "Gloves made of strong but flexible leather, giving better grip on your weapon.");
            CreateNewItem("Steel Plate Gauntlets", 8, "steel", "", "", "ArmorPlus", 2, "WeightPlus", 1, true, false, 0, false, "", 380, 3, "Gauntlets from a full plate armor. A bit heavy to wear, but grants better defence.");
            CreateNewItem("Power Gloves", 8, "leather", "steel", "magical", "ArmorPlus", 1, "StrengthPlus", 2, true, false, 0, false, "", 750, 1, "Thick leather gloves with steel details, magically granting the wearer greater strength; and some\nprotection, although not magically.");
            CreateNewItem("Monster Skin Gloves", 8, "leather", "attack increaser", "magical", "ArmorPlus", 2, "AttackDicePlus", 1, true, false, 0, false, "", 650, 1, "Thick gloves made of monster skin, rough to wear but quite protective.\nThe monster skin has some magical enhancement as well on the attack power, increasing your\nweapon skill slightly.");
            // Snake skin gloves? What should they do?

            // leg wear
            CreateNewItem("Eagle Skin Boots", 9, "leather", "magical", "", "SpeedPlus", 2, "None", 0, true, false, 0, false, "", 550, 1, "Light eagle skin boots with ornamental eagle feathers. Magically imbued with the speed of an eagle,\ngranting the wearer a boost of their mobility and swiftness.");
            CreateNewItem("Black Leather Pants", 9, "leather", "", "", "ArmorPlus", 1, "None", 0, true, false, 0, false, "", 300, 3, "Pants made of black leather. Giving some protection, but mostly granting some dashing looks.");
            CreateNewItem("Steel Plate Greaves", 9, "steel", "", "", "ArmorPlus", 2, "WeightPlus", 2, true, false, 0, false, "", 380, 3, "Steel plates to attach on your legs, giving better defence. Adds quite some to your weight, affecting your movement.");
            CreateNewItem("Strange Cloth Shoes", 9, "cloth", "speed", "", "SpeedPlus", 1, "None", 0, true, false, 20, false, "", 150, 1, "A pair of small cloth shoes, oddly colourful. No oen seem to know where it originates from.\nMakes a wearer feel lighter on their steps, but seems to be bad in moist weather, against weaponry, against fire,\nfor walking a lot and walking in difficult terrain.");
            // Tough leather boots
        }
    }
}
