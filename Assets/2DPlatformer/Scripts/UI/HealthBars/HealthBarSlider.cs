using UnityEngine.UI;

public class HealthBarSlider : HealthBar<Slider>
{
    private void Start()
    {
        _healthBar.maxValue = _health.MaxValue;
        _healthBar.value = _healthBar.maxValue;
    }

    protected override void ShowHealth(float health) =>
        _healthBar.value = health;
}
