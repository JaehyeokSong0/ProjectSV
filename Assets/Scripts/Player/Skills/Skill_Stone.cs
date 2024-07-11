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
    [SerializeField] private GameObject _effect;
    private int _stoneCount = 0;
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
        if (_effect == null)
            _effect = transform.Find("Effect").gameObject;
    }

    private void Update()
    {
        if (GameManager.Instance.IsExitPanelActivated == true)
            return;

        if (_stoneCount <= 0)
        {
            StopCheckTime();
            Destroy(gameObject);
        }
            
    }
    #endregion

    #region Method
    protected override IEnumerator C_CastSkill()
    {
        _stoneCount = (int)(Data.Value);
        _effect.gameObject.SetActive(false);
        for (int i = 0; i < Data.Value; i++)
        {
            GameObject newStone = CreateNewStone();
            
            StartCoroutine(C_Rotate(newStone));
            StartCoroutine(C_CheckHitbox(newStone));
        }

        yield return new WaitForSeconds(Data.Duration);
        StopCheckTime();
        Destroy(gameObject);
    }

    private GameObject CreateNewStone()
    {
        // Create new stone
        GameObject stone = Instantiate(_effect, transform.position, Quaternion.identity, transform);
        var randomPosition = Random.insideUnitCircle.normalized * Mathf.Sqrt(Data.Radius);
        stone.transform.Translate(randomPosition);
        stone.SetActive(true);

        return stone;
    }
    private IEnumerator C_CheckHitbox(GameObject stone)
    {
        while (_elapsedTime < Data.Duration)
        {
            var hits = Physics2D.OverlapCircleAll(
                stone.transform.position, STONE_SIZE, EnemyLayerMask);
            foreach (var hit in hits)
            {
                hit.gameObject.GetComponent<Base_EnemyManager>().OnEnemyDamaged(Data.Damage);
                break;
            }
            if (hits.Length > 0)
            {
                _stoneCount--;
                Destroy(stone);
                yield break;
            }
            yield return _fixedWait;
        }

        _stoneCount--;       
        Destroy(stone);
    }

    private IEnumerator C_Rotate(GameObject stone)
    {
        while (_elapsedTime < Data.Duration)
        {
            if(stone == null)
                yield break;

            stone.transform.RotateAround(_playerTransform.position, Vector3.forward, Data.MoveSpeed);
            yield return _fixedWait;
        }
    }
    #endregion
}
// rotate coroutine multiple