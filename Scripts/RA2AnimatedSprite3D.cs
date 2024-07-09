using System;
using Godot;

namespace RA2Survivors
{
    public partial class RA2AnimatedSprite3D : Sprite3D
    {
        public const string MASTER_ASSET_PATH = "res://Prefabs/";

        private bool Finished = true;
        private double AnimationTime;
        private double CurrentFrameTime;
        private bool Loop = false;

        public static void PlaySpriteScene(
            Vector3 globalPosition,
            string scenePath,
            double animationTime,
            double loopDuration = 0.0
        )
        {
            RA2AnimatedSprite3D sprite = (RA2AnimatedSprite3D)
                ResourceLoader
                    .Load<PackedScene>(MASTER_ASSET_PATH + scenePath)
                    .Instantiate<Sprite3D>();
            sprite.AnimationTime = animationTime;
            sprite.PixelSize = 0.1f;
            if (loopDuration > 0.0)
            {
                sprite.Loop = true;
                new LifetimedResource(loopDuration).StartLifetime(sprite);
            }
            GamemodeLevel1.instance.GetTree().Root.AddChild(sprite);
            sprite.GlobalPosition += globalPosition;
            sprite.Finished = false;
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
                        CurrentFrameTime -= AnimationTime;
                    }
                    else
                    {
                        CurrentFrameTime = AnimationTime;
                        QueueFree();
                    }
                }
                Frame = (int)
                    Math.Floor(
                        Double.Lerp(0, Vframes * Hframes - 1, CurrentFrameTime / AnimationTime)
                    );
            }
        }
    }
}
