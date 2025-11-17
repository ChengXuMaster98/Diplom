using UnityEngine;
using UnityEngine.UI;

public class LowHealthEffect : MonoBehaviour
{
    [SerializeField] private Image _overlay;
    [SerializeField] private Color _baseColor = new Color(0.6f, 0, 0, 0.25f);
    [SerializeField] private float _pulseSpeed = 3f;
    [SerializeField] private float _lowHealthThreshold = 0.3f;

    private float _currentHealthRatio;
    private bool _isActive;

    public void UpdateEffect(float healthRatio)
    {
        _currentHealthRatio = healthRatio;

        if (healthRatio <= _lowHealthThreshold)
        {
            _isActive = true;
        }
        else
        {
            _isActive = false;
            _overlay.color = Color.clear;
        }
    }

    private void Update()
    {
        if (!_isActive) return;

        float alphaPulse = (Mathf.Sin(Time.time * _pulseSpeed) + 1f) / 2f; // 0-1
        float targetAlpha = Mathf.Lerp(0.1f, 0.4f, alphaPulse);
        var color = _baseColor;
        color.a = targetAlpha;
        _overlay.color = color;
    }
}