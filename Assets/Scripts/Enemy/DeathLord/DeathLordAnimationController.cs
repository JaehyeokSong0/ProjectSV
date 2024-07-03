public class DeathLordAnimationController : Base_EnemyAnimationController
{
    protected override void Awake()
    {
        if(manager ==  null)
            manager = transform.parent.GetComponent<DeathLordManager>();

        base.Awake();
    }
}