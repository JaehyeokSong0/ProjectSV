using System.Collections;
using UnityEngine;
public class SkullActionController : Base_EnemyActionController
{
    public override Base_EnemyManager Manager
    {
        get => _manager;
        set => _manager = value as SkullManager;
    }

    [SerializeField] private SkullManager _manager;

    public override void Initialize()
    {
        if (Manager == null)
            Manager = GetComponent<SkullManager>();
        if (_animationController == null)
            _animationController = transform.Find("Model").GetComponent<SkullAnimationController>();

        base.Initialize();
    }

    /// <summary>
    /// Skull do not have NormalAttack motion. It just always move toward the player.
    /// Therefore, overrided C_Walk to not play action NormalAttack.
    /// </summary>
    /// <param name="moveSpeed">WalkSpeed</param>
    /// <returns>IEnumerator</returns>

    protected override IEnumerator C_NormalAttack()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        Manager.State.MoveState = EnemyMoveState.Idle;
        _animationController.Idle();

        // NormalAttackRange should be bigger than sum of collider radius of the player and the skull
        while (_direction.magnitude <= Manager.Data.NormalAttackRange)
        {
            EventManager.Instance.OnPlayerDamaged?.Invoke(Manager.Data.NormalAttackDamage);
            yield return _normalAttackWait;
        }

        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (_direction.magnitude < Manager.Data.NormalAttackRange)
            Manager.State.MoveState = EnemyMoveState.Idle;
        else
            Manager.State.MoveState = EnemyMoveState.Walk;
        _animationController.Move();
    }
}
