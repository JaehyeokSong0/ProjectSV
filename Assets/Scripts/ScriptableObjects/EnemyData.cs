using UnityEngine;

// Example of each enemy's data
// Not used 
[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/Create EnemyData")]
public class EnemyData : ScriptableObject
{
    public EnemyRepository.EnemyType EnemyName;

    public float Hp;
    public float WalkSpeed;
    public float NormalAttackSpeed;
    public float NormalAttackRange;
    public float NormalAttackDamage;

    public float Exp;
    public float ExpDropRate; // (0 ~ 1)
}
