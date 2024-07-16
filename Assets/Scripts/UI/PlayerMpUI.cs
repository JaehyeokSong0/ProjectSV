using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerMpUI : MonoBehaviour
{

    #region Constant
    private const int DEFAULT_MAX_MP = 10;
    private readonly Color DISABLED_MP_COLOR = new Color(80f / 255f, 80f / 255f, 80f / 255f);
    #endregion 

    #region Field
    [SerializeField] private PlayerManager _manager;
    [SerializeField] private List<Image> _mpImages = new(DEFAULT_MAX_MP);
    [SerializeField] private PlayerSkillUI _skillUI;
    private int _currentMp;
    #endregion

    private void Awake()
    {
        if (_manager == null)
            _manager = FindObjectOfType<PlayerManager>();
        if (_mpImages.Count <= 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                _mpImages.Add(transform.GetChild(i).gameObject.GetComponent<Image>());
            }
        }
        if(_skillUI == null)
            _skillUI = transform.parent.Find("Panel_Skill").GetComponent<PlayerSkillUI>();
    }

    private void Start()
    {
        _skillUI.OnMpChanged?.AddListener(UpdateMpUI);
        UpdateMpUI(_manager.Data.CurrentMp, DEFAULT_MAX_MP);
    }

    private void UpdateMpUI(int currentMp, int maxMp)
    {
        for (int i = 0; i < currentMp; i++)
            _mpImages[i].color = Color.white;
        for (int i = currentMp; i < maxMp; i++)
            _mpImages[i].color = DISABLED_MP_COLOR;
    }
}