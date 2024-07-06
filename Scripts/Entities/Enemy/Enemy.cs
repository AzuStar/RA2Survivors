using Godot;

namespace RA2Survivors
{
    public abstract partial class Enemy : Entity
    {
        public override void _Ready()
        {
            base._Ready();
            GamemodeLevel1.instance.totalEnemiesCount++;
            GamemodeLevel1.instance.enemyCount[(int)associatedEntity]++;
        }

        public override void _IntegrateForces(PhysicsDirectBodyState3D state)
        {
            if (GamemodeLevel1.instance.player == null)
                return;

            Vector3 direction = (
                GamemodeLevel1.instance.player.GlobalTransform.Origin - GlobalTransform.Origin
            ).Normalized();
            Vector3 velocity = direction * (float)stats.movementSpeed;
            state.LinearVelocity = velocity;
        }

        protected override void Dispose(bool disposing)
        {
            GamemodeLevel1.instance.totalEnemiesCount--;
            GamemodeLevel1.instance.enemyCount[(int)associatedEntity]--;
            base.Dispose(disposing);
        }
    }
}
