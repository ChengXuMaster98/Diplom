using System;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    public event Action OnFootstepEvent;

    // Animation Event: вызывается из анимации
    public void OnFootstep()
    {
        OnFootstepEvent?.Invoke();
    }
}