using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerData _data;
    [SerializeField] private PlayerAnimationController _animationController;
    [SerializeField] private PlayerStateManager _stateManager;

    private Vector2 _arrowInput;
    private Vector2 _playerDirection;

    private bool _isLeftShiftPressed = false;

    private void Awake()
    {
        if (_player == null)
            _player = GameObject.FindGameObjectWithTag("Player");
        if (_data == null)
            _data = Resources.FindObjectsOfTypeAll(typeof(PlayerData))[0] as PlayerData;
        if (_animationController == null)
            _animationController = FindObjectOfType<PlayerAnimationController>();
        if (_stateManager == null)
            _stateManager = FindObjectOfType<PlayerStateManager>();
    }

    private void FixedUpdate()
    {
        // Player Move
        if (_arrowInput != Vector2.zero)
        {
            if (_isLeftShiftPressed == false) // Player Walk
                _player.transform.position += new Vector3(_arrowInput.x, _arrowInput.y, 0f) * _data.WalkSpeed;
            else // Player Run
                _player.transform.position += new Vector3(_arrowInput.x, _arrowInput.y, 0f) * _data.RunSpeed;
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
