using UnityEngine;
using Zenject;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private WeaponData Data;

    private PlayerWeaponInventory _inventory;
    private WeaponFactory _factory;

    [Inject]
    public void Construct(PlayerWeaponInventory inventory, WeaponFactory factory)
    {
        _inventory = inventory;
        _factory = factory;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Player>(out _))
            return;

        // создаём оружие
        IWeapon weapon = _factory.Create(Data);

        // пытаемся положить в слот
        if (_inventory.TryAddWeapon(weapon))
        {
            Debug.Log($"[Pickup] Подобрано оружие: {Data.WeaponName}");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"[Pickup] Нет свободных слотов для оружия: {Data.WeaponName}");
        }
    }
}