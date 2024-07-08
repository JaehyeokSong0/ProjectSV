using UnityEngine;
using UnityEngine.Pool;

// Manages Enemy status and events
[RequireComponent(typeof(EnemyDataManager)), RequireComponent(typeof(EnemyStateManager))]
public abstract class Base_EnemyManager : MonoBehaviour
{
    #region Property
    // Should be initialized with Data.LoadData() in Initialize() of derived class
    // (e.g.) Data.LoadEnemyData((Resources.Load("Data/EnemyData") as EnemyData));
    public EnemyDataManager Data { get { return _dataManager; } }
    public EnemyStateManager State { get { return _stateManager; } }
    #endregion

    #region Field
    public IObjectPool<GameObject> Pool;

    protected EnemyStateManager _stateManager;
    protected EnemyDataManager _dataManager;
    protected Base_EnemyActionController _actionController = null; // Should be assigned in derived class
    protected EnemyDropExpController _dropExpController;
    #endregion

    #region Event Method
    protected virtual void Awake()
    {
        if (_dataManager == null)
            _dataManager = GetComponent<EnemyDataManager>();
        if (_stateManager == null)
            _stateManager = GetComponent<EnemyStateManager>();
        if (_dropExpController == null)
            _dropExpController = GetComponent<EnemyDropExpController>();
    }

    protected void OnEnable()
    {
        Initialize();
    }
    #endregion

    #region Event Callback
    public void OnEnemyDamaged(float damage) // Attacked by skill
    {
        _actionController.TakeDamage(damage);
    }

    public void OnEnemyDead()
    {
        Pool.Release(gameObject);
        _dropExpController.DropExp(_dataManager.ExpDropRate, _dataManager.Exp);
    }
    #endregion

    #region Method
    protected virtual void Initialize()
    {
        _actionController.Initialize();
    }
    #endregion
}
