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
			stats.currentHealth = stats.maxHealth;
			
			Sprite.AnimDefinitions.Add("face_s",new RA2SpriteAnim() {
				StartFrame = 4,
				EndFrame = 4,
				Loop = true
			});
			Sprite.AnimDefinitions.Add("run_n",new RA2SpriteAnim() {
				StartFrame = 8,
				EndFrame = 8+5,
				Loop = true
			});
			Sprite.AnimDefinitions.Add("run_nw",new RA2SpriteAnim() {
				StartFrame = 14,
				EndFrame = 14+5,
				Loop = true
			});
			Sprite.AnimDefinitions.Add("run_w",new RA2SpriteAnim() {
				StartFrame = 20,
				EndFrame = 20+5,
				Loop = true
			});
			Sprite.AnimDefinitions.Add("run_sw",new RA2SpriteAnim() {
				StartFrame = 26,
				EndFrame = 26+5,
				Loop = true
			});
			Sprite.AnimDefinitions.Add("run_s",new RA2SpriteAnim() {
				StartFrame = 32,
				EndFrame = 32+5,
				Loop = true
			});
			Sprite.AnimDefinitions.Add("run_se",new RA2SpriteAnim() {
				StartFrame = 38,
				EndFrame = 38+5,
				Loop = true
			});
			Sprite.AnimDefinitions.Add("run_e",new RA2SpriteAnim() {
				StartFrame = 44,
				EndFrame = 44+5,
				Loop = true
			});
			Sprite.AnimDefinitions.Add("run_ne",new RA2SpriteAnim() {
				StartFrame = 50,
				EndFrame = 50+5,
				Loop = true
			});
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);
		}
	}
}
