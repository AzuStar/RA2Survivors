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
        public static string animDirFromVelocity(Vector3 velocity)
        {
            if (velocity.Length() < 0.01)
            {
               return "s";
            }
            else if (velocity.X == 0 && velocity.Z < 0)
            {
                return "n";
            }
            else if (velocity.X < 0 && velocity.Z < 0)
            {
                return "nw";
            }
            else if (velocity.X < 0 && velocity.Z == 0)
            {
                return "w";
            }
            else if (velocity.X < 0 && velocity.Z > 0)
            {
                return "sw";
            }
            else if (velocity.X == 0 && velocity.Z > 0)
            {
                return "s";
            }
            else if (velocity.X > 0 && velocity.Z > 0)
            {
                return "se";
            }
            else if (velocity.X > 0 && velocity.Z == 0)
            {
                return "e";
            }
            else if (velocity.X > 0 && velocity.Z < 0)
            {
                return "ne";
            }
            return "s";
        }

        public Dictionary<string, RA2SpriteAnim> AnimDefinitions =
            new Dictionary<string, RA2SpriteAnim>();
        public string CurrentAnim;
        public bool CurrentAnimFinished;
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
                CurrentAnimFinished = false;
                CurrentAnimDef = AnimDefinitions[anim];
                Frame = CurrentAnimDef.StartFrame;
                CurrentFrameTime = 0.0;
            }
        }
        public void PlayAnimWithDir(string anim, Vector3 velocity, bool force = false)
        {
            PlayAnim(anim + "_" + animDirFromVelocity(velocity), force);
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            if (CurrentAnim != null)
            {
                if (CurrentAnimDef.EndFrame - CurrentAnimDef.StartFrame > 0)
                {
                    CurrentFrameTime += delta;
                    if (CurrentAnimDef.Loop && CurrentFrameTime > FrameTime)
                    {
                        CurrentFrameTime -= FrameTime;
                    }
                }
                if (!CurrentAnimDef.Loop && CurrentFrameTime > FrameTime)
                {
                    CurrentFrameTime = FrameTime;
                    CurrentAnimFinished = true;
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
