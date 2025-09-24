using UnityEngine;
using System;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Rotator))]
public class Mover : MonoBehaviour
{
    [SerializeField, Min(1f)] private float _moveSpeed;
    [SerializeField, Min(1f)] private float _jumpHeight;

    private Rigidbody2D _rigidbody;
    private Rotator _rotator;

    private Vector2 _direction;
    private bool _isRight = true;

    public event Action MoveEnabled;
    public event Action MoveDisabled;
    public event Action JumpEnabled;

    public bool IsGrounded { get; private set; }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rotator = GetComponent<Rotator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<Vector2>();

        if (_direction == Vector2.zero )
        {
            MoveDisabled?.Invoke();
        }
        else
        {
            MoveEnabled?.Invoke();
        }

        if (_direction == Vector2.right)
        {
            _isRight = true;
        }
        else if (_direction == Vector2.left)
        {
            _isRight = false;
            _direction = -_direction;
        }
    }

    public void OnJump(InputAction.CallbackContext context) =>
        Jump();

    private void Update()
    {
        IsGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, ~LayerManager.Ground);

        _rotator.Turn(_isRight);
        Move(_direction * _moveSpeed * Time.deltaTime);
    }

    private void Move(Vector2 direction) =>
        transform.Translate(direction);

    private void Jump()
    {
        if (IsGrounded)
        {
            _rigidbody.AddForce(Vector2.up * _jumpHeight, ForceMode2D.Impulse);

            JumpEnabled?.Invoke();
        }
    }
}