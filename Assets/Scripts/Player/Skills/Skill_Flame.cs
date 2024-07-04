using System.Collections;
using UnityEngine;

public class Skill_Flame : Base_Skill
{
    #region Constant
    private const float SCALE_FACTOR = 1.8f; // Just scale up its sprite
    private const float EXPLOSION_RANGE = 10f; // Multiplied to default circleCasting range
    private readonly int HASH_ACTIVATED = Animator.StringToHash("activated"); // Trigger activated when explode
    #endregion

    #region Property
    public override SkillData Data
    {
        get => _data;
        set => _data = value;
    }
    public override GameObject Icon
    {
        get => _icon;
        set => _icon = value;
    }
    #endregion

    #region Field
    [SerializeField] private SkillData _data;
    [SerializeField] private GameObject _icon;
    private Coroutine _moveCoroutine;
    private float _radius; // Used to see gizmo dynamically
    #endregion

    #region Event Method
    protected override void Awake()
    {
        base.Awake();

        if (_data == null)
            _data = Resources.Load("Data/Skills/FlameData") as SkillData;
        if (_icon == null)
            _icon = Resources.Load("Prefabs/Skills/Icons/Skill_Flame_Icon") as GameObject;
        if (_animator == null)
            _animator = transform.Find("Effect").GetComponent<Animator>();
    }
    #endregion

    #region Method
    public override void CastSkill()
    {
        _radius = Data.Radius;
        base.CastSkill();
    }
    protected override IEnumerator C_CastSkill()
    {
        // Convert normalized (x,y) to degree
        Vector3 rotationValue = Vector3.forward * Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(rotationValue);

        // Move to the direction
        _moveCoroutine = StartCoroutine(C_Move(_direction, Data.MoveSpeed));

        while (_elapsedTime < Data.Duration)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(
                transform.position, _radius, Vector2.zero, 0f, EnemyLayer);
            if (hits.Length > 0)
            {
                Explode();
                yield break;
            }
            yield return null;
        }

        StopCheckTime();
        Destroy(gameObject);
    }

    private void Explode()
    {
        // Stop moving
        StopCoroutine(_moveCoroutine);

        // Play explosion animation
        _animator.SetTrigger(HASH_ACTIVATED);

        // Set size of explosion effect and hitBox
        transform.localScale *= SCALE_FACTOR;
        _radius *= EXPLOSION_RANGE;

        // Detect enemy and damage them
        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            transform.position, _radius, Vector2.zero, 0f, EnemyLayer);
        foreach (var hit in hits)
        {
            hit.collider.gameObject.GetComponent<Base_EnemyManager>().OnEnemyDamaged(Data.Damage);
        }

        StopCheckTime();
        DestroyAfterAnimation();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, _radius);
    }
#endif
    #endregion
}
