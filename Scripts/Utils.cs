using System;
using Godot;

namespace RA2Survivors
{
    public static class Utils
    {
        public static float RandomSign()
        {
            return GD.RandRange(0, 1) == 0 ? -1 : 1;
        }

        public static void DelayedInvoke(double delay, Action action)
        {
            Timer tim = new Timer();
            tim.Autostart = true;
            tim.OneShot = true;
            tim.WaitTime = delay;
            tim.Timeout +=
                (
                    () =>
                    {
                        tim.QueueFree();
                    }
                ) + action;
            GamemodeLevel1.instance.AddChild(tim);
        }

        public static double EaseOutElastic(double x)
        {
            var c4 = 2 * Math.PI / 3;
            return x == 0
                ? 0
                : x == 1
                ? 1
                : Math.Pow(2, -10 * x) * Math.Sin((x * 10 - 0.75) * c4) + 1;
        }
        public static double EaseOutCirc(double x)
        {
            return Math.Sqrt(1 - Math.Pow(x - 1, 2));
        }

        public static double EaseInExpo(double x)
        {
            return x == 0 ? 0 : Math.Pow(2, 10 * x - 10);
        }
    }
}
