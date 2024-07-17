using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private GameLevelController _levelController;
    [SerializeField] private InputManager _inputManager;
    
    private void Awake()
    {
        Application.targetFrameRate = 180;
        
        if (_playerManager == null)
            _playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        if(_levelController == null)
            _levelController = FindFirstObjectByType<GameLevelController>();
        if (_inputManager == null)
            _inputManager = FindFirstObjectByType<InputManager>();
    }

    private void OnEnable()
    {
        _inputManager.SwitchActionMap(InputManager.ActionMapType.PlayerAction);
    }

    private void Start()
    {
        _playerManager.Initialize();
        _levelController.StartLevelControl();
    }
}