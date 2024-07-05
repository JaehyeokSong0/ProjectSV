using System.Collections;
using UnityEngine;

public abstract class Base_Skill : MonoBehaviour
{
    #region Constant
    private readonly string _enemyLayer = "Enemy";
    #endregion

    #region Property
    public LayerMask EnemyLayerMask => _enemyLayerMask;
    public abstract SkillRepository.SkillName Name { get; protected set; }
    public abstract SkillData Data { get; protected set; }
    public abstract GameObject Icon { get; protected set; }
    #endregion

    #region Field
    protected Animator _animator = null; // Can be null
    protected Vector2 _direction = Vector2.zero; // Normalized direction
    protected float _elapsedTime = 0f;
    protected Coroutine _checkTimeCoroutine = null;
    protected SkillRepository.SkillName _skillName;

    private LayerMask _enemyLayerMask;
    #endregion

    #region Event Method
    protected virtual void Awake()
    {
        _enemyLayerMask = LayerMask.GetMask(_enemyLayer);
    }
    #endregion

    #region Method
    public virtual void Initialize() { }
    public virtual void Initialize(Vector2 position, Vector2 direction)
    {
        gameObject.transform.position = position;
        _direction = direction;
    }
    /// <summary>
    /// Set skill data if needed
    /// </summary>
    public virtual void CastSkill()
    {
        StartCheckTime();
        StartCoroutine(C_CastSkill());
    }
    /// <summary>
    /// Implement skill logic
    /// </summary>
    protected virtual IEnumerator C_CastSkill() { yield return null; }
    protected IEnumerator C_Move(Vector2 direction, float speed)
    {
        Vector3 moveDirection = direction;
        while (_elapsedTime < Data.Duration)
        {
            transform.position += moveDirection * speed * Time.deltaTime;
            yield return null;
        }
    }
    protected void StartCheckTime()
    {
        _elapsedTime = 0f;
        _checkTimeCoroutine = StartCoroutine(C_CheckElapsedTime());
    }
    protected void StopCheckTime()
    {
        StopCoroutine(_checkTimeCoroutine);
    }
    protected IEnumerator C_CheckElapsedTime()
    {
        while (_elapsedTime < Data.Duration)
        {
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    protected void DestroyAfterAnimation()
    {
        if (_animator == null)
            Debug.LogError("Animator not found");
        StartCoroutine(C_DestroyAfterAnimation());
    }
    protected IEnumerator C_DestroyAfterAnimation()
    {
        float animationTime = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationTime);
        Destroy(gameObject);
    }
    #endregion
}