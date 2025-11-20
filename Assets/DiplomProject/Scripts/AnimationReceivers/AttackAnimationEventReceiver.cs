using Unity.VisualScripting;
using UnityEngine;
using Zenject;
public class AttackAnimationEventReceiver : MonoBehaviour
{
    private AttackHitBox _attackHitBox;

    public event System.Action OnAttackStart;
    public event System.Action OnAttackEnd;

    public WeaponSoundController _weaponSoundController;

    [Inject]
    public void Construct(AttackHitBox attackHitBox, WeaponSoundController weaponSoundController)
    {
        _attackHitBox = attackHitBox;
        _weaponSoundController = weaponSoundController;
    }

    // ЭТОТ метод вызывается из анимации
    public void AnimationAttackStart()
    {
        _attackHitBox.EnableHitbox();
        _weaponSoundController.PlayLightAttack();
        
        OnAttackStart?.Invoke();
    }

    // И этот тоже
    public void AnimationAttackEnd()
    {
        _attackHitBox.DisableHitbox();
        OnAttackEnd?.Invoke();
    }
}