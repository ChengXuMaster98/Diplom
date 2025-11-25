using UnityEngine;
using Zenject;

public class SaveLoadController : MonoBehaviour
{
    private ISaveService _saveService;
    private EnemySpawner[] _spawners;
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
    }

    private void Start()
    {
        // Старт игры контролируется MainMenuUI.
    }

    public void StartNewGame()
    {
        _saveService.NewGame();

        DestroyAllExistingEnemies();

        foreach (var spawner in _spawners)
            spawner.TrySpawn();

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