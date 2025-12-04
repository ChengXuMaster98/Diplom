using UnityEngine;
using Zenject;
using UnityEngine.AI;

public class EnemyInstaller : MonoInstaller
{
    private Animator _animator;
    private Transform _transform;
    [SerializeField] private EnemyStats _enemyStats;

    public override void InstallBindings()
    {

        Debug.Log("[EnemyInstaller] Выполняется установка зависимостей");


        Container.Bind<EnemyStats>().FromInstance(_enemyStats).AsSingle();


        Container.BindInterfacesAndSelfTo<SpherePlayerDetector>().FromComponentInHierarchy().AsSingle();


        Container.Bind<IEnemyAnimator>().To<EnemyAnimatorController>().FromComponentOnRoot().AsSingle();

        Container.Bind<EnemySoundController>().FromComponentOnRoot().AsSingle();

        // Бинд NavMeshAgent и DetectionArea
        Container.Bind<NavMeshAgent>().FromComponentInHierarchy().AsSingle();



        Container.Bind<IEnemyStateFactory>().To<VampireEnemyStateFactory>().AsSingle();

        Container.BindInterfacesTo<VampireStunBinder>().FromNew().AsSingle().NonLazy();

        // VFX эффекты
        Container.Bind<EnemyVFXController>().FromComponentOnRoot().AsSingle();


        // Состояния врага
        Container.BindInterfacesAndSelfTo<VampireEnemyStateMachine>().AsSingle();

        // База префабов по типам
        Container.Bind<Enemy>().FromComponentInHierarchy().AsSingle();
        Container.Bind<EnemyHealth>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<EnemyAI>().FromComponentOnRoot().AsSingle();

        Debug.Log("Зависимость EnemyAI прокает");
    }
}
