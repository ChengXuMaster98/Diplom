using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class KnightEnemyInstaller : MonoInstaller
{
    [SerializeField] private EnemyStats _enemyStats;

    public override void InstallBindings()
    {
        // статы
        Container.Bind<EnemyStats>().FromInstance(_enemyStats).AsSingle();

        // state machine + factory
        Container.BindInterfacesAndSelfTo<KnightStateMachine>().AsSingle();
        Container.BindInterfacesAndSelfTo<KnightStateFactory>().AsSingle();

        // аниматор
        Container.Bind<IKnightAnimator>().To<KnightAnimatorController>()
            .FromComponentOnRoot().AsSingle();

        // NavMeshAgent
        Container.Bind<NavMeshAgent>().FromComponentInHierarchy().AsSingle();

        // базовые компоненты
        Container.Bind<Enemy>().FromComponentInHierarchy().AsSingle();
        Container.Bind<EnemyHealth>().FromComponentInHierarchy().AsSingle();

        Container.BindInterfacesAndSelfTo<SpherePlayerDetector>().FromComponentInHierarchy().AsSingle();

        // AI
        Container.BindInterfacesAndSelfTo<KnightEnemyAI>().FromComponentInHierarchy().AsSingle();
    }
}