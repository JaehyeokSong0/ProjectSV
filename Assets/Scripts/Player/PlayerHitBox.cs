using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D)), RequireComponent (typeof(Rigidbody2D))]
public class PlayerHitBox : MonoBehaviour
{
    [SerializeField] private PlayerManager _manager;
    [SerializeField] private BoxCollider2D _hitBox;
    [SerializeField] private GameObject _effectGO;

    private bool _isDirectionLocked = false; // Same as isAttacking
    private float _coolTime = 0f;

    private void Awake()
    {
        if(_manager == null)
            _manager = FindObjectOfType<PlayerManager>();
        if (_hitBox == null )
            _hitBox = GetComponent<BoxCollider2D>();
        if (_effectGO == null)
            _effectGO = transform.Find("Effect").gameObject;
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

    public void SetHitBoxOffset(Vector2 direction, float coolTime)
    {
        _coolTime = coolTime;

        // Show normalAttack effect
        _effectGO.transform.localPosition = direction.normalized;
        
        StartCoroutine(C_SetHitBoxOffset(direction));
    }

    private IEnumerator C_SetHitBoxOffset(Vector2 direction)
    {
        if (_isDirectionLocked == false)
        {   
            _isDirectionLocked = true;
            _hitBox.offset = direction.normalized;
            _effectGO.gameObject.SetActive(true);
            yield return new WaitForSeconds(_coolTime);
            _effectGO.gameObject.SetActive(false);
            _isDirectionLocked = false;
        }
    }
}
