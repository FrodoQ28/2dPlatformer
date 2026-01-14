using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarSmooth : HealthBar<Slider>
{
    private void Start()
    {
        _healthBar.maxValue = _health.MaxValue;
        _healthBar.value = _healthBar.maxValue;
    }

    protected override void ShowHealth(float health)
    {
        StopCoroutine(ShowingHealth(health));
        StartCoroutine(ShowingHealth(health));
    }

    private IEnumerator ShowingHealth(float health)
    {
        while (_healthBar.value != health)
        {
            yield return null;
            _healthBar.value = Mathf.MoveTowards(_healthBar.value, health, 1f);
        }
    }
}

