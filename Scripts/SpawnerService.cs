using System;
using System.Collections.Generic;
using Godot;

namespace RA2Survivors
{
    public static class SpawnerService
    {
        public static double DistanceDespawnThreshold = 9999999;

        public static double spawnRangeXStart = 52;
        public static double spawnRangeXLength = 12;
        public static double spawnRangeZStart = 32;
        public static double spawnRangeZLength = 10;

        static SpawnerService()
        {
            DistanceDespawnThreshold =
                Math.Sqrt(
                    (spawnRangeXStart + spawnRangeXLength) * (spawnRangeXStart + spawnRangeXLength)
                        + (spawnRangeZStart + spawnRangeZLength)
                            * (spawnRangeZStart + spawnRangeZLength)
                ) + 1;
        }

        public static List<Enemy> SpawnEnemyCluster(EEntityType type, Vector3 center, int count)
        {
            List<Enemy> enemies = new List<Enemy>();
            Vector3 offset = SpawnRangeOffset(center);
            for (int i = 0; i < count; i++)
            {
                Enemy enemy = ResourceProvider.LoadEntity<Enemy>(type);
                enemies.Add(enemy);
                enemy.Transform = new Transform3D(Basis.Identity, offset);
            }

            return enemies;
        }

        public static Enemy SpawnEnemy(EEntityType type, Vector3 center)
        {
            Enemy enemy = ResourceProvider.LoadEntity<Enemy>(type);
            enemy.Transform = new Transform3D(Basis.Identity, SpawnRangeOffset(center));
            return enemy;
        }

        public static Vector3 SpawnRangeOffset(Vector3 center)
        {
            Vector3 offset = center;
            offset.Y = 1;
            if (GD.RandRange(0, 1) == 0)
            {
                // LEFT/RIGHT
                offset.X +=
                    RandomSign()
                    * (float)GD.RandRange(spawnRangeXStart, spawnRangeXStart + spawnRangeXLength);
                offset.Z += (float)GD.RandRange(-spawnRangeZStart, spawnRangeZStart);
            }
            else
            {
                // UP/DOWN
                offset.X += (float)GD.RandRange(-spawnRangeXStart, spawnRangeXStart);
                offset.Z +=
                    RandomSign()
                    * (float)GD.RandRange(spawnRangeZStart, spawnRangeZStart + spawnRangeZLength);
            }

            return offset;
        }

        private static float RandomSign() => GD.RandRange(0, 1) == 0 ? -1 : 1;
    }
}
