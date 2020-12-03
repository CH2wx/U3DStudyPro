using Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects;
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

        [SerializeField]
        private bool overrideConfig;

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

        public override void Save(GameDataWriter writer)
        {
            writer.Write(nextSpawnZoneIndex);
        }

        public override void Load(GameDataReader reader)
        {
            nextSpawnZoneIndex = reader.ReadInt();
        }

        //public override void ConfigureSpawn(Shape shape)
        public override Shape SpawnShape()
        {
            if (overrideConfig)
            {
                //base.ConfigureSpawn(shape);
                return base.SpawnShape();
            }
            else
            {
                int index;
                switch (spawnZoneType)
                {
                    case SpawnZoneType.Sequential:
                        index = (nextSpawnZoneIndex + 1) % spawnZones.Length;
                        break;
                    case SpawnZoneType.Random:
                    default:
                        index = Random.Range(0, spawnZones.Length);
                        break;
                }
                //spawnZones[index].ConfigureSpawn(shape);
                return spawnZones[index].SpawnShape();
            }
        }
    }
}
