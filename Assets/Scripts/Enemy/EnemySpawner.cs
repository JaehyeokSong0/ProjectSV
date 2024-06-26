using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

// Singleton Pattern
// Manages Enemy spawn events using object pool
public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance = null;

    private const int _defaultCapacity = 10;
    private const int _maxSize = 100;
    private float _spawnTime = 2.5f;
    private WaitForSeconds _spawnTimeWait;
    private Coroutine _spawnCoroutine = null;

    [SerializeField] private GameObject _deathLordGO;
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

        if (_deathLordGO == null)
        {
            _deathLordGO = Resources.Load("Prefabs/Enemy_DeathLord") as GameObject;
            if (_deathLordGO == null)
                Debug.LogError("Cannot find Enemy_DeathLord.");
        }

        InitializePool();
    }

    private void Start()
    {
        StartCreateEnemy();
        EventManager.Instance.OnPlayerDead?.AddListener(this.StopCreateEnemy);
    }

    public void StartCreateEnemy()
    {
        _spawnCoroutine = StartCoroutine(C_StartCreateEnemy());
    }

    private IEnumerator C_StartCreateEnemy()
    {
        while(true)
        {
            var enemy = Pool.Get();
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
            defaultCapacity: _defaultCapacity, maxSize:_maxSize);

        for(int i = 0; i < _defaultCapacity; i++)
        {
            var enemy = CreateEnemy().GetComponent<Base_EnemyManager>();
            enemy.Pool.Release(enemy.gameObject);
        }
    }

    private GameObject CreateEnemy()
    {
        // TEST CODE -> TODO : Refactor with dictionary
        GameObject enemy = Instantiate(_deathLordGO);
        enemy.GetComponent<Base_EnemyManager>().Pool = this.Pool;
        return enemy;
    }

    private void OnGetEnemy(GameObject enemy)
    {
        enemy.SetActive(true);
        enemy.transform.position = Random.insideUnitCircle * 10f; // TEST CODE
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
