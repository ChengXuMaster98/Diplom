using UnityEngine;
using Zenject;

public class EnemyInstaller : MonoInstaller
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _enemyGameObject;
    [SerializeField] private float _chaseSpeed = 3f;

    public override void InstallBindings()
    {
        Container.Bind<EnemyAnimatorController>()
            .AsSingle()
            .WithArguments(_animator);

        Container.BindInterfacesAndSelfTo<EnemyStateMachine>().AsSingle();

        Container.Bind<IEnemyState>().WithId("Idle")
            .To<EnemyIdleState>().AsTransient()
            .WithArguments(Container.Resolve<EnemyAnimatorController>());

        Container.Bind<IEnemyState>().WithId("Patrol")
            .To<EnemyPatrolState>().AsTransient()
            .WithArguments(Container.Resolve<EnemyAnimatorController>(), Container.Resolve<EnemyPatrolPath>());

        Container.Bind<IEnemyState>().WithId("Chase")
            .To<EnemyChaseState>().AsTransient()
            .WithArguments(transform, _player, Container.Resolve<EnemyAnimatorController>(), _chaseSpeed);

        Container.Bind<IEnemyState>().WithId("Attack")
            .To<EnemyAttackState>().AsTransient()
            .WithArguments(Container.Resolve<EnemyAnimatorController>(), Container.Resolve<IPlayerDamageable>());

        Container.Bind<IEnemyState>().WithId("Die")
            .To<EnemyDieState>().AsTransient()
            .WithArguments(Container.Resolve<EnemyAnimatorController>(), _enemyGameObject);
    }
}