using System.Collections;
using UnityEngine;

public abstract class Base_Skill : MonoBehaviour
{
    protected WaitForSecondsRealtime _timeWait = null;
    protected bool _isValid = true; // true when the skill can apply damage
    protected float _elapsedTime;
    protected Coroutine _checkElapsedTimeCoroutine;
    protected Vector2 _direction = Vector2.zero;

    // Should be assigned in Initialize() in derived class
    public SkillData Data; 
    public GameObject icon;

    protected void OnEnable()
    {
        _isValid = true;
        _elapsedTime = 0f;
    }

    public virtual void Initialize() { }
    public virtual void Initialize(Vector2 position, Vector2 direction) { } // Should initialize data and transform
    public virtual void CastSkill() { }
    protected virtual IEnumerator C_CastSkill() { yield return null; }

    protected void SetTransform(Vector2 position, Vector2 direction)
    {
        gameObject.transform.position = position;
        _direction = direction;
    }

    protected void StartCheckValidation(float tick, float duration) // Used for DOT skills
    {
        _timeWait = new WaitForSecondsRealtime(tick);
        StartCoroutine(C_StartCheckValidation(duration));
    }

    protected IEnumerator C_StartCheckValidation(float duration)
    {
        StartCoroutine(C_CheckElapsedTime());
        while(_elapsedTime < duration)
        {
            yield return null;
            _isValid = false;
            yield return _timeWait;
            _isValid = true;
            yield return null;
        }
        _isValid = false;
        StopCoroutine(C_CheckElapsedTime());
        Destroy(gameObject);
    }

    protected IEnumerator C_CheckElapsedTime()
    {
        while(true)
        {
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}