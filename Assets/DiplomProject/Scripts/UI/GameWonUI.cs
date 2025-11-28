using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameWonUI : MonoBehaviour
{

    public event Action OnMenuOpened;
    public event Action OnMenuClosed;

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _returnButton;


    private MainMenuUI _mainMenuUI;
    private CharacterMovementController _controller;
    private IPauseService _pauseService;
    private SaveService _saveService;
    private MusicController _musicController;

    [Inject]
    public void Construct(
        MainMenuUI mainMenuUI,
        IPauseService pauseService,
        CharacterMovementController controller,
        SaveService saveService,
        MusicController musicController)
    {;
        _mainMenuUI = mainMenuUI;
        _pauseService = pauseService;
        _controller = controller;
        _saveService = saveService;
        _musicController = musicController;
    }

    private void Awake()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        gameObject.SetActive(false);

        _returnButton.onClick.AddListener(ReturnToMainMenu);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;

        // Скрываем игровой интерфейс

        // Блокируем движение игрока
        _pauseService.Pause();

        //_controller.BlockMovement();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _musicController.ForceStopCombat();

        OnMenuOpened?.Invoke();

    }

    private void ReturnToMainMenu()
    {
        // Прячем GameWonUI
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        gameObject.SetActive(false);

        // Показываем главное меню
        _mainMenuUI.Show();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _saveService.NewGame(); // очищаем сейв

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        _musicController.ResetToAmbient();

        OnMenuOpened?.Invoke();
    }
}