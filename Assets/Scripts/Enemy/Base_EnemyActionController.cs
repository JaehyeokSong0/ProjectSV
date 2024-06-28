using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Manages actions of the enemy
public abstract class Base_EnemyActionController : MonoBehaviour
{
    private const float _deadRemainTime = 0.5f;

    protected GameObject _player;
    [SerializeField] protected Base_EnemyManager _manager;

    // AnimationContoller class should be downcasted in derived class
    [SerializeField] protected Base_EnemyAnimationController _animationController;
    protected Coroutine _walkCoroutine;

    // Should be initialized in derived class
    // Used to synchronize with attack animation of each enemy classes
    protected float _preAttackTime = 0f;
    protected WaitForSeconds _preAttackTimeWait;

    [SerializeField] protected SpriteRenderer _spriteRenderer;
    private WaitForSeconds _colorChangeTimeWait = new WaitForSeconds(0.1f);

    protected virtual void Awake()
    {
        if (_spriteRenderer == null)
            _spriteRenderer = transform.Find("Model").GetComponent<SpriteRenderer>();
    }

    protected void OnEnable()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player == null)
                Debug.Log("Cannot find Player");
        }
    }

    protected virtual void Start()
    {
        _preAttackTimeWait = new WaitForSeconds(_preAttackTime);

        EventManager.Instance.OnPlayerDead?.AddListener(this.OnPlayerDead);
    }

    public void Initialize()
    {
        _manager.State.IsDead = false;
        _spriteRenderer.color = Color.white;
        Walk(_manager.Data.WalkSpeed);
    }

    public void OnPlayerDead()
    {
        StopAllCoroutines();
        if (_manager.State.IsDead == false)// If not in die animation
            Idle();
    }

    public void Idle()
    {
        _manager.State.MoveState = EnemyMoveState.Idle;
        _animationController.Idle();
    }

    public void Walk(float moveSpeed)
    {
        _walkCoroutine = StartCoroutine(C_Walk(moveSpeed));
    }

    protected IEnumerator C_Walk(float moveSpeed)
    {
        _manager.State.MoveState = EnemyMoveState.Walk;
        _animationController.Walk();

        Vector3 direction;
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player == null)
                Debug.Log("Cannot find Player");
        }
        while (true)
        {
            direction = _player.transform.position - transform.position;

            if (direction.magnitude < _manager.Data.NormalAttackRange)
                yield return C_NormalAttack();

            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }

    public void NormalAttack()
    {
        StartCoroutine(C_NormalAttack());
    }

    protected IEnumerator C_NormalAttack()
    {
        _manager.State.MoveState = EnemyMoveState.Idle;
        _animationController.SetMoveAnimation("Idle");
        _animationController.NormalAttack();

        // Synchronize attack event with attack animation
        yield return _preAttackTimeWait;
        Vector3 direction = _player.transform.position - transform.position;
        if (direction.magnitude < _manager.Data.NormalAttackRange)
            EventManager.Instance.OnPlayerDamaged?.Invoke(_manager.Data.NormalAttackDamage);

        yield return new WaitForSeconds(_manager.Data.NormalAttackSpeed);

        // Reset Move Animation
        direction = _player.transform.position - transform.position;
        if (direction.magnitude < _manager.Data.NormalAttackRange)
            _manager.State.MoveState = EnemyMoveState.Idle;
        else
            _manager.State.MoveState = EnemyMoveState.Walk;
        _animationController.Move();
    }

    public void TakeDamage(float damage) 
    {
        if (_manager.State.IsDead) 
            return;
        ReduceHP(damage);
        StartCoroutine(ChangeSpriteColor(Color.red));
    }

    public void TakeDamage(float damage, float coolTime)
    {
        if (_manager.State.IsDead) 
            return;
        StartCoroutine(ReduceHP(damage, coolTime));
        StartCoroutine(ChangeSpriteColor(Color.red));
    }

    public void Die()
    {
        _manager.State.MoveState = EnemyMoveState.Idle;
        _manager.State.IsDead = true;
        _spriteRenderer.color = Color.white;
        StopCoroutine(_walkCoroutine);
        StartCoroutine(C_Die());
    }

    protected IEnumerator C_Die()
    {
        _animationController.Die();
        float animationTime = _animationController.GetCurrentAnimationLength();

        yield return new WaitForSeconds(animationTime + _deadRemainTime);
        _manager.OnEnemyDead();
    }

    private void ReduceHP(float damage)
    {
        if (_manager.Data.Hp - damage >= 0f)
            _manager.Data.Hp -= damage;
        else
            _manager.Data.Hp = 0f;

        if (_manager.Data.Hp <= 0f)
            Die();
    }

    private IEnumerator ReduceHP(float damage, float coolTime)
    {
        _manager.State.IsAttacked = true;
        if (_manager.Data.Hp - damage >= 0f)
            _manager.Data.Hp -= damage;
        else
            _manager.Data.Hp = 0f;

        if (_manager.Data.Hp <= 0f)
            Die();

        yield return new WaitForSeconds(coolTime);

        _manager.State.IsAttacked = false;
    }

    #region Utility Functions

    private IEnumerator ChangeSpriteColor(Color color)
    {
        Color colorBuffer = _spriteRenderer.color;
        _spriteRenderer.color = color;

        yield return _colorChangeTimeWait;

        _spriteRenderer.color = colorBuffer;
    }
    #endregion
}
