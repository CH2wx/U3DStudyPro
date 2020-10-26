using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes.SpawnZones
{
    class CompositeSpawnZone : SpawnZone
    {
        [SerializeField]
        private SpawnZone[] spawnZones;

        public override Vector3 SpawnPoint
        {
            get
            {
                int index = Random.Range(0, spawnZones.Length);
                Vector3 p = spawnZones[index].SpawnPoint;
                p *= GameFabricatingShapes.Instance.instantiateDistance;
                p += spawnZones[index].transform.position;
                return p;
            }
        }
    }
}
