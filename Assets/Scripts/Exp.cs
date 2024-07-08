using System.Collections;
using UnityEngine;

public class Exp : MonoBehaviour
{
    #region Constant
    private const float MOVE_SPEED = 4f;
    private const float DEAD_REMAIN_TIME = 10f;
    #endregion

    #region Field
    [SerializeField] private CircleCollider2D _collider;
    [SerializeField] private float _amount = 0f;
    #endregion

    #region Event Method
    private void Awake()
    {
        if (_collider == null)
        {
            _collider = GetComponent<CircleCollider2D>();
        }
    }
    #endregion

    #region Event Callback
    public float OnGet(Transform playerTransform)
    {
        _collider.enabled = false;
        StartCoroutine(C_Move(playerTransform));
        return _amount;
    }
    #endregion

    #region Method
    public void Initialize(float amount)
    {
        _amount = amount;
    }
    private IEnumerator C_Move(Transform playerTransform)
    {
        Vector3 direction = playerTransform.position - transform.position;
        while (direction.magnitude > 0.05f)
        {
            transform.position += direction.normalized * MOVE_SPEED * Time.deltaTime;
            direction = playerTransform.position - transform.position;
            yield return null;
        }

        Destroy(gameObject);
    }
    #endregion
}
