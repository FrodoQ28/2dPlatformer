using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _repulsiveForce;

    public void Attack(Health health)
    {
        health.TakeDamage(_damage);

        if (health.TryGetComponent(out Rigidbody2D _rigidbody))
            _rigidbody.AddForce(transform.right * _repulsiveForce, ForceMode2D.Impulse);
    }
}
