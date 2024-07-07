using Godot;

namespace RA2Survivors
{
	public partial class PauseService : Node
	{
		public static PauseService instance { get; private set; }
		public static bool IsPaused = false;

		public override void _Ready()
		{
			instance = this;
		}

		public static void PauseGame()
		{
			IsPaused = true;
			instance.GetTree().Paused = true;
		}

		public static void UnpauseGame()
		{
			IsPaused = false;
			instance.GetTree().Paused = false;
		}
	}
}
