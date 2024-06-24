using System.Collections;
using UnityEngine;

public class PlayerNormalAttackController : MonoBehaviour
{
    [SerializeField] private PlayerData _data;
    [SerializeField] private PlayerAnimationController _animationController;

    private float _attackSpeed;
    private WaitForSeconds _coolTime = null;
    private WaitForSeconds _attackDelay = new WaitForSeconds(1f);

    private void Awake()
    {
        if (_data == null)
            _data = Resources.FindObjectsOfTypeAll(typeof(PlayerData))[0] as PlayerData;
        if (_animationController == null)
            _animationController = FindObjectOfType<PlayerAnimationController>();

        _attackSpeed = _data.NormalAttackSpeed;
        _coolTime = new WaitForSeconds(_attackSpeed);
    }

    private void OnDataUpdate() // TODO
    {
        if (_attackSpeed != _data.NormalAttackSpeed)
            _coolTime = new WaitForSeconds(_data.NormalAttackSpeed);
    }

    private IEnumerator NormalAttack()
    {
        yield return _attackDelay;

        while (true)
        {
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
