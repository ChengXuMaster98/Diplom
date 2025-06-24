using Unity.VisualScripting;
using UnityEngine;
using Zenject;
public class AttackAnimationEventReceiver : MonoBehaviour
{
    private AttackHitBox _attackHitBox;

    public event System.Action OnAttackStart;
    public event System.Action OnAttackEnd;

    [Inject]
    public void Construct(AttackHitBox attackHitBox)
    {
        _attackHitBox = attackHitBox;
    }

    // ЭТОТ метод вызывается из анимации
    public void AnimationAttackStart()
    {
        _attackHitBox.EnableHitbox();
        OnAttackStart?.Invoke();
    }

    // И этот тоже
    public void AnimationAttackEnd()
    {
        _attackHitBox.DisableHitbox();
        OnAttackEnd?.Invoke();
    }
}
