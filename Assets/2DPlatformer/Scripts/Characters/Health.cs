using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxValue;

    private float _value;

    public event Action DamageTaked;
    public event Action HealTaked;
    public event Action<float> HealthChanged;

    public float MaxValue
    {
        get
        {
            return _maxValue;
        }
    }

    private void Awake()
    {
        _value = _maxValue;
    }

    public void TakeDamage(float damageValue)
    {
        if (damageValue > 0)
        {
            _value -= damageValue;

            if (_value < 0)
                _value = 0;

            DamageTaked?.Invoke();
            HealthChanged?.Invoke(_value);
        }
    }

    public void Heal(float healValue)
    {
        if (healValue > 0)
        {
            _value += healValue;

            if (_value > _maxValue)
                _value = _maxValue;

            HealTaked?.Invoke();
            HealthChanged?.Invoke(_value);
        }
    }
}