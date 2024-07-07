using System.Collections.Generic;
using Godot;

namespace RA2Survivors
{
    public partial class AK47 : Weapon
    {
        public override void _Ready()
        {
            base._Ready();
            damageMultiplier = 0.33;
            burstCount = 3;
            burstDelay = 0.05;
            reloadSpeed = 0.95;
            multishot = 1;
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
        }

        public override void Shoot(List<Enemy> target)
        {
            foreach (var t in target)
            {
                GD.Print($"AK47 shoot {t.Name} to death!");
                t.Die();
            }
        }

        public override List<Enemy> GetTargets()
        {
            return GamemodeLevel1.GetClosestEnemiesToPlayer(multishot);
        }
    }
}
