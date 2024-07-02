using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerSkillUI : MonoBehaviour
{
    private const string _iconPath = "Prefabs/Skills/Icons/";
    private const int _skillCapacity = 8;
    private const float _iconMoveSpeed = 3f;
    private int _currSkillCount = 0;
    private int _availableGridCount = 3;

    [SerializeField] private PlayerManager _manager;
    [SerializeField] private List<Transform> _skillGrids = new List<Transform>();
    [SerializeField] private List<GameObject> _skillIcons = new List<GameObject>();
    [SerializeField] private PlayerSkillController _skillController;
    [SerializeField] private GameObject _gravityIcon;

    [SerializeField] private TMP_Text _mpCountText;
    [SerializeField] private TMP_Text _deckCountText;
    [SerializeField] private Image _timeBar;

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
            _gravityIcon = Resources.Load(_iconPath + "Skill_Gravity_Icon") as GameObject;

        _skillIcons.Add(null);
    }
    private void Start()
    {
        EventManager.Instance.OnPlayerDead?.AddListener(this.OnPlayerDead);
        EventManager.Instance.OnSkillsUpdated?.AddListener(this.OnSkillsUpdated);
    }

    private void LateUpdate()
    {
        _timeBar.fillAmount = _skillController.ElaspedTime / _skillController.MaxTime;
    }

    public void OnPlayerDead()
    {
        this.enabled = false;
    }

    public void OnSkillsUpdated() // Update Skill UI
    {
        int currSkillCount = _skillController.GetSkillCount();
        if (_currSkillCount > currSkillCount) // Release(Use) Skill
        {
            UseSkill(_currSkillCount - currSkillCount);
            _currSkillCount = currSkillCount;
        }
        else if (_currSkillCount < currSkillCount) // Get(Stack) Skill
        {
            GetSkill(currSkillCount - _currSkillCount);
            _currSkillCount = currSkillCount;
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

    private void GetSkill(int count)
    {
        // Clamp max range of count
        if (_currSkillCount + count >= _skillCapacity)
            count = (_currSkillCount + count) - _skillCapacity;

        GameObject iconGO = Instantiate(_gravityIcon, _skillGrids[_skillCapacity - 1]); // TEST CODE
        for (int i = _currSkillCount; i < _currSkillCount + count; i++)
            StartCoroutine(MoveIcon(iconGO, iconGO.transform.position, _skillGrids[i].position));

        _skillIcons.Add(iconGO);
        RefreshIconList();
    }

    private void UseSkill(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (_skillIcons[i] != null)
                Destroy(_skillIcons[i]);
            _skillIcons[i] = null;
        }

        RearrangeIcons();
    }
}