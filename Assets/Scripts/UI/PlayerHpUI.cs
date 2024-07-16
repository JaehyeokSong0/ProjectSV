using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    #region Field
    [SerializeField] private PlayerManager _manager;
    [SerializeField] private Image _hpImage;
    [SerializeField] private TMP_Text _hpText;
    #endregion

    private void Awake()
    {
        if (_manager == null)
            _manager = FindObjectOfType<PlayerManager>();
        if (_hpImage == null)
            _hpImage = GetComponent<Image>();
        if (_hpText == null)
            _hpText = transform.GetChild(0).GetComponent<TMP_Text>();
    }

    private void Start()
    {
        EventManager.Instance.OnPlayerHPUpdated?.AddListener(UpdateHPUI);
        UpdateHPUI();
    }

    private void UpdateHPUI()
    {
        _hpImage.fillAmount = _manager.Data.CurrentHp / _manager.Data.MaxHp;
        _hpText.text = $"{_manager.Data.CurrentHp} / {_manager.Data.MaxHp}";
    }
}
