using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool
{
    private readonly Transform _root;
    private readonly Stack<AudioSource> _pool = new();
    private readonly Dictionary<string, AudioSource> _unique = new();
    private readonly AudioCoroutineRunner _coroutineRunner;

    public AudioSourcePool()
    {
        var go = new GameObject("[AudioPool]");
        Object.DontDestroyOnLoad(go);
        _root = go.transform;
        _coroutineRunner = go.AddComponent<AudioCoroutineRunner>();
    }

    public AudioSource Get(string id = null)
    {
        AudioSource src =
            _pool.Count > 0 ? _pool.Pop() :
            new GameObject("AudioSource").AddComponent<AudioSource>();

        src.transform.parent = _root;
        src.playOnAwake = false;
        src.spatialBlend = 1f;
        src.maxDistance = 50f;

        if (id != null)
            _unique[id] = src;

        return src;
    }

    public AudioSource GetById(string id)
    {
        return _unique.ContainsKey(id) ? _unique[id] : null;
    }

    public void Release(AudioSource src)
    {
        if (src == null) return;

        src.Stop();
        src.clip = null;
        src.transform.parent = _root;
        _pool.Push(src);
    }

    public void ReleaseAfter(AudioSource src, float seconds)
    {
        if (_coroutineRunner != null)
            _coroutineRunner.StartCoroutine(ReleaseCoroutine(src, seconds));
        else
            Release(src);
    }

    private System.Collections.IEnumerator ReleaseCoroutine(AudioSource src, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Release(src);
    }
}
