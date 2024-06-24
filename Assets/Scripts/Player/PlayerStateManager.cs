using UnityEngine;

public enum PlayerMoveState // bool parameters in animator
{
    Idle,
    Walk,
    Run,
    Length
}

public enum PlayerActionState // trigger parameters in animator
{
    NormalAttack,
    Spell,
    Hit,
    Length
}

public class PlayerStateManager : MonoBehaviour
{
    public PlayerMoveState MoveState { get; set; }

    private void Awake()
    {
        MoveState = PlayerMoveState.Idle;
    }
}
