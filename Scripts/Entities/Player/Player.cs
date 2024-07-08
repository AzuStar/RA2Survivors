using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace RA2Survivors
{
	public partial class Player : Entity
	{
		public int upgradesToSelect;

		public static double ExpFormula(int level) => 10 + 5 * level;

		public override EEntityType associatedEntity => EEntityType.Conscript;

		public List<UpgradeButtonSettings> availableUpgrades = new List<UpgradeButtonSettings>();
		public Vector3 movementVelocity = Vector3.Zero;
		public RA2Sprite3D Sprite;

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
			LevelUp(levelUps);

			stats.level = levelUps;
		}

		public void AddExp(double amount)
		{
			SetExp(stats.currentExp + amount);
		}

		public virtual void LevelUp(int times)
		{
			upgradesToSelect += times;
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

		public override void TakeDamage(Entity source, double damage)
		{
			base.TakeDamage(source, damage);
			HealthBar.SetHealth(stats.health, stats.maxHealth);
		}

		public override void _Process(double delta)
		{
			base._Process(delta);
			if (upgradesToSelect > 0)
			{
				// randomize order and select the first 3
				UpgradeButtonSettings[] upgradesToDisplay = availableUpgrades
					.OrderBy(x => GD.Randi())
					.Take(3)
					.ToArray();

				UpgradeSelector.CreateSelection(upgradesToDisplay);
				upgradesToSelect--;
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);
			if (movementVelocity.Length() > 0)
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
			movementVelocity.X = horizontalVelocity.X;
			movementVelocity.Z = horizontalVelocity.Z;

			state.LinearVelocity = movementVelocity;

			Freeze = movementVelocity.Length() < 0.01;

			if (!Sprite.CurrentAnim.StartsWith("fire") || Sprite.CurrentAnimFinished)
			{
				if (Freeze) {
					Sprite.PlayAnim("face_s");
				} else
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
