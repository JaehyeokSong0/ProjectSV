using System.Collections;
using UnityEngine;

public class LichActionController : Base_EnemyActionController
{
    public override Base_EnemyManager Manager
    {
        get => _manager;
        set => _manager = value as LichManager;
    }

    [SerializeField] private LichManager _manager;
    [SerializeField] private LichSpellController _spell;

    public override void Initialize()
    {
        if (Manager == null)
            Manager = GetComponent<LichManager>();
        if (_animationController == null)
            _animationController = transform.Find("Model").GetComponent<LichAnimationController>();
        if (_spell == null)
            _spell = transform.Find("Spell").GetComponent<LichSpellController>();

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

        while (_direction.magnitude <= Manager.Data.NormalAttackRange)
        {
            //EventManager.Instance.OnPlayerDamaged?.Invoke(Manager.Data.NormalAttackDamage);
            _spell.gameObject.SetActive(true);
            _spell.CastSpell(_direction);
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
