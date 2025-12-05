using Unity.VisualScripting;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Transform))]
public class PlayerSoundController : MonoBehaviour, IPlayerAudio
{
    [SerializeField] private PlayerSoundData _soundData;

    private AudioManager _audio;
    private Transform _self;
    private Camera _listenerCamera;

    [Inject]
    public void Construct(AudioManager audio)
    {
        _audio = audio;
        _self = transform;
        // don't inject Camera to avoid breaking existing installer — use Camera.main fallback
        _listenerCamera = Camera.main;
    }

    // API
    public void PlayStep() => PlayArray(_soundData.Step);
    public void PlayDash() => PlayArray(_soundData.Dash);
    public void PlayHurt() => PlayArray(_soundData.Hurt);
    public void PlayDeath() => PlayArray(_soundData.Death);

    public void PlayAttack() => PlayArray(_soundData.Attack);

    private void PlayArray(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;
        var clip = clips[Random.Range(0, clips.Length)];
        float vol = CalculateAttenuation(_soundData.MaxDistance, _soundData.MinVolume);
        _audio.PlayOneShot(clip, _self.position, vol);
    }

    private float CalculateAttenuation(float maxDistance, float minVol)
    {
        if (_listenerCamera == null) _listenerCamera = Camera.main;
        if (_listenerCamera == null) return 1f;

        float dist = Vector3.Distance(_self.position, _listenerCamera.transform.position);
        if (dist >= maxDistance) return minVol;
        float t = dist / maxDistance;
        return Mathf.Lerp(1f, minVol, t);
    }
}