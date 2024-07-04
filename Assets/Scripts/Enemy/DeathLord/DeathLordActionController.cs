using UnityEngine;
public class DeathLordActionController : Base_EnemyActionController
{
    public override Base_EnemyManager Manager 
    { 
        get => _manager; 
        set => _manager = value as DeathLordManager; 
    }

    [SerializeField] private DeathLordManager _manager;

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
        if (Manager == null)
            Manager = GetComponent<DeathLordManager>();
        if (_animationController == null)
            _animationController = transform.Find("Model").GetComponent<DeathLordAnimationController>();

        base.Initialize();
    }
}
