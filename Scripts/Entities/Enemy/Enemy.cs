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
		protected List<string> DyingAnims = new List<string>();

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

			if (Freeze) {
				Sprite.PlayAnim("face_s");
			} else
			{
				Sprite.PlayAnimWithDir("run", velocity);
			}
		}

		public override void OnDying()
		{
			GamemodeLevel1.instance.enemies.Remove(this);
			GamemodeLevel1.instance.enemyCount[(int)associatedEntity]--;
			GamemodeLevel1.instance.player.AddExp(stats.expDropped);
			if (DyingSounds.Count > 0) 
			{
				DyingSounds[GD.RandRange(0, DyingSounds.Count - 1)].Play();
			}
			if (DyingAnims.Count > 0) 
			{
				Sprite.PlayAnim(DyingAnims[GD.RandRange(0, DyingAnims.Count - 1)]);
			}
		}
	}
}
