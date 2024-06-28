using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillUI : MonoBehaviour
{
    private const string _iconPath = "Prefabs/Skills/Icons/";
    private const int _skillCapacity = 8;
    private const float _iconMoveSpeed = 2f;
    [SerializeField] private List<Transform> _skillGrids = new List<Transform>(); // Just used with its position value
    [SerializeField] private List<GameObject> _skillIcons = new List<GameObject>();
    [SerializeField] private PlayerSkillController _skillController;
    [SerializeField] private GameObject _gravityIcon;
    [SerializeField] private int _currSkillCount = 0;

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

        _skillIcons.Add(null);
    }
    private void Start()
    {
        EventManager.Instance.OnPlayerDead?.AddListener(this.OnPlayerDead);
    }

    private void LateUpdate()
    {
        int currSkillCount = _skillController.GetSkillCount();
        if (_currSkillCount > currSkillCount) // Release(Use) Skill
        {
            _currSkillCount = currSkillCount;
            Destroy(_skillIcons[0].gameObject); // TEST CODE -> TODO : {0 -> index}
            _skillIcons[0] = null;
            RearrangeIcons();
        }
        else if (_currSkillCount < currSkillCount) // Get(Stack) Skill
        {
            _currSkillCount = currSkillCount;
            GameObject iconGO = Instantiate(_gravityIcon, _skillGrids[_skillCapacity - 1]); // TEST CODE

            if (_skillIcons[_skillIcons.Count - 1] == null)
                _skillIcons[_skillIcons.Count - 1] = iconGO;
            else
                _skillIcons.Add(iconGO);
            RefreshIconList();
            StartCoroutine(MoveIcon(iconGO, iconGO.transform.position, _skillGrids[_currSkillCount - 1].position));
        }
    }

    public void OnPlayerDead()
    {
        this.enabled = false;
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

    private void RefreshIconList()
    {
        int maxSkillCount = _skillIcons.Count;
        for (int i = 0; i < maxSkillCount - 1; i++)
        {
            if (_skillIcons[i] != null)
                continue;

            for (int j = i + 1; j < maxSkillCount; j++)
            {
                if (_skillIcons[j] != null)
                {
                    _skillIcons[i] = _skillIcons[j];
                    _skillIcons[j] = null;
                    break;
                }
            }

        }
    }

    // Called after destroy GameObject in list[index]
    private void RearrangeIcons()
    {
        int maxSkillCount = _skillIcons.Count;
        for (int i = 0; i < maxSkillCount - 1; i++)
        {
            if (_skillIcons[i] != null)
                continue;

            for (int j = i + 1; j < maxSkillCount; j++)
            {
                if (_skillIcons[j] != null)
                {
                    StartCoroutine(MoveIcon(_skillIcons[j], _skillGrids[j].position, _skillGrids[i].position));

                    _skillIcons[i] = _skillIcons[j];
                    _skillIcons[j] = null;
                    break;
                }
            }

        }
    }
}