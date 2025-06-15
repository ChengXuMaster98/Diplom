using UnityEngine.Playables;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerHealthUI : ITickable
{
    private readonly PlayerStateSaver _playerState;
    private Slider _healthSlider;
    private Text _healthText;

    public PlayerHealthUI(PlayerStateSaver state)
    {
        _playerState = state;
        _healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        _healthText = GameObject.Find("HealthText").GetComponent<Text>();
    }

    public void Tick()
    {
        _healthSlider.value = _playerState._currentHealth;
        _healthText.text = $"HP: {_playerState._currentHealth}";
    }
}
