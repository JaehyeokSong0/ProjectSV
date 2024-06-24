using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerHitBox : MonoBehaviour
{
    [SerializeField] private PlayerManager _manager;
    [SerializeField] private BoxCollider2D _hitBox;

    private WaitForSeconds _coolTime = null;

    private bool _isDirectionLocked = false; // Same as isAttacking

    private void Awake()
    {
        if(_manager == null)
            _manager = FindObjectOfType<PlayerManager>();
        if (_hitBox == null )
            _hitBox = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        _coolTime = new WaitForSeconds(_manager.Data.NormalAttackSpeed);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_isDirectionLocked == true)
        {
            if (collision.gameObject.CompareTag("Enemy") == true)
            {
                collision.gameObject.GetComponent<EnemyManager>()?.OnEnemyDamaged(_manager.Data.NormalAttackDamage, _manager.Data.NormalAttackSpeed);
            }
        }
    }

    public void SetHitBoxOffset()
    {
        StartCoroutine(C_SetHitBoxOffset());
    }

    private IEnumerator C_SetHitBoxOffset()
    {
        Vector2 direction = _manager.GetPlayerDirection();

        if (_isDirectionLocked == false)
        {
            _isDirectionLocked = true;
            _hitBox.offset = direction.normalized;

            yield return _coolTime;

            _isDirectionLocked = false;
        }
    }
}
