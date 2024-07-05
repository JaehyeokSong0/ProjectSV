using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerSkillUI : MonoBehaviour
{
    #region Event
    public UnityEvent<int, GameObject> OnGetSkill; // {skillCount, skillIcon}
    public UnityEvent<int> OnUseSkill; // {skillCount}
    public UnityEvent<int, int> OnMpChanged; // {currentMp, maxMp}
    #endregion

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

    [SerializeField] private TMP_Text _mpText;
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

        // Check components availability
        if (_skillController == null)
            _skillController = GameObject.FindFirstObjectByType<PlayerSkillController>();
        if(_mpText == null)
            _mpText = transform.Find("Image_MPCount").GetChild(0).GetComponent<TMP_Text>();

        SetGridUIAvailability();
    }
    private void Start()
    {
        OnGetSkill?.AddListener(GetSkill);
        OnUseSkill?.AddListener(UseSkill);
        OnMpChanged?.AddListener(UpdateMPUI);
        EventManager.Instance.OnPlayerDead?.AddListener(this.OnPlayerDead);
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
    public void GetSkill(int skillCount, GameObject icon)
    {
        if (_skillCountBuffer >= skillCount)
            return;

        // Clamp max range of count
        int count = skillCount - _skillCountBuffer;
        if (_skillCountBuffer + count > SKILL_CAPACITY)
            count = SKILL_CAPACITY - _skillCountBuffer;

        SetSkillIconCapacity(skillCount);
        for (int i = _skillCountBuffer; i < _skillCountBuffer + count; i++)
        {
            GameObject iconGO = Instantiate(icon, _skillGrids[i]);
            AddSkillIcon(iconGO, i);
        }

        RefreshIconList();
        _skillCountBuffer = skillCount;
    }

    public void UseSkill(int skillCount)
    {
        if (_skillCountBuffer <= skillCount)
            return;

        int count = _skillCountBuffer - skillCount;
        for (int i = 0; i < count; i++)
        {
            if (_skillIcons[i] != null)
                Destroy(_skillIcons[i]);
            _skillIcons[i] = null;
        }

        RearrangeIcons();
        _skillCountBuffer = skillCount;
    }
    #endregion

    #region Method
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
    private void AddSkillIcon(GameObject icon, int index)
    {
        if (_skillIcons[index] == null)
            _skillIcons[index] = icon;
        else
            _skillIcons.Add(icon);
    }
    /// <summary>
    /// Set capacity of the list _skillIcon to the maximum value of skill count
    /// </summary>
    private void SetSkillIconCapacity(int needSize)
    {
        if (_skillIcons.Count < needSize)
        {
            for (int i = 0; i < needSize - _skillIcons.Count; i++)
                _skillIcons.Add(null);
        }
    }
    /// <summary>
    /// Set UI image for activated grids
    /// </summary>
    private void SetGridUIAvailability()
    {
        for (int i = 0; i < _availableGridCount; i++)
        {
            _skillGrids[i].GetComponent<Image>().enabled = false; // Activate grid UI
        }
    }
    private void UpdateMPUI(int currentMp, int maxMp)
    {
        _mpText.text = $"{currentMp} / {maxMp}";
    }
    #endregion
}