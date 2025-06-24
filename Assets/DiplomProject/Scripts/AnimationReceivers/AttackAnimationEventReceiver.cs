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

    // ���� ����� ���������� �� ��������
    public void AnimationAttackStart()
    {
        _attackHitBox.EnableHitbox();
        OnAttackStart?.Invoke();
    }

    // � ���� ����
    public void AnimationAttackEnd()
    {
        _attackHitBox.DisableHitbox();
        OnAttackEnd?.Invoke();
    }
}
