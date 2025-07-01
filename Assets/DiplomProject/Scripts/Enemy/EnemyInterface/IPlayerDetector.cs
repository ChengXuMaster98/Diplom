using System;
using UnityEngine;

public interface IPlayerDetector
{
    event Action<Transform> PlayerDetected;
    event Action PlayerLost;
}