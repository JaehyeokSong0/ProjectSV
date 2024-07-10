using System.Collections.Generic;
using UnityEngine;

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
        gameObject.SetActive(true);
        var indices = GetRandomInts(CARD_COUNT, 0, _cards.Count);
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
        _timer.ResumeTime();
    }
    private void InitializeCardList()
    {
        var cards = transform.Find("Cards");
        foreach (Transform card in cards)
            _cards.Add(card.gameObject);
    }

    private List<int> GetRandomInts(int length, int minValue, int maxValue)
    {
        List<int> result = new List<int>(length);
        while (result.Count < length)
        {
            int value = Random.Range(minValue, maxValue);
            if (result.Contains(value) == false)
                result.Add(value);
        }
        return result;
    }

    #endregion
}
