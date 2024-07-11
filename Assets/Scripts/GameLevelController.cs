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
    private Coroutine _levelControlCoroutine = null;
    #endregion

    #region Event Method
    private void Awake()
    {
        if (_enemySpawner == null)
            _enemySpawner = GameObject.Find("Enemy Spawner").GetComponent<EnemySpawner>();

    }
    private void OnEnable()
    {
        StartLevelControl();
    }
    private void Start()
    {
        EventManager.Instance.OnGameLevelUp?.AddListener(this.OnGameLevelUp);
        EventManager.Instance.OnPlayerDead?.AddListener(StopLevelControl);
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
    public void StartLevelControl()
    {
        Debug.Log($"Start Level Control : {_enemySpawner.SpawnTime}");
        _levelControlCoroutine = StartCoroutine(C_StartLevelControl());
    }
    public void StopLevelControl()
    {
        Debug.Log($"Stop Level Control : {_enemySpawner.SpawnTime}");
        if (_levelControlCoroutine != null)
            StopCoroutine(_levelControlCoroutine);
    }
    private IEnumerator C_StartLevelControl()
    {
        while (true)
        {
            yield return _levelUpWait;
            EventManager.Instance.OnGameLevelUp?.Invoke();
        }
    }
    #endregion
}