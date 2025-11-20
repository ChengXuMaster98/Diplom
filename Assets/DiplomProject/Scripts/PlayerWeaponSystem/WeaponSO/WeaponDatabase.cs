using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Weapons/Weapon Database")]
public class WeaponDatabase : ScriptableObject
{
    public List<WeaponData> Weapons;

    private Dictionary<WeaponType, WeaponData> _lookup;

    private void OnEnable()
    {
        _lookup = new Dictionary<WeaponType, WeaponData>();
        foreach (var w in Weapons)
            _lookup[w.Type] = w;
    }

    public WeaponData GetData(WeaponType type)
    {
        if (_lookup.TryGetValue(type, out var data))
            return data;

        Debug.LogError($"WeaponDatabase: no weapon for type {type}");
        return null;
    }
}