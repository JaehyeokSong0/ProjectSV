using System.Collections;
using UnityEngine;

public class PlayerExpContoller : MonoBehaviour
{
    #region Field
    [SerializeField] private PlayerManager _manager;
    private int _expLayerMask;
    private Coroutine _expCheckCoroutine = null;
    private Vector2 _checkSize = new Vector2(2f, 2f);
    public float test; // TEST CODE
    #endregion

    #region Event Method
    private void Awake()
    {
        if(_manager == null )
            _manager = transform.parent.GetComponent<PlayerManager>();

        _expLayerMask = LayerMask.GetMask("Exp");
        // _expLayerMask = 1 << LayerMask.NameToLayer("Exp");
    }
    private void Update()
    {
        test = _manager.Data.CurrentExp;
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
            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, _checkSize, 0f, Vector2.zero, 0f, _expLayerMask);
            foreach (var hit in hits)
            {
                _manager.Data.CurrentExp += hit.collider.gameObject.GetComponent<Exp>().OnGet(transform);
                CheckLevelUp();
            }
            yield return null;
        }
    }

    private void CheckLevelUp()
    {
        if (_manager.Data.CurrentExp >= _manager.Data.MaxExp)
        {
            _manager.Data.Level += 1;
            _manager.Data.MaxExp *= 2f;
            _manager.Data.CurrentExp = 0f;
            Debug.Log($"LV UP : {_manager.Data.Level} , next EXP : {_manager.Data.MaxExp}");
        }
    }
    #endregion
}
