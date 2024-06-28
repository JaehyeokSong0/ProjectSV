using UnityEngine;
using System.Collections;
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
    private WaitForSeconds _invincibleTimeWait = null;
    private float _invincibleTimeBuffer = 0f;

    public PlayerMoveState MoveState;
    public bool IsInvincible { get; private set; }

    private void Awake()
    {
        MoveState = PlayerMoveState.Idle;
    }

    public void SetInvincible(float time)
    {
        StartCoroutine(C_SetInvincible(time));
    }

    private IEnumerator C_SetInvincible(float time)
    {
        if (time != _invincibleTimeBuffer)
        {
            _invincibleTimeBuffer = time;
            _invincibleTimeWait = new WaitForSeconds(time);
        }

        IsInvincible = true;
        yield return _invincibleTimeWait;
        IsInvincible = false;
    }

}
