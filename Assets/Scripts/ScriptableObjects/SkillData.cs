using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Data/Create SkillData")]
public class SkillData : ScriptableObject
{
    public float Damage;

    public float Duration;
    public float Tick;

    public float Radius;
}
