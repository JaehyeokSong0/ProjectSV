using System.Collections;
using UnityEngine;

/// <summary>
/// Manages exp and level of the player
/// </summary>
public class PlayerExpContoller : MonoBehaviour
{
    #region Constant
    private const float EXP_INCREASE_FACTOR = 1.5f;
    #endregion

    #region Field
    [SerializeField] private PlayerManager _manager;
    private int _expLayerMask;
    private Coroutine _expCheckCoroutine = null;
    private float _checkSize = 1f;
    #endregion

    #region Event Method
    private void Awake()
    {
        if(_manager == null )
            _manager = transform.parent.GetComponent<PlayerManager>();

        _expLayerMask = LayerMask.GetMask("Exp");
        // _expLayerMask = 1 << LayerMask.NameToLayer("Exp");
    }

    private void Start()
    {
        
    }
    #endregion

    #region Method
    public void StartCheckDroppedExp()
    {
        _expCheckCoroutine = StartCoroutine(C_StartCheckDroppedExp());
    }

    public void StopCheckDroppedExp()
    {
        StopCoroutine(_expCheckCoroutine);
    }

    private IEnumerator C_StartCheckDroppedExp()
    {
        while (true)
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, _checkSize, _expLayerMask);
            foreach (var hit in hits)
            {
                _manager.Data.CurrentExp += hit.gameObject.GetComponent<Exp>().OnGet(transform);
                CheckLevelUp();
                EventManager.Instance.OnPlayerExpUpdated?.Invoke();
            }
            yield return null;
        }
    }

    private void CheckLevelUp()
    {
        if (_manager.Data.CurrentExp >= _manager.Data.MaxExp)
        {
            EventManager.Instance.OnPlayerLevelUp?.Invoke();
            _manager.Data.Level += 1;
            _manager.Data.MaxExp *= EXP_INCREASE_FACTOR;
            _manager.Data.CurrentExp = 0f;
        }
    }

    #endregion
}
