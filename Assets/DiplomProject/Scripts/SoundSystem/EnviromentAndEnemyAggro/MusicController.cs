using System;
using UnityEngine;
using Zenject;

public class MusicController : IInitializable, IDisposable
{
    private readonly MusicDatabase _db;
    private readonly EnemyAggroTracker _aggro;

    private AudioSource _ambientSource;
    private AudioSource _combatSource;

    private bool _combatActive;

    public MusicController(MusicDatabase database, EnemyAggroTracker aggro)
    {
        _db = database;
        _aggro = aggro;
    }

    public void Initialize()
    {
        // Здесь создаётсяо объект для музыки
        var go = new GameObject("[MusicController]");
        UnityEngine.Object.DontDestroyOnLoad(go);

        _ambientSource = go.AddComponent<AudioSource>();
        _combatSource = go.AddComponent<AudioSource>();

        Configure2D(_ambientSource, 0.09f);
        Configure2D(_combatSource, 0.12f);

        PlayAmbient();

        // Подписка на изменение агра
        _aggro.OnAggroCountChanged += OnAggroChanged;
    }

    private void Configure2D(AudioSource src, float volume)
    {
        src.playOnAwake = false;
        src.loop = true;
        src.spatialBlend = 0f;      // чистый 2D
        src.volume = volume;
    }

    public void Dispose()
    {
        _aggro.OnAggroCountChanged -= OnAggroChanged;
    }

    //private void Configure(AudioSource src)
    //{
    //    src.playOnAwake = false;
    //    src.loop = true;
    //    src.spatialBlend = 0f;
    //    src.volume = 0.09f;
    //}


    // ------------------------------
    // Ambient
    // ------------------------------

    private void PlayAmbient()
    {
        if (_db.AmbientTracks == null || _db.AmbientTracks.Length == 0)
            return;

        if (_ambientSource == null)
            return;

        if (_ambientSource.isPlaying && _ambientSource.clip == _db.AmbientTracks[0])
            return;

        _ambientSource.clip = _db.AmbientTracks[0];
        _ambientSource.Play();
    }

    private void StartCombat()
    {
        if (_db.CombatMusic == null || _combatSource == null)
            return;

        if (_combatActive && _combatSource.isPlaying)
            return;

        _combatActive = true;

        _ambientSource?.Stop();

        _combatSource.clip = _db.CombatMusic;
        _combatSource.Play();
    }

    private void StopCombat()
    {
        if (_combatSource != null && _combatSource.isPlaying)
            _combatSource.Stop();

        _combatActive = false;
        PlayAmbient();
    }

    private void OnAggroChanged(int count)
    {
        if (count >= 2)
        {
            StartCombat();
        }
        else if (count == 0)
        {
            StopCombat();
        }
    }

    public void ResetToAmbient()
    {
        _combatActive = false;

        if (_combatSource != null)
            _combatSource.Stop();

        if (_ambientSource != null)
        {
            _ambientSource.Stop();
            PlayAmbient();
        }
    }

    public void ForceStopCombat()
    {
        _combatActive = false;
        _combatSource.Stop();
    }

    public void ForcePlayAmbient()
    {
        _combatActive = false;
        _combatSource.Stop();
        PlayAmbient();
    }
}