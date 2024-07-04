using UnityEngine;

public class DeathLordManager : Base_EnemyManager
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Initialize()
    {
        if (ActionController == null)
            ActionController = GetComponent<DeathLordActionController>();

        Data.LoadEnemyData((Resources.Load("Data/DeathLordData") as EnemyData));
        base.Initialize();
    }
}