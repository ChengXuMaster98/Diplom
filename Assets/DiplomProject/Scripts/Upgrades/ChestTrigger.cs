using UnityEngine;
using Zenject;

public class ChestTrigger : MonoBehaviour, ISaveablePickup
{


    [SerializeField] private UpgradeUI _upgradeUIPrefab;

    [Tooltip("Уникальный ID этого сундука (upg_chest_01 и т.п.)")]
    [SerializeField] private string _id;
    public string ID => _id;


    private DiContainer _container;
    private PickupSaveSystem _pickupSave;

    private bool _collected = false;

    [Inject]
    public void Construct(DiContainer container, PickupSaveSystem pickupSave)
    {
        _container = container;
        _pickupSave = pickupSave;
    }

    private void Awake()
    {
        // Если сундук уже был открыт в сохранении — удаляем
        if (_pickupSave != null && _pickupSave.IsCollected(ID))
        {
            _collected = true;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_collected) return;
        if (!other.CompareTag("Player")) return;

        
        var ui = _container.InstantiatePrefabForComponent<UpgradeUI>(
            _upgradeUIPrefab, 
            Vector3.zero, 
            Quaternion.identity, 
            null);

        ui.ShowRandomOptions(() =>
        {
            _collected = true;

            // помечаем сундук как собранный
            _pickupSave?.MarkCollected(ID);

            Destroy(gameObject);
        });
    }
}