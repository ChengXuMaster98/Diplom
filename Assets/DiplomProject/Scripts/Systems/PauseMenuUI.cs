using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("Buttons")]
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _loadButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _quitButton;

    private IPauseService _pauseService;
    private ISaveService _saveService;
    private ISaveExecutor _saveExecutor;

    [Inject]
    public void Construct(IPauseService pauseService, ISaveService saveService, ISaveExecutor saveExecutor)
    {
        _pauseService = pauseService;
        _saveService = saveService;
        _saveExecutor = saveExecutor;
    }

    private void Awake()
    {
        Hide();

        _continueButton.onClick.AddListener(OnContinueClicked);
        _saveButton.onClick.AddListener(OnSaveClicked);
        _loadButton.onClick.AddListener(OnLoadClicked);
        _quitButton.onClick.AddListener(OnQuitClicked);
    }

    public void Show()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }

    private void OnSaveClicked()
    {
        _saveService.Save();
        Debug.Log("[PauseMenu] Game Saved!");

        Hide();
        _pauseService.Resume();
    }

    private void OnContinueClicked()
    {
        Hide();
        _pauseService.Resume();
    }

    private void OnLoadClicked()
    {
        if (_saveService.HasSave())
        {
            _saveExecutor.RequestLoadLastSave();
        }
        else
        {
            Debug.LogWarning("[PauseMenu] Нет сейва для загрузки.");
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