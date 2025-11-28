using Cinemachine;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private Player _player;
    [SerializeField] private EnemyStatsDatabase _enemyStatsDatabase;
    [SerializeField] private EnemyPrefabDatabase _enemyPrefabDatabase;
    [SerializeField] private MusicDatabase _musicDatabase;
    [SerializeField] private AttackHitBox _attackHitBox;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask Ground;

    //ThirdPersonCamera
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private CinemachineFreeLook _freeLookCamera;

    //FirstPersonCamera
    [SerializeField] private CinemachineVirtualCamera _firstPersonCamera;
    [SerializeField] private Transform _fpsCameraHolder;
    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private Transform _head;


    [SerializeField] private StaminaConfig _staminaConfig;

    [SerializeField] private UpgradeDatabase _upgradeDatabase;


    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private StaminaBar _staminaBar;
    [SerializeField] private LowHealthEffect _lowHealthEffect;

    public WeaponDatabase weaponDatabase;

    public override void InstallBindings()
    {

        //Menu
        
        // Pause
        Container.Bind<IPauseService>().To<PauseService>().AsSingle();

        // SaveLoadController, чтобы можно было инжектить в UI и др.
        Container.Bind<SaveLoadController>().FromComponentInHierarchy().AsSingle();

        // UI-менюшки
        Container.Bind<MainMenuUI>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PauseMenuUI>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GameWonUI>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GameOverUI>().FromComponentInHierarchy().AsSingle();
        Container.Bind<UIVisibilityController>().FromComponentInHierarchy().AsSingle();


        // Контроллер паузы (обрабатывает ESC)
        Container.BindInterfacesAndSelfTo<PauseMenuController>().AsSingle();

        Container.BindInterfacesAndSelfTo<SaveExecutor>().AsSingle();



        //WeaponSystem
        Container.Bind<WeaponFactory>().AsSingle();
        Container.Bind<PlayerWeaponInventory>().AsSingle();

        Container.BindInstance(weaponDatabase).AsSingle();



        //Save
        Container.BindInterfacesAndSelfTo<SaveService>().AsSingle().NonLazy();
        Container.Bind<EnemySaveSystem>().AsSingle();
        Container.Bind<PickupSaveSystem>().AsSingle();



        //PlayerUI
        Container.Bind<HealthBar>().FromInstance(_healthBar).AsSingle();
        Container.Bind<StaminaBar>().FromInstance(_staminaBar).AsSingle();
        Container.Bind<LowHealthEffect>().FromInstance(_lowHealthEffect).AsSingle();

        Container.BindInterfacesAndSelfTo<PlayerUIController>().AsSingle();


        //Audio for aggro and Enviroment
        Container.Bind<EnemyAggroTracker>().AsSingle();
        Container.BindInterfacesAndSelfTo<MusicController>().AsSingle().NonLazy();
        Container.Bind<MusicDatabase>().FromInstance(_musicDatabase).AsSingle();



        // Audio
        Container.Bind<AudioSourcePool>().AsSingle().NonLazy();
        Container.Bind<AudioManager>().AsSingle().NonLazy();


        Container.Bind<AnimationEventReceiver>().FromComponentInHierarchy().AsSingle();


        Container.Bind<WeaponSoundController>().FromComponentInHierarchy().AsSingle();


        Container.Bind<IPlayerAudio>().To<PlayerSoundController>().FromComponentInHierarchy().AsSingle();


        // ScriptableObject
        Container.Bind<PlayerStats>().FromInstance(_playerStats).AsSingle();
        Container.Bind<StaminaConfig>().FromInstance(_staminaConfig).AsSingle();

        Container.Bind<UpgradeDatabase>().FromInstance(_upgradeDatabase).AsSingle();


        Container.BindInterfacesAndSelfTo<StaminaSystem>().AsSingle();
        //Container.BindTickableExecutionOrder<StaminaSystem>(-100);

        Container.Bind<IPlayerStaminaConsumer>().To<PlayerStaminaAdapter>().AsSingle();


        Container.BindInterfacesAndSelfTo<PlayerHealth>().FromComponentInHierarchy().AsSingle();


        // Player
        Container.Bind<Player>().FromInstance(_player).AsSingle();


        // Input
        Container.Bind<IInputService>().To<InputService>().AsSingle();
        
        Container.BindInterfacesAndSelfTo<UpgradeService>().AsSingle().NonLazy();

        // First-person movement controller (Camera + Movement)
        Container.BindInterfacesTo<FirstPersonController>().AsSingle(); // ITickable
        
        Container.BindInterfacesAndSelfTo<CharacterMovementController>().AsSingle().WithArguments(_camera.transform, _groundCheck, Ground);

        // First-person camera setup
        Container.Bind<CinemachineVirtualCamera>().FromInstance(_firstPersonCamera).AsSingle();
        Container.BindInterfacesTo<CameraSwitcher>().AsSingle().WithArguments(_head);

        // Third-person camera (если используешь)
        Container.Bind<CinemachineFreeLook>().FromInstance(_freeLookCamera).AsSingle();
        
        //Container.Bind<Transform>().FromInstance(_freeLookCamera.transform).AsSingle(); // cameraTransform
        Container.BindInterfacesTo<ThirdPersonCameraController>().AsSingle().WithArguments(_cameraTarget);


        // Враги и их фабрика
        Container.Bind<EnemyStatsDatabase>().FromInstance(_enemyStatsDatabase).AsSingle();
        Container.Bind<EnemyPrefabDatabase>().FromInstance(_enemyPrefabDatabase).AsSingle();
        Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle().NonLazy();

        // Урон, атака
        Container.Bind<AttackHitBox>().FromInstance(_attackHitBox).AsSingle();
        Container.Bind<AttackAnimationEventReceiver>().FromComponentInHierarchy().AsSingle();

        // Player FSM
        Container.Bind<PlayerStateMachine>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerStateController>().AsSingle();
        //Container.Bind<PlayerAttackState>().AsSingle();
        //Container.Bind<PlayerJumpState>().AsSingle();
    }
}