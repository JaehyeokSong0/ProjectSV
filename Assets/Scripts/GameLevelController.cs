using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages game level (Not player level)
/// </summary>
public class GameLevelController : MonoBehaviour
{
    #region Field
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private WaitForSeconds _levelUpWait = new WaitForSeconds(20f);
    #endregion

    #region Event Method
    private void Awake()
    {
        if (_enemySpawner == null)
            _enemySpawner = GameObject.Find("Enemy Spawner").GetComponent<EnemySpawner>();

    }
    private void Start()
    {
        EventManager.Instance.OnGameLevelUp?.AddListener(this.OnGameLevelUp);
        StartCoroutine(C_StartLevelControl());
    }
    #endregion

    #region Event Callback
    public void OnGameLevelUp()
    {
        _enemySpawner.SetSpawnTime(_enemySpawner.SpawnTime / 1.1f);
        Debug.Log($"GAME LEVEL UP : Set spawn time to {_enemySpawner.SpawnTime}");
    }
    #endregion

    #region Method
    private IEnumerator C_StartLevelControl()
    {
        while(true)
        {
            yield return _levelUpWait;
            EventManager.Instance.OnGameLevelUp?.Invoke();
        }
    }
    #endregion
}