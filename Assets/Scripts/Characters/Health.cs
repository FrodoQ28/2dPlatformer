using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxValue;

    private float _value;

    public event Action DamageTaked;

    //private bool _isAlive = true;

    private void Awake()
    {
        _value = _maxValue;
    }

    public void TakeDamage(float damageValue)
    {
        _value -= damageValue;

        DamageTaked?.Invoke();

        Debug.Log("Çהמנמגüו" + this.gameObject.name + _value);

        //if (_value <= 0)
        //    //_isAlive = false;
    }

    public void Heal(float healValue)
    {
        _value += healValue;

        if (_value > _maxValue)
            _value = _maxValue;

        Debug.Log("Çהמנמגüו" + this.gameObject.name + _value);

    }
}
