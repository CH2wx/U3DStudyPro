using Assets.Scripts.Tools.OpenScene;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class MathSurface
{
    public static string scenePath = "Assets/Res/Tools/OpenScene/Base/MathSurface/scene.unity";

    [MenuItem("DevTool/Open/Scene/Base/MathSurface")]
    public static void OpenMathSurfaceScene()
    {
        SceneTools.CheckScenesInBuild(scenePath);

        if (EditorApplication.isPlaying)
        {
            SceneManager.LoadScene(scenePath);
        }
        else
        {
            EditorSceneManager.OpenScene(scenePath);
            EditorApplication.isPlaying = true;
        }
    }
}