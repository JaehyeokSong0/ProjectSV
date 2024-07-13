using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
public class SceneLoader : MonoBehaviour
{
    [MenuItem("CustomTools/SceneLoader/Start from Scene 0 %h")]
    public static void PlaySceneFromZero()
    {
        var path_sceneZero = EditorBuildSettings.scenes[0].path;
        var sceneZeroAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path_sceneZero);
        EditorSceneManager.playModeStartScene = sceneZeroAsset;
        UnityEditor.EditorApplication.isPlaying = true;
    }

    [MenuItem("CustomTools/SceneLoader/Start from Current Scene %j")]
    public static void PlaySceneFromCurrent()
    {
        EditorSceneManager.playModeStartScene = null;
        UnityEditor.EditorApplication.isPlaying = true;
    }
}
#endif
