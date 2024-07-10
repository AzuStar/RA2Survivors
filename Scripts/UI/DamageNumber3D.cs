using Godot;
using RA2Survivors;
using System;

public partial class DamageNumber3D : Node3D
{
    private static double LifeTimeSeconds = 2.0;
    private double CurrentLifeTime = 0.0;
    Label TextLabel;
    Sprite3D Sprite;
    private Vector3 GlobalOffset;
    private static Color DefaultColor = new Color(1, 1, 1);

    public static void SpawnDamageNumber(Entity target, double damage)
    {
        SpawnDamageNumber(target, damage, new Color(1, 1, 1));
    }

    public static void SpawnDamageNumber(Entity target, double damage, Color color)
    {
        DamageNumber3D instance = (DamageNumber3D)ResourceLoader.Load<PackedScene>("Prefabs/UI/DamageNumber3D.tscn").Instantiate<Node3D>();
        target.GetTree().Root.AddChild(instance);
        instance.GlobalOffset = target.GlobalPosition + new Vector3(0, 0, -2);
        instance.GlobalPosition = instance.GlobalOffset;
        instance.TextLabel = (Label)instance.FindChild("Label", owned:false);
        instance.TextLabel.Text = Math.Floor(damage).ToString();
        instance.Sprite = (Sprite3D)instance.FindChild("Sprite3D");
        instance.Sprite.Modulate = color;
        Utils.DelayedInvoke(LifeTimeSeconds, () =>
        {
            instance.QueueFree();
        });
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        CurrentLifeTime = Math.Min(LifeTimeSeconds, CurrentLifeTime + delta);
        double step = CurrentLifeTime / LifeTimeSeconds;
        GlobalPosition = new Vector3(GlobalOffset.X, GlobalOffset.Y, GlobalOffset.Z + (float)Utils.EaseOutCirc(step));
        Sprite.Modulate = new Color(Sprite.Modulate.R, Sprite.Modulate.G, Sprite.Modulate.B, 1.0f - (float)Utils.EaseOutCirc(step));
    }
}
