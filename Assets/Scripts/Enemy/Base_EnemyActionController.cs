using System.Collections;
using UnityEngine;

// Manages actions of the enemy
public abstract class Base_EnemyActionController : MonoBehaviour
{
    #region Constant
    private const float DEAD_REMAIN_TIME = 0.5f;
    #endregion

    #region Property
    public abstract Base_EnemyManager Manager { get; set; }
    #endregion

    #region Field
    protected Transform _playerTransform;
    // AnimationContoller class should be downcasted in derived class
    [SerializeField] protected Base_EnemyAnimationController _animationController;
    protected Coroutine _walkCoroutine;
    protected Coroutine _directionUpdateCoroutine;
    // Should be initialized in derived class
    // Used to synchronize with attack animation of each enemy classes
    protected float _preAttackTime = 0f;
    protected WaitForSeconds _preAttackTimeWait;
    protected Vector3 _direction;
    protected WaitForSeconds _directionUpdateWait = new WaitForSeconds(0.5f);
    protected WaitForFixedUpdate _fixedWait = new WaitForFixedUpdate();
    [SerializeField] protected Rigidbody2D _rigidbody;  // TEST CODE
    #endregion

    #region Event Function
    protected void OnEnable()
    {
        if (_playerTransform == null)
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            if (_playerTransform == null)
                Debug.Log("Cannot find Player");
        }
        GetComponent<CircleCollider2D>().enabled = true;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rigidbody.simulated = true;
    }

    protected virtual void Start()
    {
        _preAttackTimeWait = new WaitForSeconds(_preAttackTime);

        EventManager.Instance.OnPlayerDead?.AddListener(this.OnPlayerDead);
    }
    #endregion

    #region Event Callback
    public void OnPlayerDead()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        if (Manager.State.IsDead == false) // If not in die animation
        {
            StopCoroutine(_walkCoroutine);
            Idle();
        }
    }
    #endregion

    #region Method
    public virtual void Initialize()
    {
        Manager.State.IsDead = false;
        Walk(Manager.Data.WalkSpeed);
    }
    public void Idle()
    {
        Manager.State.MoveState = EnemyMoveState.Idle;
        _animationController.Idle();
    }

    public void Walk(float moveSpeed)
    {
        _walkCoroutine = StartCoroutine(C_Walk(moveSpeed));
    }

    protected IEnumerator C_Walk(float moveSpeed)
    {
        Manager.State.MoveState = EnemyMoveState.Walk;
        _animationController.Walk();

        if (_playerTransform == null)
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            if (_playerTransform == null)
                Debug.Log("Cannot find Player");
        }
        _directionUpdateCoroutine = StartCoroutine(C_UpdatePlayerDirectionWithDelay());
        while (true)
        {
            if (_direction.magnitude < Manager.Data.NormalAttackRange)
            {
                yield return C_NormalAttack();
            }

            // transform.position += _direction.normalized * moveSpeed * Time.deltaTime;
            _rigidbody.MovePosition(transform.position + _direction.normalized * moveSpeed * Time.deltaTime);
            yield return _fixedWait;
        }
    }

    protected IEnumerator C_UpdatePlayerDirectionWithDelay()
    {
        while (true)
        {
            _direction = _playerTransform.position - transform.position;
            yield return _directionUpdateWait;
        }

    }
    public void NormalAttack()
    {
        StartCoroutine(C_NormalAttack());
    }

    protected IEnumerator C_NormalAttack()
    {
        _rigidbody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        Manager.State.MoveState = EnemyMoveState.Idle;
        //_animationController.SetMoveAnimation("Idle"); // Prevent walk animation after attack
        _animationController.Idle();
        _animationController.NormalAttack();

        // Synchronize attack event with attack animation
        yield return _preAttackTimeWait;

        Vector3 direction = _playerTransform.position - transform.position;

        if (direction.magnitude < Manager.Data.NormalAttackRange)
            EventManager.Instance.OnPlayerDamaged?.Invoke(Manager.Data.NormalAttackDamage);

        yield return new WaitForSeconds(Manager.Data.NormalAttackSpeed);
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Reset Move Animation
        direction = _playerTransform.position - transform.position;
        if (direction.magnitude < Manager.Data.NormalAttackRange)
            Manager.State.MoveState = EnemyMoveState.Idle;
        else
            Manager.State.MoveState = EnemyMoveState.Walk;
        _animationController.Move();
    }

    public void TakeDamage(float damage)
    {
        if (Manager.State.IsDead == true)
            return;
        ReduceHP(damage);
        _animationController.ChangeSpriteColor(Color.red);
    }

    public void TakeDamage(float damage, float coolTime)
    {
        if (Manager.State.IsDead == true)
            return;
        StartCoroutine(ReduceHP(damage, coolTime));
        _animationController.ChangeSpriteColor(Color.red);
    }

    public void Die()
    {
        Manager.State.MoveState = EnemyMoveState.Idle;
        Manager.State.IsDead = true;

        GetComponent<CircleCollider2D>().enabled = false;
        _rigidbody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        _rigidbody.simulated = false;
        StopCoroutine(_walkCoroutine);
        StopCoroutine(_directionUpdateCoroutine);
        StartCoroutine(C_Die());
    }

    protected IEnumerator C_Die()
    {
        _animationController.Die();
        float animationTime = _animationController.GetCurrentAnimationLength();

        yield return new WaitForSeconds(animationTime + DEAD_REMAIN_TIME);
        Manager.OnEnemyDead();
    }

    private void ReduceHP(float damage)
    {
        if (Manager.Data.Hp - damage >= 0f)
            Manager.Data.Hp -= damage;
        else
            Manager.Data.Hp = 0f;

        if (Manager.Data.Hp <= 0f)
            Die();
    }

    private IEnumerator ReduceHP(float damage, float coolTime)
    {
        Manager.State.IsAttacked = true;

        if (Manager.Data.Hp - damage >= 0f)
            Manager.Data.Hp -= damage;
        else
            Manager.Data.Hp = 0f;

        if (Manager.Data.Hp <= 0f)
            Die();

        yield return new WaitForSeconds(coolTime);

        Manager.State.IsAttacked = false;
    }
    #endregion
}
