using System.Collections;
using UnityEngine;

public class Skill_MoveSpeedUp : Base_Skill
{
    [SerializeField] private PlayerData _playerData;

    public override void Initialize()
    {
        if (data == null)
            data = Resources.Load("Data/Skills/MoveSpeedUpData") as SkillData;
        if (icon == null)
            icon = Resources.Load("Prefabs/Skills/Icons/Skill_MoveSpeedUp_Icon") as GameObject;

        _playerData = transform.root.GetComponent<PlayerManager>().data;
        if (_playerData == null)
            Debug.LogError("Cannot find player data");
    }

    public override void CastSkill()
    {
        StartCoroutine(C_CheckElapsedTime());
        StartCoroutine(C_CastSkill());
    }

    protected override IEnumerator C_CastSkill()
    {
        _playerData.walkSpeed += data.value;
        _playerData.runSpeed += data.value;

        yield return new WaitForSeconds(data.duration);

        _playerData.walkSpeed -= data.value;
        _playerData.runSpeed -= data.value;

        StopCoroutine(C_CheckElapsedTime());
        Destroy(gameObject);
    }
}