using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HomeSceneManager : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame == true)
            SceneManager.LoadSceneAsync((int)SceneRepository.SceneName.GameScene, LoadSceneMode.Single);
    }
}