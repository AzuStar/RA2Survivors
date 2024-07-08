using System;
using System.Collections.Generic;
using Godot;

namespace RA2Survivors
{
	public partial class AK47 : Weapon
	{
		public override void _Ready()
		{
			base._Ready();
			damageMultiplier = 0.33;
			burstCount = 3;
			burstDelay = 0.05;
			reloadSpeed = 0.95;
			multishot = 1;
			FireSounds.Add((AudioStreamPlayer3D)owner.FindChild("iconatta"));
			FireSounds.Add((AudioStreamPlayer3D)owner.FindChild("iconattb"));
			FireSounds.Add((AudioStreamPlayer3D)owner.FindChild("iconattc"));
			FireSounds.Add((AudioStreamPlayer3D)owner.FindChild("iconattd"));
		}

		public override void _Process(double delta)
		{
			base._Process(delta);
		}

		public override void Shoot(List<Enemy> targets)
		{
			foreach (var t in targets)
			{
				owner.DealDamage(t, owner.stats.damage * damageMultiplier);
				owner.Sprite.PlayAnimWithDir("fire", owner.GlobalPosition.DirectionTo(t.GlobalPosition), true);
			}
		}

		public override List<Enemy> GetTargets()
		{
			return GamemodeLevel1.GetClosestEnemiesToPlayer(multishot);
		}
	}
}
