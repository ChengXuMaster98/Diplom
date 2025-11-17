using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("Buttons")]
    [SerializeField] private Button _optionA;
    [SerializeField] private Button _optionB;
    [SerializeField] private Button _optionC;

    [Header("Texts")]
    [SerializeField] private Text _textA;
    [SerializeField] private Text _textB;
    [SerializeField] private Text _textC;

    [Header("Icons")]
    [SerializeField] private Image _iconA;
    [SerializeField] private Image _iconB;
    [SerializeField] private Image _iconC;

    private IUpgradeService _upgradeService;
    private UpgradeDatabase _database;
    private Action _onCollectedCallback;

    [Inject]
    public void Construct(IUpgradeService upgradeService, UpgradeDatabase database)
    {
        _upgradeService = upgradeService;
        _database = database;
    }

    public void ShowRandomOptions(Action onCollected)
    {
        _onCollectedCallback = onCollected;
        PopulateRandomOptions();
        Show();
    }

    private void PopulateRandomOptions()
    {
        // Простая логика: случайно 3 типа (могут повторяться, можно усложнить).
        var a = _database.GetRandom();
        var b = _database.GetRandom();
        var c = _database.GetRandom();

        SetupButton(_optionA, _textA, _iconA, a);
        SetupButton(_optionB, _textB, _iconB, b);
        SetupButton(_optionC, _textC, _iconC, c);
    }

    private void SetupButton(Button button, Text text, Image icon, UpgradeData data)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => ApplyAndClose(data));

        text.text = $"{data.DisplayName} +{(data.Value * 100):0}%";
        icon.sprite = data.Icon;
    }

    private void ApplyAndClose(UpgradeData data)
    {
        _upgradeService.ApplyUpgrade(data.Type, data.Value);
        Close();
        _onCollectedCallback?.Invoke();
    }

    private void Show()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Close()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Destroy(gameObject);
    }
}