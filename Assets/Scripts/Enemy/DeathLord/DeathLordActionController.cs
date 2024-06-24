using UnityEngine;

public class DeathLordActionController : Base_EnemyActionController
{
    protected override void Start()
    {
        base.Start();
        _animationController = transform.Find("Model").GetComponent<DeathLordAnimationController>();
        if(_animationController == null)
        {
            Debug.Log("Sss?");
        }
    }

    public override void Walk(float moveSpeed)
    {
        base.Walk(moveSpeed);
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
    }
}
