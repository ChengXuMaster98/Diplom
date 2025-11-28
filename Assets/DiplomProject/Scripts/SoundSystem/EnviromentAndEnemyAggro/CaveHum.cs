using UnityEngine;
using Zenject;

public class CaveHumSound : MonoBehaviour
{
    private AudioManager _audio;
    private MusicDatabase _db;
    private AudioSource _src;

    [Inject]
    public void Construct(AudioManager audio, MusicDatabase db)
    {
        _audio = audio;
        _db = db;
    }

    private void Start()
    {
        if (_db.CaveHum == null)
            return;

        _src = _audio.PlayLoop(
            _db.CaveHum,
            transform.position,
            $"hum_{GetInstanceID()}",
            0.4f
        );

        _src.loop = true;
        _src.spatialBlend = 1f;
        _src.transform.position = transform.position;
    }
}