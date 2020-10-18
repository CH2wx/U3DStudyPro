using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects
{
    public class GameDataReader
    {
        private BinaryReader reader;

        public GameDataReader(BinaryReader reader)
        {
            this.reader = reader;
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
    }
}
