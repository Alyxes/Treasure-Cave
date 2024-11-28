namespace TreasureCave
{
    public class BountyHunter:Hero
    {
        // Constructor
        public BountyHunter()
        {
            // Creates a class BountyHunter Hero, randomizes their stats.
            randGender(this);

            AddIdStuff(this);

            warriorTypeIndex = 0;
            baseHealth = Game.warriorBaseStatArray[warriorTypeIndex, 0];
            baseStrength = Game.warriorBaseStatArray[warriorTypeIndex, 1];
            baseStamina = Game.warriorBaseStatArray[warriorTypeIndex, 2];
            baseSpeed = Game.warriorBaseStatArray[warriorTypeIndex, 3];
            baseComposure = Game.warriorBaseStatArray[warriorTypeIndex, 4];
            composure = baseComposure;

            chanceToCounterAttack = maxChanceToCounter;
            dualWieldDice = -4;

            name = NameGenerator(gender);
            type = "bounty hunter";

            level = 1; // A randomized number around the average party level should be here eventually.
            dualWieldLevel = 1;
            experience = 0;
            dualWieldExperience = 0;

            do
            {
                extraPoints = 7;

                addedStrengthPoints = UsePoints(Game.randomize.Next(2, 4), this); // +
                addedStaminaPoints = UsePoints(Game.randomize.Next(2, 4), this); // +
                addedHealthPoints = UsePoints(Game.randomize.Next(0, 3), this);

                if (extraPoints <= 3 && extraPoints >= 0)
                {
                    // Uses the rest of the points IF they're within acceptable amount.
                    addedSpeedPoints = extraPoints;
                    extraPoints = 0;
                }
                // Otherwise, do nothing and let the while loop do its thing.
            }
            while (extraPoints != 0);

            equippedArmor = randArmor(level, "armor", "None");
            warriorGear[1] = equippedArmor;

            choiceOfWeapon.Add(Game.warriorPreferredWeaponry[warriorTypeIndex, 0]);
            choiceOfWeapon.Add(Game.warriorPreferredWeaponry[warriorTypeIndex, 1]);

            isDualWielding = Game.RandomizeBool(5);

            equippedWeapon = randWeapon(this, level, choiceOfWeapon[0], choiceOfWeapon[1], "first", "None");
            warriorGear[2] = equippedWeapon;

            if (isDualWielding)
            {
                equippedSecondaryWeapon = randWeapon(this, level, choiceOfWeapon[0], choiceOfWeapon[1], "second","None");
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

            cost = Game.randomize.Next(350, 451);
            restTime = 0;

            battlecry = randCry(warriorTypeIndex);
            description = Game.warriorDescriptions[warriorTypeIndex];
        }
    }
}
