using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.Base.Math
{
    public class MathManager
    {
        public const float PI = Mathf.PI;

        public enum FunType
        {
            Fun2D, Fun3D
        };

        public enum Type
        {
            Pow2, Pow3, Other,
            Sin, Sine, MultiSine, Sine2D, MultiSine2D, MultiSine2D_1, Ripple, Cylinder, Sphere, Torus
        };

        /// <summary> 是否可以运动 </summary>
        public static bool IsAthleticType(Type type)
        {
            bool isAthletic = false;

            switch (type)
            {
                case Type.Sin:
                case Type.Sine:
                case Type.Sine2D:
                case Type.MultiSine:
                case Type.MultiSine2D:
                case Type.MultiSine2D_1:
                case Type.Ripple:
                case Type.Cylinder:
                case Type.Sphere:
                case Type.Torus:
                    isAthletic = true;
                    break;
            }
            return isAthletic;
        }
    }
}
