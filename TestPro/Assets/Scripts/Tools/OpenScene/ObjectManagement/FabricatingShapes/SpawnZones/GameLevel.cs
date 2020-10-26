using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes.SpawnZones
{
    class GameLevel : MonoBehaviour
    {
        [SerializeField]
        private SpawnZone spawnZone;

        private void Start()
        {
            GameFabricatingShapes.Instance.SpawnZoneOfLevel = spawnZone;
        }
    }
}
