using System.Collections;
using UnityEngine;

// Manages actions of the enemy
public abstract class Base_EnemyActionController : MonoBehaviour
{
    private const float _deadRemainTime = 0.5f;

    protected GameObject _player;
    [SerializeField] protected EnemyStateManager _stateManager;
    [SerializeField] protected Base_EnemyManager _manager;

    // AnimationContoller class should be downcasted in derived class
    [SerializeField] protected Base_EnemyAnimationController _animationController;
    protected Coroutine _walkCoroutine;

    [SerializeField] protected SpriteRenderer _spriteRenderer;
    private WaitForSeconds _colorChangeTimeWait = new WaitForSeconds(0.5f);


    protected virtual void Awake()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player == null)
                Debug.Log("Cannot find Player");
        }
        if (_spriteRenderer == null)
            _spriteRenderer = transform.Find("Model").GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        EventManager.Instance.OnPlayerDead?.AddListener(this.OnPlayerDead);
    }

    public void OnPlayerDead()
    {
        StopAllCoroutines();
        Idle();
    }

    public virtual void Idle()
    {
        _stateManager.MoveState = EnemyMoveState.Idle;
        _animationController.Idle();
    }

    public virtual void Walk(float moveSpeed)
    {
        _walkCoroutine = StartCoroutine(C_Walk(moveSpeed));
    }

    protected IEnumerator C_Walk(float moveSpeed)
    {
        _stateManager.MoveState = EnemyMoveState.Walk;
        _animationController.Walk();

        Vector3 direction;

        while (true)
        {
            direction = _player.transform.position - transform.position;

            if (direction.magnitude < _manager.Data.NormalAttackRange)
                yield return C_NormalAttack();

            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }

    public virtual void NormalAttack()
    {
        StartCoroutine(C_NormalAttack());
    }

    protected IEnumerator C_NormalAttack()
    {
        _stateManager.MoveState = EnemyMoveState.Idle;
        _animationController.SetMoveAnimation("Idle");

        _animationController.NormalAttack();
        EventManager.Instance.OnPlayerDamaged.Invoke(_manager.Data.NormalAttackDamage);

        yield return new WaitForSeconds(_manager.Data.NormalAttackSpeed);

        // Reset Move Animation
        Vector3 direction = _player.transform.position - transform.position;
        if (direction.magnitude < _manager.Data.NormalAttackRange)
            _stateManager.MoveState = EnemyMoveState.Idle;
        else
            _stateManager.MoveState = EnemyMoveState.Walk;
        _animationController.Move();
    }

    public virtual void TakeDamage(float damage, float coolTime)
    {
        StartCoroutine(ReduceHP(damage, coolTime));
        StartCoroutine(ChangeSpriteColor(Color.red));
    }

    public virtual void Die()
    {
        _stateManager.MoveState = EnemyMoveState.Idle;
        StopCoroutine(_walkCoroutine);
        StartCoroutine(C_Die());
    }

    protected IEnumerator C_Die()
    {
        _animationController.Die();
        float animationTime = _animationController.GetCurrentAnimationLength();

        yield return new WaitForSeconds(animationTime);
        Destroy(gameObject, _deadRemainTime);
    }

    private IEnumerator ReduceHP(float damage, float coolTime)
    {
        _manager.IsAttacked = true;
        if (_manager.Data.Hp - damage >= 0)
            _manager.Data.Hp -= damage;
        else
            _manager.Data.Hp = 0;

        if (_manager.Data.Hp <= 0)
            Die();

        yield return new WaitForSeconds(coolTime);

        _manager.IsAttacked = false;
    }

    private IEnumerator ChangeSpriteColor(Color color)
    {
        Color colorBuffer = _spriteRenderer.color;
        _spriteRenderer.color = color;

        yield return _colorChangeTimeWait;

        _spriteRenderer.color = colorBuffer;
    }
}
