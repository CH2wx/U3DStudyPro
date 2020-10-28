using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes.SpawnZones
{
    class GameLevel : MonoBehaviour
    {
        [SerializeField]
        private SpawnZone spawnZone;

        public static GameLevel Current { get; private set; }

        private void OnEnable()
        {
            Current = this;
        }

        public Vector3 SpawnPoint
        {
            get
            {
                return spawnZone.SpawnPoint;
            }
        }

        private void Start()
        {
            GameFabricatingShapes.Instance.SpawnZoneOfLevel = spawnZone;
        }
    }
}
