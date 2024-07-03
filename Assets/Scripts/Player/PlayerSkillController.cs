using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    [SerializeField] private PlayerManager _manager;
    private Queue<GameObject> _skillQueue = new Queue<GameObject>();
    [SerializeField] private GameObject _gravityGO; // TEST CODE
    [SerializeField] private GameObject _walkSpeedUpGO; // TEST CODE
    [SerializeField] private GameObject _flameGO; // TEST CODE


    public float ElaspedTime { get; private set; }
    public float MaxTime { get; private set; }

    private void Awake()
    {
        if (_manager == null)
            _manager = GetComponentInParent<PlayerManager>();
        if (_gravityGO == null)
            _gravityGO = Resources.Load("Prefabs/Skills/Skill_Gravity") as GameObject;
        if (_walkSpeedUpGO == null)
            _walkSpeedUpGO = Resources.Load("Prefabs/Skills/Skill_WalkSpeedUp") as GameObject;

        ElaspedTime = 0f;
        MaxTime = 6f;
    }

    private void Start()
    {
        StartCoroutine(TestFunc(3));
    }

    private void Update()
    {
        ElaspedTime += Time.deltaTime;
        if (ElaspedTime > MaxTime)
        {
            StartCoroutine(TestFunc(3));
            ElaspedTime = 0f;
        }
    }

    private IEnumerator TestFunc(int count) // TEST CODE -> Generates 3 skills
    {
        WaitForSeconds regenTime = new WaitForSeconds(0.1f);
        
        foreach (var skills in _skillQueue)
        {
            Destroy(skills);
        }

        _skillQueue.Clear();
        EventManager.Instance.OnUseSkill?.Invoke();

        yield return regenTime;

        while (true)
        {
            if(_skillQueue.Count < _manager.Data.SkillCapacity)
            {
                float randomV = UnityEngine.Random.value;
                if (randomV <= 0.3f)
                    GetSkill(_walkSpeedUpGO);
                else if (randomV <= 0.6f)
                    GetSkill(_flameGO);
                else
                    GetSkill(_gravityGO);
            }
            if (--count <= 0)
                yield break;

            yield return regenTime;
        }
    }

    public void GetSkill(GameObject skillGO) // Prefab in project, not in scene
    {
        var skillScript = skillGO.GetComponent<Base_Skill>();

        switch (skillScript.Data.Type)
        {
            case SkillData.SkillType.Attack:
                _skillQueue.Enqueue(Instantiate(skillGO));
                break;
            case SkillData.SkillType.Buff:
                _skillQueue.Enqueue(Instantiate(skillGO, transform.root));
                break;
        }

        EventManager.Instance.OnGetSkill?.Invoke(skillScript.icon);
    }

    public void CastSkill()
    {
        if (_skillQueue.Count <= 0)
            return;

        GameObject skillGO = _skillQueue.Dequeue();
        skillGO.SetActive(true);
        var skillScript = skillGO.GetComponent<Base_Skill>();

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
        EventManager.Instance.OnUseSkill?.Invoke();
    }

    public int GetSkillCount()
    {
        return _skillQueue.Count;
    }
}
