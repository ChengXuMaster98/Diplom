using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class BossEnemyInstaller : MonoInstaller
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

        Container.BindInterfacesAndSelfTo<BossStateMachine>().AsSingle();


        //Вызыет экран победы, когда босс крякнул
        Container.BindInterfacesAndSelfTo<BossDeathListener>().FromComponentInHierarchy().AsSingle();


        Container.BindInterfacesAndSelfTo<BossStateFactory>().AsSingle();



        Container.Bind<IBossAnimator>().To<BossAnimatorController>().FromComponentOnRoot().AsSingle();

        Container.BindInterfacesTo<BossStunBinder>().FromNew().AsSingle().NonLazy();


        // VFX эффекты

        Container.Bind<EnemyVFXController>().FromComponentOnRoot().AsSingle();



        Container.Bind<NavMeshAgent>().FromComponentInHierarchy().AsSingle();

        // Основные компоненты
        Container.BindInterfacesAndSelfTo<BossEnemyAI>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Enemy>().FromComponentInHierarchy().AsSingle();
        Container.Bind<EnemyHealth>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<SpherePlayerDetector>().FromComponentInHierarchy().AsSingle();

    }
}