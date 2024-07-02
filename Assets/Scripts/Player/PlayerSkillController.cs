using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    [SerializeField] private PlayerManager _manager;
    private Queue<GameObject> _skillQueue = new Queue<GameObject>();
    [SerializeField] private GameObject _gravityGO; // TEST CODE

    public float ElaspedTime { get; private set; }
    public float MaxTime { get; private set; }

    private void Awake()
    {
        if (_manager == null)
            _manager = GetComponentInParent<PlayerManager>();

        if (_gravityGO == null)
            _gravityGO = Resources.Load("Prefabs/Skills/Skill_Gravity") as GameObject;

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

        yield return regenTime;

        while (true)
        {
            if(_skillQueue.Count < _manager.Data.SkillCapacity)
                GetSkill(_gravityGO);
            if (--count <= 0)
                yield break;

            yield return regenTime;
        }
    }

    public void GetSkill(GameObject skillGO) // Prefab in project, not in scene
    {
        _skillQueue.Enqueue(Instantiate(skillGO));

        EventManager.Instance.OnSkillsUpdated.Invoke();
    }

    public void CastSkill()
    {
        if (_skillQueue.Count <= 0)
            return;

        GameObject skillGO = _skillQueue.Dequeue();
        skillGO.SetActive(true);

        var skillScript = skillGO.GetComponent<Base_Skill>();
        skillScript.Initialize(transform.parent.position);
        skillScript.CastSkill();

        EventManager.Instance.OnSkillsUpdated.Invoke();
    }

    public int GetSkillCount()
    {
        return _skillQueue.Count;
    }
}
