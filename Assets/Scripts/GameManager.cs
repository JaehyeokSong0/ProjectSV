using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerManager _playerManager;

    private void Awake()
    {
        if (_playerManager == null)
            _playerManager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
    }

    private void Start()
    {
        _playerManager.Initialize();
    }
}