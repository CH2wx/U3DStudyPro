using Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PersistentStorage : MonoBehaviour {
    public string savePath;
    public string pathName = "Res\\Tools\\OpenScene\\ObjectManagement\\PersistentObjects\\SaveFile";

    private void Awake()
    {
        savePath = Path.Combine(Application.dataPath, pathName);
    }

    public void Save(PersistableObject o)
    {
        using (var writer = new BinaryWriter(File.Open(savePath, FileMode.OpenOrCreate)))
        {
            o.Save(new GameDataWriter(writer));
        }
    }

    public void Load(PersistableObject o)
    {
        using (var reader = new BinaryReader(File.Open(savePath, FileMode.OpenOrCreate)))
        {
            o.Load(new GameDataReader(reader));
        }
    }
}
