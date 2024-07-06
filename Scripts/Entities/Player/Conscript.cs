namespace RA2Survivors
{
    public partial class Conscript : Player
    {
        public override void _Ready()
        {
            base._Ready();
            stats = new Stats
            {
                attackSpeed = 1,
                damage = 9,
                maxHealth = 100,
                healthRegen = 0.01,
                movementSpeed = 5,
                expGainRate = 1
            };
            stats.currentHealth = stats.maxHealth;
        }
    }
}
