using UnityEngine;
using Zenject;

public class WeaponPickup : MonoBehaviour, ISaveablePickup
{
    [SerializeField] private WeaponData Data;

    [Tooltip("Уникальный ID этого пикапа (wpn_axe_01 и т.п.)")]
    [SerializeField] private string _id;
    public string ID => _id;

    private PlayerWeaponInventory _inventory;
    private WeaponFactory _factory;
    private PickupSaveSystem _pickupSave;

    [Inject]
    public void Construct(PlayerWeaponInventory inventory, WeaponFactory factory, PickupSaveSystem pickupSave)
    {
        _inventory = inventory;
        _factory = factory;
        _pickupSave = pickupSave;
    }

    private void Awake()
    {
        // Если этот пикап уже был подобран в загруженном сейве — удаляем его
        if (_pickupSave != null && _pickupSave.IsCollected(ID))
        {
            Destroy(gameObject);
        }
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


            // помечаем как собранный
            _pickupSave?.MarkCollected(ID);

            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"[Pickup] Нет свободных слотов для оружия: {Data.WeaponName}");
        }
    }
}