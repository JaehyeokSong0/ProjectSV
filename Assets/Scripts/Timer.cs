using System.Collections;
using UnityEngine;

/// <summary>
/// Manages game time
/// </summary>
public class Timer : MonoBehaviour
{
    // TEST CODE
    private void Start()
    {
        StartCheckTime();
        EventManager.Instance.OnPlayerDead?.AddListener(StopCheckTime);
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

    #endregion
}
