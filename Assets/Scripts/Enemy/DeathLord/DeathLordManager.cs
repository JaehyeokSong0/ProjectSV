using UnityEngine;

public class DeathLordManager : Base_EnemyManager
{
    private void Awake()
    {
        if (_actionController == null)
            _actionController = GetComponent<DeathLordActionController>();
    }

    protected override void OnEnable()
    {
        Data = ScriptableObject.CreateInstance<EnemyData>();
        base.OnEnable();
    }

    public override void OnEnemyDamaged(float damage, float coolTime)
    {
        base.OnEnemyDamaged(damage, coolTime);
    }

    protected override void Initialize()
    {
        base.Initialize();
    }
}