using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Create PlayerData")]
public class PlayerData : ScriptableObject
{
    [HideInInspector] public float CurrentHp = 100f;
    [HideInInspector] public float MaxHp = 100f;
    [HideInInspector] public int CurrentMp = 3;
    [HideInInspector] public int MaxMp = 3;
    [HideInInspector] public float CurrentExp = 0f;
    [HideInInspector] public float MaxExp = 40f;

    [HideInInspector] public float InvincibleTime = 0.5f; // Invincible time after attacked
    [HideInInspector] public float WalkSpeed = 0.04f;
    [HideInInspector] public float NormalAttackSpeed = 2f;
    [HideInInspector] public float NormalAttackDamage = 50f;

    [HideInInspector] public int SkillCapacity = 8; // Must change Panel_Skill of GameScene if changed
    [HideInInspector] public int SkillCount = 3; // Draw card count
    [HideInInspector] public float SkillRegenTime = 20f;
    [HideInInspector] public int Level = 1;
}
