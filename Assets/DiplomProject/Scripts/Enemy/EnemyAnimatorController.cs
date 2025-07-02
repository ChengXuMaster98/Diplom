using UnityEngine;
using Zenject;

public class EnemyAnimatorController: MonoBehaviour, IEnemyAnimator
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _transform;

    public Transform Transform => _transform;

    private void Awake()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();
    }

    private void Update()
{
    Debug.Log($"Animator State => IsIdle: {_animator.GetBool("IsIdle")}, IsChasing: {_animator.GetBool("IsChasing")}");
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
    public void PlayIdle()
    {
        Debug.Log("PlayIdle called");
        _animator.SetBool("IsIdle", true);
        _animator.SetBool("IsChasing", false);
    }

    public void PlayChase()
    {
        Debug.Log("PlayChase called");
        _animator.SetBool("IsChasing", true);
        _animator.SetBool("IsIdle", false);
    }

    public void PlayAttack()
    {
        _animator.SetTrigger("Attack");
    }
    public void PlayDie()
    {
        _animator.SetTrigger("Die");
    }
}