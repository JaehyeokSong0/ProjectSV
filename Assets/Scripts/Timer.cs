using System.Collections;
using UnityEngine;

/// <summary>
/// Manages game time
/// </summary>
public class Timer : MonoBehaviour
{
    private void Start()
    {
        StartCheckTime();
        EventManager.Instance.OnPlayerDead?.AddListener(StopCheckTime);
        EventManager.Instance.OnPlayerDead?.AddListener(CheckTimeRecord);
    }
    #region Property
    public float ElapsedTime => _elapsedTime;
    #endregion

    #region Field
    private float _elapsedTime = 0f;
    private Coroutine _checkTimeCoroutine = null;
    #endregion

    #region Method
    public void ResetCheckTime()
    {
        _elapsedTime = 0f;
    }

    public void StartCheckTime()
    {
        _checkTimeCoroutine = StartCoroutine(C_StartCheckTime());
    }
    private IEnumerator C_StartCheckTime()
    {
        while (true)
        {
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void StopCheckTime()
    {
        if (_checkTimeCoroutine != null)
            StopCoroutine(_checkTimeCoroutine);
        else
            Debug.LogError("Cannot stop checkTimeCoroutine");
    }

    public void CheckTimeRecord()
    {
        GameManager.Instance.BestRecord = _elapsedTime;
    }
    public void PauseTime()
    {
        Time.timeScale = 0f;
    }

    public void ResumeTime()
    {
        Time.timeScale = 1f;
    }

    #endregion
}
