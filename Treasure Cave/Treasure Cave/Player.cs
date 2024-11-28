namespace TreasureCave
{
    public class Player:Hero
    {
        // Constructor
        public Player()
        {
            AddIdStuff(this);

            extraPoints = 7;

            baseHealth = 0;
            baseStrength = 0;
            baseStamina = 0;
            baseSpeed = 0;

            level = 1;
            dualWieldLevel = 1;
            experience = 0;
            dualWieldExperience = 0;

            name = "Stranger";
            gender = null;
            maxHealth = 0;
            healthpoints = 0;
            maxStrength = 0;
            strength = 0;
            maxStamina = 0;
            stamina = 0;
            maxSpeed = 0;
            speed = 0;
            composure = 0;

            chanceToCounterAttack = maxChanceToCounter;
            dualWieldDice = -4;

            armor = 0;

            description = "";
            battlecry = "";

            restTime = 0;
        }
    }
}
