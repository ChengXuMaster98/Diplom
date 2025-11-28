using UnityEngine;
using Zenject;

public class UIVisibilityController : MonoBehaviour
{
    [SerializeField] private CanvasGroup hudGroup; // HealthBar + StaminaBar + другие HUD элементы

    private IPauseService _pause;
    private MainMenuUI _mainMenu;
    private PauseMenuUI _pauseMenu;
    private GameOverUI _gameOverMenu;
    private GameWonUI _gameWon;

    [Inject]
    public void Construct(IPauseService pause,
    MainMenuUI mainMenu,
    PauseMenuUI pauseMenu,
    GameOverUI gameOverMenu,
    GameWonUI gameWon)
    {
        _pause = pause;
        _mainMenu = mainMenu;
        _pauseMenu = pauseMenu;
        _gameOverMenu = gameOverMenu;
        _gameWon = gameWon;

    }

    private void Start()
    {
        // Hide HUD when scene starts
        HideHUD();

        _mainMenu.OnMenuOpened += HideHUD;
        _mainMenu.OnMenuClosed += ShowHUD;

        _pauseMenu.OnMenuOpened += HideHUD;
        _pauseMenu.OnMenuClosed += ShowHUD;

        _gameOverMenu.OnMenuOpened += HideHUD;
        _gameOverMenu.OnMenuClosed += ShowHUD;


        _gameWon.OnMenuOpened += HideHUD;
        _gameWon.OnMenuClosed += ShowHUD;

    }

    private void HideHUD()
    {
        hudGroup.alpha = 0f;
        hudGroup.blocksRaycasts = false;
    }

    private void ShowHUD()
    {
        hudGroup.alpha = 1f;
        hudGroup.blocksRaycasts = false;
    }
}