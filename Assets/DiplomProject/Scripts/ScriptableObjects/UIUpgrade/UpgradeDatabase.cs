using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeDatabase", menuName = "ScriptableObject/UpgradeDatabase")]
public class UpgradeDatabase : ScriptableObject
{
    public UpgradeData[] Upgrades;

    public UpgradeData GetRandom()
    {
        if (Upgrades == null || Upgrades.Length == 0)
            return null;

        return Upgrades[Random.Range(0, Upgrades.Length)];
    }
}