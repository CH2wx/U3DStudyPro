using Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects;
using UnityEngine;

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
        private MeshRenderer meshRenderer;
        private Color color;

        public Vector3 Velocity { get; set; }
        public Vector3 AngularVelocity { get; set; }

        void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void GameUpdate()
        {
            transform.Rotate(AngularVelocity * Time.deltaTime);
            transform.localPosition += Velocity * Time.deltaTime;
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

        public override void Save (GameDataWriter writer)
        {
            writer.Write(ShapeId);
            writer.Write(MaterialId);
            base.Save(writer);
            writer.Write(color);
            writer.Write(AngularVelocity);
            writer.Write(Velocity);
        }

        public override void Load (GameDataReader reader)
        {
            base.Load(reader);
            SetColor(reader.Version > 0 ? reader.ReadColor() : Color.white);
            AngularVelocity = reader.Version >= 4 ? reader.ReadVector3() : Vector3.zero;
            Velocity = reader.Version >= 4 ? reader.ReadVector3() : Vector3.zero;
        }

        public void SetMaterial(Material material, int materialId)
        {
            meshRenderer.material = material;
            MaterialId = materialId;
        }

        public void SetColor (Color color)
        {
            this.color = color;
            if (shaderPropertyBlock == null)
            {
                shaderPropertyBlock = new MaterialPropertyBlock();
            }
            shaderPropertyBlock.SetColor(colorPropertyId, color);
            meshRenderer.SetPropertyBlock(shaderPropertyBlock);
        }
    }
}
