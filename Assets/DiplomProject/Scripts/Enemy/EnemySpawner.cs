using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyType type;
    [SerializeField] private string enemyId;

    private IEnemyFactory _enemyFactory;
    private EnemySaveSystem _enemySave;

    [Inject]
    public void Construct(IEnemyFactory enemyFactory, EnemySaveSystem enemySave)
    {
        _enemyFactory = enemyFactory;
        _enemySave = enemySave;

    }

    public void TrySpawn()
    {
        if (_enemySave.IsDead(enemyId))
        {
            //Debug.Log($"[EnemySpawner] Враг {enemyId} уже убит — не спавним.");
            return;
        }

        Spawn(() => OnEnemyDeath());
    }

    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(enemyId))
            enemyId = System.Guid.NewGuid().ToString();
    }

    public void Spawn(System.Action onDeathCallback)
    {

        //Debug.Log($"[EnemySpawner] Спавним врага типа: {type}");
        var enemy = _enemyFactory.Create(type, transform.position);
        enemy.OnDeath += onDeathCallback;
    }

    private void OnEnemyDeath()
    {
        //Debug.Log($"[EnemySpawner] Враг {enemyId} умер");
        _enemySave.MarkDead(enemyId);
    }
}