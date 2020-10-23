using Assets.Scripts.Tools.OpenScene.Base.Math;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.Base.UI
{
    public class Graph : MonoBehaviour {

		public Transform pointPrefab;

		[Range(10, 100)]
		public int resolution = 10;
		private int lastResolution;
        
		private List<Transform> list = new List<Transform>();

        public MathManager.Type fType = MathManager.Type.Pow2;
		private MathManager.Type lastType;

		void Awake()
		{
			DrawCube(resolution);
		}

		void Update ()
		{
			if (lastResolution != resolution || lastType != fType)
			{
				ClearList();
				DrawCube(resolution);
			}
		}

		private void DrawCube (int num)
		{
			// 根据个数调整缩放比例
			float step = 2.0f / resolution;
			// 立方体缩放的值
			Vector3 scale = Vector3.one * step;
			for (int i = 0; i < num; i++)
			{
				Transform point = Instantiate(pointPrefab);	
				Cube cubeComp = point.GetComponent<Cube>();
				// i * step 的范围是[0, 2]， 往左偏移1个单位让范围保持在[-1, 1]，因为坐标以立方体中心为坐标原点，所以还需要向右偏移半个立方体单位的长度（0.5 * step）
				float x = (i + 0.5f) * step - 1.0f;
				cubeComp.Init(x, scale, fType);
				point.SetParent(transform);
				// 储存到容器中方便操作
				list.Add(point);
			}
			lastResolution = num;
			lastType = fType;
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
