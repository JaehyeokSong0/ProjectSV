using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillUI : MonoBehaviour
{
    private const string _iconPath = "Prefabs/Skills/Icons/";
    private const int _skillCapacity = 8;
    private const float _iconMoveSpeed = 1f;
    [SerializeField] private List<Transform> _skillGrids = new List<Transform>();
    [SerializeField] private List<GameObject> _skillList;
    [SerializeField] private PlayerSkillController _skillController;
    [SerializeField] private GameObject _gravityIcon;
    private int _currSkillCount = 0;

    private void Awake()
    {
        if (_skillGrids.Count == 0)
        {
            Transform grids = transform.Find("Position");
            foreach (Transform grid in grids)
                _skillGrids.Add(grid);
        }
        if (_skillController == null)
            _skillController = FindFirstObjectByType<PlayerSkillController>();
        if (_gravityIcon == null)
            _gravityIcon = Resources.Load(_iconPath + "Skill_Gravity") as GameObject;

        _skillList = new List<GameObject>();
        _skillList.Capacity = _skillCapacity;
    }

    private void LateUpdate()
    {
        int currSkillCount = _skillController.GetSkillCount();
        if (_currSkillCount > currSkillCount) // Release(Use) Skill
        {
            _currSkillCount = currSkillCount;
            Destroy(_skillList[_currSkillCount]);
        }
        else if (_currSkillCount < currSkillCount) // Get(Stack) Skill
        {
            _currSkillCount = currSkillCount;
            GameObject skillGO = Instantiate(_gravityIcon, _skillGrids[_skillCapacity - 1]); // TEST CODE
            Debug.Log(_currSkillCount + " " + _skillList.Capacity);
            _skillList.Add(skillGO);
            StartCoroutine(MoveIcon(skillGO, skillGO.transform.position, _skillGrids[_currSkillCount - 1].position));
        }
    }

    private IEnumerator MoveIcon(GameObject icon, Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 direction = (endPosition - startPosition).normalized;
        while ((icon.transform.position - endPosition).x > 0)
        {
            yield return null;
            if (icon == null)
                yield break;
            icon.transform.position += direction * _iconMoveSpeed;
        }
    }

    private void ResetIconPosition()
    {

    }
}