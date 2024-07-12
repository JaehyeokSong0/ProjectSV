using UnityEngine;

public class SkullManager : Base_EnemyManager
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Initialize()
    {
        if (_actionController == null)
            _actionController = GetComponent<SkullActionController>();

        base.Initialize();
    }
}