using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Create PlayerData")]
public class PlayerData : ScriptableObject
{
    [HideInInspector] public float CurrentHp = 200f;
    [HideInInspector] public float MaxHp = 200f;
    [HideInInspector] public float CurrentMp = 100f;
    [HideInInspector] public float MaxMp = 100f;
    [HideInInspector] public float CurrentStamina = 100f;
    [HideInInspector] public float MaxStamina = 100f;
    [HideInInspector] public float Exp = 0f;

    [HideInInspector] public float InvincibleTime = 2f; // Invincible time after attacked
    [HideInInspector] public float WalkSpeed = 0.02f;
    [HideInInspector] public float RunSpeed = 0.04f;
    [HideInInspector] public float NormalAttackSpeed = 3.0f;
    [HideInInspector] public float NormalAttackDamage = 50f;

    [HideInInspector] public int SkillCapacity = 8;
}
