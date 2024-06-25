using UnityEngine;

public class DeathLordManager : Base_EnemyManager
{
    private void Awake()
    {
        Data = ScriptableObject.CreateInstance<EnemyData>();
        if (_actionController == null)
            _actionController = GetComponent<DeathLordActionController>();
    }

    protected override void Start()
    {
        base.Start();
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