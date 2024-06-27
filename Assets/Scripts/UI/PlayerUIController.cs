using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] private PlayerManager _manager;
    [SerializeField] private Image _hpGauge;
    [SerializeField] private TMP_Text _hpText;
    // TEST CODE
    [SerializeField] private Transform _skillStartPosition;
    [SerializeField] private Transform _skillEndPosition;
    [SerializeField] private PlayerSkillController _skillController;
    [SerializeField] private GameObject _gravityIcon;
    [SerializeField] private Queue<GameObject> _skillQueue = new Queue<GameObject>();
    private int _currSkillCount = 0;

    private void Awake()
    {
        if (_manager == null)
            _manager = FindObjectOfType<PlayerManager>();
        if (_hpGauge == null)
            _hpGauge = transform.Find("Image_HP").GetComponent<Image>();
        if (_hpText == null)
            _hpText = transform.Find("Image_HP").GetChild(0).GetComponent<TMP_Text>();
    }

    private void LateUpdate()
    {
        _hpGauge.fillAmount = _manager.Data.CurrentHp / _manager.Data.MaxHp;
        _hpText.text = $"{_manager.Data.CurrentHp} / {_manager.Data.MaxHp}";
        if (_currSkillCount > _skillController.GetSkillCount()) // Release Skill
        {
            _currSkillCount = _skillController.GetSkillCount();
            Destroy(_skillQueue.Dequeue());
        }
        else if (_currSkillCount < _skillController.GetSkillCount()) // Get Skill
        {
            _currSkillCount = _skillController.GetSkillCount();
            GameObject skillGO = Instantiate(_gravityIcon, _skillStartPosition);
            _skillQueue.Enqueue(skillGO);
            StartCoroutine(moveIcon(skillGO, _skillStartPosition.position, _skillEndPosition.position));
        }
    }

    private IEnumerator moveIcon(GameObject icon, Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 direction = (endPosition - startPosition).normalized;
        float moveSpeed = 1f;

        while((icon.transform.position - endPosition).magnitude > 0.01f)
        {
            if(icon == null)
                yield break;
            yield return null;
            icon.transform.position += direction * moveSpeed;
        }
    }

    private void ResetIconPosition()
    {

    }
}
