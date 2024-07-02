using System.Collections;
using UnityEngine;

public class Skill_MoveSpeedUp : Base_Skill
{
    [SerializeField] private PlayerData _playerData;

    public override void Initialize()
    {
        if (Data == null)
            Data = Resources.Load("Data/Skills/MoveSpeedUpData") as SkillData;

        _playerData = transform.root.GetComponent<PlayerManager>().Data;
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
        _playerData.WalkSpeed += Data.Value;
        _playerData.RunSpeed += Data.Value;

        yield return new WaitForSeconds(Data.Duration);

        _playerData.WalkSpeed -= Data.Value;
        _playerData.RunSpeed -= Data.Value;

        StopCoroutine(C_CheckElapsedTime());
        Destroy(gameObject);
    }
}