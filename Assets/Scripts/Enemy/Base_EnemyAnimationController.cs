using System.Collections;
using UnityEngine;

// Control common animations like idle, walk, dead
[RequireComponent (typeof(Animator))]
public abstract class Base_EnemyAnimationController : MonoBehaviour
{
    private const float POST_ACTION_DELAY = 0.5f; // after action(canTransition) delay

    [SerializeField] protected Base_EnemyManager manager;
    [SerializeField] protected Animator animator;
    protected GameObject playerGO;

    [SerializeField] protected SpriteRenderer spriteRenderer;
    private WaitForSeconds _colorChangeTimeWait = new WaitForSeconds(0.3f);

    [SerializeField] private bool _isDirectionLocked; // Used to lock direction of attack animation

    protected Coroutine currentAnimation = null;

    #region Event Functions
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
    }

    protected void OnEnable()
    {
        _isDirectionLocked = false;
        spriteRenderer.color = Color.white;
    }

    protected void Update()
    {
        Vector2 direction = playerGO.transform.position - transform.position;
        direction.Normalize();

        if (_isDirectionLocked == false)
        {
            animator.SetFloat("DirectionX", direction.x);
            animator.SetFloat("DirectionY", direction.y);
        }
    }
    #endregion

    public void Move()
    {
        PlayAnimation(manager.state.MoveState.ToString(), true);
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
        StopCoroutine(currentAnimation);
        PlayAnimation("Die", false);
        spriteRenderer.color = Color.white;
    }

    protected void PlayAnimation(string animationName, bool canTransition)
    { 
        currentAnimation = StartCoroutine(C_PlayAnimation(animationName, canTransition));
    }

    protected IEnumerator C_PlayAnimation(string animationName, bool canTransition)
    {
        yield return null;

        if (canTransition == true)
            SetMoveAnimation(animationName);
        else
        {
            _isDirectionLocked = true;

            animator.SetTrigger(animationName);
            float animationTime = animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animationTime + POST_ACTION_DELAY);

            SetMoveAnimation(manager.state.MoveState.ToString());

            if(animationName.Equals("Die") == false)
                _isDirectionLocked = false;
        }
    }

    private void SetMoveAnimation(string animationName)
    {
        int length = (int)EnemyMoveState.Length;
        for (int i = 0; i < length; i++)
            animator.SetBool(((EnemyMoveState)i).ToString(), false);
        animator.SetBool(animationName, true);
    }

    public float GetCurrentAnimationLength()
    {
        float animationTime = animator.GetCurrentAnimatorStateInfo(0).length;
        return animationTime;
    }

    public void ChangeSpriteColor(Color color)
    {
        StartCoroutine(C_ChangeSpriteColor(color));
    }

    private IEnumerator C_ChangeSpriteColor(Color color)
    {
        spriteRenderer.color = color;

        yield return _colorChangeTimeWait;

        spriteRenderer.color = Color.white;
    }
}
