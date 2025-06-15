using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<EnemySpawner> wave1Spawners;
    [SerializeField] private List<EnemySpawner> wave2Spawners;
    [SerializeField] private List<EnemySpawner> wave3Spawners;


    private int currentWave = 0;
    private int aliveEnemies = 0;

    public void StartWaves()
    {
        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(1f);

        switch (currentWave)
        {
            case 0: SpawnWave(wave1Spawners); break;
            case 1: SpawnWave(wave2Spawners); break;
            case 2: SpawnWave(wave3Spawners); break;
            default: EndGame(); break;
        }
    }

    private void SpawnWave(List<EnemySpawner> spawners)
    {
        aliveEnemies = spawners.Count;
        foreach (var spawner in spawners)
        {
            spawner.Spawn(OnEnemyKilled);
        }
    }

    private void OnEnemyKilled()
    {
        aliveEnemies--;
        if (aliveEnemies <= 0)
        {
            currentWave++;
        }
    }

    private void EndGame()
    {
        Debug.Log("Game Completed!");
        // Вывести финальный экран
    }
}