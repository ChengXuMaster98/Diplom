using System.Collections.Generic;

public class PickupSaveSystem
{
    // true = СФЕ онднапюмн
    private readonly Dictionary<string, bool> _pickupStates = new();

    public void MarkCollected(string id)
    {
        if (string.IsNullOrEmpty(id))
            return;

        _pickupStates[id] = true;
    }

    public bool IsCollected(string id)
    {
        if (string.IsNullOrEmpty(id))
            return false;

        return _pickupStates.TryGetValue(id, out bool value) && value;
    }

    public Dictionary<string, bool> GetAllStates()
    {
        return _pickupStates;
    }

    public void LoadStates(Dictionary<string, bool> states)
    {
        _pickupStates.Clear();
        foreach (var pair in states)
            _pickupStates[pair.Key] = pair.Value;
    }
}