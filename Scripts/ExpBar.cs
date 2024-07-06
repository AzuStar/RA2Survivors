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

        public static void SetExp(float exp)
        {
            instance.Value = exp;
        }
    }
}
