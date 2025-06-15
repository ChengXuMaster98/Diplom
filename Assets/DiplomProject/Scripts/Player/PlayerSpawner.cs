using UnityEngine;
using Zenject;

public class PlayerSpawner : IInitializable
{
    private readonly GameObject _playerPrefab;
    private readonly Transform _spawnPoint;
    private readonly DiContainer _container;

    [Inject]
    public PlayerSpawner(
        GameObject playerPrefab,
        Transform spawnPoint,
        DiContainer container)
    {
        _playerPrefab = playerPrefab;
        _spawnPoint = spawnPoint;
        _container = container;
    }

    public void Initialize()
    {
        Debug.Log("Spawning player...");

        _container.InstantiatePrefab(
            _playerPrefab,
            _spawnPoint.position,
            _spawnPoint.rotation,
            null);
        Debug.Log("Player spawned...");
    }
}