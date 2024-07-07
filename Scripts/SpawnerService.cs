using System;
using Godot;

namespace RA2Survivors
{
    public static class SpawnerService
    {
        public static double DistanceDespawnThreshold = 9999999;

        public static double spawnRangeXStart = 20;
        public static double spawnRangeXLength = 5;
        public static double spawnRangeZStart = 12;
        public static double spawnRangeZLength = 4;

        static SpawnerService()
        {
            DistanceDespawnThreshold =
                Math.Sqrt(
                    (spawnRangeXStart + spawnRangeXLength) * (spawnRangeXStart + spawnRangeXLength)
                        + (spawnRangeZStart + spawnRangeZLength)
                            * (spawnRangeZStart + spawnRangeZLength)
                ) + 1;
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
