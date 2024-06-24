using UnityEngine;

// Manages Player Data
public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerNormalAttackController _normalAttackController;
    private Vector2 _playerDirectionBuffer = Vector2.zero;
    public PlayerData Data
    { get; set; }

    private void Awake()
    {
        Data = ScriptableObject.CreateInstance<PlayerData>();
        if (_normalAttackController == null)
            _normalAttackController = GetComponent<PlayerNormalAttackController>();
    }

    public void Initialize()
    {
        _normalAttackController.StartNormalAttack();
    }

    public void ReduceHP(float damage)
    {
        if(Data.Hp - damage >= 0)
            Data.Hp -= damage;
        else
            Data.Hp = 0;

        if (Data.Hp == 0)
            EventManager.Instance.OnPlayerDead?.Invoke();
    }
    public void SetPlayerDirection(Vector2 direction)
    {
        _playerDirectionBuffer = direction;
    }

    public Vector2 GetPlayerDirection()
    {
        return _playerDirectionBuffer;
    }
}
