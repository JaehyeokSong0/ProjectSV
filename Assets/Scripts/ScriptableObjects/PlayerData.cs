using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Create PlayerData")]
public class PlayerData : ScriptableObject
{
    public float Hp = 200f;
    public float Mp = 100f;
    public float Stamina = 100f;
    public float Exp = 0f;
    public float WalkSpeed = 0.02f;
    public float RunSpeed = 0.04f;
    public float NormalAttackSpeed = 3.0f;
    public float NormalAttackDamage = 20f;
}
