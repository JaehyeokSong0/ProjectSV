using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Create PlayerData")]
public class PlayerData : ScriptableObject
{
    [HideInInspector] public float CurrentHp = 1000f;
    [HideInInspector] public float MaxHp = 1000f;
    [HideInInspector] public int CurrentMp = 3;
    [HideInInspector] public int MaxMp = 3;
    [HideInInspector] public float CurrentStamina = 100f;
    [HideInInspector] public float MaxStamina = 100f;
    [HideInInspector] public float CurrentExp = 0f;
    [HideInInspector] public float MaxExp = 50f;

    [HideInInspector] public float InvincibleTime = 2f; // Invincible time after attacked
    [HideInInspector] public float WalkSpeed = 0.03f;
    [HideInInspector] public float RunSpeed = 0.05f;
    [HideInInspector] public float NormalAttackSpeed = 2.0f;
    [HideInInspector] public float NormalAttackDamage = 50f;

    [HideInInspector] public int SkillCapacity = 8; // Must change Panel_Skill of GameScene if changed
    [HideInInspector] public int Level = 1;
}
