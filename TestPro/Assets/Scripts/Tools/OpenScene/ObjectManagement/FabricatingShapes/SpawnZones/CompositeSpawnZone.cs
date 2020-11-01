using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes.SpawnZones
{
    class CompositeSpawnZone : SpawnZone
    {
        [SerializeField]
        private SpawnZone[] spawnZones;

        [SerializeField]
        private SpawnZoneType spawnZoneType = SpawnZoneType.Sequential;
        private int nextSpawnZoneIndex = 0;

        public override Vector3 SpawnPoint
        {
            get
            {
                int index = 0;
                switch (spawnZoneType)
                {
                    case SpawnZoneType.Random:
                        index = Random.Range(0, spawnZones.Length);
                        break;
                    case SpawnZoneType.Sequential:
                        index = nextSpawnZoneIndex;
                        nextSpawnZoneIndex = (nextSpawnZoneIndex + 1) % spawnZones.Length;
                        break;
                }
                return spawnZones[index].SpawnPoint;
            }
        }
    }
}
