using UnityEngine;
using Assets.Scripts.Tools.OpenScene.Base.Math;

namespace Assets.Scripts.Tools.OpenScene.Base.Math
{
    public class Math2DFunction
    {

        public static float F(float x, float z = 0, MathManager.Type type = MathManager.Type.Sin)
        {
            float result;
            switch(type)
            {
                case MathManager.Type.Pow2:
                    result = x * x;
                    break;
                case MathManager.Type.Pow3:
                    result = x * x * x;
                    break;
                case MathManager.Type.Sin:
                    result = Mathf.Sin(MathManager.PI * x + Time.time);
                    break;
                case MathManager.Type.Sine:
                    result = SineFunction(x, Time.time);
                    break;
                case MathManager.Type.MultiSine:
                    result = MultiSineFunction(x, Time.time);
                    break;
                case MathManager.Type.Sine2D:
                    result = Sine2DFunction(x, z, Time.time);
                    break;
                case MathManager.Type.MultiSine2D:
                    result = MultiSine2DFunction(x, z, Time.time);
                    break;
                case MathManager.Type.MultiSine2D_1:
                    result = MultiSine2D_1Function(x, z, Time.time);
                    break;
                case MathManager.Type.Ripple:
                    result = Ripple(x, z, Time.time);
                    break;
                default:
                    result = x;
                    break;
            }
            return result;
        }

        /// <summary> f(x，t) = sin(π(x+t)) </summary>
        private static float SineFunction(float x, float time)
        {
            return Mathf.Sin(MathManager.PI * (x + time));
        }

        private static float MultiSineFunction(float x, float time)
        {
            // 要增加正弦波的复杂性，最简单的方法是增加另一个频率加倍的正弦波
            float y = Mathf.Sin(MathManager.PI * (x + time)) + Mathf.Sin(2f * MathManager.PI * (x + 2f * time)) / 2.0f;
            // 上式的值范围[-1.5, 1.5]，归一化
            y *= 2f / 3f;
            return y;
        }

        private static float Sine2DFunction(float x, float z, float time)
        {
            float y = Mathf.Sin(MathManager.PI * (x + z + time));

            return y;
        }

        private static float MultiSine2DFunction(float x, float z, float time)
        {
            float y = Mathf.Sin(MathManager.PI * (x + time)) + Mathf.Sin(MathManager.PI * (z + time));
            y *= 0.5f;
            return y;
        }

        /// <summary>
        /// f(x,z,t) = M + S_x + S_z
        /// M（主波）： M = sin (pi * (x + z + t/2))
        /// S_x（沿x的法线波）：S_x = sin(pi * (x + t))
        /// S_z（沿z的法线波）：S_z = sin(pi * (z + 2*t)) * 0.5f
        /// </summary>
        private static float MultiSine2D_1Function (float x, float z, float time)
        {
            float y = 4 * Mathf.Sin(MathManager.PI * (x + z + time * 0.5f));
            y += Mathf.Sin(MathManager.PI * (x + time));
            y += Mathf.Sin(MathManager.PI * (z + 2 * time)) * 0.5f;
            // 归一化
            y *= 1f / 5.5f;
            return y;
        }

        private static float Ripple(float x, float z, float t)
        {
            // 通过勾股定理来获得一个圆形图案（圆锥， [0, 根号2]）
            float d = Mathf.Sqrt(x * x + z * z);
            // 产生涟漪，并提高产生涟漪的频率（提升4倍）
            // 涟漪产生公式(d=距离)：f(x,z,t) = sin(4 * pi * d)
            // 动画效果：波纹向外移动，减t；波纹向内移动，加t（原理：sin的值往负无穷的时候，先向下再向上，得到的波纹也是先下后上）
            float y = Mathf.Sin(MathManager.PI * (4f * d - t));
            // 由于波动极端，减小振幅(防止d为0，需要一个定值)
            y /= 1f + 10f * d;
            return y;
        }
    }
}
