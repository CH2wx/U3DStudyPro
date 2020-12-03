using Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects;
using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes.Modular_Functionality
{
    class MovementShapeBehavior : ShapeBehavior
    {
        public Vector3 Velocity { get; set; }

        public override ShapeBehaviorType BehaciorType
        {
            get
            {
                return ShapeBehaviorType.Movement;
            }
        }

        public override void GameUpdate(Shape shape)
        {
            shape.transform.position += Velocity * Time.deltaTime;
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(Velocity);
        }

        public override void Load(GameDataReader reader)
        {
            Velocity = reader.ReadVector3();
        }
    }
}
