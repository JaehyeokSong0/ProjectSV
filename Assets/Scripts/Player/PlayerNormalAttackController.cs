using System.Collections;
using UnityEngine;

public class PlayerNormalAttackController : MonoBehaviour
{
    // Correction factors for attack animation time
    private const float PRE_ATTACK_TIME = 0.2f;
    private const float POST_ATTACK_TIME = 0.1f;
    private readonly Vector2 HITBOX_SIZE = new Vector2(1.2f, 1.2f);

    [SerializeField] private PlayerManager _manager;
    [SerializeField] private PlayerAnimationController _animationController;
    [SerializeField] private GameObject _effectGO;

    private float _attackSpeed;
    private WaitForSeconds _attackSpeedWait = null;
    private WaitForSeconds _preAttackDelayWait = new WaitForSeconds(0.2f); // Before attack delay.

    private int _enemyLayerMask;

    private WaitForSeconds _preAttackTimeWait;
    private Vector3 _gizmoDirection;

    private void Awake()
    {
        if (_manager == null)
            _manager = gameObject.GetComponentInParent<PlayerManager>();
        if (_animationController == null)
            _animationController = FindObjectOfType<PlayerAnimationController>();
        if (_effectGO == null)
            _effectGO = transform.Find("Effect").gameObject;

        _preAttackTimeWait = new WaitForSeconds(PRE_ATTACK_TIME);
        _enemyLayerMask = LayerMask.GetMask("Enemy");
    }

    private void Start()
    {
        _attackSpeed = _manager.Data.NormalAttackSpeed;
        _attackSpeedWait = new WaitForSeconds(_attackSpeed);
    }

    private IEnumerator NormalAttack()
    {
        yield return _preAttackDelayWait;

        while (true)
        {
            _animationController.NormalAttack();

            Vector3 direction = _manager.PlayerDirectionBuffer.normalized;
            float rotationValue = 90f + Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Vector3 rotationVector = Vector3.forward * (90f + Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            // Set effect transform and play
            _effectGO.transform.position = transform.position + direction;
            _effectGO.transform.rotation = Quaternion.Euler(rotationVector);
            PlayEffect(_animationController.GetCurrentAnimationLength() - POST_ATTACK_TIME);

            _gizmoDirection = direction;
            yield return _preAttackTimeWait;

            /*
            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position + direction, HITBOX_SIZE, rotationValue, Vector2.zero, 0f, _enemyLayer);
            foreach (var hit in hits)
            {
                hit.collider.gameObject.GetComponent<Base_EnemyManager>().OnEnemyDamaged(_manager.Data.NormalAttackDamage);
            }*/
            var hits = Physics2D.OverlapBoxAll(transform.position + direction, HITBOX_SIZE, rotationValue, _enemyLayerMask);
            foreach (var hit in hits)
            {
                hit.gameObject.GetComponent<Base_EnemyManager>().OnEnemyDamaged(_manager.Data.NormalAttackDamage);
            }
            yield return _attackSpeedWait;
            _effectGO.SetActive(false);

        }
    }

    public void StartNormalAttack()
    {
        StartCoroutine(NormalAttack());
    }

    public void PauseNormalAttack()
    {
        StopCoroutine(NormalAttack());
    }

    private void PlayEffect(float time)
    {
        StartCoroutine(C_PlayEffect(time));
    }

    private IEnumerator C_PlayEffect(float time)
    {
        _effectGO.SetActive(true);

        yield return new WaitForSeconds(time);
        _effectGO.SetActive(false);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + _gizmoDirection, HITBOX_SIZE);
    }
#endif
}
