using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Player Sound Data")]
public class PlayerSoundData : ScriptableObject
{
    public AudioClip[] Step;
    public AudioClip[] Hurt;
    public AudioClip[] Death;
    public AudioClip[] Dash;
    public AudioClip[] Attack;

    [Header("Step Settings")]
    public float StepInterval = 0.55f;

    [Header("Distance Attenuation")]
    public float MaxDistance = 10f;       // дальше звука почти не слышно
    public float MinVolume = 0.08f;       // минимальная громкость на MaxDistance
}