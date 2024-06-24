using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/Create EnemyData")]
public class EnemyData : ScriptableObject
{
    public string Name;

    [HideInInspector] public float Hp = 100f;
    [HideInInspector] public float WalkSpeed = 1f;
    [HideInInspector] public float RunSpeed = 2f;

    [HideInInspector] public float NormalAttackSpeed = 3.0f;
    [HideInInspector] public float NormalAttackRange = 0.5f;
}
