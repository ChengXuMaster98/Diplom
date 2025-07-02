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
        Debug.Log("EnemySpawner �������");
        Spawn(() => Debug.Log("���� ���� (����� Start)"));
    }

    public void Spawn(System.Action onDeathCallback)
    {

        Debug.Log($"[EnemySpawner] ������� ����� ����: {type}");
        var enemy = _enemyFactory.Create(type, transform.position);
        enemy.OnDeath += onDeathCallback;
    }
}