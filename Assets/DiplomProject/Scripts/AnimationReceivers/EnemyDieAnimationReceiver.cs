using System;
using UnityEngine;


public class EnemyDieAnimationReceiver : MonoBehaviour
{
    public event Action OnDeathAnimationEvent;

    public void OnDeathAnimationEnd()
    {
        OnDeathAnimationEvent?.Invoke();
    }

}