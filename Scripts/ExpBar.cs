using Godot;

namespace RA2Survivors
{
    public partial class ExpBar : ProgressBar
    {
        public static ExpBar instance { get; private set; }

        public override void _Ready()
        {
            instance = this;
        }

        public static void SetExp(double exp, double nextLevel)
        {
            instance.Value = exp;
            instance.MaxValue = nextLevel;
        }
    }
}
