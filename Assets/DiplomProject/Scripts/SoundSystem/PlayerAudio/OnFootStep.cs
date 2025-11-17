using UnityEngine;
using Zenject;

public class FootstepController : MonoBehaviour
{
    private IPlayerSoundController _playerSounds;

    [Inject]
    public void Construct(IPlayerSoundController playerSounds)
    {
        _playerSounds = playerSounds as IPlayerSoundController;
    }

    // Animation Event это будет
    public void OnFootstep()
    {
        _playerSounds?.PlayStep();
    }
}