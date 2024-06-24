using System.Collections;
using UnityEngine;

public class DeathLordAnimationController : Base_EnemyAnimationController
{
    protected override void Awake()
    {
        base.Awake();
    }
    
    public override void Idle()
    {
        base.Idle();
    }

    public override void Walk()
    {
        base.Walk();
    }

    public override void NormalAttack()
    {
        base.NormalAttack();
    }

    protected override IEnumerator C_PlayAnimation(string animationName, bool canTransition)
    {
        return base.C_PlayAnimation(animationName, canTransition);
    }

}