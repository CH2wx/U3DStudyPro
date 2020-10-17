using Assets.Scripts.Tools.OpenScene.Base.Math;
using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.Base.UI
{
    public class Cube : MonoBehaviour {
		private MathManager.Type type;

		void Update () {
			if (MathManager.IsAthleticType(type))
			{
				Vector3 tempVct = transform.position;
				tempVct.y = Math2DFunction.F(tempVct.x + Time.time, 0, type);
				transform.position = tempVct;
			}
		}

        /// <summary>
        /// 初始化正方体的transform信息，包括：x坐标，缩放，获取y坐标的公式类型
        /// </summary>
        public void Init(float x, Vector3 scale, MathManager.Type cubeType)
		{
			type = cubeType;
			// 根据定义的公式计算y的坐标 y = f(x)
			float y = Math2DFunction.F(x, 0, type);
			transform.localPosition = new Vector3(x, y, 0);
			// 对立方体大小进行缩放
			transform.localScale = scale;
		}
	}
}