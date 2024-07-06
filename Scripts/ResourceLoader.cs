using System;
using System.Collections.Generic;
using Godot;

namespace RA2Survivors
{
    public static class ResourceLoader
    {
        private static readonly Dictionary<string, string> ENTITY_PATHS = new Dictionary<
            string,
            string
        >()
        {
            ["Conscript"] = "res://Entities/Conscript.tscn",
            ["GI"] = "res://Entities/GI.tscn"
        };
        private static readonly Dictionary<EEntityType, string> ENTITY_NAMES = new Dictionary<
            EEntityType,
            string
        >()
        {
            [EEntityType.Conscript] = "Conscript",
            [EEntityType.GI] = "GI"
        };
        private static ExpOrbConfig[] expOrbConfigs = new ExpOrbConfig[]
        {
            new ExpOrbConfig { expThreshold = 50, scenePath = "res://ExpOrbs/ExpOrbBig.tscn" },
            new ExpOrbConfig { expThreshold = 10, scenePath = "res://ExpOrbs/ExpOrbMedium.tscn" },
            new ExpOrbConfig { expThreshold = 0, scenePath = "res://ExpOrbs/ExpOrbSmall.tscn" },
        };

        public static T LoadEntity<T>(string entityName)
            where T : RigidBody3D
        {
            return Godot
                .ResourceLoader.Load<PackedScene>(
                    ENTITY_PATHS[entityName],
                    cacheMode: Godot.ResourceLoader.CacheMode.Reuse
                )
                .Instantiate<T>();
        }

        public static T LoadEntity<T>(EEntityType entityType)
            where T : RigidBody3D
        {
            return LoadEntity<T>(ENTITY_NAMES[entityType]);
        }

        public static EEntityType SelectedCharacter = EEntityType.Conscript;

        public static ExpOrb CreateExpOrb(double expAmount)
        {
            ExpOrbConfig config = null;
            foreach (ExpOrbConfig orbConfig in expOrbConfigs)
            {
                if (expAmount >= orbConfig.expThreshold)
                {
                    config = orbConfig;
                    break;
                }
            }
            if (config == null)
            {
                config = expOrbConfigs[expOrbConfigs.Length - 1];
            }
            ExpOrb expOrb = Godot
                .ResourceLoader.Load<PackedScene>(
                    config.scenePath,
                    cacheMode: Godot.ResourceLoader.CacheMode.Reuse
                )
                .Instantiate<ExpOrb>();
            expOrb.expAmount = expAmount;
            return expOrb;
        }

        private class ExpOrbConfig
        {
            public double expThreshold;
            public string scenePath;
        }
    }
}
