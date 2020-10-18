using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects
{
    class OpenFile
    {
        public string pathName = "Res\\Tools\\OpenScene\\ObjectManagement\\PersistentObjects\\SaveFile";
        public string filePath;

        private BinaryWriter binary;

        private readonly FileStream file;
        [System.Serializable]
        public struct FileData
        {
            public Vector3 position;
            public Vector3 scale;
            public Quaternion quaternion;
        }

        public OpenFile()
        {
            filePath = Path.Combine(Application.dataPath, pathName);
            //file = File.Open(filePath, FileMode.OpenOrCreate);
        }

        public void Save(List<Transform> list)
        {
            using (var writer = new BinaryWriter(File.Open(filePath, FileMode.OpenOrCreate)))
            {
                writer.Write(list.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    writer.Write(list[i].localPosition.x);
                    writer.Write(list[i].localPosition.y);
                    writer.Write(list[i].localPosition.z);
                    writer.Write(list[i].localScale.x);
                    writer.Write(list[i].localScale.y);
                    writer.Write(list[i].localScale.z);
                    writer.Write(list[i].localRotation.x);
                    writer.Write(list[i].localRotation.y);
                    writer.Write(list[i].localRotation.z);
                    writer.Write(list[i].localRotation.w);
                }
            }
        }

        public FileData[] Load()
        {
            FileData[] datas = new FileData[0];
            using (var reader = new BinaryReader(File.Open(filePath, FileMode.OpenOrCreate)))
            {
                int count = reader.ReadInt32();
                datas = new FileData[count];

                Vector3 vct = new Vector3();
                Quaternion quaternion = new Quaternion();
                for (int i = 0; i < count; i++)
                {
                    // 读取四个字节的浮点型数字
                    vct.x = reader.ReadSingle();
                    vct.y = reader.ReadSingle();
                    vct.z = reader.ReadSingle();
                    datas[i].position = vct;

                    vct.x = reader.ReadSingle();
                    vct.y = reader.ReadSingle();
                    vct.z = reader.ReadSingle();
                    datas[i].scale = vct;

                    quaternion.x = reader.ReadSingle();
                    quaternion.y = reader.ReadSingle();
                    quaternion.z = reader.ReadSingle();
                    quaternion.w = reader.ReadSingle();
                    datas[i].quaternion = quaternion;
                }
            }
            return datas;
        }
    }
}
