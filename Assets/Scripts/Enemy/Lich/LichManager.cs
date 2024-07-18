using UnityEngine;

public class LichManager : Base_EnemyManager
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Initialize()
    {
        if (_actionController == null)
            _actionController = GetComponent<LichActionController>();

        base.Initialize();
    }
}