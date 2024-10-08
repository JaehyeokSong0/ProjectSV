using System.Collections;
using UnityEngine;

// Manages Player status and events
public class PlayerManager : MonoBehaviour
{
    public PlayerStateManager State { get { return _stateManager; } }
    public PlayerData Data { get { return _data; } }
    public Vector2 PlayerDirectionBuffer 
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

    [SerializeField] private PlayerInputController _inputController;
    [SerializeField] private PlayerStateManager _stateManager;
    [SerializeField] private PlayerData _data;
    [SerializeField] private PlayerNormalAttackController _normalAttackController;
    [SerializeField] private PlayerSkillController _skillController;
    [SerializeField] private PlayerAnimationController _animationController;
    [SerializeField] private PlayerExpContoller _expController;

    private Vector2 _playerDirectionBuffer = Vector2.down;

    #region Event Functions
    private void Awake()
    {
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
        if (_expController == null)
            _expController = transform.Find("ExpController").GetComponent<PlayerExpContoller>();
    }
    private void OnEnable()
    {
        _data = ScriptableObject.CreateInstance<PlayerData>();
    }

    private void Start()
    {
        EventManager.Instance.OnPlayerDamaged?.AddListener(this.OnPlayerDamaged);
        EventManager.Instance.OnPlayerDead?.AddListener(this.OnPlayerDead);

        StartCoroutine(C_ZoomOutCamera());
    }
    #endregion

    #region Event Callback Actions
    public void OnPlayerDamaged(float damage)
    {
        if (State.IsInvincible == true)
            return;

        State.SetInvincible(Data.InvincibleTime);
        if (Data.CurrentHp - damage >= 0f)
            Data.CurrentHp -= damage;
        else
            Data.CurrentHp = 0f;

        EventManager.Instance.OnPlayerHPUpdated?.Invoke();
        _animationController.ChangeSpriteColor(Color.red);

        if (Data.CurrentHp <= 0f)
            EventManager.Instance.OnPlayerDead?.Invoke();
    }

    public void OnPlayerDead()
    {
        _inputController.gameObject.SetActive(false);
        _normalAttackController.gameObject.SetActive(false);
        _skillController.gameObject.SetActive(false);
        _animationController.Die();
    }
    #endregion

    #region Method
    public void Initialize()
    {
        _normalAttackController.StartNormalAttack();
        _expController.StartCheckDroppedExp();
    }
    private IEnumerator C_ZoomOutCamera()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        Camera playerCamera = Camera.main;
        float sizeBuffer = playerCamera.orthographicSize;

        const float zoomSpeed = 0.5f;
        const float objectiveSize = 5f;
        float elapsedTime = 0f;
        while(playerCamera.orthographicSize < objectiveSize)
        {
            playerCamera.orthographicSize = Mathf.SmoothStep(sizeBuffer, objectiveSize, elapsedTime * zoomSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    #endregion
}
