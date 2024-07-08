using Godot;

namespace RA2Survivors
{
	public partial class GGI : Enemy
	{
		public override EEntityType associatedEntity => EEntityType.GGI;

		public override void _Ready()
		{
			base._Ready();

			stats = new Stats
			{
				damage = 10,
				maxHealth = 5,
				movementSpeed = 2,
				expDropped = 1,
			};
			stats.health = stats.maxHealth;
		}
	}
}
