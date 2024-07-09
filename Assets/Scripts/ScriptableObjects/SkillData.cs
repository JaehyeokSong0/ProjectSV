using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Data/Create SkillData")]
public class SkillData : ScriptableObject
{
    public enum SkillType
    { 
        Projectile,
        Area,
        Orbit,
        Buff,
    }

    public SkillType Type;
    public int Cost;
    public float Damage;
    public float MoveSpeed; // Used in skills such as shooting
    public float Duration;
    public float Tick;
    public float Radius;
    public float Value; // Used in skills such as Buff
}
