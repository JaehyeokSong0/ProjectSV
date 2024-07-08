using UnityEngine;

public class DeathLordManager : Base_EnemyManager
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Initialize()
    {
        if (_actionController == null)
            _actionController = GetComponent<DeathLordActionController>();

        Data.LoadEnemyData((Resources.Load("Data/DeathLordData") as EnemyData));
        base.Initialize();
    }
}