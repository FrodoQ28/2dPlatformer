using UnityEngine;

public class HealthBar<T> : MonoBehaviour
{
    [SerializeField] protected Health _health;
    [SerializeField] protected T _healthBar;

    protected void OnEnable()
    {
        _health.HealthChanged += ShowHealth;
    }

    protected void OnDisable()
    {
        _health.HealthChanged -= ShowHealth;
    }

    protected virtual void ShowHealth (float health) { }
}
