using System.Collections.Generic;
using UnityEngine;

public class EnemyRepository : MonoBehaviour
{
    #region Enum
    public enum EnemyType
    {
        Skull,
        DeathLord,
        Lich
    }
    #endregion

    #region Indexer
    public GameObject this[EnemyType name]
    {
        get => _enemyDictionary[name];
    }

    #endregion

    #region Field
    [SerializeField] private List<GameObject> _enemyPrefabs = new List<GameObject>();

    private Dictionary<EnemyType, GameObject> _enemyDictionary = new Dictionary<EnemyType, GameObject>();
    #endregion

    #region Event Method
    private void Awake()
    {
        // Initialize enemy prefab and dictionary
        var enemies = Resources.LoadAll<GameObject>("Prefabs/Enemies");
        foreach (var enemy in enemies)
        {
            _enemyPrefabs.Add(enemy);
            _enemyDictionary.Add(enemy.GetComponent<EnemyDataManager>().GetEnemyName(), enemy);
        }
    }

    public EnemyType[] GetAllEnemyType()
    {
        return System.Enum.GetValues(typeof(EnemyType)) as EnemyType[];
    }
    #endregion
}