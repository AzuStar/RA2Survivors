using Godot;

namespace RA2Survivors
{
	public partial class Spy : Enemy
	{
		public override EEntityType associatedEntity => EEntityType.Spy;

		public override void _Ready()
		{
			base._Ready();

			stats = new Stats
			{
				attackRange = 2,
				damage = 10,
				maxHealth = 5,
				movementSpeed = 2,
				expDropped = 1,
			};
			stats.health = stats.maxHealth;
		}
	}
}
