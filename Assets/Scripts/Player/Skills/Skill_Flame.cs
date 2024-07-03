using System.Collections;
using UnityEngine;

public class Skill_Flame : Base_Skill
{
    [SerializeField] private Animator animator;
    Coroutine moveCoroutine;
    float radius;
    public override void Initialize(Vector2 position, Vector2 direction)
    {
        if (Data == null)
            Data = Resources.Load("Data/Skills/FlameData") as SkillData;
        if (icon == null)
            icon = Resources.Load("Prefabs/Skills/Icons/Skill_Flame_Icon") as GameObject;
        if (animator == null)
            animator = transform.Find("Effect").GetComponent<Animator>();
        SetTransform(position, direction);
    }

    public override void CastSkill()
    {
        radius = Data.Radius;
        StartCoroutine(C_CastSkill());
    }

    protected override IEnumerator C_CastSkill()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg));
        moveCoroutine = StartCoroutine(Move(_direction));
        while (_elapsedTime < Data.Duration)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero);
            foreach (var hit in hits)
            {
                GameObject hitGO = hit.collider.gameObject;
                if (hitGO.CompareTag("Enemy") == true) // TODO -> LayerMask
                {
                    Explode();
                    yield break;
                }
            }
            yield return null;
        }

        Destroy(gameObject);
    }

    private IEnumerator Move(Vector2 direction)
    {
        while (_elapsedTime < Data.Duration)
        {
            transform.position += new Vector3(direction.x, direction.y, 0f) * 4f * Time.deltaTime;
            yield return null;
        }
    }

    private void Explode()
    {
        StopCoroutine(moveCoroutine);
        animator.SetTrigger("activated");
        transform.localScale *= 1.8f;
        radius *= 10f;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero); // TODO = Radius * Explosion scope
        foreach (var hit in hits)
        {
            GameObject hitGO = hit.collider.gameObject;
            if (hitGO.CompareTag("Enemy") == true) // TODO -> LayerMask
                hitGO.GetComponent<Base_EnemyManager>().OnEnemyDamaged(Data.Damage, 1f);
        }
        StartCoroutine(C_Explode());
    }

    private IEnumerator C_Explode()
    {
        float animationTime = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationTime);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
