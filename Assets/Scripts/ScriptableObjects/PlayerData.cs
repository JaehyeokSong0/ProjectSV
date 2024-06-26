using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Create PlayerData")]
public class PlayerData : ScriptableObject
{
    public float CurrentHp = 200f;
    public float MaxHp = 200f;
    public float CurrentMp = 100f;
    public float MaxMp = 100f;
    public float CurrentStamina = 100f;
    public float MaxStamina = 100f;
    public float Exp = 0f;

    public float WalkSpeed = 0.02f;
    public float RunSpeed = 0.04f;
    public float NormalAttackSpeed = 3.0f;
    public float NormalAttackDamage = 50f;
}
