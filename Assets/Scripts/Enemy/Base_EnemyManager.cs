using UnityEngine;
using UnityEngine.Pool;

// Manages Enemy status and events
public abstract class Base_EnemyManager : MonoBehaviour
{
    protected Base_EnemyActionController _actionController;

    // Should be assigned in Awake() of derived class
    // Data = ScriptableObject.CreateInstance<EnemyData>();
    public EnemyData Data;
    public bool IsAttacked; // If attacked recently, it has invincible time for a while

    public IObjectPool<GameObject> Pool;

    protected virtual void OnEnable()
    {
        Initialize();
    }

    public virtual void OnEnemyDamaged(float damage, float coolTime)
    {
        if (IsAttacked == false)
        {
            _actionController.TakeDamage(damage, coolTime);
        }
    }

    public virtual void OnEnemyDead()
    {
        Pool.Release(gameObject);
    }

    protected virtual void Initialize()
    {
        _actionController.Walk(Data.WalkSpeed);
    }
}
