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
            GamemodeLevel1.instance.GetTree().Root.AddChild(tim);
        }
    }
}
