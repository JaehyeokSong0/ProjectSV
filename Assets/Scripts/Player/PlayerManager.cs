using System.Collections;
using UnityEngine;

// Manages Player status and events
public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerInputController _inputController;
    [SerializeField] private PlayerNormalAttackController _normalAttackController;
    [HideInInspector] public Vector2 PlayerDirectionBuffer { get; set; }
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    private WaitForSeconds _colorChangeTimeWait = new WaitForSeconds(1.5f);

    private const float _invincibleTime = 2f;
    private WaitForSeconds _invincibleTimeWait;
    public bool IsInvincible { get; set; }

    public PlayerData Data
    { get; set; }

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

        if (Data.Hp - damage >= 0)
            Data.Hp -= damage;
        else
            Data.Hp = 0;

        StartCoroutine(C_TurnInvincible());
        StartCoroutine(ChangeSpriteColor(Color.red));

        Debug.Log($"Player damaged : {damage} , Remain HP : {Data.Hp}");

        if (Data.Hp == 0)
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
        Debug.Log("Invincible Activated");
        IsInvincible = true;
        yield return _invincibleTimeWait;
        IsInvincible = false;
        Debug.Log("Invincible Deactivated");
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
