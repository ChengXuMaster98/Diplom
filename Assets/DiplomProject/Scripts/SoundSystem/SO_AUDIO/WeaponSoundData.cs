using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Weapon Sound Data")]
public class WeaponSoundData : ScriptableObject
{
    public AudioClip[] Hit;
    public AudioClip[] LightAttack;
    public AudioClip[] HeavyAttack;
    public AudioClip[] Block;

    public float FireRateInterval = 0.08f;
}