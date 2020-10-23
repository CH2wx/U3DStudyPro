using Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes
{
    public class GameFabricatingShapes : PersistableObject
    {
        public Shape prefab;
        private GameObject objectsParent;
        public float instantiateDistance = 15f;

        [Header("是否显示生成的对象")]
        public bool isShowCreateObjects = true;

        private bool isStartCreate;
        private bool isStartDestroy;

        [Header("可操作的按钮设置")]
        public KeyCode createKey = KeyCode.C;
        public KeyCode destoryKey = KeyCode.X;
        public KeyCode newGameKey = KeyCode.V;
        public KeyCode saveKey = KeyCode.S;
        public KeyCode loadKey = KeyCode.R;

        [Range(0, 1)]
        public float createBetweenTime = 0.5f;
        private float createLastTime = 0;
        public Text createText;

        [Range(0, 1)]
        public float destroyBetweenTime = 0.5f;
        private float destroyLastTime = 0;
        public Text destoryText;

        private string saveFilePath = "\\FabricatingShapes\\SaveFile";
        public PersistentStorage storage;
        private List<Shape> shapes;
        public ShapeFactory factory;

        public int levelCount = 0;
        private int loadedLevelBuildIndex = -1;

        private const int saveVersionId = 2;

        /*********************************************************** setter // getter *******************************************************/
        public float CreationSpeed {
            get
            {
                return float.Parse(createText.text);
            }
            set
            {
                isStartCreate = value > 0;
                createText.text = value.ToString();
            }
        }

        public float DestructionSpeed
        {
            get
            {
                return float.Parse(destoryText.text);
            }
            set
            {
                isStartDestroy = value > 0;
                destoryText.text = value.ToString();
            }
        }

        /*********************************************************** Logic Function *******************************************************/
        // Use this for initialization
        void Start()
        {
            isStartCreate = false;
            isStartDestroy = false;
            shapes = new List<Shape>();
            CreationSpeed = CreationSpeed;
            DestructionSpeed = DestructionSpeed;
            objectsParent = factory.ShapesParent;

            storage.ChangePath(saveFilePath);

            if(Application.isEditor)
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    Scene loadedScene = SceneManager.GetSceneAt(i);
                    if (loadedScene.name.Contains("Level_"))
                    {
                        loadedLevelBuildIndex = int.Parse(loadedScene.name.Split('_')[1]);
                        SceneManager.SetActiveScene(SceneManager.GetSceneAt(loadedLevelBuildIndex));
                        return;
                    }
                }
            }

            StartCoroutine(LoadLevel(2));
        }

        // Update is called once per frame
        void Update()
        {
            if (objectsParent.activeSelf != isShowCreateObjects)
            {
                objectsParent.SetActive(isShowCreateObjects);
            }
            else if (Input.GetKeyDown(createKey))
            {
                CreateShape(objectsParent.transform, prefab);
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
            else if (Input.GetKeyDown(destoryKey))
            {
                DestroyShape();
            }
            else
            {
                for (int i = 1; i < 10; i++)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                    {
                        StartNewGame();
                        StartCoroutine(LoadLevel(i));
                    }
                }
            }

            if (isStartCreate)
            {
                createLastTime += Time.unscaledDeltaTime * CreationSpeed;
                while (createLastTime > createBetweenTime)
                {
                    createLastTime -= createBetweenTime;
                    CreateShape(objectsParent.transform, prefab);
                }
            }
            if (isStartDestroy)
            {
                destroyLastTime += Time.unscaledDeltaTime * DestructionSpeed;
                while (destroyLastTime > destroyBetweenTime)
                {
                    destroyLastTime -= destroyBetweenTime;
                    DestroyShape();
                }
            }
        }

        private void StartNewGame()
        {
            for (int i = 0; i < shapes.Count; i++)
            {
                factory.Reclaim(shapes[i]);
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

        private void DestroyShape()
        {
            if (shapes.Count > 0)
            {
                int index = Random.Range(0, shapes.Count);
                factory.Reclaim(shapes[index]);
                int lastIndex = shapes.Count - 1;
                shapes[index] = shapes[lastIndex];
                shapes.RemoveAt(lastIndex);
            }
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(shapes.Count);
            writer.Write(loadedLevelBuildIndex);
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
            StartCoroutine(LoadLevel(versionId < 2 ? 1 : reader.ReadInt()));
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

        private IEnumerator LoadLevel()
        {
            enabled = false;
            string sceneName = "Level_1";
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (Application.isEditor)
            {
                if (!scene.isLoaded)
                {
                    //SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                    //yield return null;
                    yield return LoadAsyncLevel(sceneName);
                    scene = SceneManager.GetSceneByName(sceneName);
                }
            }
            SceneManager.SetActiveScene(scene);
            enabled = true;
        }

        private IEnumerator LoadLevel(int levelBuildIndex)
        {
            enabled = false;
            if (loadedLevelBuildIndex >= 0)
            {
                Debug.Log(loadedLevelBuildIndex);
                yield return SceneManager.UnloadSceneAsync(loadedLevelBuildIndex);
            }
            yield return SceneManager.LoadSceneAsync(
                levelBuildIndex, LoadSceneMode.Additive
            );
            SceneManager.SetActiveScene(
                SceneManager.GetSceneByBuildIndex(levelBuildIndex)
            );
            loadedLevelBuildIndex = levelBuildIndex;
            enabled = true;
        }

        private IEnumerator LoadAsyncLevel(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
}
