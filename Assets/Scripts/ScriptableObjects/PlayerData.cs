using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Create PlayerData")]
public class PlayerData : ScriptableObject
{
    [HideInInspector] public float currentHp = 1000f;
    [HideInInspector] public float maxHp = 1000f;
    [HideInInspector] public float currentMp = 100f;
    [HideInInspector] public float maxMp = 100f;
    [HideInInspector] public float currentStamina = 100f;
    [HideInInspector] public float maxStamina = 100f;
    [HideInInspector] public float exp = 0f;

    [HideInInspector] public float invincibleTime = 2f; // Invincible time after attacked
    [HideInInspector] public float walkSpeed = 0.02f;
    [HideInInspector] public float runSpeed = 0.04f;
    [HideInInspector] public float normalAttackSpeed = 3.0f;
    [HideInInspector] public float normalAttackDamage = 50f;

    [HideInInspector] public int skillCapacity = 8; // Must change Panel_Skill of GameScene if changed
}
