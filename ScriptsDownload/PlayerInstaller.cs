using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _spawnPoint;

    public override void InstallBindings()
    {
        Container.Bind<PlayerStats>().FromInstance(_playerStats).AsSingle();
        Container.Bind<PlayerMovement>().FromComponentOn(_player).AsSingle();
        Container.Bind<PlayerAttack>().FromComponentOn(_player).AsSingle();
        Container.Bind<PlayerHealth>().FromComponentOn(_player).AsSingle();
        Container.Bind<Transform>().WithId("SpawnPoint").FromInstance(_spawnPoint).AsSingle();
    }
}