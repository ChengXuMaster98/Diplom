using UnityEngine;
using System.Collections;

public class LightningArcController : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;
    [SerializeField] private float _arcDuration = 0.2f;
    [SerializeField] private float _noiseAmplitude = 0.3f;

    private Coroutine _routine;

    public void PlayArc(Vector3 from, Vector3 to)
    {
        if (_routine != null)
            StopCoroutine(_routine);

        _routine = StartCoroutine(ArcRoutine(from, to));
    }

    private IEnumerator ArcRoutine(Vector3 from, Vector3 to)
    {
        _line.enabled = true;

        float t = 0;
        while (t < _arcDuration)
        {
            t += Time.deltaTime;

            _line.positionCount = 5;

            for (int i = 0; i < 5; i++)
            {
                float lerp = i / 4f;
                Vector3 pos = Vector3.Lerp(from, to, lerp);

                // разброс молнии
                pos += Random.insideUnitSphere * _noiseAmplitude;

                _line.SetPosition(i, pos);
            }

            yield return null;
        }

        _line.enabled = false;
    }
}