using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    private void OnEnable()
    {
        ShowGameOverUI();
    }
    private void Start()
    {
        EventManager.Instance.OnPlayerDead?.AddListener(ShowGameOverUI);
        HideGameOverUI();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // Enter
        {
            HideGameOverUI();
            SceneManager.LoadScene((int)SceneRepository.SceneName.HomeScene, LoadSceneMode.Single);
        }
    }

    public void ShowGameOverUI()
    {
        gameObject.SetActive(true);
    }
    public void HideGameOverUI()
    {
        gameObject.SetActive(false);
    }
}