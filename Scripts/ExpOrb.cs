using Godot;

namespace RA2Survivors
{
    public partial class ExpOrb : Node3D
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
                double distance = GlobalPosition.DistanceTo(magnetTarget.GlobalPosition);
                if (distance < 1)
                {
                    magnetTarget.AddExp(expAmount);
                    QueueFree();
                }
                else
                {
                    Vector3 direction = (magnetTarget.GlobalPosition - GlobalPosition).Normalized();

                    GlobalPosition += direction * (float)(movementSpeed * delta);
                    movementSpeed += movementSpeedGrow * delta;
                }
            }
        }
    }
}
