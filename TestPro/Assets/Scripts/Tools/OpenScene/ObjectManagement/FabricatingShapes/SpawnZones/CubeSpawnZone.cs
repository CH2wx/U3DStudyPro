using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes.SpawnZones
{
    class CubeSpawnZone:SpawnZone
    {
        [SerializeField]
        private bool surfaceOnly;
        public override Vector3 SpawnPoint
        {
            get
            {
                Vector3 p;
                p.x = Random.Range(-0.5f, 0.5f);
                p.y = Random.Range(-0.5f, 0.5f);
                p.z = Random.Range(-0.5f, 0.5f);
                if (surfaceOnly)
                {
                    int axis = Random.Range(0, 3);
                    p[axis] = p[axis] < 0 ? -0.5f : 0.5f;
                }
                return p;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(
                Vector3.zero, 
                new Vector3(
                    GameFabricatingShapes.Instance.instantiateDistance,
                    GameFabricatingShapes.Instance.instantiateDistance,
                    GameFabricatingShapes.Instance.instantiateDistance
                )
            );
        }
    }
}
