using System.Collections.Generic;
using Godot;

namespace RA2Survivors
{
    public partial class AirStrike : Weapon
    {
        public double airStrikeAoe = 5;
        public double suspiciousPackageChance;
        public double extraBombChance;

        public AirStrike()
        {
            damageMultiplier = 1.25;
            burstCount = 1;
            reloadSpeed = 5;
            multishot = 2;

            commonUpgrades =
            [
                new UpgradeButtonSettings
                {
                    title = "AirStrike: Call in the reinforcements!",
                    description = "Airstrike is called faster [color=#FF0000]+10%[/color]",
                    callback = () =>
                    {
                        reloadSpeed *= 0.9;
                    }
                },
                new UpgradeButtonSettings
                {
                    title = "AirStrike: Bigger Explosions",
                    description = "Airstrike bomb explosion AoE range [color=#FF0000]+1[/color]",
                    callback = () =>
                    {
                        airStrikeAoe += 1;
                    }
                },
                new UpgradeButtonSettings
                {
                    title = "AirStrike: Air Fleet",
                    description = "Airstrike bomb count [color=#FF0000]+1[/color]",
                    callback = () =>
                    {
                        multishot++;
                    }
                }
            ];
            uniqueUpgrades =
            [
                new UpgradeButtonSettings
                {
                    title = "AirStrike: Suspicious Package",
                    description =
                        "Grants [[color=#FF0000]10%[/color] chance, airstrike will deliver a package on top of the player that can be picked up to restore health",
                    callback = () =>
                    {
                        suspiciousPackageChance += 0.1;
                    }
                },
                new UpgradeButtonSettings
                {
                    title = "AirStrike: For the Union!",
                    description =
                        "Each airstrike bomb has [color=#FF0000]20%[/color] chance to spawn a bomb at a random position around your character",
                    callback = () =>
                    {
                        extraBombChance += 0.2;
                    }
                }
            ];
        }

        public override List<Enemy> GetTargets()
        {
            return GamemodeLevel1.GetClosestEnemiesToPlayer(multishot, owner.stats.attackRange);
        }

        public override void Shoot(List<Enemy> targets)
        {
            foreach (var t in targets)
            {
                Timer tim = new Timer();
                tim.OneShot = true;
                tim.WaitTime = GD.Randf() * 0.5;
                tim.Timeout += () =>
                {
                    foreach (var t in targets)
                    {
                        t.TakeDamage(owner, owner.stats.damage * damageMultiplier);
                    }
                };
                owner.AddChild(tim);
            }
        }
    }
}
