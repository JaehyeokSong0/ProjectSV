using UnityEngine;

public class LichAnimationController : Base_EnemyAnimationController
{
    protected override void Awake()
    {
        if (_manager == null)
            _manager = transform.parent.GetComponent<LichManager>();

        base.Awake();
    }
}