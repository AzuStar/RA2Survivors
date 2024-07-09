using System.Collections.Generic;
using Godot;

namespace RA2Survivors
{
    public abstract partial class Entity : RigidBody3D
    {
        public abstract EEntityType associatedEntity { get; }
        public Stats stats;
        protected List<CollisionShape3D> colliders = new List<CollisionShape3D>();
        public bool dead = false;

        private static List<string> DeathSpritePaths = new List<string>()
        {
            "Effects/death_a.tscn",
            "Effects/death_b.tscn",
            "Effects/death_c.tscn",
        };

        public override void _Ready()
        {
            base._Ready();
            foreach (var c in GetChildren())
            {
                if (c is CollisionShape3D)
                {
                    colliders.Add((CollisionShape3D)c);
                }
            }
            stats.health = stats.maxHealth;
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            Regeneration(delta);
        }

        public virtual void SetHealth(double amount)
        {
            stats.health = amount;
            if (stats.health > stats.maxHealth)
            {
                stats.health = stats.maxHealth;
            }
        }

        public void AddHealth(double amount)
        {
            SetHealth(stats.health + amount);
        }

        public virtual void Regeneration(double delta)
        {
            AddHealth(stats.maxHealth * stats.healthRegen / 5 * delta);
        }

        public virtual void DealDamage(Entity target, double damage)
        {
            target.TakeDamage(this, damage);
        }

        public virtual void TakeDamage(Entity source, double damage)
        {
            stats.health -= damage;
            if (stats.health <= 0)
            {
                Die();
            }
        }

        public virtual void OnDying() { }

        public void Die()
        {
            if (dead)
                return;
            dead = true;

            foreach (var c in colliders)
            {
                c.QueueFree();
            }
            OnDying();
            LinearVelocity = Vector3.Zero;

            SceneTreeTimer recycleTim = GetTree().CreateTimer(1.5f);
            recycleTim.Timeout += () =>
            {
                QueueFree();
                RA2AnimatedSprite3D.PlaySpriteScene(
                    GlobalPosition,
                    DeathSpritePaths[GD.RandRange(0, DeathSpritePaths.Count - 1)],
                    10
                );
            };
        }

        public void PlaySound(string soundName, bool ignorePause = false)
        {
            Sound3DService.PlaySoundAtNode(this, soundName, ignorePause);
        }
    }
}
