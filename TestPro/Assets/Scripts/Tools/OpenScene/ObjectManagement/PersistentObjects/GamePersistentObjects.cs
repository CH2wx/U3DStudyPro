using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects
{
    public class GamePersistentObjects : PersistableObject
    {
        public Transform prefab;
        public float instantiateDistance = 15f;

        public bool isStartCreate = true;
        public KeyCode createKey = KeyCode.C;
        public KeyCode newGameKey = KeyCode.V;
        public KeyCode saveKey = KeyCode.S;
        public KeyCode loadKey = KeyCode.R;

        [Range(0, 1)]
        public float createBetweenTime = 0.5f;
        private float createLastTime;

        public bool isShowCreateObjects = true;
        private GameObject objectsParent;
        public List<Transform> createObjects;

        public PersistentStorage storage;
        public List<PersistableObject> objects;
       
        private OpenFile saveFile;

        private void Awake()
        {
            isStartCreate = true;
            createObjects = new List<Transform>();
            objects = new List<PersistableObject>();
            saveFile = new OpenFile();
        }

        // Use this for initialization
        void Start()
        {
            objectsParent = new GameObject("CreateObjects");
        }

        // Update is called once per frame
        void Update()
        {
            if (objectsParent.activeSelf != isShowCreateObjects)
            {
                objectsParent.SetActive(isShowCreateObjects);
            }

            if (Input.GetKeyDown(createKey))
            {
                isStartCreate = !isStartCreate;
                createLastTime = 0;
            }
            else if (Input.GetKeyDown(newGameKey))
            {
                StartNewGame();
            }
            else if (Input.GetKeyDown(saveKey))
            {
                //saveFile.Save(createObjects);
                storage.Save(this);
            }
            else if (Input.GetKeyDown(loadKey))
            {
                StartNewGame();
                //CreatePrefabsByLoad(objectsParent.transform, prefab);
                storage.Load(this);
            }

            if (isStartCreate)
            {
                createLastTime += Time.unscaledDeltaTime;
                if (createLastTime > createBetweenTime)
                {
                CreatePrefab(objectsParent.transform, prefab);
                    createLastTime = 0;
                }
            }
        }

        private void StartNewGame()
        {
            for (int i = 0; i < createObjects.Count; i++)
            {
                Destroy(createObjects[i].gameObject);
            }
            objects.Clear();
            createObjects.Clear();
        }

        private void CreatePrefab(Transform parent, Transform prefab)
        {
            Vector3 localPosition = Random.insideUnitSphere * instantiateDistance;
            Transform obj = Instantiate(prefab, localPosition, Random.rotation, parent);
            obj.localScale = Vector3.one * Random.Range(0.1f, 1.0f);
            createObjects.Add(obj);
            objects.Add(obj.GetComponent<PersistableObject>());
        }

        private void CreatePrefabsByLoad(Transform parent, Transform preafab)
        {
            var datas = saveFile.Load();
            for (int i = 0; i < datas.Length; i++)
            {
                Transform obj = Instantiate(prefab, parent);
                obj.localPosition = datas[i].position;
                obj.localScale = datas[i].scale;
                obj.localRotation = datas[i].quaternion;
                createObjects.Add(obj);
                objects.Add(obj.GetComponent<PersistableObject>());
            }
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(objects.Count);
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].Save(writer);
            }
        }

        public override void Load(GameDataReader reader)
        {
            int count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                PersistableObject o = Instantiate(prefab, objectsParent.transform).GetComponent<PersistableObject>();
                o.name = i.ToString();
                o.Load(reader);
                objects.Add(o);
                createObjects.Add(o.transform);
            }
        }
    }
}
