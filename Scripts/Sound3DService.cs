using Godot;

namespace RA2Survivors
{
    public static class Sound3DService
    {
        public const string MASTER_ASSET_PATH = "res://Assets/Audio/";

        // public static void PlaySoundAtLocation(Vector3 location, string soundName)
        // {
        //     AudioStreamPlayer3D sound = new AudioStreamPlayer3D();
        //     sound.Stream = ResourceLoader.Load<AudioStream>(MASTER_ASSET_PATH + soundName + ".wav");
        //     AddChild(sound);
        //     sound.Finished += () => sound.QueueFree();
        //     sound.Play();
        // }

        public static void PlaySoundAtNode(Node3D node, string soundName, bool ignorePause = false)
        {
            AudioStreamPlayer3D sound = new AudioStreamPlayer3D
            {
                Stream = ResourceLoader.Load<AudioStream>(MASTER_ASSET_PATH + soundName)
            };
            GamemodeLevel1.instance.GetTree().Root.AddChild(sound);
            if (ignorePause)
                sound.ProcessMode = Node.ProcessModeEnum.Always;
            sound.GlobalTransform = node.GlobalTransform;
            sound.Finished += () => sound.QueueFree();
            sound.Play();
        }
    }
}
