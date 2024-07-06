using Godot;

namespace RA2Survivors
{
    public partial class GI : Enemy
    {
        public override void _Ready()
        {
            base._Ready();
            stats = new Stats
            {
                damage = 10,
                maxHealth = 5,
                movementSpeed = 2,
                expDropped = 1,
            };
            stats.currentHealth = stats.maxHealth;
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);

            GD.Print(Transform.Origin);
        }
    }
}
