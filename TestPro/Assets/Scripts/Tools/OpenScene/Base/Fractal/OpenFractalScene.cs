using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class OpenFractalScene{

	public static string scenePath = "Assets/Res/Tools/OpenScene/Base/Fractal/Fractal.unity";

    [MenuItem("DevTool/Open/Scene/Base/Fractal")]
    public static void OpenScene ()
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
