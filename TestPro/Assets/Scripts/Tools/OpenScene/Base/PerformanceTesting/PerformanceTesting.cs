using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PerformanceTesting : MonoBehaviour {

    private static readonly string scenePath = "Assets/Res/Tools/OpenScene/Base/PerformanceTesting/scene.unity";

    [MenuItem("DevTool/Open/Scene/Base/PerformanceTesting")]
    public static void OpenPerfomanceTesting()
    {
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
