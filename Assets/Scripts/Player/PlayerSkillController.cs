using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    #region Property
    public float ElaspedTime => _elapsedTime;
    public float MaxTime => _maxTime;
    #endregion

    #region Field
    [SerializeField] private PlayerManager _manager;
    [SerializeField] private PlayerDeckManager _deckManager;
    [SerializeField] private PlayerSkillUI _skillUI;
    private Queue<GameObject> _skillQueue = new Queue<GameObject>(); // Store prefab resources, not gameObject in scene

    private float _elapsedTime = 0f;
    private float _maxTime; // TODO -> to player data
    #endregion

    #region Event Method
    private void Awake()
    {
        if (_manager == null)
            _manager = transform.parent.GetComponent<PlayerManager>();
        if (_deckManager == null)
            _deckManager = transform.parent.Find("DeckManager").GetComponent<PlayerDeckManager>();
        if (_skillUI == null)
            _skillUI = GameObject.Find("Panel_Skill").GetComponent<PlayerSkillUI>();
    }
    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > _maxTime)
        {
            DiscardSkills();
            RestoreMp();
            for (int i = 0; i < _manager.Data.SkillCount; i++)
                InstantiateSkill(_deckManager.GetSkill());
            _elapsedTime = 0f;
        }
    }
    private void Start()
    {
        StartCoroutine(TestFunc());
        _maxTime = _manager.Data.SkillRegenTime;
    }
    #endregion
    // TEST CODE
    private IEnumerator TestFunc()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < _manager.Data.SkillCount; i++)
        {
            var skill = _deckManager.GetSkill();
            InstantiateSkill(skill);
        }
    } 

    #region Method
    public void InstantiateSkill(GameObject skillPrefab)
    {
        var skillScript = skillPrefab.GetComponent<Base_Skill>();
        switch (skillScript.Data.Type)
        {
            case SkillData.SkillType.Projectile:
            case SkillData.SkillType.Area:
                _skillQueue.Enqueue(Instantiate(skillPrefab));
                break;
            case SkillData.SkillType.Orbit:
            case SkillData.SkillType.Buff:
                _skillQueue.Enqueue(Instantiate(skillPrefab, transform.root));
                break;
        }

        _skillUI.OnGetSkill?.Invoke(_skillQueue.Count, skillScript.Icon); // Send skill icon to PlayerSkillUI
    }
    public void CastSkill()
    {
        if (_skillQueue.Count <= 0)
            return;

        GameObject skillPrefab = _skillQueue.Peek();
        var skillScript = skillPrefab.GetComponent<Base_Skill>();

        // If not enough mp, player cannot cast skill
        if (skillScript.Data.Cost > _manager.Data.CurrentMp)
            return;

        _skillQueue.Dequeue();
        skillPrefab.SetActive(true);

        // Initialize skill data
        switch (skillScript.Data.Type)
        {
            case SkillData.SkillType.Projectile:
            case SkillData.SkillType.Area:
                skillScript.Initialize(transform.parent.position, _manager.PlayerDirectionBuffer);
                break;
            case SkillData.SkillType.Orbit:
            case SkillData.SkillType.Buff:
                skillScript.Initialize();
                break;
        }

        skillScript.CastSkill();
        _manager.Data.CurrentMp -= skillScript.Data.Cost;
        _skillUI.OnMpChanged?.Invoke(_manager.Data.CurrentMp, _manager.Data.MaxMp);
        _skillUI.OnUseSkill?.Invoke(_skillQueue.Count);
    }

    public int GetSkillCount()
    {
        return _skillQueue.Count;
    }
    private void DiscardSkills()
    {
        foreach(var skill in _skillQueue)
        {
            Destroy(skill);
        }

        _skillQueue.Clear();
        _skillUI.OnUseSkill?.Invoke(0);

    }
    private void RestoreMp()
    {
        _manager.Data.CurrentMp = _manager.Data.MaxMp;
        _skillUI.OnMpChanged?.Invoke(_manager.Data.CurrentMp, _manager.Data.MaxMp);
    }
    #endregion
}

