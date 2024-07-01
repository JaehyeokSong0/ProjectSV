public class DeathLordActionController : Base_EnemyActionController
{
    private void Awake()
    {
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
