using UnityEngine;
using Zenject;

public class SaveLoadController : MonoBehaviour
{
    private ISaveService _saveService;
    private EnemySpawner[] _spawners;
    private PickupSpawner[] _pickupSpawners;

    private SaveExecutor _saveExecutor;

    [Inject]
    public void Construct(ISaveService saveService, SaveExecutor saveExecutor)
    {
        _saveService = saveService;
        _saveExecutor = saveExecutor;
    }

    private void Awake()
    {
        _spawners = FindObjectsOfType<EnemySpawner>();
        _pickupSpawners = FindObjectsOfType<PickupSpawner>();
    }

    private void Start()
    {
        // Старт игры контролируется MainMenuUI.
    }

    public void StartNewGame()
    {
        _saveService.NewGame();

        DestroyAllExistingEnemies();

        // Спавним врагов
        foreach (var spawner in _spawners)
            spawner.TrySpawn();

        // Уничтожаем подобранные ящики с оружием
        foreach (var pickup in FindObjectsOfType<WeaponPickup>())
            Destroy(pickup.gameObject);
        
        // Уничтожаем подобранные ящики с апгрейдами
        foreach (var chest in FindObjectsOfType<ChestTrigger>())
            Destroy(chest.gameObject);

        // Спавним ящики
        foreach (var p in _pickupSpawners)
            p.TrySpawn();

        var weaponController = FindObjectOfType<PlayerWeaponController>();
        weaponController?.RefreshEquippedWeapon();

        Debug.Log("[SaveLoadController] New Game started from UI.");
    }


    public void LoadLastSave()
    {
        if (!_saveService.HasSave())
        {
            Debug.LogWarning("[SaveLoadController] Нет сейва для загрузки.");
            return;
        }

        DestroyAllExistingEnemies();

        _saveService.Load();


        foreach (var spawner in _spawners)
            spawner.TrySpawn();

        foreach (var pickup in FindObjectsOfType<WeaponPickup>())
            Destroy(pickup.gameObject);

        foreach (var chest in FindObjectsOfType<ChestTrigger>())
            Destroy(chest.gameObject);

        foreach (var p in _pickupSpawners)
            p.TrySpawn();

        var weaponController = FindObjectOfType<PlayerWeaponController>();
        weaponController?.RefreshEquippedWeapon();

        Debug.Log("[SaveLoadController] Save loaded from UI.");
    }

    private void DestroyAllExistingEnemies()
    {
        var enemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemies)
            Destroy(enemy.gameObject);
    }
}