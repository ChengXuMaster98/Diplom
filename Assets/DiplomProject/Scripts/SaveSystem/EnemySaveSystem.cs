using System.Collections.Generic;

public class EnemySaveSystem
{
    private readonly Dictionary<string, bool> _enemyStates = new();

    public void MarkDead(string id)
    {
        _enemyStates[id] = true;
    }

    public bool IsDead(string id)
    {
        return _enemyStates.TryGetValue(id, out bool value) && value;
    }

    public Dictionary<string, bool> GetAllStates()
    {
        return _enemyStates;
    }

    public void LoadStates(Dictionary<string, bool> states)
    {
        _enemyStates.Clear();
        foreach (var pair in states)
            _enemyStates[pair.Key] = pair.Value;
    }
}