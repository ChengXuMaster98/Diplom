using UnityEngine;
public class AttackAnimationEventReceiver : MonoBehaviour
{
    private AttackHitBox _attackHitBox;

    public event System.Action OnAttackStart;
    public event System.Action OnAttackEnd;
    public event System.Action OnSwing;


    public void SetHitBox(AttackHitBox hitbox)
    {
        _attackHitBox = hitbox;
    }


    // ЭТОТ метод вызывается из анимации
    public void AnimationAttackStart()
    {
        if (_attackHitBox == null)
        {
            Debug.LogError("[AttackEvent] HitBox не назначен!");
            return;
        }

        _attackHitBox.EnableHitbox();
        OnAttackStart?.Invoke();
    }

    // И этот тоже
    public void AnimationAttackEnd()
    {
        _attackHitBox.DisableHitbox();
        OnAttackEnd?.Invoke();
    }

    public void AnimationSwing()
    {
        OnSwing?.Invoke();
    }
}