using Godot;

namespace RA2Survivors
{
    public partial class LifetimedResource : Timer
    {
        public LifetimedResource(double lifetime = 1.0f)
        {
            WaitTime = lifetime;
            OneShot = true;
            Autostart = true;
            Timeout += () =>
            {
                GetParent().QueueFree();
            };
        }

        public void StartLifetime(Node node)
        {
            node.AddChild(this);
        }
    }
}
