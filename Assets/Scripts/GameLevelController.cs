using System.Collections;
using UnityEngine;
using EnemyType = EnemyRepository.EnemyType;

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
    private void Start()
    {
        EventManager.Instance.OnGameLevelUp?.AddListener(this.OnGameLevelUp);
        EventManager.Instance.OnPlayerDead?.AddListener(StopLevelControl);
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
        ChangeStatusWithLevel();
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
        switch (GameManager.Instance.GameLevel)
        {
            case 1:
                {
                    _enemySpawner.EnemyInfo[EnemyType.Lich].Initialize(20f);
                    _enemySpawner.SetEnemyToCreate(EnemyType.Lich, true);

                    _enemySpawner.EnemyInfo[EnemyType.Skull].Initialize(1.5f);
                    _enemySpawner.SetEnemyToCreate(EnemyType.Skull, true);
                    break;
                }
            case 2:
                {
                    _enemySpawner.EnemyInfo[EnemyType.Skull].UpdateSpawnTime(EnemySpawner.EnemySpawnInfo.UpdateMode.Multiply, 0.95f);
                    break;
                }
            case 3:
                {
                    _enemySpawner.SetEnemyToCreate(EnemyType.Skull, false);

                    _enemySpawner.EnemyInfo[EnemyType.DeathLord].Initialize(5f);
                    _enemySpawner.SetEnemyToCreate(EnemyType.DeathLord, true);
                    break;
                }
            case 4:
                {
                    _enemySpawner.EnemyInfo[EnemyType.DeathLord].UpdateSpawnTime(EnemySpawner.EnemySpawnInfo.UpdateMode.Set, 8.5f);

                    _enemySpawner.SetEnemyToCreate(EnemyType.Skull, true);
                    break;
                }
            default:
                {
                    _enemySpawner.EnemyInfo[EnemyType.DeathLord].UpdateSpawnTime(EnemySpawner.EnemySpawnInfo.UpdateMode.Multiply, 0.95f);

                    _enemySpawner.EnemyInfo[EnemyType.Skull].UpdateSpawnTime(EnemySpawner.EnemySpawnInfo.UpdateMode.Multiply, 0.98f);
                    break;
                }
        }
        //Debug.Log("Current spawnTime of Skull : " + _enemySpawner.EnemyInfo[EnemyType.Skull].SpawnTime);
        //Debug.Log("Current spawnTime of DeathLord : " + _enemySpawner.EnemyInfo[EnemyType.DeathLord].SpawnTime);
    }
    #endregion
}