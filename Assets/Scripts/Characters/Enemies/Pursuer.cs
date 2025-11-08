using UnityEngine;

[RequireComponent(typeof(Mover2D), typeof(Attacker))]
public class Pursuer : MonoBehaviour
{
    private Mover2D _mover;
    private Rotator2D _rotator;
    private AnimationSwitch _animation;
    private Attacker _attacker;
    private Health _target;

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
            _animation.TurnOnMove();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Health player) && player == _target)
        {
            _animation.TurnOnAttack();
            _attacker.Attack(player);
        }
    }

    public void SetTarget(Health target) =>
        _target = target;
}