using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _fill;
    [SerializeField] private float _smoothSpeed = 5f;

    private float _targetValue;
    private float _currentValue;

    public void SetInstant(float value)
    {
        _currentValue = _targetValue = Mathf.Clamp01(value);
        _fill.fillAmount = _currentValue;
    }

    public void SetTarget(float value)
    {
        _targetValue = Mathf.Clamp01(value);
    }

    private void Update()
    {
        float multiplier = _targetValue < 0.1f ? 4f : 1f;
        _currentValue = Mathf.Lerp(_currentValue, _targetValue, Time.deltaTime * _smoothSpeed * multiplier);
        _fill.fillAmount = _currentValue;
    }
}