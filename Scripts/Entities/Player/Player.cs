using System;
using Godot;

namespace RA2Survivors
{
    public partial class Player : Entity
    {
        public static double ExpFormula(int level) => 10 + 5 * level;

        public override EEntityType associatedEntity => EEntityType.Conscript;

        private Vector3 _velocity = Vector3.Zero;
        protected RA2Sprite3D Sprite;

        public void SetExp(double amount)
        {
            stats.currentExp = amount;

            int levelUps = 0;
            double nextLevelExp = ExpFormula(levelUps);
            while (stats.currentExp >= nextLevelExp)
            {
                stats.currentExp -= nextLevelExp;
                levelUps++;
                nextLevelExp = ExpFormula(levelUps);
            }

            ExpBar.SetExp(stats.currentExp, nextLevelExp);

            stats.level = levelUps;
        }

        public void AddExp(double amount)
        {
            SetExp(stats.currentExp + amount);
        }

        public override void SetHealth(double amount)
        {
            base.SetHealth(amount);
            HealthBar.SetHealth(stats.health, stats.maxHealth);
        }

        public override void _Ready()
        {
            Sprite = (RA2Sprite3D)FindChild("Sprite3D");

            ContactMonitor = true;
            MaxContactsReported = 6;

            SetExp(stats.currentExp);
            SetHealth(stats.health);
        }

        public override void TakeDamage(double damage)
        {
            base.TakeDamage(damage);
            HealthBar.SetHealth(stats.health, stats.maxHealth);
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            if (_velocity.Length() > 0)
                ApplySlidingForceToRigidBodies();
        }

        public override void _IntegrateForces(PhysicsDirectBodyState3D state)
        {
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
            _velocity.X = horizontalVelocity.X;
            _velocity.Z = horizontalVelocity.Z;

            state.LinearVelocity = _velocity;

            Freeze = _velocity.X == 0 && _velocity.Z == 0;
            
            if (_velocity.X == 0 && _velocity.Z == 0)
            {
                Sprite.PlayAnim("face_s");
            }
            else if (_velocity.X == 0 && _velocity.Z < 0)
            {
                Sprite.PlayAnim("run_n");
            }
            else if (_velocity.X < 0 && _velocity.Z < 0)
            {
                Sprite.PlayAnim("run_nw");
            }
            else if (_velocity.X < 0 && _velocity.Z == 0)
            {
                Sprite.PlayAnim("run_w");
            }
            else if (_velocity.X < 0 && _velocity.Z > 0)
            {
                Sprite.PlayAnim("run_sw");
            }
            else if (_velocity.X == 0 && _velocity.Z > 0)
            {
                Sprite.PlayAnim("run_s");
            }
            else if (_velocity.X > 0 && _velocity.Z > 0)
            {
                Sprite.PlayAnim("run_se");
            }
            else if (_velocity.X > 0 && _velocity.Z == 0)
            {
                Sprite.PlayAnim("run_e");
            }
            else if (_velocity.X > 0 && _velocity.Z < 0)
            {
                Sprite.PlayAnim("run_ne");
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
                        -_velocity.Z,
                        0,
                        _velocity.X
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
