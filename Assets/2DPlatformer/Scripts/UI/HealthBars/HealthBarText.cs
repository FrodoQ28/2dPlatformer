using TMPro;
using UnityEngine;

public class HealthBarText : HealthBar<TextMeshProUGUI>
{
    private string _defaultText = "Çהמנמגüו :";
    
    private void Start()
    {
        ShowHealth(_health.MaxValue);
    }

    protected override void ShowHealth(float health) =>
        _healthBar.text = _defaultText + health + "/" + _health.MaxValue;
}
