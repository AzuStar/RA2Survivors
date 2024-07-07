using Godot;

namespace RA2Survivors
{
    public abstract partial class Enemy : Entity
    {
        public double distanceToPlayer = 999;
        private Vector3 _pushForce;
        private float _builtInMass;
        private float _pushMass;
        protected RA2Sprite3D Sprite;

        public override void _Ready()
        {
            base._Ready();
            GamemodeLevel1.instance.enemyCount[(int)associatedEntity]++;
            Sprite = (RA2Sprite3D)FindChild("Sprite3D");
            _builtInMass = Mass;
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            // when colliding with player, deal damage * delta

            if (dead)
                return;

            if (distanceToPlayer <= 1.9)
            {
                DealDamage(GamemodeLevel1.instance.player, stats.damage * delta);
            }
        }

        public void Push(Vector3 force)
        {
            _pushForce = force;
            _pushMass = 2 * _builtInMass;
        }

        public override void _IntegrateForces(PhysicsDirectBodyState3D state)
        {
            if (dead)
                return;
            if (GamemodeLevel1.instance.player == null)
                return;

            Vector3 direction = (
                GamemodeLevel1.instance.player.GlobalTransform.Origin - GlobalTransform.Origin
            ).Normalized();
            Vector3 velocity = direction * (float)stats.movementSpeed;

            state.LinearVelocity = velocity + _pushForce;
            // dampen push
            _pushForce *= 0.9f;
            _pushMass *= 0.9f;
            Mass = _builtInMass + _pushMass;

            distanceToPlayer = GlobalTransform.Origin.DistanceTo(
                GamemodeLevel1.instance.player.GlobalTransform.Origin
            );

            if (distanceToPlayer >= SpawnerService.DistanceDespawnThreshold)
            {
                Die();
            }

            if (velocity.X == 0 && velocity.Z == 0)
            {
                Sprite.PlayAnim("face_s");
            }
            else if (velocity.X == 0 && velocity.Z < 0)
            {
                Sprite.PlayAnim("run_n");
            }
            else if (velocity.X < 0 && velocity.Z < 0)
            {
                Sprite.PlayAnim("run_nw");
            }
            else if (velocity.X < 0 && velocity.Z == 0)
            {
                Sprite.PlayAnim("run_w");
            }
            else if (velocity.X < 0 && velocity.Z > 0)
            {
                Sprite.PlayAnim("run_sw");
            }
            else if (velocity.X == 0 && velocity.Z > 0)
            {
                Sprite.PlayAnim("run_s");
            }
            else if (velocity.X > 0 && velocity.Z > 0)
            {
                Sprite.PlayAnim("run_se");
            }
            else if (velocity.X > 0 && velocity.Z == 0)
            {
                Sprite.PlayAnim("run_e");
            }
            else if (velocity.X > 0 && velocity.Z < 0)
            {
                Sprite.PlayAnim("run_ne");
            }
        }

        public override void OnDying()
        {
            GamemodeLevel1.instance.enemies.Remove(this);
            GamemodeLevel1.instance.enemyCount[(int)associatedEntity]--;
        }
    }
}
