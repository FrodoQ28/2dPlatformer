using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover2D), typeof(Rotator2D), typeof(InputReader))]
[RequireComponent(typeof(Rigidbody2D), typeof(GroundDetector), typeof(Collector))]
[RequireComponent(typeof(AnimationSwitch))]
public class Player : MonoBehaviour
{
    private Mover2D _mover;
    private InputReader _input;
    private Rotator2D _rotator;
    private GroundDetector _groundDetector;
    private Rigidbody2D _rigidbody;
    private AnimationSwitch _animation;

    private bool _isRight = true;
    private Vector2 _direction;

    private void Awake()
    {
        _mover = GetComponent<Mover2D>();
        _input = GetComponent<InputReader>();
        _rotator = GetComponent<Rotator2D>();
        _groundDetector = GetComponent<GroundDetector>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animation = GetComponent<AnimationSwitch>();
    }

    private void OnEnable() =>
        _input.JumpEnabled += Jump;

    private void OnDisable() =>
        _input.JumpEnabled -= Jump;

    private void FixedUpdate()
    {
        _direction = _input.MoveDirection;

        DefineTurn();
        _rotator.Turn(_isRight);

        Move();
    }

    private void DefineTurn()
    {
        if (_input.MoveDirection == Vector2.right)
        {
            _isRight = true;
        }
        else if (_input.MoveDirection == Vector2.left)
        {
            _isRight = false;
            _direction = -_direction;
        }
    }

    private void Move()
    {
        if (_direction != Vector2.zero)
        {
            _mover.Move(_direction);
            _animation.OnMove();
        }
        else
        {
            _animation.OffMove();
        }
    }

    private void Jump()
    {
        if (_groundDetector.IsGrounded)
            StartCoroutine(Jumping());
    }

    private IEnumerator Jumping()
    {
        WaitUntil wait = new WaitUntil(() => _groundDetector.IsGrounded);

        _mover.Jump(_rigidbody);
        _animation.OnJump();

        yield return wait;

        _animation.OffJump();
    }
}