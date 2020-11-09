using Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects;
using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes.SpawnZones
{
    public abstract class SpawnZone : PersistableObject
    {
        public enum SpawnZoneType {
            Random, Sequential
        }

        public abstract Vector3 SpawnPoint { get; }
    }
}
