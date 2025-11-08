using System;
using UnityEngine;

public class PlayerEnemyDetector : MonoBehaviour
{
    [SerializeField] private float _maxRayDistance;
    [SerializeField] private LayerMask _layerMask;

    private Ray2D _ray;
    private bool _isPlayerScript;

    public event Action<Player> PlayerDetected;
    public event Action PlayerUndetected;
    public event Action<Enemy> EnemyDetected;
    public event Action EnemyUndetected;

    private void Awake()
    {
        InitializeLayerMask();
        _ray = new Ray2D(transform.position, transform.right);
    }

    private void FixedUpdate()
    {
        _ray.origin = transform.position;
        _ray.direction = transform.right;
        RaycastHit2D hit = Physics2D.Raycast(_ray.origin, _ray.direction, _maxRayDistance, _layerMask);
        Debug.DrawRay(_ray.origin, _ray.direction * _maxRayDistance, Color.yellow);

        if (hit)
        {
            if (hit.collider.gameObject.TryGetComponent(out Player player) && _isPlayerScript == false)
            {
                PlayerDetected?.Invoke(player);
            }
            else if (hit.collider.gameObject.TryGetComponent(out Enemy enemy) && _isPlayerScript)
            {
                EnemyDetected?.Invoke(enemy);
            }
        }
        else
        {
            PlayerUndetected?.Invoke();
            EnemyUndetected?.Invoke();
        }
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