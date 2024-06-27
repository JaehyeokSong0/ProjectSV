using UnityEngine;

[RequireComponent(typeof(CircleCollider2D)), RequireComponent(typeof(Rigidbody2D))]

public class Skill_Gravity_HitBox : MonoBehaviour
{
    [SerializeField] private Skill_Gravity _skill;

    private void Awake()
    {
        if (_skill == null)
            _skill = transform.parent.GetComponent<Skill_Gravity>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") == true)
        {
            if (_skill.IsValid == true)
            {                
                collision.gameObject.GetComponent<Base_EnemyManager>().OnEnemyDamaged(_skill.Data.Damage);
            }
        }
    }
}
