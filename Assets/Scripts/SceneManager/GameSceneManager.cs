using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private InputManager _inputManager;
    
    private void Awake()
    {
        Application.targetFrameRate = 180;
        
        if (_playerManager == null)
            _playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        if(_enemySpawner == null)
            _enemySpawner = FindFirstObjectByType<EnemySpawner>();
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
        _enemySpawner.StartCreateEnemy();
    }
}