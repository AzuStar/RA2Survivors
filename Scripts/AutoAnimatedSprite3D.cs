using System;
using Godot;

namespace RA2Survivors
{
    public partial class AutoAnimatedSprite3D : Sprite3D
    {
        public const string MASTER_ASSET_PATH = "res://Prefabs/";
        [Export]
        private double AnimationTime = 1.0;
        [Export]
        private bool Loop = false;
        [Export]
        private double DelayBetweenLoops = 0.0;
        private bool Finished = false;
        private double CurrentFrameTime = 0.0;

        public override void _Ready()
        {
            base._Ready();
        }

        public override void _Process(double delta)
        {
            if (!Finished)
            {
                CurrentFrameTime += delta;

                if (CurrentFrameTime > AnimationTime)
                {
                    if (Loop)
                    {
                        if (CurrentFrameTime > AnimationTime + DelayBetweenLoops)
                        {
                            CurrentFrameTime -= AnimationTime + DelayBetweenLoops;
                        }
                    }
                    else
                    {
                        Finished = true;
                    }
                }
                Frame = (int)
                    Math.Floor(
                        Double.Lerp(0, Vframes * Hframes - 1, Math.Min(AnimationTime, CurrentFrameTime) / AnimationTime)
                    );
            }
        }
    }
}
