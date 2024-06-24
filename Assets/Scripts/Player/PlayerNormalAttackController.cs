using System.Collections;
using UnityEngine;

public class PlayerNormalAttackController : MonoBehaviour
{
    [SerializeField] private PlayerManager _manager;
    [SerializeField] private PlayerAnimationController _animationController;
    [SerializeField] private PlayerHitBox _hitBox;

    private float _attackSpeed;
    private WaitForSeconds _coolTime = null;
    private WaitForSeconds _attackDelay = new WaitForSeconds(1f);

    private void Awake()
    {
        if(_manager == null)
            _manager = gameObject.GetComponentInParent<PlayerManager>();
        if (_animationController == null)
            _animationController = FindObjectOfType<PlayerAnimationController>();
        if (_hitBox == null)
            _hitBox = GameObject.Find("HitBox").GetComponent<PlayerHitBox>();
    }

    private void Start()
    {
        _attackSpeed = _manager.Data.NormalAttackSpeed;
        _coolTime = new WaitForSeconds(_attackSpeed);
    }

    private void OnDataUpdate()
    {
        if (_attackSpeed != _manager.Data.NormalAttackSpeed)
            _coolTime = new WaitForSeconds(_manager.Data.NormalAttackSpeed);
    }

    private IEnumerator NormalAttack()
    {
        yield return _attackDelay;

        while (true)
        {
            _hitBox.SetHitBoxOffset();
            _animationController.NormalAttack();
            yield return _coolTime;
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
