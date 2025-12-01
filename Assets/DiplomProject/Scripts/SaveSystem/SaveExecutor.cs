using Zenject;

public class SaveExecutor : ITickable, ISaveExecutor
{
    private readonly SaveLoadController _saveLoadController;

    private bool _loadRequested;
    private bool _newGameRequested;

    private MusicController _musicController;

    public SaveExecutor(SaveLoadController saveLoadController, MusicController musicController)
    {
        _saveLoadController = saveLoadController;
        _musicController = musicController;
    }

    public void RequestLoadLastSave()
    {
        _loadRequested = true;
    }

    public void RequestStartNewGame()
    {
        _newGameRequested = true;
    }

    public void Tick()
    {
        // Важное замечание:
        // тут мы уже в стабильном игровом цикле,
        // все Start / Initialize давно отработали

        if (_newGameRequested)
        {
            _newGameRequested = false;
            _saveLoadController.StartNewGame();
        }

        if (_loadRequested)
        {
            _loadRequested = false;
            _saveLoadController.LoadLastSave();
            _musicController.ResetToAmbient();
        }
    }
}