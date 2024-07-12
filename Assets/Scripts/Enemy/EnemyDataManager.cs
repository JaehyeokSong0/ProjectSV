using UnityEngine;
using static EnemyRepository;

// Manages current data of enemy
public class EnemyDataManager : MonoBehaviour
{
    [SerializeField] private EnemyData _data;

    [HideInInspector] public EnemyRepository.EnemyType EnemyName;
    [HideInInspector] public float Hp;
    [HideInInspector] public float WalkSpeed;
    [HideInInspector] public float NormalAttackSpeed;
    [HideInInspector] public float NormalAttackRange;
    [HideInInspector] public float NormalAttackDamage;
    [HideInInspector] public float Exp;
    [HideInInspector] public float ExpDropRate;

    // Used in derived class of Base_EnemyManager
    public EnemyRepository.EnemyType GetEnemyName()
    {
        if (_data == null)
            Debug.LogError("Cannot find data");

        return _data.EnemyName;
    }

    public void LoadEnemyData()
    {
        LoadEnemyData(_data);
    }
    public void LoadEnemyData(EnemyData data)
    {
        EnemyName = data.EnemyName;
        Hp = data.Hp;
        WalkSpeed = data.WalkSpeed;
        NormalAttackSpeed = data.NormalAttackSpeed;
        NormalAttackRange = data.NormalAttackRange;
        NormalAttackDamage = data.NormalAttackDamage;
        Exp = data.Exp;
        ExpDropRate = data.ExpDropRate;
    }
}