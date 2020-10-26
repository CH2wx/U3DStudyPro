using System.IO;
using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects
{
    public class GameDataWriter
    {
        private BinaryWriter writer;

        public GameDataWriter(BinaryWriter writer)
        {
            this.writer = writer;
        }

        public void Write(float value)
        {
            writer.Write(value);
        }

        public void Write(int value)
        {
            writer.Write(value);
        }

        public void Write(Vector3 value)
        {
            writer.Write(value.x);
            writer.Write(value.y);
            writer.Write(value.z);
        }

        public void Write(Quaternion value)
        {
            writer.Write(value.x);
            writer.Write(value.y);
            writer.Write(value.z);
            writer.Write(value.w);
        }

        internal void Write(Color color)
        {
            writer.Write(color.r);
            writer.Write(color.b);
            writer.Write(color.g);
            writer.Write(color.a);
        }

        public void Write(Random.State value)
        {
            Debug.Log(JsonUtility.ToJson(value));
            writer.Write(JsonUtility.ToJson(value));
        }
    }
}
