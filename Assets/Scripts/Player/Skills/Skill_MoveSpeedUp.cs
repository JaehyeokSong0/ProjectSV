using System.Collections;
using UnityEngine;

public class Skill_MoveSpeedUp : Base_Skill
{
    #region Property
    public override SkillData Data
    {
        get => _data;
        set => _data = value;
    }
    public override GameObject Icon
    {
        get => _icon;
        set => _icon = value;
    }
    #endregion

    #region Field
    [SerializeField] private SkillData _data;
    [SerializeField] private GameObject _icon;
    [SerializeField] private PlayerData _playerData;
    #endregion

    #region Event Method
    protected override void Awake()
    {
        base.Awake();

        if (_data == null)
            _data = Resources.Load("Data/Skills/MoveSpeedUpData") as SkillData;
        if (_icon == null)
            _icon = Resources.Load("Prefabs/Skills/Icons/Skill_MoveSpeedUp_Icon") as GameObject;        
    }
    #endregion

    #region Method
    public override void Initialize()
    {
        _playerData = transform.root.GetComponent<PlayerManager>().Data;
        if (_playerData == null)
            Debug.LogError("Cannot find player data");
    }
    protected override IEnumerator C_CastSkill()
    {
        _playerData.WalkSpeed += Data.Value;
        _playerData.RunSpeed += Data.Value;

        yield return new WaitForSeconds(Data.Duration);

        _playerData.WalkSpeed -= Data.Value;
        _playerData.RunSpeed -= Data.Value;

        StopCheckTime();
        Destroy(gameObject);
    }
    #endregion
}