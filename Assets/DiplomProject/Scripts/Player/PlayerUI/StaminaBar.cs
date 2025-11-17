using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Image _fill;
    [SerializeField] private float _smoothSpeed = 6f;

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
        _currentValue = Mathf.Lerp(_currentValue, _targetValue, Time.deltaTime * _smoothSpeed);
        _fill.fillAmount = _currentValue;
    }
}