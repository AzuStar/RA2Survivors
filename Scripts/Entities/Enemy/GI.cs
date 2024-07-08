using Godot;

namespace RA2Survivors
{
	public partial class GI : Enemy
	{
		public override EEntityType associatedEntity => EEntityType.GI;

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

			DyingSounds.Add((AudioStreamPlayer3D)FindChild("igidia"));
			DyingSounds.Add((AudioStreamPlayer3D)FindChild("igidib"));
			DyingSounds.Add((AudioStreamPlayer3D)FindChild("igidic"));
			DyingSounds.Add((AudioStreamPlayer3D)FindChild("igidid"));
			DyingSounds.Add((AudioStreamPlayer3D)FindChild("igidie"));
		}
	}
}
