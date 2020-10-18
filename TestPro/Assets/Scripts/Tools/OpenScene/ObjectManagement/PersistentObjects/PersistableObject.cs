using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.PersistentObjects
{
    [DisallowMultipleComponent] //不允许附加多个此类组件
    public class PersistableObject : MonoBehaviour
    {
        public virtual void Save(GameDataWriter writer)
        {
            writer.Write(this.transform.localPosition);
            writer.Write(this.transform.localScale);
            writer.Write(this.transform.localRotation);
        }

        public virtual void Load(GameDataReader reader)
        {
            this.transform.localPosition = reader.ReadVector3();
            this.transform.localScale = reader.ReadVector3();
            this.transform.localRotation = reader.ReadQuaternion();
        }
    }
}
