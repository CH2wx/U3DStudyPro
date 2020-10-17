using Assets.Scripts.Tools.OpenScene.Base.Math;
using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.Base.MathSurface
{
    public class MathSurfaceCube : MonoBehaviour {
		private MathManager.Type type;
        public Vector3 initPos;

		void LateUpdate () {
			if (MathManager.IsAthleticType(type))
            {
                Vector3 tempVct = transform.position;
                switch (Graph.Instance.funType)
                {
                    case MathManager.FunType.Fun2D:
                        tempVct.y = Math2DFunction.F(tempVct.x, tempVct.z, type);
                        break;
                    case MathManager.FunType.Fun3D:
                        tempVct = Math3DFunction.F(initPos.x, initPos.z, type);
                        break;
                }
				transform.position = tempVct;
			}
		}

        public void SetType (MathManager.Type type)
        {
            this.type = type;
        }
	}
}