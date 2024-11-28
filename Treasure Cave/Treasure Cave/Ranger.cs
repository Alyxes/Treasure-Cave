namespace TreasureCave
{
    public class Ranger:Hero
    {
        // Constructor
        public Ranger()
        {
            // Creates a class Ranger Hero, randomizes their stats.
            randGender(this);

            AddIdStuff(this);

            warriorTypeIndex = 2;
            baseHealth = Game.warriorBaseStatArray[warriorTypeIndex, 0];
            baseStrength = Game.warriorBaseStatArray[warriorTypeIndex, 1];
            baseStamina = Game.warriorBaseStatArray[warriorTypeIndex, 2];
            baseSpeed = Game.warriorBaseStatArray[warriorTypeIndex, 3];
            baseComposure = Game.warriorBaseStatArray[warriorTypeIndex, 4];
            composure = baseComposure;

            chanceToCounterAttack = maxChanceToCounter;
            dualWieldDice = -4;

            name = NameGenerator(gender);
            type = "ranger";

            level = 1; // A randomized number around the average party level should be here eventually.
            dualWieldLevel = 1;
            experience = 0;
            dualWieldExperience = 0;

            do
            {
                extraPoints = 7;

                addedSpeedPoints = UsePoints(Game.randomize.Next(2, 5), this); // ++
                addedStaminaPoints = UsePoints(Game.randomize.Next(2, 4), this); // +
                addedStrengthPoints = UsePoints(Game.randomize.Next(1, 4), this);

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

            choiceOfWeapon.Add(Game.warriorPreferredWeaponry[warriorTypeIndex, 0]);
            choiceOfWeapon.Add(Game.warriorPreferredWeaponry[warriorTypeIndex, 1]);

            isDualWielding = Game.RandomizeBool(16);
            // If the ranger is dual wielding, they cannot equip any of their favorite weapon types (ranged and spear) in level one.
            if (isDualWielding)
                equippedWeapon = randWeapon(this, level, "small", "medium", "first", "None");
            else
                equippedWeapon = randWeapon(this, level, choiceOfWeapon[0], choiceOfWeapon[1], "first", "None");

            warriorGear[2] = equippedWeapon;

            if (isDualWielding)
            {
                equippedSecondaryWeapon = randWeapon(this, level, "small", "medium", "first", "None");
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

            cost = Game.randomize.Next(380, 451);
            restTime = 0;

            battlecry = randCry(warriorTypeIndex);
            description = Game.warriorDescriptions[warriorTypeIndex];
        }
    }
}
