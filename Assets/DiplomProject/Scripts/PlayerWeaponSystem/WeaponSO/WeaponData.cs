using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/Weapon", order = 1)]
public class WeaponData : ScriptableObject
{
    public WeaponType Type;

    public string WeaponName;
    public float BaseDamage;

    [Header("Эффекты")]
    public bool CanStun;
    public float StunChance;
    public float StunDuration;

    public bool CanElectroDOT;
    public float DOTDamagePerSecond;
    public float DOTDuration;
    public float DOTChance;

    [Header("Модель оружия")]
    public GameObject WeaponPrefab;

    [Header("Звук")]
    public WeaponSoundData SoundData;

    [Header("Анимации")]
    public string AttackTriggerName;

    [Header("Положение при спавне")]
    public Vector3 PositionOffset;
    public Vector3 RotationOffset;
}