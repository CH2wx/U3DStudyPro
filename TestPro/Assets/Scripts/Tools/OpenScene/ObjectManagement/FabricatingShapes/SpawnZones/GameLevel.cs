using Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects;
using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes.SpawnZones
{
    class GameLevel : PersistableObject
    {
        [SerializeField]
        private SpawnZone spawnZone;

        [SerializeField]
        private PersistableObject[] persistableObjects;

        public static GameLevel Current { get; private set; }

        private void OnEnable()
        {
            Current = this;
            if (persistableObjects == null)
            {
                persistableObjects = new PersistableObject[0];
            }
        }

        //public void ConfigureSpawn(Shape shape)
        //{
        //    spawnZone.ConfigureSpawn(shape);
        //}

        public Shape SpawnShape()
        {
            return spawnZone.SpawnShape();
        }

        private void Start()
        {
            //GameFabricatingShapes.Instance.SpawnZoneOfLevel = spawnZone;
        }

        public override void Load(GameDataReader reader)
        {
            int length = reader.ReadInt();
            for (int i = 0; i < length; i++)
            {
                persistableObjects[i].Load(reader);
            }
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(persistableObjects.Length);
            for (int i = 0; i < persistableObjects.Length; i++)
            {
                persistableObjects[i].Save(writer);
            }
        }
    }
}
