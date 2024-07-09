using Godot;
using RA2Survivors;

namespace RA2Survivors
{
    public partial class ExplosiveShell : Projectile
    {
        public Vector3 targetPosition;
        public float arcHeight = 3f;
        public Vector3 startPosition;
        private float elapsedTime = 0f;
        private float travelTime;

        public ExplosiveShell()
        {
            projectileSpeed = 50;
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

            float distance = startPosition.DistanceTo(targetPosition);
            float currentDistance = startPosition.DistanceTo(newPosition);
            float height = Mathf.Sin(Mathf.Pi * (currentDistance / distance)) * arcHeight;

            GlobalPosition = new Vector3(newPosition.X, newPosition.Y, newPosition.Z - height);

            if (t >= 1.0f)
            {
                callback();
                QueueFree();
            }
        }
    }
}
