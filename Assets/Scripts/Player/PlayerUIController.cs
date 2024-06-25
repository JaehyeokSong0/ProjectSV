using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private PlayerManager _manager;
    [SerializeField] private Image _hpGauge;
    [SerializeField] private TMP_Text _hpText;

    private void Awake()
    {
        if(_manager == null)
            _manager = FindObjectOfType<PlayerManager>();
        if(_hpGauge == null)
            _hpGauge = transform.Find("Image_HP").GetComponent<Image>();
        if(_hpText == null)
            _hpText = transform.Find("Image_HP").GetChild(0).GetComponent<TMP_Text>();
    }
    
    private void LateUpdate()
    {
        _hpGauge.fillAmount = _manager.Data.CurrentHp / _manager.Data.MaxHp;
        _hpText.text = $"{_manager.Data.CurrentHp} / {_manager.Data.MaxHp}";
    }
}
