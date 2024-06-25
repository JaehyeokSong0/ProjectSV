using UnityEngine;
using UnityEngine.Events;

// Singleton Pattern
// Manages global events
public class EventManager : MonoBehaviour
{ 
    public static EventManager Instance = null;

    public UnityEvent<float> OnPlayerDamaged;
    public UnityEvent OnPlayerDead;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(Instance != this)
                Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        // TEST CODE
        OnPlayerDead?.AddListener(TestFunc);
    }

    private void TestFunc()
    {
        Debug.Log("player dead");
    }
}
