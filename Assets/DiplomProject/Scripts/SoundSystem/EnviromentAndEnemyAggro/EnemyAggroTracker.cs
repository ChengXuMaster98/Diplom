using UnityEngine;
using Zenject;
using System;

public class EnemyAggroTracker : IInitializable, IDisposable
{
    public event Action<int> OnAggroCountChanged;

    private int _aggroCount;

    private bool _inCombat;

    public int Count => _aggroCount;

    public void Increment()
    {
        _aggroCount++;
        OnAggroCountChanged?.Invoke(_aggroCount);
    }

    public void Decrement()
    {
        _aggroCount = Mathf.Max(0, _aggroCount - 1);
        OnAggroCountChanged?.Invoke(_aggroCount);
    }

    public void Initialize() { }
    public void Dispose() { }
}