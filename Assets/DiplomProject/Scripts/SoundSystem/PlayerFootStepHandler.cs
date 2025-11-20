using UnityEngine;
using Zenject;

public class PlayerFootstepHandler : MonoBehaviour
{
    private AnimationEventReceiver _animReceiver;
    private IPlayerAudio _audio;

    [Inject]
    public void Construct(AnimationEventReceiver animReceiver, IPlayerAudio audio)
    {
        _animReceiver = animReceiver;
        _audio = audio;
    }

    private void OnEnable()
    {
        _animReceiver.OnFootstepEvent += OnFootstep;
    }

    private void OnDisable()
    {
        _animReceiver.OnFootstepEvent -= OnFootstep;
    }

    private void OnFootstep()
    {
        _audio.PlayStep();
    }
}