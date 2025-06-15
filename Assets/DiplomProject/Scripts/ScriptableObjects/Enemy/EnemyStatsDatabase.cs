using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyStatsDatabase", menuName = "EnemyStatistics/Enemy Stats Database")]
public class EnemyStatsDatabase : ScriptableObject
{
    [System.Serializable]
    public class EnemyStatsEntry
    {
        public EnemyType type;
        [SerializeField] public EnemyStats stats;
    }

    [SerializeField] private List<EnemyStatsEntry> _enemyStatsList;

    private Dictionary<EnemyType, EnemyStats> _lookup;

    private void OnEnable()
    {
        _lookup = new Dictionary<EnemyType, EnemyStats>();
        foreach (var entry in _enemyStatsList)
        {
            _lookup[entry.type] = entry.stats;
        }
    }

    public EnemyStats GetStatsByType(EnemyType type)
    {
        if (_lookup.TryGetValue(type, out var stats))
            return stats;

        Debug.LogError($"No stats found for enemy type: {type}");
        return null;
    }
}