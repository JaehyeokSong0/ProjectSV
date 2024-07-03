using UnityEngine;

// Manages game flow
public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;

    private void Awake()
    {
        if (_playerManager == null)
            _playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
    }

    private void Start()
    {
        _playerManager.Initialize();
    }
}