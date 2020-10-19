using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes
{
    class OpenScene
    {
        public static string scenePath = "Assets/Res/Tools/OpenScene/ObjectManagement/FabricatingShapes/scene.unity";

        [MenuItem("DevTool/Open/Scene/ObjectManagement/FabricatingShapes")]
        public static void Open()
        {
            SceneTools.OpenScene(scenePath);
        }
    }
}
