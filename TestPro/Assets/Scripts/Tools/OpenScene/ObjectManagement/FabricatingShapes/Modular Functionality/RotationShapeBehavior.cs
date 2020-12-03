using Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects;
using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes.Modular_Functionality
{
    class RotationShapeBehavior : ShapeBehavior
    {
        public Vector3 AngularVelocity { get; set; }

        public override ShapeBehaviorType BehaciorType
        {
            get
            {
                return ShapeBehaviorType.Rotation;
            }
        }

        public override void GameUpdate(Shape shape)
        {
            shape.transform.Rotate(AngularVelocity * Time.deltaTime);
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(AngularVelocity);
        }

        public override void Load(GameDataReader reader)
        {
            AngularVelocity = reader.ReadVector3();
        }
    }
}
