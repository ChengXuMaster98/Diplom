using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class SkinnyOrkEnemyInstaller : MonoInstaller
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

        Container.BindInterfacesAndSelfTo<SkinnyOrkStateMachine>().AsSingle();

        Container.Bind<EnemySoundController>().FromComponentOnRoot().AsSingle();



        Container.BindInterfacesAndSelfTo<SkinnyOrkStateFactory>().AsSingle();



        Container.Bind<ISkinnyOrkAnimator>().To<SkinnyOrkAnimatorController>().FromComponentOnRoot().AsSingle();

        Container.BindInterfacesTo<SkinnyOrkStunBinder>().FromNew().AsSingle().NonLazy();


        // VFX эффекты

        Container.Bind<EnemyVFXController>().FromComponentOnRoot().AsSingle();



        Container.Bind<NavMeshAgent>().FromComponentInHierarchy().AsSingle();

        // Основные компоненты
        Container.BindInterfacesAndSelfTo<SkinnyOrkEnemyAI>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Enemy>().FromComponentInHierarchy().AsSingle();
        Container.Bind<EnemyHealth>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<SpherePlayerDetector>().FromComponentInHierarchy().AsSingle();

    }
}