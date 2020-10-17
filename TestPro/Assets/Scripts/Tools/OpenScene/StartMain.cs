using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StartMainScene
{
    public static string scenePath = "Assets/Scenes/Main.unity";

    [MenuItem("DevTool/Open/MainScene", false, 1)]
    public static void OpenMainScene ()
    {
        if (!UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("Main"))
        {
            EditorSceneManager.OpenScene(scenePath);
        }
        EditorApplication.ExecuteMenuItem("Edit/Play");

        // EditorSceneManager.OpenScene(scenePath);
        // EditorApplication.isPlaying = true;
    }

    // 第二个参数如果为真，则下面的这个函数会被认定为一个验证是否满足条件的函数，只有返回true的时候，同名的菜单才能进行调用
    [MenuItem("DevTool/Open/MainScene", true, 1)]
    public static bool ValidStartMainScene ()
    {
        return !Application.isPlaying;
    }
}