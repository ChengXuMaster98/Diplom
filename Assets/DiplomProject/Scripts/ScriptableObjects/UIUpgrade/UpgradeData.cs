using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObject/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public UpgradeType Type;
    public string DisplayName;
    [Range(0f, 1f)] public float Value; // 0.2 = +20%
    public Sprite Icon;
}