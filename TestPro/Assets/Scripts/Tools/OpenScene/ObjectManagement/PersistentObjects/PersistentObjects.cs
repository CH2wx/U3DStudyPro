using Assets.Scripts.Tools.OpenScene;
using UnityEditor;

public class PersistentObjects{

    public static string scenePath = "Assets/Res/Tools/OpenScene/ObjectManagement/PersistentObjects/scene.unity";

    [MenuItem("DevTool/Open/Scene/ObjectManagement/PersistentObjects")]
    public static void OpenScene()
    {
        SceneTools.OpenScene(scenePath);
    }
}
