using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Button restartButton;
    public GameObject blackScreen;

    private SaveLoadController _saveLoadController;
    private ISaveService _saveService;

    private CharacterMovementController _movementController;

    private PlayerHealth _playerHealth;

    [Inject]
    public void Construct(SaveLoadController saveLoadController, ISaveService saveService, CharacterMovementController movement, PlayerHealth playerHealth)
    {
        _saveLoadController = saveLoadController;
        _saveService = saveService;
        _movementController = movement;
        _playerHealth = playerHealth;

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
            _saveLoadController.LoadLastSave();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}