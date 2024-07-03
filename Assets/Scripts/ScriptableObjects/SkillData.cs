using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Data/Create SkillData")]
public class SkillData : ScriptableObject
{
    public enum SkillType
    { 
        Attack,
        Buff
    }

    public SkillType type;

    public float damage;
    public float moveSpeed; // Used in skills such as shooting

    public float duration;
    public float tick;

    public float radius;

    public float value; // Used in skills such as Buff
}
