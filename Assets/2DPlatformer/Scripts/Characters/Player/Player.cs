using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover2D), typeof(Rotator2D), typeof(InputReader))]
[RequireComponent(typeof(Rigidbody2D), typeof(GroundDetector), typeof(Collector))]
[RequireComponent(typeof(AnimationSwitch), typeof(Health), typeof(Attacker))]
[RequireComponent (typeof(PlayerEnemyDetector))]
public class Player : MonoBehaviour
{
    private Mover2D _mover;
    private InputReader _input;
    private Rotator2D _rotator;
    private GroundDetector _groundDetector;
    private Rigidbody2D _rigidbody;
    private AnimationSwitch _animation;
    private Health _health;
    private Attacker _attacker;
    private PlayerEnemyDetector _enemyDetector;

    private bool _canMove = true;
    private Vector2 _direction;

    private void Awake()
    {
        _mover = GetComponent<Mover2D>();
        _input = GetComponent<InputReader>();
        _rotator = GetComponent<Rotator2D>();
        _groundDetector = GetComponent<GroundDetector>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animation = GetComponent<AnimationSwitch>();
        _health = GetComponent<Health>();
        _attacker = GetComponent<Attacker>();
        _enemyDetector = GetComponent<PlayerEnemyDetector>();
    }

    private void OnEnable()
    {
        _input.JumpEnabled += Jump;
        _health.DamageTaked += MoveStop;
        _input.AttackEnabled += Attack;
    }

    private void OnDisable()
    {
        _input.JumpEnabled -= Jump;
        _health.DamageTaked -= MoveStop;
        _input.AttackEnabled -= Attack;
    }

    private void FixedUpdate()
    {
        _direction = _input.MoveDirection;

        _rotator.Turn(_direction);

        Move();
    }

    private void Move()
    {
        if (_direction != Vector2.zero && _canMove)
        {
            _mover.Move(_direction);
            _animation.TurnOnMove();
        }
        else
        {
            _animation.TurnOffMove();
        }
    }

    private void Jump()
    {
        if (_groundDetector.IsGrounded && _canMove)
            StartCoroutine(Jumping());
    }

    private IEnumerator Jumping()
    {
        WaitUntil wait = new WaitUntil(() => _groundDetector.IsGrounded);

        _mover.Jump(_rigidbody);
        _animation.TurnOnJump();

        yield return wait;

        _animation.TurnOffJump();
    }

    private void MoveStop() =>
        StartCoroutine(MovingStoped());

    private IEnumerator MovingStoped()
    {
        WaitForSeconds wait = new WaitForSeconds(1);

        _canMove = false;

        yield return wait;

        _canMove = true;
    }

    private void Attack()
    {
        _animation.TurnOnAttack();

        Health[] healths = _enemyDetector.GetEnemyInRadius();

        if (healths != null)
        {
            foreach (Health health in healths)
                _attacker.Attack(health);
        }
    }
}