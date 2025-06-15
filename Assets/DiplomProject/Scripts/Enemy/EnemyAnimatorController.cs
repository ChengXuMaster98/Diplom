using UnityEngine;
using Zenject;

public class EnemyAnimatorController: IEnemyAnimator, IInitializable
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _transform;

    public Transform Transform => _transform;

    [Inject]
    public EnemyAnimatorController(Animator animator, Transform transform)
    {
        Debug.Log($"[EnemyAnimatorController] Animator injected: {animator}");
        _animator = animator;
        _transform = transform;
    }

    public void Initialize()
    {
        Debug.Log("[AnimatorController] Initialized");
    }

    public void PlayIdle() => SetBool("IsIdle", true);
    public void StopIdle() => SetBool("IsIdle", false);

    public void PlayChase() => SetBool("IsChasing", true);
    public void StopChase() => SetBool("IsChasing", false);

    public void PlayAttack() => SetBool("IsAttacking", true);
    public void StopAttack() => SetBool("IsAttacking", false);

    public void StopDie() => SetBool("IsDead", true);
    public void PlayDie() => SetBool("IsDead", false);

    private void SetBool(string param, bool value)
    {
        Debug.Log($"[Animator] SetBool: {param} = {value}");
        if (_animator != null && _animator.HasParameter(param))
            _animator.SetBool(param, value);
    }

    public void LookAt(Vector3 position)
    {
        Vector3 direction = (position - _transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            _transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}