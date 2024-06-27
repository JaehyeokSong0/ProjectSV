using UnityEngine;
using UnityEngine.Pool;

// Manages Enemy status and events
[RequireComponent(typeof(EnemyDataManager))]
public abstract class Base_EnemyManager : MonoBehaviour
{
    // Should be initialized in derived class
    protected Base_EnemyActionController _actionController = null;

    // Should be initialized with Data.LoadData() in Initialize() of derived class
    // (e.g.) Data.LoadEnemyData((Resources.Load("Data/EnemyData") as EnemyData));
    public EnemyDataManager Data;
    public bool IsAttacked; // If attacked recently, it has invincible time for a while
    public IObjectPool<GameObject> Pool;

    protected virtual void Awake()
    {
        if(Data == null)
            Data = GetComponent<EnemyDataManager>();
    }

    protected void OnEnable()
    {
        Initialize();
    }

    public void OnEnemyDamaged(float damage) // Attacked by skill
    {
        _actionController.TakeDamage(damage);
    }
    public void OnEnemyDamaged(float damage, float coolTime) // Attacked by normalAttack
    {
        if (IsAttacked == false)
        {
            _actionController.TakeDamage(damage, coolTime);
        }
    }

    public void OnEnemyDead()
    {
        Pool.Release(gameObject);
    }

    protected virtual void Initialize()
    {
        _actionController.Initialize();
    }
}
