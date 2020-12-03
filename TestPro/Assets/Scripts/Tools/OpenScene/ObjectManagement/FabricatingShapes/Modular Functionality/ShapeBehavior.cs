using Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects;
using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes.Modular_Functionality
{
    public abstract class ShapeBehavior : MonoBehaviour
    {
        public enum ShapeBehaviorType {
            Movement, Rotation
        }

        public abstract ShapeBehaviorType BehaciorType { get; }

        public abstract void GameUpdate(Shape shape);

        public abstract void Save(GameDataWriter writer);

        public abstract void Load(GameDataReader reader);
    }
}
