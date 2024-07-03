using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

// Singleton Pattern
// Manages Enemy spawn events using object pooling
public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance = null;

    private const int DEFAULT_CAPACITY = 20;
    private const int MAX_SIZE = 50;

    private float _spawnTime = 0.5f;
    private WaitForSeconds _spawnTimeWait;
    private Coroutine _spawnCoroutine = null;

    [SerializeField] private GameObject _deathLordGO; // TODO
    public IObjectPool<GameObject> pool
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
        EventManager.instance.OnPlayerDead?.AddListener(this.StopCreateEnemy);
    }

    public void StartCreateEnemy()
    {
        _spawnCoroutine = StartCoroutine(C_StartCreateEnemy());
    }

    private IEnumerator C_StartCreateEnemy()
    {
        while(true)
        {
            pool.Get();
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
        pool = new ObjectPool<GameObject>(
            CreateEnemy, OnGetEnemy, OnReleaseEnemy, OnDestroyEnemy, 
            defaultCapacity: DEFAULT_CAPACITY, maxSize: MAX_SIZE);

        for(int i = 0; i < DEFAULT_CAPACITY; i++)
        {
            var enemy = CreateEnemy().GetComponent<Base_EnemyManager>();
            enemy.pool.Release(enemy.gameObject);
        }
    }

    private GameObject CreateEnemy()
    {
        // TEST CODE -> TODO : Refactor with dictionary
        GameObject enemy = Instantiate(_deathLordGO, Random.insideUnitCircle * 10f, Quaternion.identity);
        enemy.GetComponent<Base_EnemyManager>().pool = this.pool;
        return enemy;
    }

    private void OnGetEnemy(GameObject enemy)
    {
        enemy.transform.position = Random.insideUnitCircle * 10f; // TEST CODE
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
