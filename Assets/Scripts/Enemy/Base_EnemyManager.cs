using System.Collections;
using UnityEngine;

// Manages Enemy Data and status
public abstract class Base_EnemyManager : MonoBehaviour
{
    protected Base_EnemyActionController _actionController;

    // Should be assigned in Awake() of derived class
    // Data = ScriptableObject.CreateInstance<EnemyData>();
    public EnemyData Data 
    { get; set; }
    public bool IsAttacked
    { get; set; }

    protected virtual void Start()
    {
        Initialize();
    }

    public virtual void OnEnemyDamaged(float damage, float coolTime)
    {
        if(IsAttacked == false) // If attacked recently, it has invincible time for a while
        {
            Debug.Log($"{gameObject.name} damaged : {Data.Hp} -> {Data.Hp - damage}");

            _actionController.TakeDamage(damage, coolTime);
        }
    }

    protected virtual void Initialize()
    {
        _actionController.Walk(Data.WalkSpeed);
    }

 
}
