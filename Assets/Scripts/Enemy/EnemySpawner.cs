using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using EnemyType = EnemyRepository.EnemyType;
using Random = UnityEngine.Random;

[RequireComponent(typeof(EnemyRepository))]
// Manages Enemy spawn events using object pooling
public class EnemySpawner : MonoBehaviour
{
    private const int DEFAULT_CAPACITY = 100;
    private const int MAX_SIZE = 200;
    private const int SPAWN_GRID_ROW = 10;
    private const int SPAWN_GRID_COL = 18;

    public Dictionary<EnemyType, EnemySpawnInfo> EnemyInfo => _enemyInfo;

    [SerializeField] private Transform[] _spawnPositionAnchor = new Transform[4];
    [SerializeField] private List<Vector3> _spawnPosition = new List<Vector3>();

    [SerializeField] private EnemyRepository _repository;
    private EnemyType _enemyToCreate;

    private Dictionary<EnemyType, EnemySpawnInfo> _enemyInfo = new();

    [System.Serializable]
    public class EnemySpawnInfo
    {
        public bool IsInitialized => _isInitialized;
        public bool IsOnSpawn
        {
            get => _isOnSpawn;
            set => _isOnSpawn = value;
        }

        public float SpawnTime => _spawnTime;
        public WaitForSeconds SpawnTimeWait => _spawnTimeWait;
        public Coroutine SpawnCoroutine = null; // Spawn coroutine for each enemy

        public EnemySpawnInfo this[EnemyType type]
        {
            get => this;
        }

        private bool _isInitialized = false;
        private bool _isOnSpawn = false; // Turned on when the enemySpawner is activated
        private float _spawnTime = 0f; // EnemySpawner coroutine wait time for each enemy
        private WaitForSeconds _spawnTimeWait = null; // EnemySpawner coroutine wait object for each enemy

        public void Initialize(float spawnTime)
        {
            _isInitialized = true;
            _spawnTime = spawnTime;
            _spawnTimeWait = new WaitForSeconds(_spawnTime);
        }

        public enum UpdateMode
        {
            Set,
            Multiply,
            Subtract
        }
        public void UpdateSpawnTime(UpdateMode mode, float value)
        {
            if (mode == UpdateMode.Set)
                _spawnTime = value;
            else if (mode == UpdateMode.Multiply)
                _spawnTime *= value;
            else if (mode == UpdateMode.Subtract)
                _spawnTime -= value;
            else
                throw new ArgumentException();

            _spawnTimeWait = new WaitForSeconds(_spawnTime);
        }
    }

    public Dictionary<EnemyType, IObjectPool<GameObject>> Pool
    { get; private set; }

    private void Awake()
    {
        // Initialize
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
        InitializeEnemyInfo();
        InitializePool();
    }

    private void Start()
    {
        EventManager.Instance.OnPlayerDead?.AddListener(this.StopCreateAllEnemy);
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
    private void InitializeEnemyInfo()
    {
        foreach (var enemyType in _repository.GetAllEnemyType())
        {
            _enemyInfo.Add(enemyType, new EnemySpawnInfo());
        }
    }

    private void StartCreateEnemy(EnemyType enemyType)
    {
        if ((_enemyInfo[enemyType].IsInitialized == true) && (_enemyInfo[enemyType].IsOnSpawn == true))
            _enemyInfo[enemyType].SpawnCoroutine = StartCoroutine(C_StartCreateEnemy(enemyType));
    }

    private IEnumerator C_StartCreateEnemy(EnemyType enemyType)
    {
        while (_enemyInfo[enemyType].IsOnSpawn == true)
        {
            _enemyToCreate = enemyType;
            Pool[enemyType].Get();
            yield return _enemyInfo[enemyType].SpawnTimeWait;
        }
    }

    public void StopCreateAllEnemy()
    {
        foreach(var enemyType in _repository.GetAllEnemyType())
        {
            StopCreateEnemy(enemyType);
        }
    }
    public void StopCreateEnemy(EnemyType enemyType)
    {
        if ((_enemyInfo[enemyType].IsOnSpawn == true) && (_enemyInfo[enemyType].SpawnCoroutine != null))
            StopCoroutine(_enemyInfo[enemyType].SpawnCoroutine);
    }

    private void InitializePool()
    {
        Pool = new();
        foreach (var enemyType in _repository.GetAllEnemyType())
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

    public void SetEnemyToCreate(EnemyType key, bool value)
    {
        if (EnemyInfo[key].IsOnSpawn != value)
        {
            EnemyInfo[key].IsOnSpawn = value;
            if (value == true)
                StartCreateEnemy(key);
            else
                StopCreateEnemy(key);
        }
    }
}
