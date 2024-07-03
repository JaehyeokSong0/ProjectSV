using UnityEngine;

// Example of each enemy's data
// Not used 
[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/Create EnemyData")]
public class EnemyData : ScriptableObject
{
    public float hp;
    public float walkSpeed;
    public float runSpeed;
    public float normalAttackSpeed;
    public float normalAttackRange;
    public float normalAttackDamage;
}
