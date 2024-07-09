using Godot;

namespace RA2Survivors
{
    public partial class HealthBar : ProgressBar
    {
        public static HealthBar instance { get; private set; }

        static Theme ThemeGreen;
        static Theme ThemeYellow;
        static Theme ThemeRed;

        public override void _Ready()
        {
            instance = this;

            ThemeGreen = ResourceLoader
                .Load<Theme>("Assets/UI/healthbar_theme.tres");
            ThemeYellow = ResourceLoader
                .Load<Theme>("Assets/UI/healthbar_theme_yellow.tres");
            ThemeRed = ResourceLoader
                .Load<Theme>("Assets/UI/healthbar_theme_red.tres");
        }

        public static void SetHealth(double currentHp, double maxHp)
        {
            double value = currentHp / maxHp;
            if (value > 0.5)
            {
                instance.Theme = ThemeGreen;
            } else if (value > 0.2)
            {
                instance.Theme = ThemeYellow;
            } else {
                instance.Theme = ThemeRed;
            }
            instance.Value = currentHp;
            instance.MaxValue = maxHp;
        }
    }
}
