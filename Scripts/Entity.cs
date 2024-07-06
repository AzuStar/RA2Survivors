using Godot;

namespace RA2Survivors
{
    public abstract partial class Entity : RigidBody3D
    {
        public abstract EEntityType associatedEntity { get; }
        public Stats stats;
    }
}
