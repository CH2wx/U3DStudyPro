using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenSceneForUI
{
    public static string scenePath = "Assets/Res/Tools/OpenScene/Base/UI/UIScene.unity";

    [MenuItem("DevTool/Open/Scene/Base/UI")]
    public static void OpenUIScene ()
    {
        EditorApplication.isPlaying = false;
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