using System.IO;
using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects
{
    public class GameDataReader
    {
        public int Version { get; }
        private BinaryReader reader;

        public GameDataReader(BinaryReader reader, int version)
        {
            this.reader = reader;
            this.Version = version;
        }

        public float ReadFloat()
        {
            return reader.ReadSingle();
        }

        public int ReadInt()
        {
            return reader.ReadInt32();
        }

        public Vector3 ReadVector3()
        {
            Vector3 vct;
            vct.x = reader.ReadSingle();
            vct.y = reader.ReadSingle();
            vct.z = reader.ReadSingle();
            return vct;
        }

        public Quaternion ReadQuaternion()
        {
            Quaternion quaternion;
            quaternion.x = reader.ReadSingle();
            quaternion.y = reader.ReadSingle();
            quaternion.z = reader.ReadSingle();
            quaternion.w= reader.ReadSingle();
            return quaternion;
        }

        public Color ReadColor()
        {
            Color color;
            color.r = reader.ReadSingle();
            color.b = reader.ReadSingle();
            color.g = reader.ReadSingle();
            color.a = reader.ReadSingle();
            return color;
        }
    }
}
