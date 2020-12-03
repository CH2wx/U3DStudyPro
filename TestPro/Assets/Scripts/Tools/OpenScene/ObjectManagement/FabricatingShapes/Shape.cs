using Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes.Modular_Functionality;
using Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes.Modular_Functionality.ShapeBehavior;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes
{
    public class Shape : PersistableObject
    {
        // 通过调用着色器找到标识符
        private static int colorPropertyId = Shader.PropertyToID("_Color");
        // 当我们使用属性块时，可以使用GPU实例化在一个绘图调用中组合使用相同材质的形状，使得每个形状都通过材质属性块，来使用各自的颜色属性
        private static MaterialPropertyBlock shaderPropertyBlock;

        private int shapeId = int.MinValue;
        private int materialId = int.MinValue;
        //private MeshRenderer meshRenderer;
        [SerializeField]
        private MeshRenderer[] meshRenderers;
        //private Color color;
        private Color[] colors;

        public Vector3 Velocity { get; set; }
        public Vector3 AngularVelocity { get; set; }

        private ShapeFactory originalFactory;

        private List<ShapeBehavior> behaviorList;

        void Awake()
        {
            //meshRenderer = GetComponent<MeshRenderer>();
            colors = new Color[meshRenderers.Length];
            behaviorList = new List<ShapeBehavior>();
        }

        public void GameUpdate()
        {
            //transform.Rotate(AngularVelocity * Time.deltaTime);
            //transform.localPosition += Velocity * Time.deltaTime;
            for (int i = 0; i < behaviorList.Count; i++)
            {
                behaviorList[i].GameUpdate(this);
            }
        }

        public int ShapeId
        {
            get
            {
                return shapeId;
            }

            set
            {
                if (shapeId == int.MinValue && value != int.MinValue)
                {
                    shapeId = value;
                }
                else
                {
                    Debug.LogError("Not allowed to change shapeId!");
                }
            }
        }

        public int MaterialId
        {
            get
            {
                return materialId;
            }

            private set
            {
                if (materialId == int.MinValue && value != int.MinValue)
                {
                    materialId = value;
                }
                else
                {
                    Debug.LogError("Not allowed to change materialId!");
                }
            }
        }

        public override void Save(GameDataWriter writer)
        {
            base.Save(writer);
            //writer.Write(color);
            writer.Write(colors.Length);
            for (int i = 0; i < colors.Length; i++)
            {
                writer.Write(colors[i]);
            }
            writer.Write(behaviorList.Count);
            for (int i = 0; i < behaviorList.Count; i++)
            {
                behaviorList[i].Save(writer);
            }
            //writer.Write(AngularVelocity);
            //writer.Write(Velocity);
        }

        public override void Load(GameDataReader reader)
        {
            base.Load(reader);
            if (reader.Version >= 5)
            {
                LoadColors(reader);
            }
            else
            {
                SetColor(reader.Version > 0 ? reader.ReadColor() : Color.white);
            }
            //AngularVelocity = reader.Version >= 4 ? reader.ReadVector3() : Vector3.zero;
            //Velocity = reader.Version >= 4 ? reader.ReadVector3() : Vector3.zero;
            int count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                AddBehavior((ShapeBehaviorType)reader.ReadInt()).Load(reader);
            }
        }

        public void SetMaterial(Material material, int materialId)
        {
            //meshRenderer.material = material;
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].material = material;
            }
            MaterialId = materialId;
        }

        private void LoadColors(GameDataReader reader)
        {
            int count = reader.ReadInt();
            int i = 0;
            count = count <= colors.Length ? count : colors.Length;
            for (; i < count; i++)
            {
                SetColor(reader.ReadColor(), i);
            }

            if (count > colors.Length)
            {
                for (; i < colors.Length; i++)
                {
                    reader.ReadColor();
                }
            }
            else if (count < colors.Length)
            {
                for (; i < colors.Length; i++)
                {
                    SetColor(Color.white, i);
                }
            }
        }

        public void SetColor(Color color)
        {
            //this.color = color;
            if (shaderPropertyBlock == null)
            {
                shaderPropertyBlock = new MaterialPropertyBlock();
            }
            shaderPropertyBlock.SetColor(colorPropertyId, color);
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                colors[i] = color;
                meshRenderers[i].SetPropertyBlock(shaderPropertyBlock);
            }
            //meshRenderer.SetPropertyBlock(shaderPropertyBlock);
        }

        public void SetColor(Color color, int index)
        {
            if (shaderPropertyBlock == null)
            {
                shaderPropertyBlock = new MaterialPropertyBlock();
            }
            shaderPropertyBlock.SetColor(colorPropertyId, color);
            colors[index] = color;
            meshRenderers[index].SetPropertyBlock(shaderPropertyBlock);
        }

        public int ColorCount
        {
            get
            {
                return colors.Length;
            }
        }

        public ShapeFactory OriginalFactory
        {
            get
            {
                return originalFactory;
            }
            set
            {
                if (originalFactory == null)
                {
                    originalFactory = value;
                }
                else
                {
                    Debug.LogError("Not allowed to change origin factory.");
                }
            }
        }

        public void Reclaim()
        {
            for (int i = 0; i < behaviorList.Count; i++)
            {
                Destroy(behaviorList[i]);
            }
            OriginalFactory.Reclaim(this);
        }

        public T AddBehavior<T>() where T : ShapeBehavior
        {
            T behavior = gameObject.AddComponent<T>();
            behaviorList.Add(behavior);
            return behavior;
        }

        public ShapeBehavior AddBehavior(ShapeBehaviorType type)
        {
            switch(type)
            {
                case ShapeBehaviorType.Movement:
                    return AddBehavior< MovementShapeBehavior>();
                case ShapeBehaviorType.Rotation:
                    return AddBehavior<RotationShapeBehavior>();
            }
            return null;
        }
    }
}
