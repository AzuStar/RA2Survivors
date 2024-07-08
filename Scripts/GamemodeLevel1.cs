using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace RA2Survivors
{
    public partial class GamemodeLevel1 : Node
    {
        public static GamemodeLevel1 instance { get; private set; }
        public WaveConfig[] waveConfigs =
        {
            new WaveConfig
            {
                enemyConfig =
                [
                    new WaveEnemyConfig
                    {
                        enemyType = EEntityType.GI,
                        minEnemies = 25,
                        chancePastMin = 0.1f
                    },
                    new WaveEnemyConfig
                    {
                        enemyType = EEntityType.AttackDog,
                        minEnemies = 5,
                        chancePastMin = 0.1f
                    }
                ],
                waveDuration = 301
            },
            new WaveConfig
            {
                enemyConfig =
                [
                    new WaveEnemyConfig
                    {
                        enemyType = EEntityType.GI,
                        minEnemies = 10,
                        chancePastMin = 0.1f
                    }
                ],
                waveDuration = 30
            }
        };
        public Queue<WaveConfig> waveQueue;
        public WaveConfig currentWave;
        public Player player;
        public int[] enemyCount;
        public List<Enemy> enemies = new List<Enemy>();
        public int maxEnemiesCount = 100;

        public double spawnTimeout = 0;
        public double spawnTimer = 1;

        public double waveTimer = 0;

        public override void _Process(double delta)
        {
            base._Process(delta);
            spawnTimeout += delta;
            if (spawnTimeout >= spawnTimer)
            {
                spawnTimeout = 0;
                SpawnerTick();
            }

            if (currentWave != null)
            {
                waveTimer += delta;
                if (waveTimer >= currentWave.waveDuration)
                {
                    NextWave();
                    waveTimer = 0;
                }
            }
        }

        public override void _Ready()
        {
            instance = this;
            enemyCount = new int[Enum.GetValues(typeof(EEntityType)).Length];
            player = ResourceProvider.LoadEntity<Player>(ResourceProvider.SelectedCharacter);
            AddChild(player);
            waveQueue = new Queue<WaveConfig>(waveConfigs);
            NextWave();
        }

        public void NextWave()
        {
            currentWave = null;
            if (waveQueue.Count == 0)
            {
                GD.Print("Victory!");
                return;
            }
            currentWave = waveQueue.Dequeue();
            foreach (WaveEnemyConfig enemyConfig in currentWave.enemyConfig)
            {
                for (int i = 0; i < enemyConfig.minEnemies; i++)
                {
                    SpawnEnemy(enemyConfig.enemyType);
                }
            }
        }

        public void SpawnerTick()
        {
            if (currentWave == null)
                return;

            foreach (WaveEnemyConfig enemyConfig in currentWave.enemyConfig)
            {
                if (enemyCount[(int)enemyConfig.enemyType] < enemyConfig.minEnemies)
                {
                    int enemiesToPopulate =
                        GD.RandRange(3, 5)
                        % (enemyConfig.minEnemies - enemyCount[(int)enemyConfig.enemyType]);
                    for (int i = 0; i < enemiesToPopulate; i++)
                        SpawnEnemy(enemyConfig.enemyType);
                }
                else if (enemies.Count < maxEnemiesCount && GD.Randf() < enemyConfig.chancePastMin)
                {
                    SpawnEnemy(enemyConfig.enemyType);
                }
            }
        }

        public void SpawnEnemy(EEntityType type)
        {
            Enemy enemy = SpawnerService.SpawnEnemy(type, player.GlobalTransform.Origin);
            AddChild(enemy);
            enemies.Add(enemy);
        }

        public static List<Enemy> GetClosestEnemiesToPlayer(int amount)
        {
            return instance.enemies.OrderBy(e => e.distanceToPlayer).Take(amount).ToList();
        }
    }
}
