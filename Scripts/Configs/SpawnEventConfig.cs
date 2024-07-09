namespace RA2Survivors
{
    public class SpawnEventConfig
    {
        public double spawnTime;
        public SpawnEventEnemyConfig[] enemyConfig;
    }

    public class SpawnEventEnemyConfig
    {
        public EEntityType enemyType;
        public int enemyCount;
        public Qudrant qudrant = Qudrant.Random;
    }

    public enum Qudrant
    {
        Random,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}
