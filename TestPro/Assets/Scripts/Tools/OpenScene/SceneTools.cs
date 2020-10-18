using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Tools.OpenScene
{
    public class SceneTools
    {
        /// <summary> 检测场景是否添加进build settings，如果没有，自动添加场景</summary>
        public static void CheckScenesInBuild(string path)
        {
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            // 检查是否存在该场景在build settings里
            bool isExist = false;
            foreach (var s in scenes)
            {
                // Debug.LogFormat("{0}\t{1}\t{2}", s.path,s.enabled,s.guid);
                if (s.path == path)
                {
                    isExist = true;
                }
            }
            // 不存在的话，添加指定路径的场景
            if (!isExist)
            {
                List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>(scenes);
                editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(path, true));
                EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
            }
        }

        /// <summary> 加载工程中的资源，必须是以Assets为起点的路径 </summary>
        public static Object Load(string path)
        {
            return AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
        }

        public static void OpenScene(string scenePath)
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
}
