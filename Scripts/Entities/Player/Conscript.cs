namespace RA2Survivors
{
	public partial class Conscript : Player
	{
		public AK47 akWeapon;
		public ForTheUnion ftuPassive;

		public override void _Ready()
		{
			base._Ready();
			stats = new Stats
			{
				attackSpeed = 1,
				damage = 9,
				maxHealth = 100,
				healthRegen = 0.01,
				movementSpeed = 10,
				expGainRate = 1,
				magnetRange = 2.5,
			};
			stats.health = stats.maxHealth;
			akWeapon = new AK47();
			AddChild(akWeapon);
			ftuPassive = new ForTheUnion();
			availableUpgrades.AddRange(ftuPassive.upgrades);
			AddChild(ftuPassive);

			Sprite.AnimDefinitions.Add(
				"death",
				new RA2SpriteAnim()
				{
					StartFrame = 0,
					EndFrame = 14,
					Loop = false
				}
			);
			Sprite.AnimDefinitions.Add(
				"face_n",
				new RA2SpriteAnim()
				{
					StartFrame = 15,
					EndFrame = 15,
					Loop = true
				}
			);
			Sprite.AnimDefinitions.Add(
				"face_nw",
				new RA2SpriteAnim()
				{
					StartFrame = 16,
					EndFrame = 16,
					Loop = true
				}
			);
			Sprite.AnimDefinitions.Add(
				"face_w",
				new RA2SpriteAnim()
				{
					StartFrame = 17,
					EndFrame = 17,
					Loop = true
				}
			);
			Sprite.AnimDefinitions.Add(
				"face_sw",
				new RA2SpriteAnim()
				{
					StartFrame = 18,
					EndFrame = 18,
					Loop = true
				}
			);
			Sprite.AnimDefinitions.Add(
				"face_s",
				new RA2SpriteAnim()
				{
					StartFrame = 19,
					EndFrame = 19,
					Loop = true
				}
			);
			Sprite.AnimDefinitions.Add(
				"face_se",
				new RA2SpriteAnim()
				{
					StartFrame = 20,
					EndFrame = 20,
					Loop = true
				}
			);
			Sprite.AnimDefinitions.Add(
				"face_e",
				new RA2SpriteAnim()
				{
					StartFrame = 21,
					EndFrame = 21,
					Loop = true
				}
			);
			Sprite.AnimDefinitions.Add(
				"face_ne",
				new RA2SpriteAnim()
				{
					StartFrame = 22,
					EndFrame = 22,
					Loop = true
				}
			);
			Sprite.AnimDefinitions.Add(
				"fire_n",
				new RA2SpriteAnim()
				{
					StartFrame = 23,
					EndFrame = 23 + 5,
					Loop = false
				}
			);
			Sprite.AnimDefinitions.Add(
				"fire_nw",
				new RA2SpriteAnim()
				{
					StartFrame = 29,
					EndFrame = 29 + 5,
					Loop = false
				}
			);
			Sprite.AnimDefinitions.Add(
				"fire_w",
				new RA2SpriteAnim()
				{
					StartFrame = 35,
					EndFrame = 35 + 5,
					Loop = false
				}
			);
			Sprite.AnimDefinitions.Add(
				"fire_sw",
				new RA2SpriteAnim()
				{
					StartFrame = 41,
					EndFrame = 41 + 5,
					Loop = false
				}
			);
			Sprite.AnimDefinitions.Add(
				"fire_s",
				new RA2SpriteAnim()
				{
					StartFrame = 47,
					EndFrame = 47 + 5,
					Loop = false
				}
			);
			Sprite.AnimDefinitions.Add(
				"fire_se",
				new RA2SpriteAnim()
				{
					StartFrame = 53,
					EndFrame = 53 + 5,
					Loop = false
				}
			);
			Sprite.AnimDefinitions.Add(
				"fire_e",
				new RA2SpriteAnim()
				{
					StartFrame = 59,
					EndFrame = 59 + 5,
					Loop = false
				}
			);
			Sprite.AnimDefinitions.Add(
				"fire_ne",
				new RA2SpriteAnim()
				{
					StartFrame = 65,
					EndFrame = 65 + 5,
					Loop = false
				}
			);
			Sprite.AnimDefinitions.Add(
				"run_n",
				new RA2SpriteAnim()
				{
					StartFrame = 86,
					EndFrame = 86 + 5,
					Loop = true
				}
			);
			Sprite.AnimDefinitions.Add(
				"run_nw",
				new RA2SpriteAnim()
				{
					StartFrame = 92,
					EndFrame = 92 + 5,
					Loop = true
				}
			);
			Sprite.AnimDefinitions.Add(
				"run_w",
				new RA2SpriteAnim()
				{
					StartFrame = 98,
					EndFrame = 98 + 5,
					Loop = true
				}
			);
			Sprite.AnimDefinitions.Add(
				"run_sw",
				new RA2SpriteAnim()
				{
					StartFrame = 104,
					EndFrame = 104 + 5,
					Loop = true
				}
			);
			Sprite.AnimDefinitions.Add(
				"run_s",
				new RA2SpriteAnim()
				{
					StartFrame = 110,
					EndFrame = 110 + 5,
					Loop = true
				}
			);
			Sprite.AnimDefinitions.Add(
				"run_se",
				new RA2SpriteAnim()
				{
					StartFrame = 116,
					EndFrame = 116 + 5,
					Loop = true
				}
			);
			Sprite.AnimDefinitions.Add(
				"run_e",
				new RA2SpriteAnim()
				{
					StartFrame = 122,
					EndFrame = 122 + 5,
					Loop = true
				}
			);
			Sprite.AnimDefinitions.Add(
				"run_ne",
				new RA2SpriteAnim()
				{
					StartFrame = 128,
					EndFrame = 128 + 5,
					Loop = true
				}
			);
		}
	}
}
