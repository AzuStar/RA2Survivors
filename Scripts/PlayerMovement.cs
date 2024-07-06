using Godot;

namespace RASurvivors
{
	public partial class PlayerMovement : CharacterBody3D
	{
		[Export]
		public double speed = 200;

		private Vector3 _velocity = Vector3.Zero;

		public override void _Ready() { }

		public override void _PhysicsProcess(double delta)
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
			Vector3 horizontalVelocity = direction * (float)speed * (float)delta;
			_velocity.X = horizontalVelocity.X;
			_velocity.Z = horizontalVelocity.Z;

			Velocity = _velocity;
			MoveAndSlide();
		}
	}
}
