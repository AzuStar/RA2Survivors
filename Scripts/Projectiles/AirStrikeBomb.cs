using Godot;
using RA2Survivors;

namespace RA2Survivors
{
    public partial class AirStrikeBomb : Projectile
    {
        public Vector3 targetPosition;
        public Vector3 startPosition;
        private float elapsedTime = 0f;
        private float travelTime;

        public AirStrikeBomb()
        {
            projectileSpeed = 30;
        }

        public override void _Ready()
        {
            base._Ready();
            GlobalPosition = startPosition;
            travelTime = startPosition.DistanceTo(targetPosition) / projectileSpeed;
        }

        public override void _Process(double delta)
        {
            base._Process(delta);

            elapsedTime += (float)delta;
            float t = Mathf.Clamp(elapsedTime / travelTime, 0, 1);

            Vector3 newPosition = startPosition.Lerp(targetPosition, t);

            if (t >= 1.0f)
            {
                callback();
                QueueFree();
            }
        }
    }
}
