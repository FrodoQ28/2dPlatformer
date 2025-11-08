using System;
using UnityEngine;

public class PlayerEnemyDetector : MonoBehaviour
{
    [SerializeField] private float _maxRayDistance;
    [SerializeField] private LayerMask _layerMask;

    private Ray2D _ray;
    private bool _isPlayerScript;
    private int _enemySearchRadius = 2;

    public event Action<Health> PlayerDetected;
    public event Action PlayerUndetected;

    private void Awake()
    {
        InitializeLayerMask();
        _ray = new Ray2D(transform.position, transform.right);
    }

    private void FixedUpdate()
    {
        _ray.origin = transform.position;
        _ray.direction = transform.right;

        if (_isPlayerScript == false)
        {
            RaycastHit2D hit = Physics2D.Raycast(_ray.origin, _ray.direction, _maxRayDistance, _layerMask);
            Debug.DrawRay(_ray.origin, _ray.direction * _maxRayDistance, Color.yellow);

            if (hit)
            {
                if (hit.collider.gameObject.TryGetComponent(out Health health))
                    PlayerDetected?.Invoke(health);
            }
            else
            {
                PlayerUndetected?.Invoke();
            }
        }
    }

    public Health[] GetEnemyInRadius()
    {
        Health[] allEnemies = default;
        RaycastHit2D[] hits = Physics2D.RaycastAll(_ray.origin, _ray.direction, _enemySearchRadius, _layerMask);

        Debug.DrawRay(_ray.origin, _ray.direction * _enemySearchRadius, Color.yellow);

        if (hits != null)
        {
            allEnemies = new Health[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.TryGetComponent(out Health health))
                    allEnemies[i] = health;
            }
        }

        return allEnemies;
    }

    private void InitializeLayerMask()
    {
        if (this.TryGetComponent(out Player _))
        {
            _layerMask = 1 << LayerList.Enemy;
            _isPlayerScript = true;
        }
        else
        {
            _layerMask = 1 << LayerList.Player;
            _isPlayerScript = false;
        }
    }
}