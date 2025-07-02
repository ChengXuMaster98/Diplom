using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyType type;

    private IEnemyFactory _enemyFactory;

    [Inject]
    public void Construct(IEnemyFactory enemyFactory)
    {
        _enemyFactory = enemyFactory;
    }

    private void Start()
    {
        Debug.Log("EnemySpawner активен");
        Spawn(() => Debug.Log("Враг умер (через Start)"));
    }

    public void Spawn(System.Action onDeathCallback)
    {

        Debug.Log($"[EnemySpawner] Спавним врага типа: {type}");
        var enemy = _enemyFactory.Create(type, transform.position);
        enemy.OnDeath += onDeathCallback;
    }
}