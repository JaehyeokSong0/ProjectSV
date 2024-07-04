using UnityEngine;
using UnityEngine.Pool;

// Manages Enemy status and events
[RequireComponent(typeof(EnemyDataManager)), RequireComponent(typeof(EnemyStateManager))]
public abstract class Base_EnemyManager : MonoBehaviour
{
    protected EnemyStateManager StateManager;
    protected EnemyDataManager DataManager;
    // Should be assigned in derived class
    protected Base_EnemyActionController ActionController = null;

    // Should be initialized with Data.LoadData() in Initialize() of derived class
    // (e.g.) Data.LoadEnemyData((Resources.Load("Data/EnemyData") as EnemyData));
    public EnemyDataManager Data { get { return DataManager; } }
    public EnemyStateManager State { get { return StateManager; } }
    public IObjectPool<GameObject> Pool;

    #region Event Functions
    protected virtual void Awake()
    {
        if (DataManager == null)
            DataManager = GetComponent<EnemyDataManager>();
        if (StateManager == null)
            StateManager = GetComponent<EnemyStateManager>();
    }

    protected void OnEnable()
    {
        Initialize();
    }
    #endregion
    #region Event Callback Actions
    public void OnEnemyDamaged(float damage) // Attacked by skill
    {
        ActionController.TakeDamage(damage);
    }

    public void OnEnemyDead()
    {
        Pool.Release(gameObject);
    }
    #endregion
    protected virtual void Initialize()
    {
        ActionController.Initialize();
    }
}
