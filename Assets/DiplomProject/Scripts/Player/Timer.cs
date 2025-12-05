using System;
using UnityEngine;

public static class Timer
{
    public static void After(float seconds, Action callback)
    {
        CoroutineRunner.Instance.StartCoroutine(Delay(seconds, callback));
    }

    private static System.Collections.IEnumerator Delay(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback?.Invoke();
    }
}