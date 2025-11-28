using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameOverUI : MonoBehaviour
{

    public event Action OnMenuOpened;
    public event Action OnMenuClosed;

    public GameObject gameOverPanel;
    public Button restartButton;
    public GameObject blackScreen;

    private ISaveExecutor _saveExecutor;
    private ISaveService _saveService;
    private CharacterMovementController _movementController;
    private PlayerHealth _playerHealth;

    private MusicController _musicController;

    [Inject]
    public void Construct(
        ISaveExecutor saveExecutor,
        ISaveService saveService,
        CharacterMovementController movement,
        PlayerHealth playerHealth,
        MusicController musicController)
    {
        _saveExecutor = saveExecutor;
        _saveService = saveService;
        _movementController = movement;
        _playerHealth = playerHealth;
        _musicController = musicController;
    }

    private void Awake()
    {
        gameOverPanel.SetActive(false);
        blackScreen.SetActive(false);
    }

    public void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true);
        blackScreen.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f;

        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(RestartGame);
        OnMenuOpened?.Invoke();
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;

        gameOverPanel.SetActive(false);
        blackScreen.SetActive(false);

        _movementController.UnblockMovement();
        _playerHealth.ForceSetHealth(_playerHealth.MaxHealth);

        Cursor.visible = false;

        if (_saveService.HasSave())
        {
            _saveExecutor.RequestLoadLastSave();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        _musicController.ResetToAmbient();
        OnMenuClosed?.Invoke();
    }
}