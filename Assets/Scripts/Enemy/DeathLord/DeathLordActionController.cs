using System.Collections;
using UnityEngine;

public class DeathLordActionController : Base_EnemyActionController
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private WaitForSeconds _colorChangeTime;

    private void Awake()
    {
        if(_spriteRenderer == null)
            _spriteRenderer = transform.Find("Model").GetComponent<SpriteRenderer>();
    }
    protected override void Start()
    {
        base.Start();
        _animationController = transform.Find("Model").GetComponent<DeathLordAnimationController>();
        _colorChangeTime = new WaitForSeconds(0.5f);
    }

    public override void TakeDamage(float damage, float coolTime)
    {
        base.TakeDamage(damage, coolTime);
        StartCoroutine(ChangeSpriteColor(Color.red));
    }

    private IEnumerator ChangeSpriteColor(Color color)
    {
        Color colorBuffer = _spriteRenderer.color;
        _spriteRenderer.color = color;

        yield return _colorChangeTime;

        _spriteRenderer.color = colorBuffer;
    }
}
