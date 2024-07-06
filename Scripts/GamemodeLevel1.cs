using System.Collections.Generic;
using Godot;

namespace RA2Survivors
{
    public partial class GamemodeLevel1 : Node
    {
        public WaveConfig[] waveConfigs = new WaveConfig[]
        {
            new WaveConfig
            {
                enemyConfig = new WaveEnemyConfig[]
                {
                    new WaveEnemyConfig
                    {
                        enemyType = EEntityType.GI,
                        minEnemies = 5,
                        chancePastMin = 0.05f
                    }
                },
                waveDuration = 30
            }
        };
        public Queue<WaveConfig> waveQueue;
        public Player player;

        public float spawnRangeXStart = 10;
        public float spawnRangeXLength = 5;
        public float spawnRangeYStart = 8;
        public float spawnRangeYLength = 2;

        public override void _Ready()
        {
            player = GlobalVariables.LoadEntity<Conscript>(GlobalVariables.SelectedCharacter);
            AddChild(player);
            waveQueue = new Queue<WaveConfig>(waveConfigs);
            NextWave();
        }

        public void NextWave()
        {
            if (waveQueue.Count == 0)
            {
                GD.Print("Victory!");
                return;
            }
            WaveConfig wave = waveQueue.Dequeue();
            SpawnEnemy(EEntityType.GI);
            // foreach (WaveEnemyConfig enemyConfig in wave.enemyConfig)
            // {
            //     for (int i = 0; i < enemyConfig.minEnemies; i++)
            //     {
            //         SpawnEnemy(enemyConfig.enemyType);
            //     }
            // }
        }

        public void SpawnEnemy(EEntityType type)
        {
            CharacterBody3D enemy = GlobalVariables.LoadEntity<Enemy>(type);
            enemy.Transform = new Transform3D(Basis.Identity, SpawnRangeOffset());
            AddChild(enemy);
        }

        public Vector3 SpawnRangeOffset()
        {
            Vector3 offset = player.Transform.Origin;
            offset.X +=
                (float)GD.RandRange(spawnRangeXStart, spawnRangeXStart + spawnRangeXLength)
                * (GD.RandRange(0, 1) - 1);
            return offset;
        }
    }
}
