using UnityEngine;

[CreateAssetMenu(fileName = "MusicDatabase", menuName = "Audio/Music Database")]
public class MusicDatabase : ScriptableObject
{
    [Header("Background music (looped)")]
    public AudioClip[] AmbientTracks;

    [Header("Combat music")]
    public AudioClip CombatMusic;

    [Header("Environment SFX")]
    public AudioClip TorchFire;
    public AudioClip CaveHum;
    public AudioClip StrangeSound;

}