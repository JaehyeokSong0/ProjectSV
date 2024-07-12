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

        _enemySpawner.SetEnemyToCreate(EnemyRepository.EnemyType.Skull, true);
    }
    #endregion

    #region Event Callback
    public void OnGameLevelUp()
    {
        GameManager.Instance.GameLevel += 1;
        ChangeStatusWithLevel();
    }
    #endregion

    #region Method
    public void StartLevelControl()
    {
        _levelControlCoroutine = StartCoroutine(C_StartLevelControl());
    }
    public void StopLevelControl()
    {
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

    /// <summary>
    /// Change game logic or status
    /// </summary>
    public void ChangeStatusWithLevel()
    {
        _enemySpawner.SetSpawnTime(_enemySpawner.SpawnTime / 1.1f);
        if (GameManager.Instance.GameLevel > 1)
            _enemySpawner.SetEnemyToCreate(EnemyRepository.EnemyType.DeathLord, true);
    }
    #endregion
}