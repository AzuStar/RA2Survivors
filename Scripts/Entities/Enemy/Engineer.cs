using Godot;

namespace RA2Survivors
{
	public partial class Engineer : Enemy
	{
		public override EEntityType associatedEntity => EEntityType.Engineer;

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
