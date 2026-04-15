using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class PlayerLifeStealAbility2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Health _playerHealth;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private TextMeshProUGUI _cooldownText;

    [Header("Ability Settings")]
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _duration = 6f;
    [SerializeField] private float _cooldown = 4f;
    [SerializeField] private float _damagePerSecond = 10f;
    [SerializeField] private float _lifestealMultiplier = 1f;

    [Header("Visual Settings")]
    [SerializeField] private int _circleSegments = 64;
    [SerializeField] private Color _activeColor = Color.red;
    [SerializeField] private float _lineWidth = 0.05f;

    [Header("Cooldown Text Visual")]
    [SerializeField] private Color _readyColor = new Color(1f, 0.84f, 0f);
    [SerializeField] private Color _emptyColor = Color.red;
    [SerializeField] private float _glowScaleAmplitude = 0.1f;
    [SerializeField] private float _glowPulseSpeed = 4f;

    private Vector3 _cooldownTextBaseScale;

    private LifeStealAbility _ability;
    private ILifeStealTargetProvider _targetProvider;

    private LineRenderer _radiusLineRenderer;
    private Vector3[] _circlePoints;

    private void Awake()
    {
        if (_playerHealth == null)
        {
            _playerHealth = GetComponent<Health>();
        }

        _targetProvider = new RaycastEnemyTargetProvider2D(32, _enemyLayerMask);

        _ability = new LifeStealAbility(
            _playerHealth,
            _targetProvider,
            _radius,
            _duration,
            _cooldown,
            _damagePerSecond,
            _lifestealMultiplier
        );

        if (_cooldownText != null)
        {
            _cooldownText.text = string.Empty;
            _cooldownTextBaseScale = _cooldownText.transform.localScale;
        }

        _radiusLineRenderer = GetComponent<LineRenderer>();
        ConfigureRadiusLineRenderer();
        GenerateCirclePoints();
    }

    private void ConfigureRadiusLineRenderer()
    {
        _radiusLineRenderer.useWorldSpace = true;
        _radiusLineRenderer.loop = true;
        _radiusLineRenderer.positionCount = _circleSegments;
        _radiusLineRenderer.startWidth = _lineWidth;
        _radiusLineRenderer.endWidth = _lineWidth;
    }

    private void GenerateCirclePoints()
    {
        _circlePoints = new Vector3[_circleSegments];
        float angleStep = 2f * Mathf.PI / _circleSegments;

        for (int i = 0; i < _circleSegments; i++)
        {
            float angle = i * angleStep;
            float x = Mathf.Cos(angle) * _radius;
            float y = Mathf.Sin(angle) * _radius;
            _circlePoints[i] = new Vector3(x, y, 0f);
        }
    }

    private void LateUpdate()
    {
        Health currentTarget = _ability.Tick(Time.deltaTime, transform.position);
        UpdateRadiusVisual();
        UpdateCooldownText();
    }

    public void OnLifeSteal(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _ability.TryActivate();
        }
    }

    private void UpdateRadiusVisual()
    {
        bool isActive = _ability.IsActive;

        _radiusLineRenderer.enabled = isActive;

        if (!isActive || _circlePoints == null)
        {
            return;
        }

        Vector3 offset = transform.position;

        for (int i = 0; i < _circleSegments; i++)
        {
            _radiusLineRenderer.SetPosition(i, _circlePoints[i] + offset);
        }

        _radiusLineRenderer.startColor = _activeColor;
        _radiusLineRenderer.endColor = _activeColor;
    }

    private void UpdateCooldownText()
    {
        if (_cooldownText == null)
        {
            return;
        }

        float percent;

        if (_ability.IsActive)
        {
            percent = _ability.ActivePercent * 100f;
        }
        else if (_ability.IsOnCooldown)
        {
            percent = _ability.CooldownPercent * 100f;
        }
        else
        {
            percent = 100f;
        }

        int intPercent = Mathf.RoundToInt(percent);
        _cooldownText.text = intPercent + "%";

        float t = Mathf.Clamp01(percent / 100f);
        Color currentColor = Color.Lerp(_emptyColor, _readyColor, t);
        _cooldownText.color = currentColor;

        if (intPercent >= 100)
        {
            float pulse = 1f + Mathf.Sin(Time.time * _glowPulseSpeed) * _glowScaleAmplitude;
            _cooldownText.transform.localScale = _cooldownTextBaseScale * pulse;
        }
        else
        {
            _cooldownText.transform.localScale = _cooldownTextBaseScale;
        }
    }
}