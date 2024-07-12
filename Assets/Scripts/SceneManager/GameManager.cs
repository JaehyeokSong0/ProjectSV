using UnityEngine;

/// <summary>
/// Singleton Pattern
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Property
    public bool IsExitPanelActivated
    {
        get { return _isExitPanelActivated; }
        set { _isExitPanelActivated = value; }
    }
    public float BestRecord
    {
        get { return _bestRecord; }
        set
        {
            if (value <= _bestRecord)
                return;
            _bestRecord = value;
        }
    }
    public int GameLevel
    {
        get { return _gameLevel; }
        set
        {
            if (value <= 0)
                return;
            _gameLevel = value;
        }
    }
    #endregion

    #region Field
    public static GameManager Instance = null;
    [SerializeField] private GameObject _exitPanel;
    private bool _isExitPanelActivated = false;
    private float _bestRecord = 0f;
    private int _gameLevel = 1;
    #endregion

    #region Event Method
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
                Destroy(this.gameObject);
        }

        if (_exitPanel == null)
            _exitPanel = transform.Find("ExitPanel").gameObject;
    }

    private void Update()
    {
        if (_isExitPanelActivated == true)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActivateExitPanel();
        }
    }
    #endregion

    #region Method
    /// <summary>
    /// Activate exit panel
    /// Should be binded to key ESC
    /// </summary>
    public void ActivateExitPanel()
    {
        _exitPanel.GetComponent<ExitPanel>().ActivateExitPanel();
    }
    #endregion
}
