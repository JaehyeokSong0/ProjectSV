using System.Collections;
using UnityEngine;

public class Skill_Stone : Base_Skill
{
    #region Constant
    private const float STONE_SIZE = 0.3f;
    #endregion

    #region Property
    public override SkillRepository.SkillName Name
    {
        get => _name;
        protected set => _name = value;
    }
    public override SkillData Data
    {
        get => _data;
        protected set => _data = value;
    }
    public override GameObject Icon
    {
        get => _icon;
        protected set => _icon = value;
    }
    #endregion

    #region Field
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private SkillRepository.SkillName _name;
    [SerializeField] private SkillData _data;
    [SerializeField] private GameObject _icon;
    private Coroutine _rotateCoroutine;
    #endregion

    #region Event Method
    protected override void Awake()
    {
        base.Awake();

        _name = SkillRepository.SkillName.Stone;
        _playerTransform = transform.root;
        if (_data == null)
            _data = Resources.Load("Data/Skills/StoneData") as SkillData;
        if (_icon == null)
            _icon = Resources.Load("Prefabs/Skills/Icons/Skill_Stone_Icon") as GameObject;

        transform.Translate(new Vector3(Data.Radius, 0f, 0f));
    }
    #endregion

    #region Method
    protected override IEnumerator C_CastSkill()
    {
        yield return null;
        _rotateCoroutine = StartCoroutine(C_Rotate());
        StartCoroutine(C_RotateStone(transform));

        GameObject newStone = CreateNewStone();

        StartCoroutine(C_RotateStone(newStone.transform));
    }

    private GameObject CreateNewStone()
    {
        // Create new stone
        GameObject stone2 = Instantiate(new GameObject(), transform);
        Vector3 stone2Position = transform.position;
        stone2Position.x -= Data.Radius * 2;
        stone2.transform.position = stone2Position;

        return stone2;
    }
    private IEnumerator C_RotateStone(Transform stoneTransform)
    {
        while (_elapsedTime < Data.Duration)
        {
            var hits = Physics2D.OverlapCircleAll(
                stoneTransform.position, STONE_SIZE, EnemyLayerMask);
            foreach (var hit in hits)
            {
                hit.gameObject.GetComponent<Base_EnemyManager>().OnEnemyDamaged(Data.Damage);
            }
            if (hits.Length > 0)
            {
                StopCheckTime();
                StopCoroutine(_rotateCoroutine);
                Destroy(gameObject);
                yield break;
            }
            yield return null;
        }

        StopCheckTime();
        StopCoroutine(_rotateCoroutine);
        Destroy(gameObject);
    }

    private IEnumerator C_Rotate()
    {
        while (_elapsedTime < Data.Duration)
        {
            transform.RotateAround(_playerTransform.position, Vector3.forward, Data.MoveSpeed);
            yield return null;
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(transform.position, STONE_SIZE);
    }
#endif
    #endregion
}
// rotate coroutine multiple