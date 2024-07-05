using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillUI : MonoBehaviour
{
    #region Constant
    private const int SKILL_CAPACITY = 8;
    private const float ICON_MOVE_SPEED = 3f;
    #endregion

    #region Field
    private int _skillCountBuffer = 0;
    private int _availableGridCount = 3;

    [SerializeField] private PlayerSkillController _skillController;

    [SerializeField] private List<Transform> _skillGrids = new List<Transform>();
    [SerializeField] private List<GameObject> _skillIcons = new List<GameObject>();

    [SerializeField] private TMP_Text _mpCountText;
    [SerializeField] private TMP_Text _deckCountText;
    [SerializeField] private Image _timeBar;
    #endregion

    #region Event Method
    private void Awake()
    {
        // Check skill grid conformity
        bool isReseted = false;
        foreach (var grid in _skillGrids)
        {
            if (grid == null)
            {
                ResetSkillGrids();
                isReseted = true;
                break;
            }
        }
        if (isReseted == false && _skillGrids.Count != SKILL_CAPACITY)
        {
            ResetSkillGrids();
        }

        if (_skillController == null)
            _skillController = GameObject.FindFirstObjectByType<PlayerSkillController>();
    }
    private void Start()
    {
        EventManager.Instance.OnPlayerDead?.AddListener(this.OnPlayerDead);
        EventManager.Instance.OnGetSkill?.AddListener(this.OnGetSkill);
        EventManager.Instance.OnUseSkill?.AddListener(this.OnUseSkill);
    }

    private void LateUpdate()
    {
        _timeBar.fillAmount = _skillController.ElaspedTime / _skillController.MaxTime;
    }
    #endregion

    #region Event Callback
    public void OnPlayerDead()
    {
        this.enabled = false;
    }
    public void OnGetSkill(GameObject icon)
    {
        int currSkillCount = _skillController.GetSkillCount();
        if (_skillCountBuffer < currSkillCount)
        {
            GetSkill(currSkillCount - _skillCountBuffer, icon);
            _skillCountBuffer = currSkillCount;
        }
    }
    public void OnUseSkill()
    {
        int currSkillCount = _skillController.GetSkillCount();

        if (_skillCountBuffer > currSkillCount)
        {
            UseSkill(_skillCountBuffer - currSkillCount);
            _skillCountBuffer = currSkillCount;
        }
    }
    #endregion
    private void ResetSkillGrids()
    {
        _skillGrids.Clear();
    }

    private IEnumerator MoveIcon(GameObject icon, Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 direction = (endPosition - startPosition).normalized;
        while ((icon.transform.position - endPosition).x > 0)
        {
            yield return null;
            if (icon == null)
                yield break;
            icon.transform.position += direction * ICON_MOVE_SPEED;
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
    private void GetSkill(int count, GameObject icon)
    {
        // Clamp max range of count
        if (_skillCountBuffer + count > SKILL_CAPACITY)
            count = SKILL_CAPACITY - _skillCountBuffer;

        //GameObject iconGO = Instantiate(icon, _skillGrids[SKILL_CAPACITY - 1]); // TEST CODE
        for (int i = _skillCountBuffer; i < _skillCountBuffer + count; i++)
        {
            GameObject iconGO = Instantiate(icon, _skillGrids[i]);
            AddSkillIcon(iconGO, i);
            //StartCoroutine(MoveIcon(iconGO, iconGO.transform.position, _skillGrids[i].position));
        }

        //AddSkillIcon(iconGO);
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

    private void AddSkillIcon(GameObject icon, int index)
    {
        SetSkillIconCapacity();
        if (_skillIcons[index] == null)
            _skillIcons[index] = icon;
        else
            _skillIcons.Add(icon);
    }

    /// <summary>
    /// Set capacity of the list _skillIcon to the maximum value of skill count
    /// </summary>
    private void SetSkillIconCapacity()
    {
        int currentSkillCount = _skillController.GetSkillCount();
        if (_skillIcons.Count < currentSkillCount)
        {
            for(int i = 0; i < currentSkillCount - _skillIcons.Count; i++)
                _skillIcons.Add(null);
        }
    }
}