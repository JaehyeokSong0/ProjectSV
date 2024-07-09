using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerManager _manager;
    [SerializeField] private PlayerAnimationController _animationController;
    [SerializeField] private PlayerNormalAttackController _normalAttackController;
    [SerializeField] private PlayerSkillController _skillController;

    private Vector2 _arrowInput;
    private Vector2 _playerDirection;

    private void Awake()
    {
        if (_player == null)
            _player = transform.root.gameObject;
        if(_manager == null)
            _manager = GetComponentInParent<PlayerManager>();
        if (_animationController == null)
            _animationController = FindObjectOfType<PlayerAnimationController>();
        if(_normalAttackController == null)
            _normalAttackController = FindObjectOfType<PlayerNormalAttackController>(); 
    }

    private void FixedUpdate()
    {
        // Player Move
        if (_arrowInput != Vector2.zero)
        {
            _player.transform.position += new Vector3(_arrowInput.x, _arrowInput.y, 0f) * _manager.Data.WalkSpeed;
        }
        else
        {
            if (_manager.State.MoveState != PlayerMoveState.Idle)
                Idle();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started == true)
            return;

        _arrowInput = context.ReadValue<Vector2>();
        if (_arrowInput != Vector2.zero)
            _playerDirection = _arrowInput;

        _manager.PlayerDirectionBuffer = _playerDirection;
 
        Walk();

        if (context.canceled == true)
            Idle();
    }

    public void OnKeyQPressed(InputAction.CallbackContext context)
    {
        if (context.performed == true)
            _skillController.CastSkill();
    }

    private void Idle()
    {
        _manager.State.MoveState = PlayerMoveState.Idle;
        _animationController.Idle();
    }

    private void Walk()
    {
        _manager.State.MoveState = PlayerMoveState.Walk;
        _animationController.Walk(_playerDirection);
    }
}
