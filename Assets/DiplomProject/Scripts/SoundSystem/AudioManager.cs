using UnityEngine;
using Zenject;

public class AudioManager
{
    private readonly AudioSourcePool _pool;

    public AudioManager(AudioSourcePool pool)
    {
        _pool = pool;
    }


    public AudioSource PlayOneShot(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return null;

        var source = _pool.Get();
        source.transform.position = position;
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 1f;
        source.Play();

        _pool.ReleaseAfter(source, clip.length + 0.05f);
        return source;
    }


    public AudioSource PlayUnique(AudioClip clip, Vector3 position, string id, float volume = 1f)
    {
        if (clip == null) return null;

        var existing = _pool.GetById(id);
        if (existing != null && existing.isPlaying)
            return existing;

        var source = _pool.Get(id);
        source.transform.position = position;
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 1f;
        source.Play();


        _pool.ReleaseAfter(source, clip.length + 0.05f);
        return source;
    }
}