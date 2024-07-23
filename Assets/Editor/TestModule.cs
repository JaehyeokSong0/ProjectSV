using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using EnemyType = EnemyRepository.EnemyType;
using System.Runtime.InteropServices;
using Unity.VisualScripting;

#if UNITY_EDITOR
[InitializeOnLoad]
public class TestModule : EditorWindow
{
    #region Property
    public bool IsDeathLordActivated
    {
        get => _isDeathLordActivated;
        set
        {
            if (_isDeathLordActivated != value)
            {
                _isDeathLordActivated = value;
                OnEnemySelectToggle(EnemyType.DeathLord, _isDeathLordActivated);
            }
        }
    }

    public bool IsSkullActivated
    {
        get => _isSkullActivated;
        set
        {
            if (_isSkullActivated != value)
            {
                _isSkullActivated = value;
                OnEnemySelectToggle(EnemyType.Skull, _isSkullActivated);
            }
        }
    }
    public bool IsLichActivated
    {
        get => _isLichActivated;
        set
        {
            if (_isLichActivated != value)
            {
                _isLichActivated = value;
                OnEnemySelectToggle(EnemyType.Lich, _isLichActivated);
            }
        }
    }
    #endregion

    #region Field
    private string _enemyCountText;
    private string _expCountText;
    private bool _isDeathLordActivated;
    private bool _isSkullActivated;
    private bool _isLichActivated;

    // In Hierarchy
    [SerializeField] private GameObject _testEnemyModule;
    [SerializeField] private GameObject _testExpModule;
    [SerializeField] private EnemySpawner _enemySpawner;

    // In Project
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _expPrefab;
    #endregion


    private void OnGUI()
    {
        DrawEnemySpawnerPanel();
        GUILayout.Space(20);
        DrawEnemyModulePanel();
        GUILayout.Space(20);
        DrawExpModulePanel();
        AssignDependencies();
    }

    [MenuItem("CustomTools/TestModule/Run TestModule %t")]
    public static void Run()
    {
        EditorWindow.GetWindow<TestModule>();
    }

    private void AssignDependencies()
    {
        if (_enemyPrefab == null)
            _enemyPrefab = Resources.Load("Prefabs/Enemy_DeathLord") as GameObject;
        if (_expPrefab == null)
            _expPrefab = Resources.Load("Prefabs/Exp_1") as GameObject;
        if (_testEnemyModule == null)
            _testEnemyModule = GameObject.Find("EnemyModule");
        if (_testExpModule == null)
            _testExpModule = GameObject.Find("ExpModule");
        if (_enemySpawner == null)
            _enemySpawner = FindFirstObjectByType<EnemySpawner>();
    }

    private void DrawEnemySpawnerPanel()
    {
        _enemySpawner = EditorGUILayout.ObjectField("EnemySpawner", _enemySpawner, typeof(GameObject), true) as EnemySpawner;

        EditorGUILayout.BeginVertical();
        IsDeathLordActivated = EditorGUILayout.Toggle("DeathLord", IsDeathLordActivated);
        IsSkullActivated = EditorGUILayout.Toggle("Skull", IsSkullActivated);
        IsLichActivated = EditorGUILayout.Toggle("Lich", IsLichActivated);
        EditorGUILayout.EndVertical();
    }

    private void DrawEnemyModulePanel()
    {
        _enemyPrefab = EditorGUILayout.ObjectField("Enemy Prefab", _enemyPrefab, typeof(GameObject), true) as GameObject;
        _testEnemyModule = EditorGUILayout.ObjectField("Enemy module", _testEnemyModule, typeof(GameObject), true) as GameObject;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Input EnemySpawn size");
        _enemyCountText = EditorGUILayout.TextField(_enemyCountText, GUILayout.Width(100f));
        EditorGUILayout.EndHorizontal();

        // Buttons
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Set", GUILayout.Width(50f), GUILayout.Height(50f)) == true)
        {
            OnEnemySpawnButtonPressed(int.Parse(_enemyCountText));
        }
        if (GUILayout.Button("Reset", GUILayout.Width(50f), GUILayout.Height(50f)) == true)
        {
            OnEnemyResetButtonPressed();
        }
        GUILayout.EndHorizontal();
    }

    private void DrawExpModulePanel()
    {
        _expPrefab = EditorGUILayout.ObjectField("Exp Prefab", _expPrefab, typeof(GameObject), true) as GameObject;
        _testExpModule = EditorGUILayout.ObjectField("Exp module", _testExpModule, typeof(GameObject), true) as GameObject;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Input ExpCreate size");
        _expCountText = EditorGUILayout.TextField(_expCountText, GUILayout.Width(100f));
        EditorGUILayout.EndHorizontal();

        // Buttons
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Set", GUILayout.Width(50f), GUILayout.Height(50f)) == true)
        {
            OnExpCreateButtonPressed(int.Parse(_expCountText));
        }
        if (GUILayout.Button("Reset", GUILayout.Width(50f), GUILayout.Height(50f)) == true)
        {
            OnExpResetButtonPressed();
        }
        GUILayout.EndHorizontal();
    }

    private void OnEnemySelectToggle(EnemyType enemyType, bool isActivated)
    {
        if (_enemySpawner.EnemyInfo[enemyType].IsInitialized == true)
            _enemySpawner.SetEnemyToCreate(enemyType, isActivated);
        else
        {
            _enemySpawner.EnemyInfo[enemyType].Initialize(1f);
            _enemySpawner.SetEnemyToCreate(enemyType, isActivated);
        }
    }

    private void OnEnemySpawnButtonPressed(int size)
    {
        if ((size <= 0) || (size >= 1000))
            Debug.LogWarning("EnemySpawn size should be in range of 1 ~ 999");

        for (int i = 0; i < size; i++)
        {
            Instantiate(_enemyPrefab, Random.insideUnitCircle * 10f, Quaternion.identity, _testEnemyModule.transform);
        }
    }

    private void OnEnemyResetButtonPressed()
    {
        while (_testEnemyModule.transform.childCount > 0)
            DestroyImmediate(_testEnemyModule.transform.GetChild(0).gameObject);
    }

    private void OnExpCreateButtonPressed(int size)
    {
        if ((size <= 0) || (size >= 1000))
            Debug.LogWarning("ExpPrefab size should be in range of 1 ~ 999");

        for (int i = 0; i < size; i++)
        {
            Instantiate(_expPrefab, Random.insideUnitCircle * 5f, Quaternion.identity, _testExpModule.transform);
        }
    }

    private void OnExpResetButtonPressed()
    {
        while (_testExpModule.transform.childCount > 0)
            DestroyImmediate(_testExpModule.transform.GetChild(0).gameObject);
    }
}
#endif