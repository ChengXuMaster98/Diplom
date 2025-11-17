using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Enemy Sound Data")]
public class EnemySoundData : ScriptableObject
{
    public AudioClip[] Idle;
    public AudioClip[] Attack;
    public AudioClip[] Hurt;
    public AudioClip[] Step;
    public AudioClip[] Death;

    public float StepInterval = 0.55f;

    [Header("Distance Attenuation")]
    public float MaxDistance = 20f;
    public float MinVolume = 0.06f;
}