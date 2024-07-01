using System.Collections;
using UnityEngine;

public class Skill_Gravity : Base_Skill
{
    private WaitForSeconds testEnum = new WaitForSeconds(1f);
    // TEST CODE
    [SerializeField] private Skill_Gravity_HitBox _hitBox;

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
        //StartCoroutine(testFunc());
    }

    private IEnumerator testFunc()
    {
        while (true)
        {

            _hitBox.enabled = true;
            yield return testEnum;
            _hitBox.enabled = false;
            yield return null;
        }
    }
}
