using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Manages UI about player's status
public class PlayerStatusUI : MonoBehaviour
{
    #region Field
    [SerializeField] private PlayerManager _manager;
    [SerializeField] private Image _hpGauge;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private Image _expGauge;
    [SerializeField] private TMP_Text _expText;
    #endregion

    #region Event Method
    private void Awake()
    {
        if (_manager == null)
            _manager = FindObjectOfType<PlayerManager>();
        if (_hpGauge == null)
            _hpGauge = transform.Find("Image_HP").GetComponent<Image>();
        if (_hpText == null)
            _hpText = transform.Find("Image_HP").GetChild(0).GetComponent<TMP_Text>();
        if (_expGauge == null)
            _expGauge = transform.Find("Panel_Exp").GetChild(0).GetComponent<Image>();
        if (_expText == null)
            _expText = transform.Find("Panel_Exp").GetChild(1).GetComponent<TMP_Text>();
    }

    private void Start()
    {
        EventManager.Instance.OnPlayerHPUpdated?.AddListener(UpdateHPUI);
        EventManager.Instance.OnPlayerExpUpdated?.AddListener(UpdateEXPUI);
        UpdateHPUI();
    }
    #endregion

    #region Method
    private void UpdateHPUI() // attached to UnityEvent<float> so use dummy parameter
    {
        _hpGauge.fillAmount = _manager.Data.CurrentHp / _manager.Data.MaxHp;
        _hpText.text = $"{_manager.Data.CurrentHp} / {_manager.Data.MaxHp}";
    }

    private void UpdateEXPUI()
    {
        _expGauge.fillAmount = _manager.Data.CurrentExp / _manager.Data.MaxExp;
        _expText.text = _manager.Data.Level.ToString();
    }
    #endregion
}
