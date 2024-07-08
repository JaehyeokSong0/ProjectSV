using UnityEditor;
using UnityEngine;

public class TestModule : EditorWindow
{
    #region Field
    private string _enemyCountText;
    private string _expCountText;

    // In Hierarchy
    [SerializeField] private GameObject _testEnemyModule;
    [SerializeField] private GameObject _testExpModule;

    // In Project
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _expPrefab;
    #endregion


    private void OnGUI()
    {
        DrawEnemyModulePanel();
        DrawExpModulePanel();
        AssignDependencies();
    }

    [MenuItem("TestModule/Run")]
    public static void Run()
    {
        EditorWindow.GetWindow<TestModule>();
    }

    private void AssignDependencies()
    {
        if(_enemyPrefab == null)
            _enemyPrefab = Resources.Load("Prefabs/Enemy_DeathLord") as GameObject;
        if (_expPrefab == null)
            _expPrefab = Resources.Load("Prefabs/Exp_1") as GameObject;
        if (_testEnemyModule == null)
            _testEnemyModule = GameObject.Find("EnemyModule");
        if (_testExpModule == null)
            _testExpModule = GameObject.Find("ExpModule");
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
        GUILayout.BeginVertical();
        if (GUILayout.Button("Set", GUILayout.Width(50f), GUILayout.Height(50f)) == true)
        {
            OnEnemySpawnButtonPressed(int.Parse(_enemyCountText));
        }
        if (GUILayout.Button("Reset", GUILayout.Width(50f), GUILayout.Height(50f)) == true)
        {
            OnEnemyResetButtonPressed();
        }
        GUILayout.EndVertical();
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
        GUILayout.BeginVertical();
        if (GUILayout.Button("Set", GUILayout.Width(50f), GUILayout.Height(50f)) == true)
        {
            OnExpCreateButtonPressed(int.Parse(_expCountText));
        }
        if (GUILayout.Button("Reset", GUILayout.Width(50f), GUILayout.Height(50f)) == true)
        {
            OnExpResetButtonPressed();
        }
        GUILayout.EndVertical();
    }

    private void OnEnemySpawnButtonPressed(int size)
    {
        if ((size <= 0) || (size >= 1000))
            Debug.LogWarning("EnemySpawn size should be in range of 1 ~ 999");

        for(int i = 0; i < size; i++)
        {
            Instantiate(_enemyPrefab, Random.insideUnitCircle * 10f, Quaternion.identity, _testEnemyModule.transform);
        }
    }

    private void OnEnemyResetButtonPressed()
    {
        while(_testEnemyModule.transform.childCount > 0)
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
