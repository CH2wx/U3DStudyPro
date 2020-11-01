using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects
{
    class RotatingObject : PersistableObject
    {
        [SerializeField]
        private Vector3 angularVelocity;        //角速度

        [SerializeField, Range(1.0f, 10.0f)]
        private float rotateSpeed = 1.0f;

        private void FixedUpdate()
        {
            transform.Rotate(angularVelocity * rotateSpeed * Time.deltaTime);
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(angularVelocity);
            writer.Write(rotateSpeed);
        }

        public override void Load(GameDataReader reader)
        {
            angularVelocity = reader.ReadVector3();
            rotateSpeed = reader.ReadFloat();
        }
    }
}
