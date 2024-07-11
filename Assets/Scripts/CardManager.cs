using UnityEngine;

/// <summary>
/// Temporary card click event manager
/// </summary>
public class CardManager : MonoBehaviour
{
    #region Field
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private PlayerSkillUI _skillUI;
    #endregion

    #region Method
    public void MoveSpeedUp()
    {
        _playerManager.Data.WalkSpeed *= 1.05f;
    }
    public void NormalAttackSpeedUp()
    {
        _playerManager.Data.NormalAttackSpeed /= 0.9f;
    }
    public void NormalAttackDamageUp()
    {
        _playerManager.Data.NormalAttackDamage += 5f;
    }
    public void MpIncrease()
    {
        _playerManager.Data.MaxMp += 1;
        Debug.Log(_playerManager.Data.MaxMp);
        _skillUI.OnMpChanged?.Invoke(_playerManager.Data.CurrentMp, _playerManager.Data.MaxMp);
    }
    public void SkillCountIncrease()
    {
        if(_playerManager.Data.SkillCount < _playerManager.Data.SkillCapacity)
        {
            _skillUI.IncreaseSkillCount();
            _playerManager.Data.SkillCount += 1;
        }
    }
    #endregion
}
