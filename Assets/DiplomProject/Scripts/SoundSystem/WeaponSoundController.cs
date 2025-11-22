using UnityEngine;
using Zenject;

[RequireComponent(typeof(Transform))]
public class WeaponSoundController : MonoBehaviour
{

    private AudioManager _audio;
    private Transform _self;

    [Inject]
    public void Construct(AudioManager audio)
    {
        _audio = audio;
        _self = transform;
    }

    public void PlayHit(WeaponSoundData data) => PlayRandom(data?.Hit);
    public void PlayLightAttack(WeaponSoundData data) => PlayRandom(data.LightAttack);
    public void PlayHeavyAttack(WeaponSoundData data) => PlayRandom(data.HeavyAttack);
    public void PlayBlock(WeaponSoundData data) => PlayRandom(data.Block);

    private void PlayRandom(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;
        var clip = clips[Random.Range(0, clips.Length)];
        _audio.PlayOneShot(clip, _self.position);
    }
}