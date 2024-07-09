using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace RA2Survivors
{
    public partial class GamemodeLevel1 : Node
    {
        [Export]
        public Node enemyNode;

        [Export]
        public Node expOrbNode;
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
                        minEnemies = 5,
                        chancePastMin = 0.2f
                    },
                ],
                waveDuration = 30,
                waveMusic = "BullyKit.mp3"
            },
            new WaveConfig
            {
                enemyConfig =
                [
                    new WaveEnemyConfig
                    {
                        enemyType = EEntityType.GI,
                        minEnemies = 10,
                        chancePastMin = 0.2f
                    },
                    new WaveEnemyConfig
                    {
                        enemyType = EEntityType.AttackDog,
                        minEnemies = 2,
                        chancePastMin = 0.1f
                    },
                ],
                waveDuration = 30
            },
            new WaveConfig
            {
                enemyConfig =
                [
                    new WaveEnemyConfig
                    {
                        enemyType = EEntityType.GI,
                        minEnemies = 10,
                        chancePastMin = 0.2f
                    },
                    new WaveEnemyConfig
                    {
                        enemyType = EEntityType.AttackDog,
                        minEnemies = 2,
                        chancePastMin = 0.1f
                    },
                ],
                waveDuration = 30
            }
        };
        public Queue<WaveConfig> waveQueue;
        public WaveConfig currentWave;
        public Player player;
        public int[] enemyCount;

        public int totalEnemyCount => enemyNode.GetChildCount();
        public Enemy[] enemies => enemyNode.GetChildren().Cast<Enemy>().ToArray();
        public ExpOrb[] expOrbs => expOrbNode.GetChildren().Cast<ExpOrb>().ToArray();
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
            player.GlobalTransform = new Transform3D(Basis.Identity, new Vector3(-2, 2, 0));
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
            if (!String.IsNullOrEmpty(currentWave.waveMusic))
            {
                MusicService.PlayMusic(currentWave.waveMusic);
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
                else if (
                    totalEnemyCount < maxEnemiesCount
                    && GD.Randf() < enemyConfig.chancePastMin
                )
                {
                    SpawnEnemy(enemyConfig.enemyType);
                }
            }
        }

        public void SpawnEnemy(EEntityType type)
        {
            Enemy enemy = SpawnerService.SpawnEnemy(type, player.GlobalTransform.Origin);
            enemyNode.AddChild(enemy);
        }

        public void SpawnExpOrb(Vector3 position, double expAmount)
        {
            ExpOrb expOrb = ResourceProvider.CreateExpOrb(position, expAmount);
            expOrbNode.AddChild(expOrb);
        }

        public static List<Enemy> GetEnemiesInRange(Vector3 location, double range)
        {
            return instance
                .enemies.Where(e => e.GlobalPosition.DistanceTo(location) < range)
                .ToList();
        }

        public static List<Enemy> GetClosestEnemiesToPlayer(
            int amount,
            double minRange = double.MaxValue
        )
        {
            return instance
                .enemies.OrderBy(e => e.distanceToPlayer)
                .Where(e => e.distanceToPlayer < minRange)
                .Take(amount)
                .ToList();
        }
    }
}
