using UnityEngine;
using UnityEngine.UI;

public class ExitPanel : MonoBehaviour
{
    #region Constant
    private readonly Color NON_HIGHLIGHTED_COLOR = new Color(0f / 255f, 0f / 255f, 0f / 255f, 240f / 255f);
    private readonly Color HIGHLIGHTED_COLOR = new Color(120f / 255f, 120f / 255f, 120f / 255f, 240f / 255f);
    #endregion

    #region Field
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _returnButton;
    private bool _isOnReturnButton = true;
    #endregion

    #region Event Method
    private void Start()
    {
        _exitButton.onClick.AddListener(Utility.ExitGame);
        _returnButton.onClick.AddListener(DeactivateExitPanel);
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        _isOnReturnButton = true;
        HighlightReturnButton();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // Enter
        {
            if (_isOnReturnButton == true)
                _returnButton.onClick.Invoke();
            else
                _exitButton.onClick.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_isOnReturnButton == false)
                return;

            _isOnReturnButton = false;
            HighlightExitButton();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_isOnReturnButton == true)
                return;

            _isOnReturnButton = true;
            HighlightReturnButton();
        }
    }
    #endregion

    #region
    public void ActivateExitPanel()
    {
        if (GameManager.Instance.IsExitPanelActivated == false)
        {
            Time.timeScale = 0f;
            GameManager.Instance.IsExitPanelActivated = true;
            gameObject.SetActive(true);
        }
    }
    public void DeactivateExitPanel()
    {
        if (GameManager.Instance.IsExitPanelActivated == true)
        {
            Time.timeScale = 1f;
            GameManager.Instance.IsExitPanelActivated = false;
            gameObject.SetActive(false);
        }
    }
    private void HighlightExitButton()
    {
        _exitButton.gameObject.GetComponent<Image>().color = HIGHLIGHTED_COLOR;
        _returnButton.gameObject.GetComponent<Image>().color = NON_HIGHLIGHTED_COLOR;
    }
    private void HighlightReturnButton()
    {
        _returnButton.gameObject.GetComponent<Image>().color = HIGHLIGHTED_COLOR;
        _exitButton.gameObject.GetComponent<Image>().color = NON_HIGHLIGHTED_COLOR;
    }
    #endregion
}