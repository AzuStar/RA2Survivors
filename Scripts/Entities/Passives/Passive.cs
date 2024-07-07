using Godot;

namespace RA2Survivors
{
    public abstract partial class Passive : Node
    {
        public Player owner;

        public override void _Ready()
        {
            owner = GetParent<Player>();
        }
    }
}
