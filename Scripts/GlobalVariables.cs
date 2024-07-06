using System.Collections.Generic;
using Godot;

namespace RA2Survivors
{
    public static class GlobalVariables
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

        public static T LoadEntity<T>(string entityName)
            where T : RigidBody3D
        {
            return GD.Load<PackedScene>(ENTITY_PATHS[entityName]).Instantiate<T>();
        }

        public static T LoadEntity<T>(EEntityType entityType)
            where T : RigidBody3D
        {
            return LoadEntity<T>(ENTITY_NAMES[entityType]);
        }

        public static EEntityType SelectedCharacter = EEntityType.Conscript;

        // public static ExpOrb CreateExpOrb(double expAmount) { }
    }
}
