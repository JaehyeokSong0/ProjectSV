using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerHitBox : MonoBehaviour
{
    [SerializeField] private PlayerManager _manager;
    [SerializeField] private BoxCollider2D _hitBox;

    private bool _isDirectionLocked = false; // Same as isAttacking
    private float _coolTime = 0f;

    private void Awake()
    {
        if(_manager == null)
            _manager = FindObjectOfType<PlayerManager>();
        if (_hitBox == null )
            _hitBox = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_isDirectionLocked == true)
        {
            if (collision.gameObject.CompareTag("Enemy") == true)
            {
                collision.gameObject.GetComponent<Base_EnemyManager>()?.OnEnemyDamaged(_manager.Data.NormalAttackDamage, _coolTime);
            }
        }
    }

    public void SetHitBoxOffset(float coolTime)
    {
        _coolTime = coolTime;
        StartCoroutine(C_SetHitBoxOffset());
    }

    private IEnumerator C_SetHitBoxOffset()
    {
        if (_isDirectionLocked == false)
        {                
            Vector2 direction = _manager.GetPlayerDirection();

            _isDirectionLocked = true;
            _hitBox.offset = direction.normalized;

            yield return new WaitForSeconds(_coolTime);

            _isDirectionLocked = false;
        }
    }
}
