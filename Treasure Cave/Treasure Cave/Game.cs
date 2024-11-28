using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;

namespace TreasureCave
{
    public class Game
    {
        public static Random randomize = new Random();

        public static bool testmode = false;

        public static Hero You;
        // List of heroes that you will have in your party.
        public static List<Hero> Party = new List<Hero>();
        public static int maximumPartyEnlisting = 0;
        public static int averagePartyLevel = 1;

        // A list of values indicating the amount of experience points needed to rise to the next level. First number is what it takes to rise to level 2.
        public static int[] levelRiseValues = { 500, 1000, 1700, 2700, 4000, 5800, 8000, 10500, 16000,
                                                22500, 29600, 37400, 45900, 55200, 70000, 86000, 103100, 121200, 146800,
                                                173700, 202200, 246900, 293700, 342700, 402400, 464500, 529100, 596200, 695600,
                                                798500, 915700, 1037000, 1162000, 1302000, 1447000, 1671000, 1914000, 2163000, 2433000,
                                                2700000, 3000000 };

        public static int[] dualWieldLevelRiseValues = { 900, 2500, 6000, 10000, 17000, 27000, 40000, 60000, 90000,
                                                         140000, 210000, 300000, 450000, 650000, 900000, 1100000, 1600000, 2200000, 3000000 };

        // dualWieldDice { first one is dualWieldDice, second is value to add to chanceToCounterAttack } Both are to be used at each dual wield level.
        public static int[] dualWieldLevelStepData = {  -3, 0, -2, 1, -1, 0, 0, 1, 1, 0, 1, 1, 2, 0, 2, 1, 3, 0,
                                                        3, 1, 3, 1, 4, 0, 4, 1, 4, 2, 5, 0, 5, 2, 6, 1, 6, 2, 7, 3 };
        // sum chanceToCounterAttack = 17;

        public static List<Gear> partyGear = new List<Gear>();

        public static int Branzen = 1500; // Branzen you start with. Instead of gold... Gold sounds so very expensive. 100 Branzen is 1 gold, maybe?
        // Rubels? Rubins? Reen? Goul? Girren? Branzen? Bickel? Guildens? Fure? Fein? Cling? Crowns? Shale? Chiff? Silvens? Siv? Quin? Qunits? Antall? Beckers?

        // Time related ints. One hour is four time units. One time unit is the smallest time that can pass.
        // I have added a bit more variables than I'm using yet, but they're planned to have a use eventually.
        public static int timeUnits = 0;
        public static int hourOfDay = 8;
        public static string timeOfDay = "morning";
        public static string[] hourNames = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve" };
        public static int hourAmountOfTheDay = 24;
        public static int morning = 7;
        public static int daytime = 11;
        public static int evening = 16;
        public static int sunset = 18;
        public static int nighttime = 19;
        public static int weekDay = 1;
        public static int amountOfWeekDays = 7;
        public static string dayOfWeek = "verday";
        public static string[] weekDayNames = { "verday", "nirday", "ornday", "goraday", "saraday", "lagarday", "brimday" };

        // int to keep track of the amount of times all warriors have fought an enemy.
        public static int battleRound = 0;
        // Bool to keep track of when you can rest your team.
        public static bool justRested = false;
        public static bool inTheCave = false;

        // int to get the first cave, and to keep the first cave as the entrance cave.
        public static int startingCave;
        // Bool to keep track of when you enter a new cave room for the first time.
        public static bool justEntered = true;
        // int used for making sure caves get at least a certain amount of exits.
        public static int minimalNeedForEntrances = 4;

        public static string[] ailments = { "bleeding", "fatigue", "berserk", "unconscious", "stunned", "immobilized", "confused", "panicking", "dying", "poisoned"};
        public static List<int> partyMembersInPeril = new List<int>(); // party members with an ailment, that has been defeated or that has lost their composure is put in this list.
        // partyMembersInPeril will be used for when others in the party will help them, we need to know which members need help. It's an 'int' because I plan to save the warriors alltimePartyId here,
        // beacause that never changes, regardless of amount of warriors in the party or order of them.
        // Want to make an ailment called "skin rot" that affects little in the beginning and the description of the sick one changes over time. Skin rot will make your skin turn soft and a bad colour,
        // eventually it will be dead and easily tear off, leaving your flesh bear. It spreads slowly, so treating it in time is not impossible, but when the skin is dead it's dead, and skin falling
        // off is a dangerous injury. Letting it be for some time can be fatal. One can get it from bites of disgusting creatures, such as the rat king.
        // As the disease spreads it affects the sick one, they get weaker and tired and lower composure the longer they're sick.

        // This is an array of the names of the different warrior types.
        public static string[] warriorTypes = { "bounty Hunter", "barbarian", "ranger", "rogue", "duelist", "fighter"};

        // This is an array of all the warrior types base stats.
        // Reads as follows:

        // 1 Bounty Hunter
        // 2 Barbarian
        // 3 Ranger
        // 4 Rogue
        // 5 Duelist
        // 6 Fighter

        // And the stats are in order: health, strength, stamina, speed, composure.
        public static int[,] warriorBaseStatArray = new int[6, 5]
        {
            { 7,9,9,7,   10 },
            { 10,10,6,6, 9 },
            { 7,6,8,11,  12 },
            { 10,5,9,8,  8 },
            { 7,9,6,10,  14 },
            { 10,6,7,9,  8 }
        };
        // This is an array of all the warrior types' descriptions. Same order as base stats above.
        public static string[] warriorDescriptions = { "A strong and resilient warrior, well used to all mid-sized and ranged weaponry.\nThey see all fights as an opportunity to wealth, " +
            "and keeping something from the bounty as a trophy is common.\nBounty hunters often get extra cash by selling monster parts to druids and collectors." +
            "\nThey are proficient on monster facts, certainly their weaknesses. But often also human weaknesses.",
            "A powerful and enduring warrior, favouring large weapons.\nThey love to rush right into battle with their weapon high and often gain an upper hand by the " +
            "simple fact that\nthey are imposing.\nSome barbarians can berserk, becoming an even more powerful ally, but are very dangerous if they lose\ntheir composure too much...",
            "A fast and resilient warrior; prefers and are great at ranged weaponry.\nGood at finding their way in terrain, knowing trees, herbs and mushrooms, " +
            "they are excellent outdoor survivors.\nThey hunt and hide well.\nThey often collect and sell rare herbs and mushrooms, as well as hides and parts from animals.",
            "A resilient and enduring warrior. Expert at small weapons, often good at dual wielding.\nMany are versed in thievery, almost all in hiding in shadows, " +
            "moving unseen and unheard. Surviving on smarts as\nmuch as they can, but are not ill fitted for fights.\nKnow value of treasure more than its history.",
            "A dedicated weapon master, focusing on strength and speed to win battles fast and effectively.\nOften proud of their skills, they may be high-hats, " +
            "but has proof of their words if anyone is unwise enough\nto confront them." +
            "\nLong and hard training in their single weapon renders them quite stale with any other type of equipment.",
            "A warrior used to all kinds of weapons but rarely specialized in any.\nThis is someone who's been a soldier for some time, but now live as a mercenary " +
            "after loosing their job, moral code\nor conviction, or they were a young person fighting in the streets until that could make them a living." +
            "\nThey can take a lot of beating, and they're fast."};

        // This is an array of all the warrior types' battlecries. Same order as base stats above.
        public static string[,] warriorBattlecries = new string[6, 9]
        {
            { "I'll take a piece of you as proof of my victory!", "I doubt your bounty is anything at all... Your life will simply be my prize!", "I'm a hunter, you're a bounty, nothing more!",
                "Your pieces will probably sell for something...", "You're no match. You are merchandise, creature.", "I'm not paid to draw this out, only to kill you!",
                "If you knew your worth, this wouldn't be your choice.", "You give worthless a new face, creature!", "Can I sell you? Guess I'll figure it out after I chop you up!" },
            { "I will slash you into pieces, fiend!!", "Die, monster!!", "Disintegrate before my might, fiend!", "This meeting is your doom, beast!","RRRAAAAAAAGHHHHH!!!",
                "I'll shatter your skull and splatter your guts!", "You will try to run back into your darkness soon, beast, but your time ran out when you met me!",
                "Your blood will paint the walls, monster!", "Never will you rise from my blow, vile creature!" },
            { "Oh, if you're game, you're dead, beast!", "You won't know what hit you before this is over!",
                "Given any other circumstance, you would never have seen me before falling by my hand.", "Beast, killing you is my livelihood!",
                "You're unedible! But a raging boar must be put down!", "You clearly lack animal instincts attacking me!", "You dear attack me, beast!? I hunt you for a living!",
                "You waste of a being. Your dead body must be unedible...", "This beast is unnatural! It must be removed to protect the balance of these lands!" },
            { "I don't expect you to play fair! I assure you, I won't!", "I resent these torchlights too, fellow shadow dweller, but we must play our parts.",
                "Moral is off the map in here, monster! I'm not great at it out there either!", "An animal huh? Not a problem, it's just easier to trick!",
                "You're free to give up any time, between each cut I deal you that is.", "Come on, turn your back on me, I dare you!",
                "Foul smell! In my experience that indicates foul play as well!", "Animal instincts by all means, but yours are off, I tell ya.", "I'll bite back, you animal..." },
            { "I'll try not to underestimate you, beast. But you're welcome to do so with me.", "One strike, one kill.", "Let's not draw this out, come at me, beast!",
                "A simple beast against my art? Crude strokes are beneath me!", "Your animal rage cannot touch my honed mind.", "Your death is imminent.", "I renounce no duel!",
                "A battle is always about life or death.", "Your focus can't match mine, my sharp attacks will cut your wild outbursts short." },
            { "Come on then! I can do this all day!", "Oooh! Don't you get me started!", "How much can you take? I don't need this to end soon!", "Let's fight then, monster!",
                "Blood is gonna flow, more from you than me I wager!", "Another fight! Another round! A life lived full circle is the only battle that ends!",
                "I lost count of my fights... Aaagh, whatever, I've lost count of how many fights ago that was too!", "Fight me! I'm yours if you can floor me!",
                "Another day, another fight..." }
        };

        // 1 Bounty Hunter
        // 2 Barbarian
        // 3 Ranger
        // 4 Rogue
        // 5 Duelist
        // 6 Fighter

        // This is an array of all the warrior types' favoured weapon types. Or names of them. Same order as base stats above.
        public static string[,] warriorPreferredWeaponry = new string[6, 2]
        {
            { "medium", "ranged" },
            { "melee", "twohanded" },
            { "ranged", "spear" },
            { "sickle", "knife" },
            { "any weapon type of your choice", "any weapon size of your choice" }, // duelist is a special case. Check their constructor for details. These lines are for the beginning phase when the player is creating a new warrior.
            { "medium", "melee" }
        };
        // An array of names of starting items for different warrior types, the actual items are later picked from an object array based on these names.
        public static string[,] warriorTypeStartingItems = new string[6, 2]
        {
            { "Small Stamina Potion", "Small Zen Potion" },
            { "Grilled Meat", "Berserk Potion" },
            { "Small Steroid Potion", "Small Stamina Potion" },
            { "Rum", "Small Zen Potion" },
            { "Small Stamina Potion", "Gaia Fruit" },
            { "Small Stamina Potion", "Rum" }
        };

        // Method to aid text writing methods in adding colour.
        static void ColourChooser(string col, string bcol)
        {
            if (col == "gr")
                gr();
            else if (col == "dg")
                dg();
            else if (col == "c")
                c();
            else if (col == "dc")
                dc();
            else if (col == "yl")
                yl();
            else if (col == "dy")
                dy();
            else if (col == "r")
                r();
            else if (col == "b")
                b();
            else if (col == "m")
                m();
            else if (col == "dm")
                dm();
            else if (col == "w")
                w();
            else if (col == "bl")
                bl();

            if (bcol == "gr")
                Bgr();
            else if (bcol == "dg")
                Bdg();
            else if (bcol == "c")
                Bc();
            else if (bcol == "dc")
                Bdc();
            else if (bcol == "yl")
                Byl();
            else if (bcol == "dy")
                Bdy();
            else if (bcol == "r")
                Br();
            else if (bcol == "b")
                Bb();
            else if (bcol == "m")
                Bm();
            else if (bcol == "dm")
                Bdm();
            else if (bcol == "w")
                Bw();
            else if (bcol == "bl")
                Bbl();
        }
        // Several small methods for creating coloured text and such. Over loaded for ease of use.
        public static void WrtL(string text) { Console.WriteLine(text); }

        // These types of methods take four arguments: colour for text, colour for backgrund, text to write, whether to reset the colour or not.
        public static void WrtL(string colour, string Bcolour, string text, bool reset)
        {
            ColourChooser(colour, Bcolour);
            WrtL(text);
            if (reset)
                res();
        }
        public static void WrtL(int numbers) { Console.WriteLine(numbers); }
        public static void WrtL(string colour, string Bcolour, int numbers, bool reset)
        {
            ColourChooser(colour, Bcolour);
            WrtL(numbers);
            if (reset)
                res();
        }
        public static void Wrt(string text) { Console.Write(text); }
        public static void Wrt(string colour, string Bcolour, string text, bool reset)
        {
            ColourChooser(colour, Bcolour);
            Wrt(text);
            if (reset)
                res();
        }
        public static void Wrt(int numbers) { Console.Write(numbers); }
        public static void Wrt(string colour, string Bcolour, int numbers, bool reset)
        {
            ColourChooser(colour, Bcolour);
            Wrt(numbers);
            if (reset)
                res();
        }
        // A heap of methods used to shorten text colour switching.
        public static void gr() { Console.ForegroundColor = ConsoleColor.Green; }
        public static void dg() { Console.ForegroundColor = ConsoleColor.DarkGreen; }
        public static void c() { Console.ForegroundColor = ConsoleColor.Cyan; }
        public static void dc() { Console.ForegroundColor = ConsoleColor.DarkCyan; }
        public static void yl() { Console.ForegroundColor = ConsoleColor.Yellow; }
        public static void dy() { Console.ForegroundColor = ConsoleColor.DarkYellow; }
        public static void r() { Console.ForegroundColor = ConsoleColor.Red; }
        public static void b() { Console.ForegroundColor = ConsoleColor.Blue; }
        public static void m() { Console.ForegroundColor = ConsoleColor.Magenta; }
        public static void dm() { Console.ForegroundColor = ConsoleColor.DarkMagenta; }
        public static void w() { Console.ForegroundColor = ConsoleColor.White; }
        public static void bl() { Console.ForegroundColor = ConsoleColor.Black; }

        public static void Bgr() { Console.BackgroundColor = ConsoleColor.Green; }
        public static void Bdg() { Console.BackgroundColor = ConsoleColor.DarkGreen; }
        public static void Bc() { Console.BackgroundColor = ConsoleColor.Cyan; }
        public static void Bdc() { Console.BackgroundColor = ConsoleColor.DarkCyan; }
        public static void Byl() { Console.BackgroundColor = ConsoleColor.Yellow; }
        public static void Bdy() { Console.BackgroundColor = ConsoleColor.DarkYellow; }
        public static void Br() { Console.BackgroundColor = ConsoleColor.Red; }
        public static void Bb() { Console.BackgroundColor = ConsoleColor.Blue; }
        public static void Bm() { Console.BackgroundColor = ConsoleColor.Magenta; }
        public static void Bdm() { Console.BackgroundColor = ConsoleColor.DarkMagenta; }
        public static void Bw() { Console.BackgroundColor = ConsoleColor.White; }
        public static void Bbl() { Console.BackgroundColor = ConsoleColor.Black; }
        public static void res() { Console.ResetColor(); }
        public static void Main(string[] args)
        {
            Console.SetWindowSize(120, 35); // 120, 30 is standard. NOT real pixel size...
            // The beginning
            WrtL("yl", "", "\n  TTTTTT  RRRR   EEEEE     A       SSS    U   U   RRRR   EEEEE\r\n    T     R   R  E        A A     S       U   U   R   R  E\r\n    T     RRRR   EEEE    AAAAA     SSS    U   U   RRRR   EEEE\r\n    T     R  R   E      A     A       S   U   U   R  R   E\r\n    T     R   R  EEEEE A       A   SSS     UUU    R   R  EEEEE\r\n\r\n                CCCC       A     V       V   EEEEE\r\n               C    C     A A     V     V    E\r\n               C         AAAAA     V   V     EEEE\r\n               C    C   A     A     V V      E\r\n                CCCC   A       A     V       EEEEE\n\n", true);
            WrtL("TREASURE CAVE\nA game by Timothy Lennryd, 2023");
            WrtL("\nversion: 0.7");
            WrtL("w", "", "\n\nPlease press enter to start\n\n", true);

            //WrtL(Console.WindowWidth);
            //WrtL(Console.WindowHeight);

            AwaitKeyEnter();

            Console.Clear();
            WrtL("w", "", "Gaming controls info:", true);
            WrtL("\nWhenever the game stops and wait, it awaits you pressing enter most of the time, otherwise it will say\nwhat is expected.");
            WrtL("\nIn most cases your choices are to move up and down in menus with the arrow keys. You press enter to choose something,\nor spacebar to go back a step in the menus.");
            WrtL("Sometimes you can't go back, because a choice has to be made, or you can't press enter, because\nit's only an informational page.");
            WrtL("\nInside the cave things are a little different.\nYou can move in four directions with the arrow keys to navigate the visual map.\nThe spacebar opens up a pause menu, and enter is used to enter pathways only.");
            WrtL("w", "", "\nNow please press enter again to continue", true);

            AwaitKeyEnter();

            // Puts together all caves and gear into objects to fetch later.
            Cave.InitiateAllCaves();
            Weapon.CreateAllWeapons();
            Armor.CreateAllArmor();
            Item.CreateAllItems();
            Gear.BuildProbabilityGearLists();

            // randomizes a new cave.
            do
            {
                startingCave = randomize.Next(Cave.Caves.Count);
            }
            while (startingCave == 11); // Cave 11 is closed around the lower entrance, so... not that one as the first.
            // Puts the cave in the "used" list. Will be checked through in a method.
            Cave.usedCaveRooms.Add(startingCave);

            // Places cave doors on the first map.
            Cave.FixEntranceNumbers(startingCave);
            Cave.CreateEntrances(startingCave);
            // Lessens minimalNeedForEntrances after first cave.
            minimalNeedForEntrances = 2;

            // caveMap[15,7] = game start position in the big cave. Places the entrance cave on that spot in the caveMap array.
            Cave.caveMap[Cave.BigCavePosition[0], Cave.BigCavePosition[1]] = Cave.Caves[startingCave];

            // The player will choose to make a new character or load one.
            Menu.NewHeroOrLoadMenu();

            // And the gaming starts.
            GameMenu();
        }
        public static void CreateStartCharacter()
        {
            Console.Clear();

            You = new Player();

            WrtL("Who are you?");
            Wrt("\nLet's start with a ");
            Wrt("w", "", "name", true);
            Wrt(" and a ");
            Wrt("w", "", "warrior type", true);
            Wrt(". ");
            Wrt("yl", "", "But don't worry", true);
            WrtL(", both can be changed before you commit.");

            do
            {
                if (You.name == "")
                {
                    WrtL("dy", "", "\nNot buying that. Really now, what's your name?", true);
                }

                You.name = "";
                Wrt("\nMy name is: ");
                You.name = Console.ReadLine();

                if (!ReasonableName(You.name))
                {
                    You.name = "";
                }
            }
            while (You.name == "");

            if (You.name == "T")
            {
                // Test mode. Randomizes a warrior for you.
                testmode = true;

                You = RandomizeWarrior();

                Console.Clear();

                WrtL("Turns out, this is you:\n");

                Menu.PresentFinalizedPlayer(You);
                WrtL("\nHave fun.\n");
            }
            else
            {
                You.name = CapitalizeFirstLetter(You.name);

                WrtL("\n" + You.name + " huh? A name as good as any.\n");
                WrtL("Now...\n");

                AwaitKeyEnter();

                Menu.UpdateWarriorType(You);
                Hero.UpdateWarriorStatsBeginning(You);

                Console.Clear();

                Hero.PresentPlayerBaseSkills(You);

                AwaitKeyEnter();

                Menu.CharacterBuilderMenu(You);

                Console.Clear();
                WrtL("Would you like to save this character?");
                WrtL("If you do, you can reuse all these settings next time you play, without having to enter all the data again.\n");
                WrtL("Press Enter to save " + You.name + ".\nPress Spacebar to continue without saving this character.");

                var keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    WrtL("w", "", "Saving " + You.name + "...", false);

                    PrepareHeroForSave(You);

                    Thread.Sleep(500);
                    WrtL("...");
                    Thread.Sleep(500);
                    WrtL(" -.. - .~ * .. =# ¤ . - - !");
                    Thread.Sleep(500);
                    WrtL("\nThere! Saved!\n");
                    AwaitKeyEnter();
                    Console.Clear();
                }
                else if (keyPressed.Key == ConsoleKey.Spacebar)
                {
                    Console.Clear();
                    WrtL("Not saving.\n");
                    WrtL("Let's continue!\n");
                    AwaitKeyEnter();
                    Console.Clear();
                }
            }

            FinalizePlayerStuff(You);
        }
        public static void FinalizePlayerStuff(Hero You)
        {
            partyGear.Add(Gear.GearWithName("Small Health Potion"));
            partyGear.Add(Gear.GearWithName(warriorTypeStartingItems[You.warriorTypeIndex, 0]));
            SortPartyGear();
            partyGear.Add(Gear.GearWithName(warriorTypeStartingItems[You.warriorTypeIndex, 1]));
            SortPartyGear();

            WrtL("w","", "You have these items in your inventory:\n", true);
            for (int i = 0; i < partyGear.Count; i++)
            {
                WrtL(partyGear[i].name);
            }
            WrtL("");

            Party.Add(You);
            maximumPartyEnlisting++;
        }
        public static Hero RandomizeWarrior()
        {
            Hero warrior;

            int randWarriorType = randomize.Next(6);
            // Creates a class Hero(subclass) warrior, randomizes their stats.
            // "Bounty Hunter", "Barbarian", "Ranger", "Rogue", "Duelist", "Fighter"
            switch (randWarriorType)
            {
                case 0:
                    warrior = new BountyHunter();
                    break;
                case 1:
                    warrior = new Barbarian();
                    break;
                case 2:
                    warrior = new Ranger();
                    break;
                case 3:
                    warrior = new Rogue();
                    break;
                case 4:
                    warrior = new Duelist();
                    break;
                case 5:
                    warrior = new Fighter();
                    break;
                default:
                    warrior = new Fighter();
                    break;
            }

            return warrior;
        }

        // This is the main menu, where the player enters the cave or the town, or quits the game. One only gets out of this menu if quitting the game.
        public static void GameMenu()
        {
            while (true)
            {
                // StartMenu will return a choice from the user.
                int choice = Menu.StartMenu();

                switch (choice)
                {
                    case 0:
                        // Start exploring the cave.
                        EnterCave();
                        break;
                    case 1:
                        // You're going to the village.
                        GoToTheVillage();
                        break;
                    case 2:
                        // Let's do stuff with the party.
                        // NOTE! Unsure if this boolean is doing what it should, which is to keep track of when something has actually been changed, and that should affect time passing or not.
                        bool doneStuff = false;

                        while (true)
                        {
                            int warriorIndex = Menu.ChoosePartyMemberMenu(0, "Which party member needs something?");
                            if (warriorIndex == -1)
                            {
                                break;
                            }
                            else
                            {
                                string[] menuOptions = new string[] { "Use Item", "Equip Gear" };
                                int markedOption = 0;

                                while (true)
                                {
                                    Console.Clear();
                                    Console.CursorVisible = false;

                                    WrtL("What does " + Party[warriorIndex].name + " need?\n");

                                    Menu.MenuPrinter(menuOptions, markedOption, "  ");

                                    WrtL("\n");
                                    Hero.TakeAlookAt(Party[warriorIndex]);

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
                                        break;
                                    }
                                    else if (keyPressed.Key == ConsoleKey.Enter)
                                    {
                                        if (markedOption == 0)
                                        {
                                            int itemUse = Menu.UseItemMenu(Party[warriorIndex]);
                                            if (itemUse == 1)
                                                doneStuff = true;

                                            continue;
                                        }
                                        else
                                        {
                                            int GearResult = Menu.EquipGearMenu(Party[warriorIndex]);
                                            if (GearResult == 0)
                                            {
                                                // Player backed out of menu without equipping anything.
                                                continue;
                                            }
                                            else
                                            {
                                                // Player did equip something.
                                                doneStuff = true;
                                                Hero.UpdateWarriorStats(Party[warriorIndex]);
                                                continue;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (doneStuff)
                            TimeUnitsPass(1, true);

                        break;
                    case 3:
                        // You gotta go if you gotta go.
                        EndGame();
                        break;
                }
            }
        }
        // If the player chooses to go into the village.
        public static void GoToTheVillage()
        {
            while(true)
            {
                int choice = Menu.InTheVillage();

                switch (choice)
                {
                    case -1:
                        // Back, no choice made.
                        return;
                    case 0:
                        // Market Street
                        Shop();
                        break;
                    case 1:
                        // The Tavern 'Mead Maiden'
                        Recruit();
                        break;
                    case 2:
                        // Drowzy Donkey Inn
                        AtTheInn();
                        break;
                }
            }
        }
        // If the player chooses the market street.
        public static void Shop()
        {
            Console.Clear();

            WrtL("yl", "", "   MARKET STREET", true);
            WrtL("\nYou have a decent range of shops, stands and rolling merchants here. It's hardly buzzling these days, but there's\nalways some people around here despite the tiring rain, grey weather and wet pavestone.");
            
            AwaitKeyEnter();
            Menu.ShopMenu();
        }
        // Here is the recruiting done. Randomizes an amount of warriors at the tavern, presents them to the player one by one, letting them choose to hire or not.
        public static void Recruit()
        {
            Console.Clear();
            WrtL("yl", "", "   THE TAVERN 'MEAD MAIDEN'", true);

            WrtL("\nYou enter the tavern.");
            WrtL("You need warriors to join you for entering the cave.\n");

            AwaitKeyEnter();

            int amountOfWarriorsHereToday = 1 + randomize.Next(3);

            for (int i = 0; i < amountOfWarriorsHereToday; i++)
            {
                Console.Clear();

                Wrt("Available Branzen: ");
                WrtL("yl", "", Branzen + "\n", true);

                Hero warrior = RandomizeWarrior();

                if (i == 0)
                    WrtL("After some chit chatting around, you find a warrior.\n");
                else
                {
                    WrtL("Further chit chatting results in yet another warrior.\n");
                    AwaitKeyEnter();
                }

                Hero.Present(warrior);

                Wrt("\n\n" + CapitalizeFirstLetter(warrior.pronoun[0]) + " wants ");
                Wrt("dy", "", warrior.cost, true);
                Wrt(" Br to join your party.\n\n");

                if (Branzen >= warrior.cost)
                {
                    WrtL("Is that agreeable with you?\n");
                    WrtL("Enter to agree");
                    WrtL("Spacebar to deny");

                    var keyPressed = Console.ReadKey();

                    if (keyPressed.Key == ConsoleKey.Spacebar)
                    {
                        WrtL("\nYou excuse yourself and keep looking...\n");
                    }
                    else if (keyPressed.Key == ConsoleKey.Enter)
                    {
                        Branzen -= warrior.cost;
                        Party.Add(warrior);
                        maximumPartyEnlisting++;

                        WrtL("\n" + warrior.name + " joined your party!");
                        Wrt("\nYou have ");
                        Wrt("yl", "", Branzen, true);
                        Wrt(" Branzen left.\n");
                    }
                }
                else
                {
                    WrtL("You don't have that much Branzen right now.");
                    WrtL("You excuse yourself and keep looking...\n");
                }
                AwaitKeyEnter();
            }

            Console.Clear();
            WrtL("The smell of booze and tobacco is taking its toll.\nYou go back outside to get some air.\n");

            TimeUnitsPass(1, true);

            AwaitKeyEnter();
        }
        // If the player chooses to go to the inn. Checks for specific ailments in your party that disqualifies you for sleeping there.
        static void AtTheInn()
        {
            Console.Clear();

            WrtL("yl", "", "DROWZY DONKEY INN", true);
            WrtL("\nCosts 100 Branzen per head for a good meal and a bed. So don't carry around your dead mates' heads...");

            bool bleeding = false;
            bool poisoned = false;
            Hero bleedingWarrior = null;
            Hero poisonedWarrior = null;

            for (int i = 0; i < Party.Count; i++)
            {
                if (HasAilment(Party[i], ailments[0]))
                {
                    bleeding = true;
                    bleedingWarrior = Party[i];
                    break;
                }
                if (HasAilment(Party[i], ailments[9]))
                {
                    poisoned = true;
                    poisonedWarrior = Party[i];
                    break;
                }
            }

            if (bleeding && poisoned)
            {
                WrtL("\nYour group look to be in quite bad shape, looks like poisoning and heavy bleeding and whatnot... Some of you will probably die during the night if you don't tend to them first.");
                WrtL("It will look really bad in our book if we have guests dying sleeping here... We won't have it.");
                WrtL("Please leave. Hope you can save them. Welcome back...");

                AwaitKeyEnter();
                return;
            }
            else if (bleeding)
            {
                WrtL("\nYou have a heavily bleeding warrior among you. " + CapitalizeFirstLetter(bleedingWarrior.pronoun[0]) + " will die during the night if you don't fix " + bleedingWarrior.pronoun[1] + "...");
                WrtL(CapitalizeFirstLetter(bleedingWarrior.pronoun[0]) + " will bleed on the sheets! We won't have it.");
                WrtL("Please leave. Hope you can save " + bleedingWarrior.pronoun[1] + ". Welcome back...");

                AwaitKeyEnter();
                return;
            }
            else if (poisoned)
            {
                WrtL("\nYou have a poisoned warrior among you. " + CapitalizeFirstLetter(poisonedWarrior.pronoun[0]) + " will die during the night if you don't cure " + poisonedWarrior.pronoun[1] + "...");
                WrtL("It will look really bad in our book if we have a guest dying sleeping here... We won't have it.");
                WrtL("Please leave. Hope you can save " + poisonedWarrior.pronoun[1] + ". Welcome back...");

                AwaitKeyEnter();
                return;
            }

            if (Branzen >= 100 * Party.Count)
            {
                WrtL("Are you done for the day and ready to pay?\n");
                WrtL("Enter to pay and stay.");
                WrtL("Spacebar to wait and leave.");

                var keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.Spacebar)
                {
                    Console.Clear();

                    WrtL("You're not weary or hungry yet. Let's go.");
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    Branzen -= 100 * Party.Count;

                    Console.Clear();

                    int timeunitsToPass = ((24 - hourOfDay + 7) * 4) - timeUnits;

                    TimeUnitsPass(timeunitsToPass, false);

                    WrtL(hourOfDay);
                    WrtL(timeOfDay);

                    WrtL("You all get a good meal and a good nights sleep, feeling fully restored the next morning.");
                    WrtL("You're awakened at seven in the morning. The sun has just risen.");

                    for (int i = 0; i < Party.Count; i++)
                    {
                        Party[i].healthpoints = Party[i].maxHealth;
                        Party[i].stamina = Party[i].maxStamina;
                        Party[i].composure = Party[i].baseComposure;
                    }
                }
            }
            else
            {
                WrtL("\nSadly, you can't afford that all of you spend your night here...");
            }

            AwaitKeyEnter();
        }

        // This method is only for when entering the cave, not for moving between cave rooms.
        static void EnterCave()
        {
            Console.Clear();

            WrtL("You take a good look at your party.\n");

            // AwaitKeyEnter();

            LookAtParty();

            WrtL("Then you leave for the cave.");

            AwaitKeyEnter();

            Console.Clear();

            TimeUnitsPass(2, false);

            WrtL("Walking through the dead forest in the thick mists, uphill where the mountain starts peaking out from the ground.");
            WrtL("Getting lost here would almost be a certainty without the map with the clear land marks...");

            AwaitKeyEnter();

            WrtL("\nBut it's not very far, and following your map makes it a short walk of about thirty minutes.\n");

            AwaitKeyEnter();

            WrtL("You reach the cave entrance. Its gaping darkness and grotesque, jagged edges truly make it look like\na portal to the other side...");
            WrtL("Nevertheless, you are here to explore it.\nSo you enter.");

            Game.Wrt("\n\nTime: ");
            Game.WrtL("yl", "", Game.Clock(), true);

            AwaitKeyEnter();

            if (Cave.currentCaveNr != startingCave)
                Cave.currentCaveNr = startingCave;

            inTheCave = true;

            // Starts the walking around!
            Cave.NavigateCave(Cave.Caves[startingCave]);
        }

        // A method for sorting gear in the players inventory, placing items that are the same after each other.
        // Only works if run once every time an item is added to the inventory.
        public static void SortPartyGear()
        {
            if (partyGear.Count == 1)
                return;

            if (partyGear.Count > 2)
            {
                Gear lastAdded = partyGear[partyGear.Count-1];

                for (int i = 0; i < partyGear.Count; i++)
                {
                    if (lastAdded.Id <= partyGear[i].Id)
                    {
                        partyGear.Insert(i, lastAdded);
                        partyGear.RemoveAt(partyGear.Count-1);
                        break;
                    }
                }
            }
            else 
            {
                // Count is 2...
                if (partyGear[0].Id > partyGear[1].Id)
                {
                    Gear smallerIdgear = partyGear[1];
                    partyGear[1] = partyGear[0];
                    partyGear[0] = smallerIdgear;
                }
            }
        }
        // Method to calculate an average level of the party of warriors.
        public static void calcAveragePartyLevel()
        {
            if (Party.Count == 1)
            {
                averagePartyLevel = Party[0].level;
                return;
            } 

            int sumOfWarriorLevels = 0;

            for (int i = 0; i < Party.Count; i++)
            {
                sumOfWarriorLevels += Party[i].level;
            }

            averagePartyLevel = sumOfWarriorLevels / Party.Count;
        }
        // Method for time passing, using timeUnits, updating time of day, day name and so on.
        // Is capable of running time with or without ailments affecting the warriors. Necessary in certain places to make the game not too unforgiving.
        public static void TimeUnitsPass(int value, bool ailmentsShouldRun)
        {
            // Each time unit is 15 min. 4 time units is therefore 1 hour.
            //timeUnits = 0;
            //hourOfDay = 8;
            //timeOfDay = "morning";
            //hourNames = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve" };
            //hourAmountOfTheDay = 24;
            //morning = 7;
            //daytime = 11;
            //evening = 16;
            //sunset = 18;
            //nighttime = 19;
            //weekDay = 1;
            //amountOfWeekDays = 7;
            //dayOfWeek = "verday";
            //weekDayNames = { "verday", "nirday", "ornday", "goraday", "saraday", "lagarday", "brimday" };

            while (value != 0)
            {
                timeUnits++;

                for (int i = 0; i < Party.Count; i++)
                {
                    if (Party[i].recoverFromPoisoningCounter > 0)
                        Hero.RecoverFromPoisoning(Party[i]);
                }

                if (ailmentsShouldRun)
                {
                    // Any warriors killed by ailments before this time unit passed are damned to be dead forever now.
                    RemoveDeadWarriors();
                    // Any warriors killed by ailments this time unit have until the next time unit passes to be saved. Nothing further need be said after this, the ailment methods make
                    // sure to place dead warriors in the right lists and comments it.
                    RunPartyAilments();
                    // HOWEVER. If ALL warriors are dead after RunPartyAilments, we have to know...
                    CheckIfAllDied();
                }

                string oldTime = timeOfDay;

                if (timeUnits == 4)
                {
                    hourOfDay++;
                    if (hourOfDay >= morning && hourOfDay < daytime)
                        timeOfDay = "morning";
                    else if (hourOfDay >= daytime && hourOfDay < evening)
                        timeOfDay = "daytime";
                    else if (hourOfDay >= evening && hourOfDay < sunset)
                        timeOfDay = "evening";
                    else if (hourOfDay >= sunset && hourOfDay < nighttime)
                        timeOfDay = "sunset";
                    else if (hourOfDay >= nighttime && hourOfDay < daytime)
                        timeOfDay = "nighttime";

                    timeUnits = 0;

                    if (hourOfDay > 23)
                    {
                        hourOfDay = 0;
                        weekDay++;

                        if (weekDay > amountOfWeekDays)
                        {
                            weekDay = 1;
                        }

                        dayOfWeek = weekDayNames[weekDay - 1];
                    } 
                }
                if ((oldTime != timeOfDay) && inTheCave)
                {
                    bool pressEnter = false;
                    if (timeOfDay == "sunset")
                    {
                        Console.Clear();
                        WrtL("The sun is setting once again. The monsters are going to feel the surge of the night in an hour.");
                        WrtL("Hurry out, or prepare for some fierce fighting...");
                        pressEnter = true;
                    }
                    else if (timeOfDay == "nighttime")
                    {
                        Console.Clear();
                        WrtL("Nighttime is here. You can hear monsters in the deepest darkness howl in excitement.");
                        WrtL("Hurry out... Any battle now will be with creatures at their peak.");
                        pressEnter = true;
                    }
                    else if (timeOfDay == "morning")
                    {
                        Console.Clear();
                        WrtL("Morning has come. A few moaning creatures can be heard out there in the dark, feeling how their power wanes.");
                        WrtL("They will still be dangerous, and try to purge your existence from this cave, but now is your best chance to defeat them.");
                        pressEnter = true;
                    }
                    if (pressEnter)
                    {
                        AwaitKeyEnter();
                    }
                }
                
                value--;
            }

            if (inTheCave)
                Cave.samePathway = false;
        }
        // Method to remove warriors that died during a battle and wasn't revived. After a battle it's too late.
        public static void RemoveDeadWarriors()
        {
            bool anyDeath = false;

            for (int i = 0; i < Party.Count; i++)
            {
                if (Party[i].healthpoints <= 0)
                {
                    if (Party.Count > 1 && Party[i].alltimePartyId != 1)
                    {
                        WrtL(Party[i].name + " is dead, and too much time has passed before any resurrection attempts have been made...");
                        WrtL(CapitalizeFirstLetter(Party[i].pronoun[0]) + "'s gone. You have to leave " + Party[i].pronoun[2] + " body behind, it's slowing you down too much.");

                        if (!partyMembersInPeril.Remove(Party[i].alltimePartyId))
                        {
                            WrtL("alltimePartyId not found in partyMembersInPeril-List!");
                            AwaitKeyEnter();
                        }
                        Party.RemoveAt(i);
                        i--;

                        if (!anyDeath)
                            anyDeath = true;
                    }
                    else
                    {
                        WrtL("You have died from your ailments, nothing could be done. There wasn't time.");

                        if (Party.Count > 1)
                        {
                            WrtL("Your death pulls the morale through the floor and the others panic and scatter into the darkness.");
                            WrtL("This party is over...");
                        }

                        AwaitKeyEnter();
                        EndGameByDeath(0);
                    }
                }
            }
            if (anyDeath)
            {
                Hero.ReCalculateCurrentPartyId();
                calcAveragePartyLevel();
            }
        }
        // Method that goes through the list om party members that has ailments.
        public static void RunPartyAilments()
        {
            bool first = true;
            int count = 0;

            if (partyMembersInPeril.Count > 0)
            {
                for (int i = 0; i < partyMembersInPeril.Count; i++)
                {
                    for (int j = 0; j < Party.Count; j++)
                    {
                        if (Party[j].alltimePartyId == partyMembersInPeril[i])
                        {
                            if (first)
                            {
                                Console.Clear();
                                WrtL("w", "", "Ailments of your warriors are affecting them over time. Try to tend to them before they die.\n", true);

                                first = false;
                            }

                            count += GoThroughAilments(Party[i]);

                            if (count == 5)
                            {
                                Console.Clear();

                                count = 0;
                            }
                        }
                    }
                }
            }
        }
        // The method that runs every specific ailments' symptoms. Loops as many times as the warrior's ailment count specifies.
        public static int GoThroughAilments(Hero warrior)
        {
            // "bleeding", "fatigue", "berserk", "unconscious", "stunned", "immobilized", "confused", "panicking", "dying", "poisoned"
            int count = 0;

            if (warrior.ailments.Count == 0)
                return count;

            for (int i = 0; i < warrior.ailments.Count; i++)
            {
                count++;
                // I take them in order of lethality.
                if (warrior.ailments[i] == "bleeding")
                    AilmentBleeding(warrior);
                else if (warrior.ailments[i] == "poisoned")
                    AilmentPoisoned(warrior);
                else if (warrior.ailments[i] == "fatigue")
                    AilmentFatigue(warrior);
                else if (warrior.ailments[i] == "berserk")
                    AilmentBerserk(warrior);
                //else if (warrior.ailments[i] == "unconscious")
                //    AilmentUnconscious(warrior);
                //else if (warrior.ailments[i] == "immobilized")
                //    AilmentImmobilized(warrior);
                //else if (warrior.ailments[i] == "stunned")
                //    AilmentStunned(warrior);
                //else if (warrior.ailments[i] == "confused")
                //    AilmentConfused(warrior);
                WrtL("");
            }
            return count;
        }
        // Method to tell wether a specific warrior has a specific ailment.
        public static bool HasAilment(Hero warrior, string ailmentInQuestion)
        {
            for (int i = 0; i < warrior.ailments.Count; i++)
            {
                if (warrior.ailments[i] == ailmentInQuestion)
                    return true;
            }
            return false;
        }
        // Method to remove specific ailment from specific warrior if it has it.
        public static bool RemoveIfHasAilment(Hero warrior, string ailmentInQuestion)
        {
            if (HasAilment(warrior, ailmentInQuestion))
            {
                RemoveAilment(warrior, ailmentInQuestion);
                return true;
            }
            return false;
        }
        // Method that tries to remove specific ailment from specific warrior and if unsuccesful prints an error message.
        public static void RemoveAilment(Hero warrior, string ailmentInQuestion)
        {
            if (!warrior.ailments.Remove(ailmentInQuestion))
            {
                WrtL(ailmentInQuestion + " not found in warrior's ailment list!");
                AwaitKeyEnter();
            }
            if (warrior.ailments.Count == 0)
            {
                if (!partyMembersInPeril.Remove(warrior.alltimePartyId))
                {
                    WrtL("alltimePartyId not found in partyMembersInPeril-List!");
                    AwaitKeyEnter();
                }
            }
        }
        // Method to add a specific warrior to the list of sick people, checks that they're not already on it first.
        public static void AddToPerilListIfAbsent(int warriorId)
        {
            if (partyMembersInPeril.Count > 0)
            {
                for (int i = 0; i < partyMembersInPeril.Count; i++)
                {
                    if (partyMembersInPeril[i] == warriorId)
                    {
                        return;
                    }
                }
            }

            partyMembersInPeril.Add(warriorId);
        }
        // Method for afflicting warrior symptoms of ailment bleeding and giving the player some information about it.
        public static void AilmentBleeding(Hero warrior)
        {
            warrior.healthpoints--;

            if (warrior.healthpoints > 0)
            {
                WrtL("yl" ,"" , warrior.name + " is bleeding heavily!", true);
                WrtL(CapitalizeFirstLetter(warrior.pronoun[0]) + "'s lost another health point!\n");
            }
            else
            {
                warrior.healthpoints = 0;
                WrtL("yl", "", warrior.name + " is bleeding heavily, and just bled to death!", true);
                WrtL("Only magic can revive " + warrior.pronoun[1] + " now...\n");

                if (!HasAilment(warrior, ailments[8]))
                    warrior.ailments.Add(ailments[8]); // dying...
                // We need not add this warrior to the peril list as they are there already from heavily bleeding.
                // The bleeding ailment won't go away until they are revived (with something magical), and it will make sure that physical revival methods won't work.
            }
            AwaitKeyEnter();
        }
        // Method for afflicting warrior symptoms of ailment fatigue and giving the player some information about it.
        public static void AilmentFatigue(Hero warrior)
        {
            WrtL("yl", "", warrior.name + " suffers from fatigue!", true);

            int lostStamina = warrior.stamina / 3;
            if (lostStamina < 1)
                lostStamina = 1;
            warrior.stamina -= lostStamina;
            if (warrior.stamina <= 0)
                warrior.stamina = 0;

            WrtL(CapitalizeFirstLetter(warrior.pronoun[2]) + " stamina has diminished by " + lostStamina + "!\n");
            AwaitKeyEnter();
        }
        // Method for afflicting warrior symptoms of ailment berserk and giving the player some information about it.
        public static void AilmentBerserk(Hero warrior)
        {
            WrtL("yl", "", warrior.name + " is going berserk!", true);

            warrior.composure--;

            if (warrior.composure <= 0)
            {
                warrior.composure = 0;

                if (!HasAilment(warrior, ailments[7]))
                    warrior.ailments.Add(ailments[7]); // panicking

                warrior.ailments.Remove(ailments[2]); // berserk

                warrior.strength -= warrior.berserkValue;
                if (warrior.strength <= 0)
                    warrior.strength = 1;

                warrior.speed -= warrior.berserkValue;
                if (warrior.speed <= 0)
                    warrior.speed = 1;

                warrior.berserkValue = 0;

                WrtL("It's been eating away at " + warrior.pronoun[2] + " composure, and now " + warrior.pronoun[0] + "'s lost it!");
                WrtL("The berserking has switched to panicking!");
            }
            else
                WrtL("It drains " + warrior.pronoun[2] + " composure.");

            AwaitKeyEnter();
        }
        // Method for afflicting warrior symptoms of ailment poisoning and giving the player some information about it.
        public static void AilmentPoisoned(Hero warrior)
        {
            WrtL("yl", "", warrior.name + " is poisoned!", true);
            // All strenght enhancing equipment and potions are nullified when poisoned.
            warrior.strength -= warrior.extraStrengthPoints;
            warrior.extraStrengthPoints = 0;
            warrior.strength--;

            if (HasAilment(warrior, "berserk"))
            {
                warrior.strength--;
                warrior.stamina--;

                WrtL("Being berserk is weakening " + warrior.pronoun[1] + " faster!");
            }

            if (warrior.strength < 1)
                warrior.strength = 1;

            if (warrior.strength == 1 && warrior.healthpoints < 3)
            {
                warrior.stamina--;
                // Since equipping a new weapon most likely resets the attackDice, this must be done in several places here.
                warrior.attackDice = warrior.attackDice / 2;

                if (warrior.healthpoints == 1)
                {
                    WrtL("It's gone too long without an antidote... " + CapitalizeFirstLetter(warrior.pronoun[0]) + "gurgles and gasps, then legs give up and " + warrior.pronoun[0] + "collapses.\n");
                    WrtL(CapitalizeFirstLetter(warrior.pronoun[0]) + " will need an antidote even if brought back, the poison is still in the body...");
                }
                else if (warrior.healthpoints == 2)
                {
                    WrtL("It's gone too far! " + CapitalizeFirstLetter(warrior.pronoun[0]) + " will die very soon if " + warrior.pronoun[0] + " can't get an antidote right now!\n");
                }
                
                warrior.healthpoints--;
            }
            else
            {
                if (warrior.strength >= 3)
                    WrtL("It's slowly weakening " + warrior.pronoun[1] + ".");

                if (warrior.strength < warrior.strength / 2)
                {
                    warrior.stamina--;

                    if (warrior.strength >= 3) // half strength may actually be this low for some warrior types.
                        WrtL("It has started to eat away at " + warrior.pronoun[2] + " stamina...");
                }

                if (warrior.strength < 3)
                {
                    warrior.healthpoints -= warrior.healthpoints / 2;
                    // Since equipping a new weapon most likely resets the attackDice, this must be done in several places here.
                    warrior.attackDice = warrior.attackDice / 2;

                    WrtL("It's not long before " + warrior.pronoun[0] + " dies! Health is plummeting now!");
                }
                else if (warrior.strength == 1)
                {
                    // Since equipping a new weapon most likely resets the attackDice, this must be done in several places here.
                    warrior.attackDice = warrior.attackDice / 2;

                    if (warrior.healthpoints > 3)
                    {
                        warrior.healthpoints = 3;
                    }
                    warrior.healthpoints--;

                    WrtL("It's critical! " + CapitalizeFirstLetter(warrior.pronoun[2]) + " health is really bad and gets worse by the minute!");
                }
            }

            if (warrior.stamina <= 0)
                warrior.stamina = 0;

            if (warrior.healthpoints <= 0)
            {
                warrior.healthpoints = 0;
                if (!HasAilment(warrior, ailments[8]))
                    warrior.ailments.Add(ailments[8]); // dying
                // But the poison stays...
            }

            AwaitKeyEnter();
        }

        // Method for looking at all your warriors' stats.
        public static void LookAtParty()
        {
            // Console.Clear();
            for (int i = 0; i < Party.Count; i++)
            {
                Hero.TakeAlookAt(Party[i]);
                WrtL("");

                if (i % 2 == 1 && i < Party.Count - 1)
                {
                    Wrt("w", "", "Press Enter", true);
                    WrtL(" for next page of warriors.");
                    AwaitKeyEnter();
                    Console.Clear();
                }
            }
        }
        // Method for looking at all the gear that the party has collected/looted or bought.
        public static void LookAtGear()
        {
            if (partyGear.Count == 0)
            {
                Console.Clear();
                WrtL("You have no gear in your inventory right now.");
                WrtL("Gear that is equipped on warriors are not on this list, but can be viewed on the warrior in other menus.\n");
                return;
            }

            List<string> menuOptions = new List<string>();
            List<string> optionDescriptions = new List<string>();
            List<int> optionOriginalIndex = new List<int>();

            string ItemNameAndAmount;
            int amount = 0;

            for (int i = 0; i < partyGear.Count; i++)
            {
                // For this to work as supposed requires that the list of party gear is sorted by type, more easily done by Id-number, every time something is added to that list.
                Gear item = partyGear[i];
                int nextItemId = -1;

                if (i + 1 < partyGear.Count)
                    nextItemId = partyGear[i + 1].Id;

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

                WrtL("Gear that is equipped on warriors are not on this list, but can be viewed on the warrior in other menus.");
                WrtL("This is just a view of your inventory, you can't use or equip anything from here.\n");

                Menu.MenuPrinter(menuOptions, markedOption, "  ");

                WrtL("\n\n" + optionDescriptions[markedOption] + "\n");

                Gear.PresentGearData(partyGear[optionOriginalIndex[markedOption]]);

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
                    break;
                else if (keyPressed.Key == ConsoleKey.Enter)
                    break;
            }
        }
        // Method to rest the party while in the cave. Presents a list of possible things to do while resting. Increases risk of being assaulted by monsters the longer one chooses to rest.
        public static void RestParty()
        {
            Console.Clear();

            if (Party.Count > 1) 
                WrtL("The team stops, sits down to rest.");
            else
                WrtL("You stop and sit down to rest.");

            AwaitKeyEnter();

            int restTime = 0;
            int risk;
            Cave thisCave = Cave.caveMap[Cave.BigCavePosition[0], Cave.BigCavePosition[1]];
            Cave room = Cave.Caves[thisCave.id];
            double monsterRisk = room.riskOfMonsters/2;
            int choice;
            bool keepResting = true;

            while (keepResting)
            {
                Console.Clear();
                WrtL("Monster risk = " + monsterRisk);

                AwaitKeyEnter();

                risk = randomize.Next(100);

                if (room.MonsterList.Count > 0 && (100 * monsterRisk) > risk)
                {
                    // A monster appeared and battle must occur!
                    int randoM = randomize.Next(room.amountOfMonsters);
                    Monster monster = room.MonsterList[randoM];
                    Monster.AdaptMonsterLevel(monster);

                    int result = Cave.UnexpectedAttack(room, monster);

                    if (result == 0)
                    {
                        // Monster still alive.
                        Cave.Battle(room, monster, true); 
                    }
                    //else monster died. Will have been talked about and made real from the Battle method. Could happen as defensive attacks are a thing and could kill the monster
                    //before a regular attack phase need to start.

                    // Results are in. Remove any eventual dead warriors from Party.
                    Cave.GoThroughBattleResult();

                    // Erase an eventual red X left.
                    Cave.EraseRedOnMap(room);
                    // Put a new X on last battle spot.
                    room.caveFloor[Cave.PartyPosition[0], Cave.PartyPosition[1]] = 6;

                    Console.Clear();

                    WrtL("The commotion of the battle will attract more monsters. This site is too dangerous to stay at any longer.");
                    if (Party.Count > 1)
                        WrtL("The party gets ready to move again.\n");
                    else
                        WrtL("You get ready to move again.\n");

                    keepResting = false;

                    AwaitKeyEnter();
                }
                else
                {
                    restTime++;
                    monsterRisk += room.riskOfMonsters / 2;

                    for (int i = 0; i < Party.Count; i++)
                    {
                        Party[i].stamina += 2;
                        if (Party[i].stamina > Party[i].maxStamina)
                            Party[i].stamina = Party[i].maxStamina;

                        if (restTime % 2 == 0)
                        {
                            if (Party[i].composure < Party[i].baseComposure)
                                Party[i].composure++;
                        }
                    }

                    if (Party.Count > 1)
                    {
                        Wrt("Everybody gets + 2 stamina");
                    }
                    else
                    {
                        Wrt("You get + 2 stamina");
                    }
                    if (restTime % 2 == 0)
                    {
                        Wrt(" and 1 composure");
                    }
                    WrtL(".");

                    AwaitKeyEnter();

                    bool nothingDoneYet = true;

                    while (nothingDoneYet)
                    {
                        choice = Menu.RestMenu();
                        int warriorIndex;

                        switch (choice)
                        {
                            case 0:
                                // Lets take a look at all the stuff you've got.
                                LookAtGear();
                                continue;
                            case 1:
                                // Lets take a closer look at the party members.
                                Console.Clear();
                                LookAtParty();
                                AwaitKeyEnter();
                                continue;
                            case 2:
                                warriorIndex = Menu.ChoosePartyMemberMenu(0, "Which party member needs to use an item?");
                                if (warriorIndex == -1)
                                    continue;
                                // NOTE! Use Item menu can be backed out of without picking an item, I have to look into what happens then.
                                int ItemResult = Menu.UseItemMenu(Party[warriorIndex]);
                                if (ItemResult == 0)
                                {
                                    // Player backed out of menu without using an item.
                                    continue;
                                }
                                else
                                {
                                    // Player did use an item.
                                    nothingDoneYet = false;
                                    break;
                                }
                            case 3:
                                warriorIndex = Menu.ChoosePartyMemberMenu(0, "Which party member's equipment do you want to change?");
                                if (warriorIndex == -1)
                                    continue;
                                int GearResult = Menu.EquipGearMenu(Party[warriorIndex]);
                                if (GearResult == 0)
                                {
                                    // Player backed out of menu without equipping anything.
                                    continue;
                                }
                                else
                                {
                                    // Player did equip something.
                                    Hero hero = Party[warriorIndex];
                                    Hero.UpdateWarriorStats(hero);
                                    nothingDoneYet = false;
                                    break;
                                }
                            case 4:
                                Console.Clear();
                                if (Party.Count > 1)
                                    WrtL("The party just need to rest some more...\n");
                                else
                                    WrtL("You just need to rest some more...\n");

                                nothingDoneYet = false;

                                AwaitKeyEnter();
                                break;
                            case 5:
                                Console.Clear();
                                WrtL("It's dangerous to stay in one place for too long, and resting makes you less vigilant as well.");
                                if (Party.Count > 1)
                                    WrtL("The party quickly packs up to get moving again.");
                                else
                                    WrtL("You quickly pack up to get moving again.");

                                nothingDoneYet = false;
                                keepResting = false;

                                AwaitKeyEnter();
                                break;
                        }
                    }
                    // This runs here at the end, because otherwise dead teammates have no time to be saved.
                    TimeUnitsPass(1, true);
                    // NOTE! It feels unfair that one only has time to save one dying warrior if several are dying. Maybe? As soon as an item is used above code runs, and the rest dies forever,
                    // even if the player has the means to save them. This could be made into a specific battle end session, but one can discuss that the player should have to consider this
                    // during the battle, hence it being a part of the difficulty of the game.
                }
            }
        }
        // Method for reversing the order of all objects in list Party. Party is global and the only list we could need do this with, therefore no list argument needed.
        public static void ReversePartyOrder()
        {
            for (var i = Party.Count - 1; i > -1; i--)
            {
                Hero w = Party[i];
                Party.Remove(Party[i]);
                Party.Add(w);
            }
            Console.Clear();
            LookAtParty();
        }
        // Method for sorting the Party using selection sort by attached argument.
        public static void SortPartyBySpecificInput(string sortThis)
        {
            int posMax;

            for (int i = 0; i < Party.Count - 1; i++)
            {
                posMax = i;

                for (int j = i + 1; j < Party.Count; j++)
                {
                    if (sortThis == "health")
                    {
                        if (Party[j].healthpoints > Party[posMax].healthpoints)
                        {
                            posMax = j;
                        }
                    }
                    else if (sortThis == "attackpower")
                    {
                        int sumOfAttackj = Party[j].strength + Party[j].speed + Party[j].attackDice;
                        int sumOfAttacki = Party[i].strength + Party[i].speed + Party[i].attackDice;
                        if (sumOfAttackj > sumOfAttacki)
                        {
                            posMax = j;
                        }
                    }
                }
                if (posMax != i)
                {
                    var temp = Party[i];
                    Party[i] = Party[posMax];
                    Party[posMax] = temp;
                }
            }
            // Lets the player see the result.
            Console.Clear();
            LookAtParty();
        }
        // Method for creating a string of the current time in the game.
        public static string Clock()
        {
            string time;

            int minutes = timeUnits * 15;
            string perhapsZero = "0";
            string Minutes = "" + minutes;
            if (minutes == 0)
                Minutes = "00";
            if (hourOfDay > 9)
                perhapsZero = "";

            time = perhapsZero + hourOfDay + ":" + Minutes;

            return time;
        }
        // Method for randomizing a boolean result from an amount of chances given in the argument.
        public static bool RandomizeBool(int chances)
        {
            int rando = randomize.Next(chances);

            if (rando == 0)
                return true;
            else
                return false;
        }

        // Example code to be used eventually.
        //{
        //    string s1 = "*****0000abc000****";

        //    char[] charsToTrim1 = { '*', '0' };

        //    // string to be trimmed
        //    string s2 = "  abc";
        //    string s3 = "  -GFG-";
        //    string s4 = "  GeeksforGeeks";

        //    // Before TrimStart method call
        //    Console.WriteLine("Before:");
        //    Console.WriteLine(s1);
        //    Console.WriteLine(s2);
        //    Console.WriteLine(s3);
        //    Console.WriteLine(s4);

        //    Console.WriteLine("");

        //    // After TrimStart method call
        //    Console.WriteLine("After:");

        //    // argument as char array
        //    Console.WriteLine(s1.TrimStart(charsToTrim1));
        //}

        // Method for checking that the name given in the beginning is reasonable. For now it just checks for completely empty or spacebar string.
        // Above example code will be used to simply trim away all spaces before and after a name.
        // Maybe there will be some other signs that can't be in a name, but checking if something is just scrambled letters seem too hard to be worth it.
        // A player can name themself KLlnfq¨'knsad-ikdsa,.f990u 902-3rej<<agik'sdnof if they want to, who cares?
        private static bool ReasonableName(string check)
        {
            // Checks string 'check', first if only spacebar was pressed, then if the first or last (or only) sign is space.
            if (check == "" || (char)check[0] == 32 || (char)check[check.Length - 1] == 32)
            {
                return false;
            }
            return true;
        }
        // Method for capitalizing the first letter in a string.
        public static string CapitalizeFirstLetter(string str)
        {
            if (str == null)
                return null;

            string capStr = str;
            if (capStr.Length > 0)
            {
                capStr = char.ToUpper(capStr[0]) + capStr.Substring(1);
            }
            return capStr;
        }
        // Method for only allowing return as key to continue.
        public static void AwaitKeyEnter()
        {
            var keyPressed = Console.ReadKey();

            while (keyPressed.Key != ConsoleKey.Enter)
            {
                keyPressed = Console.ReadKey();
                if (keyPressed.Key == ConsoleKey.Enter)
                {
                    break;
                }
            }
        }
        // Method for emptying the console to stop the game from skipping things when a player has spammed the return button.
        // I still find it hard to implement at the right places and knowing where it's needed...
        public static void FlushKeyboard()
        {
            if (Console.KeyAvailable)
            {
                // There is input. Before exiting the method we empty the console so that the next pause isn't automatically ended by spammed enters or what not.
                while (Console.KeyAvailable)
                    Console.ReadKey(false);
            }
        }
        // Method for checking if all warriors have died...
        public static void CheckIfAllDied()
        {
            if (Party.Count == 1 && Party[0].healthpoints <= 0)
            {
                WrtL("You have died from your ailments, nothing could be done. There wasn't time.");

                AwaitKeyEnter();
                EndGameByDeath(0);
            }
            else
            {
                int warriorsDead = 0;
                for (int i = 0; i < Party.Count; i++)
                {
                    if (Party[i].healthpoints <= 0)
                        warriorsDead++;
                }
                if (warriorsDead >= Party.Count)
                {
                    WrtL("Every one of the party has fallen from ailments... With no one standing, nothing can be done.");
                    WrtL("If you're lucky, all of you will stop breathing before any monster finds this free heap of meat...");

                    AwaitKeyEnter();
                    EndGameByDeath(0);
                }
            }
        }
        // Method for when ending the game voluntarily.
        public static void EndGame()
        {
            Console.Clear();

            Wrt("You pressed ");
            Wrt("yl", "", "Quit", true);
            WrtL(", which will end the game.");
            Wrt("\nKeep in mind that you ");
            Wrt("yl", "","lose your progress", true);
            Wrt(" when quitting.\nIf you have ");
            Wrt("w", "", "saved your character", true);
            WrtL(" it will be possible to load it and try again of course, but everything will be");
            WrtL("yl", "", "reset to starting conditions.", true);
            WrtL("\nYou fight until you win or die... Or you quit.");
            WrtL("Are you quitting?");
            WrtL("dy", "", "\nEnter to confirm\nSpacebar to go back to the game menu\n", true);

            var keyPressed = Console.ReadKey();

            if (keyPressed.Key == ConsoleKey.Enter)
            {
                Console.Clear();

                WrtL("Well then. Goodbye.");

                AwaitKeyEnter();
                Environment.Exit(0);
            }
            else if (keyPressed.Key == ConsoleKey.Spacebar)
            {
                return;
            }
        }
        // Method for the game ending because all warriors died.
        public static void EndGameByDeath(int reason)
        {
            if (reason == 0)
            {
                // 0 = All warriors died, you last.
                Console.Clear();

                Game.WrtL("Many are those who went into these caves and never came back.\n");
                Game.WrtL("Another party of adventurers might be luckier.\n");
                Game.WrtL("\nFarewell");
            }

            AwaitKeyEnter();
            Environment.Exit(1);
        }
        // Method for preparing a warrior for being saved, removing gear (objects inside the object) and replacing them with strings that can be used to recreate all of it again.
        public static void PrepareHeroForSave(Hero player)
        {
            // NOTE! A little confusing detail is that bonusattackdice is bundled with attackdice in the file. It makes it look like the math is wrong in the file, since the weapon's
            // own attackdice isn't mentioned there.
            // Maybe it should be arranged so that it looks right when looking at the file, to make it easier to see when real bugs happen.
            // Thought: One could reset attackdice to zero before saving. The weapon will make sure one gets the right numbers anyway.

            // Saves stuff for resetting after save.
            Gear eArmor = player.equippedArmor;
            Gear eWeapon = player.equippedWeapon;
            Gear eSweapon = player.equippedSecondaryWeapon;
            Gear eShield = player.equippedShield;

            // Nullifies stuff.
            player.equippedArmor = null;
            player.equippedWeapon = null;
            player.equippedSecondaryWeapon = null;
            player.equippedShield = null;

            for (int i = 0; i < player.warriorGear.Length; i++)
            {
                player.warriorGear[i] = null;
            }

            // Saves the amount of branzen left.
            player.branzen = Branzen;

            // Now we can save the object.
            Save(player.name + ".xml", player);

            // Resets all the stuff.
            player.equippedArmor = eArmor;
            player.equippedWeapon = eWeapon;
            player.equippedSecondaryWeapon = eSweapon;
            player.equippedShield = eShield;

            player.warriorGear[1] = player.equippedArmor;
            player.warriorGear[2] = player.equippedWeapon;
            player.warriorGear[3] = player.equippedSecondaryWeapon;
            player.warriorGear[4] = player.equippedShield;
        }
        // Method for saving a warrior by writing all that object's data to an xml file.
        public static void Save(string FileName, object obj)
        {
            string path = @"save\" + FileName;

            using (var writer = new System.IO.StreamWriter(path))
            {
                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(writer, obj);
                writer.Flush();
            }
        }
        // Method to recreate a warrior from an xml file by taking the strings that is supposed to be objects and fetching them by name from the list of gear.
        public static Player RecreateLoadedPlayer(string name)
        {
            string path = @"save\" + name + ".xml";

            // Creating a player object by loading the file.
            Player You = Load(path);

            // Here are the strings representing the gear objects in the warrior file used to find the right objects and applied to the correct variables in the warrior file.
            if (You.armorName != null)
            {
                You.equippedArmor = Gear.GearWithName(You.armorName);
                You.warriorGear[1] = You.equippedArmor;
            }
            if (You.weaponName != null)
            {
                You.equippedWeapon = Gear.GearWithName(You.weaponName);
                You.warriorGear[2] = You.equippedWeapon;
            }

            if (You.secondaryWeaponName != null)
            {
                You.equippedSecondaryWeapon = Gear.GearWithName(You.secondaryWeaponName);
                You.warriorGear[3] = You.equippedSecondaryWeapon;
            }
            else if (You.shieldName != null)
            {
                You.equippedShield = Gear.GearWithName(You.shieldName);
                You.warriorGear[4] = You.equippedShield;
            }

            Branzen = You.branzen;

            Hero.UpdateWarriorStatsBeginning(You);

            return You;
        }
        // Method for loading a warrior object from an xml file.
        public static Player Load(string FileName)
        {
            using (var stream = System.IO.File.OpenRead(FileName))
            {
                var serializer = new XmlSerializer(typeof(Player));
                return serializer.Deserialize(stream) as Player;
            }
        }
    }
}
