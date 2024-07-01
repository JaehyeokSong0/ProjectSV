public class DeathLordActionController : Base_EnemyActionController
{
    protected override void Awake()
    {
        base.Awake();
        _preAttackTime = 1f; // TODO
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void Initialize()
    {
        if (_manager == null)
            _manager = GetComponent<DeathLordManager>();
        if (_animationController == null)
            _animationController = transform.Find("Model").GetComponent<DeathLordAnimationController>();

        base.Initialize();
    }
}
