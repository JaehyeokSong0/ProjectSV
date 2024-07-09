using System.Collections;
using UnityEngine;

public class Skill_MoveSpeedUp : Base_Skill
{
    #region Property
    public override SkillRepository.SkillName Name
    {
        get => _name;
        protected set => _name = value;
    }
    public override SkillData Data
    {
        get => _data;
        protected set => _data = value;
    }
    public override GameObject Icon
    {
        get => _icon;
        protected set => _icon = value;
    }
    #endregion

    #region Field
    [SerializeField] private SkillRepository.SkillName _name;
    [SerializeField] private SkillData _data;
    [SerializeField] private GameObject _icon;
    [SerializeField] private PlayerManager _manager;
    [SerializeField] private PlayerAnimationController _animationController;
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
        _manager = transform.root.GetComponent<PlayerManager>();
        _animationController = transform.root.Find("Model").GetComponent<PlayerAnimationController>();
    }
    protected override IEnumerator C_CastSkill()
    {
        _animationController.Run(_manager.PlayerDirectionBuffer);

        _manager.Data.WalkSpeed += Data.Value;

        yield return new WaitForSeconds(Data.Duration);

        _manager.Data.WalkSpeed -= Data.Value;

        StopCheckTime();
        Destroy(gameObject);
    }
    #endregion
}