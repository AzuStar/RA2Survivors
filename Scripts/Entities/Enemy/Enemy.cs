using Godot;

namespace RA2Survivors
{
    public abstract partial class Enemy : Entity
    {
        public override void _Ready()
        {
            base._Ready();
            GamemodeLevel1.Instance.totalEnemiesCount++;
            GamemodeLevel1.Instance.enemyCount[(int)associatedEntity]++;
        }

        public override void _PhysicsProcess(double delta)
        {
            if (GamemodeLevel1.Instance.player == null)
                return;

            Vector3 direction = (
                GamemodeLevel1.Instance.player.GlobalTransform.Origin - GlobalTransform.Origin
            ).Normalized();
        }

        protected override void Dispose(bool disposing)
        {
            GamemodeLevel1.Instance.totalEnemiesCount--;
            GamemodeLevel1.Instance.enemyCount[(int)associatedEntity]--;
            base.Dispose(disposing);
        }
    }
}
