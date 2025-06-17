using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private PlayerStateSaver _playerState;
    [SerializeField] private Player _player;
    [SerializeField] private EnemyStatsDatabase _enemyStatsDatabase;
    [SerializeField] private EnemyPrefabDatabase _enemyPrefabDatabase;
    [SerializeField] private AttackHitBox _attackHitBox;


    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private CinemachineFreeLook _freeLookCamera;

    [SerializeField] private StaminaConfig _staminaConfig;


    public override void InstallBindings()
    {
        // ScriptableObject
        Container.Bind<PlayerStats>().FromInstance(_playerStats).AsSingle();
        Container.Bind<StaminaConfig>().FromInstance(_staminaConfig).AsSingle();

        Container.BindInterfacesAndSelfTo<StaminaSystem>().AsSingle();

        Container.Bind<IPlayerStaminaConsumer>().To<PlayerStaminaAdapter>().AsSingle();

        Container.BindTickableExecutionOrder<StaminaSystem>(-100);


        // Player Health
        Container.BindInterfacesAndSelfTo<PlayerHealth>().AsSingle();

        //Хитбокс атаки
        Container.Bind<AttackHitBox>().FromInstance(_attackHitBox).AsSingle();

        Container.Bind<PlayerStateSaver>().FromInstance(_playerState).AsSingle();
        Container.Bind<Player>().FromInstance(_player).AsSingle();

        // Враг
        Container.Bind<EnemyStatsDatabase>().FromInstance(_enemyStatsDatabase).AsSingle();
        Container.Bind<EnemyPrefabDatabase>().FromInstance(_enemyPrefabDatabase).AsSingle();

        // Фабрика врагов
        Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle().NonLazy();

        // Дфижение игрока и поворот камеры с телом
        Container.Bind<CinemachineFreeLook>().FromInstance(_freeLookCamera).AsSingle();
        Container.Bind<Transform>().FromInstance(_freeLookCamera.transform).AsSingle(); // cameraTransform

        Container.BindInterfacesAndSelfTo<CharacterMovementController>().AsSingle();
        //Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesTo<ThirdPersonCameraController>().AsSingle().WithArguments(_cameraTarget);



        Container.Bind<PlayerStateMachine>().AsSingle();

        Container.BindInterfacesAndSelfTo<PlayerStateController>().AsSingle();

        Container.Bind<PlayerAttackState>().AsSingle();
        Container.Bind<PlayerJumpState>().AsSingle();

    }
}