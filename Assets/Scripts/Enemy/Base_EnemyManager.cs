using UnityEngine;
using UnityEngine.Pool;

// Manages Enemy status and events
[RequireComponent(typeof(EnemyDataManager)), RequireComponent(typeof(EnemyStateManager))]
public abstract class Base_EnemyManager : MonoBehaviour
{
    protected EnemyStateManager _stateManager;
    protected EnemyDataManager _dataManager;
    // Should be assigned in derived class
    protected Base_EnemyActionController _actionController = null;

    // Should be initialized with Data.LoadData() in Initialize() of derived class
    // (e.g.) Data.LoadEnemyData((Resources.Load("Data/EnemyData") as EnemyData));
    public EnemyDataManager Data { get { return _dataManager; } }
    public EnemyStateManager State { get { return _stateManager; } }
    public IObjectPool<GameObject> Pool;

    #region Event Functions
    protected virtual void Awake()
    {
        if (_dataManager == null)
            _dataManager = GetComponent<EnemyDataManager>();
        if (_stateManager == null)
            _stateManager = GetComponent<EnemyStateManager>();
    }

    protected void OnEnable()
    {
        Initialize();
    }
    #endregion
    #region Event Callback Actions
    public void OnEnemyDamaged(float damage) // Attacked by skill
    {
        _actionController.TakeDamage(damage);
    }

    public void OnEnemyDamaged(float damage, float coolTime) // Attacked by normalAttack
    {
        if (State.IsAttacked == false)
        {
            _actionController.TakeDamage(damage, coolTime);
        }
    }

    public void OnEnemyDead()
    {
        Pool.Release(gameObject);
    }
    #endregion
    protected virtual void Initialize()
    {
        _actionController.Initialize();
    }
}
