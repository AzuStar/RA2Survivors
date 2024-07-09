using Godot;

namespace RA2Survivors
{
    public partial class MusicService : AudioStreamPlayer
    {
        public const string MASTER_MUSIC_PATH = "res://Assets/Music/";
        public static MusicService instance { get; private set; }

        public MusicService()
        {
            instance = this;
        }

        public override void _Ready()
        {
            Finished += () => Play();
        }

        public static void PlayMusic(string musicPath)
        {
            instance.Stop();
            instance.Stream = ResourceLoader.Load<AudioStream>(MASTER_MUSIC_PATH + musicPath);
            instance.Play();
        }
    }
}
