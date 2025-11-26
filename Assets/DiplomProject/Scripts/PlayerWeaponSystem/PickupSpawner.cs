using UnityEngine;
using Zenject;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private string pickupId;
    [SerializeField] private GameObject pickupPrefab;

    private PickupSaveSystem _save;
    private DiContainer _container;

    [Inject]
    public void Construct(PickupSaveSystem save, DiContainer container)
    {
        _save = save;
        _container = container;
    }

    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(pickupId))
            pickupId = System.Guid.NewGuid().ToString();
    }

    public void TrySpawn()
    {
        if (_save.IsCollected(pickupId))
        {
            // Уже подобран → не спавним
            return;
        }

        // Спавним через Zenject + инжектим всех детей
        var go = _container.InstantiatePrefab(pickupPrefab, transform.position, transform.rotation, null);
        _container.InjectGameObject(go);

        // Передаём себя пикапу
        foreach (var pickup in go.GetComponentsInChildren<MonoBehaviour>())
        {
            if (pickup is IPickupListener listener)
                listener.Initialize(this);
        }
    }

    public void MarkCollected()
    {
        _save.MarkCollected(pickupId);
    }
}