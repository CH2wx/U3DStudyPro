using System;
using System.Collections.Generic;
using Assets.Scripts.Tools.OpenScene.Base.Math;
using Assets.Scripts.Tools.OpenScene.Base.UI;
using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.Base.MathSurface
{
    public class Graph : MonoBehaviour
    {
        public Transform pointPrefab;
        private List<Transform> list = new List<Transform>();

        [Range(10, 100)]
        public int resolution = 10;
        private int lastResolution;

        public MathManager.Type fType = MathManager.Type.Sin;
        private MathManager.Type lastType;

        public MathManager.FunType funType = MathManager.FunType.Fun2D;

        public bool isUpdate = false;

        private static Graph _instance;

        public static Graph Instance {
            get
            {
                return _instance;
            }
        }

        void Awake()
        {
            _instance = this;
            if (pointPrefab == null)
            {
                GameObject cube = SceneTools.Load("Assets/Res/Tools/OpenScene/Base/UI/Prefabs/Cube.prefab") as GameObject;
                pointPrefab = cube.transform;
            }

            //DrawCubes(resolution);
            float step = 2f / resolution;
            Vector3 scale = Vector3.one * step;
            ClearList();
            for (int i = 0; i < resolution; i++)
            {
                for (int j = 0; j < resolution; j++)
                {
                    Transform tf = Instantiate(pointPrefab);
                    tf.localScale = scale;
                    tf.SetParent(transform);
                    list.Add(tf);
                }
            }
        }

        void Update()
        {
            _instance = this;
            if (lastResolution != resolution || lastType != fType || isUpdate)
            {
                //ClearList();
                //DrawCubes(resolution);
                float t = Time.time;
                float step = 2f / resolution;
                for (int i = 0, z = 0; z < resolution; z++)
                {
                    float v = (z + 0.5f) * step - 1f;
                    for (int j = 0; j < resolution; j++, i++)
                    {
                        float u = (j + 0.5f) * step - 1f;
                        list[i].localPosition = Math3DFunction.F(u, v, fType);
                    }
                }
            }
        }

        private void DrawCubes(int num)
        {
            switch (funType)
            {
                case MathManager.FunType.Fun2D:
                    DrawCubesBy2DFun(num);
                    break;
                case MathManager.FunType.Fun3D:
                    DrawCubesBy3DFun(num);
                    break;
            }
        }

        private void DrawCubesBy3DFun(int num)
        {
            // 根据个数调整缩放比例
            float step = 2.0f / resolution;
            // 立方体缩放的值
            Vector3 scale = Vector3.one * step;
            Vector3 pos = Vector3.zero;
            for (int i = 0; i < num; i++)
            {
                pos.z = (i + 0.5f) * step - 1.0f;
                for (int j = 0; j < num; j++)
                {
                    // i * step 的范围是[0, 2]， 往左偏移1个单位让范围保持在[-1, 1]，因为坐标以立方体中心为坐标原点，所以还需要向右偏移半个立方体单位的长度（0.5 * step）
                    pos.x = (j + 0.5f) * step - 1.0f;
                    
                    CreateCube(Math3DFunction.F(pos.x, pos.z, fType), scale, fType, transform, i + "_" + j);
                }
            }
            lastResolution = resolution;
            lastType = fType;
        }

        private void DrawCubesBy2DFun(int num)
        {
            // 根据个数调整缩放比例
            float step = 2.0f / resolution;
            // 立方体缩放的值
            Vector3 scale = Vector3.one * step;
            Vector3 pos = Vector3.zero;
            for (int i = 0, z = 0; i < num; i++, z++)
            {
                pos.z = (z + 0.5f) * step - 1.0f;
                for (int j = 0; j < num; j++)
                {
                    // i * step 的范围是[0, 2]， 往左偏移1个单位让范围保持在[-1, 1]，因为坐标以立方体中心为坐标原点，所以还需要向右偏移半个立方体单位的长度（0.5 * step）
                    pos.x = (j + 0.5f) * step - 1.0f;
                    pos.y = Math2DFunction.F(pos.x, pos.z, fType);

                    CreateCube(pos, scale, fType, transform, i + "_" + j);
                }
            }
            lastResolution = resolution;
            lastType = fType;
        }

        private Transform CreateCube (Vector3 pos, Vector3 scale, MathManager.Type type, Transform parent, string name)
        {
            Transform obj = Instantiate(pointPrefab, parent, false);

            if (obj.GetComponent<Cube>() != null)
            {
                Destroy(obj.GetComponent<Cube>());
            }
            MathSurfaceCube cube = obj.GetComponent<MathSurfaceCube>();
            if (cube == null)
            {
                cube = obj.gameObject.AddComponent<MathSurfaceCube>();
            }

            //Debug.LogFormat("{0}\t{1}", cube == null, type);
            cube.SetType(type);
            cube.initPos = pos;

            obj.position = pos;
            obj.localScale = scale;
            obj.name = name;

            // 储存到容器中方便操作
            list.Add(obj);

            return obj;
        }

        private void ClearList()
        {
            for (int i = 0; i < list.Count; i++)
            {
                Destroy(list[i].gameObject);
            }
            list.Clear();
        }
    }
}