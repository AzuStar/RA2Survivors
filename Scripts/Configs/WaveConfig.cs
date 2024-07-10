namespace RA2Survivors
{
    public class WaveConfig
    {
        public WaveEnemyConfig[] enemyConfig;
        public double waveDuration;
        public string waveMusic;
        public string waveName;
    }

    public class WaveEnemyConfig
    {
        public EEntityType enemyType;
        public int minEnemies;
        public float chancePastMin;
    }
}
