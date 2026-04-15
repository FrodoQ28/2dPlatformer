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

    public float CurrentValue
    {
        get
        {
            return _value;
        }
    }

    public bool IsDead
    {
        get
        {
            return _value <= 0f;
        }
    }

    private void Awake()
    {
        _value = _maxValue;
    }

    public void TakeDamage(float damageValue)
    {
        if (damageValue > 0f && !IsDead)
        {
            _value -= damageValue;

            if (_value < 0f)
            {
                _value = 0f;
            }

            DamageTaked?.Invoke();
            HealthChanged?.Invoke(_value);
        }
    }

    public void Heal(float healValue)
    {
        if (healValue > 0f && !IsDead)
        {
            _value += healValue;

            if (_value > _maxValue)
            {
                _value = _maxValue;
            }

            HealTaked?.Invoke();
            HealthChanged?.Invoke(_value);
        }
    }
}