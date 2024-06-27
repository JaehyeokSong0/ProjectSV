using UnityEngine;

public class DeathLordManager : Base_EnemyManager
{
    protected override void Awake()
    {
        base.Awake();
        if (_actionController == null)
            _actionController = GetComponent<DeathLordActionController>();
    }

    protected override void Initialize()
    {
        Data.LoadEnemyData((Resources.Load("Data/DeathLordData") as EnemyData));
        base.Initialize();
    }
}