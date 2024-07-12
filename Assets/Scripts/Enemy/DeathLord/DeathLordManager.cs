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

        base.Initialize();
    }
}