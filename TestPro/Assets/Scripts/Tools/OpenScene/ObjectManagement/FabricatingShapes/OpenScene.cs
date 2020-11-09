using UnityEditor;

namespace Assets.Scripts.Tools.OpenScene.ObjectManagement.FabricatingShapes
{
    class OpenScene
    {
        public static string scenePath = "Assets/Res/Tools/OpenScene/ObjectManagement/FabricatingShapes/Scenes/MainScene.unity";

        [MenuItem("DevTool/Open/Scene/ObjectManagement/FabricatingShapes")]
        public static void Open()
        {
            SceneTools.OpenScene(scenePath);
        }
    }
}
