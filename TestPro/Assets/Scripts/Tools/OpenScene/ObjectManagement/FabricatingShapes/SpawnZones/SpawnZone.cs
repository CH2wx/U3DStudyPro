using Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes.Modular_Functionality;
using Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects;
using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes.SpawnZones
{
    public enum SpawnMovementDirection
    {
        Forward, Upward, Outward, Random
    }

    public enum SpawnZoneType
    {
        Random, Sequential
    }    

    [System.Serializable]
    public struct SpawnConfiguration
    {
        public SpawnMovementDirection movementDirection;
        public FloatRange speed;
        public FloatRange scale;
        public FloatRange angularSpeed;

        public bool uniformColor;               // 是否使用统一的颜色
        public ColorRangeHSV color;

        public ShapeFactory[] factories;

        public void SetSpeed(float min, float max)
        {
            speed.SetRange(min, max);
        }

        public void SetScale(float min, float max)
        {
            scale.SetRange(min, max);
        }

        public void SetVelocity(float min, float max)
        {
            angularSpeed.SetRange(min, max);
        }

        public void SetColor(float hueMin, float hueMax,
                             float saturationMin, float saturationMax,
                             float valueMin, float valueMax,
                             float alphaMin, float alphaMax)
        {
            color.SetRange(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax, alphaMin, alphaMax);
        }
    }

    public abstract class SpawnZone : PersistableObject
    {
        [SerializeField]
        private SpawnConfiguration spawnConfig;

        public abstract Vector3 SpawnPoint { get; }

        private void Start()
        {
            spawnConfig.SetSpeed(0.1f, 2.0f);
            spawnConfig.SetScale(0.1f, 1.0f);
            spawnConfig.SetVelocity(1f, 90f);
            spawnConfig.SetColor(0f, 1f, 0.5f, 1f, 0.25f, 1f, 1f, 1f);
        }

        //public virtual void ConfigureSpawn(Shape shape)
        public virtual Shape SpawnShape()
        {
            //Shape instance = shape;
            int factoryIndex = Random.Range(0, spawnConfig.factories.Length);
            Shape instance = spawnConfig.factories[factoryIndex].GetRandom();
            instance.transform.localPosition = SpawnPoint;
            instance.transform.localRotation = Random.rotation;
            instance.transform.localScale = Vector3.one * spawnConfig.scale.RandomValueInRange;

            float angularSpeed = spawnConfig.angularSpeed.RandomValueInRange;
            if (angularSpeed != 0)
            {
                var rotation = instance.AddBehavior<RotationShapeBehavior>();
                rotation.AngularVelocity = Random.onUnitSphere * angularSpeed;
            }

            float speed = spawnConfig.speed.RandomValueInRange;
            if (speed != 0)
            {
                Vector3 direction;
                switch (spawnConfig.movementDirection)
                {
                    case SpawnMovementDirection.Forward:
                        direction = transform.forward;
                        break;
                    case SpawnMovementDirection.Upward:
                        direction = transform.up;
                        break;
                    case SpawnMovementDirection.Outward:
                        direction = (instance.transform.localPosition - transform.position).normalized;
                        break;
                    case SpawnMovementDirection.Random:
                    default:
                        direction = Random.onUnitSphere.normalized;
                        break;
                }
                var movement = instance.AddBehavior<MovementShapeBehavior>();
                movement.Velocity = direction * speed;
            }

            // hue 色调：0-360°；saturation 饱和度：0-100%； value 亮度：0-100%； alpha 透明度：0-100%
            //instance.SetColor(Random.ColorHSV(
            //                            hueMin: 0f, hueMax: 1f,
            //                            saturationMin: 0.5f, saturationMax: 1f,
            //                            valueMin: 0.25f, valueMax: 1f,
            //                            alphaMin: 1f, alphaMax: 1f));
            if (spawnConfig.uniformColor)
            {
                instance.SetColor(spawnConfig.color.RandomInRange);
            }
            else
            {
                for (int i = 0; i < instance.ColorCount; i++)
                {
                    instance.SetColor(spawnConfig.color.RandomInRange, i);
                }
            }

            return instance;
        }
    }
}
