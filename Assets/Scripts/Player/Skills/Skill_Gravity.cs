using UnityEngine;

public class Skill_Gravity : Base_Skill
{
    // Damage system is implemented in Skill_Gravity_HitBox
    public override void Initialize(Vector3 position)
    {
        if (Data == null)
            Data = Resources.Load("Data/Skills/GravityData") as SkillData;
        SetTransform(position); // TEST CODE
    }

    public override void CastSkill()
    {
        CastAreaSkill(Data.Tick, Data.Duration);
    }
}
