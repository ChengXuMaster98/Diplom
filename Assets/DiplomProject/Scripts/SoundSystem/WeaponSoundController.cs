using UnityEngine;
using Zenject;

[RequireComponent(typeof(Transform))]
public class WeaponSoundController : MonoBehaviour
{
    [SerializeField] private WeaponSoundData _soundData;

    private AudioManager _audio;
    private Transform _self;

    [Inject]
    public void Construct(AudioManager audio)
    {
        _audio = audio;
        _self = transform;
    }

    public void PlayHit() => PlayRandom(_soundData.Hit);
    public void PlayLightAttack() => PlayRandom(_soundData.LightAttack);
    public void PlayHeavyAttack() => PlayRandom(_soundData.HeavyAttack);
    public void PlayBlock() => PlayRandom(_soundData.Block);

    private void PlayRandom(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;
        var clip = clips[Random.Range(0, clips.Length)];
        _audio.PlayOneShot(clip, _self.position);
    }
}