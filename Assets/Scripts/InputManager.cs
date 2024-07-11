using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Singleton Pattern
/// Manages input action map
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
    public static InputManager Instance = null;
    [SerializeField] private PlayerInput _input;
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
        if (_input == null) 
            _input = GetComponent<PlayerInput>();
    }
    #endregion

    #region Method
    public void SwitchActionMap(ActionMapType actionMap)
    {
        _input.SwitchCurrentActionMap(actionMap.ToString());
        Debug.Log($"Switch action map to {actionMap.ToString()}");
    }
    #endregion
}
