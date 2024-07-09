using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace RA2Survivors
{
    public partial class AK47 : Weapon
    {
        public double explosiveShellChance = 0;
        public double instantKillChance = 0.2;
        public double critChance = 0;

        public AK47()
        {
            damageMultiplier = 0.33;
            burstCount = 3;
            burstDelay = 0.05;
            reloadSpeed = 0.95;
            multishot = 1;

            commonUpgrades =
            [
                new UpgradeButtonSettings
                {
                    title = "AK47: Extended magazine shell",
                    description = "Increases burst count +1",
                    callback = () =>
                    {
                        burstCount++;
                    }
                },
                new UpgradeButtonSettings
                {
                    title = "AK47: Faster reload",
                    description = "Reload speed +10%",
                    callback = () =>
                    {
                        reloadSpeed *= 0.9;
                    }
                },
                new UpgradeButtonSettings
                {
                    title = "AK47: Critical hit",
                    description = "Increases chance to critically Strke by 5%",
                    callback = () =>
                    {
                        critChance += 0.05;
                    }
                }
            ];
            uniqueUpgrades =
            [
                new UpgradeButtonSettings
                {
                    title = "AK47: You are sure?",
                    description =
                        "Grants 11% chance to replace a burst with an explosive shell that deals damage in AoE",
                    callback = () =>
                    {
                        explosiveShellChance += 0.11;
                    }
                },
                new UpgradeButtonSettings
                {
                    title = "AK47: Attacking!",
                    description = "Slows down reload by 30%, but shoots +2 targets",
                    callback = () =>
                    {
                        reloadSpeed *= 1.3;
                        multishot += 2;
                    }
                },
                new UpgradeButtonSettings
                {
                    title = "AK47: Radiation bullets",
                    description = "Grants 4% chance to instantly kill a target",
                    callback = () =>
                    {
                        instantKillChance += 0.04;
                    }
                }
            ];
        }

        public override void _Ready()
        {
            base._Ready();
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
        }

        public override void Shoot(List<Enemy> targets)
        {
            if (_burstsLeft == burstCount && GD.Randf() < explosiveShellChance)
            {
                currentState = EWeaponState.Idle;
                owner.PlaySound("iconatc.wav");
                Timer tim = new Timer();
                tim.OneShot = true;
                tim.WaitTime = 0.15;
                tim.Timeout += () =>
                {
                    owner.PlaySound("iconatta.wav");
                    owner.Sprite.PlayAnimWithDir("fire", Vector3.Zero, true);
                    foreach (var t in targets)
                    {
                        ExplosiveShell shell = ResourceProvider.CreateResource<ExplosiveShell>(
                            "Projectiles/ExplosiveShell.tscn"
                        );
                        shell.targetPosition = t.GlobalPosition;
                        shell.startPosition = owner.GlobalPosition;
                        shell.callback = () =>
                        {
                            Node3D crater = ResourceProvider.CreateResource<Node3D>(
                                "Effects/Crater1.tscn"
                            );
                            GetTree().Root.AddChild(crater);
                            crater.GlobalPosition = shell.GlobalPosition;
                            new LifetimedResource(1.25).StartLifetime(crater);
                            Sound3DService.PlaySoundAtNode(crater, "boom1.wav");
                            var list = GamemodeLevel1.GetEnemiesInRange(shell.GlobalPosition, 4);
                            foreach (var t in list)
                            {
                                owner.DealDamage(t, owner.stats.damage * damageMultiplier);
                            }
                        };
                        AddChild(shell);
                    }
                    currentState = EWeaponState.Reloading;
                    tim.QueueFree();
                };
                tim.Autostart = true;
                AddChild(tim);
                return;
            }
            foreach (var t in targets)
            {
                if (GD.Randf() < instantKillChance)
                {
                    RA2AnimatedSprite3D.PlaySpriteScene(t.GlobalPosition, "Effects/nukedie.tscn", 2);
                    Sprite3D sprite = (Sprite3D)t.FindChild("Sprite3D");
                    if (sprite != null)
                    {
                        sprite.Hide();
                    }
                    t.Die();
                }
                else
                {
                    owner.DealDamage(t, owner.stats.damage * damageMultiplier);
                    RA2AnimatedSprite3D.PlaySpriteScene(t.GlobalPosition, "Effects/piffpiff.tscn", 1.0/5.0);
                }
            }

            owner.PlaySound("iconatta.wav");
            owner.Sprite.PlayAnimWithDir(
                "fire",
                targets.Last().GlobalPosition - owner.GlobalPosition,
                true
            );
        }

        public override List<Enemy> GetTargets()
        {
            return GamemodeLevel1.GetClosestEnemiesToPlayer(multishot, owner.stats.attackRange);
        }
    }
}
