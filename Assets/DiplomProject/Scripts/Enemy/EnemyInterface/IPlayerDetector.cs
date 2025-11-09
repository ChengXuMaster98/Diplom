using System;
using UnityEngine;

public interface IPlayerDetector
{
    event Action<Transform> PlayerDetected;
    event Action PlayerLost;

    Transform Player { get; }
}