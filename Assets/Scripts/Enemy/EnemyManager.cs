using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyData _data;
    private bool _isAttacked;

    private void Awake()
    {
        _data = ScriptableObject.CreateInstance<EnemyData>();
    }

    public void OnEnemyDamaged(float damage, float coolTime)
    {
        if(_isAttacked == false) // If attacked recently, it has invincible time for a while
        {
            Debug.Log($"{gameObject.name} damaged : {_data.Hp} -> {_data.Hp - damage}");

            StartCoroutine(ReduceHP(damage, coolTime));
        }
    }

    public void OnEnemyDead()
    {
        Debug.Log($"{gameObject.name} Dead"); // TEST CODE
        Destroy(gameObject);
    }

    private IEnumerator ReduceHP(float damage, float coolTime)
    {
        _isAttacked = true;
        if (_data.Hp - damage >= 0)
            _data.Hp -= damage;
        else
            _data.Hp = 0;

        if (_data.Hp == 0)
            OnEnemyDead();

        yield return new WaitForSeconds(coolTime);

        _isAttacked = false;
    }
}
