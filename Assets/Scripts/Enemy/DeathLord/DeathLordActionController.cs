public class DeathLordActionController : Base_EnemyActionController
{
    protected override void Awake()
    {
        base.Awake();
        _preAttackTime = 1f;
    }

    protected override void Start()
    {
        base.Start();
        _animationController = transform.Find("Model").GetComponent<DeathLordAnimationController>();
    }
}
