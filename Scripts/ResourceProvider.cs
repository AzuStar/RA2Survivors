using System;
using System.Collections.Generic;
using Godot;

namespace RA2Survivors
{
    public static class ResourceProvider
    {
        public const string RESOURCE_MASTER_PATH = "res://Prefabs/";

        public static T CreateResource<T>(string name)
            where T : Node
        {
            return ResourceLoader.Load<PackedScene>(RESOURCE_MASTER_PATH + name).Instantiate<T>();
        }

        static ResourceProvider() { }

        #region Entities
        private static readonly Dictionary<string, string> ENTITY_PATHS = new Dictionary<
            string,
            string
        >()
        {
            ["Conscript"] = RESOURCE_MASTER_PATH + "Entities/Conscript.tscn",
            ["GI"] = RESOURCE_MASTER_PATH + "Entities/GI.tscn",
            ["GGI"] = RESOURCE_MASTER_PATH + "Entities/GGI.tscn",
            ["Sniper"] = RESOURCE_MASTER_PATH + "Entities/Sniper.tscn",
            ["Seal"] = RESOURCE_MASTER_PATH + "Entities/Seal.tscn",
            ["Spy"] = RESOURCE_MASTER_PATH + "Entities/Spy.tscn",
            ["Engineer"] = RESOURCE_MASTER_PATH + "Entities/Engineer.tscn",
            ["AttackDog"] = RESOURCE_MASTER_PATH + "Entities/AttackDog.tscn",
            ["Tanya"] = RESOURCE_MASTER_PATH + "Entities/Tanya.tscn"
        };
        private static readonly Dictionary<EEntityType, string> ENTITY_NAMES = new Dictionary<
            EEntityType,
            string
        >()
        {
            [EEntityType.Conscript] = "Conscript",
            [EEntityType.GI] = "GI",
            [EEntityType.GGI] = "GGI",
            [EEntityType.Sniper] = "Sniper",
            [EEntityType.Seal] = "Seal",
            [EEntityType.Spy] = "Spy",
            [EEntityType.Engineer] = "Engineer",
            [EEntityType.AttackDog] = "AttackDog",
            [EEntityType.Tanya] = "Tanya"
        };

        public static readonly Dictionary<EEntityType, List<string>> EntityDyingSounds = new Dictionary<
            EEntityType,
            List<string>
        >()
        {
            [EEntityType.Conscript] = new List<string>() { 
                "icondia.wav", "icondib.wav", "icondic.wav", "icondid.wav", "icondie.wav"
            },
            [EEntityType.GI] = new List<string>() { 
                "igidia.wav", "igidib.wav", "igidic.wav", "igidid.wav", "igidie.wav"
            },
            [EEntityType.GGI] = new List<string>() { 
                "iggidia.wav", "iggidib.wav", "iggidic.wav", "iggidid.wav", "iggidie.wav"
            },
            [EEntityType.Sniper] = new List<string>() { 
                "isnidia.wav", "isnidib.wav", "isnidic.wav"
            },
            [EEntityType.Seal] = new List<string>() { 
                "iseadia.wav", "iseadib.wav", "iseadic.wav"
            },
            [EEntityType.Spy] = new List<string>() { 
            },
            [EEntityType.Engineer] =new List<string>() { 
            },
            [EEntityType.AttackDog] = new List<string>() { 
                "idogdiea.wav"
            },
            [EEntityType.Tanya] = new List<string>() { 
                "itandia.wav", "itandib.wav", "itandic.wav"
            }
        };

        public static T CreateEntity<T>(string entityName)
            where T : RigidBody3D
        {
            return ResourceLoader.Load<PackedScene>(ENTITY_PATHS[entityName]).Instantiate<T>();
        }

        public static T LoadEntity<T>(EEntityType entityType)
            where T : RigidBody3D
        {
            return CreateEntity<T>(ENTITY_NAMES[entityType]);
        }

        #endregion

        #region FloatingText
        public static string FloatingTextPath = RESOURCE_MASTER_PATH + "/FloatingText.tscn";

        public static FloatingText CreateFloatingText()
        {
            FloatingText floatingText = ResourceLoader
                .Load<PackedScene>(FloatingTextPath)
                .Instantiate<FloatingText>();
            return floatingText;
        }
        #endregion

        public static EEntityType SelectedCharacter = EEntityType.Conscript;

        #region Exp Orbs
        private const string EXP_ORB_RESOURCE_PATH = RESOURCE_MASTER_PATH + "ExpOrbs/";
        private static ExpOrbConfig[] expOrbConfigs = new ExpOrbConfig[]
        {
            new ExpOrbConfig { expThreshold = 50, resourcePath = "ExpOrbBig.tscn" },
            new ExpOrbConfig { expThreshold = 10, resourcePath = "ExpOrbMedium.tscn" },
            new ExpOrbConfig { expThreshold = 0, resourcePath = "ExpOrbTiny.tscn" },
        };

        public static ExpOrb CreateExpOrb(Vector3 position, double expAmount)
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
            ExpOrb expOrb = ResourceLoader
                .Load<PackedScene>(EXP_ORB_RESOURCE_PATH + config.resourcePath)
                .Instantiate<ExpOrb>();
            expOrb.expAmount = expAmount;
            expOrb.GlobalTransform = new Transform3D(Basis.Identity, position);
            return expOrb;
        }

        private class ExpOrbConfig
        {
            public double expThreshold;
            public string resourcePath;
        }

        #endregion
    }
}
