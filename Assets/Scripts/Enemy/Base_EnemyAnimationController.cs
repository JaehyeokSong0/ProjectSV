using System.Collections;
using UnityEngine;

// Control common animations like idle, walk, dead
[RequireComponent (typeof(Animator))]
public abstract class Base_EnemyAnimationController : MonoBehaviour
{
    private const float _postActionDelay = 0.5f; // after action(canTransition) delay

    [SerializeField] protected Base_EnemyManager _manager;
    [SerializeField] protected Animator _animator;
    protected GameObject _player;

    [SerializeField] protected SpriteRenderer _spriteRenderer;
    private WaitForSeconds _colorChangeTimeWait = new WaitForSeconds(0.3f);

    [SerializeField] public bool _isDirectionLocked; // Used to lock direction of attack animation

    protected Coroutine _currentAnimation = null;

    #region Event Functions
    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    protected void OnEnable()
    {
        _isDirectionLocked = false;
        _spriteRenderer.color = Color.white;
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
    #endregion

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
        StopCoroutine(_currentAnimation);
        PlayAnimation("Die", false);
        _spriteRenderer.color = Color.white;
    }

    protected void PlayAnimation(string animationName, bool canTransition)
    { 
        _currentAnimation = StartCoroutine(C_PlayAnimation(animationName, canTransition));
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

            if(animationName.Equals("Die") == false)
                _isDirectionLocked = false;
        }
    }

    private void SetMoveAnimation(string animationName)
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

    public void ChangeSpriteColor(Color color)
    {
        StartCoroutine(C_ChangeSpriteColor(color));
    }

    private IEnumerator C_ChangeSpriteColor(Color color)
    {
        _spriteRenderer.color = color;

        yield return _colorChangeTimeWait;

        _spriteRenderer.color = Color.white;
    }
}
