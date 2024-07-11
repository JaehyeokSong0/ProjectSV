using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputController : MonoBehaviour
{
    #region Field
    [SerializeField] private LevelUpUI _levelUpUI;
    #endregion

    #region Event Method
    private void Awake()
    {
        if (_levelUpUI == null)
            _levelUpUI = FindObjectOfType<LevelUpUI>();
    }
    #endregion

    #region Event Callback
    public void OnKey1Pressed(InputAction.CallbackContext context)
    {
        if (context.performed == true)
            _levelUpUI.SelectCard(0);
    }
    public void OnKey2Pressed(InputAction.CallbackContext context)
    {
        if (context.performed == true)
            _levelUpUI.SelectCard(1);
    }
    public void OnKey3Pressed(InputAction.CallbackContext context)
    {
        if (context.performed == true)
            _levelUpUI.SelectCard(2);
    }
    #endregion
}
