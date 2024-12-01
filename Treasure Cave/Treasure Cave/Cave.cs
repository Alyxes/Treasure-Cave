using System;
using System.Collections.Generic;

namespace TreasureCave
{
    public class Cave
    {
        // Lots of variables to use. Declaration and assigning.
        // Partys position in current cave room.
        public static int[] PartyPosition = new int[2] {6,3};
        // position of current cave room in the caveMap array.
        public static int[] BigCavePosition = new int[2] {15, 7};
        // int to keep track of which number a cave has.
        public static int currentCaveNr = -1;
        // Bool to keep track of if the player passes the same pathway again.
        public static bool samePathway = true;

        // The list of all the cave rooms.
        public static List<Cave> Caves = new List<Cave>();
        // The list of all cave rooms used!
        public static List<int> usedCaveRooms = new List<int>();

        // The amount of rooms, a constant because the array that will use it is sad otherwise.
        public const int rooms = 20;
        // That plus one! Because I want to keep one empty room in the array for easy copying.
        const int roomsPlusOneEmpty = rooms + 1;

        // Difficulties of the rooms. Only Easy is used yet.
        static string e = "Easy";
        static string m = "Medium";
        static string p = "Pushy";
        static string t = "Tough";
        static string h = "Hard";
        static string w = "Wild";
        static string a = "Alarming";
        static string s = "SuperHard";
        static string i = "Insane";
        static string l = "Lunacy";
        static string r = "Ridiculous";
        static string g = "GodMode";

        // These four lists are used to compile each cave room. Each corresponding value is put into each room, here is no randomness.
        public static string[] floorDifficulty = new string[rooms] { e, e, e, e, e, e, e, e, e, e, e, e, e, e, e, e, e, e, e, e };
        public static int[] monsterAmount = new int[rooms] { 5, 6, 7, 6, 6, 5, 7, 4, 4, 5, 5, 5, 6, 4, 4, 5, 5, 7, 3, 4};
        public static double[] monsterRisk = new double[rooms] { 0.12, 0.13, 0.1, 0.12, 0.11, 0.09, 0.12, 0.1, 0.14, 0.11, 0.12, 0.1, 0.11, 0.1, 0.1, 0.12, 0.12, 0.13, 0.09, 0.11};
        // I wanted to be able to have several room maps designed by me in the same list. A 3-dimensional array seemed the best way.
        // I love the neat presentation of it and how easy it gets to design a new room right here in the code.
        // 0 = floor, 1 = wall, 3 = treasure, 2 was doors, they now randomize, 4 was monsters, they also now randomize.
        public static int[,,] floorMaps = new int[roomsPlusOneEmpty, 7, 7] {

            {{0,3,1,0,0,0,3}, //0
             {0,0,1,0,0,0,1},
             {1,0,0,0,1,0,0},
             {0,0,1,1,1,0,0},
             {0,0,0,0,0,0,0},
             {0,1,0,0,0,1,1},
             {3,1,0,0,0,0,3}},

            {{0,0,0,0,0,0,3}, //1
             {3,0,0,1,0,1,1},
             {1,1,0,1,0,0,0},
             {0,0,0,1,3,0,0},
             {1,0,1,1,1,0,1},
             {3,0,0,1,0,0,3},
             {0,0,0,0,0,0,0}},

            {{1,1,0,0,0,1,1}, //2
             {1,3,0,0,0,1,1},
             {0,0,1,0,0,0,1},
             {0,0,0,0,1,0,0},
             {0,1,0,0,0,0,0},
             {1,3,0,0,1,3,1},
             {1,1,0,0,1,1,1}},

            {{3,0,0,0,0,0,1}, //3
             {1,0,1,1,1,0,0},
             {0,0,0,3,0,0,0},
             {0,1,1,0,1,1,0},
             {0,1,3,0,0,0,0},
             {0,0,1,1,1,0,0},
             {3,0,0,0,0,0,1}},

            {{0,0,0,0,1,0,1}, //4
             {3,1,0,0,1,0,0},
             {1,0,0,1,0,0,0},
             {0,0,1,0,0,1,3},
             {0,0,0,0,1,1,1},
             {0,1,0,0,1,3,0},
             {0,3,1,0,0,0,0}},

            {{1,1,0,0,1,0,3}, //5
             {0,1,1,0,0,0,1},
             {0,3,1,0,1,1,1},
             {0,0,1,0,0,0,0},
             {0,0,0,0,0,0,0},
             {1,1,0,0,1,3,0},
             {1,0,0,3,1,1,1}},

            {{1,1,1,0,0,1,3}, //6
             {0,0,0,0,0,0,0},
             {0,1,3,0,1,1,1},
             {0,0,0,0,0,0,3},
             {0,0,1,1,1,0,1},
             {1,0,0,0,0,0,0},
             {3,0,0,0,0,1,3}},

            {{0,3,1,0,0,1,3}, //7
             {0,0,1,0,0,1,0},
             {0,0,0,1,0,0,0},
             {0,0,0,1,0,0,0},
             {0,0,0,1,0,0,0},
             {0,0,1,0,0,0,1},
             {3,1,3,0,0,1,1}},

            {{1,1,1,3,1,1,1}, //8
             {1,1,1,0,1,1,1},
             {1,1,0,0,0,1,1},
             {3,0,0,3,0,0,3},
             {1,1,0,0,0,1,1},
             {1,1,1,0,1,1,1},
             {1,1,1,3,1,1,1}},

            {{0,0,0,0,0,0,1}, //9
             {3,0,0,0,0,3,1},
             {1,1,0,0,1,1,1},
             {3,0,1,3,1,0,0},
             {0,0,1,1,0,0,0},
             {1,0,0,0,0,0,0},
             {1,1,0,0,0,3,1}},

            {{1,3,0,0,1,1,3}, //10
             {0,0,0,0,1,0,0},
             {0,1,1,1,0,0,1},
             {0,0,1,0,0,0,0},
             {0,0,3,1,0,1,0},
             {0,0,0,1,1,3,0},
             {1,0,0,0,0,1,0}},

            {{0,0,0,0,0,1,3}, //11
             {3,1,0,0,0,0,0},
             {1,0,0,1,0,0,1},
             {0,0,1,3,1,0,0},
             {0,1,1,0,1,1,0},
             {0,1,0,0,0,1,0},
             {3,1,0,0,0,1,0}},

            {{1,0,0,0,0,0,0}, //12
             {3,0,1,0,0,1,0},
             {1,1,0,0,1,1,0},
             {0,0,0,1,0,0,0},
             {0,1,1,0,0,0,1},
             {0,0,0,0,1,0,3},
             {1,3,1,0,0,0,0}},

            {{1,0,0,0,0,1,1}, //13
             {1,0,0,0,0,1,3},
             {0,0,0,0,0,1,0},
             {0,0,0,0,1,0,0},
             {0,0,1,1,0,0,0},
             {3,1,3,0,0,0,0},
             {1,1,0,0,0,0,1}},

            {{3,0,0,0,1,0,3}, //14
             {1,0,0,0,0,1,0},
             {0,1,1,0,0,0,0},
             {0,0,0,1,0,0,0},
             {1,0,0,0,1,0,0},
             {3,1,0,0,0,1,1},
             {0,0,0,0,0,0,3}},

            {{0,3,1,0,0,3,1}, //15
             {0,0,1,0,0,1,3},
             {0,0,0,0,1,0,0},
             {0,1,0,0,0,0,0},
             {0,0,1,0,0,1,1},
             {0,0,0,1,0,0,3},
             {1,0,0,3,1,0,0}},

            {{1,1,0,0,0,0,3}, //16
             {1,1,0,0,0,1,1},
             {0,0,0,0,0,1,1},
             {0,0,1,1,0,0,0},
             {0,0,1,1,0,0,0},
             {0,0,0,0,1,1,0},
             {3,0,0,0,1,1,3}},

            {{1,0,0,0,0,1,0}, //17
             {0,0,0,1,0,0,0},
             {0,3,0,0,0,3,0},
             {0,0,0,1,0,0,0},
             {0,0,0,0,0,1,0},
             {0,1,3,0,0,0,0},
             {0,0,0,0,1,0,0}},

            {{0,0,0,0,0,0,0}, //18
             {0,1,3,0,1,0,1},
             {0,1,1,1,1,0,0},
             {0,0,1,1,1,1,0},
             {0,1,1,1,3,1,0},
             {0,3,1,1,0,0,0},
             {0,0,0,0,0,0,1}},

            {{1,1,1,0,0,1,1}, //19
             {3,0,0,0,0,0,1},
             {1,0,0,1,1,0,3},
             {0,0,1,1,1,0,0},
             {1,0,0,0,1,1,0},
             {1,1,1,0,0,0,0},
             {1,1,1,3,0,1,1}},

            {{0,0,0,0,0,0,0}, //
             {0,0,0,0,0,0,0},
             {0,0,0,0,0,0,0},
             {0,0,0,0,0,0,0},
             {0,0,0,0,0,0,0},
             {0,0,0,0,0,0,0},
             {0,0,0,0,0,0,0}}

            };

        // Declares and assigns the map of the cave. Quite sizeable to allow maximum stray for the user.
        public static Cave[,] caveMap = new Cave[16,16];

        // This is for the Branzen gathered in the cave. As of yet, it's not added to the player Branzen until they exit the cave, however that is redundant as one either exits the cave, dies
        // or finishes the game. But this int is still a little bit useful in order to have a number for the newly looted Branzen.
        public static int lootBranzen = 0;

        // Declaring variables of a cave room that will be assigned later.
        public int id;
        public int amountOfMonsters;
        public double riskOfMonsters;
        public string difficulty;
        public int[] exits;
        public List<Monster> MonsterList; 
        public int[,] caveFloor;

        // The method for turning lots of variables into cave room objects and put them in a list.
        public static void InitiateAllCaves()
        {
            for (var i = 0; i< rooms; i++)
            {
                Cave room = new Cave();

                room.id = i;
                room.caveFloor = GetCaveFloor(i);
                room.amountOfMonsters = monsterAmount[i];
                room.riskOfMonsters = monsterRisk[i];
                room.MonsterList = new List<Monster>();
                room.difficulty = floorDifficulty[i];
                room.exits = new int[4] { 0, 0, 0, 0 }; // Up, right, down, left. 

                Caves.Add(room);

                Monster.GenerateMonsters(room.amountOfMonsters, Caves[i]);
            }
        }

        // Method for taking one of the caves in the 3D-array floorMaps and save it as a new 2D-array.
        static int[,] GetCaveFloor(int z)
        {
            var k = floorMaps;

            int[,] r = new int[7, 7] {
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0}};

            for (var x = 0; x < k.GetLength(1); x++)
            {
                for (var y = 0; y < k.GetLength(2); y++)
                {
                    r[x, y] = k[z, x, y];
                }
            }
            return r;
        }
        // Method for removing a red X on the map.
        public static void EraseRedOnMap(Cave room)
        {
            for (int x = 0; x < room.caveFloor.GetLength(0); x++)
            {
                for (int y = 0; y < room.caveFloor.GetLength(1); y++)
                {
                    if (room.caveFloor[x, y] == 6)
                    {
                        room.caveFloor[x, y] = 0;
                    }
                }
            }
        }
        // Method for drawing the cave for the user. Regards the numbers of the map and changes colours and such.
        public static void DrawCave(Cave room)
        {
            Console.Clear();
            Console.CursorVisible = false;

            string bgCol;

            for (int x = 0; x < room.caveFloor.GetLength(0); x++)
            {
                Game.WrtL("-----------------------------");
                Game.Wrt("|");
                for (int y = 0; y < room.caveFloor.GetLength(1); y++)
                {
                    // These numbers will give special background colour beneath the Party.
                    if (room.caveFloor[x, y] == 2 || room.caveFloor[x, y] == 4 || room.caveFloor[x, y] == 7)
                        bgCol = "dc";
                    else if (room.caveFloor[x, y] == 5)
                        bgCol = "dy";
                    else
                        bgCol = "";

                    if (room.caveFloor[x, y] == 1)
                    {
                        Game.Wrt("", "b", "   ", true);
                        Game.Wrt("|");
                    }
                    else if (x == PartyPosition[0] && y == PartyPosition[1])
                    {
                        Game.Wrt("gr", bgCol, " P ", true);
                        Game.Wrt("|");
                    }
                    else if (room.caveFloor[x, y] == 4)
                    {
                        Game.Wrt("", "dc", "   ", true);
                        Game.Wrt("|");
                    }
                    else if (room.caveFloor[x, y] == 5)
                    {
                        Game.Wrt("", "dy", "   ", true);
                        Game.Wrt("|");
                    }
                    else if (room.caveFloor[x, y] == 6)
                    {
                        Game.Wrt("r", "", " X ", true);
                        Game.Wrt("|");
                    }
                    else if (room.caveFloor[x, y] == 7)
                    {
                        Game.Wrt("w", "c", " * ", true);
                        Game.Wrt("|");
                    }
                    else
                        Game.Wrt("   |");
                }
                Game.WrtL("");
            }
            Game.WrtL("-----------------------------");

            Game.Wrt("\nGathered Branzen: ");
            Game.Wrt("yl", "", lootBranzen, true);

            Game.Wrt("\n\nTime: ");
            Game.WrtL("yl", "", Game.Clock(), true);

            Game.WrtL("dy","","\nNavigate with arrow keys.\nPress enter to use pathways.\nPress space for the pause menu.",true);
        }
        // Method for letting the player navigate the cave with the arrow keys and enter pathways.
        public static void NavigateCave(Cave current)
        {
            bool moved = false;
            CheckNewPosition(current);

            DrawCave(current);

            int x;
            int y;

            do
            {
                // Checks if the user just moved, and makes sure to check the position again. Also removes the need to redraw the cave if user tried to move against a wall.
                if (moved)
                {
                    DrawCave(current);
                    // If the new position was a special place, redraw the map.
                    if (CheckNewPosition(current))
                        DrawCave(current);
                }

                moved = false;

                x = PartyPosition[0];
                y = PartyPosition[1];

                var keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    if (CheckIfNotWall(x - 1, y, current))
                    {
                        Move(x - 1, y);
                        moved = true;
                    }
                }
                else if (keyPressed.Key == ConsoleKey.RightArrow)
                {
                    if (CheckIfNotWall(x, y + 1, current))
                    {
                        Move(x, y + 1);
                        moved = true;
                    }
                }
                else if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    if (CheckIfNotWall(x + 1, y, current))
                    {
                        Move(x + 1, y);
                        moved = true;
                    }
                }
                else if (keyPressed.Key == ConsoleKey.LeftArrow)
                {
                    if (CheckIfNotWall(x, y - 1, current))
                    {
                        Move(x, y - 1);
                        moved = true;
                    }
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    // Entering pathways inside the cave.
                    if (current.caveFloor[x, y] == 2 || current.caveFloor[x, y] == 4)
                    {
                        EraseRedOnMap(current);
                        EnterCaveDoor();
                    }
                    // Exiting through the cave entrance.
                    else if (current.caveFloor[x, y] == 7)
                    {
                        EraseRedOnMap(current);
                        ExitCave();
                    }
                }
                else if (keyPressed.Key == ConsoleKey.Spacebar)
                {
                    // Pause menu for some nice choices.
                    Menu.PauseMenu(current);
                    DrawCave(current);
                }
            }
            while (true);
        }
        // Method for checking next square if it's a wall. Returns false for walls.
        static bool CheckIfNotWall(int x,int y, Cave room)
        {
            if (OutOfBounds(x, y) || room.caveFloor[x,y] == 1) //  1 is wall.
                return false;
            else
                return true;
        }
        // Method for checking if a space is outside the cave array, returns true if so.
        static bool OutOfBounds(int x, int y)
        {
            if (x < 0 || y < 0 || x > 6 || y > 6)
                return true;
            else
                return false;
        }
        // Method for setting the Party's position to the new position after player successfully steered around the walls.
        static void Move(int x, int y)
        {
            //NOTE! Thinking about a counter in Move to make Game.TimeUnitsPass(1, true) run if one walks around alot, without encounters of any kind. Maybe not.
            PartyPosition[0] = x;
            PartyPosition[1] = y;
        }
        // Method for checking what's up on this new position.
        static bool CheckNewPosition(Cave room)
        {
            // We are assuming this won't be something out of the ordinary.
            // This bool decides wether the map is redrawn again, which is necessary if something happened that wiped the map and printed some text.
            bool specialPlace = false;

            int x = PartyPosition[0];
            int y = PartyPosition[1];

            // You have entered a new cave, but not necessarily the first one, and your position should become an entrance whether it is or not already.
            if (Game.justEntered)
            {
                if (room.caveFloor[x, y] != 4)
                    room.caveFloor[x, y] = 4;

                if (usedCaveRooms.Count == 1 && x == 6 && y == 3)
                {
                    // This is THE GRAND ENTRANCE! This should only happen once.
                    if (room.caveFloor[x, y] != 7)
                        room.caveFloor[x, y] = 7;
                }
                Game.justEntered = false;
            }

            if (room.caveFloor[x, y] == 0 && room.amountOfMonsters > 0)
            {
                // It's a regular floor, but since there's still monsters in this cave we have to see if one will appear.
                int risk = Game.randomize.Next(100);
                if ((100 * room.riskOfMonsters) > risk)
                {
                    // A monster appeared and battle must occur!
                    int randoM = Game.randomize.Next(room.amountOfMonsters);
                    Monster monster = room.MonsterList[randoM];

                    // We have to remove warriors that died in the last fight and haven't had time to die yet, otherwise searching for monsters is a way to stay alive...
                    Game.RemoveDeadWarriors();
                    // Above, average party level is recalculated if any warriors have died, which affects the monster level.
                    Monster.AdaptMonsterLevel(monster);
                    
                    int rand = Game.randomize.Next(monster.surpriseAttackChance + Game.Party.Count);

                    if (rand == 0)
                    {
                        int result = UnexpectedAttack(room, monster);

                        if (result == 0)
                        {
                            // monster still alive.
                            Battle(room, monster, true);
                        }
                        // else monster died from a counter attack.
                        // No Battle method runs, just go to GoThroughBattleResult.
                    }
                    else
                    {
                        Battle(room, monster, false);
                    }
                    // Results are in. Remove any eventual dead warriors from Party.
                    GoThroughBattleResult();

                    if (Game.Party.Count == 0)
                    {
                        // This shouldn't happen, death is supposed to happen inside the battle method already.
                        Game.EndGameByDeath(0);
                    }
                    else
                    {
                        // This means you beat the monster.
                        // Erase a eventual red X left.
                        EraseRedOnMap(room);
                        // Put a new X on last battle spot.
                        room.caveFloor[x, y] = 6;
                        specialPlace = true;
                    }
                }
            }
            else if (room.caveFloor[x, y] == 2)
            {
                // Found pathway! Transform it into an always visible pathway (square of dark cyan paint).
                room.caveFloor[x, y] = 4;
            }
            else if (room.caveFloor[x, y] == 3)
            {
                // Treasure chest. Mm-mm. The reason you are here.
                int newBranzen = Game.randomize.Next(150, 381);

                room.caveFloor[x, y] = 5; // Transforms it into a visible opened chest (square of yellow paint).

                Console.Clear();
                Game.WrtL("yl","","You found treasure!\n",true);
                Game.Wrt("It's ");
                Game.Wrt("yl", "", newBranzen + " Branzen!\n", true);

                lootBranzen += newBranzen;

                float rand = Game.randomize.Next(2);
                if (rand <= 0.67)
                {
                    // Item!
                    string ItemName = Gear.RandomizeLootGear();
                    string ItemName2 = "Nothing";

                    rand = Game.randomize.Next(2);
                    if (rand <= 0.35)
                    {
                        // SECOND ITEEEEEM!!
                        ItemName2 = Gear.RandomizeLootGear();
                    }

                    if (ItemName == ItemName2)
                        Game.WrtL("\nYou've found two " + ItemName + "s!");
                    else if (ItemName2 != "Nothing")
                    {
                        Game.WrtL("\nYou've found one " + ItemName + " and one " + ItemName2 + "!");
                    }
                    else
                        Game.WrtL("\nYou've found one " + ItemName + "!");
                }

                Game.WrtL("\nLet's hope you get out with it alive.\n");

                Game.AwaitKeyEnter();

                Game.TimeUnitsPass(1, true);

                // This truly was a special place. (Since we have wiped the console clean, we need to redraw the cave when we return to the navigation view from this method. Returning true does that.
                specialPlace = true;
            }
            return specialPlace;
        }
        // Method for when a monster has sneaked up on the party and attacks first.
        public static int UnexpectedAttack(Cave room, Monster monster)
        {
            // A monster has taken the group by surprise, it gets the first attack towards a random party member.
            Console.Clear();

            int warriorToAttack;
            if (Game.Party.Count == 1)
                warriorToAttack = 0;
            else
                warriorToAttack = Monster.MonsterDecisions(monster);

            if (warriorToAttack <= -1)
            {
                // Shouldn't be able to happen here. Resets to 0.
                Game.WrtL("\nwarriorToAttack = -1! Shouldn't happen here!");
                Game.WrtL("Resets to 0.\n\n");
                warriorToAttack = 0;
            }

            Game.WrtL("A " + monster.name + " has snuck up on you from the darkness and attacks " + Game.Party[warriorToAttack].name + " in the back!\n");
            Game.WrtL(monster.battlecry + "\n");

            Monster.Attack(monster, Game.Party[warriorToAttack]);

            return Monster.DeathResults(warriorToAttack, monster);
        }
        // This is were the battles happen.
        public static void Battle(Cave room, Monster monster, bool alreadyAttacked)
        {
            Console.Clear();

            bool victory = false;
            bool monsterDead = false;
            int reaction;

            int amountInParty = 1;
            int warriorsUsed = 0;

            if (alreadyAttacked == false)
            {
                Game.WrtL(monster.enter_message);
                Game.WrtL(monster.battlecry);
                Game.WrtL("\nYou have to fight it!\n");
            }
            else
            {
                Game.WrtL("Now you have to fight the " + monster.name + "!\n");
            }

            Game.AwaitKeyEnter();

            Console.Clear();

            // Presents the name of everyone in the Party.
            if (Game.Party.Count > 1)
            {
                amountInParty = Game.Party.Count;
                Game.WrtL("Your party has " + amountInParty + " warriors!");
                Game.WrtL("They are:\n");
            }
            else
            {
                Game.WrtL("You're going solo. Bold but reckless!");
                Game.WrtL("You are:\n");
            }

            Game.w();
            for (var i = 0; i < Game.Party.Count; i++)
            {
                Game.WrtL(Game.Party[i].name);
            }
            Game.res();

            if (amountInParty > 1)
                Game.WrtL("\nEach of them will now attack!");
            else
                Game.WrtL("\nYou will now attack!");

            Game.AwaitKeyEnter();

            do
            {
                Game.battleRound++;
                // Goes through Party members.
                for (int i = 0; i < Game.Party.Count; i++)
                {
                    Console.Clear();

                    Hero P = Game.Party[i];

                    bool usedDisposableWeapon = false;

                    Game.GoThroughAilments(P);
                    Game.CheckIfAllDied();

                    // If Party members have been killed during this fight or just died from an ailment, they are skipped.
                    if (P.healthpoints <= 0)
                    {
                        Game.WrtL(P.name + " is dying on the ground and can't do anything!");
                        Game.AwaitKeyEnter();

                        continue;
                    }

                    if (P.recoverFromPoisoningCounter > 0)
                    {
                        Hero.RecoverFromPoisoning(P);
                    }

                    // If Party members have lost their composure they now flee into the darkness, and are lost.
                    if (P.composure <= 0)
                    {
                        if (Game.Party.Count == 1 || P.alltimePartyId == 1)
                        {
                            Game.WrtL("You panic, drop your torch and runs out into the darkness, to be easily killed by any beast.");
                            if (Game.Party.Count > 1)
                                Game.WrtL("Without you the party also panics and scatters into the darkness, lost in the cave, to get killed one by one...");

                            Game.WrtL("\nBut you fought bravely to the end.");

                            Game.AwaitKeyEnter();
                            Game.EndGameByDeath(0);
                        }
                        else
                        {
                            Game.WrtL(P.name + " has lost " + P.pronoun[2] + " composure and is panicking! " + Game.CapitalizeFirstLetter(P.pronoun[0]) + " runs away into the darkness and disappears!");

                            if (!Game.partyMembersInPeril.Remove(P.alltimePartyId))
                            {
                                Game.WrtL("alltimePartyId not found in partyMembersInPeril-List!");
                            }

                            Game.Party.RemoveAt(i);
                            i--;

                            Hero.ReCalculateCurrentPartyId();

                            Game.AwaitKeyEnter();
                            continue;
                        }
                    }
                    
                    // If the battle is drawing out and the warrior's health is waning their composure takes a hit.
                    if (Game.battleRound > 4 && P.healthpoints <= P.maxHealth/2)
                    {
                        Game.WrtL("The drawn out battle and lesser health of " + P.name + " diminishes " + P.pronoun[2] + " composure!\n");
                        if (P.composure > 0)
                            P.composure--;
                    }
                    if (P.stamina <= 0)
                    {
                        Game.WrtL(P.name + " is too exhausted to attack!\n");
                        if (P.composure > 0)
                            P.composure--;
                        Game.AwaitKeyEnter();
                    }
                    // If this warrior's composure just hit bottom the player has another battle round to fix it before this warrior runs off, unless it's the player character and they're
                    // going alone...
                    if (P.composure <= 0)
                    {
                        P.composure = 0;

                        if (P.alltimePartyId == 1)
                        {
                            if (Game.Party.Count == 1)
                            {
                                Game.WrtL("You panic, drop your torch and runs out into the darkness, to be easily killed by any beast.");
                                Game.WrtL("\nBut you fought bravely to the end.");

                                Game.AwaitKeyEnter();
                                Game.EndGameByDeath(0);
                            }
                            else
                            {
                                P.ailments.Add(Game.ailments[7]); // panicking
                                Game.AddToPerilListIfAbsent(P.alltimePartyId);

                                Game.WrtL("You're panicking and can't do anything right now!");
                                Game.WrtL("Hopefully, the rest of the team will be able to win the battle soon and calm you down...");
                            }
                            Game.AwaitKeyEnter();
                        }
                        else
                        {
                            P.ailments.Add(Game.ailments[7]); // panicking
                            Game.AddToPerilListIfAbsent(P.alltimePartyId);

                            Game.WrtL(P.name + " is panicking and can't do anything right now! " + Game.CapitalizeFirstLetter(P.pronoun[0]) + " will shortly run away and be lost\nto the darkness if not held back in some way!");
                            
                            Game.AwaitKeyEnter();
                            continue;
                        }
                    }
                    if (warriorsUsed < amountInParty)
                        warriorsUsed++;

                    while(true)
                    {
                        int choice = Menu.BattleMenu(i, monster, usedDisposableWeapon);
                        // 0 = "Attack", 1 = "Rest/Defence", 2 = "Use Item", 3 = "Check Team"
                        switch (choice)
                        {
                            case -2:
                                // Using disposable weapons will only be possible once per turn thanks to this boolean.
                                usedDisposableWeapon = true;

                                if (monster.healthpoints <= 0)
                                {
                                    Console.Clear();
                                    Game.WrtL("The " + monster.name + " was killed!");

                                    monsterDead = true;
                                    victory = true;

                                    Game.AwaitKeyEnter();
                                    break;
                                }
                                else if (monster.composure <= 0)
                                {
                                    Console.Clear();
                                    int fleeResult = Monster.TryToFlee();
                                    if (fleeResult == 1)
                                    {
                                        // It succeeds to flee
                                        Game.WrtL("The " + monster.name + " panics and runs away into the darkness!");
                                        monster.composure = monster.baseComposure;

                                        victory = true;

                                        Game.AwaitKeyEnter();
                                        break;
                                    }
                                    else
                                    {
                                        // It failed to flee.
                                    }
                                }

                                continue;
                            case -1:
                                // Something done that let's player choose again.
                                continue;
                            case 0:
                                Console.Clear();
                                Game.WrtL(P.name + " attacks the monster!\n");

                                // Runs the hero's Attack method.
                                Hero.Attack(Game.Party[i], monster);
                                // The monster automatically will be interested in attacking the warrior that attacked it.
                                monster.aggravatedAgainst = i;

                                if (monster.healthpoints <= 0)
                                {
                                    // Yay!
                                    monsterDead = true;
                                    victory = true;
                                    break;
                                }

                                // We need to see what the monster decides to do now. Maybe it needs to rest instead of attacking. It all happens in Reaction.
                                Game.WrtL("");
                                reaction = Monster.Reaction(monster, i);

                                if (reaction == 1)
                                {
                                    monsterDead = true; // The monster died somehow.
                                    victory = true;
                                }
                                else if (reaction == -1)
                                {
                                    int fleeResult = Monster.TryToFlee();
                                    if (fleeResult == 1)
                                    {
                                        // It succeeds to flee
                                        monster.composure = monster.baseComposure;
                                        victory = true;
                                        break;
                                    }
                                    else
                                    {
                                        // It failed to flee.
                                    }
                                }
                                // else if (reaction == 0) // The monster decided to rest, nothing more need to happen here.

                                break;
                            case 1:
                                // Runs the Rest method for the heroes.
                                Console.Clear();

                                Hero.Rest(Game.Party[i]);

                                Game.AwaitKeyEnter();

                                // The monster can still decide to attack!
                                reaction = Monster.Reaction(monster, i);

                                if (reaction == -1)
                                {
                                    int fleeResult = Monster.TryToFlee();
                                    if (fleeResult == 1)
                                    {
                                        // It succeeds to flee
                                        victory = true;
                                        break;
                                    }
                                    else
                                    {
                                        // It failed to flee.
                                    }
                                }
                                else if (reaction == 1)
                                    victory = true; // The monster died somehow.

                                break;
                            case 2:
                                int result = Menu.UseItemMenu(P);
                                if (result == 0)
                                {
                                    // No item was used.
                                    continue;
                                }
                                else
                                {
                                    // An item was used. The player's turn is used up.
                                    // The monster can still decide to attack!
                                    Console.Clear();

                                    reaction = Monster.Reaction(monster, i);
                                    if (reaction == -1)
                                    {
                                        int fleeResult = Monster.TryToFlee();
                                        if (fleeResult == 1)
                                        {
                                            // It succeeds to flee
                                            victory = true;
                                            break;
                                        }
                                        else
                                        {
                                            // It failed to flee.
                                        }
                                    }
                                    else if (reaction == 1)
                                        victory = true; // The monster died somehow.
                                    // else
                                        // None of the above. Battle continues.
                                    break;
                                }
                                //case 3:
                                // Happens in the menu instead, as it shouldn't take this warriors turn to LookAtParty().
                        }
                        break;
                    }
                    if (victory)
                    {
                        break;
                    }
                }
            }
            while (victory == false);

            // Here I make sure that warriors that didn't get to fight in the battle (if it was short) still suffers from their ailments. Which is fair.
            if (Game.partyMembersInPeril.Count > 0 && Game.battleRound == 1 && warriorsUsed < amountInParty)
            {
                // To prevent warriors from suffering ailments twice, we start at int warriorsUsed.
                for (int i = warriorsUsed; i < amountInParty; i++)
                {
                    for (int j = 0; j < Game.partyMembersInPeril.Count; j++)
                    {
                        if (Game.partyMembersInPeril[j] == Game.Party[i].alltimePartyId)
                        {
                            if (Game.Party[i].healthpoints > 0)
                                Game.GoThroughAilments(Game.Party[i]);
                        }
                    }
                }
            }
            // We do not need to check if every one is dead here, because below, a check is done to see if the player character is dead, in which case the game ends.
            // It has different phrasing than CheckIfAllDied() because it's in the end of a battle.

            Game.battleRound = 0;

            Console.Clear();
            if (monsterDead)
            {
                Game.WrtL("You defeated the " + monster.name + "!");
                // Removes the monster from the room-list so that it can't come back from the dead.
                room.MonsterList.Remove(monster);
                room.amountOfMonsters--;
            }
            else
            {
                Game.WrtL("The " + monster.name + " got away!");
                Game.WrtL("It lives to pester you another day...");
            }

            Game.TimeUnitsPass(1, false);

            // Because of ailments, we have to go through the party once again to make sure that the player character isn't in fact dead.
            // Only player character is important, because if they are dead, the team will dissipate and the game is over, if they aren't dead, the team can be revived.
            for (int i = 0; i < Game.Party.Count; i++)
            {
                Hero warrior = Game.Party[i];

                if (warrior.alltimePartyId == 1 && warrior.healthpoints <= 0)
                {
                    Console.Clear();

                    if (Game.Party.Count > 1)
                    {
                        Game.WrtL("Even though the battle ended in your favor, you are fallen and were unsavable.\nYour death pulls the groups morale through the floor, they panic and scatter into the darkness.");
                        Game.WrtL("This party is over...");
                    }
                    else
                    {
                        Game.WrtL("Even though the battle ended in your favor, your ailments took your last power to stand up.\nWith no one else by your side your last breaths are seconds away...");
                        Game.WrtL("Such a shame having your last battle end this way.");
                    }

                    Game.AwaitKeyEnter();
                    Game.EndGameByDeath(0);
                }
            }

            Game.justRested = false;

            Game.AwaitKeyEnter();
        }
        // Method running after battles, for looking over which warriors are actually dead, removing ailment panicking, end certain potion effects and leveling up.
        public static void GoThroughBattleResult()
        {
            for (int i = 0; i < Game.Party.Count; i++)
            {
                Hero hero = Game.Party[i];

                if (hero.healthpoints <= 0)
                {
                    if (!Game.partyMembersInPeril.Remove(hero.alltimePartyId))
                    {
                        Game.WrtL("alltimePartyId not found in partyMembersInPeril-List!");
                        Game.AwaitKeyEnter();
                    }
                    
                    Game.Party.RemoveAt(i);
                    i--;
                }
                else
                {
                    // Warriors that panicked but for some reason didn't manage to flee will stay when the monster is gone, but they are very unstable and need rest and probably potions.
                    if (hero.composure < hero.baseComposure)
                    {
                        if (hero.composure <= 0)
                        {
                            Game.RemoveAilment(hero, Game.ailments[7]); // panicking
                        }

                        hero.composure++;
                    }

                    // We have to nullify all effects of potions that shouldn't last beyond the fight.
                    // Of course then we can't have potions that last more than one fight... Maybe not a problem.
                    // Does health need to be tended to here..? Not as of yet.

                    if (hero.strength > hero.maxStrength)
                        hero.strength = hero.maxStrength;

                    if (hero.speed > hero.maxSpeed)
                        hero.speed = hero.maxSpeed;

                    if (hero.stamina > hero.maxStamina)
                        hero.stamina = hero.maxStamina;

                    if (hero.composure > hero.baseComposure)
                        hero.composure = hero.baseComposure;

                    // By doing this one here, possible equipped things that should allow some stats to be greater will be added back.
                    Hero.UpdateWarriorStats(hero);

                    // Leveling up if experience is enough.
                    if (hero.experience >= Game.levelRiseValues[hero.level-1])
                    {
                        Console.Clear();
                        Game.WrtL(hero.name + " has leveled up!");
                        Game.AwaitKeyEnter();
                        Hero.LevelUp(hero, hero.level + 1);
                    }
                    if (hero.dualWieldExperience >= Game.dualWieldLevelRiseValues[hero.dualWieldLevel-1])
                    {
                        Console.Clear();
                        Game.WrtL(hero.name + " has leveled up in dual wielding!");
                        Game.AwaitKeyEnter();
                        Hero.LevelUpDualWielding(hero, hero.dualWieldLevel + 1);
                    }
                }
            }
            Hero.ReCalculateCurrentPartyId();

            Game.calcAveragePartyLevel();
        }
        // Method for checking if a new place on the cave map has a room assigned already or not.
        public static Cave CheckMap(Cave pos)
        {
            if (pos == null)
                return null;

            return pos;
        }
        // Method for checking if a specific cave room has been used already.
        public static bool CaveIsInList(int nc)
        {
            foreach (int i in usedCaveRooms)
            {
                if (i == nc) return true;
            }
            return false;
        }
        // Method for randomly selecting a cave to place in the newly entered coordinates on the cave map.
        public static Cave FindNewCave()
        {
            int nCave;
            do
            {
                // NOTE! This one needs to be changed as soon as there are caves with higher difficulty.
                nCave = Game.randomize.Next(Caves.Count); // randomizes a new cave.
            }
            while (CaveIsInList(nCave));

            usedCaveRooms.Add(nCave);

            FixEntranceNumbers(nCave);
            CreateEntrances(nCave);

            // Puts this randomizer here to make some difference in the chance of entrances.
            float chanceToSwitch = Game.randomize.Next(1);

            if (chanceToSwitch < 0.15)
            {
                Game.minimalNeedForEntrances = 1;
            }
            else if (chanceToSwitch >= 0.15 && chanceToSwitch < 0.65)
            {
                Game.minimalNeedForEntrances = 2;
            }
            else if (chanceToSwitch >= 0.65 && chanceToSwitch < 0.93)
            {
                Game.minimalNeedForEntrances = 3;
            }
            else if (chanceToSwitch >= 0.93)
            {
                Game.minimalNeedForEntrances = 4;
            }

            return Caves[nCave];
        }
        // Method from placing the player on correct position on both the cave map and in cave room, when entering a cave room.
        // Attributes(Player position in big cave X, Player position in big cave Y, direction difference X, direction difference Y, New position in new cave room X, New position in new cave room Y)
        static void EnterRoomFromDirection(int bigCaveX, int bigCaveY, int dirX, int dirY, int entrancePosX, int entrancePosY)
        {
            // Uses CheckMap to see if there is a map or not already in entered direction.
            Cave nextCave = CheckMap(caveMap[bigCaveX + dirX, bigCaveY + dirY]);

            // Which direction the player enters from.
            PartyPosition[0] = entrancePosX;
            PartyPosition[1] = entrancePosY;

            // position in the big cave
            BigCavePosition[0] = bigCaveX + dirX;
            BigCavePosition[1] = bigCaveY + dirY;

            // Checks if this was the path you just passed through. If nothing else has been done in between, time haven't had time to pass.
            if (!samePathway)
                Game.TimeUnitsPass(1, true);

            Game.justEntered = true;
            samePathway = true;

            if (nextCave != null)
            {
                currentCaveNr = nextCave.id;

                NavigateCave(Caves[nextCave.id]);
            }
            else
            {
                // No room there! Places a cave room on the desired position, randomly picked from the list of rooms.
                Cave newCave = FindNewCave();
                caveMap[bigCaveX + dirX, bigCaveY + dirY] = newCave;
                currentCaveNr = newCave.id;

                NavigateCave(newCave);
            }
        }

        // Method for entering a cave in the direction you exited a pathway.
        static void EnterCaveDoor()
        {
            Console.Clear();

            int x = BigCavePosition[0];
            int y = BigCavePosition[1];

            int[] p = PartyPosition;

            if (p[0] == 0 && p[1] == 3)
            {
                // up door
                EnterRoomFromDirection(x, y, -1, 0, 6, 3);
            }
            if (p[0] == 3 && p[1] == 6)
            {
                // right door
                EnterRoomFromDirection(x, y, 0, 1, 3, 0);
            }
            if (p[0] == 6 && p[1] == 3)
            {
                // down door
                EnterRoomFromDirection(x, y, 1, 0, 0, 3);
            }
            if (p[0] == 3 && p[1] == 0)
            {
                // left door
                EnterRoomFromDirection(x, y, 0, -1, 3, 6);
            }
        }
        // Method to randomize the position and amount of entrances/exits in a new cave room.
        public static void FixEntranceNumbers(int caveNr)
        {
            int up;
            int right;
            int down;
            int left;

            int[] e = Caves[caveNr].exits;
            int exitBe;

            do
            {
                exitBe = 0;
                // To have different chances for different outcomes, we give them a tenth as smallest difference.
                up = Game.randomize.Next(0, 10);
                right = Game.randomize.Next(0, 10);
                down = Game.randomize.Next(0, 10);
                left = Game.randomize.Next(0, 10);

                e[0] = up;
                e[1] = right;
                e[2] = down;
                e[3] = left;

                for (int i = 0; i < e.Length; i++)
                {
                    if (e[i] == 9) // It could become an extra treasure chest!
                        e[i] = 3;
                    else
                    {
                        // Using hard set chances here for different directions.
                        if (i == 0) // up
                        {
                            // It's a pretty high chance that there is an exit up.
                            if (e[i] > -1 && e[i] < 2)
                            {
                                e[i] = 0;
                            }
                            else // 2 - 8 of 10 to be an exit.
                            {
                                e[i] = 2;
                                exitBe++;
                            }
                        }
                        else if (i == 2) // down
                        {
                            // It's a lower chance that there is an exit down.
                            if (e[i] > -1 && e[i] < 6)
                            {
                                e[i] = 0;
                            }
                            else // 6 - 8 of 10 to be an exit.
                            {
                                e[i] = 2;
                                exitBe++;
                            }
                        }
                        else // left or right
                        {
                            // Sideway exits are quite regular as well.
                            if (e[i] > -1 && e[i] < 3)
                            {
                                e[i] = 0;
                            }
                            else // 3 - 8 of 10 to be an exit.
                            {
                                e[i] = 2;
                                exitBe++;
                            }
                        }
                    }
                }
            }
            while (exitBe < Game.minimalNeedForEntrances); // It has to be as many or more exits as the game has decided for the moment. It's 1 after the first cave.
        }
        // Method for checking that randomized entrances doesn't lead outside the cavemap array, and to actually change the cave rooms array data to be exits.
        public static void CreateEntrances(int caveNr)
        {
            int x = BigCavePosition[0];
            int y = BigCavePosition[1];

            int[] e = Caves[caveNr].exits;

            for (int i = 0; i < e.Length; i++)
            {
                if (e[i] == 0)
                    continue; // Floor. Don't change the maps preset value.
                // exit list index && is that direction inside the cave map?
                if (i == 0 && x-1 > -1)
                    Caves[caveNr].caveFloor[0, 3] = e[i]; // Creates an exit or treasure chest in the middle of the wall in this direction.
                else if (i == 1 && y+1 < 16)
                    Caves[caveNr].caveFloor[3, 6] = e[i];
                else if (i == 2 && x+1 < 16)
                    Caves[caveNr].caveFloor[6, 3] = e[i];
                else if (i == 3 && y-1 > -1)
                    Caves[caveNr].caveFloor[3, 0] = e[i];
            }
        }
        // Method for returning out to the game menu/hub.
        static void ExitCave()
        {
            Game.Branzen += lootBranzen;

            lootBranzen = 0;

            Game.inTheCave = false;

            Console.Clear();
            Game.WrtL("You escaped!");
            Game.WrtL("\nYou now have " + Game.Branzen + " Branzen to spend!");

            if (Game.partyMembersInPeril.Count > 0)
            {
                Game.WrtL("\nThe fresh air outside the cave makes you feel better; ailments won't end you on the way back.");
            }

            Game.Wrt("\nTime: ");
            Game.WrtL("yl", "", Game.Clock(), true);

            Game.AwaitKeyEnter();

            Game.TimeUnitsPass(2, false);

            Game.GameMenu();
        }
    }
}
