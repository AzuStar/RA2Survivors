using System;
using Godot;
using RA2Survivors;

public partial class WaveTextButton : Button
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (GamemodeLevel1.instance.currentWave != null)
        {
            var timeLeft = GamemodeLevel1.instance.currentWave.waveDuration - GamemodeLevel1.instance.waveTimer;
            var seconds = (Math.Floor(timeLeft) % 60).ToString("00");
            var minutes = (Math.Floor(timeLeft) / 60 % 60).ToString("00");
            Text = GamemodeLevel1.instance.currentWave.waveName + " " + minutes + ":" + seconds;
        }
    }
}
