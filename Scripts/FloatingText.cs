using Godot;

namespace RA2Survivors
{
    public partial class FloatingText : Label3D
    {
        public Color Color
        {
            get => Modulate;
            set { Modulate = value; }
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
        }
    }
}
