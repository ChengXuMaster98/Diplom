using System.ComponentModel;
using UnityEngine;
using Zenject.SpaceFighter;
using Zenject;
using UnityEngine.AI;

public class EnemyInstaller : MonoInstaller
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _transform;
    public override void InstallBindings()
    {

        Debug.Log("[EnemyInstaller] Выполняется установка зависимостей");

        // Аниматор контроллер врага
        //Container.Bind<Animator>().FromComponentInHierarchy().AsSingle();

        // EnemyAnimatorController как интерфейс
        //Container.BindInterfacesTo<EnemyAnimatorController>().AsSingle().WithArguments(_animator, _transform);
       
        //Container.BindInterfacesTo<EnemyAnimatorController>().AsSingle().WithArguments(_transform);
        Container.Bind<IEnemyAnimator>().To<EnemyAnimatorController>().AsSingle().WithArguments(_animator, _transform).NonLazy();

        // Бинд NavMeshAgent и DetectionArea
        Container.Bind<NavMeshAgent>().FromComponentInHierarchy().AsSingle();
        Container.Bind<DetectionArea>().FromComponentInChildren().AsSingle();


        // Состояния врага
        Container.BindInterfacesAndSelfTo<EnemyStateMachine>().AsSingle();


        // База префабов по типам
        Container.Bind<EnemyHealth>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesTo<EnemyAI>().FromComponentInHierarchy().AsSingle();
        Debug.Log("Зависимость EnemyAI прокает");



        Container.Bind<IEnemyState>().To<EnemyIdleState>().AsTransient().WhenInjectedInto<EnemyStateMachine>();
        Container.Bind<EnemyIdleState>().AsTransient();
        Container.Bind<EnemyChaseState>().AsTransient();
        Container.Bind<EnemyAttackState>().AsTransient();
        Container.Bind<EnemyDieState>().AsTransient();
    }
}
