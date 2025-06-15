using Zenject;
using UnityEngine;

public class EnemyFactory : IEnemyFactory
{
    private readonly DiContainer _container;
    private readonly EnemyPrefabDatabase _enemyPrefabDatabase;
    private readonly EnemyStatsDatabase _enemyStatsDatabase;

    public EnemyFactory(DiContainer container, EnemyPrefabDatabase prefabDatabase, EnemyStatsDatabase statsDatabase)
    {
        _container = container;
        _enemyPrefabDatabase = prefabDatabase;
        _enemyStatsDatabase = statsDatabase;
    }

    public Enemy Create(EnemyType type, Vector3 position)
    {
        var prefab = _enemyPrefabDatabase.GetPrefabByType(type);
        var enemy = _container.InstantiatePrefabForComponent<Enemy>(prefab, position, Quaternion.identity, null);


        var stats = _enemyStatsDatabase.GetStatsByType(type);


        enemy.Initialize(stats);

        return enemy;
    }
}