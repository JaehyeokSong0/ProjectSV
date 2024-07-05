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
    [SerializeField] private GameObject[] _skillPrefabs = new GameObject[3]; // TODO
    private Queue<GameObject> _skillQueue = new Queue<GameObject>();

    private float _elapsedTime = 0f;
    private float _maxTime = 6f; // TODO
    #endregion

    #region Event Method
    private void Awake()
    {
        if (_manager == null)
            _manager = transform.parent.GetComponent<PlayerManager>();
    }
    private void Start()
    {
        StartCoroutine(TestFunc(3));
    }
    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > _maxTime)
        {
            StartCoroutine(TestFunc(3));
            _elapsedTime = 0f;
        }
    }
    #endregion

    #region Method
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
            if (_skillQueue.Count < _manager.Data.SkillCapacity)
            {
                float randomV = UnityEngine.Random.value;
                if (randomV <= 0.3f)
                    GetSkill(_skillPrefabs[0]);
                else if (randomV <= 0.6f)
                    GetSkill(_skillPrefabs[1]);
                else
                    GetSkill(_skillPrefabs[2]);
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

        EventManager.Instance.OnGetSkill?.Invoke(skillScript.Icon);
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
    #endregion
}

