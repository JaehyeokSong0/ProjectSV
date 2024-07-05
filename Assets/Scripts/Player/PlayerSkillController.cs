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
    private float _maxTime = 6f; // TODO
    #endregion

    #region Event Method
    private void Awake()
    {
        if (_manager == null)
            _manager = transform.parent.GetComponent<PlayerManager>();
        if(_deckManager == null)
            _deckManager = transform.parent.Find("DeckManager").GetComponent<PlayerDeckManager>();
        if (_skillUI == null)
            _skillUI = GameObject.Find("Panel_Skill").GetComponent<PlayerSkillUI>();
    }
    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > _maxTime)
        {
            _elapsedTime = 0f;
        }
    }
    private void Start()
    {
        StartCoroutine(TestFunc());
    }
    #endregion

    private IEnumerator TestFunc()
    {
        var skill = _deckManager.GetSkill();
        yield return new WaitForSeconds(2f);
        InstantiateSkill(skill);
    } // TEST CODE

    #region Method
    public void InstantiateSkill(GameObject skillPrefab)
    {
        GameObject skill = Instantiate(skillPrefab);
        var skillScript = skillPrefab.GetComponent<Base_Skill>();
        switch (skillScript.Data.Type)
        {
            case SkillData.SkillType.Attack:
                _skillQueue.Enqueue(Instantiate(skill));
                break;
            case SkillData.SkillType.Buff:
                _skillQueue.Enqueue(Instantiate(skill, transform.root));
                break;
        }

        _skillUI.OnGetSkill?.Invoke(_skillQueue.Count , skillScript.Icon); // Send skill icon to PlayerSkillUI
    }
    
    public void CastSkill()
    {
        if (_skillQueue.Count <= 0)
            return;

        GameObject skillPrefab = _skillQueue.Dequeue();
        skillPrefab.SetActive(true);
        var skillScript = skillPrefab.GetComponent<Base_Skill>();

        // Initialize skill data
        switch (skillScript.Data.Type)
        {
            case SkillData.SkillType.Attack:
                skillScript.Initialize(transform.parent.position, _manager.PlayerDirectionBuffer);
                break;
            case SkillData.SkillType.Buff:
                skillScript.Initialize();
                break;
        }

        skillScript.CastSkill();
        _skillUI.OnUseSkill?.Invoke(_skillQueue.Count);
    }

    public int GetSkillCount()
    {
        return _skillQueue.Count;
    }
    #endregion
}

