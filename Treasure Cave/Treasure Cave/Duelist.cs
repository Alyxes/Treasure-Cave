namespace TreasureCave
{
    public class Duelist:Hero
    {
        // Constructor
        public Duelist()
        {
            // Creates a class Duelist Hero, randomizes their stats.
            randGender(this);

            AddIdStuff(this);

            warriorTypeIndex = 4;
            baseHealth = Game.warriorBaseStatArray[warriorTypeIndex, 0];
            baseStrength = Game.warriorBaseStatArray[warriorTypeIndex, 1];
            baseStamina = Game.warriorBaseStatArray[warriorTypeIndex, 2];
            baseSpeed = Game.warriorBaseStatArray[warriorTypeIndex, 3];
            baseComposure = Game.warriorBaseStatArray[warriorTypeIndex, 4];
            composure = baseComposure;

            chanceToCounterAttack = maxChanceToCounter;
            dualWieldDice = -4;

            name = NameGenerator(gender);
            type = "duelist";

            level = 1; // A randomized number around the average party level should be here eventually.
            dualWieldLevel = 1;
            experience = 0;
            dualWieldExperience = 0;

            do
            {
                extraPoints = 7;

                addedStrengthPoints = UsePoints(Game.randomize.Next(2, 5), this); // +
                addedSpeedPoints = UsePoints(Game.randomize.Next(2, 4), this); // +
                addedStaminaPoints = UsePoints(Game.randomize.Next(1, 4), this);
                
                if (extraPoints <= 2 && extraPoints >= 0)
                {
                    // Uses the rest of the points IF they're within acceptable amount.
                    addedHealthPoints = extraPoints;
                    extraPoints = 0;
                }
                // Otherwise, do nothing and let the while loop do its thing.
            }
            while (extraPoints != 0);

            equippedArmor = randArmor(level, "armor", "None");
            warriorGear[1] = equippedArmor;

            int randW = Game.randomize.Next(9);
            string weapon = Gear.weaponClasses[randW];
            choiceOfWeapon.Add(weapon);

            randW = Game.randomize.Next(3);
            string size = Gear.sizes[randW];
            choiceOfWeapon.Add(size);

            isDualWielding = Game.RandomizeBool(12);
            if (isDualWielding && ((choiceOfWeapon[0] == "spear" && choiceOfWeapon[1] == "big") || choiceOfWeapon[0] == "bow" || choiceOfWeapon[0] == "crossbow"))
                isDualWielding = false; // With above combinations, a level 1 warrior can't dual wield any of them.

            equippedWeapon = randWeapon(this, level, choiceOfWeapon[0], choiceOfWeapon[1], "first", "None");
            warriorGear[2] = equippedWeapon;

            if (isDualWielding)
            {
                equippedSecondaryWeapon = randWeapon(this, level, choiceOfWeapon[0], choiceOfWeapon[1], "second", "None");
                warriorGear[3] = equippedSecondaryWeapon;
            }
            else
            {
                if (!isUsingDoubleHandedWeapon)
                {
                    equippedShield = randArmor(level, "shield", "None");
                    warriorGear[4] = equippedShield;
                }
            }

            UpdateWarriorStatsBeginning(this);

            cost = Game.randomize.Next(380, 551);
            restTime = 0;

            battlecry = randCry(warriorTypeIndex);
            description = Game.warriorDescriptions[warriorTypeIndex];
        }
    }
}
