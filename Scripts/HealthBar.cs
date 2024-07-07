using Godot;

namespace RA2Survivors
{
    public partial class HealthBar : ProgressBar
    {
        public static HealthBar instance { get; private set; }

        public override void _Ready()
        {
            instance = this;
        }

        public static void SetHealth(double currentHp, double maxHp)
        {
            instance.Value = currentHp;
            instance.MaxValue = maxHp;
        }
    }
}
