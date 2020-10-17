using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.Base.Math
{
    public class Math3DFunction
    {
        public const float PI = MathManager.PI;

        public static Vector3 F(float u, float v, MathManager.Type type)
        {
            Vector3 vct = Vector3.zero;
            switch (type)
            {
                case MathManager.Type.Torus:
                    vct = Torus(u, v, Time.time);
                    break;
                case MathManager.Type.Sphere:
                    vct = Sphere(u, v, Time.time);
                    break;
                case MathManager.Type.Cylinder:
                    vct = Cylinder(u, v, Time.time);
                    break;
                case MathManager.Type.Ripple:
                    vct = Ripple(u, v, Time.time);
                    break;
                case MathManager.Type.MultiSine2D_1:
                    vct = MultiSine2D_1Function(u, v, Time.time);
                    break;
                case MathManager.Type.MultiSine2D:
                    vct = MultiSine2DFunction(u, v, Time.time);
                    break;
                case MathManager.Type.MultiSine:
                    vct = MultiSineFunction(u, v, Time.time);
                    break;
                case MathManager.Type.Sine:
                    vct = SineFunction(u, v, Time.time);
                    break;
                case MathManager.Type.Sine2D:
                    vct = Sine2DFunction(u, v, Time.time);
                    break;
            }
            //Debug.LogFormat("{0}\t{1}\t{2}", u, v, vct);
            return vct;
        }

        private static Vector3 MultiSine2D_1Function(float u, float v, float t)
        {
            return MultiSine2DFunction(u, v, t);
        }

        private static Vector3 Torus (float u, float v, float t)
        {
            float r1 = 0.65f + Mathf.Sin(PI * (6f * u + t)) * 0.1f;
            float r2 = 0.2f + Mathf.Sin(PI * (4f * v + t)) * 0.1f;
            float s = r2 *  Mathf.Cos(PI * v) + r1;
            Vector3 p = new Vector3()
            {
                x = s * Mathf.Sin(PI * u),
                y = r2 * Mathf.Sin(PI * v),
                z = s * Mathf.Cos(PI * u)
            };
            return p;
        }

        private static Vector3 Sphere (float u, float v, float t)
        {
            float r = 0.8f + Mathf.Sin(PI * (6f * u + t)) * 0.1f;
            r += Mathf.Sin(PI * (4f * v + t)) * 0.1f;
            float s = r * Mathf.Cos(PI * 0.5f * v);
            Vector3 p = new Vector3(0, v, 0)
            {
                x = s * Mathf.Sin(PI * u),
                y = r * Mathf.Sin(PI * 0.5f * v),
                z = s * Mathf.Cos(PI * u)
            };
            return p;
        }

        private static Vector3 Cylinder (float u, float v, float t)
        {
            float r = 1f + Mathf.Sin(PI * (6f *u + 2f * v + t)) * 0.2f;
            //r = 1f;
            Vector3 p = new Vector3(u, 0, v)
            {
                x = r * Mathf.Sin(PI * u),
                y = v,
                z = r * Mathf.Cos(PI * u)
            };
            return p;
        }

        private static Vector3 Ripple (float u, float v, float t)
        {
            float d = Mathf.Sqrt(u * u + v * v);
            Vector3 p = new Vector3(u, 0, v)
            {
                y = Mathf.Sin(PI * (4f * d - t))
            };
            p.y /= 1f + 10f * d;
            return p;
        }

        private static Vector3 MultiSine2DFunction(float u, float v, float t)
        {
            Vector3 p = new Vector3(u, 0, v);
            p.y = 4f * Mathf.Sin(PI * (u + v + t * 0.5f));
            p.y += Mathf.Sin(PI * (u + t));
            p.y += Mathf.Sin(2f * PI * (v + 2f * t)) * 0.5f;
            p.y *= 1f / 5.5f;
            return p;
        }

        private static Vector3 MultiSineFunction(float u, float v, float t)
        {
            Vector3 p = new Vector3(u, 0, v);
            p.y = Mathf.Sin(PI * (u + t));
            p.y += Mathf.Sin(2f * PI * (u + 2f * t)) * 0.5f;
            p.y *= 2f / 3f;
            return p;
        }

        private static Vector3 Sine2DFunction(float u, float v, float t)
        {
            Vector3 p = new Vector3(u, 0, v);
            p.y = Mathf.Sin(PI * (u + t));
            p.y += Mathf.Sin(PI * (v + t));
            p.y *= 0.5f;
            return p;
        }

        private static Vector3 SineFunction(float u, float v, float t)
        {
            Vector3 p = new Vector3(u, Mathf.Sin(PI * (u + t)), v);
            return p;
        }
    }
}
