using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerNormalAttackController _normalAttackController;
    private void Awake()
    {
        if (_normalAttackController == null)
            _normalAttackController = GetComponent<PlayerNormalAttackController>();
    }
    public void Initialize()
    {
        _normalAttackController.StartNormalAttack();
    }
}
