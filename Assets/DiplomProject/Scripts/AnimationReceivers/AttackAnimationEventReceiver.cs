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

    // ���� ����� ���������� �� ��������
    public void AnimationAttackStart()
    {
        _attackHitBox.EnableHitbox();
    }

    // � ���� ����
    public void AnimationAttackEnd()
    {
        _attackHitBox.DisableHitbox();
    }
}
