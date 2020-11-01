using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes.SpawnZones
{
    public abstract class SpawnZone : MonoBehaviour
    {
        public enum SpawnZoneType {
            Random, Sequential
        }

        public abstract Vector3 SpawnPoint { get; }
    }
}
