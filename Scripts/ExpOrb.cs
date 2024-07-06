using Godot;

namespace RA2Survivors
{
    public partial class ExpOrb : RigidBody3D
    {
        public double expAmount;

        public override void _Ready()
        {
            SetPhysicsProcess(true);
        }

        // public override void _PhysicsProcess(float delta)
        // {
        //     if (IsInstanceValid(this))
        //     {
        //         if (
        //             GlobalTransform.Origin.DistanceTo(
        //                 GamemodeLevel1.instance.player.GlobalTransform.Origin
        //             ) < 1
        //         )
        //         {
        //             GamemodeLevel1.instance.player.stats.currentExp += expAmount;
        //             QueueFree();
        //         }
        //     }
        // }
    }
}
