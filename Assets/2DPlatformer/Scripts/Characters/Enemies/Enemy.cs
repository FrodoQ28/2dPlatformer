using UnityEngine;

[RequireComponent(typeof(Patrol), typeof(Rigidbody2D), typeof(Pursuer))]
[RequireComponent(typeof(Health), typeof(PlayerEnemyDetector))]
public class Enemy : MonoBehaviour
{
    private Patrol _patrol;
    private Pursuer _pursuer;
    private PlayerEnemyDetector _playerDetector;

    private bool _isPursuing = true;

    private void Awake()
    {
        _patrol = GetComponent<Patrol>();
        _pursuer = GetComponent<Pursuer>();
        _playerDetector = GetComponent<PlayerEnemyDetector>();
    }

    private void OnEnable()
    {
        _playerDetector.PlayerDetected += OnPursuit;
        _playerDetector.PlayerUndetected += OffPursuit;
    }

    private void OnDisable()
    {
        _playerDetector.PlayerDetected -= OnPursuit;
        _playerDetector.PlayerUndetected -= OffPursuit;
    }

    private void OnPursuit(Health health)
    {
        if (_isPursuing == false)
        {
            _patrol.Disable();
            _pursuer.Enable();

            _pursuer.SetTarget(health);

            _isPursuing = true;
        }
    }

    private void OffPursuit()
    {
        if (_isPursuing)
        {
            _pursuer.Disable();
            _patrol.Enable();

            _isPursuing = false;
        }
    }
}