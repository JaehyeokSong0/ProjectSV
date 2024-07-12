using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using EnemyType = EnemyRepository.EnemyType;

[RequireComponent(typeof(EnemyRepository))]
// Manages Enemy spawn events using object pooling
public class EnemySpawner : MonoBehaviour
{
    public float SpawnTime => _spawnTime;

    private const int DEFAULT_CAPACITY = 100;
    private const int MAX_SIZE = 200;
    private const int SPAWN_GRID_ROW = 10;
    private const int SPAWN_GRID_COL = 18;
    private float _spawnTime = 1.5f;
    private WaitForSeconds _spawnTimeWait;
    private Coroutine _spawnCoroutine = null;
    [SerializeField] private Transform[] _spawnPositionAnchor = new Transform[4];
    [SerializeField] private List<Vector3> _spawnPosition = new List<Vector3>();
    [SerializeField] private EnemyRepository _repository;
    [SerializeField] private Dictionary<EnemyType, bool> _isEnemyOnSpawn = new();
    [SerializeField] private EnemyType _enemyToCreate;

    public Dictionary<EnemyType, IObjectPool<GameObject>> Pool
    { get; private set; }

    private void Awake()
    {
        // Initialize
        _spawnTimeWait = new WaitForSeconds(_spawnTime);
        if (_repository == null)
            _repository = GetComponent<EnemyRepository>();

        if (_spawnPositionAnchor[0] == null)
        {
            var spawnPositionGrids = GameObject.Find("EnemySpawnPosition");
            for (int i = 0; i < 4; i++)
            {
                _spawnPositionAnchor[i] = spawnPositionGrids.transform.GetChild(i);
            }
        }

        InitializeSpawnPosition();
        InitializePool();
    }

    private void Start()
    {
        EventManager.Instance.OnPlayerDead?.AddListener(this.StopCreateEnemy);
    }

    public void SetSpawnTime(float newSpawnTime)
    {
        _spawnTime = newSpawnTime;
        _spawnTimeWait = new WaitForSeconds(newSpawnTime);
    }
    private void InitializeSpawnPosition()
    {
        // Top
        for (float i = 1f; i < SPAWN_GRID_COL; i++)
        {
            Vector3 spawnPosition = new()
            {
                x = Mathf.Lerp(_spawnPositionAnchor[0].position.x, _spawnPositionAnchor[1].position.x, i / SPAWN_GRID_COL),
                y = Mathf.Lerp(_spawnPositionAnchor[0].position.y, _spawnPositionAnchor[1].position.y, i / SPAWN_GRID_COL)
            };
            _spawnPosition.Add(spawnPosition);
        }
        // Right
        for (float i = 1f; i < SPAWN_GRID_ROW; i++)
        {
            Vector3 spawnPosition = new()
            {
                x = Mathf.Lerp(_spawnPositionAnchor[0].position.x, _spawnPositionAnchor[3].position.x, i / SPAWN_GRID_ROW),
                y = Mathf.Lerp(_spawnPositionAnchor[0].position.y, _spawnPositionAnchor[3].position.y, i / SPAWN_GRID_ROW)
            };
            _spawnPosition.Add(spawnPosition);
        }
        // Bottom
        for (float i = 1f; i < SPAWN_GRID_COL; i++)
        {
            Vector3 spawnPosition = new()
            {
                x = Mathf.Lerp(_spawnPositionAnchor[3].position.x, _spawnPositionAnchor[2].position.x, i / SPAWN_GRID_COL),
                y = Mathf.Lerp(_spawnPositionAnchor[3].position.y, _spawnPositionAnchor[2].position.y, i / SPAWN_GRID_COL)
            };
            _spawnPosition.Add(spawnPosition);
        }
        // Left
        for (float i = 1f; i < SPAWN_GRID_ROW; i++)
        {
            Vector3 spawnPosition = new()
            {
                x = Mathf.Lerp(_spawnPositionAnchor[1].position.x, _spawnPositionAnchor[2].position.x, i / SPAWN_GRID_ROW),
                y = Mathf.Lerp(_spawnPositionAnchor[2].position.y, _spawnPositionAnchor[2].position.y, i / SPAWN_GRID_ROW)
            };
            _spawnPosition.Add(spawnPosition);
        }
    }
    public void StartCreateEnemy()
    {
        _spawnCoroutine = StartCoroutine(C_StartCreateEnemy());
    }

    private IEnumerator C_StartCreateEnemy()
    {
        while (true)
        {
            foreach (var enemy in _isEnemyOnSpawn)
            {
                if (enemy.Value == true)
                {
                    _enemyToCreate = enemy.Key;
                    Pool[enemy.Key].Get();
                }
            }
            yield return _spawnTimeWait;
        }
    }

    public void StopCreateEnemy()
    {
        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);
    }

    private void InitializePool()
    {
        Pool = new();
        foreach (EnemyType enemyType in System.Enum.GetValues(typeof(EnemyType)))
        {
            _enemyToCreate = enemyType;
            var pool = new ObjectPool<GameObject>(
                CreateEnemy, OnGetEnemy, OnReleaseEnemy, OnDestroyEnemy,
                defaultCapacity: DEFAULT_CAPACITY, maxSize: MAX_SIZE);
            Pool.Add(enemyType, pool);

            for (int i = 0; i < DEFAULT_CAPACITY; i++)
            {
                var enemy = CreateEnemy().GetComponent<Base_EnemyManager>();
                enemy.Pool.Release(enemy.gameObject);
            }
        }
    }

    private GameObject CreateEnemy()
    {
        Vector3 spawnPosition = _spawnPosition[Random.Range(0, _spawnPosition.Count)];

        var createdEnemy = Instantiate(_repository[_enemyToCreate], spawnPosition, Quaternion.identity, transform);
        createdEnemy.GetComponent<Base_EnemyManager>().Pool = this.Pool[_enemyToCreate];

        return createdEnemy;
    }

    private void OnGetEnemy(GameObject enemy)
    {
        Vector3 spawnPosition = _spawnPosition[Random.Range(0, _spawnPosition.Count)];
        enemy.transform.position = spawnPosition;
        enemy.SetActive(true);
    }

    private void OnReleaseEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
    }

    private void OnDestroyEnemy(GameObject enemy)
    {
        Destroy(enemy);
    }

    public bool GetEnemyToCreate(EnemyType key)
    {
        return _isEnemyOnSpawn[key];
    }
    public void SetEnemyToCreate(EnemyType key, bool value)
    {
        _isEnemyOnSpawn[key] = value;
    }
}
