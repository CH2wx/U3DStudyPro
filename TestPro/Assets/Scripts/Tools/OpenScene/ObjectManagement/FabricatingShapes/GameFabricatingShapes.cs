using Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes.SpawnZones;
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
        [SerializeField]
        private Shape prefab;
        //private GameObject objectsParent;
        public float instantiateDistance = 15f;

        [SerializeField, Header("是否显示生成的对象")]
        public bool isShowCreateObjects = true;

        private bool isStartCreate;
        private bool isStartDestroy;

        [Header("可操作的按钮设置")]
        [SerializeField]
        private KeyCode createKey = KeyCode.C;
        [SerializeField]
        private KeyCode destoryKey = KeyCode.X;
        [SerializeField]
        private KeyCode newGameKey = KeyCode.V;
        [SerializeField]
        private KeyCode saveKey = KeyCode.S;
        [SerializeField]
        private KeyCode loadKey = KeyCode.R;

        [SerializeField, Range(0, 1)]
        private float createBetweenTime = 0.5f;
        private float createLastTime = 0;
        [SerializeField]
        private Text createText;
        [SerializeField]
        private Slider creationSlider;

        [SerializeField, Range(0, 1)]
        private float destroyBetweenTime = 0.5f;
        private float destroyLastTime = 0;
        [SerializeField]
        private Text destoryText;
        [SerializeField]
        private Slider destroySlider;

        private string saveFilePath = "\\FabricatingShapes\\SaveFile";
        public PersistentStorage storage;
        private List<Shape> shapes;
        //public ShapeFactory factory;
        public ShapeFactory[] factories;
        //public SpawnZone SpawnZoneOfLevel { set; get; }

        [SerializeField]
        private bool reseedOnLoad;          //是否在加载是重新刷新随机种子
        private Random.State mainRandomState;

        private int loadedLevelBuildIndex = -1;

        private const int saveVersionId = 5;

        public static GameFabricatingShapes Instance { get; private set; }

        /*********************************************************** setter // getter *******************************************************/
        public float CreationSpeed {
            get
            {
                return float.Parse(createText.text);
            }
            set
            {
                isStartCreate = value > 0;
                creationSlider.value = value;
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
                destroySlider.value = value;
                destoryText.text = value.ToString();
            }
        }

        /*********************************************************** Logic Function *******************************************************/

        // OnEnable在Start之前被调用，用于初始化静态变量，方便其他方法在Start中进行调用
        private void OnEnable()
        {
            Instance = this;
            if (factories[0].FacotryId != 0)
            {
                for (int i = 0; i < factories.Length; i++)
                {
                    factories[i].FacotryId = i;
                }
            }
        }

        // Use this for initialization
        void Start()
        {
            isStartCreate = false;
            isStartDestroy = false;
            shapes = new List<Shape>();
            CreationSpeed = CreationSpeed;
            DestructionSpeed = DestructionSpeed;
            //objectsParent = factory.ShapesParent;
            mainRandomState = Random.state;

            storage.ChangePath(saveFilePath);

            // 如果场景里存在关卡场景，那么激活它并不再加载其他关卡场景
            if(Application.isEditor)
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    Scene loadedScene = SceneManager.GetSceneAt(i);
                    if (loadedScene.name.Contains("Level_"))
                    {
                        loadedLevelBuildIndex = int.Parse(loadedScene.name.Split('_')[1]);
                        SceneManager.SetActiveScene(loadedScene);
                        return;
                    }
                }
            }

            StartNewGame();
            StartCoroutine(LoadLevel(3));
        }
        
        private void FixedUpdate()
        {
            if (factories[0].ShapesParent.activeSelf != isShowCreateObjects)
            {
                for (int i = 0; i < factories.Length; i++)
                {
                    factories[i].ShapesParent.SetActive(isShowCreateObjects);
                }
            }
            else if (Input.GetKeyDown(createKey))
            {
                CreateShape(prefab);
            }
            else if (Input.GetKeyDown(newGameKey))
            {
                StartNewGame();
                StartCoroutine(LoadLevel(loadedLevelBuildIndex));
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
                    CreateShape(prefab);
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

            for (int i = 0; i < shapes.Count; i++)
            {
                shapes[i].GameUpdate();
            }
        }

        private void StartNewGame()
        {
            for (int i = 0; i < shapes.Count; i++)
            {
                //factory.Reclaim(shapes[i]);
                shapes[i].Reclaim();
            }
            shapes.Clear();

            Random.state = mainRandomState;
            int seed = Random.Range(0, int.MaxValue ^ (int)Time.unscaledTime);
            mainRandomState = Random.state;
            Random.InitState(seed);

            CreationSpeed = 0;
            DestructionSpeed = 0;
        }

        private void CreateShape(Shape prefab)
        {
            //Shape instance = factory.GetRandom();
            //GameLevel.Current.ConfigureSpawn(instance);
            Shape instance = GameLevel.Current.SpawnShape();
            instance.transform.SetParent(factories[instance.OriginalFactory.FacotryId].ShapesParent.transform);
            shapes.Add(instance);
        }

        private void DestroyShape()
        {
            if (shapes.Count > 0)
            {
                int index = Random.Range(0, shapes.Count);
                //factory.Reclaim(shapes[index]);
                shapes[index].Reclaim();
                int lastIndex = shapes.Count - 1;
                shapes[index] = shapes[lastIndex];
                shapes.RemoveAt(lastIndex);
            }
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(shapes.Count);
            // Random.Range是一个伪随机数，所以如果在保存之后出现的物体位置一样，保存Random的伪随机序列即可
            writer.Write(Random.state);
            writer.Write(CreationSpeed);
            writer.Write(DestructionSpeed);
            // 储存最后一次加载的Level关卡
            writer.Write(loadedLevelBuildIndex);
            GameLevel.Current.Save(writer);
            for (int i = 0; i < shapes.Count; i++)
            {
                writer.Write(shapes[i].OriginalFactory.FacotryId);
                writer.Write(shapes[i].ShapeId);
                writer.Write(shapes[i].MaterialId);
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

            StartCoroutine(LoadGame(reader));
        }

        private IEnumerator LoadGame(GameDataReader reader)
        {
            int versionId = reader.Version;
            int count = reader.ReadInt();
            if (versionId >= 3)
            {
                Random.State state = reader.ReadRandomState();
                if (!reseedOnLoad)
                {
                    Random.state = state;
                }
                CreationSpeed = reader.ReadFloat();
                DestructionSpeed = reader.ReadFloat();
            }

            yield return LoadLevel(versionId < 2 ? 1 : reader.ReadInt());
            GameLevel.Current.Load(reader);
            for (int i = 0; i < count; i++)
            {
                int originalFacotryId = reader.ReadInt();
                int shapeId = reader.ReadInt();
                int materialId = reader.ReadInt();
                Shape instance = factories[originalFacotryId].Get(shapeId, materialId);
                instance.transform.SetParent(factories[originalFacotryId].ShapesParent.transform);
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
