using System.Collections.Generic;
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

        protected List<AudioStreamPlayer3D> DyingSounds = new List<AudioStreamPlayer3D>();
        public List<string> DyingAnims = new List<string>();

        public override void _Ready()
        {
            base._Ready();
            GamemodeLevel1.instance.enemyCount[(int)associatedEntity]++;
            _builtInMass = Mass;

            Sprite = (RA2Sprite3D)FindChild("Sprite3D");
            Sprite.AnimDefinitions.Add(
                "death",
                new RA2SpriteAnim()
                {
                    StartFrame = 56,
                    EndFrame = 56 + 14,
                    Loop = false
                }
            );
            Sprite.AnimDefinitions.Add(
                "death2",
                new RA2SpriteAnim()
                {
                    StartFrame = 71,
                    EndFrame = 71 + 14,
                    Loop = false
                }
            );
            Sprite.AnimDefinitions.Add(
                "face_s",
                new RA2SpriteAnim()
                {
                    StartFrame = 4,
                    EndFrame = 4,
                    Loop = true
                }
            );
            Sprite.AnimDefinitions.Add(
                "run_n",
                new RA2SpriteAnim()
                {
                    StartFrame = 8,
                    EndFrame = 8 + 5,
                    Loop = true
                }
            );
            Sprite.AnimDefinitions.Add(
                "run_nw",
                new RA2SpriteAnim()
                {
                    StartFrame = 14,
                    EndFrame = 14 + 5,
                    Loop = true
                }
            );
            Sprite.AnimDefinitions.Add(
                "run_w",
                new RA2SpriteAnim()
                {
                    StartFrame = 20,
                    EndFrame = 20 + 5,
                    Loop = true
                }
            );
            Sprite.AnimDefinitions.Add(
                "run_sw",
                new RA2SpriteAnim()
                {
                    StartFrame = 26,
                    EndFrame = 26 + 5,
                    Loop = true
                }
            );
            Sprite.AnimDefinitions.Add(
                "run_s",
                new RA2SpriteAnim()
                {
                    StartFrame = 32,
                    EndFrame = 32 + 5,
                    Loop = true
                }
            );
            Sprite.AnimDefinitions.Add(
                "run_se",
                new RA2SpriteAnim()
                {
                    StartFrame = 38,
                    EndFrame = 38 + 5,
                    Loop = true
                }
            );
            Sprite.AnimDefinitions.Add(
                "run_e",
                new RA2SpriteAnim()
                {
                    StartFrame = 44,
                    EndFrame = 44 + 5,
                    Loop = true
                }
            );
            Sprite.AnimDefinitions.Add(
                "run_ne",
                new RA2SpriteAnim()
                {
                    StartFrame = 50,
                    EndFrame = 50 + 5,
                    Loop = true
                }
            );

            DyingAnims.Add("death");
            DyingAnims.Add("death2");

            Node dyingsounds = FindChild("dyingsounds");
            if (dyingsounds != null)
            {
                foreach (var s in dyingsounds.GetChildren())
                {
                    DyingSounds.Add((AudioStreamPlayer3D)s);
                }
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            // when colliding with player, deal damage * delta

            if (dead)
                return;

            if (distanceToPlayer <= stats.attackRange)
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
                QueueFree();
            }

            if (Freeze)
            {
                Sprite.PlayAnim("face_s");
            }
            else
            {
                Sprite.PlayAnimWithDir("run", velocity);
            }
        }

        public override void OnDying()
        {
            GamemodeLevel1.instance.SpawnExpOrb(GlobalPosition, stats.expDropped);
            if (DyingSounds.Count > 0)
            {
                DyingSounds[GD.RandRange(0, DyingSounds.Count - 1)].Play();
            }
            if (DyingAnims.Count > 0)
            {
                Sprite.PlayAnim(DyingAnims[GD.RandRange(0, DyingAnims.Count - 1)]);
            }
        }

        protected override void Dispose(bool disposing)
        {
            GamemodeLevel1.instance.enemyCount[(int)associatedEntity]--;
            base.Dispose(disposing);
        }
    }
}
