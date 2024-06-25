using System.Collections;
using UnityEngine;

public class PlayerNormalAttackController : MonoBehaviour
{
    [SerializeField] private PlayerManager _manager;
    [SerializeField] private PlayerAnimationController _animationController;
    [SerializeField] private PlayerHitBox _hitBox;

    private float _attackSpeed;
    private WaitForSeconds _attackSpeedWait = null;
    private WaitForSeconds _attackDelay = new WaitForSeconds(1f);

    // Correction factors for attack animation time
    private const float _preAttackTime = 0.2f;
    private const float _postAttackTime = 0.1f;
    private WaitForSeconds _preAttackTimeWait;

    private void Awake()
    {
        if(_manager == null)
            _manager = gameObject.GetComponentInParent<PlayerManager>();
        if (_animationController == null)
            _animationController = FindObjectOfType<PlayerAnimationController>();
        if (_hitBox == null)
            _hitBox = GameObject.Find("HitBox").GetComponent<PlayerHitBox>();

        _preAttackTimeWait = new WaitForSeconds(_preAttackTime);
    }

    private void Start()
    {
        _attackSpeed = _manager.Data.NormalAttackSpeed;
        _attackSpeedWait = new WaitForSeconds(_attackSpeed);
    }

    private IEnumerator NormalAttack()
    {
        yield return _attackDelay;

        while (true)
        {
            _animationController.NormalAttack();
            yield return _preAttackTimeWait;
            _hitBox.SetHitBoxOffset(_animationController.GetCurrentAnimationLength() - _postAttackTime);
            yield return _attackSpeedWait;
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
}
