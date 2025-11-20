using UnityEngine;
using Zenject;

public class WeaponPickup : MonoBehaviour
{
    public WeaponData Data;

    private PlayerWeaponInventory _inventory;

    [Inject]
    public void Construct(PlayerWeaponInventory inventory)
    {
        _inventory = inventory;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Player>(out var player))
            return;

        if (_inventory.TryAddWeapon(Data))
        {
            Destroy(gameObject);
        }
    }
}