using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/Create EnemyData")]
public class EnemyData : ScriptableObject
{
    public float Hp = 100f;
    public float WalkSpeed = 1f;
    public float RunSpeed = 2f;
    public float NormalAttackSpeed = 3f;
    public float NormalAttackRange = 2f;
    public float NormalAttackDamage = 20f;
}
