using Godot;

namespace RA2Survivors
{
    public partial class ExpOrb : Pickup
    {
        public double expAmount;
        public Player magnetTarget;

        public double movementSpeedGrow = 12;
        private double movementSpeed = 3;

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            if (magnetTarget != null)
            {
                Vector3 magnetOffsetPosition = magnetTarget.GlobalPosition + new Vector3(0, 0, -2);
                double distance = GlobalPosition.DistanceTo(magnetOffsetPosition);
                if (distance < 2)
                {
                    // prevent lvl up popup on end screen
                    if (!magnetTarget.dead)
                    {
                        magnetTarget.AddExp(expAmount);
                    }
                    QueueFree();
                }
                else
                {
                    Vector3 direction = (magnetOffsetPosition - GlobalPosition).Normalized();

                    GlobalPosition += direction * (float)(movementSpeed * delta);
                    movementSpeed += movementSpeedGrow * delta;
                }
            }
        }
    }
}
