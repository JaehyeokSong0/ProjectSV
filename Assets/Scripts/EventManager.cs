using UnityEngine;
using UnityEngine.Events;

// Singleton Pattern
// Manages global events
public class EventManager : MonoBehaviour
{ 
    public static EventManager instance = null;

    public UnityEvent<float> OnPlayerDamaged; // {Damage}
    public UnityEvent OnPlayerDead;
    public UnityEvent<GameObject> OnGetSkill; // {IconResource}
    public UnityEvent OnUseSkill;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance != this)
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
