using System.Collections;
using UnityEngine;

public abstract class Base_EnemyActionController : MonoBehaviour
{
    protected GameObject _player;
    [SerializeField] protected EnemyStateManager _stateManager;
    [SerializeField] protected EnemyData _enemyData;

    // AnimationContoller class should be downcasted in derived class
    [SerializeField] protected Base_EnemyAnimationController _animationController;
    
    protected virtual void Start()
    {
        if(_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player == null)
                Debug.Log("Cannot find Player");
        }
    }

    public virtual void Walk(float moveSpeed)
    {
        StartCoroutine(C_Walk(moveSpeed));
    }

    protected virtual IEnumerator C_Walk(float moveSpeed)
    {        
        _stateManager.MoveState = EnemyMoveState.Walk;
        _animationController.Walk();

        Vector3 direction;
        
        while (true)
        {
            direction = _player.transform.position - transform.position;

            if (direction.magnitude < _enemyData.NormalAttackRange)
                yield return C_NormalAttack();

            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }

    public virtual void NormalAttack()
    {        
        StartCoroutine(C_NormalAttack());
    }

    protected virtual IEnumerator C_NormalAttack()
    {
        _stateManager.MoveState = EnemyMoveState.Idle;
        _animationController.SetMoveAnimation("Idle");

        _animationController.NormalAttack();

        yield return new WaitForSeconds(_enemyData.NormalAttackSpeed);

        // Reset Move Animation
        Vector3 direction = _player.transform.position - transform.position;
        if (direction.magnitude < _enemyData.NormalAttackRange)
            _stateManager.MoveState = EnemyMoveState.Idle;
        else
            _stateManager.MoveState = EnemyMoveState.Walk;
        _animationController.Move();
    }
}
