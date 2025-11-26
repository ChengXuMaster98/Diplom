using UnityEngine;
using Zenject;

public class ChestTrigger : MonoBehaviour, IPickupListener
{
    [SerializeField] private UpgradeUI _upgradeUIPrefab;

    private DiContainer _container;
    private bool _opened = false;
    private PickupSpawner _spawner;

    [Inject]
    public void Construct(DiContainer container)
    {
        _container = container;
    }

    public void Initialize(PickupSpawner spawner)
    {
        _spawner = spawner;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_opened) return;
        if (!other.CompareTag("Player")) return;

        var ui = _container.InstantiatePrefabForComponent<UpgradeUI>(_upgradeUIPrefab);
        _container.Inject(ui.gameObject);

        ui.ShowRandomOptions(() =>
        {
            _opened = true;

            // Сообщаем системе что этот пикап собран
            _spawner?.MarkCollected();

            Destroy(gameObject);
        });
    }
}