using UnityEngine;

public class DeathLordManager : MonoBehaviour
{
    [SerializeField] private EnemyData _data;
    [SerializeField] private DeathLordActionController _actionController;

    private void Awake()
    {
        if (_actionController == null)
            _actionController = GetComponent<DeathLordActionController>();
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _actionController.Walk(_data.WalkSpeed);
    }
}