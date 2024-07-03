using UnityEngine;

// Manages Player status and events
public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerInputController _inputController;
    [SerializeField] private PlayerStateManager _stateManager;
    [SerializeField] private PlayerData _data;
    [SerializeField] private PlayerNormalAttackController _normalAttackController;
    [SerializeField] private PlayerSkillController _skillController;
    [SerializeField] private PlayerAnimationController _animationController;

    private Vector2 _playerDirectionBuffer = Vector2.down;

    public PlayerStateManager state { get { return _stateManager; } }
    public PlayerData data { get { return _data; } }
    public Vector2 playerDirectionBuffer 
    { 
        get 
        { 
            return _playerDirectionBuffer; 
        }
        set 
        { 
            if (value == Vector2.zero) 
                _playerDirectionBuffer = Vector2.down; 
            else 
                _playerDirectionBuffer = value; 
        } 
    }

    #region Event Functions
    private void Awake()
    {
        _data = ScriptableObject.CreateInstance<PlayerData>();

        if (_inputController == null )
            _inputController = transform.Find("InputController").GetComponent<PlayerInputController>();
        if (_stateManager == null)
            _stateManager = GetComponent<PlayerStateManager>();       
        if (_normalAttackController == null)
            _normalAttackController = transform.Find("NormalAttackController").GetComponent<PlayerNormalAttackController>();
        if (_skillController == null)
            _skillController = transform.Find("SkillController").GetComponent<PlayerSkillController>();
        if (_animationController == null)
            _animationController = transform.Find("Model").GetComponent<PlayerAnimationController>();
    }

    private void Start()
    {
        EventManager.instance.OnPlayerDamaged?.AddListener(this.OnPlayerDamaged);
        EventManager.instance.OnPlayerDead?.AddListener(this.OnPlayerDead);
    }
    #endregion

    #region Event Callback Actions
    public void OnPlayerDamaged(float damage)
    {
        if (state.isInvincible == true)
            return;

        state.SetInvincible(data.invincibleTime);
        if (data.currentHp - damage >= 0f)
            data.currentHp -= damage;
        else
            data.currentHp = 0f;

        _animationController.ChangeSpriteColor(Color.red);

        if (data.currentHp <= 0f)
            EventManager.instance.OnPlayerDead?.Invoke();
    }

    public void OnPlayerDead()
    {
        _inputController.gameObject.SetActive(false);
        _normalAttackController.gameObject.SetActive(false);
        _skillController.gameObject.SetActive(false);
        _animationController.Die();
    }
    #endregion

    public void Initialize()
    {
        _normalAttackController.StartNormalAttack();
    }
}
