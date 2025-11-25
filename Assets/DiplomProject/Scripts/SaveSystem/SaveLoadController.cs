using UnityEngine;
using Zenject;

public class SaveLoadController : MonoBehaviour
{
    private ISaveService _saveService;
    private EnemySpawner[] _spawners;

    [Inject]
    public void Construct(ISaveService saveService)
    {
        _saveService = saveService;
    }

    private void Awake()
    {
        _spawners = FindObjectsOfType<EnemySpawner>();
    }

    private void Start()
    {
        // Больше НИЧЕГО не делаем здесь.
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

    private void Update()
    {
        // F5 - сохранить
        if (Input.GetKeyDown(KeyCode.F5))
        {
            _saveService.Save();
        }

        // F9 - загрузить
        if (Input.GetKeyDown(KeyCode.F9))
        {
            _saveService.Load();

            foreach (var spawner in _spawners)
                spawner.TrySpawn();

            var weaponController = FindObjectOfType<PlayerWeaponController>();
            weaponController?.RefreshEquippedWeapon();
        }

        // Delete - удаляем сейв
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            _saveService.DeleteSave();
        }

        // Нажимаем N - новая игра
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartNewGame();
        }
    }

}