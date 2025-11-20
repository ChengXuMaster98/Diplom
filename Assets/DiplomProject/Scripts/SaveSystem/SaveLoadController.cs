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
        // Запоминаем все спавнеры врагов в сцене
        _spawners = FindObjectsOfType<EnemySpawner>();
    }

    private void Start()
    {

        if (_saveService.HasSave())
        {
            Debug.Log("[SaveLoadController] Обнаружен сейв — загружаем автоматически");
            _saveService.Load();
        }
        else
        {
            Debug.Log("[SaveLoadController] Сейва нет — новая игра");
            _saveService.NewGame(); // очищает списки убитых врагов
            DestroyAllExistingEnemies();
        }
        // Найдём всех спавнеров
        //_spawners = FindObjectsOfType<EnemySpawner>();

        // Если хочешь автоматический запуск новой игры при отсутствии сейва:
        foreach (var spawner in _spawners)
            spawner.TrySpawn();
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

    private void StartNewGame()
    {
        _saveService.NewGame();

        DestroyAllExistingEnemies();

        foreach (var spawner in _spawners)
            spawner.TrySpawn(); // теперь все враги должны появиться

        Debug.Log("[SaveLoadController] New Game started.");
    }

    private void DestroyAllExistingEnemies()
    {
        var enemies = FindObjectsOfType<Enemy>();

        foreach (var enemy in enemies)
            Destroy(enemy.gameObject);
    }
}