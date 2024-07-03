using System.Collections;
using UnityEngine;

// Manages actions of the enemy
public abstract class Base_EnemyActionController : MonoBehaviour
{
    private const float DEAD_REMAIN_TIME = 0.5f;

    [SerializeField] protected Base_EnemyManager manager;
    protected GameObject playerGO;

    // AnimationContoller class should be downcasted in derived class
    [SerializeField] protected Base_EnemyAnimationController animationController;
    protected Coroutine walkCoroutine;

    // Should be initialized in derived class
    // Used to synchronize with attack animation of each enemy classes
    protected float preAttackTime = 0f;
    protected WaitForSeconds preAttackTimeWait;

    #region Event Functions
    protected void OnEnable()
    {
        if (playerGO == null)
        {
            playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO == null)
                Debug.Log("Cannot find Player");
        }
    }

    protected virtual void Start()
    {
        preAttackTimeWait = new WaitForSeconds(preAttackTime);

        EventManager.instance.OnPlayerDead?.AddListener(this.OnPlayerDead);
    }
    #endregion
    public virtual void Initialize()
    {
        manager.state.isDead = false;
        Walk(manager.data.WalkSpeed);
    }

    #region Event Callback Actions
    public void OnPlayerDead()
    {
        if (manager.state.isDead == false) // If not in die animation
        {
            StopCoroutine(walkCoroutine);
            Idle();
        }
    }
    #endregion
    public void Idle()
    {
        manager.state.MoveState = EnemyMoveState.Idle;
        animationController.Idle();
    }

    public void Walk(float moveSpeed)
    {
        walkCoroutine = StartCoroutine(C_Walk(moveSpeed));
    }

    protected IEnumerator C_Walk(float moveSpeed)
    {
        manager.state.MoveState = EnemyMoveState.Walk;
        animationController.Walk();

        if (playerGO == null)
        {
            playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO == null)
                Debug.Log("Cannot find Player");
        }
        while (true)
        {
            Vector3 direction = playerGO.transform.position - transform.position;
            if (direction.magnitude < manager.data.NormalAttackRange)
            {
                yield return C_NormalAttack();
            }

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
        manager.state.MoveState = EnemyMoveState.Idle;
        //_animationController.SetMoveAnimation("Idle"); // Prevent walk animation after attack
        animationController.Idle();
        animationController.NormalAttack();

        // Synchronize attack event with attack animation
        yield return preAttackTimeWait;

        Vector3 direction = playerGO.transform.position - transform.position;

        if (direction.magnitude < manager.data.NormalAttackRange)
            EventManager.instance.OnPlayerDamaged?.Invoke(manager.data.NormalAttackDamage);

        yield return new WaitForSeconds(manager.data.NormalAttackSpeed);

        // Reset Move Animation
        direction = playerGO.transform.position - transform.position;
        if (direction.magnitude < manager.data.NormalAttackRange)
            manager.state.MoveState = EnemyMoveState.Idle;
        else
            manager.state.MoveState = EnemyMoveState.Walk;
        animationController.Move();
    }

    public void TakeDamage(float damage)
    {
        if (manager.state.isDead == true)
            return;
        ReduceHP(damage);
        animationController.ChangeSpriteColor(Color.red);
    }

    public void TakeDamage(float damage, float coolTime)
    {
        if (manager.state.isDead == true)
            return;
        StartCoroutine(ReduceHP(damage, coolTime));
        animationController.ChangeSpriteColor(Color.red);
    }

    public void Die()
    {
        manager.state.MoveState = EnemyMoveState.Idle;
        manager.state.isDead = true;

        StopCoroutine(walkCoroutine);
        StartCoroutine(C_Die());
    }

    protected IEnumerator C_Die()
    {
        animationController.Die();
        float animationTime = animationController.GetCurrentAnimationLength();

        yield return new WaitForSeconds(animationTime + DEAD_REMAIN_TIME);
        manager.OnEnemyDead();
    }

    private void ReduceHP(float damage)
    {
        if (manager.data.Hp - damage >= 0f)
            manager.data.Hp -= damage;
        else
            manager.data.Hp = 0f;

        if (manager.data.Hp <= 0f)
            Die();
    }

    private IEnumerator ReduceHP(float damage, float coolTime)
    {
        manager.state.IsAttacked = true;

        if (manager.data.Hp - damage >= 0f)
            manager.data.Hp -= damage;
        else
            manager.data.Hp = 0f;

        if (manager.data.Hp <= 0f)
            Die();

        yield return new WaitForSeconds(coolTime);

        manager.state.IsAttacked = false;
    }
}
