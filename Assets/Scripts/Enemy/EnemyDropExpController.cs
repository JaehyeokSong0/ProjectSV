using UnityEngine;

public class EnemyDropExpController : MonoBehaviour
{
    #region Field
    [SerializeField] private GameObject _expPrefab;
    #endregion

    #region Event Method
    private void Awake()
    {
        if(_expPrefab == null)
            _expPrefab = Resources.Load("Prefabs/Exp_1") as GameObject; // Default Exp
    }
    #endregion

    #region Method
    public void DropExp(float rate, float amount)
    {
        if (canDropExp(rate) == false)
            return;

        var exp = Instantiate(_expPrefab);
        exp.transform.position = transform.position;
        exp.GetComponent<Exp>().Initialize(amount);
    }
    private bool canDropExp(float rate)
    {
        if (rate <= 0f)
            return false;

        float randomValue = Random.value;
        if(rate >= randomValue)
            return true;
        else 
            return false;
    }
    #endregion
}
