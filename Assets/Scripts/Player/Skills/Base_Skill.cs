using System.Collections;
using UnityEngine;

public abstract class Base_Skill : MonoBehaviour
{
    protected int enemyLayer;

    protected Animator animator = null; // Can be null
    protected WaitForSecondsRealtime timeWait = null;
    protected bool isValid = true; // True when the skill can apply damage
    protected float elapsedTime;
    protected Coroutine checkElapsedTimeCoroutine;
    protected Vector2 direction = Vector2.zero;

    // Should be assigned in Initialize() in derived class
    public SkillData data; 
    public GameObject icon;

    protected void Awake()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    protected void OnEnable()
    {
        isValid = true;
        elapsedTime = 0f;
    }

    public virtual void Initialize() { }
    public virtual void Initialize(Vector2 position, Vector2 direction) { } // Should initialize data and transform
    public virtual void CastSkill() { }
    protected virtual IEnumerator C_CastSkill() { yield return null; }

    protected void SetTransform(Vector2 position, Vector2 direction)
    {
        gameObject.transform.position = position;
        this.direction = direction;
    }

    protected IEnumerator Move(Vector2 direction, float speed)
    {
        while (elapsedTime < data.duration)
        {
            transform.position += new Vector3(direction.x, direction.y, 0f) * speed * Time.deltaTime;
            yield return null;
        }
    }

    protected void StartCheckValidation(float tick, float duration) // Used for DOT skills
    {
        timeWait = new WaitForSecondsRealtime(tick);
        StartCoroutine(C_StartCheckValidation(duration));
    }

    protected IEnumerator C_StartCheckValidation(float duration)
    {
        StartCoroutine(C_CheckElapsedTime());
        while(elapsedTime < duration)
        {
            yield return null;
            isValid = false;
            yield return timeWait;
            isValid = true;
            yield return null;
        }
        isValid = false;
        StopCoroutine(C_CheckElapsedTime());
        Destroy(gameObject);
    }

    protected IEnumerator C_CheckElapsedTime()
    {
        while(true)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    protected void DestroyAfterAnimation()
    {
        StartCoroutine(C_DestroyAfterAnimation());
    }

    protected IEnumerator C_DestroyAfterAnimation()
    {
        float animationTime = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationTime);
        Destroy(gameObject);
    }
}