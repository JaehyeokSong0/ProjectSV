using System.Collections.Generic;
using UnityEngine;

public class SkillRepository : MonoBehaviour
{
    #region Enum
    public enum SkillName
    {
        Gravity,
        Flame,
        MoveSpeedUp,
        Stone
    }
    #endregion

    #region Constant
    private const int SKILL_NUMBERS = 4;
    #endregion

    #region Indexer
    public GameObject this[SkillName name]
    {
        get => _skillDictionary[name];
    }

    #endregion

    #region Field
    [SerializeField] private GameObject[] _skillPrefabs = new GameObject[SKILL_NUMBERS];
    private Dictionary<SkillName, GameObject> _skillDictionary = new Dictionary<SkillName, GameObject>();
    #endregion

    #region Event Method
    private void Awake()
    {
        // Initialize skill dictionary
        for (int i = 0; i < SKILL_NUMBERS; i++)
        {
            _skillDictionary.Add(_skillPrefabs[i].GetComponent<Base_Skill>().Name, _skillPrefabs[i]);
        }
    }
    #endregion
}