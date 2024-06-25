using System.Collections;
using UnityEngine;

// Manages Player status and events
public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerInputController _inputController;
    [SerializeField] private PlayerNormalAttackController _normalAttackController;

    private Vector2 _playerDirectionBuffer = Vector2.down;
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    private WaitForSeconds _colorChangeTimeWait = new WaitForSeconds(2f);

    private const float _invincibleTime = 2f;
    private WaitForSeconds _invincibleTimeWait;
    public bool IsInvincible { get; set; }
    public PlayerData Data { get; set; }
    public Vector2 PlayerDirectionBuffer 
    { 
        get 
        { return _playerDirectionBuffer; }
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
        Data = ScriptableObject.CreateInstance<PlayerData>();
        if(_inputController == null )
            _inputController = transform.Find("InputController").GetComponent<PlayerInputController>();
        if (_normalAttackController == null)
            _normalAttackController = GetComponent<PlayerNormalAttackController>();
        if (_spriteRenderer == null)
            _spriteRenderer = transform.Find("Model").GetComponent<SpriteRenderer>();

        _invincibleTimeWait = new WaitForSeconds(_invincibleTime);
    }

    private void Start()
    {
        EventManager.Instance.OnPlayerDamaged?.AddListener(this.OnPlayerDamaged);
        EventManager.Instance.OnPlayerDead?.AddListener(this.OnPlayerDead);
    }
    #endregion

    #region Event Callback Actions
    public void OnPlayerDamaged(float damage)
    {
        if (IsInvincible == true)
            return;

        if (Data.CurrentHp - damage >= 0)
            Data.CurrentHp -= damage;
        else
            Data.CurrentHp = 0;

        StartCoroutine(C_TurnInvincible());
        StartCoroutine(ChangeSpriteColor(Color.red));

        if (Data.CurrentHp == 0)
            EventManager.Instance.OnPlayerDead?.Invoke();
    }

    public void OnPlayerDead()
    {
        // Die animation is processed in PlayerAnimationController
        _inputController.gameObject.SetActive(false);
    }

    #endregion

    public void Initialize()
    {
        _normalAttackController.StartNormalAttack();
    }

    #region Utility Functions
    private IEnumerator C_TurnInvincible()
    {
        IsInvincible = true;
        yield return _invincibleTimeWait;
        IsInvincible = false;
    }

    private IEnumerator ChangeSpriteColor(Color color)
    {
        Color colorBuffer = _spriteRenderer.color;
        _spriteRenderer.color = color;

        yield return _colorChangeTimeWait;

        _spriteRenderer.color = colorBuffer;
    }
    #endregion
}
