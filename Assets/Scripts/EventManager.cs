using UnityEngine;
using UnityEngine.Events;

// Singleton Pattern
// Manages global events
public class EventManager : MonoBehaviour
{
    public static EventManager Instance = null;

    public UnityEvent<float> OnPlayerDamaged; // {Damage}
    public UnityEvent OnPlayerHPUpdated;
    public UnityEvent OnPlayerDead;
    public UnityEvent<GameObject> OnGetSkill; // {IconResource}
    public UnityEvent OnPlayerLevelUp;
    public UnityEvent OnPlayerExpUpdated;

    public UnityEvent OnGameLevelUp;


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
    }
}
