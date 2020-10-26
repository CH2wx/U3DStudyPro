using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes.SpawnZones
{
    class SphereSpawnZone : SpawnZone
    {
        [SerializeField]
        private bool surfaceOnly;

        public override Vector3 SpawnPoint
        {
            get
            {
                return surfaceOnly ? Random.onUnitSphere : Random.insideUnitSphere;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireSphere(Vector3.zero, GameFabricatingShapes.Instance.instantiateDistance);
        }
    }
}
