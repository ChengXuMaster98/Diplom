using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyTracker : MonoBehaviour
{
    private List<EnemyHealth> _enemies = new List<EnemyHealth>();
    public LevelManager levelManager;

    private void Start()
    {
        _enemies = FindObjectsOfType<EnemyHealth>().ToList();
        foreach (var enemy in _enemies)
        {
            enemy.OnDeath += CheckEnemiesRemaining;
        }
    }

    private void CheckEnemiesRemaining()
    {
        _enemies = _enemies.Where(e => e != null).ToList();
        if (_enemies.Count == 0)
        {
            levelManager.LoadNextLevel();
        }
    }
}