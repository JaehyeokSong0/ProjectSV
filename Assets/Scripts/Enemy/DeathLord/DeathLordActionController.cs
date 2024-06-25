using System.Collections;
using UnityEngine;

public class DeathLordActionController : Base_EnemyActionController
{

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        _animationController = transform.Find("Model").GetComponent<DeathLordAnimationController>();
    }
}
