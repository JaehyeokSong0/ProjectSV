using UnityEngine;

// Example of each enemy's data
// Not used 
[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/Create EnemyData")]
public class EnemyData : ScriptableObject
{
    public float Hp;
    public float WalkSpeed;
    public float RunSpeed;
    public float NormalAttackSpeed;
    public float NormalAttackRange;
    public float NormalAttackDamage;
}
