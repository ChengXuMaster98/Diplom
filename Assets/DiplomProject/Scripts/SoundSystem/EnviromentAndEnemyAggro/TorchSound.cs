using UnityEngine;
using Zenject;

public class TorchSound : MonoBehaviour
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
        if (_db.TorchFire == null)
            return;

        _src = _audio.PlayLoop(
            _db.TorchFire,
            transform.position,
            $"torch_{GetInstanceID()}",
            1f
        );

        _src.loop = true;
        _src.spatialBlend = 1f;
        _src.transform.position = transform.position;
    }
}