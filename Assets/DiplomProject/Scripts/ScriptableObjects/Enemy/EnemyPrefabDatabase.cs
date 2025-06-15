using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyPrefabDatabase", menuName = "Enemy/Prefab Database")]
public class EnemyPrefabDatabase : ScriptableObject
{
    [System.Serializable]
    public class EnemyPrefabEntry
    {
        public EnemyType type;
        public Enemy prefab;
    }

    [SerializeField] private List<EnemyPrefabEntry> _enemyPrefabs;

    private Dictionary<EnemyType, Enemy> _lookup;

    private void OnEnable()
    {
        _lookup = new Dictionary<EnemyType, Enemy>();
        foreach (var entry in _enemyPrefabs)
        {
            _lookup[entry.type] = entry.prefab;
        }
    }

    public Enemy GetPrefabByType(EnemyType type)
    {
        if (_lookup == null)
        {
            Debug.LogError("EnemyPrefabDatabase: _lookup is null! Возможно, OnEnable() не вызвался.");
        }

        if (_lookup.TryGetValue(type, out var prefab))
        {
            Debug.Log($"[EnemyPrefabDatabase] Префаб найден: {type}");
            return prefab;
        }

        Debug.LogError($"[EnemyPrefabDatabase] Prefab not found for type: {type}");
        return null;
    }
}