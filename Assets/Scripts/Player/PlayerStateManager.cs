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

    public PlayerMoveState moveState;
    public bool isInvincible { get; private set; }

    private void Awake()
    {
        moveState = PlayerMoveState.Idle;
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

        isInvincible = true;
        yield return _invincibleTimeWait;
        isInvincible = false;
    }

}
