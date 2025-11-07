using UnityEngine;

[RequireComponent(typeof(Mover2D), typeof(Attacker))]
public class Pursuer : MonoBehaviour
{
    private Mover2D _mover;
    private Rotator2D _rotator;
    private AnimationSwitch _animation;
    private Attacker _attacker;
    private Player _target;

    private Vector2 _direction;

    private void Awake()
    {
        _mover = GetComponent<Mover2D>();
        _rotator = GetComponent<Rotator2D>();
        _animation = GetComponent<AnimationSwitch>();
        _attacker = GetComponent<Attacker>();
    }

    private void Update()
    {
        if (_target != null)
        {
            _direction = (_target.transform.position - transform.position).normalized;
            _direction = _rotator.DefineTurn(_direction);
        }
    }

    private void FixedUpdate()
    {
        if (_direction != null)
        {
            _rotator.Turn();
            _mover.Move(_direction);
            _animation.OnMove();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out _) == _target)
        {
            if (_target.TryGetComponent(out Health playerHealth))
                _attacker.Attack(playerHealth);
        }
    }

    public void SetTarget(Player target)
    {
        _target = target;
    }
}
