using UnityEngine;

// Manages current data of enemy
public class EnemyDataManager : MonoBehaviour
{
    [HideInInspector] public float Hp;
    [HideInInspector] public float WalkSpeed;
    [HideInInspector] public float RunSpeed;
    [HideInInspector] public float NormalAttackSpeed;
    [HideInInspector] public float NormalAttackRange;
    [HideInInspector] public float NormalAttackDamage;

    // Used in derived class of Base_EnemyManager
    public void LoadEnemyData(EnemyData data)
    {
        Hp = data.hp;
        WalkSpeed = data.walkSpeed;
        RunSpeed = data.runSpeed;
        NormalAttackSpeed = data.normalAttackSpeed;
        NormalAttackRange = data.normalAttackRange;
        NormalAttackDamage = data.normalAttackDamage;
    }
}