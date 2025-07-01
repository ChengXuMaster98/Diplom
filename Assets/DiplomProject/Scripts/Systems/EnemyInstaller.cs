using System.ComponentModel;
using UnityEngine;
using Zenject.SpaceFighter;
using Zenject;
using UnityEngine.AI;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class EnemyInstaller : MonoInstaller
{
    private Animator _animator;
    private Transform _transform;
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
        //Container.Bind<SpherePlayerDetector>().FromComponentInChildren().AsSingle();

        Container.Bind<IPlayerDetector>().To<SpherePlayerDetector>().FromComponentInChildren().AsSingle();
        Container.Bind<IEnemyStateFactory>().To<VampireEnemyStateFactory>().AsSingle();



        // Состояния врага
        Container.BindInterfacesAndSelfTo<VampireEnemyStateMachine>().AsSingle();

        // База префабов по типам
        Container.Bind<EnemyHealth>().FromComponentInHierarchy().AsSingle();
        Container.Bind<EnemyAI>().FromComponentInHierarchy().AsSingle();
        Debug.Log("Зависимость EnemyAI прокает");



        Container.Bind<IEnemyState>().To<VampireEnemyIdleState>().AsTransient().WhenInjectedInto<VampireEnemyStateMachine>();
        Container.Bind<VampireEnemyIdleState>().AsTransient();
        Container.Bind<VampireEnemyChaseState>().AsTransient();
        Container.Bind<VampireAttackState>().AsTransient();
        Container.Bind<VampireEnemyDieState>().AsTransient();
    }
}
