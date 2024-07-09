using Godot;

namespace RA2Survivors
{
    public partial class Seal : Enemy
    {
        public override EEntityType associatedEntity => EEntityType.Seal;

        public Seal()
        {
            stats = new Stats
            {
                attackRange = 1.9,
                damage = 20,
                maxHealth = 12,
                movementSpeed = 5.5,
                expDropped = 3,
            };
        }
    }
}
