using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace TreasureCave
{
    public class Menu
    {
        // Method running the menu for choosing to create character and menu for loading a character.
        public static void NewHeroOrLoadMenu()
        {
            string[] menuOptions = new string[] { "Create New Character", "Load Character" };
            int markedOption = 0;

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                // Game.WrtL("");

                MenuPrinter(menuOptions, markedOption, "\t");

                Game.WrtL("dy", "", "\n\nArrow keys up and down to choose, Enter to commit.", true);

                var keyPressed = Console.ReadKey();

                // I find it better to let the user rotate the list instead of stopping at the top and bottom.
                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption++;
                    if (markedOption >= menuOptions.Length)
                        markedOption = 0;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption--;
                    if (markedOption < 0)
                        markedOption = menuOptions.Length - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    if (markedOption == 0)
                    {
                        Console.Clear();
                        Game.WrtL("w", "", "You will now create a character.\n", true);
                        Game.AwaitKeyEnter();
                        Game.CreateStartCharacter();
                        Game.AwaitKeyEnter();
                        return;
                    }
                    else if (markedOption == 1)
                    {
                        Console.Clear();
                        Console.CursorVisible = true;

                        string path = @"save/";

                        string[] loadMenuOptions = Directory.GetFiles(path);

                        if (loadMenuOptions.Length <= 0)
                        {
                            Game.WrtL("You have no saved characters yet. Create a new one instead!\n");
                            Game.AwaitKeyEnter();
                            continue;
                        }

                        for (int i = 0; i < loadMenuOptions.Length; i++)
                        {
                            char[] charsToTrim1 = { 's', 'a' , 'v', 'e'};
                            char[] charToTrimFinal1 = { '/' };
                            char[] charsToTrim2 = { 'x', 'm', 'l' };
                            char[] charToTrimFinal2 = { '.' };
                            loadMenuOptions[i] = loadMenuOptions[i].TrimStart(charsToTrim1);
                            loadMenuOptions[i] = loadMenuOptions[i].TrimStart(charToTrimFinal1);
                            loadMenuOptions[i] = loadMenuOptions[i].TrimEnd(charsToTrim2);
                            loadMenuOptions[i] = loadMenuOptions[i].TrimEnd(charToTrimFinal2);
                        }

                        int LoadMenuMarkedOption = 0;

                        while (true)
                        {
                            Console.Clear();
                            Console.CursorVisible = false;

                            Game.WrtL("w", "", "Which character would you like to load?", true);
                            Game.WrtL("If you wanna go back and create a new character instead, press space.\n");

                            MenuPrinter(loadMenuOptions, LoadMenuMarkedOption, "   ");

                            Game.WrtL("dy", "", "\n\nArrow keys up and down to choose, Enter to load.", true);

                            keyPressed = Console.ReadKey();

                            // I find it better to let the user rotate the list instead of stopping at the top and bottom.
                            if (keyPressed.Key == ConsoleKey.DownArrow)
                            {
                                LoadMenuMarkedOption++;
                                if (LoadMenuMarkedOption >= loadMenuOptions.Length)
                                    LoadMenuMarkedOption = 0;
                            }
                            else if (keyPressed.Key == ConsoleKey.UpArrow)
                            {
                                LoadMenuMarkedOption--;
                                if (LoadMenuMarkedOption < 0)
                                    LoadMenuMarkedOption = loadMenuOptions.Length - 1;
                            }
                            else if (keyPressed.Key == ConsoleKey.Spacebar)
                            {
                                break;
                            }
                            else if (keyPressed.Key == ConsoleKey.Enter)
                            {
                                Player You = Game.RecreateLoadedPlayer(loadMenuOptions[LoadMenuMarkedOption]);

                                Console.Clear();
                                Game.WrtL("Alright! This is the character loaded!\n");

                                PresentFinalizedPlayer(You);

                                Game.AwaitKeyEnter();

                                Game.WrtL("\n");
                                Game.WrtL("Are you perfectly satisfied with the skills and equipment or would you like to change something about this\nwarrior this time around?\n");
                                Game.WrtL("dy", "", "Press enter to PLAY NOW NOW NOW!", true);
                                Game.WrtL("dy", "", "Press space to change something", true);

                                keyPressed = Console.ReadKey();

                                if (keyPressed.Key == ConsoleKey.Enter)
                                {
                                    Console.Clear();
                                    Game.WrtL("Of course! Let's go!\n");
                                    PresentFinalizedPlayer(You);
                                }
                                else if (keyPressed.Key == ConsoleKey.Spacebar)
                                {
                                    Console.Clear();
                                    Game.WrtL("Absolutely.");
                                    Game.WrtL("w", "", "Remember that the name of the character will be the file name, so if you want to save both the old character\nand the new one, rename this version slightly, or move the old file from the Save folder.", true);
                                    Game.WrtL("\nNow just press Enter to go into the character builder.");

                                    Game.AwaitKeyEnter();

                                    CharacterBuilderMenu(You);

                                    SaveCharacterOrNot(You);

                                    PresentFinalizedPlayer(You);
                                }

                                Game.WrtL("\n");
                                Game.FinalizePlayerStuff(You);

                                Game.AwaitKeyEnter();

                                Console.Clear();
                                Game.WrtL("The fun, terrifying, dark cave crawling among monsters can begin!");

                                Game.AwaitKeyEnter();
                                return;
                            }
                        }
                        continue;
                    }
                }
            }
        }
        // Method for choosing to save or not save the recently created character.
        public static void SaveCharacterOrNot(Hero You)
        {
            Console.Clear();
            Game.WrtL("w", "", "Would you like to save this character?", true);
            Game.WrtL("If you do, you can reuse all these settings next time you play, without having to enter all the data again.");
            Game.WrtL("If you have just altered an existing saved character, remember that the name will be the file name, so if the\nname is exactly the same you will write over the old character...\n");
            Game.Wrt("dy", "", "Press Enter to save ", true);
            Game.WrtL("w", "", You.name, true);
            Game.WrtL("dy", "", "Press Spacebar to continue without saving this character.", true);

            var keyPressed = Console.ReadKey();

            if (keyPressed.Key == ConsoleKey.Enter)
            {
                Console.Clear();
                Game.WrtL("w", "", "Saving " + You.name + "...", false);

                Game.PrepareHeroForSave(You);

                Thread.Sleep(500);
                Game.WrtL("...");
                Thread.Sleep(500);
                Game.WrtL(" -.. /~ ¨_^ *' .~ * .. =# ¤.' - .- .. .' !");
                Thread.Sleep(500);
                Game.WrtL("\nThere! Saved!\n");
                Game.AwaitKeyEnter();
                Console.Clear();
            }
            else if (keyPressed.Key == ConsoleKey.Spacebar)
            {
                Console.Clear();
                Game.WrtL("Not saving.\n");
                Game.WrtL("Let's continue!\n");
                Game.AwaitKeyEnter();
                Console.Clear();
            }
        }

        // Method for the first game menu. Oh, so recurring.
        public static int StartMenu()
        {
            string[] menuOptions = new string[] { "To The Cave\t", "Go Into Village", "Manage Team\t", "Quit\t\t" };
            int markedOption = 0;

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                MenuPrinter(menuOptions, markedOption, "  ");

                Game.Wrt("\n\nBranzen: ");
                Game.WrtL("yl", "", Game.Branzen, true);
                Game.Wrt("\nTime: ");
                Game.WrtL("yl", "", Game.Clock(), true);

                var keyPressed = Console.ReadKey();

                // I find it better to let the user rotate the list instead of stopping at the top and bottom.
                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption++;
                    if (markedOption >= menuOptions.Length)
                        markedOption = 0;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption--;
                    if (markedOption < 0)
                        markedOption = menuOptions.Length - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    return markedOption;
                }
            }
        }
        // Method for the menu when in the town.
        public static int InTheVillage()
        {
            string[] menuOptions = new string[] { "Market Street\t\t", "The Tavern 'Mead Maiden'", "Drowzy Donkey Inn\t" };

            int markedOption = 0;

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                MenuPrinter(menuOptions, markedOption, "\t");

                Game.Wrt("\n\nBranzen: ");
                Game.WrtL("yl", "", Game.Branzen, true);
                Game.Wrt("\nTime: ");
                Game.WrtL("yl", "", Game.Clock(), true);

                if (markedOption == 0)
                {
                    // Market Street
                    Game.WrtL("\n\nIt's not a big village. This is mainly the street where all stores, stands and rolling merchants you might\nneed can be found.");
                }
                else if (markedOption == 1)
                {
                    // The Tavern 'Mead Maiden'
                    Game.WrtL("\n\nThis is where you best look for warriors that might join you to the cave.\nBut they probably won't come as cheap as the booze here do.");
                }
                else if (markedOption == 2)
                {
                    // Drowzy Donkey Inn
                    Game.WrtL("\n\nAfter a long or testing - or both - day in the cave, you might want to rejuvenate with a good meal and a good\nnights rest. This is the only decent place in the village to get that.");
                }

                var keyPressed = Console.ReadKey();

                // I find it better to let the user rotate the list instead of stopping at the top and bottom.
                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption++;
                    if (markedOption >= menuOptions.Length)
                        markedOption = 0;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption--;
                    if (markedOption < 0)
                        markedOption = menuOptions.Length - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Spacebar)
                {
                    return -1;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    return markedOption;
                }
            }
        }
        // Method for the menu when shopping. All logic for obtaining new items and removing them from the Party Gear List is in here.
        public static void ShopMenu()
        {
            int shopped = 0;

            string[] BuyOrSellOptions = new string[] { "Buy", "Sell" };

            int markedOptionInitial = 0;

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                Game.WrtL("yl", "", "   MARKET STREET", true);

                Game.Wrt("\nAvailable Branzen: ");
                Game.WrtL("yl", "", Game.Branzen, true);

                Game.WrtL("\nAre you here to buy or sell?\n");

                MenuPrinter(BuyOrSellOptions, markedOptionInitial, "   ");

                var keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOptionInitial++;
                    if (markedOptionInitial >= BuyOrSellOptions.Length)
                        markedOptionInitial = 0;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOptionInitial--;
                    if (markedOptionInitial < 0)
                        markedOptionInitial = BuyOrSellOptions.Length - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Spacebar)
                {
                    if (shopped > 0)
                    {
                        int shoppy = shopped/5;

                        if (shoppy < 1)
                            shoppy = 1;

                        Game.TimeUnitsPass(shoppy, false);
                    }
                    break;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    if (markedOptionInitial == 0)
                    {
                        // buy
                        List<string> menuOptions = new List<string>();
                        List<string> optionDescriptions = new List<string>();
                        List<string> detailedDescriptions = new List<string>();
                        // These exist in the shop every day:
                        List<string> shopItemsAvailable = new List<string> { "Soft Bread", "Dried Meat", "Fruit", "Grilled Meat", "Wine", "Biscuits", "Big Turnip",
                                                                 "Small Health Potion" , "Big Health Potion", "Small Stamina Potion", "Big Stamina Potion", "Small Zen Potion", "Antidote",
                                                                 "Bandage", "Heart Shot", "Bag of Zen Herbs", "Fishing Net", "Blinding Cracker" };

                        // I will add certain wares at different times when time is a real thing in the game, and there's more items that work...

                        for (int i = 0; i < shopItemsAvailable.Count; i++)
                        {
                            Gear shopItem = Gear.GearWithName(shopItemsAvailable[i]);
                            menuOptions.Add(shopItem.name + " -- " + shopItem.cost + " Br");
                            optionDescriptions.Add(shopItem.description);
                            detailedDescriptions.Add(shopItem.specialDataDescription);
                        }

                        int markedOption = 0;

                        while (true)
                        {
                            Console.Clear();
                            Console.CursorVisible = false;

                            Game.WrtL("yl", "", "   MARKET STREET", true);

                            Game.Wrt("\nAvailable Branzen: ");
                            Game.WrtL("yl", "", Game.Branzen, true);
                            Game.WrtL("\nThese are the wares to find here today:\n");

                            MenuPrinter(menuOptions, markedOption, "\t  ");

                            Game.WrtL("\n" + optionDescriptions[markedOption]);
                            Game.WrtL("\n" + detailedDescriptions[markedOption]);

                            keyPressed = Console.ReadKey();

                            // I find it better to let the user rotate the list instead of stopping at the top and bottom.
                            if (keyPressed.Key == ConsoleKey.DownArrow)
                            {
                                markedOption++;
                                if (markedOption >= menuOptions.Count)
                                    markedOption = 0;
                            }
                            else if (keyPressed.Key == ConsoleKey.UpArrow)
                            {
                                markedOption--;
                                if (markedOption < 0)
                                    markedOption = menuOptions.Count - 1;
                            }
                            else if (keyPressed.Key == ConsoleKey.Spacebar)
                            {
                                break;
                            }
                            else if (keyPressed.Key == ConsoleKey.Enter)
                            {
                                Console.Clear();
                                Console.CursorVisible = true;

                                Gear Item = Gear.GearWithName(shopItemsAvailable[markedOption]);

                                if (Item.cost > Game.Branzen)
                                {
                                    Game.WrtL("Sorry, you can't afford even one of them right now...");

                                    Game.AwaitKeyEnter();
                                    continue;
                                }
                                else
                                {
                                    string wantedAmount = "nothing";
                                    int amount;

                                    Game.WrtL(menuOptions[markedOption]);

                                    Game.Wrt("\nAvailable Branzen: ");
                                    Game.WrtL("yl", "", Game.Branzen, true);

                                    do
                                    {
                                        if (wantedAmount == "")
                                        {
                                            Game.WrtL("\nYou have to write a number.");
                                            Game.WrtL("Try again.");
                                        }
                                        else if (wantedAmount == "unaffordable")
                                        {
                                            Game.WrtL("\nYou don't have enough Branzen for that many...");
                                            Game.WrtL("Try a smaller amount.");
                                        }

                                        Game.WrtL("\nHow many do you need?");
                                        Game.Wrt("Amount: ");
                                        Game.w();
                                        wantedAmount = Console.ReadLine();
                                        Game.res();

                                        if (wantedAmount == "")
                                        {
                                            // Just pressed enter, assume 1.
                                            amount = 1;
                                        }
                                        else if (wantedAmount == " " || wantedAmount == "0")
                                        {
                                            amount = 0;
                                        }
                                        else
                                        {
                                            try
                                            {
                                                amount = Convert.ToInt32(wantedAmount);
                                            }
                                            catch
                                            {
                                                amount = -1;
                                                wantedAmount = "";
                                            }

                                            if (amount > -1 && ((amount * Item.cost) > Game.Branzen))
                                            {
                                                amount = -1;
                                                wantedAmount = "unaffordable";
                                            }
                                        }
                                    }
                                    while (amount == -1);

                                    if (amount == 0)
                                    {
                                        // Player pressed spacebar or wrote 0, they want to back out.
                                        continue;
                                    }

                                    Game.Wrt("\n" + amount + " " + Item.name);
                                    if (amount > 1)
                                        Game.Wrt("s");

                                    Game.WrtL(" will cost you " + (amount * Item.cost) + " Branzen.");
                                    Game.WrtL("Do we have a deal?");
                                    Game.WrtL("\nPress enter to purchase");
                                    Game.WrtL("\nPress space to decline");

                                    keyPressed = Console.ReadKey();

                                    if (keyPressed.Key == ConsoleKey.Spacebar)
                                    {
                                        Console.Clear();

                                        Game.WrtL("Perhaps something else will interest you, warrior.");

                                        Game.AwaitKeyEnter();
                                        continue;
                                    }
                                    else if (keyPressed.Key == ConsoleKey.Enter)
                                    {
                                        Game.Branzen -= Item.cost * amount;

                                        Console.Clear();

                                        Game.WrtL("Thank you, warrior!");

                                        for (int i = 0; i < amount; i++)
                                        {
                                            Game.partyGear.Add(Item);
                                            Game.SortPartyGear();
                                        }

                                        Game.Wrt("\nYou bought " + amount + " " + Item.name);
                                        if (amount > 1)
                                            Game.Wrt("s");

                                        Game.WrtL(".");

                                        shopped++;

                                        Game.AwaitKeyEnter();
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                    else if (markedOptionInitial == 1)
                    {
                        // sell
                        if (Game.partyGear.Count == 0)
                        {
                            Console.Clear();
                            Console.CursorVisible = true;

                            Game.WrtL("But you have nothing to sell right now...\n");

                            Game.AwaitKeyEnter();
                            continue;
                        }
                        
                        List<string> menuOptions = new List<string>();
                        List<string> optionDescriptions = new List<string>();
                        List<int> optionOriginalIndex = new List<int>();
                        List<int> optionAmount = new List<int>();

                        string ItemNameAndAmount;
                        int amount = 0;

                        bool soldSomething = true;
                        int markedOption = 0;

                        while (true)
                        {
                            if (Game.partyGear.Count == 0)
                            {
                                Console.Clear();
                                Console.CursorVisible = true;

                                Game.WrtL("You have sold all your items.\n");

                                Game.AwaitKeyEnter();
                                break;
                            }

                            if (soldSomething)
                            {
                                menuOptions.Clear();
                                optionAmount.Clear();
                                optionDescriptions.Clear();
                                optionOriginalIndex.Clear();

                                for (int i = 0; i < Game.partyGear.Count; i++)
                                {
                                    Gear item = Game.partyGear[i];
                                    int nextItemId = -1;

                                    if (i + 1 < Game.partyGear.Count)
                                        nextItemId = Game.partyGear[i + 1].Id;

                                    if (nextItemId == item.Id)
                                    {
                                        amount++;
                                    }
                                    else
                                    {
                                        amount++;
                                        int sellPrice = item.cost / 2;
                                        ItemNameAndAmount = item.name + ": " + amount + " -- sells for " + sellPrice + " Br";

                                        menuOptions.Add(ItemNameAndAmount);
                                        optionAmount.Add(amount);
                                        optionDescriptions.Add(item.description);
                                        optionOriginalIndex.Add(i);

                                        amount = 0;
                                    }
                                }
                                soldSomething = false;
                                markedOption = 0;
                            }

                            Console.Clear();
                            Console.CursorVisible = false;

                            Game.WrtL("Which items would you like to sell, warrior?\n");

                            MenuPrinter(menuOptions, markedOption, "  ");

                            Game.WrtL("\n\n" + optionDescriptions[markedOption]);

                            keyPressed = Console.ReadKey();

                            if (keyPressed.Key == ConsoleKey.DownArrow)
                            {
                                markedOption++;
                                if (markedOption >= menuOptions.Count)
                                    markedOption = 0;
                            }
                            else if (keyPressed.Key == ConsoleKey.UpArrow)
                            {
                                markedOption--;
                                if (markedOption < 0)
                                    markedOption = menuOptions.Count - 1;
                            }
                            else if (keyPressed.Key == ConsoleKey.Spacebar)
                            {
                                break;
                            }
                            else if (keyPressed.Key == ConsoleKey.Enter)
                            {
                                Console.Clear();
                                Console.CursorVisible = true;

                                Gear Item = Gear.GearWithName(Game.partyGear[optionOriginalIndex[markedOption]].name);

                                string sellingAmount = "nothing";
                                int amountToSell;

                                Game.WrtL(menuOptions[markedOption]);

                                if (optionAmount[markedOption] > 1)
                                {
                                    do
                                    {
                                        if (sellingAmount == "")
                                        {
                                            Game.WrtL("\nYou have to write a number.");
                                            Game.WrtL("Try again.");
                                        }
                                        else if (sellingAmount == "tooMany")
                                        {
                                            Game.WrtL("\nYou don't have that many...");
                                            Game.WrtL("Try a smaller amount.");
                                        }

                                        Game.WrtL("\nHow many do you want to sell?");
                                        Game.Wrt("Amount: ");
                                        Game.w();
                                        sellingAmount = Console.ReadLine();
                                        Game.res();

                                        try
                                        {
                                            amountToSell = Convert.ToInt32(sellingAmount);
                                        }
                                        catch
                                        {
                                            amountToSell = -1;
                                            sellingAmount = "";
                                        }

                                        if (amountToSell > -1 && (amountToSell > optionAmount[markedOption]))
                                        {
                                            amountToSell = -1;
                                            sellingAmount = "tooMany";
                                        }
                                    }
                                    while (amountToSell == -1);
                                }
                                else
                                    amountToSell = 1;

                                Game.Wrt("\n" + amountToSell + " " + Item.name);
                                if (amountToSell > 1)
                                    Game.Wrt("s");

                                Game.WrtL(" will give you " + (amountToSell * (Item.cost / 2)) + " Branzen.");
                                Game.WrtL("Do we have a deal?");
                                Game.WrtL("\nPress enter to sell");
                                Game.WrtL("\nPress space to decline");

                                keyPressed = Console.ReadKey();

                                if (keyPressed.Key == ConsoleKey.Spacebar)
                                {
                                    Console.Clear();

                                    Game.WrtL("Perhaps you want to sell something else, warrior.");

                                    Game.AwaitKeyEnter();
                                    continue;
                                }
                                else if (keyPressed.Key == ConsoleKey.Enter)
                                {
                                    Game.Branzen += amountToSell * (Item.cost / 2);

                                    for (int i = 0; i < amountToSell; i++)
                                    {
                                        Game.partyGear.Remove(Item);
                                    }

                                    soldSomething = true;
                                    shopped++;

                                    Console.Clear();

                                    Game.WrtL("Thank you, warrior!");
                                    Game.Wrt("\nYou sold " + amountToSell + " " + Item.name);
                                    if (amountToSell > 1)
                                        Game.Wrt("s");

                                    Game.WrtL(".");

                                    Game.AwaitKeyEnter();
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
        }
        // Method for applying stats from a chosen warrior type to the player object.
        public static void UpdateWarriorType(Hero player)
        {
            int choice = WarriorTypeOptionMenu(player);

            player.warriorTypeIndex = choice;

            player.type = Game.warriorTypes[choice];
            player.baseHealth = Game.warriorBaseStatArray[choice, 0];
            player.baseStrength = Game.warriorBaseStatArray[choice, 1];
            player.baseStamina = Game.warriorBaseStatArray[choice, 2];
            player.baseSpeed = Game.warriorBaseStatArray[choice, 3];
            player.baseComposure = Game.warriorBaseStatArray[choice, 4];
            player.composure = player.baseComposure;

            player.choiceOfWeapon.Clear();

            player.choiceOfWeapon.Add(Game.warriorPreferredWeaponry[choice, 0]);
            player.choiceOfWeapon.Add(Game.warriorPreferredWeaponry[choice, 1]);

            player.description = Game.warriorDescriptions[choice];

            Hero.UpdateWarriorStatsBeginning(player);
        }
        // Method for the character making menu, specifically the warrior type choice.
        public static int WarriorTypeOptionMenu(Hero player)
        {
            string[] menuOptions = new string[] { "Bounty Hunter", "Barbarian", "Ranger", "Rogue\t", "Duelist", "Fighter" };

            int markedOption = 0;

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                Game.WrtL("Choose a warrior type. Each one has certain base skills and preferred weapons, which will all be seen below.");
                Game.WrtL("Remember, when switching warrior type, it's a good idea to change your weapons to fit your new warrior type.\n");

                MenuPrinter(menuOptions, markedOption, "\t");

                Game.WrtL("\n" + Game.warriorDescriptions[markedOption]);
                Game.WrtL("\nBase skills:");
                Game.WrtL("Health: " + Game.warriorBaseStatArray[markedOption, 0] +
                          "\nStrength: " + Game.warriorBaseStatArray[markedOption, 1] +
                          "\nStamina: " + Game.warriorBaseStatArray[markedOption, 2] +
                          "\nSpeed: " + Game.warriorBaseStatArray[markedOption, 3]);
                Game.WrtL("\nThese can be increased, but not decreased.\n");
                if (markedOption == 4)
                {
                    // Duelist is marked.
                    Game.WrtL("As a duelist, any weapon you choose in the beginning will become your preferred weapon type.");
                    Game.WrtL("There will be two preferences, the first will be the weapon type, and the other the weapon size.");
                    Game.WrtL("Once you finish your warrior, these types are set and unchangable. However, one extra preference can be added later.");
                }
                else
                {
                    Game.Wrt("Favoured weapon types are " + Game.warriorPreferredWeaponry[markedOption, 0] + " and " + Game.warriorPreferredWeaponry[markedOption, 1] + ". ");
                    Game.WrtL("However, one extra preference can be added later.");
                }

                Game.WrtL("\nOne Small Health Potion is in your possession from the beginning, and this warrior type has additionally\nthe items "
                          + Game.warriorTypeStartingItems[markedOption, 0] + " and " + Game.warriorTypeStartingItems[markedOption, 1] + ".");

                var keyPressed = Console.ReadKey();

                // I find it better to let the user rotate the list instead of stopping at the top and bottom.
                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption++;
                    if (markedOption >= menuOptions.Length)
                        markedOption = 0;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption--;
                    if (markedOption < 0)
                        markedOption = menuOptions.Length - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    return markedOption;
                }
            }
        }
        // Method for the character builder menu. Some of the menu choices have functionality within this method, some of the more extensive ones have their own method.
        // All choices will eventually be put in their own method.
        public static void CharacterBuilderMenu(Hero player)
        {
            string[] menuOptions;

            int markedOption = 0;

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                string yourSpeedStatus = "Speed: " + player.speed + " (" + player.baseSpeed + " + " + player.addedSpeedPoints + ")";
                if (player.weightOfEquipment > 0)
                    yourSpeedStatus = "Speed: " + player.speed + " (" + player.baseSpeed + " + " + player.addedSpeedPoints + " - weight of equipment: " + player.weightOfEquipment + ")";

                string yourArmorStatus = "Armor: ";
                if (player.equippedArmor != null)
                    yourArmorStatus = "Armor: " + player.equippedArmor.name + " - defense: " + player.equippedArmor.defence;

                string yourWeaponStatus = "Right Hand - Weapon: ";
                if (player.equippedWeapon != null)
                    yourWeaponStatus = "Right Hand - Weapon: " + player.equippedWeapon.name + " - attack: " + player.equippedWeapon.attack;

                string yourSecondHandStatus;
                if (player.equippedSecondaryWeapon != null)
                {
                    yourSecondHandStatus = "Left Hand - Weapon: " + player.equippedSecondaryWeapon.name + " - attack: " + player.equippedSecondaryWeapon.attack;
                }
                else
                {
                    yourSecondHandStatus = "Left Hand - Shield: ";
                    if (player.equippedShield != null)
                        yourSecondHandStatus = "Left Hand: " + player.equippedShield.name + " - defense: " + player.equippedShield.defence;
                }

                menuOptions = new string[] { "Name: " + player.name, "Type: " + Game.CapitalizeFirstLetter(player.type), "Gender: " + Game.CapitalizeFirstLetter(player.gender),
                                             "Health: " + player.maxHealth + " (" + player.baseHealth + " + " + player.addedHealthPoints + ")",
                                             "Strength: " + player.strength + " (" + player.baseStrength + " + " + player.addedStrengthPoints + ")",
                                             "Stamina: " + player.stamina + " (" + player.baseStamina + " + part of your strength: " + (player.strength/3) + " + " + player.addedStaminaPoints + ")",
                                             yourSpeedStatus,
                                             yourArmorStatus, yourWeaponStatus, yourSecondHandStatus,
                                             "Battle cry: \"" + player.battlecry + "\"",
                                             "Done. This is me." };

                Game.Wrt("Points left to add wherever you like: ");
                Game.WrtL("yl", "", player.extraPoints, true);
                Game.WrtL("Additionally you must choose at least a weapon, armor and gender.");
                Game.WrtL("When you add points to a skill, the other skills updates as well.\nThis is because some skills affect other skills.\nTry it out.\n");
                Game.Wrt("Branzen left to use on gear: ");
                Game.Wrt("yl", "", Game.Branzen, true);
                Game.WrtL("  - Currently dual wielding: " + player.isDualWielding);
                Game.WrtL("\nBut remember, it's a good idea to keep some Branzen for later to be able to recruit extra warriors and/or get some\nextra gear before entering the cave.\n\n");
                MenuPrinter(menuOptions, markedOption, "   ");

                var keyPressed = Console.ReadKey();

                // I find it better to let the user rotate the list instead of stopping at the top and bottom.
                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption++;
                    if (markedOption >= menuOptions.Length)
                        markedOption = 0;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption--;
                    if (markedOption < 0)
                        markedOption = menuOptions.Length - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    switch (markedOption)
                    {
                        case 0:
                            // name
                            Console.CursorVisible = true;
                            bool tryAgain;
                            string newName;

                            do
                            {
                                Console.Clear();
                                tryAgain = false;
                                Game.WrtL("Enter a new name, then press Enter.");
                                Game.WrtL("You can have a randomized name proposed to you if you just write \"r\" and enter.");
                                Game.WrtL("\nCurrent name: " + player.name);
                                Game.Wrt("\nNew name: ");

                                newName = Console.ReadLine();

                                if (newName == "r")
                                {
                                    do
                                    {
                                        newName = Game.RandomizeName(player.gender);
                                        
                                        if (newName == " writeownagain ")
                                        {
                                            tryAgain = true;
                                        }
                                    }
                                    while (newName == "r");
                                    if (!tryAgain)
                                    {
                                        player.name = newName;
                                        Game.WrtL("\nGreat! New name!");
                                        Game.AwaitKeyEnter();
                                        break;
                                    }
                                }
                                if (!tryAgain)
                                {
                                    Console.Clear();

                                    newName = Game.ReasonableName(newName);
                                    newName = Game.CapitalizeFirstLetter(newName);

                                    Game.WrtL(newName);
                                    Game.WrtL("\nIs your new name satisfactory?\n");
                                    Game.WrtL("dy", "", "Enter = confirm\nSpace = go back to old name\nLeft Arrow key = try again.", true);

                                    keyPressed = Console.ReadKey();

                                    if (keyPressed.Key == ConsoleKey.Enter)
                                    {
                                        player.name = newName;
                                        Game.WrtL("\nGreat! New name!");
                                        Game.AwaitKeyEnter();
                                    }
                                    else if (keyPressed.Key == ConsoleKey.Spacebar)
                                    {
                                        Game.WrtL("\nNo new name then.");
                                        Game.WrtL("Your name is still " + player.name);
                                        Game.AwaitKeyEnter();
                                    }
                                    else if (keyPressed.Key == ConsoleKey.LeftArrow)
                                    {
                                        tryAgain = true;
                                        Game.WrtL("\nDidn't get it right huh? Let's try that again then.");
                                        Game.AwaitKeyEnter();
                                    }
                                }
                            }
                            while (tryAgain);
                            break;
                        case 1:
                            // type
                            UpdateWarriorType(player);
                            Hero.UpdateWarriorStatsBeginning(player);
                            Console.Clear();
                            Hero.PresentPlayerBaseSkills(player);
                            Game.AwaitKeyEnter();
                            break;
                        case 2:
                            // gender
                            int genderChoice = GenderChoiceMenu();
                            Hero.setGender(player, genderChoice);
                            break;
                        case 3:
                            // health
                            UpdateWarriorStat(player, "health");
                            Hero.UpdateWarriorStatsBeginning(player);
                            break;
                        case 4:
                            // strength
                            UpdateWarriorStat(player, "strength");
                            Hero.UpdateWarriorStatsBeginning(player);
                            break;
                        case 5:
                            // stamina
                            UpdateWarriorStat(player, "stamina");
                            Hero.UpdateWarriorStatsBeginning(player);
                            break;
                        case 6:
                            // speed
                            UpdateWarriorStat(player, "speed");
                            Hero.UpdateWarriorStatsBeginning(player);
                            break;
                        case 7:
                            // armor
                            ArmorBeginningChoiceMenu(player, "armor");
                            Hero.UpdateWarriorStatsBeginning(player);
                            break;
                        case 8:
                            // weapon
                            WeaponBeginningChoiceMenu(player, "first");
                            Hero.UpdateWarriorStatsBeginning(player);
                            break;
                        case 9:
                            // shield/second weapon
                            SecondHandBeginningChoiceMenu(player);
                            Hero.UpdateWarriorStatsBeginning(player);
                            break;
                        case 10:
                            // battlecry
                            CreateYourBattleCryMenu(player);
                            break;
                        case 11:
                            // finish
                            Console.Clear();
                            Console.CursorVisible = true;

                            bool allMandatoryStuffDone = false;
                            bool allStatsWellFilled = false;

                            if (player.gender != null && player.equippedWeapon != null && player.equippedArmor != null)
                                allMandatoryStuffDone = true;

                            if (player.extraPoints == 0 && player.battlecry != null && (player.equippedSecondaryWeapon != null || player.equippedShield != null))
                                allStatsWellFilled = true;

                            // Thinking about adding more options here. Being able to play with less things to play harder could be an option.
                            if (allMandatoryStuffDone)
                            {
                                if (allStatsWellFilled)
                                {
                                    Console.Clear();
                                    Console.CursorVisible = true;

                                    Game.WrtL("Exciting!");
                                    Game.WrtL("But...\n");

                                    Game.AwaitKeyEnter();

                                    if (SecondGuesses(player, "full"))
                                    {
                                        Console.Clear();
                                        Console.CursorVisible = true;

                                        Game.WrtL("Great!");
                                        Game.WrtL("The fun, terrifying, dark cave crawling among monsters can begin!");

                                        return;
                                    }
                                    else
                                    {
                                        Game.AwaitKeyEnter();
                                    }
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.CursorVisible = true;

                                    Game.WrtL("All right.");
                                    Game.WrtL("Just a few pointers!\n");

                                    Game.AwaitKeyEnter();

                                    if (SecondGuesses(player, "semi"))
                                    {
                                        Console.Clear();
                                        Console.CursorVisible = true;

                                        Game.WrtL("A daredevil, I say!");
                                        Game.WrtL("The fun, terrifying, dark cave crawling among monsters can begin!");

                                        return;
                                    }
                                    else
                                    {
                                        Game.AwaitKeyEnter();
                                    }
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.CursorVisible = true;

                                Game.WrtL("Ah. Well... You're not quite done yet.");

                                Thread.Sleep(1000);
                                Game.FlushKeyboard();

                                Game.WrtL("\nYou've missed some crucial things.\nYou need to have at least a weapon equipped in your right hand, some body armor, and a gender.");
                                Game.WrtL("Check through everything again.");

                                Thread.Sleep(1000);
                                Game.FlushKeyboard();

                                Game.AwaitKeyEnter();
                            }
                            break;
                    }
                }
            }
        }
        // Method for giving the player info on what they might have forgotten in the making of their character and the choice to revise it or go forth with it.
        public static bool SecondGuesses(Hero player, string statsFilled)
        {
            if (statsFilled == "full")
            {
                Game.WrtL("Are you now sure about this warrior? Is everything as you would like?\n");
                PresentFinalizedPlayer(player);
                Game.WrtL("\nOnce you press enter here, there's no turning back,\nthis is you until you die.\n");
                Game.WrtL("dy", "", "Enter to start playing.\nSpacebar to take another look at your skills and equipment.\n", true);

                var keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.Enter)
                {
                    return true;
                }
                else if (keyPressed.Key == ConsoleKey.Spacebar)
                {
                    Game.WrtL("\nSure thing, let's have another look...");
                }
                return false;
            }
            else
            {
                Game.WrtL("You have given the information necessary to continue, but not all the information.");
                Game.WrtL("This may mean that you forgot to equip a shield or secondary weapon, haven't used all your extra skill points,\nperhaps just haven't chosen a battlecry, or several of these.");
                Game.WrtL("Take a look.\n");
                PresentFinalizedPlayer(player);
                Game.WrtL("\nIt's fully possible to play this way, but it could mean that you start more or less uphill than necessary.");
                Game.WrtL("\nIf this is your choice, ");
                Game.WrtL("dy", "", "press Enter to start playing,\notherwise, press Spacebar to look over your character again.", true);

                var keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.Enter)
                {
                    return true;
                }
                else if (keyPressed.Key == ConsoleKey.Spacebar)
                {
                    Game.WrtL("\nSure thing, let's have another look...");
                }
                return false;
            }
        }
        // Method for showing all data for the final character the player has built (or loaded).
        public static void PresentFinalizedPlayer(Hero player)
        {
            Game.Wrt("w", "", player.name, true);
            Game.WrtL(" the " + player.type);
            Game.Wrt("Health: ");
            Game.WrtL("yl", "", player.healthpoints, true);
            Game.Wrt("Strength: ");
            Game.WrtL("yl", "", player.strength, true);
            Game.Wrt("Stamina: ");
            Game.WrtL("yl", "", player.stamina, true);
            Game.Wrt("Speed: ");
            Game.WrtL("yl", "", player.speed, true);
            Game.Wrt("Armor: ");
            Game.Wrt("w", "", player.equippedArmor.name, true);
            Game.Wrt(" with defence ");
            Game.Wrt("yl", "", player.equippedArmor.defence, true);

            if (player.equippedShield != null)
            {
                Game.Wrt(" and ");
                Game.Wrt("w", "", player.equippedShield.name, true);
                Game.Wrt(" with defence ");
                Game.Wrt("yl", "", player.equippedShield.defence, true);
            }
            Game.Wrt("\n");
            Game.Wrt("Attack: ");
            Game.WrtL("yl", "", player.attackDice, true);

            if (player.isDualWielding)
            {
                Game.Wrt("Weapons: ");
                Game.Wrt("yl", "", player.equippedWeapon.name, true);
                Game.Wrt(" and ");
                Game.Wrt("yl", "", player.equippedSecondaryWeapon.name, true);
                if (player.equippedWeapon.defence > 0 || player.equippedSecondaryWeapon.defence > 0)
                {
                    Game.Wrt(" giving ");
                    Game.Wrt("yl", "", player.equippedWeapon.defence + player.equippedSecondaryWeapon.defence, true);
                    Game.WrtL(" extra defence.");
                }
                else
                    Game.Wrt("\n");
            }
            else
            {
                Game.Wrt("Weapon: ");
                Game.WrtL("yl", "", player.equippedWeapon.name, true);
                if (player.equippedWeapon.defence > 0)
                {
                    Game.Wrt(" giving ");
                    Game.Wrt("yl", "", player.equippedWeapon.defence, true);
                    Game.WrtL(" extra defence.");
                }
                else
                    Game.Wrt("\n");
            }
            Game.Wrt("Battlecry: ");
            Game.WrtL("w", "", player.battlecry, true);
        }
        // Method for choosing gender for the character.
        public static int GenderChoiceMenu()
        {
            string[] menuOptions = new string[] { "Male", "Female", "Non binary" };
            int markedOption = 0;

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                Game.WrtL("What gender are you anyway?");

                MenuPrinter(menuOptions, markedOption, "\t");

                var keyPressed = Console.ReadKey();

                // I find it better to let the user rotate the list instead of stopping at the top and bottom.
                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption++;
                    if (markedOption >= menuOptions.Length)
                        markedOption = 0;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption--;
                    if (markedOption < 0)
                        markedOption = menuOptions.Length - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    return markedOption;
                }
            }
        }
        // Method for the first time the player chooses armor for their character. Withdraws money from starting budget.
        public static void ArmorBeginningChoiceMenu(Hero player, string armorType)
        {
            List<Gear> armorAvailable = Gear.allGear.Where(armor => armor.types[0] == armorType && armor.wieldLvl == 1).ToList();
            List<string> menuOptions = new List<string>();

            for (int i = 0; i < armorAvailable.Count; i++)
            {
                var armor = armorAvailable[i];
                menuOptions.Add(armor.name + " with defense " + armor.defence + " - Weight: " + armor.weight + " -- " + armor.cost + " Br");
            }

            int markedOption = 0;

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                if (armorType == "armor")
                {
                    Game.Wrt("w", "", "Choose a type of armor.", true);
                    Game.WrtL(" Equipment weight will subtract from your speed.");
                    Game.WrtL("Both armor and speed are important in battle; both affects your defence, but speed affects your attack as well.");
                    Game.WrtL("Consider your warrior type and its base skills when making your choice.");

                    if (player.equippedArmor != null)
                    {
                        Game.WrtL("You have a " + player.equippedArmor.name + " equipped. Cost: " + player.equippedArmor.cost);
                        Game.Wrt("\nAvailable Branzen: " + Game.Branzen + " + " + player.equippedArmor.cost + " = ");
                        Game.WrtL("yl", "", Game.Branzen + player.equippedArmor.cost + "\n", true);
                    }
                    else
                    {
                        Game.Wrt("\nAvailable Branzen: ");
                        Game.WrtL("yl", "", Game.Branzen + "\n", true);
                    }
                        
                }
                else
                {
                    Game.Wrt("w", "", "Choose a type of shield.", true);
                    Game.WrtL(" Equipment weight will subtract from your speed.");
                    Game.WrtL("Both armor and speed are important in battle; both affects your defence, but speed affects your attack as well.");
                    Game.WrtL("Consider your warrior type and its base skills when making your choice.");

                    if (player.equippedShield != null)
                    {
                        Game.WrtL("You have a " + player.equippedShield.name + " equipped. Cost: " + player.equippedShield.cost);
                        Game.Wrt("\nAvailable Branzen: " + Game.Branzen + " + " + player.equippedShield.cost + " = ");
                        Game.WrtL("yl", "", Game.Branzen + player.equippedShield.cost + "\n", true);
                    }
                    else
                    {
                        Game.Wrt("\nAvailable Branzen: ");
                        Game.WrtL("yl", "", Game.Branzen + "\n", true);
                    }
                }

                MenuPrinter(menuOptions, markedOption, "   ");

                Game.WrtL("\n" + armorAvailable[markedOption].description);

                var keyPressed = Console.ReadKey();

                // I find it better to let the user rotate the list instead of stopping at the top and bottom.
                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption++;
                    if (markedOption >= menuOptions.Count)
                        markedOption = 0;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption--;
                    if (markedOption < 0)
                        markedOption = menuOptions.Count - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Spacebar)
                {
                    break;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    int currentBranzen;
                    int oldArmorCost = 0;

                    if (armorType == "armor")
                    {
                        currentBranzen = Game.Branzen;

                        if (player.equippedArmor != null)
                        {
                            oldArmorCost = player.equippedArmor.cost;
                            currentBranzen += oldArmorCost;
                        }

                        if (CanYouAffordThis(currentBranzen, armorAvailable[markedOption].cost, "Branzen", "equip this armor"))
                        {
                            Game.Branzen += oldArmorCost;
                            Game.Branzen -= armorAvailable[markedOption].cost;
                            player.equippedArmor = armorAvailable[markedOption];
                            return;
                        }
                    }   
                    else
                    {
                        currentBranzen = Game.Branzen;

                        if (player.equippedShield != null)
                        {
                            oldArmorCost = player.equippedShield.cost;
                            currentBranzen += oldArmorCost;
                        }

                        if (CanYouAffordThis(currentBranzen, armorAvailable[markedOption].cost, "Branzen", "equip this shield"))
                        {
                            Game.Branzen += oldArmorCost;
                            Game.Branzen -= armorAvailable[markedOption].cost;
                            player.equippedShield = armorAvailable[markedOption];
                            return;
                        }
                    }
                }
            }
        }
        // Method for the first time the player chooses main weapon for their character. Withdraws money from starting budget.
        public static void WeaponBeginningChoiceMenu(Hero player, string whichHand)
        {
            var wTypes = Gear.weaponClasses;
            string[] menuOptions1;

            // "sword", "knife", "axe", "blunt", "bow", "crossbow", "spear", "sickle", "whip"
            // I put them all in separate strings to shorten the lists.
            string Sword = Game.CapitalizeFirstLetter(wTypes[0]);
            string Knife = Game.CapitalizeFirstLetter(wTypes[1]);
            string Axe = Game.CapitalizeFirstLetter(wTypes[2]);
            string Blunt = Game.CapitalizeFirstLetter(wTypes[3] + " Weapon");
            string Bow = Game.CapitalizeFirstLetter(wTypes[4]);
            string Crossbow = Game.CapitalizeFirstLetter(wTypes[5]);
            string Spear = Game.CapitalizeFirstLetter(wTypes[6]);
            string Sickle = Game.CapitalizeFirstLetter(wTypes[7]);
            string Whip = Game.CapitalizeFirstLetter(wTypes[8]);

            if (whichHand == "first")
                menuOptions1 = new string[] { Sword + "\t", Knife + "\t", Axe + "\t", Blunt, Bow + "\t", Crossbow + "\t", Spear + "\t", Sickle + "\t", Whip + "\t", "Unequip" };
            else
                menuOptions1 = new string[] { Sword + "\t", Knife + "\t", Axe + "\t", Blunt, Bow + "\t", Crossbow + "\t", Spear + "\t", Sickle + "\t", Whip + "\t" };

            int markedOption1 = 0;

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                Game.WrtL("First, choose weapon type.");

                if (player.type == "duelist")
                {
                    if (player.choiceOfWeapon[0] == "any weapon type")
                        Game.WrtL("Your warrior types' preferred weapon types are right now " + player.choiceOfWeapon[0] + " and " + player.choiceOfWeapon[1] + " of your choice,");
                    else
                        Game.WrtL("Your warrior types' preferred weapon types are right now " + player.choiceOfWeapon[0] + " and " + player.choiceOfWeapon[1] + " by your choice,");

                    Game.WrtL("but because you've chosen the duelist it will adapt to any new choice as long as you're building your character.");

                    if (whichHand == "second")
                    {
                        Game.WrtL("Remember, only the weapon in your first hand defines your preferences.");
                        Game.WrtL("Dual wielding reduces attack initially, it takes some time to get bonus attack from it.");
                    }
                }
                else
                {
                    Game.WrtL("Your warrior types' preferred weapon types are " + player.choiceOfWeapon[0] + " and " + player.choiceOfWeapon[1] + ".");

                    if (whichHand == "second")
                    {
                        Game.WrtL("Remember, dual wielding reduces attack initially, it takes some time to get bonus attack from it.");
                    }
                }

                Game.WrtL("Using a weapon with a preferred weapon type give +1 attack.\n");

                MenuPrinter(menuOptions1, markedOption1, "  ");

                var keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption1++;
                    if (markedOption1 >= menuOptions1.Length)
                        markedOption1 = 0;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption1--;
                    if (markedOption1 < 0)
                        markedOption1 = menuOptions1.Length - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Spacebar)
                {
                    return;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    if (markedOption1 == 9 && whichHand == "first")
                    {
                        if (player.equippedWeapon != null)
                        {
                            Game.Branzen += player.equippedWeapon.cost;
                            player.equippedWeapon = null;

                            if (player.isUsingDoubleHandedWeapon)
                                // No need to check the secondary weapon as these menus will most likely only be used in the beginning, and a beginner warrior can't carry two weapons if one of them is double handed.
                                player.isUsingDoubleHandedWeapon = false;

                            if (player.isDualWielding)
                                player.isDualWielding = false;

                            return;
                        }
                        else
                        {
                            Console.Clear();
                            Console.CursorVisible = true;

                            Game.WrtL("You have no equipped weapon in this hand yet.");

                            Game.AwaitKeyEnter();
                        }
                        break;
                    }

                    List<Gear> weaponsAvailable;

                    if (whichHand == "first" && player.equippedSecondaryWeapon == null)
                        // Adds all weapons that are certain type and can be wielded at level 1.
                        weaponsAvailable = Gear.allGear.Where(wpn => wpn.types[1] == Gear.weaponClasses[markedOption1] && wpn.wieldLvl == 1).ToList();
                    else
                        // Does the same plus makes sure the weaopns can be DUAL wielded at level 1.
                        weaponsAvailable = Gear.allGear.Where(wpn => wpn.types[1] == Gear.weaponClasses[markedOption1] && wpn.wieldLvl == 1 && wpn.dualWieldLvl == 1 && wpn.isDoubleHandheld == false).ToList();

                    List<string> menuOptions2 = new List<string>();
                    string weaponClassInThislist = weaponsAvailable[0].weaponClass;

                    for (int i = 0; i < weaponsAvailable.Count; i++)
                    {
                        var weapon = weaponsAvailable[i];
                        if (weapon.isDoubleHandheld)
                            menuOptions2.Add(weapon.name + " - Size: " + weapon.size + ", two-handed" + " - Attack: " + weapon.attack + " - Critical: " + weapon.criticalChance + " - Defence: " + weapon.defenceDice + " -- " + weapon.cost + " Br");
                        else
                            menuOptions2.Add(weapon.name + " - Size: " + weapon.size + " - Attack: " + weapon.attack + " - Critical: " + weapon.criticalChance + " - Defence: " + weapon.defenceDice + " -- " + weapon.cost + " Br");
                    }

                    int markedOption2 = 0;

                    while (true)
                    {
                        Console.Clear();
                        Console.CursorVisible = false;

                        if (weaponsAvailable.Count <= 0)
                        {
                            Console.Clear();
                            Console.CursorVisible = true;

                            Game.WrtL("There are no weapons in this category that you can equip right now.");
                            Game.WrtL("It might be because this category requires both hands, and your other hand is busy already, or that nothing here is\npossible to equip as dual wielded at your level.");

                            Game.AwaitKeyEnter();
                            break;
                        }

                        if (whichHand == "first")
                            Game.Wrt("w", "", "Choose a weapon. ", true);
                        else
                            Game.Wrt("w", "", "Choose a weapon for your second hand. ", true);

                        if (player.type == "duelist")
                        {
                            Game.Wrt("\nYour warrior types' preferred weapon types are right now " + player.choiceOfWeapon[0] + " and " + player.choiceOfWeapon[1] + ",\n");
                            Game.WrtL("but because you've chosen the duelist it will adapt to any new choice as long as you're building your character.");
                            if (whichHand == "second")
                            {
                                Game.WrtL("Remember, only the weapon in your first hand defines your preferences.");
                            }
                        }
                        else
                            Game.WrtL("Your warrior types' preferred weapon types are " + player.choiceOfWeapon[0] + " and " + player.choiceOfWeapon[1] + ".");

                        Game.WrtL("Using a weapon with a preferred weapon type give +1 attack.");

                        if (whichHand == "first" && player.equippedWeapon != null)
                        {
                            Game.Wrt("You have a ");
                            Game.Wrt("w", "", player.equippedWeapon.name, true);
                            Game.WrtL(" equipped. Cost: " + player.equippedWeapon.cost);
                            Game.Wrt("\nAvailable Branzen: " + Game.Branzen + " + " + player.equippedWeapon.cost + " = ");
                            Game.WrtL("yl", "", Game.Branzen + player.equippedWeapon.cost + "\n", true);
                        }
                        else if (whichHand == "second" && player.equippedSecondaryWeapon != null)
                        {
                            Game.WrtL("You have a " + player.equippedSecondaryWeapon.name + " equipped. Cost: " + player.equippedSecondaryWeapon.cost);
                            Game.Wrt("\nAvailable Branzen: " + Game.Branzen + " + " + player.equippedSecondaryWeapon.cost + " = ");
                            Game.WrtL("yl", "", Game.Branzen + player.equippedSecondaryWeapon.cost + "\n", true);
                        }
                        else
                        {
                            Game.Wrt("\nAvailable Branzen: ");
                            Game.WrtL("yl", "", Game.Branzen + "\n", true);
                        }

                        Game.WrtL("Weapon type: " + Game.CapitalizeFirstLetter(weaponClassInThislist) + "\n");

                        MenuPrinter(menuOptions2, markedOption2, "   ");

                        Game.WrtL("\n" + weaponsAvailable[markedOption2].description);

                        var keyPressedMenu2 = Console.ReadKey();

                        // I find it better to let the user rotate the list instead of stopping at the top and bottom.
                        if (keyPressedMenu2.Key == ConsoleKey.DownArrow)
                        {
                            markedOption2++;
                            if (markedOption2 >= menuOptions2.Count)
                                markedOption2 = 0;
                        }
                        else if (keyPressedMenu2.Key == ConsoleKey.UpArrow)
                        {
                            markedOption2--;
                            if (markedOption2 < 0)
                                markedOption2 = menuOptions2.Count - 1;
                        }
                        else if (keyPressedMenu2.Key == ConsoleKey.Spacebar)
                        {
                            break;
                        }
                        else if (keyPressedMenu2.Key == ConsoleKey.Enter)
                        {
                            bool newWpn = false;
                            int oldWeaponCost = 0;
                            int currentBranzen;
                            Gear chosenWeapon = weaponsAvailable[markedOption2];

                            if (whichHand == "second")
                            {
                                currentBranzen = Game.Branzen;

                                if (player.equippedSecondaryWeapon != null)
                                {
                                    oldWeaponCost = player.equippedSecondaryWeapon.cost;
                                    currentBranzen += oldWeaponCost;
                                }

                                if (CanYouAffordThis(currentBranzen, chosenWeapon.cost, "Branzen", "equip this weapon"))
                                {
                                    Game.Branzen += oldWeaponCost;
                                    Game.Branzen -= chosenWeapon.cost;

                                    player.equippedSecondaryWeapon = chosenWeapon;

                                    if (player.equippedWeapon != null)
                                        player.isDualWielding = true;

                                    newWpn = true;
                                }
                            }
                            else
                            {
                                currentBranzen = Game.Branzen;

                                if (player.equippedWeapon != null)
                                {
                                    oldWeaponCost = player.equippedWeapon.cost;
                                    currentBranzen += oldWeaponCost;
                                }

                                if (CanYouAffordThis(currentBranzen, chosenWeapon.cost, "Branzen", "equip this weapon"))
                                {
                                    if (chosenWeapon.isDoubleHandheld)
                                        player.isUsingDoubleHandedWeapon = true;
                                    else
                                    {
                                        if (player.isUsingDoubleHandedWeapon)
                                            player.isUsingDoubleHandedWeapon = false;
                                    }

                                    Game.Branzen += oldWeaponCost;
                                    Game.Branzen -= chosenWeapon.cost;
                                    player.equippedWeapon = chosenWeapon;

                                    if (player.equippedSecondaryWeapon != null)
                                        player.isDualWielding = true;

                                    if (player.type == "duelist" && whichHand == "first")
                                    {
                                        // weapon type
                                        player.choiceOfWeapon[0] = player.equippedWeapon.types[1];
                                        // weapon size
                                        player.choiceOfWeapon[1] = player.equippedWeapon.types[2];
                                    }
                                        Game.WrtL("Your warrior types' preferred weapon types are " + player.choiceOfWeapon[0] + "\nand " + player.choiceOfWeapon[1] + ".");
                                    newWpn = true;
                                }
                            }
                            if (newWpn)
                            {
                                return;
                            }
                        }
                    }
                }
            }
        }
        // Method for the first time the player chooses second hand weapon for their character. Withdraws money from starting budget.
        public static void SecondHandBeginningChoiceMenu(Hero player)
        {
            if (player.isUsingDoubleHandedWeapon)
            {
                Console.Clear();
                Console.CursorVisible = true;

                Game.WrtL("You're carrying a weapon that at your level requires the use of both hands.\nYou cannot equip anything to your second hand.");
                Game.WrtL("In order to do so you must first choose a one hand weapon instead.");

                Game.AwaitKeyEnter();
                return;
            }

            string[] menuOptions1 = new string[] { "Shield", "Second Weapon", "Unequip" };

            int markedOption1 = 0;

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                Game.WrtL("Choose wether to carry a shield or another weapon in your second hand, or to unequip whatever you're holding now.");
                Game.WrtL("A second weapon adds to your attack, but without a shield your defence might be a bit weak.´\n");

                MenuPrinter(menuOptions1, markedOption1, "\t");

                var keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption1++;
                    if (markedOption1 >= menuOptions1.Length)
                        markedOption1 = 0;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption1--;
                    if (markedOption1 < 0)
                        markedOption1 = menuOptions1.Length - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Spacebar)
                {
                    return;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    switch (markedOption1)
                    {
                        case 0:
                            ArmorBeginningChoiceMenu(player, "shield");
                            return;
                        case 1:
                            WeaponBeginningChoiceMenu(player, "second");
                            return;
                        case 2:
                            if (player.equippedSecondaryWeapon != null)
                            {
                                Game.Branzen += player.equippedSecondaryWeapon.cost;
                                player.equippedSecondaryWeapon = null;
                                player.isDualWielding = false;

                                Console.Clear();
                                Console.CursorVisible = true;

                                Game.WrtL("You unequipped your secondary weapon.");

                                Game.AwaitKeyEnter();
                            }
                            else if (player.equippedShield != null)
                            {
                                Game.Branzen += player.equippedShield.cost;
                                player.equippedShield = null;

                                Console.Clear();
                                Console.CursorVisible = true;

                                Game.WrtL("You unequipped your shield.");

                                Game.AwaitKeyEnter();
                            }
                            else
                            {
                                Console.Clear();
                                Console.CursorVisible = true;

                                Game.WrtL("You have nothing equipped in this hand.");

                                Game.AwaitKeyEnter();
                            }
                            break;
                    }
                }
            }
        }
        // Method for checking wether player has enough starting money to get chosen item.
        // Is only used in the beginning yet, but I think I've planned to be able to use this method for a wide range of things, including to check if something requires more experience to wield.
        public static bool CanYouAffordThis(int source, int amountToSubtract, string wordOfCurrency, string wordsOfAcquiring)
        {
            if (source - amountToSubtract >= 0)
            {
                return true;
            }
            else
            {
                Console.Clear();
                Console.CursorVisible = true;

                Game.WrtL("You don't have enough " + wordOfCurrency + " to " + wordsOfAcquiring + ".");
                Game.AwaitKeyEnter();
                return false;
            }
        }
        // Method for when choosing to add points to certain stats.
        public static void UpdateWarriorStat(Hero player, string statInQuestion)
        {
            int oldStat = 0;
            int newStat = 0;
            if (statInQuestion == "health")
            {
                oldStat = player.addedHealthPoints;
                newStat = StatChangerLoop("Health", player.maxHealth, player.baseHealth, player.extraPoints, player.addedHealthPoints);
                player.addedHealthPoints = newStat;
            }
            else if (statInQuestion == "strength")
            {
                oldStat = player.addedStrengthPoints;
                newStat = StatChangerLoop("Strength", player.strength, player.baseStrength, player.extraPoints, player.addedStrengthPoints);
                player.addedStrengthPoints = newStat;
            }
            else if (statInQuestion == "stamina")
            {
                oldStat = player.addedStaminaPoints;
                newStat = StatChangerLoop("Stamina", player.maxStamina, player.baseStamina, player.extraPoints, player.addedStaminaPoints);
                player.addedStaminaPoints = newStat;
            }
            else if (statInQuestion == "speed")
            {
                oldStat = player.addedSpeedPoints;
                newStat = StatChangerLoop("Speed", player.speed, player.baseSpeed, player.extraPoints, player.addedSpeedPoints);
                player.addedSpeedPoints = newStat;
            }
            player.extraPoints += oldStat;
            player.extraPoints -= newStat;
        }
        // Method for entering amount of points on certain stat, both to increase or decrease the added points, and warning if it's too much.
        public static int StatChangerLoop(string stat, int maxStat, int baseStat, int extraPoints, int newPoints)
        {
            Console.CursorVisible = true;
            int newStat;
            string newStatString = "empty";
            int pointsThatCount = extraPoints + newPoints;

            do
            {
                Console.Clear();

                Game.Wrt(stat + " - base skill points: ");
                Game.Wrt("yl", "", baseStat, true);
                if (newPoints > 0)
                {
                    Game.Wrt(" + added points: ");
                    Game.Wrt("w", "", newPoints, true);
                }

                Game.Wrt("\n\nPoints left to use: ");
                Game.WrtL("w", "", extraPoints, true);

                if (newPoints > 0)
                {
                    Game.Wrt("Amount of points you can put on this skill: ");
                    Game.WrtL("w", "", pointsThatCount, true);

                    Game.Wrt("\nYou can decrease the amount you already put here by entering a lower number than ");
                    Game.Wrt("w", "", newPoints, true);
                    Game.WrtL(", or enter zero\nto remove all extra points.");
                }

                Game.WrtL("\nIf you don't want to change anything, press only enter.");
                Game.WrtL("Otherwise, please enter a positive number.\n");

                if (newStatString == "wrong")
                {
                    Game.WrtL("It has to be a number\n");
                }
                else if (newStatString == "too low or high")
                {
                    Game.WrtL("It has to be " + pointsThatCount + " or lower, and not less than 0.\n");
                }

                newStatString = Console.ReadLine();

                if (newStatString == "")
                {
                    newStat = newPoints;
                }
                else
                {
                    try
                    {
                        newStat = Convert.ToInt32(newStatString);
                    }
                    catch
                    {
                        newStat = -1;
                        newStatString = "wrong";
                    }

                    if (newStatString != "wrong" && (newStat > pointsThatCount || newStat < 0))
                    {
                        newStat = -1;
                        newStatString = "too low or high";
                    }
                }
            }
            while (newStat == -1);

            return newStat;
        }
        // Method for the menu for choosing or creating your own battle cry.
        public static void CreateYourBattleCryMenu(Hero player)
        {
            string[] menuOptions = new string[] { "\"" + Game.warriorBattlecries[player.warriorTypeIndex,0] + "\"",
                    "\"" + Game.warriorBattlecries[player.warriorTypeIndex,1] + "\"",
                    "\"" + Game.warriorBattlecries[player.warriorTypeIndex,2] + "\"",
                    "\"" + Game.warriorBattlecries[player.warriorTypeIndex,3] + "\"",
                    "\"" + Game.warriorBattlecries[player.warriorTypeIndex,4] + "\"",
                    "\"" + Game.warriorBattlecries[player.warriorTypeIndex,5] + "\"",
                    "\"" + Game.warriorBattlecries[player.warriorTypeIndex,6] + "\"",
                    "\"" + Game.warriorBattlecries[player.warriorTypeIndex,7] + "\"",
                    "\"" + Game.warriorBattlecries[player.warriorTypeIndex,8] + "\"",
                    "Write my own"};

            int markedOption = 0;

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                string oldCry = player.battlecry;

                Game.WrtL("Here you can choose a battle cry, something that you say (or yell) in battle sometimes.");
                Game.WrtL("\nYour chosen warrior type's description:\n\"" + player.description + "\"");

                if (player.battlecry != "")
                {
                    Game.WrtL("\nAnd here's your old battle cry:\n\"" + player.battlecry + "\"");
                }
                Game.WrtL("\nYou can choose from your warrior type's classical warrior cries, or write your own.\n");

                MenuPrinter(menuOptions, markedOption, "   ");

                var keyPressed = Console.ReadKey();

                // I find it better to let the user rotate the list instead of stopping at the top and bottom.
                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption++;
                    if (markedOption >= menuOptions.Length)
                        markedOption = 0;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption--;
                    if (markedOption < 0)
                        markedOption = menuOptions.Length - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    if (markedOption == 9)
                    {
                        // Make your own.
                        bool tryAgain;

                        do
                        {
                            Console.Clear();
                            Console.CursorVisible = true;

                            tryAgain = false;

                            Game.WrtL("Your chosen warrior type's description:\n\"" + player.description + "\"");

                            if (player.battlecry != "")
                            {
                                Game.WrtL("\nAnd here's your old battle cry:\n\"" + player.battlecry + "\"");
                            }

                            Game.WrtL("\nWrite your own battle cry:\n");

                            player.battlecry = Console.ReadLine();

                            Console.Clear();

                            Game.WrtL("\"" + player.battlecry + "\"");
                            Game.WrtL("\nIs your battle cry satisfactory?");
                            Game.WrtL("Enter = confirm, Space = go back to old battle cry, Left Arrow key = try again.");

                            keyPressed = Console.ReadKey();

                            if (keyPressed.Key == ConsoleKey.Enter)
                            {
                                Game.WrtL("\nGood! New battle cry!");

                                Game.AwaitKeyEnter();
                            }
                            else if (keyPressed.Key == ConsoleKey.Spacebar)
                            {
                                player.battlecry = oldCry;
                                Game.WrtL("\nNo new battle cry then.");
                                if (player.battlecry != "")
                                    Game.WrtL("Your battle cry is still \"" + player.battlecry + "\"\n");

                                Game.AwaitKeyEnter();
                            }
                            else if (keyPressed.Key == ConsoleKey.LeftArrow)
                            {
                                player.battlecry = oldCry;
                                tryAgain = true;
                                Game.WrtL("\nDidn't get it right huh? Let's try that again then.");

                                Game.AwaitKeyEnter();
                            }
                        }
                        while (tryAgain);
                    }
                    else
                    {
                        Console.Clear();

                        player.battlecry = Game.warriorBattlecries[player.warriorTypeIndex, markedOption];
                        Game.WrtL("Great choice!");

                        Game.AwaitKeyEnter();
                    }
                    return;
                }
            }
        }

        // Method for the battle menu.
        public static int BattleMenu(int i, Monster monster, bool usedDisposableWeapon)
        {
            List<string> menuOptions = new List<string>();
            List<int> originalIndex = new List<int>(); // Using index here won't be a problem, because the next time we're going to use an item, the list will be recounted again.

            int markedOption = 0;

            // string[] menuOptions = new string[] { "Attack", "Rest/Defence", "Use Item", "Check Team" };

            while (true)
            {
                // Look for warrior type specific attack (later), and equipped gear function options.
                int amount = 0;
                int extraOptionCount = 0;

                menuOptions.Clear();

                menuOptions.Add("Attack");
                menuOptions.Add("Rest/Defence");

                for (int k = 0; k < Game.partyGear.Count; k++)
                {
                    //(Game.partyGear[k].types[1] == "disposable weapon" || Game.partyGear[k].types[1] == "arifact") &&
                    //     Game.partyGear[k].isEquippable == false && Game.partyGear[k].isPartyEquippable == false

                    // Go through inventory and add disposable weapons menu options.
                    if (Game.partyGear[k].addsBattleOption && !Game.partyGear[k].isEquippable && !Game.partyGear[k].isPartyEquippable)
                    {
                        Gear item = Game.partyGear[k];
                        int nextItemId = -1;

                        if (k + 1 < Game.partyGear.Count)
                            nextItemId = Game.partyGear[k + 1].Id;

                        if (nextItemId == item.Id)
                        {
                            amount++;
                        }
                        else
                        {
                            amount++;
                            menuOptions.Add(item.battleOptionPrompt + ": " + amount);
                            originalIndex.Add(k);
                            extraOptionCount++;
                            amount = 0;
                        }
                    }
                }
                menuOptions.Add("Use Item");
                menuOptions.Add("Check Team");

                Console.Clear();
                Console.CursorVisible = false;

                Game.Wrt("It's ");
                Game.Wrt("w", "", Game.Party[i].name + "'s", true);
                Game.WrtL(" turn! What shall " + Game.Party[i].pronoun[0] + " do?\n");

                if (Game.Party[i].stamina == 0)
                {
                    // Lets you know.
                    Game.WrtL(Game.CapitalizeFirstLetter(Game.Party[i].pronoun[0]) + " needs rest now!\n");
                }

                MenuPrinter(menuOptions, markedOption, "\t");

                Game.WrtL("");
                Hero.SeeVitalStats(Game.Party[i]);
                Hero.PresentAilments(Game.Party[i]);

                var keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption++;
                    if (markedOption >= menuOptions.Count)
                        markedOption = 0;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption--;
                    if (markedOption < 0)
                        markedOption = menuOptions.Count - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    if (markedOption == 0)
                    {
                        if (Game.Party[i].stamina == 0)
                        {
                            Console.Clear();
                            Game.WrtL("Impossible! " + Game.Party[i].name + " is too tired to attack!");
                            Game.AwaitKeyEnter();
                            // Puts you back in the choosing business.
                            return -1;
                        }
                        else
                            return 0;
                    }
                    else if (markedOption == 1)
                        // "Rest/Defence"
                        return markedOption;
                    else if (extraOptionCount > 0 && markedOption > 1 && markedOption < (2 + extraOptionCount))
                    {
                        if (!usedDisposableWeapon)
                        {
                            Console.Clear();
                            Gear disposableWeapon = Game.partyGear[originalIndex[markedOption - 2]];
                            bool success = Gear.RunBattleMenuOptionMessage(Game.Party[i], monster, disposableWeapon.battleOptionPrompt);
                            
                            if (success)
                                Gear.RunBattleMenuFunction(Game.Party[i], monster, disposableWeapon.specialPower1, disposableWeapon.specialPower1Value);
                            else
                                Game.AwaitKeyEnter();

                            Game.partyGear.Remove(disposableWeapon);

                            return -2;
                        }
                        else
                        {
                            Console.Clear();
                            Game.WrtL("Already used a disposable weapon! there's no time for another!");
                            Game.AwaitKeyEnter();
                            return -1;
                        }
                    }
                    else if (markedOption == (2 + extraOptionCount))
                        // "Use Item"
                        return 2;
                    else if (markedOption == (3 + extraOptionCount))
                    {
                        // Runs it here to let this choice be one to make without loosing your chance to attack or heal.
                        Console.Clear();
                        Game.LookAtParty();
                        Game.AwaitKeyEnter();
                        return -1;
                    }
                }
            }
        }
        // Method for the menu of items possible to use from the Use Item option during battle.
        public static int UseItemMenu(Hero warrior)
        {
            List<Gear> itemsAvailable = Game.partyGear.Where(item =>
                item.types[0] == "item"
                && (item.types[1] == "food"
                || item.types[1] == "potion"
                || item.types[1] == "gadget"
                || item.types[1] == "artifact")
                && item.isEquippable == false
                && item.isPartyEquippable == false
                ).ToList();

            if (itemsAvailable.Count <= 0)
            {
                Console.Clear();

                Game.WrtL("You have no items that can be used at this moment!");

                Game.AwaitKeyEnter();
                return 0;
            }

            List<string> menuOptions = new List<string>();
            List<string> optionDescriptions = new List<string>();
            List<int> optionOriginalIndex = new List<int>();

            string ItemNameAndAmount;
            int amount = 0;

            for (int i = 0; i < itemsAvailable.Count; i++)
            {
                // For this to work as supposed requires that the list of party gear is sorted by type, more easily done by Id-number, every time something is added to that list.
                Gear item = itemsAvailable[i];
                int nextItemId = -1;

                if (i + 1 < itemsAvailable.Count)
                    nextItemId = itemsAvailable[i + 1].Id;

                if (nextItemId == item.Id)
                {
                    amount++;
                }
                else
                {
                    amount++;
                    ItemNameAndAmount = item.name + ": " + amount;

                    menuOptions.Add(ItemNameAndAmount);
                    optionDescriptions.Add(item.description);
                    optionOriginalIndex.Add(i);

                    amount = 0;
                }
            }

            int markedOption = 0;

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                MenuPrinter(menuOptions, markedOption, "  ");

                Game.WrtL("\n\n" + optionDescriptions[markedOption]);
                Game.WrtL("\n" + itemsAvailable[optionOriginalIndex[markedOption]].specialDataDescription);

                var keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption++;
                    if (markedOption >= menuOptions.Count)
                        markedOption = 0;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption--;
                    if (markedOption < 0)
                        markedOption = menuOptions.Count - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Spacebar)
                {
                    return 0;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    Console.CursorVisible = true;

                    Game.FlushKeyboard();
                    Hero thisWarrior = warrior;

                    if (Game.Party.Count > 1)
                    {
                        int partyIndex = ChoosePartyMemberMenu(warrior.currentPartyId-1, "Which team member do you want to use " + itemsAvailable[optionOriginalIndex[markedOption]].name + " on?");
                        if (partyIndex < 0)
                            continue;
                        else
                            thisWarrior = Game.Party[partyIndex];

                        Console.Clear();
                    }

                    Game.WrtL("Are you sure you want to use " + itemsAvailable[optionOriginalIndex[markedOption]].name + " on " + thisWarrior.name + " now?");
                    Game.WrtL("\nEnter to use\nSpacebar to check item list again");

                    keyPressed = Console.ReadKey();

                    if (keyPressed.Key == ConsoleKey.Spacebar)
                    {
                        return 0;
                    }
                    else if (keyPressed.Key == ConsoleKey.Enter)
                    {
                        Console.Clear();

                        string specialPow1 = itemsAvailable[optionOriginalIndex[markedOption]].specialPower1;
                        string specialPow2 = itemsAvailable[optionOriginalIndex[markedOption]].specialPower2;
                        int specialPow1Value = itemsAvailable[optionOriginalIndex[markedOption]].specialPower1Value;

                        Gear.UseItemFunction(thisWarrior, specialPow1, specialPow1Value);

                        if (specialPow2 != "None")
                        {
                            Game.WrtL("");
                            int specialPow2Value = itemsAvailable[optionOriginalIndex[markedOption]].specialPower2Value;

                            Gear.UseItemFunction(thisWarrior, specialPow2, specialPow2Value);
                        }

                        for (int i = 0; i < Game.partyGear.Count; i++)
                        {
                            if (Game.partyGear[i].name == itemsAvailable[optionOriginalIndex[markedOption]].name)
                                Game.partyGear.RemoveAt(i);
                        }
                        Game.AwaitKeyEnter();
                        return 1;
                    }
                }
            }
        }
        // Method for the menu of party members when choosing who to use an item on.
        public static int ChoosePartyMemberMenu(int partyIndex, string presentation)
        {
            List<string> inYourParty = new List<string>();

            for (int i = 0; i < Game.Party.Count; i++)
            {
                Hero hero = Game.Party[i];
                inYourParty.Add(hero.name + " - Health: " + hero.healthpoints + " of " + hero.maxHealth + " - Stamina: " + hero.stamina + " of " + hero.maxStamina + " - Composure: " + hero.composure + " of " + hero.baseComposure);
            }

            int markedOption = partyIndex; // Because sometimes you want the one who opened the Use Item menu to be the prechosen one.

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                Game.WrtL(presentation + "\n");

                MenuPrinter(inYourParty, markedOption, "  ");

                Game.WrtL("\n");
                Hero.TakeAlookAt(Game.Party[markedOption]);

                var keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption++;
                    if (markedOption >= inYourParty.Count)
                        markedOption = 0;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption--;
                    if (markedOption < 0)
                        markedOption = inYourParty.Count - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Spacebar)
                {
                    return -1;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    return markedOption;
                }
            }
            // Will add a boolean for telling if this is used in the cave or outside, if it's used outside one should be able to keep equipping gear without it exiting the method once equipped something.
        }
        // Method for the menu of equipped gear of a certain warrior, and for changing any of the gear, unequipping or replacing it with new gear.
        public static int EquipGearMenu(Hero warrior)
        {
            // NOTE! It's possible to equip a second weapon through here even when it shouldn't be possible, such as when someone wields a double-handed weapon. Look over the checking of that.
            int markedOption = 0;

            string helmet = "Helmet: ";
            string armor = "Armor: ";
            string weapon = "Main Weapon: ";
            string second_weapon = "Second Weapon: ";
            string shield = "Shield: ";
            string gauntlet = "Gauntlets: ";
            string legwear = "Legs: ";
            string clothes1 = "Clothing 1: ";
            string clothes2 = "Clothing 2: ";
            string jewelry1 = "Jewelry 1: ";
            string jewelry2 = "Jewelry 2: ";
            string jewelry3 = "Jewelry 3: ";
            string artifact1 = "Artifact 1: ";
            string artifact2 = "Artifact 2: ";

            string[] menuOptions;

            while (true)
            {
                menuOptions = new string[] { helmet, armor, weapon, second_weapon, shield, gauntlet, legwear, clothes1, clothes2, jewelry1, jewelry2, jewelry3, artifact1, artifact2 };

                for (int i = 0; i < warrior.warriorGear.Length; i++)
                {
                    if (warrior.warriorGear[i] == null)
                        menuOptions[i] += "None";
                    else
                        menuOptions[i] += warrior.warriorGear[i].name;
                }

                Console.Clear();
                Console.CursorVisible = false;

                Game.WrtL(warrior.name + "s equipment:\n");

                MenuPrinter(menuOptions, markedOption, "   ");

                if (warrior.warriorGear[markedOption] != null)
                {
                    Gear gear = warrior.warriorGear[markedOption];
                    Game.WrtL("\n" + gear.description + "\n");
                    Gear.PresentGearData(gear);
                }

                var keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption++;
                    if (markedOption >= menuOptions.Length)
                        markedOption = 0;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption--;
                    if (markedOption < 0)
                        markedOption = menuOptions.Length - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Spacebar)
                {
                    return 0;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    List<Gear> gearAvailable;
                    Gear choice;
                    // helmet, armor, weapon, second_weapon, shield, gauntlet, legwear, clothes1, clothes2, jewelry1, jewelry2, jewelry3, artifact1, artifact2
                    switch (markedOption)
                    {
                        case 0:
                            // helmet
                            gearAvailable = Game.partyGear.Where(gear => gear.types[1] == "helmet").ToList();
                            choice = EquippableGearMenu(warrior, gearAvailable, warrior.warriorGear[markedOption], markedOption);
                            // No new gear equipped;
                            if (choice == null)
                            {
                                if (warrior.warriorGear[markedOption] == null)
                                {
                                    warrior.equippedHelmet = choice;
                                    Hero.UpdateWarriorStats(warrior);
                                }   
                                continue;
                            }
                            else // Gear equipped, leave.
                            {
                                warrior.equippedHelmet = choice;
                                return 1;
                            } 
                        case 1:
                            // armor
                            gearAvailable = Game.partyGear.Where(gear => gear.types[0] == "armor").ToList();
                            choice = EquippableGearMenu(warrior, gearAvailable, warrior.warriorGear[markedOption], markedOption);
                            // No new gear equipped;
                            if (choice == null)
                            {
                                if (warrior.warriorGear[markedOption] == null)
                                {
                                    warrior.equippedArmor = choice;
                                    Hero.UpdateWarriorStats(warrior);
                                }
                                continue;
                            }
                            else // Gear equipped, leave.
                            {
                                warrior.equippedArmor = choice;
                                return 1;
                            }
                        case 2:
                            // weapon
                            gearAvailable = Game.partyGear.Where(gear => gear.types[0] == "weapon").ToList();
                            choice = EquippableGearMenu(warrior, gearAvailable, warrior.warriorGear[markedOption], markedOption);
                            // No new gear equipped;
                            if (choice == null)
                            {
                                if (warrior.warriorGear[markedOption] == null)
                                {
                                    warrior.equippedWeapon = choice;
                                    Hero.UpdateWarriorStats(warrior);
                                }
                                continue;
                            }
                            else // Gear equipped, leave.
                            {
                                warrior.equippedWeapon = choice;
                                return 1;
                            }
                        case 3:
                            // second_weapon
                            if (warrior.equippedShield == null)
                            {
                                gearAvailable = Game.partyGear.Where(gear => gear.types[0] == "weapon").ToList();
                                choice = EquippableGearMenu(warrior, gearAvailable, warrior.warriorGear[markedOption], markedOption);
                                // No new gear equipped;
                                if (choice == null)
                                {
                                    if (warrior.warriorGear[markedOption] == null)
                                    {
                                        warrior.equippedSecondaryWeapon = choice;
                                        Hero.UpdateWarriorStats(warrior);
                                    }
                                    continue;
                                }
                                else // Gear equipped, leave.
                                {
                                    warrior.equippedSecondaryWeapon = choice;
                                    return 1;
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.CursorVisible = true;

                                Game.WrtL("You have a shield equipped, so you can't equip a second weapon. Unequip the shield first.");

                                Game.AwaitKeyEnter();
                                continue;
                            }
                        case 4:
                            // shield
                            if (warrior.equippedSecondaryWeapon == null)
                            {
                                gearAvailable = Game.partyGear.Where(gear => gear.types[0] == "shield").ToList();
                                choice = EquippableGearMenu(warrior, gearAvailable, warrior.warriorGear[markedOption], markedOption);
                                // No new gear equipped;
                                if (choice == null)
                                {
                                    if (warrior.warriorGear[markedOption] == null)
                                    {
                                        warrior.equippedShield = choice;
                                        Hero.UpdateWarriorStats(warrior);
                                    }
                                    continue;
                                }
                                else // Gear equipped, leave.
                                {
                                    warrior.equippedShield = choice;
                                    return 1;
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.CursorVisible = true;

                                Game.WrtL("You have a secondary weapon equipped, so you can't equip a shield. Unequip the secondary weapon first.");

                                Game.AwaitKeyEnter();
                                continue;
                            }
                        case 5:
                            // gauntlet
                            gearAvailable = Game.partyGear.Where(gear => gear.types[1] == "gauntlet").ToList();
                            choice = EquippableGearMenu(warrior, gearAvailable, warrior.warriorGear[markedOption], markedOption);
                            // No new gear equipped;
                            if (choice == null)
                            {
                                if (warrior.warriorGear[markedOption] == null)
                                {
                                    warrior.equippedGauntlet = choice;
                                    Hero.UpdateWarriorStats(warrior);
                                }
                                continue;
                            }
                            else // Gear equipped, leave.
                            {
                                warrior.equippedGauntlet = choice;
                                return 1;
                            }
                        case 6:
                            // legwear
                            gearAvailable = Game.partyGear.Where(gear => gear.types[1] == "leg wear").ToList();
                            choice = EquippableGearMenu(warrior, gearAvailable, warrior.warriorGear[markedOption], markedOption);
                            // No new gear equipped;
                            if (choice == null)
                            {
                                if (warrior.warriorGear[markedOption] == null)
                                {
                                    warrior.equippedLegWear = choice;
                                    Hero.UpdateWarriorStats(warrior);
                                }
                                continue;
                            }
                            else // Gear equipped, leave.
                            {
                                warrior.equippedLegWear = choice;
                                return 1;
                            }
                        case 7:
                            // clothes1
                            gearAvailable = Game.partyGear.Where(gear => gear.types[1] == "clothes").ToList();
                            choice = EquippableGearMenu(warrior, gearAvailable, warrior.warriorGear[markedOption], markedOption);
                            // No new gear equipped;
                            if (choice == null)
                            {
                                if (warrior.warriorGear[markedOption] == null)
                                {
                                    warrior.wornClothes1 = choice;
                                    Hero.UpdateWarriorStats(warrior);
                                }
                                continue;
                            }
                            else // Gear equipped, leave.
                            {
                                warrior.wornClothes1 = choice;
                                return 1;
                            }
                        case 8:
                            // clothes2
                            gearAvailable = Game.partyGear.Where(gear => gear.types[1] == "clothes").ToList();
                            choice = EquippableGearMenu(warrior, gearAvailable, warrior.warriorGear[markedOption], markedOption);
                            // No new gear equipped;
                            if (choice == null)
                            {
                                if (warrior.warriorGear[markedOption] == null)
                                {
                                    warrior.wornClothes2 = choice;
                                    Hero.UpdateWarriorStats(warrior);
                                }
                                continue;
                            }
                            else // Gear equipped, leave.
                            {
                                warrior.wornClothes2 = choice;
                                return 1;
                            }
                        case 9:
                            // jewelry1
                            gearAvailable = Game.partyGear.Where(gear => gear.types[1] == "jewelry").ToList();
                            choice = EquippableGearMenu(warrior, gearAvailable, warrior.warriorGear[markedOption], markedOption);
                            // No new gear equipped;
                            if (choice == null)
                            {
                                if (warrior.warriorGear[markedOption] == null)
                                {
                                    warrior.equippedJewelry1 = choice;
                                    Hero.UpdateWarriorStats(warrior);
                                }
                                continue;
                            }
                            else // Gear equipped, leave.
                            {
                                warrior.equippedJewelry1 = choice;
                                return 1;
                            }
                        case 10:
                            // jewelry2
                            gearAvailable = Game.partyGear.Where(gear => gear.types[1] == "jewelry").ToList();
                            choice = EquippableGearMenu(warrior, gearAvailable, warrior.warriorGear[markedOption], markedOption);
                            // No new gear equipped;
                            if (choice == null)
                            {
                                if (warrior.warriorGear[markedOption] == null)
                                {
                                    warrior.equippedJewelry2 = choice;
                                    Hero.UpdateWarriorStats(warrior);
                                }
                                continue;
                            }
                            else // Gear equipped, leave.
                            {
                                warrior.equippedJewelry2 = choice;
                                return 1;
                            }
                        case 11:
                            // jewelry3
                            gearAvailable = Game.partyGear.Where(gear => gear.types[1] == "jewelry").ToList();
                            choice = EquippableGearMenu(warrior, gearAvailable, warrior.warriorGear[markedOption], markedOption);
                            // No new gear equipped;
                            if (choice == null)
                            {
                                if (warrior.warriorGear[markedOption] == null)
                                {
                                    warrior.equippedJewelry3 = choice;
                                    Hero.UpdateWarriorStats(warrior);
                                }
                                continue;
                            }
                            else // Gear equipped, leave.
                            {
                                warrior.equippedJewelry3 = choice;
                                return 1;
                            }
                        case 12:
                            // artifact1
                            gearAvailable = Game.partyGear.Where(gear => gear.types[1] == "artifact" && (gear.isEquippable || gear.isPartyEquippable)).ToList();
                            choice = EquippableGearMenu(warrior, gearAvailable, warrior.warriorGear[markedOption], markedOption);
                            // No new gear equipped;
                            if (choice == null)
                            {
                                if (warrior.warriorGear[markedOption] == null)
                                {
                                    warrior.equippedArtifact1 = choice;
                                    Hero.UpdateWarriorStats(warrior);
                                }
                                continue;
                            }
                            else // Gear equipped, leave.
                            {
                                warrior.equippedArtifact1 = choice;
                                return 1;
                            }
                        case 13:
                            // artifact2
                            gearAvailable = Game.partyGear.Where(gear => gear.types[1] == "artifact" && (gear.isEquippable || gear.isPartyEquippable)).ToList();
                            choice = EquippableGearMenu(warrior, gearAvailable, warrior.warriorGear[markedOption], markedOption);
                            // No new gear equipped;
                            if (choice == null)
                            {
                                if (warrior.warriorGear[markedOption] == null)
                                { 
                                    warrior.equippedArtifact2 = choice;
                                    Hero.UpdateWarriorStats(warrior);
                                }
                                continue;
                            }
                            else // Gear equipped, leave.
                            {
                                warrior.equippedArtifact2 = choice;
                                return 1;
                            }
                    }
                }
            }
        }
        // Method for a menu of available gear specific for a certain equippable space (attribute 'List<Gear> available'). Warning if there are no more gear to equip.
        public static Gear EquippableGearMenu(Hero warrior, List<Gear> available, Gear equippedGear, int warriorGearIndex)
        {
            Console.Clear();
            Console.CursorVisible = true;

            List<string> menuOptions = new List<string>();

            int markedOption = 0;
            bool onlyEquippedGear = false;

            if (available.Count == 0)
            {
                if (equippedGear != null)
                {
                    onlyEquippedGear = true;
                    available.Add(equippedGear);
                }
                else
                {
                    Game.WrtL("You have nothing equipped or to equip here.\n");
                    Game.AwaitKeyEnter();
                    return null;
                }
            }
            else
            {
                if (equippedGear != null)
                {
                    available.Insert(0, equippedGear);
                }  
            }

            for (int i = 0; i < available.Count; i++)
            {
                if (i == 0 && equippedGear != null)
                {
                    menuOptions.Add("Currently Equipped: " + equippedGear.name);
                }
                else
                {
                    menuOptions.Add(available[i].name);
                }
            }

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                if (onlyEquippedGear)
                {
                    Game.WrtL("You don't have any new gear of this type to equip.");
                    Game.WrtL("But this is your currently equipped gear:\n");
                }

                MenuPrinter(menuOptions, markedOption, "   ");

                Game.WrtL("\n" + available[markedOption].description + "\n");

                Gear.PresentGearData(available[markedOption]);

                var keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption++;
                    if (markedOption >= menuOptions.Count)
                        markedOption = 0;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption--;
                    if (markedOption < 0)
                        markedOption = menuOptions.Count - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Spacebar)
                {
                    return null;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    if (markedOption == 0 && equippedGear != null)
                    {
                        Console.Clear();
                        Console.CursorVisible = true;

                        Game.WrtL("Would you like to unequip " + equippedGear.name + "?\n");
                        Game.WrtL("Enter to unequip");
                        Game.WrtL("Spacebar to go back to list");

                        keyPressed = Console.ReadKey();

                        if (keyPressed.Key == ConsoleKey.Spacebar)
                        {
                            return null;
                        }
                        else if (keyPressed.Key == ConsoleKey.Enter)
                        {
                            string oldEquip = equippedGear.name;

                            Game.partyGear.Add(equippedGear);
                            Game.SortPartyGear();

                            warrior.warriorGear[warriorGearIndex] = null;

                            if (warrior.isDualWielding && (warriorGearIndex == 2 || warriorGearIndex == 3))
                                warrior.isDualWielding = false;

                            if (equippedGear.isDoubleHandheld)
                            {
                                // Eventually, when warriors can get so strong that they can carry such weapons in one hand, exceptions need to be made here.
                                // The question rises: Will the warrior's isUsingDoubleHandedWeapon truly be true if they are strong enough for the weapons to not require both hands?
                                warrior.isUsingDoubleHandedWeapon = false;
                            }

                            Console.Clear();

                            Game.WrtL("You unequipped " + oldEquip + "!");

                            Game.AwaitKeyEnter();
                            return null;
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.CursorVisible = true;

                        if (available[markedOption].wieldLvl > warrior.level)
                        {
                            Game.WrtL(menuOptions[markedOption] + " requires level " + available[markedOption].wieldLvl + " to wield, and " + warrior.name + "'s current level is only " + warrior.level + ".");
                            
                            Game.AwaitKeyEnter();
                            return null;
                        }
                        else if (available[markedOption].dualWieldLvl > warrior.dualWieldLevel)
                        {
                            if (warrior.isDualWielding ||
                               (warrior.equippedWeapon != null && warriorGearIndex == 3) ||
                               (warrior.equippedSecondaryWeapon != null && warriorGearIndex == 2))
                            {
                                if (available[markedOption].dualWieldLvl == 99)
                                {
                                    Game.WrtL(menuOptions[markedOption] + " is impossible to dual wield, and " + warrior.name + " is already carrying one weapon.");
                                }
                                else
                                {
                                    Game.WrtL(menuOptions[markedOption] + " requires level " + available[markedOption].dualWieldLvl + " to dual wield, and " + warrior.name + "'s current dual wield level is only " + warrior.dualWieldLevel + ".");
                                }

                                Game.AwaitKeyEnter();
                                return null;
                            }
                        }
                        else if (available[markedOption].isDoubleHandheld &&
                                ((warrior.equippedWeapon != null && warriorGearIndex == 3) ||
                                (warrior.equippedSecondaryWeapon != null && warriorGearIndex == 2) ||
                                (warrior.equippedShield != null)))
                        {
                            Game.WrtL(menuOptions[markedOption] + " requires both hands to wield and " + warrior.name + " is already carrying a weapon or a shield.");

                            Game.AwaitKeyEnter();
                            return null;
                        }

                        if (equippedGear == null)
                            Game.WrtL("Would you like to equip " + menuOptions[markedOption] + "?\n");
                        else
                            Game.WrtL("Would you like to equip " + menuOptions[markedOption] + " instead of " + equippedGear.name + "?\n");

                        // NOTE! Add a warning if the weapon in question isn't the warrior's preferable weapon type and therefore won't add preference points.

                        Game.WrtL("Enter to equip");
                        Game.WrtL("Spacebar to go back to list");

                        keyPressed = Console.ReadKey();

                        if (keyPressed.Key == ConsoleKey.Spacebar)
                        {
                            return null;
                        }
                        else if (keyPressed.Key == ConsoleKey.Enter)
                        {
                            if (equippedGear != null)
                            {
                                Game.partyGear.Add(equippedGear);
                                Game.SortPartyGear();
                            }

                            warrior.warriorGear[warriorGearIndex] = available[markedOption];
                            Game.partyGear.Remove(available[markedOption]);

                            if ((warrior.equippedWeapon != null && warriorGearIndex == 3) ||
                                (warrior.equippedSecondaryWeapon != null && warriorGearIndex == 2))
                            {
                                warrior.isDualWielding = true;
                            }
                            if (available[markedOption].isDoubleHandheld)
                            {
                                warrior.isUsingDoubleHandedWeapon = true;
                            }

                            Console.Clear();

                            Game.WrtL("You equipped " + available[markedOption].name + "!\n");

                            Game.AwaitKeyEnter();
                            return available[markedOption];
                        }
                    }
                } 
            }
        }
        // Method for the pause menu when pressing spacebar in the cave, outside of combat.
        public static void PauseMenu(Cave room)
        {
            string[] menuOptions = new string[] { "Pause And Rest", "Rearrange Team", "Check Team\t", "Check Gear\t" };

            int markedOption = 0;

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                Game.WrtL("yl", "", "   PAUSE MENU\n", true);

                MenuPrinter(menuOptions, markedOption, "   ");

                Hero.SeePartyVitals();

                var keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption++;
                    if (markedOption >= menuOptions.Length)
                        markedOption = 0;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption--;
                    if (markedOption < 0)
                        markedOption = menuOptions.Length - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    switch (markedOption)
                    {
                        case 0:
                            // Lets make sure the whole team feels a little better.
                            int cavePos = room.caveFloor[Cave.PartyPosition[0], Cave.PartyPosition[1]];
                            bool restIsOK = true;
                            if (cavePos == 6)
                            {
                                restIsOK = false;

                                Console.Clear();
                                Game.WrtL("You recently had a battle at this location, it's still likely to attract more monsters.");
                                Game.WrtL("It's really unsafe to stop and rest here.\n");

                                Game.AwaitKeyEnter();
                            }
                            else if (cavePos == 4 || cavePos == 7)
                            {
                                restIsOK = false;

                                Console.Clear();
                                Game.WrtL("At pathways the chance of monsters passing through between cave rooms is too high a risk.");
                                Game.WrtL("It's really unwise to stop and rest here.\n");

                                Game.AwaitKeyEnter();
                            }
                            else if (cavePos == 5)
                            {
                                restIsOK = false;

                                Console.Clear();
                                Game.WrtL("Near treasure chests the light from your torches will create reflections which can reach farther than you anticipate.\nIt might give away your location a lot easier than you want.");
                                Game.WrtL("It's an unnecessary risk to stop and rest here.\n");

                                Game.AwaitKeyEnter();
                            }

                            if (restIsOK)
                            {
                                Game.RestParty();
                                return;
                            }
                            else
                            {
                                break;
                            }
                        case 1:
                            // Lets change the order of the warriors in the Party.
                            RearrangeTeamMenu();
                            break;
                        case 2:
                            // Lets just have an old school look at the team.
                            Console.Clear();
                            Game.LookAtParty();
                            Game.AwaitKeyEnter();
                            break;
                        case 3:
                            // Lets take a look at all the stuff you've got.
                            Game.LookAtGear();
                            break;
                    }
                }
                else if (keyPressed.Key == ConsoleKey.Spacebar)
                {
                    return;
                }
            }
        }
        // Method for the scrolling of the rest menu, returning the choice as int.
        public static int RestMenu()
        {
            string[] menuOptions = new string[] { "Check Gear\t", "Check Team\t", "Use Item\t", "Equip\t\t", "Rest Some More", "Get Going\t" };

            int markedOption = 0;

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                Game.WrtL("What will you do next?\n");

                MenuPrinter(menuOptions, markedOption, "  ");

                Hero.SeePartyVitals();

                var keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption++;
                    if (markedOption >= menuOptions.Length)
                        markedOption = 0;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption--;
                    if (markedOption < 0)
                        markedOption = menuOptions.Length - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    return markedOption;
                }
            }
        }
        // Method for adding a new point to a stat when leveling up, giving a menu of the stats available to upgrade.
        public static void AddNewLevelPointMenu(Hero warrior)
        {
            string[] menuOptions = new string[] { "Health: " + warrior.maxHealth, "Strength: " + warrior.maxStrength, "Stamina: " + warrior.maxStamina, "Speed: " + warrior.maxSpeed};

            int markedOption = 0;

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                Game.WrtL("This is " + warrior.name + "'s current skills.");
                Game.WrtL("Which one do you want to add the new point to?\n");

                MenuPrinter(menuOptions, markedOption, " + 1? ");

                // NOTE! Maybe there should be a live update on how the point affects the other stats?
                // As of now, one chooses and that's it, no regrets. Maybe that is enough?

                var keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption++;
                    if (markedOption >= menuOptions.Length)
                        markedOption = 0;
                }

                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption--;
                    if (markedOption < 0)
                        markedOption = menuOptions.Length - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    Console.CursorVisible = true;

                    int newValue;

                    if (markedOption == 0)
                    {
                        warrior.addedHealthPoints++;
                        warrior.maxHealth = warrior.baseHealth + warrior.addedHealthPoints;
                        warrior.healthpoints++;

                        newValue = warrior.maxHealth;

                        Game.WrtL("You added to the health!");
                        Game.Wrt("New maximum health (without equipped bonuses): ");
                        Game.WrtL("w", "", newValue, true);
                    }
                    else if (markedOption == 1)
                    {
                        warrior.addedStrengthPoints++;
                        warrior.maxStrength = warrior.baseStrength + warrior.addedStrengthPoints;
                        warrior.strength++;
                        
                        newValue = warrior.maxStrength;

                        Game.WrtL("You added to the strength!");
                        Game.Wrt("New maximum strength (without equipped bonuses): ");
                        Game.WrtL("w", "", newValue, true);
                    }
                    else if (markedOption == 2)
                    {
                        warrior.addedStaminaPoints++;
                        warrior.maxStamina = warrior.baseStamina + warrior.addedStaminaPoints;
                        warrior.stamina++;

                        newValue = warrior.maxStamina;

                        Game.WrtL("You added to the stamina!");
                        Game.Wrt("New maximum stamina (without equipped bonuses): ");
                        Game.WrtL("w", "", newValue, true);
                    }
                    else if (markedOption == 3)
                    {
                        warrior.addedSpeedPoints++;
                        warrior.maxSpeed = warrior.baseSpeed + warrior.addedSpeedPoints;
                        warrior.speed++;

                        newValue = warrior.maxSpeed;

                        Game.WrtL("You added to the speed!");
                        Game.Wrt("New maximum speed (without equipped bonuses): ");
                        Game.WrtL("w", "", newValue, true);
                    }
                    
                    Hero.UpdateWarriorStats(warrior);

                    Game.AwaitKeyEnter();
                    break;
                }
            }
        }
        // Method for the menu to rearrange the team.
        public static int RearrangeTeamMenu()
        {
            string[] menuOptions = new string[] { "Reverse Team Order\t\t", "Put Healthiest Warriors In Front", "Put Strongest Warriors In Front" };

            int markedOption = 0;

            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                Game.WrtL("yl","","   REARRANGE TEAM\n", true);
                Game.WrtL("You can choose to rearrange the team, for example to have certain warriors first in combat,\nor injured warriors last in combat.\n\n");

                MenuPrinter(menuOptions, markedOption, "\t");

                var keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption++;
                    if (markedOption >= menuOptions.Length)
                        markedOption = 0;
                }

                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption--;
                    if (markedOption < 0)
                        markedOption = menuOptions.Length - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    switch (markedOption)
                    {
                        case 0:
                            // Reverse Team Order, as simple as it says.
                            Game.ReversePartyOrder();
                            Game.AwaitKeyEnter();
                            break;
                        case 1:
                            // Put Healthiest Warriors in front.
                            Game.SortPartyBySpecificInput("health");
                            Game.AwaitKeyEnter();
                            break;
                        case 2:
                            // Put Strongest Warriors in front. Best Attack power, not just strength.
                            Game.SortPartyBySpecificInput("attackpower");
                            Game.AwaitKeyEnter();
                            break;
                    }
                }
                else if (keyPressed.Key == ConsoleKey.Spacebar)
                {
                    return -1;
                }
            }
        }

        // Method to print an unmarked part of a menu.
        public static void Option(string[] menu, int index)
        {
            Game.WrtL("  " + menu[index]);
        }
        // Method to print the marked part of a menu.
        public static void MarkedOption(string[] menu, int index, string spaceOfChoice)
        {
            Game.WrtL("w", "", "> " + menu[index] + spaceOfChoice + "<--", true);
        }

        // Method to print an unmarked part of a menu. Overloaded.
        public static void Option(List<string> menu, int index)
        {
            Game.WrtL("  " + menu[index]);
        }
        // Method to print the marked part of a menu. Overloaded.
        public static void MarkedOption(List<string> menu, int index, string spaceOfChoice)
        {
            Game.WrtL("w", "", "> " + menu[index] + spaceOfChoice + "<--", true);
        }

        // !!NOT USED!! Mehod to print a list as a menu where one can navigate up and down and choose one of the options, returns a number of ones choice.
        // NOTE! When I start using this, remember to create an overloaded version for lists instead of arrays, and maybe a couple more having the choice to go back in menus with spacebar.
        public int MenuChooser(string[] menu, string text, string spaceOfChoice)
        {
            string[] menuChoice = menu;

            int markedOption = 0;

            // Redraws everything every time one switches option.
            while (true)
            {
                Console.Clear();
                Console.CursorVisible = false;

                Game.WrtL(text + "\n");

                MenuPrinter(menuChoice, markedOption, spaceOfChoice);

                Console.WriteLine("\n\nUse arrow buttons up and down, press Enter to choose.");

                var keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    markedOption++;
                    // Enables looping the menu for user friendlyness.
                    if (markedOption >= menuChoice.Length)
                        markedOption = 0;
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    markedOption--;
                    if (markedOption < 0)
                        markedOption = menuChoice.Length - 1;
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    return markedOption;
                }
            }
        }
        // Method for deciding wether the option is the marked one or not, and using the correct print method accordingly. For arrays.
        public static void MenuPrinter(string[] menuChoice, int markedOption, string spaceOfChoice)
        {
            for (var i = 0; i < menuChoice.Length; i++)
            {
                if (markedOption == i)
                {
                    MarkedOption(menuChoice, i, spaceOfChoice);
                }
                else
                {
                    Option(menuChoice, i);
                }
            }
        }
        // Method for deciding wether the option is the marked one or not, and using the correct print method accordingly. For lists. Overloaded.
        public static void MenuPrinter(List<string> menuChoice, int markedOption, string spaceOfChoice)
        {
            for (var i = 0; i < menuChoice.Count; i++)
            {
                if (markedOption == i)
                {
                    MarkedOption(menuChoice, i, spaceOfChoice);
                }
                else
                {
                    Option(menuChoice, i);
                }
            }
        }
    }
}
