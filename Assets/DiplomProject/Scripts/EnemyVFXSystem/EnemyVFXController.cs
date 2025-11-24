using UnityEngine;
using System.Collections;
using Zenject;

public class EnemyVFXController : MonoBehaviour
{
    [Header("Blood FX")]
    [SerializeField] private ParticleSystem _bloodFX;

    [Header("Stun FX")]
    [SerializeField] private ParticleSystem _stunFX;

    [Header("Electric DOT FX")]
    [SerializeField] private ParticleSystem _electricFX;

    private Enemy _enemy;
    private Coroutine _dotRoutine;
    private Coroutine _stunRoutine;

    [Inject]
    public void Construct(Enemy enemy)
    {
        _enemy = enemy;

        enemy.OnDamaged += PlayBlood;
        enemy.OnStunned += PlayStun;
        enemy.DotApplied += PlayDoT;
        enemy.OnDeath += StopAllFX;
    }

    private void PlayBlood() => _bloodFX?.Play();

    private void PlayStun(float duration)
    {
        if (_stunFX == null) return;

        _stunFX.Play();

        if(_stunRoutine != null)
            StopCoroutine(_stunRoutine);

        _stunRoutine = StartCoroutine(StopAfter(_stunFX, duration));
    }

    private void PlayDoT(float duration)
    {
        if (_electricFX == null) return;

        if (_dotRoutine != null)
            StopCoroutine(_dotRoutine);

        _dotRoutine = StartCoroutine(DotFXRoutine(duration));
    }

    private IEnumerator DotFXRoutine(float duration)
    {
        _electricFX.Play();

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            yield return null;
        }

        _electricFX.Stop();
    }

    private IEnumerator StopAfter(ParticleSystem fx, float duration)
    {
        yield return new WaitForSeconds(duration);
        fx.Stop();
    }

    private void StopAllFX()
    {
        _bloodFX?.Stop();
        _stunFX?.Stop();
        _electricFX?.Stop();
    }
}