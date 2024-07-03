using UnityEngine;

public class DeathLordManager : Base_EnemyManager
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Initialize()
    {
        if (actionController == null)
            actionController = GetComponent<DeathLordActionController>();

        data.LoadEnemyData((Resources.Load("Data/DeathLordData") as EnemyData));
        base.Initialize();
    }
}