using System.Collections;
using UnityEngine;

public abstract class Base_Skill : MonoBehaviour
{
    protected WaitForSecondsRealtime _timeWait = null;
    protected bool _isValid = true; // true when the skill can apply damage
    protected float _elapsedTime;
    protected Coroutine _checkElapsedTimeCoroutine;

    public SkillData Data; // Should be initiailzed in derived class
    protected void OnEnable()
    {
        _isValid = true;
        _elapsedTime = 0f;
    }

    public abstract void Initialize(Vector3 position); // Should initialize data and transform
    public virtual void CastSkill() { }
    protected virtual IEnumerator C_CastSkill() { yield return null; }
    protected void SetTransform(Vector3 position)
    {
        gameObject.transform.position = position;
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