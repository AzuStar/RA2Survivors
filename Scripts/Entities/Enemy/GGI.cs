using Godot;

namespace RA2Survivors
{
    public partial class GGI : Enemy
    {
        public override EEntityType associatedEntity => EEntityType.GGI;

        public override void _Ready()
        {
            base._Ready();

            stats = new Stats
            {
                attackRange = 2,
                damage = 100,
                maxHealth = 50,
                movementSpeed = 3,
                expDropped = 5,
            };
            stats.health = stats.maxHealth;
        }
    }
}
