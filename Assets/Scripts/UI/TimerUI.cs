using System;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    #region Field
    [SerializeField] private Timer _timer;
    [SerializeField] private TMP_Text _timeText;
    #endregion

    #region Event Method
    private void Awake()
    {
        if (_timer == null)
            _timer = GameObject.FindFirstObjectByType<Timer>();
        if (_timeText == null)
            _timeText = transform.GetChild(0).GetComponent<TMP_Text>();
    }
    private void LateUpdate()
    {
        _timeText.text = $"{TimeSpan.FromSeconds(_timer.ElapsedTime).ToString(@"mm\:ss")}";
    }
    #endregion
}
