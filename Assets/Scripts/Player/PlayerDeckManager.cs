using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SkillName = SkillRepository.SkillName;

[RequireComponent(typeof(SkillRepository))]
public class PlayerDeckManager : MonoBehaviour
{
    #region Constant
    private const int DEFAULT_DECK_SIZE = 10;
    #endregion

    #region Property
    public int DeckCount { get => _deck.Count; }
    public int AllSkillCount { get => _deck.Count + _discardDeck.Count; }
    #endregion

    #region Field
    [SerializeField] private SkillRepository _skillRepository;
    private Stack<SkillName> _deck = new Stack<SkillName>(); // Current deck, not initial deck
    private Stack<SkillName> _discardDeck = new Stack<SkillName>();
    #endregion

    #region Event Method
    private void Awake()
    {
        if (_skillRepository == null)
            _skillRepository = GetComponent<SkillRepository>();
        // TEST CODE
        InitializeDeck();
    }
    #endregion

    #region Method
    /// <summary>
    /// Reorganize deck with used skills and skills in current deck
    /// </summary>
    public void ReorganizeDeck()
    {
        // Add remain skills to discarded skills
        var deckList = _discardDeck.ToList();
        for (int i = 0; i < _deck.Count; i++)
            deckList.Add(_deck.Pop());

        _discardDeck.Clear();
        _deck.Clear();

        for (int i = 0; i < deckList.Count; i++)
            _deck.Push(deckList[i]);

        ShuffleDeck();
    }
    /// <summary>
    /// Shuffle current deck (Exclude used skill)
    /// </summary>
    public void ShuffleDeck()
    {
        var deckList = _deck.ToList();
        _deck.Clear();

        for (int i = 0; i < deckList.Count - 1; i++)
        {
            int randomValue = Random.Range(i, deckList.Count); // Get random value from (i ~ deckArray.Length - 1)
            (deckList[i], deckList[randomValue]) = (deckList[randomValue], deckList[i]);
        }

        for (int i = 0; i < deckList.Count; i++)
            _deck.Push(deckList[i]);
    }
    /// <summary>
    /// Get skill from the top of the deck and push it to the discardDeck
    /// </summary>
    /// <returns> 
    /// Skill prefab resource
    /// </returns>
    public GameObject GetSkill()
    {
        if (_deck.Count <= 0)
            ReorganizeDeck();

        var skill = _deck.Pop();
        _discardDeck.Push(skill);

        return _skillRepository[skill];
    }
    /// <summary>
    /// Initialize deck
    /// </summary>
    private void InitializeDeck()
    {
        for (int i = 0; i < 2; i++)
            _deck.Push(SkillName.Gravity);
        for (int i = 0; i < 3; i++)
            _deck.Push(SkillName.Flame);
        for (int i = 0; i < 3; i++)
            _deck.Push(SkillName.MoveSpeedUp);
        for (int i = 0; i < 2; i++)
            _deck.Push(SkillName.Stone);
        ShuffleDeck();
    }
    #endregion
}
