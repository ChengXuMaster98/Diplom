using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Enemy Sound Data")]
public class EnemySoundData : ScriptableObject
{
    public AudioClip[] IdleVocal;
    public AudioClip[] AggroVocal;

    public AudioClip[] Attack;

    public AudioClip[] Hurt;
    public AudioClip[] Step;
    public AudioClip[] Death;

    [Header("Vocal Intervals")]
    public float IdleMinInterval = 1.5f;
    public float IdleMaxInterval = 5f;

    public float AggroMinInterval = 2.5f;
    public float AggroMaxInterval = 5f;

    public float AttackMinInterval = 0.5f;
    public float AttackMaxInterval = 1f;


    public float StepInterval = 0.55f;

    [Header("Distance Attenuation")]
    public float MaxDistance = 10f;
    public float MinVolume = 1f;
}