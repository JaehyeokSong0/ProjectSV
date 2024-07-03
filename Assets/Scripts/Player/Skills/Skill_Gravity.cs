using System.Collections;
using UnityEngine;

public class Skill_Gravity : Base_Skill
{
    public override void Initialize(Vector2 position, Vector2 direction)
    {
        if (data == null)
            data = Resources.Load("Data/Skills/GravityData") as SkillData;
        if (icon == null)
            icon = Resources.Load("Prefabs/Skills/Icons/Skill_Gravity_Icon") as GameObject;

        SetTransform(position, direction);
    }

    public override void CastSkill()
    {
        StartCoroutine(C_CheckElapsedTime());
        StartCoroutine(C_CastSkill());
    }

    protected override IEnumerator C_CastSkill()
    {
        while (elapsedTime < data.duration)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, data.radius, Vector2.zero, 0f, enemyLayer);
            foreach (var hit in hits)
            {
                if (isValid == true)
                {
                    hit.collider.gameObject.GetComponent<Base_EnemyManager>().OnEnemyDamaged(data.damage);
                }
            }
            yield return new WaitForSeconds(data.tick);
        }

        isValid = false;
        StopCoroutine(C_CheckElapsedTime());
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, data.radius);
    }
#endif
}
