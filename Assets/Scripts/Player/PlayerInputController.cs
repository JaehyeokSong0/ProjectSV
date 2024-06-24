using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private PlayerManager _manager;
    [SerializeField] private GameObject _playerGO;
    [SerializeField] private PlayerAnimationController _animationController;
    [SerializeField] private PlayerStateManager _stateManager;
    [SerializeField] private PlayerNormalAttackController _normalAttackController;

    private Vector2 _arrowInput;
    private Vector2 _playerDirection;
    private bool _isLeftShiftPressed = false;

    public Vector2 PlayerDirection
    { get { return _playerDirection; } }

    private void Awake()
    {
        if(_manager == null)
            _manager = GetComponentInParent<PlayerManager>();
        if (_playerGO == null)
            _playerGO = GameObject.FindGameObjectWithTag("Player");
        if (_animationController == null)
            _animationController = FindObjectOfType<PlayerAnimationController>();
        if (_stateManager == null)
            _stateManager = FindObjectOfType<PlayerStateManager>();
        if(_normalAttackController == null)
            _normalAttackController = FindObjectOfType<PlayerNormalAttackController>(); 
    }

    private void FixedUpdate()
    {
        // Player Move
        if (_arrowInput != Vector2.zero)
        {
            if (_isLeftShiftPressed == false) // Player Walk
                _playerGO.transform.position += new Vector3(_arrowInput.x, _arrowInput.y, 0f) * _manager.Data.WalkSpeed;
            else // Player Run
                _playerGO.transform.position += new Vector3(_arrowInput.x, _arrowInput.y, 0f) * _manager.Data.RunSpeed;
        }
        else
        {
            if (_stateManager.MoveState != PlayerMoveState.Idle)
                Idle();
        }
    }

    public void OnLeftShiftPressed(InputAction.CallbackContext context)
    {
        if (context.started == true)
            return;

        _isLeftShiftPressed = true;

        if (_stateManager.MoveState != PlayerMoveState.Idle) // IsMoving
            Run();

        if (context.canceled)
        {
            _isLeftShiftPressed = false;
            if (_stateManager.MoveState != PlayerMoveState.Idle)
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

        _manager.SetPlayerDirection(_playerDirection);

        if (_isLeftShiftPressed == false)
            Walk();
        else
            Run();

        if (context.canceled == true)
            Idle();
    }

    private void Idle()
    {
        _stateManager.MoveState = PlayerMoveState.Idle;
        _animationController.Idle();
    }

    private void Walk()
    {
        _stateManager.MoveState = PlayerMoveState.Walk;
        _animationController.Walk(_playerDirection);
    }

    private void Run()
    {
        _stateManager.MoveState = PlayerMoveState.Run;
        _animationController.Run(_playerDirection);
    }
}
