using UnityEngine;

public enum EnemyMoveState // bool parameters in animator
{
    Idle, // Might show only game ends
    Walk,
    Length
}

public enum EnemyActionState // trigger parameters in animator
{
    NormalAttack,
    Jump,
    Hit,
    Length
}

public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] private EnemyMoveState _enemyMoveState;
    public EnemyMoveState MoveState { get { return _enemyMoveState; } set { _enemyMoveState = value; } }
    [HideInInspector] public bool IsDead;
    [HideInInspector] public bool IsAttacked; // If attacked recently, it has invincible time for a while


    private void Awake()
    {
        MoveState = EnemyMoveState.Walk;
    }

    private void OnEnable()
    {
        IsDead = false;
    }
}
