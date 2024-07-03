using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private PlayerManager _manager;
    [SerializeField] private GameObject _playerGO;
    [SerializeField] private PlayerAnimationController _animationController;
    [SerializeField] private PlayerNormalAttackController _normalAttackController;
    [SerializeField] private PlayerSkillController _skillController;

    private Vector2 _arrowInput;
    private Vector2 _playerDirection;
    private bool _isLeftShiftPressed = false;

    private void Awake()
    {
        if(_manager == null)
            _manager = GetComponentInParent<PlayerManager>();
        if (_playerGO == null)
            _playerGO = GameObject.FindGameObjectWithTag("Player");
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
            if (_isLeftShiftPressed == false) // Player Walk
                _playerGO.transform.position += new Vector3(_arrowInput.x, _arrowInput.y, 0f) * _manager.data.walkSpeed;
            else // Player Run
                _playerGO.transform.position += new Vector3(_arrowInput.x, _arrowInput.y, 0f) * _manager.data.runSpeed;
        }
        else
        {
            if (_manager.state.moveState != PlayerMoveState.Idle)
                Idle();
        }
    }

    public void OnLeftShiftPressed(InputAction.CallbackContext context)
    {
        if (context.started == true)
            return;

        _isLeftShiftPressed = true;

        if (_manager.state.moveState != PlayerMoveState.Idle) // IsMoving
            Run();

        if (context.canceled)
        {
            _isLeftShiftPressed = false;
            if (_manager.state.moveState != PlayerMoveState.Idle)
                Walk();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started == true)
            return;

        _arrowInput = context.ReadValue<Vector2>();
        if (_arrowInput != Vector2.zero)
            _playerDirection = _arrowInput;

        _manager.playerDirectionBuffer = _playerDirection;

        if (_isLeftShiftPressed == false)
            Walk();
        else
            Run();

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
        _manager.state.moveState = PlayerMoveState.Idle;
        _animationController.Idle();
    }

    private void Walk()
    {
        _manager.state.moveState = PlayerMoveState.Walk;
        _animationController.Walk(_playerDirection);
    }

    private void Run()
    {
        _manager.state.moveState = PlayerMoveState.Run;
        _animationController.Run(_playerDirection);
    }
}
