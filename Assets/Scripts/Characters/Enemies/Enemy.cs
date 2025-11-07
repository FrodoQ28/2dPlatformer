using UnityEngine;

[RequireComponent (typeof(Patrol), typeof(Rigidbody2D), typeof(Pursuer))]
[RequireComponent(typeof(Health), typeof(PlayerEnemyDetector))]
public class Enemy : MonoBehaviour 
{
    private Patrol _patrol;
    private Pursuer _pursuer;
    private PlayerEnemyDetector _playerDetector;

    private void Awake()
    {
        _patrol = GetComponent<Patrol>();
        _pursuer = GetComponent<Pursuer>();
        _playerDetector = GetComponent<PlayerEnemyDetector>();

        _pursuer.enabled = false;
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

    private void OnPursuit(Player player)
    {
        _patrol.enabled = false;
        _pursuer.enabled = true;

        _pursuer.SetTarget(player);
    }

    private void OffPursuit()
    {
        _patrol.enabled = true;
        _pursuer.enabled = false;
    }
}