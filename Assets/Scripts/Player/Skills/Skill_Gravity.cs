using System.Collections;
using UnityEngine;

public class Skill_Gravity : Base_Skill
{
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
    #endregion

    #region Event Method
    protected override void Awake()
    {
        base.Awake();

        if (_data == null)
            _data = Resources.Load("Data/Skills/GravityData") as SkillData;
        if (_icon == null)
            _icon = Resources.Load("Prefabs/Skills/Icons/Skill_Gravity_Icon") as GameObject;
    }
    #endregion

    #region Method
    protected override IEnumerator C_CastSkill()
    {
        WaitForSeconds tickWait =  new WaitForSeconds(Data.Tick);
        while (_elapsedTime < Data.Duration)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(
                transform.position, Data.Radius, Vector2.zero, 0f, EnemyLayer);
            foreach (var hit in hits)
            {
                hit.collider.gameObject.GetComponent<Base_EnemyManager>().OnEnemyDamaged(Data.Damage);
            }
            yield return tickWait;
        }

        StopCheckTime();
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, Data.Radius);
    }
#endif
    #endregion
}
