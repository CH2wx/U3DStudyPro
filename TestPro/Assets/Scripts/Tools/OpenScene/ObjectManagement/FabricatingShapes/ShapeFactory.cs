using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes
{
    [CreateAssetMenu]
    public class ShapeFactory : ScriptableObject
    {
        [SerializeField]
        private Shape[] prefabs;

        [SerializeField]
        private Material[] materials;

        [SerializeField, Tooltip("是否开启对象池")]
        private bool recycle;
        private List<Shape>[] pools;

        private Scene poolScene;
        private GameObject shapesParent;

        [System.NonSerialized]
        private int facotryId = int.MinValue;
        public int FacotryId
        {
            get
            {
                return facotryId;
            }
            set
            {
                if (facotryId == int.MinValue && value != int.MinValue)
                {
                    facotryId = value;
                }
                else
                {
                    Debug.LogError("Not allowed to change factoryId.");
                }
            }
        }

        public List<Shape>[] Pools
        {
            get
            {
                if (pools == null)
                {
                    pools = CreatePools(prefabs.Length);

                    if (Application.isEditor)
                    {
                        poolScene = SceneManager.GetSceneByName(name);
                        if (poolScene.isLoaded)
                        {
                            GameObject[] rootObjects = poolScene.GetRootGameObjects();
                            Debug.Log(rootObjects);
                            for (int i = 0; i < rootObjects.Length; i++)
                            {
                                Shape pooledShape = rootObjects[i].GetComponent<Shape>();
                                if (!pooledShape.gameObject.activeSelf)
                                {
                                    Debug.Log(pooledShape);
                                    pools[pooledShape.ShapeId].Add(pooledShape);
                                }
                            }
                            return pools;
                        }
                    }
                    poolScene = SceneManager.CreateScene(name);
                    SceneManager.MoveGameObjectToScene(ShapesParent, poolScene);
                }
                return pools;
            }

            set
            {
                pools = value;
            }
        }

        public GameObject ShapesParent
        {
            get
            {
                if (shapesParent == null)
                {
                    shapesParent = new GameObject("shapesPool");
                }
                return shapesParent;
            }

            set
            {
                shapesParent = value;
            }
        }

        private List<Shape>[] CreatePools(int length)
        {
            List<Shape>[] newPools = new List<Shape>[length];
            for (int i = 0; i < newPools.Length; i++)
            {
                newPools[i] = new List<Shape>();
            }

            return newPools;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="shapeToRecycle"></param>
        public void Reclaim(Shape shapeToRecycle)
        {
            if (shapeToRecycle.OriginalFactory != this)
            {
                Debug.LogError("Tried to reclaim shape with wrong factory.");
                return;
            }
            if (recycle)
            {
                shapeToRecycle.gameObject.SetActive(false);
                pools[shapeToRecycle.ShapeId].Add(shapeToRecycle);
            }
            else
            {
                Destroy(shapeToRecycle.gameObject);
            }
        }

        public Shape Get(int shapeId = 0, int materialId = 0)
        {
            Shape instance = null;
            if (recycle)
            {
                List<Shape> pool = Pools[shapeId];
                if (pool.Count > 0)
                {
                    int lastIndex = pool.Count - 1;
                    instance = pool[lastIndex];
                    instance.gameObject.SetActive(true);
                    pool.RemoveAt(lastIndex);
                }
            }
            if (instance == null)
            {
                instance = Instantiate(prefabs[shapeId]);
                instance.OriginalFactory = this;
                instance.ShapeId = shapeId;
                instance.SetMaterial(materials[materialId], materialId);
            }
            return instance;
        }

        public Shape GetRandom()
        {
            return Get(
                Random.Range(0, prefabs.Length),
                Random.Range(0, materials.Length)
            );
        }
    }
}

