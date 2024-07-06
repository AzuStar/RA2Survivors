using System;
using System.Collections.Generic;
using Godot;

namespace RA2Survivors
{
    public struct RA2SpriteAnim
    {
        public RA2SpriteAnim(int startFrame, int endFrame)
        {
            StartFrame = startFrame;
            EndFrame = endFrame;
        }

        public int StartFrame;
        public int EndFrame;
        public bool Loop = false;
    }

    public partial class RA2Sprite3D : Sprite3D
    {
        public Dictionary<string, RA2SpriteAnim> AnimDefinitions =
            new Dictionary<string, RA2SpriteAnim>();
        private string CurrentAnim;
        private RA2SpriteAnim CurrentAnimDef;
        private double CurrentFrameTime;

        private double FrameTime = 1.0 / 5; // TODO this is wrong lol

        // Called when the node enters the scene tree for the first time.
        public override void _Ready() { }

        public void PlayAnim(string anim, bool force = false)
        {
            if (force || anim != CurrentAnim)
            {
                CurrentAnim = anim;
                CurrentAnimDef = AnimDefinitions[anim];
                Frame = CurrentAnimDef.StartFrame;
                CurrentFrameTime = 0.0;
            }
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            if (CurrentAnim != null)
            {
                CurrentFrameTime += delta;
                if (CurrentAnimDef.Loop && CurrentFrameTime > FrameTime)
                {
                    CurrentFrameTime -= FrameTime;
                }
                else if (!CurrentAnimDef.Loop && CurrentFrameTime > FrameTime)
                {
                    CurrentFrameTime = FrameTime;
                }
                Frame = (int)
                    Math.Floor(
                        Double.Lerp(
                            CurrentAnimDef.StartFrame,
                            CurrentAnimDef.EndFrame,
                            CurrentFrameTime / FrameTime
                        )
                    );
            }
        }
    }
}
