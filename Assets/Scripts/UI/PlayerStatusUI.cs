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
    }

    private void Start()
    {
        EventManager.Instance.OnPlayerDamaged?.AddListener(UpdateHPUI);
    }
    #endregion

    #region Method
    private void UpdateHPUI(float dummy) // attached to UnityEvent<float> so use dummy parameter
    {
        _hpGauge.fillAmount = _manager.Data.CurrentHp / _manager.Data.MaxHp;
        _hpText.text = $"{_manager.Data.CurrentHp} / {_manager.Data.MaxHp}";
    }
    #endregion
}
