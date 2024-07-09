using Godot;

namespace RA2Survivors
{
    public partial class AttackDog : Enemy
    {
        public override EEntityType associatedEntity => EEntityType.AttackDog;

        public override void _Ready()
        {
            base._Ready();

            stats = new Stats
            {
                attackRange = 1.8,
                damage = 50,
                maxHealth = 5,
                movementSpeed = 14,
                expDropped = 1,
            };
            stats.health = stats.maxHealth;
        }
    }
}
