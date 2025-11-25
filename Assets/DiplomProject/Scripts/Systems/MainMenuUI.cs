using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("Buttons")]
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _quitButton;

    private ISaveService _saveService;
    private SaveLoadController _saveLoad;
    private IPauseService _pauseService;

    [Inject]
    public void Construct(ISaveService saveService, SaveLoadController saveLoad, IPauseService pauseService)
    {
        _saveService = saveService;
        _saveLoad = saveLoad;
        _pauseService = pauseService;
    }

    private void Start()
    {
        Debug.Log("HasSave in Build = " + _saveService.HasSave());
        // При старте игры — пауза и главное меню
        _pauseService.Pause();
        Show();

        // Кнопка "Продолжить" активна только если есть сейв
        _continueButton.interactable = _saveService.HasSave();

        _newGameButton.onClick.AddListener(OnNewGameClicked);
        _continueButton.onClick.AddListener(OnContinueClicked);
        _quitButton.onClick.AddListener(OnQuitClicked);
    }

    private void Show()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    private void Hide()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }

    private void OnNewGameClicked()
    {
        // Полный сброс прогресса и новая игра
        _saveLoad.StartNewGame();

        Hide();
        _pauseService.Resume();
    }

    private void OnContinueClicked()
    {
        if (_saveService.HasSave())
        {
            _saveLoad.LoadLastSave();
        }
        else
        {
            // fallback: если почему-то нажали, а сейва нет — новая игра
            _saveLoad.StartNewGame();
        }

        Hide();
        _pauseService.Resume();
    }

    private void OnQuitClicked()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}