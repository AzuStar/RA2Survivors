using Godot;

namespace RA2Survivors
{
	public partial class Tanya : Enemy
	{
		public override EEntityType associatedEntity => EEntityType.Tanya;

		public override void _Ready()
		{
			base._Ready();

			stats = new Stats
			{
				attackRange = 2.1,
				damage = 10,
				maxHealth = 5,
				movementSpeed = 2,
				expDropped = 1,
			};
			stats.health = stats.maxHealth;
		}
	}
}
