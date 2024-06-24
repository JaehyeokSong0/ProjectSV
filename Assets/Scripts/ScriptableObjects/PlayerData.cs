using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/Create PlayerData")]
public class PlayerData : ScriptableObject
{
    [HideInInspector] public float Hp = 100f;
    [HideInInspector] public float Mp = 100f;
    [HideInInspector] public float Stamina = 100f;
    [HideInInspector] public float Exp = 0f;
    [HideInInspector] public float WalkSpeed = 0.02f;
    [HideInInspector] public float RunSpeed = 0.04f;

    [HideInInspector] public float NormalAttackSpeed = 3.0f;
}
