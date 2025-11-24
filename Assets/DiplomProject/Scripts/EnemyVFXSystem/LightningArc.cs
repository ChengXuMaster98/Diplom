using UnityEngine;

public class LightningArc : MonoBehaviour
{
    private static LightningArc _instance;

    [SerializeField] private LineRenderer line;
    [SerializeField] private int points = 12;
    [SerializeField] private float noise = 0.2f;
    [SerializeField] private float duration = 0.15f;

    private float _timer;

    private void Awake()
    {
        _instance = this;
        line.enabled = false;
    }

    public static void Play(Vector3 from, Vector3 to)
    {
        if (_instance == null)
        {
            Debug.LogWarning("LightningArc instance not found in scene!");
            return;
        }

        _instance.DrawArc(from, to);
    }

    private void DrawArc(Vector3 from, Vector3 to)
    {
        line.positionCount = points;

        for (int i = 0; i < points; i++)
        {
            float t = i / (float)(points - 1);
            Vector3 pos = Vector3.Lerp(from, to, t);

            pos += new Vector3(
                Random.Range(-noise, noise),
                Random.Range(-noise, noise),
                Random.Range(-noise, noise)
            );

            line.SetPosition(i, pos);
        }

        line.enabled = true;
        _timer = duration;
    }

    private void Update()
    {
        if (!line.enabled) return;

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            line.enabled = false;
        }
    }
}