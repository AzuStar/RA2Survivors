using Godot;

namespace RA2Survivors
{
    public partial class Tanya : Enemy
    {
        public override EEntityType associatedEntity => EEntityType.Tanya;

        public Tanya()
        {
            stats = new Stats
            {
                attackRange = 2.1,
                damage = 200,
                maxHealth = 200,
                movementSpeed = 6,
                expDropped = 50,
            };
            despawnImmune = true;
        }

        public override void OnDying()
        {
            base.OnDying();
            GamemodeLevel1.instance.SpawnCrate(GlobalPosition);
        }
    }
}
