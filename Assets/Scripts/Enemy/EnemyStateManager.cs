using UnityEngine;

// TODO
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
    public EnemyMoveState MoveState;
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
