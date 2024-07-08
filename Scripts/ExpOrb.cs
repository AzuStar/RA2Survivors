using Godot;

namespace RA2Survivors
{
    public partial class ExpOrb : Node3D
    {
        public double expAmount;

        public override void _Ready()
        {
            base._Ready();
            GamemodeLevel1.instance.expOrbs.Add(this);
        }

        protected override void Dispose(bool disposing)
        {
            GamemodeLevel1.instance.expOrbs.Remove(this);
            base.Dispose(disposing);
        }
    }
}
