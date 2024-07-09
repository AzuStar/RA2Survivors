using System;
using Godot;

namespace RA2Survivors
{
    public abstract partial class Projectile : Node3D
    {
        public float projectileSpeed = 10;
        public Action callback;
    }
}
