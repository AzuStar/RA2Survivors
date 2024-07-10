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
        public Node pickupsNode;
        public static GamemodeLevel1 instance { get; private set; }

        public bool GameEnded = false;
        public WaveConfig[] waveConfigs =
        {
            // Wave 0000 - 0030
            new WaveConfig
            {
                enemyConfig =
                [
                    new WaveEnemyConfig
                    {
                        enemyType = EEntityType.GI,
                        minEnemies = 100,
                        chancePastMin = 0.2f
                    },
                ],
                waveDuration = 30,
                waveMusic = "BullyKit.mp3",
                waveName = "Wave 1"
            },
            // Wave 0030 - 0100
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
                waveDuration = 30,
                waveName = "Wave 2"
            },
            // // Wave 0100 - 0130
            // new WaveConfig
            // {
            //     enemyConfig =
            //     [
            //         new WaveEnemyConfig
            //         {
            //             enemyType = EEntityType.GI,
            //             minEnemies = 10,
            //             chancePastMin = 0.4f
            //         },
            //         new WaveEnemyConfig
            //         {
            //             enemyType = EEntityType.AttackDog,
            //             minEnemies = 2,
            //             chancePastMin = 0.2f
            //         },
            //     ],
            //     waveDuration = 30,
            //     waveName = "Wave 3"
            // },
            // // Wave 0130 - 0300
            // new WaveConfig
            // {
            //     enemyConfig =
            //     [
            //         new WaveEnemyConfig
            //         {
            //             enemyType = EEntityType.GI,
            //             minEnemies = 15,
            //             chancePastMin = 0.4f
            //         },
            //         new WaveEnemyConfig
            //         {
            //             enemyType = EEntityType.AttackDog,
            //             minEnemies = 2,
            //             chancePastMin = 0.2f
            //         }
            //     ],
            //     waveDuration = 90,
            //     waveName = "Wave 4"
            // },
            // // Wave 0300 - 0400
            // new WaveConfig
            // {
            //     enemyConfig =
            //     [
            //         new WaveEnemyConfig
            //         {
            //             enemyType = EEntityType.Seal,
            //             minEnemies = 10,
            //             chancePastMin = 0.2f
            //         },
            //         new WaveEnemyConfig
            //         {
            //             enemyType = EEntityType.AttackDog,
            //             minEnemies = 2,
            //             chancePastMin = 0.2f
            //         }
            //     ],
            //     waveDuration = 60,
            //     waveName = "Wave 5"
            // },
            // // Wave 0400 - 0500
            // new WaveConfig
            // {
            //     enemyConfig =
            //     [
            //         new WaveEnemyConfig
            //         {
            //             enemyType = EEntityType.Seal,
            //             minEnemies = 10,
            //             chancePastMin = 0.4f
            //         },
            //         new WaveEnemyConfig
            //         {
            //             enemyType = EEntityType.AttackDog,
            //             minEnemies = 2,
            //             chancePastMin = 0.3f
            //         }
            //     ],
            //     waveDuration = 60,
            //     waveName = "Final Wave"
            // },
        };

        public SpawnEventConfig[] spawnEvents =
        [
            new SpawnEventConfig
            {
                spawnTime = 20,
                enemyConfig =
                [
                    new SpawnEventEnemyConfig
                    {
                        enemyType = EEntityType.AttackDog,
                        enemyCount = 5,
                        qudrant = Qudrant.Random
                    }
                ]
            },
            new SpawnEventConfig
            {
                spawnTime = 60,
                enemyConfig =
                [
                    new SpawnEventEnemyConfig
                    {
                        enemyType = EEntityType.GGI,
                        enemyCount = 1,
                        qudrant = Qudrant.Random
                    }
                ]
            },
            new SpawnEventConfig
            {
                spawnTime = 100,
                enemyConfig =
                [
                    new SpawnEventEnemyConfig
                    {
                        enemyType = EEntityType.GI,
                        enemyCount = 6,
                        qudrant = Qudrant.Random
                    }
                ]
            },
            new SpawnEventConfig
            {
                spawnTime = 130,
                enemyConfig =
                [
                    new SpawnEventEnemyConfig
                    {
                        enemyType = EEntityType.GI,
                        enemyCount = 6,
                        qudrant = Qudrant.Random
                    }
                ]
            },
            new SpawnEventConfig
            {
                spawnTime = 160,
                enemyConfig =
                [
                    new SpawnEventEnemyConfig
                    {
                        enemyType = EEntityType.GI,
                        enemyCount = 6,
                        qudrant = Qudrant.Random
                    }
                ]
            },
            new SpawnEventConfig
            {
                spawnTime = 300,
                enemyConfig =
                [
                    new SpawnEventEnemyConfig
                    {
                        enemyType = EEntityType.Tanya,
                        enemyCount = 1,
                        qudrant = Qudrant.Random
                    }
                ]
            }
        ];

        public Queue<WaveConfig> waveQueue;
        public WaveConfig currentWave;
        public Player player;
        public int[] enemyCount;

        public int totalEnemyCount => enemyNode.GetChildCount();
        public Enemy[] enemies => enemyNode.GetChildren().Cast<Enemy>().ToArray();
        public Pickup[] pickups => pickupsNode.GetChildren().Cast<Pickup>().ToArray();
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
            RegisterSpawnEvents();
        }

        private void RegisterSpawnEvents()
        {
            foreach (SpawnEventConfig spawnEvent in spawnEvents)
            {
                Utils.DelayedInvoke(spawnEvent.spawnTime, () => SpawnEvent(spawnEvent.enemyConfig));
            }
        }

        public void SpawnEvent(SpawnEventEnemyConfig[] enemyConfigs)
        {
            foreach (SpawnEventEnemyConfig enemyConfig in enemyConfigs)
            {
                List<Enemy> enemies = SpawnerService.SpawnEnemyCluster(
                    enemyConfig.enemyType,
                    player.GlobalTransform.Origin,
                    enemyConfig.enemyCount
                );
                foreach (Enemy enemy in enemies)
                    enemyNode.AddChild(enemy);
            }
        }

        public void NextWave()
        {
            currentWave = null;
            if (waveQueue.Count == 0)
            {
                HandleVictory();
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
            pickupsNode.AddChild(expOrb);
        }

        public void SpawnCrate(Vector3 position)
        {
            UpgradeCrate crate = ResourceProvider.CreateResource<UpgradeCrate>("UpgradeCrate.tscn");
            pickupsNode.AddChild(crate);
            crate.GlobalPosition = position;
        }

        public static List<Enemy> GetEnemiesInRange(Vector3 location, double range)
        {
            return instance
                .enemies.Where(e => !e.dead)
                .Where(e => e.GlobalPosition.DistanceTo(location) < range)
                .ToList();
        }

        public static List<Enemy> GetClosestEnemiesToPlayer(
            int amount,
            double minRange = double.MaxValue
        )
        {
            return instance
                .enemies.Where(e => !e.dead)
                .OrderBy(e => e.distanceToPlayer)
                .Where(e => e.distanceToPlayer < minRange)
                .Take(amount)
                .ToList();
        }

        public void HandleDefeat()
        {
            if (GameEnded)
            {
                return;
            }
            GameEnded = true;
            Sound3DService.PlaySoundAtNode(player, "csof023.wav");
            GetTree().Root.AddChild(ResourceLoader.Load<PackedScene>("Prefabs/UI/DefeatScene.tscn").Instantiate<Control>());
        }

        public void HandleVictory()
        {
            if (GameEnded)
            {
                return;
            }
            GameEnded = true;
            Sound3DService.PlaySoundAtNode(player, "csof022.wav");
            GetTree().Root.AddChild(ResourceLoader.Load<PackedScene>("Prefabs/UI/VictoryScene.tscn").Instantiate<Control>());
        }
    }
}
