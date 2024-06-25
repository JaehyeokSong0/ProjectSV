using UnityEngine;

[CreateAssetMenu(fileName = "DeathLordData", menuName = "Data/Create DeathLordData")]
public class EnemyData : ScriptableObject
{
    public float Hp = 100f;
    public float WalkSpeed = 1f;
    public float RunSpeed = 2f;
    public float NormalAttackSpeed = 3.0f;
    public float NormalAttackRange = 0.5f;
    public float NormalAttackDamage = 20f;
}
