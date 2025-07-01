using UnityEngine;
using Zenject;

public class EnemyAnimatorController: IEnemyAnimator, IInitializable
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _transform;
    private bool _isInitialized;

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
        if (_isInitialized) return;
        Debug.Log("Animator Controller: " + _animator.runtimeAnimatorController?.name);

        if (_animator.runtimeAnimatorController == null)
        {
            Debug.LogError("[Animator] Контроллер НЕ установлен!");
        }
        else
        {
            Debug.Log("[Animator] Контроллер установлен корректно.");
        }

        _isInitialized = true;
        Debug.Log("[AnimatorController] Initialized");

    }

    public void PlayIdle()
    {
        _animator.SetBool("IsIdle", true);
        _animator.SetBool("IsChasing", false);
        _animator.SetBool("IsAttacking", false);
    }

    public void PlayChase()
    {
        _animator.SetBool("IsIdle", false);
        _animator.SetBool("IsChasing", true);
        _animator.SetBool("IsAttacking", false);
    } 

    public void PlayAttack()
    {
        _animator.SetTrigger("Attack");
    }
    public void PlayDie()
    {
        _animator.SetTrigger("Die");
    }

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