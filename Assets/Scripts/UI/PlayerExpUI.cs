using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExpUI : MonoBehaviour
{
    #region Field
    [SerializeField] private PlayerManager _manager;
    [SerializeField] private Image _expGauge;
    [SerializeField] private TMP_Text _expText;
    #endregion

    #region Event Method
    private void Awake()
    {
        if (_manager == null)
            _manager = FindObjectOfType<PlayerManager>();
        if (_expGauge == null)
            _expGauge = transform.Find("Image_Exp").GetComponent<Image>();
        if (_expText == null)
            _expText = transform.Find("Text_Level").GetComponent<TMP_Text>();
    }

    private void Start()
    {
        EventManager.Instance.OnPlayerExpUpdated?.AddListener(UpdateEXPUI);
    }
    #endregion

    #region Method

    private void UpdateEXPUI()
    {
        _expGauge.fillAmount = _manager.Data.CurrentExp / _manager.Data.MaxExp;
        _expText.text = _manager.Data.Level.ToString();
    }
    #endregion
}
