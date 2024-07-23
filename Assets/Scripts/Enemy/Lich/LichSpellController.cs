using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LichSpellController : MonoBehaviour
{
    public class SpellDescription
    {
        public float Speed;
        public float Duration;
        public float Tick;
        public float Radius;
    }

    private readonly string PLAYER_LAYER = "Player";

    [SerializeField] private LichManager _manager;
    [SerializeField] private Animator _animator;

    private SpellDescription _lightningDescription;
    private Vector3 _direction;
    private WaitForSecondsRealtime _tickWait;
    private int _playerLayer;


    private void Awake()
    {
        if (_manager == null)
            _manager = transform.parent.GetComponent<LichManager>();
        if(_animator == null)
            _animator = GetComponent<Animator>();

        _lightningDescription = new SpellDescription { Speed = 0.8f, Duration = 4f, Tick = 0.7f, Radius = 0.2f };

        // Always need to be shorter than normal attack delay
        if (_manager.Data.NormalAttackSpeed <= _lightningDescription.Duration)
            _lightningDescription.Duration = _manager.Data.NormalAttackSpeed - 0.1f;

        _tickWait = new WaitForSecondsRealtime(_lightningDescription.Tick);
        _playerLayer = LayerMask.NameToLayer(PLAYER_LAYER);
    }

    public void CastSpell(Vector3 direction)
    {
        transform.position = _manager.transform.position;
        _direction = direction.normalized;
        StartCoroutine(C_CastSpell());
        StartCoroutine(C_DisableOnTime(_lightningDescription.Duration));
    }

    private IEnumerator C_CastSpell()
    {
        while(true)
        {
            transform.position += _direction * _lightningDescription.Speed;
            _animator.SetTrigger("activated");
            var hit = Physics2D.OverlapCircle(transform.position, _lightningDescription.Radius, 1 << _playerLayer);
            if(hit != null)
            {
                EventManager.Instance.OnPlayerDamaged?.Invoke(_manager.Data.NormalAttackDamage);
            }

            yield return _tickWait;
        }
    }

    private IEnumerator C_DisableOnTime(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Color gizmoColor = new Color(1, 0, 1);
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, _lightningDescription.Radius);
    }
}
