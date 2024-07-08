using Godot;

namespace RA2Survivors
{
	public partial class Seal : Enemy
	{
		public override EEntityType associatedEntity => EEntityType.Seal;

		public override void _Ready()
		{
			base._Ready();

			stats = new Stats
			{
				attackRange = 1.9,
				damage = 10,
				maxHealth = 5,
				movementSpeed = 2,
				expDropped = 1,
			};
			stats.health = stats.maxHealth;
		}
	}
}
