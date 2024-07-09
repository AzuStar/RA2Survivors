using Godot;

namespace RA2Survivors
{
    public partial class GI : Enemy
    {
        public override EEntityType associatedEntity => EEntityType.GI;

        public override void _Ready()
        {
            base._Ready();

            stats = new Stats
            {
                attackRange = 1.9,
                damage = 10,
                maxHealth = 50000,
                movementSpeed = 0,
                expDropped = 1,
            };
            stats.health = stats.maxHealth;
        }
    }
}
