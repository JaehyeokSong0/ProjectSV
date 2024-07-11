using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    #region Constant
    private const int CARD_COUNT = 3;
    #endregion

    #region Field
    [SerializeField] private Timer _timer;
    [SerializeField] private PlayerExpContoller _expController;
    [SerializeField] private List<GameObject> _cards;
    [SerializeField] private Vector3[] _cardPositions = new Vector3[CARD_COUNT];
    #endregion

    #region Event Method
    private void Awake()
    {
        if (_timer == null)
            _timer = GameObject.FindFirstObjectByType<Timer>();
        if (_expController == null)
            _expController = GameObject.FindFirstObjectByType<PlayerExpContoller>();

        if (_cardPositions[0] == Vector3.zero) // Not set
        {
            var transforms = transform.Find("SelectionTransforms");
            int index = 0;
            foreach (Transform offset in transforms)
            {
                _cardPositions[index++] = offset.position;
            }
        }
        InitializeCardList();
    }

    private void Start()
    {
        EventManager.Instance.OnPlayerLevelUp?.AddListener(ShowLevelUpUI);
        gameObject.SetActive(false);
    }
    #endregion

    #region Event Callback
    public void OnClickCard()
    {
        foreach (var card in _cards)
        {
            card.gameObject.SetActive(false);
        }
        HideLevelUpUI();
    }
    #endregion

    #region Method
    public void ShowLevelUpUI()
    {
        InputManager.Instance.SwitchActionMap(InputManager.ActionMapType.UIAction);
        gameObject.SetActive(true);
        var indices = Utility.GetRandomInts(CARD_COUNT, 0, _cards.Count);
        for (int i = 0; i < CARD_COUNT; i++)
        {
            _cards[indices[i]].transform.position = _cardPositions[i];
            _cards[indices[i]].SetActive(true);
        }
        _timer.PauseTime();
    }

    private void HideLevelUpUI()
    {
        gameObject.SetActive(false);
        InputManager.Instance.SwitchActionMap(InputManager.ActionMapType.PlayerAction);
        _timer.ResumeTime();
    }
    private void InitializeCardList()
    {
        var cards = transform.Find("Cards");
        foreach (Transform card in cards)
            _cards.Add(card.gameObject);
    }
    /// <summary>
    /// Select card by index
    /// Called in PlayerInputController to select card by keyboard event
    /// </summary>
    /// <param name="index">card index</param>
    public void SelectCard(int index)
    {
        if ((index < 0) || (index >= CARD_COUNT))
            return;

        _cards[index].GetComponent<Button>().onClick.Invoke();
    }
    #endregion
}
