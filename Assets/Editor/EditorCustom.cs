using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
public class EditorCustom : MonoBehaviour
{
    [MenuItem("CustomTools/EditorCustom/Toggle GameView %g")]
    public static void ToggleGameView()
    {
        if (EditorApplication.isPlaying)
            EditorWindow.focusedWindow.maximized = !EditorWindow.focusedWindow.maximized;
    }
}
#endif