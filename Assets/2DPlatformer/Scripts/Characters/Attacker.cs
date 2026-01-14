using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _repulsiveForce;

    private Vector2 _repulsiveDirection;

    public void Attack(Health health)
    {
        health.TakeDamage(_damage);

        _repulsiveDirection = (health.transform.position - transform.position).normalized;

        if (health.TryGetComponent(out Rigidbody2D _rigidbody))
            _rigidbody.AddForce(_repulsiveDirection * _repulsiveForce, ForceMode2D.Impulse);
    }
}