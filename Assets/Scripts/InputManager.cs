using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages input action map
/// Set script execution order if needed
/// </summary>
public class InputManager : MonoBehaviour
{
    #region Enum
    public enum ActionMapType
    { 
        PlayerAction,
        UIAction
    }
    #endregion

    #region Field
    [SerializeField] private PlayerInput _input;
    #endregion

    #region Event Method
    private void OnEnable()
    {
        if (_input == null) 
            _input = GetComponent<PlayerInput>();
    }
    #endregion

    #region Method
    public void SwitchActionMap(ActionMapType actionMap)
    {
        if (_input.currentActionMap.ToString() == actionMap.ToString())
            return;

        _input.SwitchCurrentActionMap(actionMap.ToString());
        Debug.Log($"Switch action map to {actionMap.ToString()}");
    }
    #endregion
}
