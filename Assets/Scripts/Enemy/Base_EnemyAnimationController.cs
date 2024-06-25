using System.Collections;
using UnityEngine;

// Control common animations like idle, walk, dead
[RequireComponent (typeof(Animator))]
public abstract class Base_EnemyAnimationController : MonoBehaviour
{
    private const float _attackDelay = 0.5f;

    [SerializeField] protected Animator _animator;
    [SerializeField] protected EnemyStateManager _stateManager;
    protected GameObject _player;

    private bool _isDirectionLocked = false; // Used to lock direction of attack animation

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        if(_stateManager == null)
            _stateManager = gameObject.GetComponentInParent<EnemyStateManager>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void Update()
    {
        Vector2 direction = _player.transform.position - transform.position;
        direction.Normalize();

        if (_isDirectionLocked == false)
        {
            _animator.SetFloat("DirectionX", direction.x);
            _animator.SetFloat("DirectionY", direction.y);
        }
    }

    public virtual void Move()
    {
        PlayAnimation(_stateManager.MoveState.ToString(), true);
    }

    public virtual void Idle()
    {
        PlayAnimation("Idle", true);
    }

    public virtual void Walk()
    {
        PlayAnimation("Walk", true);
    }

    public virtual void NormalAttack()
    {
        PlayAnimation("NormalAttack", false);
    }

    public virtual void Die()
    {
        PlayAnimation("Die", false);
    }

    protected virtual void PlayAnimation(string animationName, bool canTransition)
    { 
        StartCoroutine(C_PlayAnimation(animationName, canTransition));
    }

    protected virtual IEnumerator C_PlayAnimation(string animationName, bool canTransition)
    {
        yield return null;

        if (canTransition == true)
            SetMoveAnimation(animationName);
        else
        {
            _isDirectionLocked = true;

            _animator.SetTrigger(animationName);
            float animationTime = _animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animationTime + _attackDelay);

            SetMoveAnimation(_stateManager.MoveState.ToString());

            _isDirectionLocked = false;
        }
    }

    public void SetMoveAnimation(string animationName)
    {
        int length = (int)EnemyMoveState.Length;
        for (int i = 0; i < length; i++)
            _animator.SetBool(((EnemyMoveState)i).ToString(), false);
        _animator.SetBool(animationName, true);
    }

    public float GetCurrentAnimationLength()
    {
        float animationTime = _animator.GetCurrentAnimatorStateInfo(0).length;
        return animationTime;
    }
}
