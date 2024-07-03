using System.Collections;
using UnityEngine;

public class Skill_Gravity : Base_Skill
{
    public override void Initialize(Vector2 position, Vector2 direction)
    {
        if(Data == null)
            Data = Resources.Load("Data/Skills/GravityData") as SkillData;
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
        while (_elapsedTime < Data.Duration)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, Data.Radius, Vector2.zero);
            foreach (var hit in hits)
            {
                GameObject hitGO = hit.collider.gameObject;
                if (hitGO.CompareTag("Enemy") == true) // TODO -> LayerMask
                {
                    if (_isValid == true)
                    {
                        hitGO.GetComponent<Base_EnemyManager>().OnEnemyDamaged(Data.Damage);
                    }
                }
            }
            yield return new WaitForSeconds(Data.Tick);
        }

        _isValid = false;
        StopCoroutine(C_CheckElapsedTime());
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, Data.Radius);
    }
}
