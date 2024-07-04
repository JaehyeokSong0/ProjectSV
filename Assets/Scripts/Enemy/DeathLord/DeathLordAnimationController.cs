public class DeathLordAnimationController : Base_EnemyAnimationController
{
    protected override void Awake()
    {
        if(_manager ==  null)
            _manager = transform.parent.GetComponent<DeathLordManager>();

        base.Awake();
    }
}