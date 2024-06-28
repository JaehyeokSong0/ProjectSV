using System.Collections;
using UnityEngine;

// Control common animations like idle, walk, dead
[RequireComponent (typeof(Animator))]
public abstract class Base_EnemyAnimationController : MonoBehaviour
{
    private const float _postActionDelay = 0.5f; // after action(canTransition) delay

    [SerializeField] protected Animator _animator;
    [SerializeField] protected Base_EnemyManager _manager;
    protected GameObject _player;

    [SerializeField] private bool _isDirectionLocked; // Used to lock direction of attack animation

    protected void Awake()
    {
        _animator = GetComponent<Animator>();
        if(_manager == null)
            _manager = GetComponentInParent<Base_EnemyManager>(); // TODO : fix initialize
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    protected void OnEnable()
    {
        _isDirectionLocked = false;
    }

    protected void OnDisable()
    {
        StopAllCoroutines();
    }

    protected void Update()
    {
        Vector2 direction = _player.transform.position - transform.position;
        direction.Normalize();

        if (_isDirectionLocked == false)
        {
            _animator.SetFloat("DirectionX", direction.x);
            _animator.SetFloat("DirectionY", direction.y);
        }
    }

    public void Move()
    {
        PlayAnimation(_manager.State.MoveState.ToString(), true);
    }

    public void Idle()
    {
        PlayAnimation("Idle", true);
    }

    public void Walk()
    {
        PlayAnimation("Walk", true);
    }

    public void NormalAttack()
    {
        PlayAnimation("NormalAttack", false);
    }

    public void Die()
    {
        PlayAnimation("Die", false);
    }

    protected void PlayAnimation(string animationName, bool canTransition)
    { 
        StartCoroutine(C_PlayAnimation(animationName, canTransition));
    }

    protected IEnumerator C_PlayAnimation(string animationName, bool canTransition)
    {
        yield return null;

        if (canTransition == true)
            SetMoveAnimation(animationName);
        else
        {
            _isDirectionLocked = true;

            _animator.SetTrigger(animationName);
            float animationTime = _animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animationTime + _postActionDelay);

            SetMoveAnimation(_manager.State.MoveState.ToString());

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
