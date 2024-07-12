using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HomeSceneManager : MonoBehaviour
{
    #region Field
    [SerializeField] private TMP_Text _bestRecordText;
    #endregion

    #region Event Method
    private void Awake()
    {
        if(_bestRecordText == null)
            _bestRecordText = GameObject.Find("Text_BestRecord").GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        SetBestRecord();
    }
    private void Update()
    {
        if (GameManager.Instance.IsExitPanelActivated == true)
            return;

        if (Keyboard.current.anyKey.wasPressedThisFrame == true)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame == true)
                return;

            GameManager.Instance.GameLevel = 1;
            SceneManager.LoadSceneAsync((int)SceneRepository.SceneName.GameScene, LoadSceneMode.Single);
        }
    }
    #endregion

    #region Method
    private void SetBestRecord()
    {
        _bestRecordText.text =  $"Best\n{TimeSpan.FromSeconds(GameManager.Instance.BestRecord).ToString(@"mm\:ss")}";
    }
    #endregion
}