using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/Create EnemyData")]
public class EnemyData : ScriptableObject
{
    public string Name;

    public float Hp = 100f;
    public float WalkSpeed = 1f;
    public float RunSpeed = 2f;
    public float NormalAttackSpeed = 3.0f;
    public float NormalAttackRange = 0.5f;
    public float NormalAttackDamage = 20f;
}
