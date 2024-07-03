using UnityEngine;
using UnityEngine.Pool;

// Manages Enemy status and events
[RequireComponent(typeof(EnemyDataManager)), RequireComponent(typeof(EnemyStateManager))]
public abstract class Base_EnemyManager : MonoBehaviour
{
    protected EnemyStateManager stateManager;
    protected EnemyDataManager dataManager;
    // Should be assigned in derived class
    protected Base_EnemyActionController actionController = null;

    // Should be initialized with Data.LoadData() in Initialize() of derived class
    // (e.g.) Data.LoadEnemyData((Resources.Load("Data/EnemyData") as EnemyData));
    public EnemyDataManager data { get { return dataManager; } }
    public EnemyStateManager state { get { return stateManager; } }
    public IObjectPool<GameObject> pool;

    #region Event Functions
    protected virtual void Awake()
    {
        if (dataManager == null)
            dataManager = GetComponent<EnemyDataManager>();
        if (stateManager == null)
            stateManager = GetComponent<EnemyStateManager>();
    }

    protected void OnEnable()
    {
        Initialize();
    }
    #endregion
    #region Event Callback Actions
    public void OnEnemyDamaged(float damage) // Attacked by skill
    {
        actionController.TakeDamage(damage);
    }

    public void OnEnemyDead()
    {
        pool.Release(gameObject);
    }
    #endregion
    protected virtual void Initialize()
    {
        actionController.Initialize();
    }
}
