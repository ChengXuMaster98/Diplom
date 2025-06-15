using UnityEngine;
using Zenject;
    public class AttackAnimationEventReceiver: MonoBehaviour
{
    private AttackHitBox _attackHitBox;

    [Inject]
    public void Construct(AttackHitBox attackHitBox)
    {
        _attackHitBox = attackHitBox;
    }

    // ЭТОТ метод вызывается из анимации
    public void AnimationAttackStart()
    {
        _attackHitBox.EnableHitbox();
    }

    // И этот тоже
    public void AnimationAttackEnd()
    {
        _attackHitBox.DisableHitbox();
    }
}
