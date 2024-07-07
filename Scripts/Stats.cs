using Godot;

namespace RA2Survivors
{
    public struct Stats
    {
        public double attackSpeed;
        public double damage;
        public double health;
        public double maxHealth;
        public double healthRegen;

        public double movementSpeed;

        public double expDropped;
        public double currentExp;
        public int level;

        public double expGainRate;

        // + operator

        public static Stats operator +(Stats a, Stats b)
        {
            Stats result = new Stats();
            result.attackSpeed = a.attackSpeed + b.attackSpeed;
            result.damage = a.damage + b.damage;
            result.health = a.health + b.health;
            result.maxHealth = a.maxHealth + b.maxHealth;
            result.healthRegen = a.healthRegen + b.healthRegen;
            result.movementSpeed = a.movementSpeed + b.movementSpeed;
            result.expDropped = a.expDropped + b.expDropped;
            result.currentExp = a.currentExp + b.currentExp;
            result.level = a.level + b.level;
            result.expGainRate = a.expGainRate + b.expGainRate;
            return result;
        }

        // - operator
        public static Stats operator -(Stats a, Stats b)
        {
            Stats result = new Stats();
            result.attackSpeed = a.attackSpeed - b.attackSpeed;
            result.damage = a.damage - b.damage;
            result.health = a.health - b.health;
            result.maxHealth = a.maxHealth - b.maxHealth;
            result.healthRegen = a.healthRegen - b.healthRegen;
            result.movementSpeed = a.movementSpeed - b.movementSpeed;
            result.expDropped = a.expDropped - b.expDropped;
            result.currentExp = a.currentExp - b.currentExp;
            result.level = a.level - b.level;
            result.expGainRate = a.expGainRate - b.expGainRate;
            return result;
        }
    }
}
