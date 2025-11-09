using UnityEngine;
using Zenject;

public class SkeletonEnemyInstaller : MonoInstaller
{
    [SerializeField] private EnemyStats _enemyStats;

    public override void InstallBindings()
    {

        Debug.Log("SkeletonEnemyInstaller BINDING EXECUTED");

        // Статы
        Container.Bind<EnemyStats>().FromInstance(_enemyStats).AsSingle();

        // Машина и фабрика состояний
        //Container.Bind<ISkeletonStateFactory>().To<SkeletonStateFactory>().AsSingle();
        //Container.Bind<ISkeletonStateMachine>().To<SkeletonStateMachine>().AsSingle();

        Container.BindInterfacesAndSelfTo<SkeletonStateMachine>().AsSingle();

        // Основные компоненты
        Container.BindInterfacesAndSelfTo<SkeletonEnemyAI>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Enemy>().FromComponentInHierarchy().AsSingle();
        Container.Bind<EnemyHealth>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IPlayerDetector>().To<SpherePlayerDetector>().FromComponentInHierarchy().AsSingle();

    }
}