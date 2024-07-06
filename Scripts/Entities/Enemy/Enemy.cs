using Godot;

namespace RA2Survivors
{
    public abstract partial class Enemy : Entity
    {
        protected RA2Sprite3D Sprite; 

        public override void _Ready()
        {
            base._Ready();
            GamemodeLevel1.Instance.totalEnemiesCount++;
            GamemodeLevel1.Instance.enemyCount[(int)associatedEntity]++;
			Sprite = (RA2Sprite3D)FindChild("Sprite3D");
        }

        // public override void _PhysicsProcess(double delta)
        // {
        //     if (GamemodeLevel1.Instance.player == null)
        //         return;

        //     Vector3 direction = (
        //         GamemodeLevel1.Instance.player.GlobalTransform.Origin - GlobalTransform.Origin
        //     ).Normalized();

        //     Vector3 horizontalVelocity = direction * (float)stats.movementSpeed;
        // }

        public override void _IntegrateForces(PhysicsDirectBodyState3D state)
        {
            if (GamemodeLevel1.Instance.player == null)
                return;

            Vector3 direction = (
                GamemodeLevel1.Instance.player.GlobalTransform.Origin - GlobalTransform.Origin
            ).Normalized();
            Vector3 velocity = direction * (float)stats.movementSpeed;
            state.LinearVelocity = velocity;

			if (velocity.X == 0 && velocity.Z == 0)
			{
				Sprite.PlayAnim("face_s");
			} else if (velocity.X == 0 && velocity.Z < 0)
			{
				Sprite.PlayAnim("run_n");
			}
			else if (velocity.X < 0 && velocity.Z < 0)
			{
				Sprite.PlayAnim("run_nw");
			}
			else if (velocity.X < 0 && velocity.Z == 0)
			{
				Sprite.PlayAnim("run_w");
			}
			else if (velocity.X < 0 && velocity.Z > 0)
			{
				Sprite.PlayAnim("run_sw");
			}
			else if (velocity.X == 0 && velocity.Z > 0)
			{
				Sprite.PlayAnim("run_s");
			}
			else if (velocity.X > 0 && velocity.Z > 0)
			{
				Sprite.PlayAnim("run_se");
			}
			else if (velocity.X > 0 && velocity.Z == 0)
			{
				Sprite.PlayAnim("run_e");
			}
			else if (velocity.X > 0 && velocity.Z < 0)
			{
				Sprite.PlayAnim("run_ne");
			}
        }

        protected override void Dispose(bool disposing)
        {
            GamemodeLevel1.Instance.totalEnemiesCount--;
            GamemodeLevel1.Instance.enemyCount[(int)associatedEntity]--;
            base.Dispose(disposing);
        }
    }
}
