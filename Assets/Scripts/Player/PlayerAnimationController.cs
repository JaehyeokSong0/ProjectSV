using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerManager _manager;
    [SerializeField] private Animator _animator;
    [SerializeField] protected SpriteRenderer _spriteRenderer;

    private bool _isDirectionLocked = false; // Used to lock direction of attack animation
    private WaitForSeconds _colorChangeTimeWait = new WaitForSeconds(2f);

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if(_manager == null)
            _manager = transform.parent.GetComponent<PlayerManager>();
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Idle()
    {
        PlayAnimation("Idle", true);
    }

    public void Walk(Vector2 direction)
    {
        UpdateDirection(direction);
        PlayAnimation("Walk", true);
    }

    public void Run(Vector2 direction)
    {
        UpdateDirection(direction);
        PlayAnimation("Run", true);
    }

    public void NormalAttack()
    {
        PlayAnimation("NormalAttack", false);
    }

    public void Die()
    {
        PlayAnimation("Die", false);
    }

    public void UpdateDirection(Vector2 direction)
    {
        _manager.PlayerDirectionBuffer = direction;
        if(_isDirectionLocked == false)
        {
            _animator.SetFloat("DirectionX", direction.x);
            _animator.SetFloat("DirectionY", direction.y);
        }
    }

    private void PlayAnimation(string animationName, bool canTransition)
    {
        StartCoroutine(C_PlayAnimation(animationName, canTransition));
    }

    private IEnumerator C_PlayAnimation(string animationName, bool canTransition)
    {
        yield return null;

        if (canTransition)
            SetMoveAnimation(animationName);
        else
        {
            _isDirectionLocked = true;

            _animator.SetTrigger(animationName);
            float animationTime = _animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animationTime);

            _isDirectionLocked = false;
            UpdateDirection(_manager.PlayerDirectionBuffer);
            SetMoveAnimation(_manager.State.MoveState.ToString());
        }
    }
    private void SetMoveAnimation(string animationName)
    {
        int length = (int)PlayerMoveState.Length;
        for (int i = 0; i < length; i++)
            _animator.SetBool(((PlayerMoveState)i).ToString(), false);
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
        Color colorBuffer = _spriteRenderer.color;
        _spriteRenderer.color = color;

        yield return _colorChangeTimeWait;

        _spriteRenderer.color = colorBuffer;
    }
}
