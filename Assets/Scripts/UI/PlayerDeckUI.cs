using TMPro;
using UnityEngine;

public class PlayerDeckUI : MonoBehaviour
{
    #region Field
    [SerializeField] private PlayerDeckManager _deckManager;
    [SerializeField] private TMP_Text _deckCountText;
    #endregion

    private void Awake()
    {
        if( _deckManager == null )
            _deckManager = FindFirstObjectByType<PlayerDeckManager>();
        if (_deckCountText == null)
            _deckCountText = transform.Find("Text_Deck").GetComponent<TMP_Text>();

    }
    private void Start()
    {
        _deckManager.OnDeckCountUpdated?.AddListener(UpdateDeckUI);
    }

    private void UpdateDeckUI()
    {
        _deckCountText.text = _deckManager.DeckCount.ToString();
    }

}
