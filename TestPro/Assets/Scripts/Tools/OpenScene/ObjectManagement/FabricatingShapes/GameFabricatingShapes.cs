using Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes
{
    public class GameFabricatingShapes : PersistableObject
    {
        public Shape prefab;
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

        private string saveFilePath = "\\FabricatingShapes\\SaveFile";
        public PersistentStorage storage;
        public List<Shape> shapes;
        public ShapeFactory factory;

        public const int saveVersionId = 1;

        private void Awake()
        {
            isStartCreate = true;
            shapes = new List<Shape>();

            storage.ChangePath(saveFilePath);
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
                storage.Save(this, saveVersionId);
            }
            else if (Input.GetKeyDown(loadKey))
            {
                StartNewGame();
                storage.Load(this);
            }

            if (isStartCreate)
            {
                createLastTime += Time.unscaledDeltaTime;
                if (createLastTime > createBetweenTime)
                {
                    CreateShape(objectsParent.transform, prefab);
                    createLastTime = 0;
                }
            }
        }

        private void StartNewGame()
        {
            for (int i = 0; i < shapes.Count; i++)
            {
                Destroy(shapes[i].gameObject);
            }
            shapes.Clear();
        }

        private void CreateShape(Transform parent, Shape prefab)
        {
            Shape instance = factory.GetRandom();
            instance.transform.localPosition = Random.insideUnitSphere * instantiateDistance;
            instance.transform.localRotation = Random.rotation;
            instance.transform.localScale = Vector3.one * Random.Range(0.1f, 1.0f);
            instance.transform.SetParent(parent);

            // hue 色调：0-360°；saturation 饱和度：0-100%； value 亮度：0-100%； alpha 透明度：0-100%
            instance.SetColor(Random.ColorHSV(
                                        hueMin: 0f, hueMax: 1f,
                                        saturationMin: 0.5f, saturationMax: 1f,
                                        valueMin: 0.25f, valueMax: 1f,
                                        alphaMin: 1f, alphaMax: 1f));

            shapes.Add(instance);
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(shapes.Count);
            for (int i = 0; i < shapes.Count; i++)
            {
                shapes[i].Save(writer);
            }
        }

        public override void Load(GameDataReader reader)
        {
            // 确保版本正常
            int versionId = reader.Version;
            if (versionId > saveVersionId)
            {
                Debug.LogError("Unsupported future save version " + versionId);
                return;
            }

            int count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                int shapeId = reader.ReadInt();
                int materialId = reader.ReadInt();
                Shape instance = factory.Get(shapeId, materialId);
                instance.transform.SetParent(objectsParent.transform);
                instance.name = i.ToString();
                instance.Load(reader);
                shapes.Add(instance);
            }
        }
    }
}
