public class DeathLordActionController : Base_EnemyActionController
{
    private void Awake()
    {
        preAttackTime = 1f; // TODO
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void Initialize()
    {
        if (manager == null)
            manager = GetComponent<DeathLordManager>();
        if (animationController == null)
            animationController = transform.Find("Model").GetComponent<DeathLordAnimationController>();

        base.Initialize();
    }
}
