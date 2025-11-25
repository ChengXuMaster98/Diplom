using UnityEngine;
using Zenject;

public class PauseMenuController : ITickable
{
    private readonly IPauseService _pauseService;
    private readonly PauseMenuUI _pauseMenuUI;

    public PauseMenuController(IPauseService pauseService, PauseMenuUI pauseMenuUI)
    {
        _pauseService = pauseService;
        _pauseMenuUI = pauseMenuUI;
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseService.IsPaused)
            {
                // Если уже на паузе (PauseMenu на экране) — продолжаем
                _pauseMenuUI.Hide();
                _pauseService.Resume();
            }
            else
            {
                // Ставим на паузу и показываем меню
                _pauseService.Pause();
                _pauseMenuUI.Show();
            }
        }
    }
}