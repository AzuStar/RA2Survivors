using Godot;

namespace RA2Survivors
{
    public partial class Player : Entity
    {
        public override EEntityType associatedEntity => EEntityType.Conscript;

        private Vector3 _velocity = Vector3.Zero;
		protected RA2Sprite3D Sprite; 

        public override void _Ready() {
			Sprite = (RA2Sprite3D)FindChild("Sprite3D");
		 }

        public override void _IntegrateForces(PhysicsDirectBodyState3D state)
        {
            // Handle movement input
            Vector3 direction = Vector3.Zero;

            if (Input.IsActionPressed("MoveUp"))
            {
                direction.Z -= 1;
            }
            if (Input.IsActionPressed("MoveDown"))
            {
                direction.Z += 1;
            }
            if (Input.IsActionPressed("MoveLeft"))
            {
                direction.X -= 1;
            }
            if (Input.IsActionPressed("MoveRight"))
            {
                direction.X += 1;
            }
            direction = direction.Normalized();
            Vector3 horizontalVelocity = direction * (float)stats.movementSpeed;
            _velocity.X = horizontalVelocity.X;
            _velocity.Z = horizontalVelocity.Z;

            state.LinearVelocity = _velocity;

            // if (MoveAndSlide())
            // PushEntities();

			if (_velocity.X == 0 && _velocity.Z == 0)
			{
				Sprite.PlayAnim("face_s");
			} else if (_velocity.X == 0 && _velocity.Z < 0)
			{
				Sprite.PlayAnim("run_n");
			}
			else if (_velocity.X < 0 && _velocity.Z < 0)
			{
				Sprite.PlayAnim("run_nw");
			}
			else if (_velocity.X < 0 && _velocity.Z == 0)
			{
				Sprite.PlayAnim("run_w");
			}
			else if (_velocity.X < 0 && _velocity.Z > 0)
			{
				Sprite.PlayAnim("run_sw");
			}
			else if (_velocity.X == 0 && _velocity.Z > 0)
			{
				Sprite.PlayAnim("run_s");
			}
			else if (_velocity.X > 0 && _velocity.Z > 0)
			{
				Sprite.PlayAnim("run_se");
			}
			else if (_velocity.X > 0 && _velocity.Z == 0)
			{
				Sprite.PlayAnim("run_e");
			}
			else if (_velocity.X > 0 && _velocity.Z < 0)
			{
				Sprite.PlayAnim("run_ne");
			}
        }

        // public void PushEntities()
        // {
        //     for (int i = 0; i < GetSlideCollisionCount(); i++)
        //     {
        //         KinematicCollision3D collision = GetSlideCollision(i);
        //         if (collision.GetCollider() is Enemy)
        //         {
        //             Enemy enemy = (Enemy)collision.GetCollider();
        //             Vector3 pushDirection = (
        //                 collision.GetPosition() - GlobalTransform.Origin
        //             ).Normalized();
        //             enemy.Velocity = pushDirection * (float)10f;
        //         }
        //     }
        // }
    }
}
