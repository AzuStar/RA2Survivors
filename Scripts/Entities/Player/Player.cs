using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace RA2Survivors
{
    public partial class Player : Entity
    {
        public int uniqueUpgradesToSelect;
        public int commonUpgradesToSelect;

        public static double ExpFormula(int level) => 10 + 5 * level;

        public override EEntityType associatedEntity => EEntityType.Conscript;

        public List<UpgradeButtonSettings> commonUpgrades = new List<UpgradeButtonSettings>();
        public List<UpgradeButtonSettings> uniqueUpgrades = new List<UpgradeButtonSettings>();

        public Vector3 movementVelocity = Vector3.Zero;
        public RA2Sprite3D Sprite;

        private ulong LastQuoteTime = 0;
        private static ulong QuoteIntervalMsec = 15000;

        public void SetExp(double amount)
        {
            stats.currentExp = amount;

            int levelUps = 0;
            double nextLevelExp = ExpFormula(stats.level + levelUps);
            while (stats.currentExp >= nextLevelExp)
            {
                stats.currentExp -= nextLevelExp;
                levelUps++;
                nextLevelExp = ExpFormula(stats.level + levelUps);
            }

            ExpBar.SetExp(stats.currentExp, nextLevelExp);
            LevelUp(levelUps);

            stats.level += levelUps;
        }

        public void AddExp(double amount)
        {
            SetExp(stats.currentExp + amount);
        }

        public virtual void LevelUp(int times)
        {
            commonUpgradesToSelect += times;
        }

        public override void SetHealth(double amount)
        {
            base.SetHealth(amount);
            HealthBar.SetHealth(stats.health, stats.maxHealth);
        }

        public override void _Ready()
        {
            base._Ready();
            Sprite = (RA2Sprite3D)FindChild("Sprite3D");

            ContactMonitor = true;
            MaxContactsReported = 6;

            SetExp(stats.currentExp);
            SetHealth(stats.health);

            commonUpgrades.AddRange(
                [
                    new UpgradeButtonSettings
                    {
                        title = "Attack Speed Upgrade",
                        description = "Increases attack speed by [color=#FF0000]5%[/color]",
                        callback = () =>
                        {
                            stats.attackSpeed += 0.05;
                        }
                    },
                    new UpgradeButtonSettings
                    {
                        title = "Movement Speed Upgrade",
                        description = "Increases movement speed by [color=#FF0000]0.5[/color]",
                        callback = () =>
                        {
                            stats.movementSpeed += 0.5;
                        }
                    },
                    new UpgradeButtonSettings
                    {
                        title = "Health Regen Upgrade",
                        description = "Increases health regen by [color=#FF0000]1% / 5s[/color]",
                        callback = () =>
                        {
                            stats.healthRegen += 0.01;
                        }
                    },
                    new UpgradeButtonSettings
                    {
                        title = "Max Health Upgrade",
                        description = "Increases max health by [color=#FF0000]10[/color]",
                        callback = () =>
                        {
                            stats.health += 10;
                        }
                    },
                ]
            );

            uniqueUpgrades.Add(
                new UpgradeButtonSettings
                {
                    title = "Soviet Power Supreme!",
                    description = "You will now get [color=#860ac9]100%[/color] more experience!",
                    callback = () =>
                    {
                        stats.expGainRate += 1;
                    }
                }
            );
        }

        public override void TakeDamage(Entity source, double damage)
        {
            // Do not destruct on death - handle manually
            // DamageNumber3D.SpawnDamageNumber(this, damage, TakeDamageColor); // can't be done
            if (dead)
            {
                return;
            }
            stats.health -= damage;
            HealthBar.SetHealth(stats.health, stats.maxHealth);
            if (stats.health <= 0)
            {
                dead = true;
                Freeze = true;
                if (ResourceProvider.EntityDyingSounds[associatedEntity].Count > 0)
                {
                    Sound3DService.PlaySoundAtNode(this, ResourceProvider.EntityDyingSounds[associatedEntity][GD.RandRange(0, ResourceProvider.EntityDyingSounds[associatedEntity].Count - 1)]);
                }
                ((Sprite3D)FindChild("HealthBar3D")).Hide();
                Sprite.PlayAnim("death", true);
                GamemodeLevel1.instance.HandleDefeat();
                return;
            }

            ulong currentTime = Time.GetTicksMsec();
            if (LastQuoteTime == 0 || currentTime > LastQuoteTime + QuoteIntervalMsec)
            {
                double healthValue = stats.health / stats.maxHealth;
                if (healthValue < 0.2)
                {
                    Sound3DService.PlaySoundAtNode(this, "iconfea.wav");
                    LastQuoteTime = currentTime;
                }
                else if (healthValue < 0.5)
                {
                    Sound3DService.PlaySoundAtNode(this, "iconfeb.wav");
                    LastQuoteTime = currentTime;
                }
            }
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            if (GamemodeLevel1.instance.GameEnded)
            {
                return;
            }
            if (commonUpgradesToSelect > 0)
            {
                // randomize order and select the first 3
                UpgradeButtonSettings[] upgradesToDisplay = commonUpgrades
                    .OrderBy(x => GD.Randi())
                    .Take(3)
                    .ToArray();

                UpgradeSelector.CreateSelection(upgradesToDisplay);
                commonUpgradesToSelect--;
            }
            else if (uniqueUpgradesToSelect > 0)
            {
                UpgradeButtonSettings[] upgradesToDisplay = uniqueUpgrades
                    .OrderBy(x => GD.Randi())
                    .Take(3)
                    .ToArray();

                UpgradeSelector.CreateSelection(
                    upgradesToDisplay,
                    (upgradeSettings) =>
                    {
                        uniqueUpgrades.Remove(upgradeSettings);
                    }
                );
                uniqueUpgradesToSelect--;
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            if (movementVelocity.Length() > 0)
                ApplySlidingForceToRigidBodies();

            foreach (Pickup pickup in GamemodeLevel1.instance.pickups)
            {
                switch (pickup)
                {
                    case ExpOrb orb:
                        if (orb.magnetTarget != null)
                            continue;

                        double distance = GlobalPosition.DistanceTo(orb.GlobalPosition);
                        if (distance < stats.magnetRange)
                        {
                            orb.magnetTarget = this;
                        }
                        break;

                    case UpgradeCrate crate:
                        distance = GlobalPosition.DistanceTo(crate.GlobalPosition);
                        if (distance < 2)
                        {
                            crate.QueueFree();
                            PlaySound("UpgradeCrate.wav", ignorePause: true);
                            uniqueUpgradesToSelect++;
                        }
                        break;
                }
            }
        }

        public override void _IntegrateForces(PhysicsDirectBodyState3D state)
        {
            if (dead)
            {
                return;
            }
            // Handle movement input
            Vector3 direction = Vector3.Zero;

            if (Input.IsActionPressed("MoveUp"))
            {
                direction.Z -= 1;
            }
            if (Input.IsActionPressed("MoveDown"))
            {
                direction.Z += 1;
            }
            if (Input.IsActionPressed("MoveLeft"))
            {
                direction.X -= 1;
            }
            if (Input.IsActionPressed("MoveRight"))
            {
                direction.X += 1;
            }
            direction = direction.Normalized();
            Vector3 horizontalVelocity = direction * (float)stats.movementSpeed;
            movementVelocity.X = horizontalVelocity.X;
            movementVelocity.Z = horizontalVelocity.Z;

            state.LinearVelocity = movementVelocity;

            Freeze = movementVelocity.Length() < 0.01;

            if (
                !(Sprite.CurrentAnim != null && Sprite.CurrentAnim.StartsWith("fire"))
                || Sprite.CurrentAnimFinished
            )
            {
                if (Freeze)
                {
                    Sprite.PlayAnim("face_s");
                }
                else
                {
                    Sprite.PlayAnimWithDir("run", movementVelocity);
                }
            }
        }

        private void ApplySlidingForceToRigidBodies()
        {
            float slideForce = 10.0f;

            foreach (var collider in GetCollidingBodies())
            {
                if (collider is Enemy enemy)
                {
                    Vector3 toEnemy = enemy.GlobalTransform.Origin - GlobalTransform.Origin;
                    Vector3 perpendicularDirection = new Vector3(
                        -movementVelocity.Z,
                        0,
                        movementVelocity.X
                    ).Normalized();

                    float dotProduct = toEnemy.Dot(perpendicularDirection);
                    if (dotProduct > 0)
                    {
                        perpendicularDirection = -perpendicularDirection;
                    }
                    enemy.Push(-perpendicularDirection * slideForce);
                }
            }
        }
    }
}
