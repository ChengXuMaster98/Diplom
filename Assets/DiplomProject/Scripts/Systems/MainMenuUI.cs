using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuUI : MonoBehaviour
{

    public event Action OnMenuOpened;
    public event Action OnMenuClosed;

    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("Buttons")]
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _quitButton;

    private ISaveService _saveService;
    private ISaveExecutor _saveExecutor;
    private IPauseService _pauseService;

    [Inject]
    public void Construct(ISaveService saveService, ISaveExecutor saveExecutor, IPauseService pauseService)
    {
        _saveService = saveService;
        _saveExecutor = saveExecutor;
        _pauseService = pauseService;
    }

    private void Start()
    {
        Debug.Log("HasSave in Build = " + _saveService.HasSave());

        _pauseService.Pause();
        Show();

        _continueButton.interactable = _saveService.HasSave();

        _newGameButton.onClick.AddListener(OnNewGameClicked);
        _continueButton.onClick.AddListener(OnContinueClicked);
        _quitButton.onClick.AddListener(OnQuitClicked);
    }

    public void Show()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
        OnMenuOpened?.Invoke();
    }

    private void Hide()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
        OnMenuClosed?.Invoke();
    }

    private void OnNewGameClicked()
    {
        _saveExecutor.RequestStartNewGame();

        Hide();
        _pauseService.Resume();
    }

    private void OnContinueClicked()
    {
        if (_saveService.HasSave())
        {
            _saveExecutor.RequestLoadLastSave();
        }
        else
        {
            _saveExecutor.RequestStartNewGame();
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