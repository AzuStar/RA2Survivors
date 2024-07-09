using Godot;

namespace RA2Survivors
{
    public abstract partial class Passive : Node
    {
        public Player owner;
        public UpgradeButtonSettings[] commonUpgrades;
        public UpgradeButtonSettings[] uniqueUpgrades;

        public override void _Ready()
        {
            owner = GetParent<Player>();
        }

        public abstract void Apply();

        public abstract void UnApply();
    }
}
