public class SkullAnimationController : Base_EnemyAnimationController
{
    protected override void Awake()
    {
        if (_manager == null)
            _manager = transform.parent.GetComponent<SkullManager>();

        base.Awake();
    }
}