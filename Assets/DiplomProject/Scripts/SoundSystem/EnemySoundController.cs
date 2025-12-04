using UnityEngine;
using Zenject;

[RequireComponent(typeof(Transform))]
public class EnemySoundController : MonoBehaviour
{
    [SerializeField] private EnemySoundData _soundData;

    private AudioManager _audio;
    private Transform _self;
    private Camera _listenerCamera;

    public int RandomIdleInterval;

    [Inject]
    public void Construct(AudioManager audio)
    {
        _audio = audio;
        _self = transform;
        _listenerCamera = Camera.main;
    }

    public void PlayIdle() => PlayArray(_soundData.IdleVocal);

    public void PlayAggroVocal() => PlayArray(_soundData.AggroVocal);

    public void PlayAttack() => PlayArray(_soundData.Attack);

    public void PlayHurt() => PlayArray(_soundData.Hurt);
    public void PlayStep() => PlayArray(_soundData.Step);
    public void PlayDeath() => PlayArray(_soundData.Death);


    public float GetRandomIdleInterval()
       => Random.Range(_soundData.IdleMinInterval, _soundData.IdleMaxInterval);

    public float GetRandomAggroInterval()
        => Random.Range(_soundData.AggroMinInterval, _soundData.AggroMaxInterval);

    public float GetRandomAttackInterval()
        => Random.Range(_soundData.AggroMinInterval, _soundData.AttackMaxInterval);

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