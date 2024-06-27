using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Data/Create SkillData")]
public class SkillData : ScriptableObject
{
    public bool IsDOT;
    public float Tick; // Used if IsDot == true
    public float Duration;
    public float Damage;
}
