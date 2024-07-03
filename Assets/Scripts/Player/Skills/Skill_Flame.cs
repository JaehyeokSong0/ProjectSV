using System.Collections;
using UnityEngine;

public class Skill_Flame : Base_Skill
{
    private const float GO_SCALE_FACTOR = 1.8f; // Just scale up its GO
    private const float EXPLOSION_RANGE = 10f; // Affect circleCast scope of explosion
    private readonly int HASH_ACTIVATED = Animator.StringToHash("activated");

    private Coroutine _moveCoroutine;
    private float _radius; // Used to see gizmo dynamically

    public override void Initialize(Vector2 position, Vector2 direction)
    {
        if (Data == null)
            Data = Resources.Load("Data/Skills/FlameData") as SkillData;
        if (icon == null)
            icon = Resources.Load("Prefabs/Skills/Icons/Skill_Flame_Icon") as GameObject;
        _animator = transform.Find("Effect").GetComponent<Animator>();

        SetTransform(position, direction);
    }

    public override void CastSkill()
    {
        _radius = Data.Radius;
        StartCoroutine(C_CastSkill());
    }

    protected override IEnumerator C_CastSkill()
    {
        Vector3 rotationValue = Vector3.forward * Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(rotationValue);

        _moveCoroutine = StartCoroutine(Move(_direction, Data.MoveSpeed));

        while (_elapsedTime < Data.Duration)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _radius, Vector2.zero, 0f, ENEMY_LAYER);
            if(hits.Length > 0)
            {
                Explode();
                yield break;
            }
            yield return null;
        }

        Destroy(gameObject);
    }

    
    private void Explode()
    {
        StopCoroutine(_moveCoroutine);

        _animator.SetTrigger(HASH_ACTIVATED);
        DestroyAfterAnimation();

        transform.localScale *= GO_SCALE_FACTOR;
        _radius *= EXPLOSION_RANGE;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _radius, Vector2.zero, 0f, ENEMY_LAYER);
        foreach (var hit in hits)
        {
            hit.collider.gameObject.GetComponent<Base_EnemyManager>().OnEnemyDamaged(Data.Damage);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, _radius);
    }
#endif
}
