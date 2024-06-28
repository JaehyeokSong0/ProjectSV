using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    [SerializeField] private PlayerManager _manager;
    private Queue<GameObject> _skillQueue = new Queue<GameObject>();
    [SerializeField] private GameObject _gravityGO; // TEST CODE
    private void Awake()
    {
        if (_manager == null)
            _manager = GetComponentInParent<PlayerManager>();

        if (_gravityGO == null)
            _gravityGO = Resources.Load("Prefabs/Skills/Skill_Gravity") as GameObject;
    }

    private void Start()
    {
        StartCoroutine(TestFunc());
    }

    private IEnumerator TestFunc() // TEST CODE
    {
        WaitForSeconds regenTime = new WaitForSeconds(4f);
        yield return new WaitForSeconds(3f); // Start delay
        while (true)
        {
            if(_skillQueue.Count < _manager.Data.SkillCapacity)
                GetSkill(_gravityGO);
            yield return regenTime;
        }
    }

    public void GetSkill(GameObject skillGO) // Prefab in project, not in scene
    {
        _skillQueue.Enqueue(Instantiate(skillGO));
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
    }

    public int GetSkillCount()
    {
        return _skillQueue.Count;
    }
}
