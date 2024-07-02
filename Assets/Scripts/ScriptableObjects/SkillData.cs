using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Data/Create SkillData")]
public class SkillData : ScriptableObject
{
    public enum SkillType
    { 
        Attack,
        Buff
    }

    public SkillType Type;

    public float Damage;

    public float Duration;
    public float Tick;

    public float Radius;

    public float Value; // Used in skills such as Buff
}
