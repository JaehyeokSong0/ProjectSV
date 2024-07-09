using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// Singleton Pattern
// Manages Enemy spawn events using object pooling
public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance = null;
    public float SpawnTime => _spawnTime;

    private const int DEFAULT_CAPACITY = 5;
    private const int MAX_SIZE = 5;
    private const int SPAWN_GRID_ROW = 10;
    private const int SPAWN_GRID_COL = 18;
    private float _spawnTime = 1.5f;
    private WaitForSeconds _spawnTimeWait;
    private Coroutine _spawnCoroutine = null;
    [SerializeField] private Transform[] _spawnPositionAnchor = new Transform[4];
    [SerializeField] private List<Vector3> _spawnPosition = new List<Vector3>();
    [SerializeField] private GameObject _deathLordGO; // TODO
    public IObjectPool<GameObject> Pool
    { get; private set; }

    private void Awake()
    {
        // Set class singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
                Destroy(this.gameObject);
        }

        // Initialize
        _spawnTimeWait = new WaitForSeconds(_spawnTime);

        if (_spawnPositionAnchor[0] == null)
        {
            var spawnPositionGrids = GameObject.Find("EnemySpawnPosition");
            for (int i = 0; i < 4; i++)
            {
                _spawnPositionAnchor[i] = spawnPositionGrids.transform.GetChild(i);
            }
        }

        if (_deathLordGO == null)
        {
            _deathLordGO = Resources.Load("Prefabs/Enemy_DeathLord") as GameObject;
            if (_deathLordGO == null)
                Debug.LogError("Cannot find Enemy_DeathLord.");
        }
        InitializeSpawnPosition();
        InitializePool();
    }

    private void Start()
    {
        StartCreateEnemy();
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
            Pool.Get();
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
        Pool = new ObjectPool<GameObject>(
            CreateEnemy, OnGetEnemy, OnReleaseEnemy, OnDestroyEnemy,
            defaultCapacity: DEFAULT_CAPACITY, maxSize: MAX_SIZE);

        for (int i = 0; i < DEFAULT_CAPACITY; i++)
        {
            var enemy = CreateEnemy().GetComponent<Base_EnemyManager>();
            enemy.Pool.Release(enemy.gameObject);
        }
    }

    private GameObject CreateEnemy()
    {
        Vector3 spawnPosition = _spawnPosition[Random.Range(0, _spawnPosition.Count)];
        GameObject enemy = Instantiate(_deathLordGO, spawnPosition, Quaternion.identity);
        //GameObject enemy = Instantiate(_deathLordGO, Random.insideUnitCircle * 10f, Quaternion.identity);
        enemy.GetComponent<Base_EnemyManager>().Pool = this.Pool;
        return enemy;
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
}
