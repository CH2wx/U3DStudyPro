using Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PersistentStorage : MonoBehaviour
{
    public string storageParentPath = "Res\\Tools\\OpenScene\\ObjectManagement";
    public string savePath;
    public string filePath = "\\PersistentObjects\\SaveFile";

    private void Awake()
    {
        savePath = Path.Combine(Application.dataPath, storageParentPath);
        filePath = savePath + filePath;
    }

    public void ChangePath(string path)
    {
        filePath = savePath + path;
    }

    public void Save(PersistableObject o, int version = 1)
    {
        using (var writer = new BinaryWriter(File.Open(filePath, FileMode.OpenOrCreate)))
        {
            writer.Write(version);
            o.Save(new GameDataWriter(writer));
        }
    }

    public void Load(PersistableObject o)
    {
        //using (var reader = new BinaryReader(File.Open(filePath, FileMode.OpenOrCreate)))
        //{
        //    o.Load(new GameDataReader(reader, reader.ReadInt32()));
        //}

        byte[] bytes = File.ReadAllBytes(filePath);
        var reader = new BinaryReader(new MemoryStream(bytes));
        o.Load(new GameDataReader(reader, reader.ReadInt32()));
    }
}
