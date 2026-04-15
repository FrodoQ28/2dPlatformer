using UnityEngine;

public class LifeStealAbility
{
    private Health _playerHealth;
    private ILifeStealTargetProvider _targetProvider;

    private float _radius;
    private float _duration;
    private float _cooldown;
    private float _damagePerSecond;
    private float _lifestealMultiplier;

    private float _currentTime;
    private float _cooldownTimer;
    private bool _isActive;

    public bool IsActive => _isActive;
    public bool IsOnCooldown => _cooldownTimer > 0f;

    public float CooldownRemaining
    {
        get
        {
            return _cooldownTimer;
        }
    }

    public float CooldownPercent
    {
        get
        {
            if (_cooldown <= 0f)
            {
                return 0f;
            }

            float raw = 1f - (_cooldownTimer / _cooldown);

            return Mathf.Clamp01(raw);
        }
    }

    public float ActivePercent
    {
        get
        {
            if (!_isActive || _duration <= 0f)
            {
                return 0f;
            }

            return Mathf.Clamp01(_currentTime / _duration);
        }
    }

    public LifeStealAbility(
        Health playerHealth,
        ILifeStealTargetProvider targetProvider,
        float radius,
        float duration,
        float cooldown,
        float damagePerSecond,
        float lifestealMultiplier)
    {
        _playerHealth = playerHealth;
        _targetProvider = targetProvider;
        _radius = radius;
        _duration = duration;
        _cooldown = cooldown;
        _damagePerSecond = damagePerSecond;
        _lifestealMultiplier = lifestealMultiplier;
    }

    public void TryActivate()
    {
        if (_isActive || _cooldownTimer > 0f)
        {
            return;
        }

        _isActive = true;
        _currentTime = _duration;
    }

    public Health Tick(float deltaTime, Vector2 playerPosition)
    {
        Health target = _targetProvider.GetTarget(playerPosition, _radius);

        if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= deltaTime;

            if (_cooldownTimer < 0f)
            {
                _cooldownTimer = 0f;
            }
        }

        if (!_isActive)
        {
            return null;
        }

        _currentTime -= deltaTime;

        if (_currentTime <= 0f)
        {
            _isActive = false;
            _cooldownTimer = _cooldown;
            return null;
        }

        if (target == null || target.IsDead)
        {
            return null;
        }

        float damage = _damagePerSecond * deltaTime;

        if (damage <= 0f)
        {
            return null;
        }

        target.TakeDamage(damage);

        if (target.IsDead)
        {
            return null;
        }

        float healAmount = damage * _lifestealMultiplier;
        _playerHealth.Heal(healAmount);

        return target;
    }
}