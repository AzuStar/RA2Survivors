using Godot;

namespace RA2Survivors
{
    public abstract partial class Entity : CharacterBody3D
    {
        public abstract EEntityType associatedEntity { get; }
        public Stats stats;
    }
}
