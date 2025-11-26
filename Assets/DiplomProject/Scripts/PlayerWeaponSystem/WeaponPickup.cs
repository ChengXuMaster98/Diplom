using UnityEngine;
using Zenject;

public class WeaponPickup : MonoBehaviour, IPickupListener
{
    [SerializeField] private WeaponData Data;

    private PlayerWeaponInventory _inventory;
    private WeaponFactory _factory;
    private PickupSpawner _spawner;

    [Inject]
    public void Construct(PlayerWeaponInventory inventory, WeaponFactory factory)
    {
        _inventory = inventory;
        _factory = factory;
    }

    public void Initialize(PickupSpawner spawner)
    {
        _spawner = spawner;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Player>(out _))
            return;

        var weapon = _factory.Create(Data);

        if (_inventory.TryAddWeapon(weapon))
        {
            Debug.Log($"[Pickup] Подобрано оружие: {Data.WeaponName}");

            // Сообщаем системе, что пикап подобран
            _spawner?.MarkCollected();

            Destroy(gameObject);
        }
    }
}