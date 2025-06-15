using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private PlayerStateSaver _playerState;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _spawnPoint;

    public override void InstallBindings()
    {
        Container.Bind<PlayerStats>().FromInstance(_playerStats).AsSingle();
        Container.Bind<PlayerStateSaver>().FromInstance(_playerState).AsSingle();

        Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();
        Container.Bind<IInitializable>().To<PlayerSpawner>().AsSingle().WithArguments(_playerPrefab, _spawnPoint);
    }
}